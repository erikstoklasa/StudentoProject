using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SchoolGradebook.Services;

namespace SchoolGradebook.Areas.Student.Pages.Grades
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Analytics _analytics;
        public string UserId { get; set; }
        public IList<Grade> Grades { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            _httpContextAccessor = httpContextAccessor;
            _analytics = analytics;

            UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task OnGetAsync()
        {
            Grades = await _analytics.getGradesAsync(UserId);
        }
    }
}
