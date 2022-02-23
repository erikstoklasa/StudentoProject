using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Services;
using StudentoMainProject.API.Models;
using StudentoMainProject.Models;
using StudentoMainProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
        private readonly LogItemService logItemService;

        public string UserAuthId { get; set; }

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IHttpContextAccessor httpContextAccessor,
                              StudentService studentService,
                              TeacherService teacherService,
                              LogItemService logItemService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.logItemService = logItemService;
            UserAuthId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }
        public IPAddress IPAddress { get; set; }
        /// <summary>
        /// Gets the auth session cookie
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseObject>> LogIn(LoginObject credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(credentials.Email);
                var roles = await userManager.GetRolesAsync(user);
                if (roles == null)
                {
                    return BadRequest(new LoginResponseObject() { Error = "Uživatel nemá žádnou roli" });
                }
                await logItemService.Log(
                    new LogItem
                    {
                        EventType = "AuthSuccess",
                        Timestamp = DateTime.UtcNow,
                        UserAuthId = UserAuthId,
                        UserRole = roles.FirstOrDefault(),
                        IPAddress = IPAddress.ToString()
                    });
                UserObject userObject = new();
                switch (roles.FirstOrDefault())
                {
                    case "student":
                        userObject.UserType = UserObject.USERTYPE.Student;
                        break;
                    case "admin":
                        userObject.UserType = UserObject.USERTYPE.Admin;
                        break;
                    case "teacher":
                        userObject.UserType = UserObject.USERTYPE.Teacher;
                        break;
                    default:
                        break;
                }
                return new LoginResponseObject() { User = userObject};
            }
            if (result.RequiresTwoFactor)
            {
                return BadRequest(new LoginResponseObject() { Error = "Authentifikace vyžaduje dvoufaktotové ověření" });
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new LoginResponseObject() { Error = "Účet byl zablokován" });
            }
            return BadRequest(new LoginResponseObject() { Error = "Neověřená emailová adresa, nebo špatné údaje" });
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
                        Id = userId,
                        UserType = UserObject.USERTYPE.TeacherClassmaster,
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName
                    };
                }
                else //User is a teacher, but has no assigned classes
                {
                    return new UserObject()
                    {
                        Id = userId,
                        UserType = UserObject.USERTYPE.Teacher,
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
                    Id = userId,
                    UserType = UserObject.USERTYPE.Student,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                };
                return user;
            }
        }
    }
    public class LoginObject
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class LoginResponseObject
    {
        public UserObject User { get; set; }
        public string Error { get; set; }
    }
}
