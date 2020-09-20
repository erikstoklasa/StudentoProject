using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Subjects.SubjectTypes
{
    public class DeleteModel : PageModel
    {
        private readonly SubjectService subjectService;

        public DeleteModel(SubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        [BindProperty]
        public SubjectType SubjectType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectType = await subjectService.GetSubjectTypeAsync((int)id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await subjectService.DeleteSubjectTypeAsync((int)id);

            return RedirectToPage("./Index");
        }
    }
}
