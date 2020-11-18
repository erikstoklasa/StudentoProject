using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly Analytics _analytics;
        private readonly SubjectService subjectService;
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly StudentService studentService;
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly GradeService gradeService;

        public List<Grade> Grades { get; set; }

        public List<SubjectMaterial> SubjectMaterials { get; set; }
        public double SubjectAverage { get; set; }

        public DetailsModel(IHttpContextAccessor httpContextAccessor,
                            Analytics analytics,
                            SubjectService subjectService,
                            StudentAccessValidation studentAccessValidation,
                            StudentService studentService,
                            SubjectMaterialService subjectMaterialService,
                            GradeService gradeService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _analytics = analytics;
            this.subjectService = subjectService;
            this.studentAccessValidation = studentAccessValidation;
            this.studentService = studentService;
            this.subjectMaterialService = subjectMaterialService;
            this.gradeService = gradeService;
        }

        public string UserId { get; private set; }
        public SubjectInstance Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await subjectService.GetSubjectInstanceAsync((int)id);

            if (Subject == null)
            {
                return NotFound();
            }

            int? studentId = await studentService.GetStudentId(UserId);

            if (studentId == null)
            {
                return Forbid();
            }
            bool studentHasAccessToSubject = await studentAccessValidation.HasAccessToSubject((int)studentId, (int)id);//HERE IS A PROBLEM!
            if (!studentHasAccessToSubject)
            {
                return BadRequest();
            }

            Grades = await gradeService.GetAllGradesByStudentSubjectInstance((int)studentId, (int)id);

            SubjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstance((int)id);
            SubjectAverage = await _analytics.GetSubjectAverageForStudentAsync((int)studentId, Subject.Id);

            double currentAvg = await _analytics.GetSubjectAverageForStudentAsync((int)studentId, Subject.Id);
            double comparisonAvg = await _analytics.GetSubjectAverageForStudentAsync((int)studentId, Subject.Id, 365, 30);
            ViewData["ComparisonString"] = LanguageHelper.getAverageComparisonString(currentAvg, comparisonAvg);


            return Page();
        }
    }
}
