using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Pages.HumanCodes
{
    public class CreateModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public Student Student { get; set; }
        public Models.Teacher Teacher { get; set; }
        HumanActivationCode code;


        public CreateModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;

            code = new HumanActivationCode();
            //Return to view either name of teahcer or student so that they can view the list and choose for whom they want to generate a code
        }

        public async Task<IActionResult> OnGetAsync(int targetId, CodeType codeType)
        {
            HumanActivationCode oldCode = await _context.HumanActivationCodes.Where(s => s.TargetId == targetId && s.CodeType == codeType).FirstOrDefaultAsync();
            if (oldCode != null)
            {
                _context.HumanActivationCodes.Remove(oldCode);
            }
            code.TargetId = targetId;
            code.CodeType = codeType;
            do
            {
                code.HumanCode = code.getNewHumanCode();
            }
            while (_context.HumanActivationCodes.Where(s => s.HumanCode == code.HumanCode).Any()); //Until no HumanCodes duplicates exist
            //Generate new ids until there are no duplicates in the db
            await _context.HumanActivationCodes.AddAsync(code);
            await _context.SaveChangesAsync();
            if (User.IsInRole("admin"))
            {
                if (codeType == CodeType.Student)
                {
                    return RedirectToPage("/Admin/Students/Index");
                }
                else
                {
                    return RedirectToPage("/Admin/Teachers/Index");
                }
            }
            else
            {
                return RedirectToPage("/Teacher/Students/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int targetId, CodeType codeType)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (targetId != 0) //Internal post from /Students/index or /Teachers/index
            {
                code.TargetId = targetId;
                code.CodeType = codeType;
            }
            code.HumanCode = code.getNewHumanCode();
            _context.HumanActivationCodes.Add(code);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
