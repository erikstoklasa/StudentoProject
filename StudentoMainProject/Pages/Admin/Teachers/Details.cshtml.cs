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

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly ApprobationService approbationService;

        public DetailsModel(TeacherService teacherService, ApprobationService approbationService)
        {
            this.teacherService = teacherService;
            this.approbationService = approbationService;
        }

        public Models.Teacher Teacher { get; set; }
        public List<Approbation> Approbations { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher = await teacherService.GetTeacherAsync((int)id);

            if (Teacher == null)
            {
                return NotFound();
            }

            Approbations = await approbationService.GetAllApprobations(Teacher.Id);

            return Page();
        }
        public async Task<IActionResult> OnGetPrintAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Teacher = await teacherService.GetTeacherAsync((int)id);
            if (Teacher == null)
            {
                return NotFound();
            }
            Teacher.Approbations = await approbationService.GetAllApprobations((int)id);
            using MemoryStream stream = new();
            PdfDocument pdfDoc = new(new PdfWriter(stream));
            Document doc = new(pdfDoc);
            PdfFont defaultFont = PdfFontFactory.CreateFont("wwwroot/fonts/OpenSans-Regular.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            //Image picture = new Image(ImageDataFactory.Create("wwwroot/images/girl.png"));
            //picture.ScaleToFit(100,150);
            //doc.Add(new Paragraph().Add(picture).SetHorizontalAlignment(HorizontalAlignment.RIGHT));
            doc.Add(new Paragraph(Teacher.GetFullName()).SetFont(defaultFont).SetFontSize(30));
            Text t = new Text($"Aktuální k: {DateTime.UtcNow.ToLocalTime():d. M. yyyy HH:mm}").SetFont(defaultFont);
            t.SetFontSize(10);
            doc.Add(new Paragraph(t));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            table.AddCell("Věk").SetFont(defaultFont);
            table.AddCell(((DateTime.Now - Teacher.Birthdate.GetValueOrDefault()).Days / 365).ToString()).SetFont(defaultFont);

            table.AddCell("Datum narození").SetFont(defaultFont);
            table.AddCell(Teacher.Birthdate.GetValueOrDefault().ToString("d. M. yyyy")).SetFont(defaultFont);

            Teacher.Email ??= "";
            table.AddCell("Email").SetFont(defaultFont);
            table.AddCell(Teacher.Email).SetFont(defaultFont);

            Teacher.PhoneNumber ??= "";
            table.AddCell("Telefoní číslo").SetFont(defaultFont);
            table.AddCell(Teacher.PhoneNumber).SetFont(defaultFont);

            Teacher.PersonalIdentifNumber ??= "";
            table.AddCell("Rodné číslo").SetFont(defaultFont);
            table.AddCell(Teacher.PersonalIdentifNumber).SetFont(defaultFont);

            Teacher.PlaceOfBirth ??= "";
            table.AddCell("Místo narození").SetFont(defaultFont);
            table.AddCell(Teacher.PlaceOfBirth).SetFont(defaultFont);

            Teacher.IdentifCardNumber ??= "";
            table.AddCell("Číslo občanského průkazu").SetFont(defaultFont);
            table.AddCell(Teacher.IdentifCardNumber).SetFont(defaultFont);

            Teacher.InsuranceCompany ??= "";
            table.AddCell("Pojišťovna").SetFont(defaultFont);
            table.AddCell(Teacher.InsuranceCompany).SetFont(defaultFont);

            Teacher.StreetAddress ??= "";
            table.AddCell("Trvalé bydliště").SetFont(defaultFont);
            table.AddCell($"{Teacher.StreetAddress}, {Teacher.ZipCode} {Teacher.CityAddress}").SetFont(defaultFont);

            table.AddCell("Datum nástupu").SetFont(defaultFont);
            table.AddCell(Teacher.StartDate.GetValueOrDefault().ToString("d. M. yyyy")).SetFont(defaultFont);

            List<string> approbationNames = new();
            if (Teacher.Approbations != null)
            {
                foreach (var a in Teacher.Approbations)
                {
                    approbationNames.Add(a.SubjectType.Name);
                }
            }
            else
            {
                approbationNames.Add(" ");
            }
            table.AddCell("Aprobace").SetFont(defaultFont);
            table.AddCell(String.Join(", ", approbationNames)).SetFont(defaultFont);

            Teacher.EducationLevel ??= "";
            table.AddCell("Dosažené vzdělání").SetFont(defaultFont);
            table.AddCell(Teacher.EducationLevel).SetFont(defaultFont);

            doc.Add(table);
            doc.Close();
            return File(stream.ToArray(), "application/pdf", $"{Teacher.GetFullName()} - výpis.pdf");
        }
    }
}
