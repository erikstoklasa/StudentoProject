using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class CreateModel : PageModel
    {
        public IList<Models.Student> Students { get; private set; }

        private string UserId { get; set; }
        public string SubjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubjectInstanceId { get; set; }
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly GradeService gradeService;
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;

        public int TeacherId { get; set; }

        public CreateModel(IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, GradeService gradeService, SubjectService subjectService, StudentService studentService)
        {
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.gradeService = gradeService;
            this.subjectService = subjectService;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Students = await studentService.GetAllStudentsBySubjectInstanceAsync(SubjectInstanceId);
            TeacherId = await teacherService.GetTeacherId(UserId);

            bool hasAccessToSubject = await teacherAccessValidation.HasAccessToSubject(TeacherId, SubjectInstanceId);
            if (!hasAccessToSubject)
            {
                return Forbid();
            }

            SubjectInstance s = await subjectService.GetSubjectInstanceAsync(SubjectInstanceId);
            SubjectName = s.SubjectType.Name;

            return Page();
        }

        [BindProperty]
        public Grade Grade { set; get; }

        public async Task<IActionResult> OnPostAsync()
        {
            // parsing it out quite manually because aspnet doesnt seem to support post arrays/objects
            var studentIdGradePairs = new List<Tuple<int,short>>();
            for (int i = 0;; i++)
            {
                if (!Request.Form.ContainsKey($"grades[{i}][studentId]"))
                    break;

                var studentId = int.Parse(Request.Form[$"grades[{i}][studentId]"]);
                var grade = Request.Form[$"grades[{i}][grade]"].ToString();

                Tuple<int, short> studentIdGradePair;
                try {
                    studentIdGradePair = new Tuple<int, short>(studentId, short.Parse(grade));
                } catch {
                    continue;
                }
                studentIdGradePairs.Add(studentIdGradePair);
            }

            Students = await studentService.GetAllStudentsBySubjectInstanceAsync(SubjectInstanceId);
            TeacherId = await teacherService.GetTeacherId(UserId);

            if (!await teacherAccessValidation.HasAccessToSubject(TeacherId, SubjectInstanceId))
                return Forbid();

            foreach(Tuple<int,short> studentIdGradePair in studentIdGradePairs)
            {
                // Validation so rogue teacher can't give grades to any studentId (s)he wants
                if(Students.Where(s => s.Id == studentIdGradePair.Item1).Any())
                {
                    Grade g = new Grade
                    {
                        Name = Grade.Name,
                        Added = DateTime.UtcNow,
                        SubjectInstanceId = SubjectInstanceId,
                        StudentId = studentIdGradePair.Item1,
                        Value = studentIdGradePair.Item2
                    };

                    if (!ModelState.IsValid)
                        return Page();
                    await gradeService.AddGradeAsync(g);
                }
            }

            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectInstanceId }");
        }
    }
}