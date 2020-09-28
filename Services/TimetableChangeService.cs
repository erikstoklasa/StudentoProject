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
    public class TimetableChangeService
    {
        private readonly SchoolContext context;

        public TimetableChangeService(SchoolContext context)
            => this.context = context;

        public async Task<TimetableChange[]> GetAllTimetableChanges()
            => await context.TimetableChanges.AsNoTracking().ToArrayAsync();
        public async Task<TimetableChange[]> GetAllTimetableChangesByStudent(int studentId)
       => await context.TimetableChanges.Where(tch => tch.).AsNoTracking().ToArrayAsync();

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
