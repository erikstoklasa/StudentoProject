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

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LogIn(InputModelObject credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            if (result.RequiresTwoFactor)
            {
                return BadRequest("You require 2FA");
            }
            if (result.IsLockedOut)
            {
                return BadRequest("You are locked out");
            }
            return BadRequest("Neověřená emailová adresa, nebo špatné údaje");
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
}
