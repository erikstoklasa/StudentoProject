using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SchoolContext _context;

        public IndexModel(ILogger<IndexModel> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (User.IsInRole("teacher"))
            {
                return LocalRedirect("~/Teacher/Index");
            }
            else if (User.IsInRole("student"))
            {
                return LocalRedirect("~/Student/Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
