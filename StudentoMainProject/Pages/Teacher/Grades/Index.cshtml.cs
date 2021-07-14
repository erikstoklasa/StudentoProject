using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? SubjectInstanceId { get; set; }
        public IndexModel()
        {

        }
        public void OnGet()
        {

        }
    }
}
