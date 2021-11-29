using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class DetailsModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public DetailsModel(StudentService studentService, SubjectService subjectService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
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

            using MemoryStream stream = new();
            PdfDocument pdfDoc = new(new PdfWriter(stream));
            Document doc = new(pdfDoc);
            PdfFont defaultFont = PdfFontFactory.CreateFont("wwwroot/fonts/OpenSans-Regular.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
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
