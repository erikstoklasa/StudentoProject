using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TeacherService
    {
        private readonly SchoolContext context;

        public TeacherService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<int> GetTeacherId(string userAuthId)
        {
            Teacher teacher = await context.Teachers
                .Where(t => t.UserAuthId == userAuthId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return teacher.Id;
        }
        public async Task<Teacher> GetTeacherAsync(int teacherId)
        {
            Teacher teacher = await context.Teachers
                .FindAsync(teacherId);
            return teacher;
        }
        public async Task<Teacher[]> GetAllTeachersAsync()
        {
            Teacher[] teacher = await context.Teachers
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ToArrayAsync();
            return teacher;
        }
        public async Task<int> GetTeacherCountAsync()
        {
            return (await GetAllTeachersAsync()).Length;
        }
    }
}
