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

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
        }
        public async Task OnGet()
        {
            TeacherCount = await _analytics.GetTeachersCountAsync();
            StudentCount = await _analytics.GetStudentsCountAsync();
            StudentToTeacherRatio = (double) StudentCount / TeacherCount;
        }
    }
}
