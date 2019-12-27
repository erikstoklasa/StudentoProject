using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Pages.HumanCodes
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public DetailsModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        public HumanActivationCode HumanActivationCode { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HumanActivationCode = await _context.HumanActivationCodes.FirstOrDefaultAsync(m => m.Id == id);

            if (HumanActivationCode == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
