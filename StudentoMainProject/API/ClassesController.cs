﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentoMainProject.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ClassService classService;
        private readonly AdminService adminService;
        private readonly StudentService studentService;

        private string UserId { get; set; }

        public ClassesController(IHttpContextAccessor httpContextAccessor,
                                 ClassService classService,
                                 AdminService adminService,
                                 StudentService studentService)
        {
            this.classService = classService;
            this.adminService = adminService;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// Gets all classes at admin's school
        /// </summary>
        /// <returns>Returns all classes at admin's school</returns>
        [HttpGet]
        [Authorize(policy: "AdminAndTeacher")]
        public async Task<ICollection<ClassObject>> TeacherGetAllClasses()
        {
            List<Class> classes = await classService.GetAllClasses();
            List<ClassObject> outputs = new();
            foreach (var c in classes)
            {
                outputs.Add(
                    new ClassObject
                    {
                        Grade = c.Grade,
                        Id = c.Id,
                        Name = c.Name,
                        RoomId = c.BaseRoomId,
                        TeacherId = c.TeacherId
                    });
            }
            return outputs;
        }
        /// <summary>
        /// Deletes class
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("Admin")]
        [Authorize(policy: "OnlyAdmin")]
        public async Task<IActionResult> AdminDeleteClass(int id)
        {
            Class classObject = await classService.GetPlainClassAsync(id);
            
            if (classObject == null)
            {
                return BadRequest("Class not found");
            }
            
            Admin admin = await adminService.GetAdminByUserAuthId(UserId);

            if (classObject.SchoolId != admin.SchoolId)
            {
                return Forbid();
            }

            bool classHasStudents = await studentService.HasAnyStudentsInClassAsync(id);

            if (classHasStudents)
            {
                return BadRequest("You need to remove students from the class first.");
            }

            await classService.DeleteClassAsync(id);
            return Ok();

        }
        /// <summary>
        /// Adds class to admin's school
        /// </summary>
        /// <param name="classObject"></param>
        [HttpPost("Admin")]
        [Authorize(policy: "OnlyAdmin")]
        public async Task<IActionResult> AdminPostClass(ClassObject classObject)
        {
            Admin admin = await adminService.GetAdminByUserAuthId(UserId);
            bool success = await classService.AddClassAsync(
                new Class
                {
                    TeacherId = classObject.TeacherId,
                    SchoolId = admin.SchoolId,
                    BaseRoomId = classObject.RoomId,
                    Grade = classObject.Grade,
                    Name = classObject.Name
                });
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
