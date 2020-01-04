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

namespace SchoolGradebook.Pages.Grades
{
    public class IndexModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string UserId { get; set; }
        private bool isTeacher = false;

        public IndexModel(SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = UserId == null ? "" : UserId;
            isTeacher = _httpContextAccessor.HttpContext.User.IsInRole("teacher");
        }

        public IList<Grade> Grades { get;set; }
        public IList<Subject> Subjects { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }

        public async Task OnGetAsync()
        {
            if (Subject != null)
            {
                Teacher = Subject.Teacher;
            }     
            if (isTeacher)
            {
                Subjects = await _context.Subjects
                    .Where(g => g.Teacher.UserAuthId == UserId)
                .ToListAsync();
            }
            else
            {
                Grades = await _context.Grades
                        .Where(g => g.Student.UserAuthId == UserId)
                        .Include(g => g.Subject)
                            .ThenInclude(g => g.Teacher)
                        .ToListAsync();

            }
            
        }
    }
}
