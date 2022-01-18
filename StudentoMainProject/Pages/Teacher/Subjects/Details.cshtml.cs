using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.Models;
using StudentoMainProject.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly GradeService gradeService;
        private readonly StudentGroupService studentGroupService;
        private readonly LogItemService logItemService;

        public DetailsModel(IHttpContextAccessor httpContextAccessor,
                            TeacherAccessValidation teacherAccessValidation,
                            TeacherService teacherService,
                            StudentService studentService,
                            SubjectService subjectService,
                            SubjectMaterialService subjectMaterialService,
                            GradeService gradeService,
                            StudentGroupService studentGroupService,
                            LogItemService logItemService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.subjectMaterialService = subjectMaterialService;
            this.gradeService = gradeService;
            this.studentGroupService = studentGroupService;
            this.logItemService = logItemService;
            StudentGrades = new List<List<Grade>>();
            SubjectMaterials = new List<SubjectMaterial>();
            StudentAverages = new List<double>();
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
            StudentGroupNames = new List<string>();
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }

        public string UserId { get; private set; }
        public SubjectInstance Subject { get; set; }
        public Models.Student[] Students { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        public List<double> StudentAverages { get; set; }
        public List<List<Grade>> StudentGrades { get; set; }
        public List<string> StudentGroupNames { get; set; }
        public double SubjectAvg { get; set; }
        public List<SubjectMaterial> SubjectMaterials { get; set; }
        public List<(Models.Student student, double studentAverage, List<Grade> studentGrades)> StudentsAndAverageAndGrades;
        public IPAddress IPAddress { get; set; }

        public void OnGet(){}
        public async Task<IActionResult> OnGetPrintAsync(int? id)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            bool teacherHasAccessToSubject = await teacherAccessValidation.HasAccessToSubject(teacherId, (int)id);
            if (!teacherHasAccessToSubject)
            {
                return BadRequest();
            }

            Subject = await subjectService.GetSubjectInstanceAsync((int)id);
            if (Subject == null)
            {
                return NotFound();
            }
            await logItemService.Log(
                new LogItem
                {
                    EventType = "SubjectDetailPrint",
                    Timestamp = DateTime.UtcNow,
                    UserAuthId = UserId,
                    UserId = teacherId,
                    UserRole = "teacher",
                    IPAddress = IPAddress.ToString()
                });
            SubjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstance(Subject.Id);
            Students = await studentService.GetAllStudentsBySubjectInstanceAsync(Subject.Id);

            foreach (Models.Student s in Students)
            {
                StudentAverages.Add(
                    AnalyticsService.GetSubjectAverageForStudentAsync(
                        await gradeService.GetAllGradesByStudentSubjectInstance(s.Id, Subject.Id)
                    ));
                StudentGrades.Add((await gradeService.GetGradesAddedByTeacherAsync(s.Id, Subject.Id)).ToList());
            }
            StudentsAndAverageAndGrades = Enumerable
                .Range(0, Students.Length)
                .Select(i => Tuple.Create(Students[i], StudentAverages[i], StudentGrades[i]).ToValueTuple())
                .ToList();
            StudentGroups = await studentGroupService.GetAllGroupsBySubjectInstanceAsync((int)id);
            foreach (var studentGroup in StudentGroups)
            {
                StudentGroupNames.Add(studentGroup.Name);
            }
            using MemoryStream stream = new();
            PdfDocument pdfDoc = new(new PdfWriter(stream));
            Document doc = new(pdfDoc);
            PdfFont defaultFont = PdfFontFactory.CreateFont("wwwroot/fonts/OpenSans-Regular.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            //Image picture = new Image(ImageDataFactory.Create("wwwroot/images/girl.png"));
            //picture.ScaleToFit(100,150);
            //doc.Add(new Paragraph().Add(picture).SetHorizontalAlignment(HorizontalAlignment.RIGHT));
            doc.Add(new Paragraph(Subject.GetFullName())).SetFont(defaultFont);
            doc.Add(new Paragraph(String.Join(" + ", StudentGroupNames))).SetFont(defaultFont);
            doc.Add(new Paragraph($"Počet studentů: {Students.Length}")).SetFont(defaultFont);
            Text t = new($"Aktuální k: {DateTime.UtcNow.ToLocalTime()}");
            t.SetFontSize(10);
            doc.Add(new Paragraph(t)).SetFont(defaultFont);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();

            table.AddHeaderCell("Jméno a přijímení");
            table.AddHeaderCell("Průměr");
            table.AddHeaderCell("Známky");
            foreach (var (student, studentAverage, studentGrades) in StudentsAndAverageAndGrades)
            {
                table.AddCell(student.GetFullName());
                table.AddCell(studentAverage.ToString("f2"));
                List<string> onlyGradeValues = new();
                foreach (var y in studentGrades)
                {
                    onlyGradeValues.Add(y.GetGradeValue());
                }
                string gradesString = String.Join(",", onlyGradeValues);
                Cell cell = new();
                cell.SetMinHeight(20);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(gradesString);
            }

            doc.Add(table).SetFont(defaultFont);
            doc.Close();
            return File(stream.ToArray(), "application/pdf", $"{Subject.GetFullName()} (výpis známek).pdf");
        }
    }
}
