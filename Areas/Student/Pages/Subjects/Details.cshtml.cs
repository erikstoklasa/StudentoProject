using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System.Security.Claims;

namespace SchoolGradebook.Areas.Student.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public DetailsModel(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context = context;
        }

        public string UserId { get; private set; }
        public Subject Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await _context.Subjects
                .Include(s => s.Teacher).FirstOrDefaultAsync(m => m.Id == id);

            if (Subject == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
