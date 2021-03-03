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
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly StudentService studentService;

        private string UserId { get; set; }

        public GradesController(GradeService gradeService, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, StudentAccessValidation studentAccessValidation, StudentService studentService)
        {
            this.gradeService = gradeService;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.studentAccessValidation = studentAccessValidation;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// Gets all grades by the selected filter
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="subjectInstanceId"></param>
        /// <returns>Grades</returns>
        /// <response code="200">Returns grades</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected student/subject</response>
        [HttpGet("Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<IEnumerable<GradeObject>>> TeacherGetGrades(int? studentId, int? subjectInstanceId)
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
                var grades = await gradeService.GetAllGradesAddedByTeacherAsync((int)studentId);
                foreach (var g in grades)
                {
                    gradeObjects.Add(
                        new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.GetGradeValueInDecimal().ToString() }
                        );
                }
                return gradeObjects;
            }
            if (subjectInstanceId != null)
            {
                if (!await teacherAccessValidation.HasAccessToSubject(teacherId, (int)subjectInstanceId))
                {
                    return StatusCode(403);
                }
                var grades = await gradeService.GetAllGradesAddedByTeacherAsync((int)subjectInstanceId);
                foreach (var g in grades)
                {
                    gradeObjects.Add(
                        new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.GetGradeValueInDecimal().ToString() }
                        );
                }
                return gradeObjects;
            }
            return Ok();
        }

        /// <summary>
        /// Gets a single grade
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Grade</returns>
        /// <response code="200">Returns the grade</response>
        /// <response code="404">If grade is not found</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected grade</response>
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Teacher/{id}")]
        public async Task<ActionResult<GradeObject>> TeacherGetGrade(int id)
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
            return new GradeObject
            { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, Value = g.GetGradeValueInDecimal().ToString() };
        }

        /// <summary>
        /// Creates a single grade for teacher
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        [HttpPost("Teacher")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherPostGrade(GradeObject grade)
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
                SubjectInstanceId = grade.SubjectInstanceId
            };
            g.SetGradeValue(grade.Value);
            try
            {
                await gradeService.AddGradeAsync(g, Grade.USERTYPE.Teacher);
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

        /// <summary>
        /// Updates a single grade for teacher
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        [HttpPut("Teacher")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherUpdateGrade(GradeObject grade)
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

        /// <summary>
        /// Creates grades for teachers in batch
        /// </summary>
        /// <param name="grades"></param>
        /// <returns></returns>
        [HttpPost("Teacher/Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherPostGrades(ICollection<GradeObject> grades)
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
                var newGrade = new Grade()
                {
                    Added = DateTime.UtcNow,
                    Name = g.Name,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId
                };
                newGrade.SetGradeValue(g.Value);
                gradesToCreate.Add(newGrade);
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

        /// <summary>
        /// Updates grades for teacher in batch 
        /// </summary>
        /// <param name="grades"></param>
        /// <returns></returns>
        [HttpPut("Teacher/Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherUpdateGrades(ICollection<GradeObject> grades)
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
                var newGrade = new Grade()
                {
                    Id = g.Id,
                    Added = DateTime.UtcNow,
                    Name = g.Name,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId
                };
                newGrade.SetGradeValue(g.Value);
                gradesToUpdate.Add(newGrade);
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

        /// <summary>
        /// Deletes grades for teacher in batch
        /// </summary>
        /// <param name="gradeIds"></param>
        /// <returns></returns>
        [HttpDelete("Teacher/Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherDeleteGrades(ICollection<int> gradeIds)
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

        //****************
        //Student requests
        //****************

        /// <summary>
        /// Gets all grades by the selected filter for students
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <param name="formatGrades">True for grade value format 1+, 3- or 2 (falls back to decimal if not found), False (default) for format 0.6; 3,4; 2</param>
        /// <returns>Grades</returns>
        /// <response code="200">Returns grades</response>
        /// <response code="403">If the user is not a student, or is not allowed to view the selected subject</response>
        [HttpGet("Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<IEnumerable<GradeObject>>> StudentGetGrades(int? subjectInstanceId, bool formatGrades = false)
        {
            List<GradeObject> gradeObjects = new List<GradeObject>();
            int loggedInStudentId = await studentService.GetStudentId(UserId);
            if (loggedInStudentId == -1)
            {
                return StatusCode(403);
            }

            if (subjectInstanceId != null)
            {
                if (!await studentAccessValidation.HasAccessToSubject(loggedInStudentId, (int)subjectInstanceId))
                {
                    return StatusCode(403);
                }
                var grades = await gradeService.GetAllGradesByStudentSubjectInstance(loggedInStudentId, (int)subjectInstanceId);
                foreach (var g in grades)
                {
                    var utype = g.AddedBy;
                    var newGrade = new GradeObject
                    {
                        Added = g.Added,
                        AddedBy = (GradeObject.USERTYPE)utype,
                        Id = g.Id,
                        Name = g.Name,
                        StudentId = g.StudentId,
                        SubjectInstanceId = g.SubjectInstanceId
                    };
                    if (formatGrades)
                    {
                        newGrade.Value = g.GetGradeValue();
                    }
                    else
                    {
                        newGrade.Value = g.GetGradeValueInDecimal().ToString();
                    }
                    gradeObjects.Add(newGrade);
                }
                return gradeObjects;
            }
            else //No subject instance id set -> display all grades for student
            {
                var grades = await gradeService.GetAllGradesAsync(loggedInStudentId);
                foreach (var g in grades)
                {
                    var utype = g.AddedBy;
                    var newGrade = new GradeObject
                    {
                        Added = g.Added,
                        AddedBy = (GradeObject.USERTYPE)utype, //Casting the Grade UserType data model enum to the response GradeObject UserType
                        Id = g.Id,
                        Name = g.Name,
                        StudentId = g.StudentId,
                        SubjectInstanceId = g.SubjectInstanceId,
                        Value = g.GetGradeValueInDecimal().ToString()
                    };
                    gradeObjects.Add(newGrade);
                }
                return gradeObjects;
            }
        }


        /// <summary>
        /// Adds a single grade for a student
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("Student")]
        [Authorize(policy: "OnlyStudent")]
        public async Task<IActionResult> StudentPostGrade(GradeObject grade)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (studentId == -1)
            {
                return StatusCode(403);
            }
            if (!await studentAccessValidation.HasAccessToSubject(studentId, grade.SubjectInstanceId))
            {
                return StatusCode(403);
            }
            Grade g = new Grade()
            {
                Added = DateTime.UtcNow,
                Name = grade.Name,
                StudentId = studentId,
                SubjectInstanceId = grade.SubjectInstanceId
            };
            g.SetGradeValue(grade.Value);
            try
            {
                await gradeService.AddGradeAsync(g, Grade.USERTYPE.Student);
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

            return CreatedAtAction("StudentPostGrade", new { id = g.Id }, g);
        }

        /// <summary>
        /// Deletes grades for students in batch
        /// </summary>
        /// <param name="gradeIds"></param>
        /// <returns></returns>
        [HttpDelete("Student/Batch")]
        [Authorize(policy: "OnlyStudent")]
        public async Task<IActionResult> StudentDeleteGrades(ICollection<int> gradeIds)
        {
            int studentId = await studentService.GetStudentId(UserId);
            List<int> idsToDelete = new List<int>();
            if (studentId == -1)
            {
                return StatusCode(403);
            }
            foreach (int id in gradeIds)
            {
                var g = await gradeService.GetPlainGradeAsync(id);
                if (g.AddedBy == Grade.USERTYPE.Student && g.StudentId == studentId)
                {
                    idsToDelete.Add(g.Id);
                }
            }
            try
            {
                await gradeService.DeleteGradesAsync(idsToDelete);
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
        public enum USERTYPE { Teacher, Student }
        public int Id { get; set; }
        public string Value { get; set; }
        public int SubjectInstanceId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public USERTYPE? AddedBy { get; set; }

    }
    public class ErrorObject
    {
        public string Message { get; set; }
    }
}
