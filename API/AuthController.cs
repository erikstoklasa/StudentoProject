using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseObject>> LogIn(InputModelObject credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(credentials.Email);
                var roles = await userManager.GetRolesAsync(user);
                if (roles == null)
                {
                    return BadRequest(new AuthResponseObject() { Error = "User has no role" });
                }
                return new AuthResponseObject() { UserType = roles.FirstOrDefault() };
            }
            if (result.RequiresTwoFactor)
            {
                return BadRequest(new AuthResponseObject() { Error = "Authentication requires 2FA" });
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new AuthResponseObject() { Error = "Account locked out" });
            }
            return BadRequest(new AuthResponseObject() { Error = "Neověřená emailová adresa, nebo špatné údaje" });
        }
        [HttpPost("Logout")]
        public async Task<ActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
    public class InputModelObject
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class AuthResponseObject
    {
        public string UserType { get; set; }
        public string Error { get; set; }
    }
}
