using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly SubjectService subjectService;
        private readonly ApprobationService approbationService;
        private readonly AdminService adminService;
        public string UserId { get; set; }

        public CreateModel(TeacherService teacherService, SubjectService subjectService, ApprobationService approbationService, AdminService adminService, IHttpContextAccessor httpContextAccessor)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.approbationService = approbationService;
            this.adminService = adminService;
            Approbations = new List<int>();
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SubjectTypes = await subjectService.GetAllSubjectTypesAsync();
            return Page();
        }
        public List<SubjectType> SubjectTypes { get; set; }
        [BindProperty]
        public List<int> Approbations { get; set; }
        [BindProperty]
        public Models.Teacher Teacher { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            int adminId = await adminService.GetAdminId(UserId);
            Teacher.SchoolId = (await adminService.GetAdminById(adminId)).SchoolId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await teacherService.AddTeacherAsync(Teacher, Approbations);
            return RedirectToPage("./Index");
        }
    }
}
