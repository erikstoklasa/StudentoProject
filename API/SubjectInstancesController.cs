using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectInstancesController : ControllerBase
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;

        public SubjectInstancesController(SubjectService subjectService, StudentService studentService)
        {
            this.subjectService = subjectService;
            this.studentService = studentService;
        }
        [HttpGet("{id}")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<SubjectInstanceObject>> GetSubjectInstance(int id)
        {
            SubjectInstance si = await subjectService.GetSubjectInstanceAsync(id);
            ICollection<Student> students = await studentService.GetAllStudentsBySubjectInstanceAsync(id);
            List<UserObject> userObjects = new List<UserObject>();
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