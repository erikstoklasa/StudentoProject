using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages
{
    public class FirstPasswordReset : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> SignInManager;

        [BindProperty]
        public string Password { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        public FirstPasswordReset(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            SignInManager = signInManager;
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
                        if (e.Code == "PasswordRequiresLower")
                        {
                            ModelState.AddModelError(e.Code, "Heslo musí obsahovat nejméně jedno malé písmeno");
                        }
                        else if (e.Code == "PasswordRequiresUpper")
                        {
                            ModelState.AddModelError(e.Code, "Heslo musí obsahovat nejméně jedno velké písmeno");
                        }
                        else if (e.Code == "PasswordTooShort")
                        {
                            ModelState.AddModelError(e.Code, "Heslo musí mít nejméně 6 znaků");
                        }
                        else
                        {
                            ModelState.AddModelError(e.Code, e.Description);
                        }

                    }
                    return Page();
                }

            }
            return Page();
        }
    }
}
