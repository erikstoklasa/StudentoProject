using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        private string UserId { get; set; }
        public List<int> StudentsCount { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, int studentCount)> SubjectsAndStudentCounts;
        public List<SubjectInstance> Subjects { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, TeacherService teacherService, StudentService studentService, SubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            StudentsCount = new List<int>();
        }
        public async Task<IActionResult> OnGetAsync()
        {

            int teacherId = await teacherService.GetTeacherId(UserId);

            Subjects = await subjectService.GetAllSubjectInstancesByTeacherAsync(teacherId);

            foreach (SubjectInstance si in Subjects)
            {
                StudentsCount.Add(await studentService.GetStudentCountBySubjectAsync(si.Id));
            }
            SubjectsAndStudentCounts = Subjects.Zip(StudentsCount, (si, sc) => (si, sc));
            if (teacherId == -1)
            {
                return LocalRedirect("/ActivateAccount");
            }
            return Page();
        }
    }
}
