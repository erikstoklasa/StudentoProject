using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;

namespace SchoolGradebook.Pages.Teacher.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly Analytics _analytics;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly GradeService gradeService;

        public DetailsModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, TeacherAccessValidation teacherAccessValidation, TeacherService teacherService, StudentService studentService, SubjectService subjectService, SubjectMaterialService subjectMaterialService, GradeService gradeService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _analytics = analytics;
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.subjectMaterialService = subjectMaterialService;
            this.gradeService = gradeService;
            StudentGrades = new List<List<Grade>>();
            SubjectMaterials = new List<SubjectMaterial>();
            StudentAverages = new List<double>();
        }

        public string UserId { get; private set; }
        public SubjectInstance Subject { get; set; }
        public Models.Student[] Students { get; set; }
        public List<double> StudentAverages { get; set; }
        public List<List<Grade>> StudentGrades { get; set; }
        public List<SubjectMaterial> SubjectMaterials { get; set; }
        public List<(Models.Student student, double studentAverage, List<Grade> studentGrades)> StudentsAndAverageAndGrades;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int TeacherId = await teacherService.GetTeacherId(UserId);
            bool teacherHasAccessToSubject = await teacherAccessValidation.HasAccessToSubject(TeacherId, (int)id);
            if (!teacherHasAccessToSubject)
            {
                return BadRequest();
            }

            Subject = await subjectService.GetSubjectInstanceAsync((int)id);
            if (Subject == null)
            {
                return NotFound();
            }
            SubjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstance(Subject.Id);
            Students = await studentService.GetAllStudentsBySubjectInstanceAsync(Subject.Id);

            foreach (Models.Student s in Students)
            {
                StudentAverages.Add(await _analytics.GetSubjectAverageForStudentByStudentIdAsync(s.Id, Subject.Id));
                StudentGrades.Add(await gradeService.GetAllGradesByStudentSubjectInstance(s.Id, Subject.Id));
            }
            StudentsAndAverageAndGrades = Enumerable
                .Range(0, Students.Length)
                .Select(i => Tuple.Create(Students[i], StudentAverages[i], StudentGrades[i]).ToValueTuple())
                .ToList();
            return Page();
        }
    }
}
