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

namespace SchoolGradebook.Pages.Admin.Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly StudentGroupService studentGroupService;

        public DeleteModel(StudentGroupService studentGroupService)
        {
            this.studentGroupService = studentGroupService;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ReturnUrl ??= Url.Content("~/Admin/Students");
            if (id == null)
            {
                return NotFound();
            }

            await studentGroupService.RemoveStudentGroupEnrollment((int)id);

            return LocalRedirect(ReturnUrl);
        }
    }
}
