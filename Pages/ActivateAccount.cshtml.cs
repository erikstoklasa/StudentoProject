using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Pages
{
    public class ActivateAccountModel : PageModel
    {
        private readonly SchoolContext _context;
        private UserManager<IdentityUser> UsersManager;
        private SignInManager<IdentityUser> SignInManager;
        private string UserId { get; set; }

        public ActivateAccountModel(SchoolContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        {
            _context = context;
            UsersManager = userManager;
            SignInManager = signInManager;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId = UserId == null ? "" : UserId;

        }
        public async Task<IActionResult> OnPostAsync(string humanCode)
        {
            /*
             1) Access Human Readable codes
             2) Edit Student UserAuthId with the one gotten from Human readable codes db
            */
            HumanActivationCode code = await _context.HumanActivationCodes
                    .Where(g => g.HumanCode == humanCode).FirstOrDefaultAsync();
            int id = code.Id; //Student Or Teacher id
            CodeType codeType = code.CodeType;

            if (codeType == CodeType.Student)
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound(); //Needs to be redone
                }
                if(student.UserAuthId != null)
                {
                    return NotFound(); //UserAuthId is already set
                    //Needs to be redone
                }
                //Updating User Auth id
                student.UserAuthId = UserId;
                if (await TryUpdateModelAsync<Student>(
                    student,
                    "student",
                    s => s.UserAuthId, s => s.FirstName, s => s.LastName))
                {
                    _context.HumanActivationCodes.Remove(code);
                    await _context.SaveChangesAsync();
                }
                //Assigning user a student role
                IdentityUser user;
                user = UsersManager.FindByIdAsync(UserId).Result;
                UsersManager.AddToRoleAsync(user, "student").Wait();
            }
            else
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    return NotFound();
                }
                teacher.UserAuthId = UserId;
                if (await TryUpdateModelAsync<Teacher>(
                    teacher,
                    "teacher",
                    t => t.UserAuthId, t => t.FirstName, t => t.LastName))
                {
                    _context.HumanActivationCodes.Remove(code);
                    await _context.SaveChangesAsync();
                }
                //Assigning user a teacher role
                IdentityUser user;
                user = UsersManager.FindByIdAsync(UserId).Result;
                UsersManager.AddToRoleAsync(user, "teacher").Wait();
            }
            await SignInManager.SignOutAsync();
            return LocalRedirect(Url.Page("/Account/Login", new { area = "Identity" }));
        }
    }
}
