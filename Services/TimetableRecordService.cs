using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TimetableRecordService
    {
        private readonly SchoolContext context;

        public TimetableRecordService(SchoolContext context)
            => this.context = context;

        public async Task<TimetableRecord[]> GetAllTimetableRecords()
        {
            return await context.TimetableRecords
                .Include(tr => tr.SubjectInstance)
                .Include(tr => tr.SubjectInstance.Teacher)
                .Include(tr => tr.SubjectInstance.SubjectType)
                .Include(tr => tr.Room)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<TimetableRecord[]> GetTimetableRecords(Expression<Func<TimetableRecord, bool>> expression)
            => await context.TimetableRecords.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<TimetableRecord[]> GetTimetableRecordsBySubjectInstanceIdAsync(int id)
        {
            return await context.TimetableRecords
                .Where(tr => tr.SubjectInstanceId == id)
                .Include(tr => tr.SubjectInstance)
                .Include(tr => tr.SubjectInstance.Teacher)
                .Include(tr => tr.SubjectInstance.SubjectType)
                .Include(tr => tr.Room)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<TimetableRecord[]> GetTimetableRecordsByRoomId(int id)
            => await GetTimetableRecords(tr => tr.RoomId == id);

        public async Task<TimetableRecord[]> GetTimetableRecordsByStudentGroupId(int id)
        {
            var subjectInstances = await context.GetService<SubjectService>().GetSubjectInstancesByGroupId(id);
            return await GetTimetableRecords(tr => subjectInstances.Contains(tr.SubjectInstance));
        }

        public async Task<List<TimetableRecord>> GetTimetableRecordsByStudentId(int id)
        {
            var output = new List<TimetableRecord>();
            TimetableRecord[] trAll = await GetAllTimetableRecords();
            foreach (var subjectInstance in await context.GetService<SubjectService>().GetAllSubjectInstancesByStudentAsync(id))
            {
                output.AddRange(trAll.Where(tr => tr.SubjectInstanceId == subjectInstance.Id));
            }
            return output;
        }
        public async Task<TimetableRecord[]> GetTimetableRecordsByTeacher(int id)
        {
            return await context.TimetableRecords.Where(tr => tr.SubjectInstance.TeacherId == id)
                .Include(tr => tr.Room)
                .Include(tr => tr.SubjectInstance)
                .Include(tr => tr.SubjectInstance.SubjectType)
                .Include(tr => tr.SubjectInstance.Enrollments)
                .ThenInclude(e => e.StudentGroup)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<TimetableRecord> GetTimetableRecordById(int id)
            => await context.TimetableRecords.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task AddTimetableRecord(TimetableRecord tr)
        {
            await context.TimetableRecords.AddAsync(tr);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimetableRecord(TimetableRecord tr)
        {
            context.TimetableRecords.Remove(tr);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimetableRecord(int id)
            => await RemoveTimetableRecord(await GetTimetableRecordById(id));

        public async Task UpdateTimeFrame(TimetableRecord tr)
        {
            context.Attach(tr).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
