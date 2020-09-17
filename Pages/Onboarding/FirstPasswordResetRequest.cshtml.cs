using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages
{
    public class FirstPasswordResetRequest : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender emailSender;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        public FirstPasswordResetRequest(UserManager<IdentityUser> userManager, IEmailSender emailSender, StudentService studentService, TeacherService teacherService)
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
                IdentityUser user = new IdentityUser();
                Models.Student s;
                Models.Teacher t = null;
                s = await studentService.GetStudentByEmailAsync(Email);
                if (s != null)
                {
                    user.Email = s.Email;
                    user.UserName = s.Email;
                }
                else
                {
                    t = await teacherService.GetTeacherByEmailAsync(Email);
                    if (t != null)
                    {
                        user.Email = t.Email;
                        user.UserName = t.Email;
                    } else
                    {
                        ModelState.AddModelError("UserNotFound", "User was not found by the email you entered.");
                    }
                }
                var result = await _userManager.CreateAsync(user, RandomString(10) + "l1D!"); //GENERATE RANDOM PASSWORD
                if (!result.Succeeded)
                {
                    foreach(var e in result.Errors)
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
                else if (t != null)
                {
                    _userManager.AddToRoleAsync(user, "teacher").Wait();
                    t.UserAuthId = user.Id;
                    await teacherService.UpdateTeacherAsync(t, null);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Onboarding/FirstPasswordReset",
                    pageHandler: null,
                    values: new { code, userId = user.Id },
                    protocol: Request.Scheme);

                await emailSender.SendEmailAsync(
                    Email,
                    "Vítejte ve Studentu!",
                    $"Vytvořte si prosím své heslo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknutím zde</a>.");

                ViewData["status"] = "Úspěch, zkontrolujte si svůj email, na který jsme vám poslali odkaz pro vytvoření nového hesla.";
                return Page();
            }
            return Page();
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
