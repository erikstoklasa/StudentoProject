using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly Analytics _analytics;
        public string UserId { get; private set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _analytics = analytics;
        }
        public async Task OnGet()
        {
            double currentAvg = await _analytics.GetTotalAverageAsync(UserId);
            double comparisonAvg = await _analytics.GetTotalAverageAsync(UserId, 365, 30);
            ViewData["ComparisonString"] = LanguageHelper.getAverageComparisonString(currentAvg, comparisonAvg);
        }
    }
}
