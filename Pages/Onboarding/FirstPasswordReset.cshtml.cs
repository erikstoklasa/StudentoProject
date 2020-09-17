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

namespace SchoolGradebook.Pages
{
    public class FirstPasswordReset : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly IEmailSender emailSender;

        [BindProperty]
        public string Password { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        public FirstPasswordReset(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            SignInManager = signInManager;
            this.emailSender = emailSender;

        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser user;
                user = _userManager.FindByIdAsync(UserId).Result;
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                var result = await _userManager.ResetPasswordAsync(user, Code, Password);
                if (result.Succeeded)
                {
                    string emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, emailConfirmToken);
                    await SignInManager.SignInAsync(user, null);
                    return LocalRedirect("/");
                }
                else
                {
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError(e.Code, e.Description);
                    }
                    return Page();
                }

            }
            return Page();
        }
    }
}
