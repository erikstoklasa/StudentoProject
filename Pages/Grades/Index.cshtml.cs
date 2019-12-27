using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolGradebook
{
    public class IndexModelGrades : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string UserId { get; set; }
        private bool isTeacher = false;

        public IndexModelGrades(SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = UserId == null ? "" : UserId;
            isTeacher = _httpContextAccessor.HttpContext.User.IsInRole("teacher");
        }

        public string getRelativeTime(DateTime dateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public IList<Grade> Grade { get;set; }
        public IList<Subject> Subject { get; set; }
        public string SubjectId { get; set; }

        public async Task OnGetAsync(string subjectId)
        {
            SubjectId = subjectId;
            if (isTeacher)
            {
                Subject = await _context.Subjects
                    .Where(g => g.Teacher.UserAuthId == UserId)
                .ToListAsync();
                /*Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                    .ThenInclude(g => g.Teacher)
                    .Where(g => g.Subject.Teacher.UserAuthId == UserId)
                .ToListAsync();*/
            }
            else
            {
                Grade = await _context.Grades
                .Include(g => g.Student)
                .Where(g => g.Student.UserAuthId == UserId)
                .Include(g => g.Subject)
                    .ThenInclude(g => g.Teacher)
                .ToListAsync();
            }
            
        }
    }
}
