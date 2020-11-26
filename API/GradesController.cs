using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly GradeService gradeService;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;

        private string UserId { get; set; }

        public GradesController(GradeService gradeService, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation)
        {
            this.gradeService = gradeService;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/Grades?studentId=5
        // GET: api/Grades?subjectInstanceId=5
        [HttpGet]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<IEnumerable<GradeObject>>> GetGrades(int? studentId, int? subjectInstanceId)
        {
            List<GradeObject> gradeObjects = new List<GradeObject>();
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }

            if (studentId != null)
            {
                var grades = await gradeService.GetAllGradesByStudentAsync((int)studentId);
                foreach (var g in grades)
                {
                    gradeObjects.Add(new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.Value });
                }
                return gradeObjects;
            }
            if (subjectInstanceId != null)
            {
                if (!await teacherAccessValidation.HasAccessToSubject(teacherId, (int)subjectInstanceId))
                {
                    return StatusCode(403);
                }
                var grades = await gradeService.GetAllGradesBySubjectInstance((int)subjectInstanceId);
                foreach (var g in grades)
                {
                    gradeObjects.Add(new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.Value });
                }
                return gradeObjects;
            }
            return Ok();
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeObject>> GetGrade(int id)
        {
            var g = await gradeService.GetGradeAsync(id);

            if (g == null)
            {
                return StatusCode(404);
            }
            return new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.Value };
        }

        // POST: api/Grades
        [HttpPost]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> PostGrade(GradeObject grade)
        {
            Grade g = new Grade()
            {
                Id = grade.Id,
                Added = grade.Added,
                Name = grade.Name,
                StudentId = grade.StudentId,
                SubjectInstanceId = grade.SubjectInstanceId,
                Value = grade.Value
            };
            try
            {
                await gradeService.AddGradeAsync(g);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

            return CreatedAtAction("PostGrade", new { id = g.Id }, g);
        }

        // POST: api/Grades/Batch
        [HttpPost("Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> PostGrades(ICollection<GradeObject> grades)
        {
            List<Grade> gradesToCreate = new List<Grade>();
            foreach (var g in grades)
            {
                gradesToCreate.Add(new Grade()
                {
                    Id = g.Id,
                    Added = g.Added,
                    Name = g.Name,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    Value = g.Value
                });
            }
            try
            {
                await gradeService.AddGradesAsync(gradesToCreate);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

            return StatusCode(201);
        }

        // Delete: api/Grades/Batch
        [HttpDelete("Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> DeleteGrades(ICollection<int> gradeIds)
        {
            try
            {
                await gradeService.DeleteGradesAsync(gradeIds);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

            return StatusCode(200);
        }
    }
    public class GradeObject
    {
        public int Id { get; set; }
        public int? Value { get; set; }
        public int SubjectInstanceId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }

    }
    public class ErrorObject
    {
        public string Message { get; set; }
    }
}
