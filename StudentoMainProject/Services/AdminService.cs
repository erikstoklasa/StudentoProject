using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class AdminService
    {
        private readonly SchoolContext context;

        public AdminService(SchoolContext context)
            => this.context = context;
        public async Task<int> GetAdminId(string userAuthId)
        {
            Admin a = await context.Admins.Where(a => a.UserAuthId == userAuthId).AsNoTracking().FirstOrDefaultAsync();
            if (a == null)
            {
                throw new Exception("Admin not found by supplied userAuthId");
            }
            return a.Id;
        }

        public async Task<Admin[]> GetAllAdmmins()
            => await context.Admins.AsNoTracking().AsNoTracking().ToArrayAsync();

        public async Task<Admin[]> GetAdmins(Expression<Func<Admin, bool>> expression)
            => await context.Admins.Where(expression).AsNoTracking().ToArrayAsync();

        public async Task<Admin> GetAdminById(int id)
            => await context.Admins.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<Admin> GetAdminByUserAuthId(string id)
            => await context.Admins.Where(a => a.UserAuthId == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<bool> IsAdminSufficientLevel(int adminId, int level)
            => (await GetAdminById(adminId)).AdminLevel >= level;

        public async Task AddAdmin(Admin admin)
        {
            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAdmin(Admin admin)
        {
            context.Remove(admin);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAdmin(int id)
            => await RemoveAdmin(await GetAdminById(id));

        public async Task UpdateAdmin(Admin admin)
        {
            context.Attach(admin).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
