using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public double StudentToTeacherRatio { get; set; }
        public string UserId { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        private readonly Analytics _analytics;
        private readonly AdminService adminService;

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, AdminService adminService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
            this.adminService = adminService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await adminService.IsAdminSufficientLevel(await adminService.GetAdminId(UserId), 1))
            {
                return Forbid();
            }
            TeacherCount = await _analytics.GetTeachersCountAsync();
            StudentCount = await _analytics.GetStudentsCountAsync();
            StudentToTeacherRatio = (double)StudentCount / TeacherCount;
            return Page();
        }
    }
}
