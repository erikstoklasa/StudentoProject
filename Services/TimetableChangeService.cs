using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TimetableChangeService
    {
        private readonly SchoolContext context;

        public TimetableChangeService(SchoolContext context)
            => this.context = context;

        public async Task<TimetableChange[]> GetAllTimetableChanges()
            => await context.TimetableChanges.AsNoTracking().ToArrayAsync();
        public async Task<TimetableChange[]> GetAllTimetableChangesByStudent(int studentId, int week)
        {
            List<StudentGroupEnrollment> SGEnrollments = await context.GetService<StudentGroupService>().GetAllGroupEnrollmentsByStudentAsync(studentId);
            List<TimetableChange> timetableChanges = new List<TimetableChange>();
            foreach (var sge in SGEnrollments)
            {
                timetableChanges.AddRange(
                    await context.TimetableChanges
                    .Where(tch => tch.StudentGroupId == sge.StudentGroupId && tch.Week == week)
                    .Include(tch => tch.CurrentSubjectInstance)
                    .Include(tch => tch.CurrentSubjectInstance.SubjectType)
                    .Include(tch => tch.CurrentRoom)
                    .Include(tch => tch.CurrentTeacher)
                    .AsNoTracking()
                    .ToListAsync()
                    );
            }
            return timetableChanges.ToArray();
        }
        public async Task<TimetableChange[]> GetAllTimetableChangesByTeacher(int teacherId, int week)
        {
            List<TimetableChange> timetableChanges = new List<TimetableChange>();
            List<SubjectInstance> subjectInstances = await context.GetService<SubjectService>().GetAllSubjectInstancesByTeacherAsync(teacherId);
            foreach (var si in subjectInstances)
            {
                timetableChanges.AddRange(
                    await context.TimetableChanges
                    .Where(tch => (tch.CurrentTeacherId == teacherId || tch.SubjectInstanceId == si.Id) && tch.Week == week)
                    .Include(tch => tch.CurrentSubjectInstance)
                    .Include(tch => tch.CurrentSubjectInstance.Enrollments)
                        .ThenInclude(e => e.StudentGroup)
                    .Include(tch => tch.CurrentSubjectInstance.SubjectType)
                    .Include(tch => tch.CurrentRoom)
                    .Include(tch => tch.StudentGroup)
                    .AsNoTracking()
                    .ToListAsync()
                    );
            }

            return timetableChanges.Distinct().ToArray();
        }
        public async Task<TimetableChange[]> GetTimetableChanges(Expression<Func<TimetableChange, bool>> expression)
            => await context.TimetableChanges.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<TimetableChange> GetTimetableChangeById(int id)
            => await context.TimetableChanges.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task AddTimetableChange(TimetableChange timetableChange)
        {
            await context.TimetableChanges.AddAsync(timetableChange);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimetableChange(TimetableChange timetableChange)
        {
            context.Remove(timetableChange);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimetableChange(int id)
            => await RemoveTimetableChange(await GetTimetableChangeById(id));

        public async Task UpdateTimetableChange(TimetableChange timetableChange)
        {
            context.Attach(timetableChange).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
