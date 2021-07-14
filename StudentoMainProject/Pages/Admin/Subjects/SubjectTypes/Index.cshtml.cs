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
    public class IndexModel : PageModel
    {
        private readonly SubjectService subjectService;

        public IndexModel(SubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        public IList<SubjectType> SubjectTypes { get;set; }

        public async Task OnGetAsync()
        {
            SubjectTypes = await subjectService.GetAllSubjectTypesAsync();
        }
    }
}
