using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolGradebook.Models;

namespace SchoolGradebook.Services
{
    public class AttendanceService
    {
        private readonly SchoolContext context;
        public AttendanceService(SchoolContext context)
            => this.context = context;

        public async Task<AttendanceRecord[]> GetAttendanceRecords(Expression<Func<AttendanceRecord, bool>> expression)
            => await context.AttendanceRecords.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<AttendanceRecord> GetAttendanceRecordById(int id)
            => await context.AttendanceRecords.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<AttendanceRecord> GetAttendanceRecordByLessonRecordId(int id)
            => await context.AttendanceRecords.Where(a => a.LessonRecordId == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task AddAttendanceRecord(AttendanceRecord attendance)
        {
            await context.AttendanceRecords.AddAsync(attendance);
            await context.SaveChangesAsync();
        }

        public async Task AddAttendanceRecords(IEnumerable<AttendanceRecord> records)
        {
            await context.AddRangeAsync(records);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAttendanceRecord(AttendanceRecord attendance)
        {
            context.Remove(attendance);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAttendenceRecord(int id)
            => await RemoveAttendanceRecord(await GetAttendanceRecordById(id));

        public async Task UpdateAttendanceRecord(AttendanceRecord attendance)
        {
            context.Attach(attendance).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateAttendanceRecords(IEnumerable<AttendanceRecord> records)
        {
            foreach (var r in records)
                context.Attach(r).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task RemoveOrCreateAttendenceRecords(IEnumerable<AttendanceRecord> newRecords, int lessonRecordId)
        {
            List<AttendanceRecord> currentAttendanceRecords = (await GetAttendanceRecordsByLessonRecordId(lessonRecordId)).ToList();
            context.RemoveRange(currentAttendanceRecords);
            await context.AddRangeAsync(newRecords);
            await context.SaveChangesAsync();
        }

        public async Task<AttendanceRecord[]> GetAttendanceRecordsByLessonRecordId(int id)
            => await GetAttendanceRecords(r => r.LessonRecord.Id == id);

    }

}
