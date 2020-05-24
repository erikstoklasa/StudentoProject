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

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public string UserId { get; set; }
        public IList<Subject> Subjects { get; set; }

        public IndexModel(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = UserId == null ? "" : UserId;
        }

        public async Task OnGetAsync()
        {
            Subjects = await _context.Subjects
                .Where(g => g.Teacher.UserAuthId == UserId)
            .ToListAsync();
        }
    }
}
