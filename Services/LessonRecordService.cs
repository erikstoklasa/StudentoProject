using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

        public async Task<LessonRecord> GetFullLessonRecordById(int id)
            => await context.LessonRecords.Where(r => r.Id == id).AsNoTracking()
                .Include(r => r.SubstitutionTeacher)
                .Include(r => r.SubstitutionSubjectInstance)
                .Include(r => r.TimeFrame)
                .AsNoTracking()
                .FirstOrDefaultAsync();

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
