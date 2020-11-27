using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class DetailsModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public DetailsModel(IHttpContextAccessor httpContextAccessor, TeacherService teacherService, StudentService studentService, SubjectService subjectService)
        {
            this.teacherService = teacherService;
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

            return Page();
        }
        public async Task<IActionResult> OnGetPrintAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Student = await studentService.GetStudentFullProfileAsync((int)id);
            if (Student == null)
            {
                return NotFound();
            }

            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(stream));
            Document doc = new Document(pdfDoc);
            PdfFont defaultFont = PdfFontFactory.CreateFont("wwwroot/fonts/OpenSans-Regular.ttf", PdfEncodings.IDENTITY_H, false);
            //Image picture = new Image(ImageDataFactory.Create("wwwroot/images/girl.png"));
            //picture.ScaleToFit(100,150);
            //doc.Add(new Paragraph().Add(picture).SetHorizontalAlignment(HorizontalAlignment.RIGHT));
            doc.Add(new Paragraph(Student.GetFullName()).SetFont(defaultFont).SetFontSize(30));
            doc.Add(new Paragraph($"{Student.Class.GetName()} ({Student.Class.Teacher.GetFullName()})").SetFont(defaultFont));
            Text t = new Text($"Aktuální k: {DateTime.UtcNow.ToLocalTime():d. M. yyyy HH:mm}").SetFont(defaultFont);
            t.SetFontSize(10);
            doc.Add(new Paragraph(t));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            table.AddCell("Věk").SetFont(defaultFont);
            table.AddCell(((DateTime.Now - Student.Birthdate.GetValueOrDefault()).Days / 365).ToString()).SetFont(defaultFont);

            table.AddCell("Datum narození").SetFont(defaultFont);
            table.AddCell(Student.Birthdate.GetValueOrDefault().ToString("d. M. yyyy")).SetFont(defaultFont);

            Student.Email ??= "";
            table.AddCell("Email").SetFont(defaultFont);
            table.AddCell(Student.Email).SetFont(defaultFont);

            Student.PhoneNumber ??= "";
            table.AddCell("Telefoní číslo").SetFont(defaultFont);
            table.AddCell(Student.PhoneNumber).SetFont(defaultFont);

            Student.PersonalIdentifNumber ??= "";
            table.AddCell("Rodné číslo").SetFont(defaultFont);
            table.AddCell(Student.PersonalIdentifNumber).SetFont(defaultFont);

            Student.PlaceOfBirth ??= "";
            table.AddCell("Místo narození").SetFont(defaultFont);
            table.AddCell(Student.PlaceOfBirth).SetFont(defaultFont);

            Student.IdentifCardNumber ??= "";
            table.AddCell("Číslo občanského průkazu").SetFont(defaultFont);
            table.AddCell(Student.IdentifCardNumber).SetFont(defaultFont);

            Student.InsuranceCompany ??= "";
            table.AddCell("Pojišťovna").SetFont(defaultFont);
            table.AddCell(Student.InsuranceCompany).SetFont(defaultFont);

            Student.StreetAddress ??= "";
            table.AddCell("Trvalé bydliště").SetFont(defaultFont);
            table.AddCell($"{Student.StreetAddress}, {Student.ZipCode} {Student.CityAddress}").SetFont(defaultFont);

            doc.Add(table);
            doc.Close();
            return File(stream.ToArray(), "application/pdf", $"{Student.GetFullName()} - výpis.pdf");
        }
    }
}
