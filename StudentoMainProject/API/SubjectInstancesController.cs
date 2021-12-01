using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.Services;
using StudentoMainProject.Models;
using System.Net;

namespace SchoolGradebook.API.SubjectInstances
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectInstancesController : ControllerBase
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly LogItemService logItemService;

        private string UserId { get; set; }
        public IPAddress IPAddress { get; set; }

        public SubjectInstancesController(SubjectService subjectService,
                                          StudentService studentService,
                                          TeacherService teacherService,
                                          TeacherAccessValidation teacherAccessValidation,
                                          StudentAccessValidation studentAccessValidation,
                                          IHttpContextAccessor httpContextAccessor,
                                          LogItemService logItemService)
        {
            this.subjectService = subjectService;
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.studentAccessValidation = studentAccessValidation;
            this.logItemService = logItemService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }

        /// <summary>
        /// Gets a single subject instance for teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Teacher/{id}")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<SubjectInstanceObject>> TeacherGetSubjectInstance(int id)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, id))
            {
                return StatusCode(403);
            }
            SubjectInstance si = await subjectService.GetSubjectInstanceAsync(id);
            ICollection<Student> students = await studentService.GetAllStudentsBySubjectInstanceAsync(id);
            List<UserObject> userObjects = new();
            foreach (var s in students)
            {
                userObjects.Add(new UserObject() { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName });
            }
            return new SubjectInstanceObject()
            {
                Id = si.Id,
                Name = si.SubjectType.Name,
                Teacher = new UserObject() { Id = si.TeacherId, FirstName = si.Teacher.FirstName, LastName = si.Teacher.LastName },
                Students = userObjects
            };
        }

        /// <summary>
        /// Gets a single subject instance for student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Student/{id}")]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<SubjectInstanceObject>> StudentGetSubjectInstance(int id)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (studentId == -1)
            {
                return StatusCode(403);
            }
            if (!await studentAccessValidation.HasAccessToSubject(studentId, id))
            {
                return StatusCode(403);
            }
            await logItemService.Log(
                new LogItem
                {
                    EventType = "SubjectDetail",
                    Timestamp = DateTime.UtcNow,
                    UserAuthId = UserId,
                    UserId = studentId,
                    UserRole = "student",
                    IPAddress = IPAddress.ToString(),
                    EventParam = id
                });
            SubjectInstance si = await subjectService.GetSubjectInstanceAsync(id);
            ICollection<Student> students = await studentService.GetAllStudentsBySubjectInstanceAsync(id);
            List<UserObject> userObjects = new();
            foreach (var s in students)
            {
                userObjects.Add(new UserObject() { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName });
            }
            ;
            return new SubjectInstanceObject()
            {
                Id = si.Id,
                Name = si.SubjectType.Name,
                Teacher = new UserObject() { Id = si.TeacherId, FirstName = si.Teacher.FirstName, LastName = si.Teacher.LastName },
                Students = userObjects
            };
        }
        /// <summary>
        /// Gets all subject instances for student
        /// </summary>
        /// <returns></returns>
        [HttpGet("Student")]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<ICollection<SubjectInstanceObject>>> StudentGetAllSubjectInstances()
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (studentId == -1)
            {
                return StatusCode(403);
            }
            List<SubjectInstance> subjectInstances = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            List<SubjectInstanceObject> output = new();
            foreach (var si in subjectInstances)
            {
                output.Add(new SubjectInstanceObject()
                {
                    Id = si.Id,
                    Name = si.SubjectType.Name,
                    Teacher = new UserObject() { Id = si.TeacherId, FirstName = si.Teacher.FirstName, LastName = si.Teacher.LastName }
                });
            }
            return output;
        }
        /// <summary>
        /// Get all subject instances for teacher
        /// </summary>
        /// <remarks>
        /// Does not retrive teacher (because the caller is the teacher) or students enrolled
        /// </remarks>
        /// <returns></returns>
        [HttpGet("Teacher")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<ICollection<SubjectInstanceObject>>> TeacherGetAllSubjectInstances()
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            List<SubjectInstance> subjectInstances = await subjectService.GetAllSubjectInstancesByTeacherAsync(teacherId);
            List<SubjectInstanceObject> subjectInstanceObjects = new();
            foreach (var si in subjectInstances)
            {
                subjectInstanceObjects.Add(
                    new SubjectInstanceObject()
                    {
                        Id = si.Id,
                        Name = si.SubjectType.Name
                    });
            }
            return subjectInstanceObjects;
        }
    }


    public class SubjectInstanceObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserObject Teacher { get; set; }
        public ICollection<UserObject> Students { get; set; }
    }
    public class UserObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}