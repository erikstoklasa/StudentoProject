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
using SchoolGradebook.Services;
using System.Security.Claims;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public DetailsModel(SchoolContext context, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation)
        {
            _context = context;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int gradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToGrade = await teacherAccessValidation.HasAccessToGrade(TeacherId, gradeId);
            if (!hasAccessToGrade)
            {
                return NotFound();
            }

            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject.Teacher)
                .Include(g => g.Subject).FirstOrDefaultAsync(m => m.Id == gradeId);

            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
