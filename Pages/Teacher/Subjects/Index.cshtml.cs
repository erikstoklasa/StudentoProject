using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Teacher.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        public List<SubjectInstance> Subjects { get; set; }
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;
        private readonly SubjectService subjectService;

        public List<int> StudentsCount { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, int studentCount)> SubjectsAndStudentCounts;

        public IndexModel(IHttpContextAccessor httpContextAccessor, StudentService studentService, TeacherService teacherService, SubjectService subjectService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            StudentsCount = new List<int>();
        }

        public async Task OnGetAsync()
        {
            int teacherId = await teacherService.GetTeacherId(UserId);

            Subjects = await subjectService.GetAllSubjectInstancesByTeacherAsync(teacherId);
            //Subjects = await _analytics.GetAllSubjectsByTeacherUserAuthAsync(UserId);
            
            foreach (SubjectInstance si in Subjects)
            {
                StudentsCount.Add(await studentService.GetStudentCountBySubjectAsync(si.Id));
            }
            SubjectsAndStudentCounts = Subjects.Zip(StudentsCount, (si, sc) => (si, sc));
        }
    }
}
