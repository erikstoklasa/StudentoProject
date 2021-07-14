using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    public class LessonRecordService
    {
        private readonly SchoolContext context;

        public LessonRecordService(SchoolContext context)
            => this.context = context;

        public async Task<LessonRecord[]> GetLessonRecordsAsync(Expression<Func<LessonRecord, bool>> expression)
            => await context.LessonRecords.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<LessonRecord> GetLessonRecordById(int id)
            => await context.LessonRecords.Where(r => r.Id == id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<List<LessonRecord>> GetLessonRecordsByStudentAndWeek(int studentId, int week)
        {
            List<SubjectInstance> subjectInstances = new List<SubjectInstance>();
            List<StudentGroupEnrollment> studentGroupEnrollments = await context.GetService<StudentGroupService>().GetAllGroupEnrollmentsByStudentAsync(studentId);
            foreach (var sge in studentGroupEnrollments)
            {
                subjectInstances.AddRange(
                    await context.GetService<SubjectService>().GetSubjectInstancesByGroupId(sge.StudentGroupId)
                    );
            }
            List<LessonRecord> output = new List<LessonRecord>();
            foreach (var si in subjectInstances)
            {
                output.AddRange(await context.LessonRecords.Where(lr => lr.SubjectInstanceId == si.Id && lr.Week == week)
                                                           .AsNoTracking()
                                                           .ToListAsync());
            }
            return output;
        }
        public async Task<List<LessonRecord>> GetLessonRecordsByTeacherAndWeek(int teacherId, int week)
        {
            List<SubjectInstance> subjectInstances = await context.GetService<SubjectService>().GetAllSubjectInstancesByTeacherAsync(teacherId);
            List<LessonRecord> output = new List<LessonRecord>();
            foreach (var si in subjectInstances)
            {
                output.AddRange(await context.LessonRecords.Where(lr => lr.SubjectInstanceId == si.Id && lr.Week == week)
                                                           .AsNoTracking()
                                                           .ToListAsync());
            }
            return output;
        }
        public async Task<List<LessonRecord>> GetAllLessonRecordsByTeacher(int teacherId)
        {
            List<SubjectInstance> subjectInstances = await context.SubjectInstances
                .Where(si => si.TeacherId == teacherId)
                .AsNoTracking()
                .ToListAsync();
            List<LessonRecord> output = new List<LessonRecord>();
            foreach (var si in subjectInstances)
            {
                output.AddRange(await context.LessonRecords
                    .Where(lr => lr.SubjectInstanceId == si.Id)
                    .AsNoTracking()
                    .ToListAsync());
            }
            return output;
        }

        public async Task<LessonRecord> GetFullLessonRecordById(int id)
            => await context.LessonRecords.Where(r => r.Id == id).AsNoTracking()
                .Include(r => r.TimeFrame)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        public async Task<LessonRecord> GetLessonRecord(int week, int subjectInstanceId, int timeFrameId)
        {
            return await context.LessonRecords
                       .Where(r => r.Week == week && r.SubjectInstanceId == subjectInstanceId && r.TimeFrameId == timeFrameId)
                       .Include(r => r.TimeFrame)
                       .Include(r => r.SubjectInstance)
                       .ThenInclude(si => si.SubjectType)
                       .AsNoTracking()
                       .FirstOrDefaultAsync();
        }

        public async Task AddLessonRecord(LessonRecord record)
        {
            await context.LessonRecords.AddAsync(record);
            await context.SaveChangesAsync();
        }

        public async Task RemoveLessonRecord(LessonRecord record)
        {
            context.Remove(record);
            await context.SaveChangesAsync();
        }

        public async Task RemoveLessonRecord(int id)
            => await RemoveLessonRecord(await GetLessonRecordById(id));

        public async Task UpdateLessonRecord(LessonRecord record)
        {
            context.Attach(record).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateLessonRecords(IEnumerable<LessonRecord> records)
        {
            foreach (var r in records)
                context.Attach(r).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
