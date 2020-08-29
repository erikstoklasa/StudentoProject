using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class DetailsModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public DetailsModel(IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, StudentService studentService, SubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.studentService = studentService;
            this.subjectService = subjectService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        public Models.Student Student { get; set; }
        public List<Models.SubjectInstance> SubjectInstances { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student = await studentService.GetStudentFullProfileAsync((int)id);

            if (Student == null)
            {
                return NotFound();
            }
            SubjectInstances = await subjectService.GetAllSubjectInstancesByStudentAsync((int)id);
            TeacherId = await teacherService.GetTeacherId(UserId);
            //bool hasaccesstostudent = await teacheraccessvalidation.hasaccesstostudent(teacherid, student.id);
            //if (!hasaccesstostudent)
            //{
            //    return badrequest();
            //}

            return Page();
        }
    }
}
