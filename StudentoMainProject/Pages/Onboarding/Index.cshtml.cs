using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SchoolGradebook.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        public RequestResponse EmailRequestResponse;
        [BindProperty(SupportsGet = true)]
        [EmailAddress]
        public string Email { get; set; }
        public IConfiguration Configuration { get; }

        public FirstPasswordResetRequest(IConfiguration configuration, UserManager<IdentityUser> userManager, StudentService studentService, TeacherService teacherService)
        {
            Configuration = configuration;
            _userManager = userManager;
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            IdentityUser user = new();
            Models.Teacher teacher = null;
            Models.Student student = await studentService.GetStudentByEmailAsync(Email);
            string firstName;
            if (student != null)
            {

                if (string.IsNullOrEmpty(student.UserAuthId)) //Is not already assigned a user account
                {
                    user.Email = student.Email;
                    user.UserName = student.Email;
                    firstName = student.FirstName;
                }
                else
                {
                    ModelState.AddModelError("UserAlreadyAssigned", "Tenhle email už má aktivovaný účet. Stačí se přihlásit na hlavní stránce Studento.cz");
                    return Page();
                }
            }
            else //Should be teacher
            {
                teacher = await teacherService.GetTeacherByEmailAsync(Email);
                if (teacher != null)
                {
                    if (string.IsNullOrEmpty(teacher.UserAuthId))
                    {
                        user.Email = teacher.Email;
                        user.UserName = teacher.Email;
                        firstName = teacher.FirstName;
                    }
                    else
                    {
                        ModelState.AddModelError("UserAlreadyAssigned", "Tenhle email už má aktivovaný účet. Stačí se přihlásit na hlavní stránce Studento.cz");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError("UserNotFound", "Zkontroluj zadaný email, protože tenhle bohužel není pozvaný.");
                    return Page();
                }
            }
            await _userManager.CreateAsync(user, RandomString(10) + "l1D!"); //Generate temp random password

            if (student != null)
            {
                await _userManager.AddToRoleAsync(user, "student");
                student.UserAuthId = user.Id;
                await studentService.UpdateStudentAsync(student);
            }
            else if (teacher != null)
            {
                await _userManager.AddToRoleAsync(user, "teacher");
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
            //Sending email
            var client = new SendGridClient(Configuration.GetConnectionString("SEND_GRID_KEY"));
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("mailer@studento.cz", "Studento.cz"),
                ReplyTo = new EmailAddress("moje@studento.cz")
            };
            msg.SetTemplateId("d-dc8c591fcd6b4cf8b88d3904a7c44b61");
            msg.SetTemplateData(new
            {
                resetUrl = HtmlEncoder.Default.Encode(callbackUrl),
                name = LanguageHelper.Sklonuj(firstName)
            });
            msg.AddTo(new EmailAddress(Email));
            var requestResult = await client.SendEmailAsync(msg);
            //Done sending email
            if (requestResult.IsSuccessStatusCode)
            {
                EmailRequestResponse.Message = "Super, teď si zkontroluj svůj email, na který jsme ti poslali odkaz.";
                EmailRequestResponse.Success = true;
            }
            else
            {
                EmailRequestResponse.Message = "Nepodařilo se odeslat email na tvojí emailovou schránku, zkus to prosím později.";
                EmailRequestResponse.Success = false;
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
