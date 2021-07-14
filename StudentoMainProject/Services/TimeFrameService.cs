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
    public class TimeFrameService
    {
        private readonly SchoolContext context;

        public TimeFrameService(SchoolContext context)
            => this.context = context;
        
        public async Task<TimeFrame[]> GetAllTimeFrames()
            => await context.TimeFrames.AsNoTracking().AsNoTracking().ToArrayAsync();

        public async Task<TimeFrame[]> GetTimeFrames(Expression<Func<TimeFrame, bool>> expression)
            => await context.TimeFrames.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<TimeFrame> GetTimeFrameById(int id)
            => await context.TimeFrames.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();

        ///<exception cref="ArgumentException">Throws ArgumentException if TimeFrame has invalid start and end times</exception>
        public async Task AddTimeFrame(TimeFrame frame)
        {
            if((frame.End - frame.Start).TotalMinutes < 0)
            {
                throw new ArgumentException("Timeframe cannot start later than the end time");
            }
            await context.TimeFrames.AddAsync(frame);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimeFrame(TimeFrame frame)
        {
            context.Remove(frame);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTimeFrame(int id)
            => await RemoveTimeFrame(await GetTimeFrameById(id));

        public async Task UpdateTimeFrame(TimeFrame frame)
        {
            context.Attach(frame).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
