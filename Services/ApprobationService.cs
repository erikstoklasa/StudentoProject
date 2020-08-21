using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class ApprobationService
    {
        private readonly SchoolContext context;

        public ApprobationService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<List<Approbation>> GetAllApprobations()
        {
            return await context.Approbations.AsNoTracking().ToListAsync();
        }
        public async Task<List<Approbation>> GetAllApprobations(int teacherId)
        {
            return await context.Approbations
                .Where(a => a.TeacherId == teacherId)
                .Include(a => a.SubjectType)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> AddApprobation(int teacherId, int subjectTypeId)
        {
            context.Approbations.Add(new Approbation { SubjectTypeId = subjectTypeId, TeacherId = teacherId });
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
