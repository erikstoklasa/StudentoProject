using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        public string UserAuthId { get; set; }

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, StudentService studentService, TeacherService teacherService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.studentService = studentService;
            this.teacherService = teacherService;
            UserAuthId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// Gets the auth session cookie
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Clears the auth session cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<ActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        /// <summary>
        /// Gets the information of a user's role and id, only if logged in
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        public async Task<ActionResult<UserObject>> GetUserInfo()
        {
            int userId = await studentService.GetStudentId(UserAuthId);
            //Is user a student?
            if (userId == -1) //User auth id was not found in our student table
            {
                //Is user a teacher?
                userId = await teacherService.GetTeacherId(UserAuthId);
                if (userId == -1)
                {
                    return BadRequest();
                    //User auth id was not found in our teacher table
                }
                
                var teacher = await teacherService.GetTeacherAsync(userId);
                if (await teacherService.TeacherIsClassmaster(userId)) //UserId is teacherId in this case
                                                                       //User is a teacher and has one or more assigned classes
                {
                    return new UserObject()
                    {
                        UserId = userId,
                        UserType = "teacher classmaster",
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName
                    };
                }
                else //User is a teacher, but has no assigned classes
                {
                    return new UserObject()
                    {
                        UserId = userId,
                        UserType = "teacher",
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName
                    };
                }
            }
            else //User is a student
            {
                
                var student = await studentService.GetStudentBasicInfoAsync(userId);
                UserObject user = new()
                {
                    UserId = userId,
                    UserType = "student",
                    FirstName = student.FirstName,
                    LastName = student.LastName
                };
                return user;
            }
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
    public class UserObject
    {
        public int UserId { get; set; }
        public string UserType { get; set; } //Either student, teacher, or classmaster teacher 
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
