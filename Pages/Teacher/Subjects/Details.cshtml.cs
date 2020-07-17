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

        public DetailsModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, TeacherAccessValidation teacherAccessValidation, TeacherService teacherService, StudentService studentService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _analytics = analytics;
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            this.studentService = studentService;
            StudentGrades = new List<Grade[]>();
            SubjectMaterials = new List<SubjectMaterial>();
        }

        public string UserId { get; private set; }
        public Subject Subject { get; set; }
        public Models.Student[] Students { get; set; }
        public double[] StudentAverages { get; set; }
        public List<Grade[]> StudentGrades { get; set; }
        public List<SubjectMaterial> SubjectMaterials { get; set; }

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

            Subject = await _analytics.GetSubjectAsync((int)id);
            if (Subject == null)
            {
                return NotFound();
            }
            SubjectMaterials = (await _analytics.GetAllSubjectMaterialsAsync(Subject.Id)).ToList();
            Students = await studentService.GetAllStudentsBySubjectAsync(Subject.Id);
            StudentAverages = new double[Students.Length];

            for (int i = 0; i < Students.Length; i++)
            {
                StudentAverages[i] = await _analytics.GetSubjectAverageForStudentByStudentIdAsync(Students[i].Id, Subject.Id);
                StudentGrades.Add(await _analytics.GetGradesByTeacherUserAuthIdAsync(UserId, Subject.Id, Students[i].Id));
            }

            return Page();
        }
    }
}
