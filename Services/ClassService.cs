using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class ClassService
    {
        private readonly SchoolContext context;

        public ClassService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<Class> GetClassAsync(int classId)
        {
            Class classOut = await context.Classes
                .Where(s => s.Id == classId)
                .Include(s=>s.Teacher)
                .Include(s=>s.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return classOut;
        }
        public async Task<bool> AddClassAsync(Class classItem)
        {
            try
            {
                await context.Classes.AddAsync(classItem);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<List<Class>> GetAllClasses()
        {
            List<Class> classes = await context.Classes
                .Include(c => c.Teacher)
                .AsNoTracking()
                .ToListAsync();
            return classes.OrderBy(c => c.GetName()).ToList();
        }
        public async Task<bool> DeleteClassAsync(int classId)
        {
            Class classItem = await context.Classes.FindAsync(classId);

            if (classItem != null)
            {
                context.Classes.Remove(classItem);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateClassAsync(Class classItem)
        {
            context.Attach(classItem).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
