using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public string UserId { get; set; }
        private bool isTeacher = false;
        private Analytics analytics;

        public IndexModel(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor, Analytics _analytics)
        {
            _context = context;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = UserId == null ? "" : UserId;
            isTeacher = httpContextAccessor.HttpContext.User.IsInRole("teacher");
            analytics = _analytics;
        }

        public IList<Subject> Subjects { get;set; }

        public async Task OnGetAsync()
        {
            Subjects = analytics.getAllSubjectsByUserId(UserId);
        }
    }
}
