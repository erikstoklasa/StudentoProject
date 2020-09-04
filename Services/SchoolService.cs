using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class SchoolService
    {
        private readonly SchoolContext context;

        public SchoolService(SchoolContext context)
            => this.context = context;

        public async Task AddSchoolAsync(School school)
        {
            await context.Schools.AddAsync(school);
            await context.SaveChangesAsync();
        }

        public async Task UpdateSchoolAsync(School school)
        {
            context.Attach(school).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<School> GetSchoolById(int id)
            => await context.Schools.Where(r => r.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<School[]> GetAllSchools()
            => await context.Schools.AsNoTracking().ToArrayAsync();
    }
}
