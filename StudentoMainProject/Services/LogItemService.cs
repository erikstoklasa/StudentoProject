using SchoolGradebook.Data;
using StudentoMainProject.Models;
using System.Threading.Tasks;

namespace StudentoMainProject.Services
{
    public class LogItemService
    {
        private readonly SchoolContext context;

        public LogItemService(SchoolContext context) => this.context = context;
        public async Task Log(LogItem logItem)
        {
            await context.LogItems.AddAsync(logItem);
            await context.SaveChangesAsync();
        }
    }
}
