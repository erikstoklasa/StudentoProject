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
    public class IndexModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public IndexModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<HumanActivationCode> HumanActivationCodes { get;set; }

        public async Task OnGetAsync()
        {
            HumanActivationCodes = await _context.HumanActivationCodes.ToListAsync();
        }
    }
}
