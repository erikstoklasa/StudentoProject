using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Subjects.SubjectTypes
{
    public class EditModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly AdminService adminService;
        public string UserId { get; set; }

        public EditModel(SubjectService subjectService, IHttpContextAccessor httpContextAccessor, AdminService adminService)
        {
            this.subjectService = subjectService;
            this.adminService = adminService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        [BindProperty]
        public SubjectType SubjectType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectType = await subjectService.GetSubjectTypeAsync((int)id);

            if (SubjectType == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int adminId = await adminService.GetAdminId(UserId);
            Models.Admin admin = await adminService.GetAdminById(adminId);
            SubjectType.SchoolId = admin.SchoolId;

            await subjectService.UpdateSubjectTypeAsync(SubjectType);

            return RedirectToPage("./Index");
        }
    }
}
