using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.API.Grades
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

        /// <summary>
        /// Gets all grades by the selected filter
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Grades
        ///     {
        ///        "studentId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="studentId"></param>
        /// <param name="subjectInstanceId"></param>
        /// <returns>Grades</returns>
        /// <response code="200">Returns grades</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected student/subject</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                if (!await teacherAccessValidation.HasAccessToStudent(teacherId, (int)studentId))
                {
                    return StatusCode(403);
                }
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

        /// <summary>
        /// Gets a single grade
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Grades
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Grade</returns>
        /// <response code="200">Returns the grade</response>
        /// <response code="404">If grade is not found</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected grade</response>
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeObject>> GetGrade(int id)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            if (!await teacherAccessValidation.HasAccessToGrade(teacherId, id))
            {
                return StatusCode(403);
            }
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
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, grade.SubjectInstanceId))
            {
                return StatusCode(403);
            }
            Grade g = new Grade()
            {
                Added = DateTime.UtcNow,
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

        [HttpPut]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> UpdateGrade(GradeObject grade)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, grade.SubjectInstanceId))
            {
                return StatusCode(403);
            }
            Grade g = new Grade()
            {
                Id = grade.Id,
                Added = DateTime.UtcNow,
                Name = grade.Name,
                StudentId = grade.StudentId,
                SubjectInstanceId = grade.SubjectInstanceId,
                Value = grade.Value
            };
            try
            {
                await gradeService.UpdateGradeAsync(g);
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

            return CreatedAtAction("PutGrade", new { id = g.Id }, g);
        }

        // POST: api/Grades/Batch
        [HttpPost("Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> PostGrades(ICollection<GradeObject> grades)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            List<int> subjectInstanceIdsToCheck = new List<int>();
            List<Grade> gradesToCreate = new List<Grade>();
            List<int> gradeIds = new List<int>();

            foreach (var g in grades)
            {
                gradesToCreate.Add(new Grade()
                {
                    Added = DateTime.UtcNow,
                    Name = g.Name,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    Value = g.Value
                });
                if (!subjectInstanceIdsToCheck.Contains(g.SubjectInstanceId))
                {
                    subjectInstanceIdsToCheck.Add(g.SubjectInstanceId);
                }
            }

            foreach (int id in subjectInstanceIdsToCheck)
            {
                if (!await teacherAccessValidation.HasAccessToSubject(teacherId, id))
                {
                    return StatusCode(403);
                }
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
            foreach (var g in gradesToCreate)
            {
                gradeIds.Add(g.Id);
            }
            return CreatedAtAction("PostGrade", gradeIds, gradesToCreate);
        }

        // PUT: api/Grades/Batch
        [HttpPut("Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> UpdateGrades(ICollection<GradeObject> grades)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            List<int> subjectInstanceIdsToCheck = new List<int>();
            List<Grade> gradesToUpdate = new List<Grade>();

            foreach (var g in grades)
            {
                gradesToUpdate.Add(new Grade()
                {
                    Id = g.Id,
                    Added = DateTime.UtcNow,
                    Name = g.Name,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    Value = g.Value
                });
                if (!subjectInstanceIdsToCheck.Contains(g.SubjectInstanceId))
                {
                    subjectInstanceIdsToCheck.Add(g.SubjectInstanceId);
                }
            }

            foreach (int id in subjectInstanceIdsToCheck)
            {
                if (!await teacherAccessValidation.HasAccessToSubject(teacherId, id))
                {
                    return StatusCode(403);
                }
            }

            try
            {
                await gradeService.UpdateGradesAsync(gradesToUpdate);
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
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            foreach (int id in gradeIds)
            {
                if (!await teacherAccessValidation.HasAccessToGrade(teacherId, id))
                {
                    return StatusCode(403);
                }
            }
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
