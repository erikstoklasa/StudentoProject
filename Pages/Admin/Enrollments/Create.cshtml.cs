using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly StudentGroupService studentGroupService;

        public CreateModel(StudentService studentService, StudentGroupService studentGroupService)
        {
            Students = new List<SelectListItem>();
            StudentGroups = new List<SelectListItem>();
            this.studentService = studentService;
            this.studentGroupService = studentGroupService;
        }

        public List<SelectListItem> Students { get; set; }
        public List<SelectListItem> StudentGroups { get; set; }
        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if ((!Url.IsLocalUrl(HttpUtility.UrlDecode(ReturnUrl))) || ReturnUrl == null)
            {
                ReturnUrl = Url.Content("~/Admin/Students");
            }
            ViewData["ReturnUrl"] = ReturnUrl;
            if (StudentId != 0)
            {
                var s = await studentService.GetStudentFullProfileAsync(StudentId);
                string className = s.Class != null ? s.Class.GetName() : "";
                Students.Add(new SelectListItem(text: $"{s.GetFullName()} - {className}", value: s.Id.ToString()));
            }
            else
            {
                foreach (var s in await studentService.GetAllStudentsAsync())
                {
                    string className = s.Class != null ? s.Class.GetName() : "";
                    Students.Add(new SelectListItem(text: $"{s.GetFullName()} - {className}", value: s.Id.ToString()));
                }
            }
            foreach (var g in await studentGroupService.GetAllGroupsAsync())
            {
                List<StudentGroupEnrollment> studentGroupEnrollments = await studentGroupService.GetAllGroupEnrollmentsByStudentAsync(StudentId);
                if (!studentGroupEnrollments.Where(sge => sge.StudentGroupId == g.Id).Any())
                {
                    StudentGroups.Add(new SelectListItem(text: g.Name, value: g.Id.ToString()));
                }

            }
            return Page();
        }

        [BindProperty]
        public StudentGroupEnrollment StudentGroupEnrollment { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl ??= Url.Content("~/Admin/Students");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await studentGroupService.AddStudentToGroup(StudentGroupEnrollment.StudentId, StudentGroupEnrollment.StudentGroupId);

            return LocalRedirect(ReturnUrl);
        }
    }
}
