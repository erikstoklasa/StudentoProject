using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Subjects.SubjectTypes
{
    public class CreateModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly AdminService adminService;

        public string UserId { get; set; }

        public CreateModel(SubjectService subjectService, IHttpContextAccessor httpContextAccessor, AdminService adminService)
        {
            this.subjectService = subjectService;
            this.adminService = adminService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SubjectType SubjectType { get; set; }

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

            await subjectService.AddSubjectTypeAsync(SubjectType);

            return RedirectToPage("./Index");
        }
    }
}
