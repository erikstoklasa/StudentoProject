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
        HumanActivationCode humanActivationCode;


        public CreateModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;

            humanActivationCode = new HumanActivationCode();
            //Return to view either name of teahcer or student so that they can view the list and choose for whom they want to generate a code
        }

        public async Task<IActionResult> OnGetAsync(int targetId, CodeType codeType)
        {

            humanActivationCode.TargetId = targetId;
            humanActivationCode.CodeType = codeType;
            humanActivationCode.HumanCode = humanActivationCode.getNewHumanCode();
            _context.HumanActivationCodes.Add(humanActivationCode);
            await _context.SaveChangesAsync();
            if(codeType == CodeType.Student)
            {
                return RedirectToPage("/Students/Index");
            } else
            {
                return RedirectToPage("/Teachers/Index");
            }
            
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int targetId, CodeType codeType)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (targetId != 0) //Internal post from /Students/index or /Teachers/index
            {
                humanActivationCode.TargetId = targetId;
                humanActivationCode.CodeType = codeType;
            }
            humanActivationCode.HumanCode = humanActivationCode.getNewHumanCode();
            _context.HumanActivationCodes.Add(humanActivationCode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
