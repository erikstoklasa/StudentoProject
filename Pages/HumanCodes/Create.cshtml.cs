using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Pages.HumanCodes
{
    public class CreateModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        

        public CreateModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
            

            //Return to view either name of teahcer or student so that they can view the list and choose for whom they want to generate a code
        }

        public IActionResult OnGet()
        {

            return Page();
        }

        [BindProperty]
        public HumanActivationCode HumanActivationCode { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            HumanActivationCode.HumanCode = getNewHumanCode();
            _context.HumanActivationCodes.Add(HumanActivationCode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public string getNewHumanCode()
        {
            char[] alphabet = { 'a','b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string code = "";
            Random r = new Random();
            for(int i = 0; i < 6; i++)
            {
                int indexOfChar = (int) Math.Floor(r.NextDouble() * 34);
                code += alphabet[indexOfChar];
            }
            return code;
            
        }
    }
}
