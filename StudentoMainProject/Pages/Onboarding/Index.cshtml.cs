using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolGradebook.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages
{
    public class FirstPasswordResetRequest : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailSender emailSender;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;


        public RequestResponse EmailRequestResponse;
        [BindProperty(SupportsGet = true)]
        [EmailAddress]
        public string Email { get; set; }

        public FirstPasswordResetRequest(UserManager<IdentityUser> userManager, EmailSender emailSender, StudentService studentService, TeacherService teacherService)
        {
            _userManager = userManager;
            this.emailSender = emailSender;
            this.studentService = studentService;
            this.teacherService = teacherService;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Email = Email.Trim();
            if (ModelState.IsValid)
            {
                IdentityUser user = new();
                Models.Student s;
                Models.Teacher teacher = null;
                s = await studentService.GetStudentByEmailAsync(Email);
                if (s != null && String.IsNullOrEmpty(s.UserAuthId))
                {
                    user.Email = s.Email;
                    user.UserName = s.Email;
                }
                else
                {
                    teacher = await teacherService.GetTeacherByEmailAsync(Email);
                    if (teacher != null && String.IsNullOrEmpty(teacher.UserAuthId))
                    {
                        user.Email = teacher.Email;
                        user.UserName = teacher.Email;
                    }
                    else
                    {
                        ModelState.AddModelError("UserNotFound", "Zkontroluj zadaný email, protože tenhle bohužel není pozvaný.");
                        return Page();
                    }
                }
                var result = await _userManager.CreateAsync(user, RandomString(10) + "l1D!"); //GENERATE RANDOM PASSWORD
                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError(e.Code, e.Description);
                    }
                    return Page();
                }

                if (s != null)
                {
                    _userManager.AddToRoleAsync(user, "student").Wait();
                    s.UserAuthId = user.Id;
                    await studentService.UpdateStudentAsync(s);
                }
                else if (teacher != null)
                {
                    _userManager.AddToRoleAsync(user, "teacher").Wait();
                    teacher.UserAuthId = user.Id;
                    await teacherService.UpdateTeacherAsync(teacher, null);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Onboarding/FirstPasswordReset",
                    pageHandler: null,
                    values: new { code, userId = user.Id },
                    protocol: Request.Scheme);

                var requestResult = await emailSender.SendEmailWithResponseAsync(
                    Email,
                    "Vítej ve Studentu! 🎉",
                    $"Ahoj, dokonči vytváření svého účtu do aplikace Studento <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknutím zde</a>.");
                if (requestResult.IsSuccessStatusCode)
                {
                    EmailRequestResponse.Message = "Úspěch, zkontroluj si svůj email, na který jsme ti poslali odkaz.";
                    EmailRequestResponse.Success = true;
                }
                else
                {
                    EmailRequestResponse.Message = "Nepodařilo se odeslat email na tvojí emailovou schránku, zkus to prosím později.";
                    EmailRequestResponse.Success = false;
                }
                return Page();
            }
            return Page();
        }
        private static readonly Random random = new();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    public struct RequestResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
}
