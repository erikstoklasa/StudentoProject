using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolGradebook
{
    public class CreateModelGrades : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public List<SelectListItem> Subjects { get; private set; }
        public List<SelectListItem> Students { get; private set; }
        private string UserId { get; set; }

        public CreateModelGrades(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Student> tempStud = _context.Students.ToList();
            List<Subject> tempSubj = _context.Subjects
                .Where(s => s.Teacher.UserAuthId == UserId)
                .ToList();
            Subjects = new List<SelectListItem> { };
            Students = new List<SelectListItem> { };
            foreach (Subject s in tempSubj)
            {
                foreach (Student stud in tempStud)
                {
                    Students.Add(new SelectListItem(stud.FirstName + " " + stud.LastName, stud.Id.ToString()));
                }
                Subjects.Add(new SelectListItem(s.Name, s.Id.ToString()));
            }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Grade Grade { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int subjectId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
