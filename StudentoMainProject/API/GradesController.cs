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
        /// <param name="gradeValueFormat">0 = "internal" (default) from -10 to 110, 1 = "decimal" from 0.6 to 5.0, 2 = "display" from 1+ to 5- (falls back to decimal)</param>
        /// <returns>Grades</returns>
        /// <response code="200">Returns grades</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected student/subject</response>
        [HttpGet("Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<IEnumerable<GradeObject>>> TeacherGetGrades(int? studentId, int? subjectInstanceId, int gradeValueFormat = 0)
        {
            List<GradeObject> gradeObjects = new();
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

                    GradeObject newGrade = new(){ Added = g.Added,
                        Id = g.Id,
                        Name = g.Name,
                        Weight = g.Weight,
                        StudentId = g.StudentId,
                        SubjectInstanceId = g.SubjectInstanceId,
                        GradeGroupId = g.GradeGroupId,
                        GradeGroupName = g.GradeGroup?.Name,
                        GradeGroupWeight = g.GradeGroup?.Weight,
                        GradeGroupAdded = (DateTime)(g.GradeGroup?.Added)
                    };
                    newGrade.Value = gradeValueFormat switch
                    {
                        //Internal
                        0 => g.GetInternalGradeValue().ToString(),
                        //Decimal
                        1 => g.GetGradeValueInDecimal().ToString(),
                        //Display
                        2 => g.GetGradeValue(),
                        _ => g.GetInternalGradeValue().ToString(),
                    };
                    gradeObjects.Add(newGrade);
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
                    GradeObject newGrade = new() { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, GradeGroupId = g.GradeGroupId, GradeGroupName = g.GradeGroup?.Name, GradeGroupWeight = g.GradeGroup?.Weight };
                    newGrade.Value = gradeValueFormat switch
                    {
                        //Internal
                        0 => g.GetInternalGradeValue().ToString(),
                        //Decimal
                        1 => g.GetGradeValueInDecimal().ToString(),
                        //Display
                        2 => g.GetGradeValue(),
                        _ => g.GetInternalGradeValue().ToString(),
                    };
                    gradeObjects.Add(newGrade);
                }
                return gradeObjects;
            }
            return Ok();
        }

        /// <summary>
        /// Gets a single grade
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gradeValueFormat">0 = "internal" (default) from -10 to 110, 1 = "decimal" from 0.6 to 5.0, 2 = "display" from 1+ to 5- (falls back to decimal)</param>
        /// <returns>Grade</returns>
        /// <response code="200">Returns the grade</response>
        /// <response code="404">If grade is not found</response>
        /// <response code="403">If the user is not a teacher, or is not allowed to view the selected grade</response>
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Teacher/{id}")]
        public async Task<ActionResult<GradeObject>> TeacherGetGrade(int id, int gradeValueFormat = 0)
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
            GradeObject newGrade = new()
            { Added = g.Added,
                Id = g.Id,
                Name = g.Name,
                Weight = g.Weight,
                StudentId = g.StudentId,
                SubjectInstanceId = g.SubjectInstanceId,
                Value = g.GetInternalGradeValue().ToString(),
                GradeGroupId = g.GradeGroupId,
                GradeGroupName = g.GradeGroup?.Name,
                GradeGroupWeight = g.GradeGroup?.Weight,
                GradeGroupAdded = (DateTime)(g.GradeGroup?.Added)
            };
            newGrade.Value = gradeValueFormat switch
            {
                //Internal
                0 => g.GetInternalGradeValue().ToString(),
                //Decimal
                1 => g.GetGradeValueInDecimal().ToString(),
                //Display
                2 => g.GetGradeValue(),
                _ => g.GetInternalGradeValue().ToString(),
            };
            return newGrade;
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
            Grade g = new()
            {
                Added = grade.Added,
                Name = grade.Name,
                Weight = grade.Weight,
                StudentId = grade.StudentId,
                SubjectInstanceId = grade.SubjectInstanceId,
                GradeGroupId = grade.GradeGroupId,
            };
            try
            {
                int internalGradeValue = Int16.Parse(grade.Value);
                try
                {
                    g.SetGradeValue(internalGradeValue);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    return BadRequest(new ErrorObject() { Message = e.Message });
                }

            }
            catch (FormatException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
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
            Grade g = new()
            {
                Id = grade.Id,
                Added = grade.Added,
                Name = grade.Name,
                Weight = grade.Weight,
                StudentId = grade.StudentId,
                SubjectInstanceId = grade.SubjectInstanceId,
                GradeGroupId = grade.GradeGroupId,
            };
            try
            {
                int internalGradeValue = Int16.Parse(grade.Value);
                try
                {
                    g.SetGradeValue(internalGradeValue);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    return BadRequest(new ErrorObject() { Message = e.Message });
                }

            }
            catch (FormatException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
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
            List<int> subjectInstanceIdsToCheck = new();
            List<Grade> gradesToCreate = new();
            List<int> gradeIds = new();

            foreach (var g in grades)
            {
                var newGrade = new Grade()
                {
                    Added = g.Added,
                    Name = g.Name,
                    Weight = g.Weight,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    GradeGroupId = g.GradeGroupId,
                };
                try
                {
                    int internalGradeValue = Int16.Parse(g.Value);
                    try
                    {
                        newGrade.SetGradeValue(internalGradeValue);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        return BadRequest(new ErrorObject() { Message = e.Message });
                    }

                }
                catch (FormatException e)
                {
                    return BadRequest(new ErrorObject() { Message = e.Message });
                }
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
            List<GradeObject> gradesOutput = new();
            foreach (var g in gradesToCreate)
            {
                gradesOutput.Add(new GradeObject()
                {
                    Added = g.Added,
                    Id = g.Id,
                    Weight = g.Weight,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    Value = g.GetInternalGradeValue().ToString(),
                    GradeGroupId = g.GradeGroupId
                });
                gradeIds.Add(g.Id);
            }
            return CreatedAtAction("PostGrade", gradeIds, gradesOutput);
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
            List<int> subjectInstanceIdsToCheck = new();
            List<Grade> gradesToUpdate = new();

            foreach (var g in grades)
            {
                Grade newGrade = new()
                {
                    Id = g.Id,
                    Added = g.Added,
                    Name = g.Name,
                    Weight = g.Weight,
                    StudentId = g.StudentId,
                    SubjectInstanceId = g.SubjectInstanceId,
                    GradeGroupId = g.GradeGroupId,
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

        //-------------------
        //GradeGroup SECTION
        //-------------------

        /// <summary>
        /// Adds a grade group
        /// </summary>
        /// <param name="gradeGroup"></param>
        /// <returns></returns>
        [HttpPost("Teacher/GradeGroup")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherPostGradeGroup(GradeGroupObject gradeGroup)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            GradeGroup g = new()
            {
                Name = gradeGroup.Name,
                AddedById = teacherId,
                AddedBy = GradeGroup.USERTYPE.Teacher,
                Weight = gradeGroup.Weight,
                Added = gradeGroup.Added
            };
            try
            {
                await gradeService.AddGradeGroupAsync(g);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

            return CreatedAtAction("PostGradeGroup", new { id = g.Id });
        }

        /// <summary>
        /// Updates a single grade group for teacher
        /// </summary>
        /// <param name="gradeGroup"></param>
        /// <returns></returns>
        [HttpPut("Teacher/GradeGroup")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherUpdateGradeGroup(GradeGroupObject gradeGroup)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            if (!await teacherAccessValidation.HasAccessToGradeGroup(teacherId, gradeGroup.Id))
            {
                return StatusCode(403);
            }
            GradeGroup gg = new()
            {
                Id = gradeGroup.Id,
                AddedBy = GradeGroup.USERTYPE.Teacher,
                AddedById = teacherId,
                Name = gradeGroup.Name,
                Weight = gradeGroup.Weight,
                Added = gradeGroup.Added
            };

            try
            {
                await gradeService.UpdateGradeGroupAsync(gg);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

            return CreatedAtAction("PutGradeGroup", new { id = gg.Id });
        }

        /// <summary>
        /// Deletes grade groups for teacher in batch
        /// </summary>
        /// <param name="gradeGroupIds"></param>
        /// <returns></returns>
        [HttpDelete("Teacher/GradeGroup/Batch")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<IActionResult> TeacherDeleteGradeGroups(ICollection<int> gradeGroupIds)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (teacherId == -1)
            {
                return StatusCode(403);
            }
            foreach (int ggId in gradeGroupIds)
            {
                if (!await teacherAccessValidation.HasAccessToGradeGroup(teacherId, ggId))
                {
                    return StatusCode(403);
                }
            }
            try
            {
                await gradeService.DeleteGradeGroupsAsync(gradeGroupIds);
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
        /// <param name="gradeValueFormat">0 = "internal" (default) from -10 to 110, 1 = "decimal" from 0.6 to 5.0, 2 = "display" from 1+ to 5- (falls back to decimal)</param>
        /// <returns>Grades</returns>
        /// <response code="200">Returns grades</response>
        /// <response code="403">If the user is not a student, or is not allowed to view the selected subject</response>
        [HttpGet("Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<IEnumerable<GradeObject>>> StudentGetGrades(int? subjectInstanceId, int gradeValueFormat = 0)
        {
            List<GradeObject> gradeObjects = new();
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
                        Weight = g.Weight,
                        StudentId = g.StudentId,
                        SubjectInstanceId = g.SubjectInstanceId,
                        GradeGroupId = g.GradeGroupId,
                        GradeGroupName = g.GradeGroup?.Name,
                        GradeGroupWeight = g.GradeGroup?.Weight,
                        GradeGroupAdded = g.GradeGroup?.Added,
                    };
                    newGrade.Value = gradeValueFormat switch
                    {
                        //Internal
                        0 => g.GetInternalGradeValue().ToString(),
                        //Decimal
                        1 => g.GetGradeValueInDecimal().ToString(),
                        //Display
                        2 => g.GetGradeValue(),
                        _ => g.GetInternalGradeValue().ToString(),
                    };
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
                        Weight = g.Weight,
                        StudentId = g.StudentId,
                        SubjectInstanceId = g.SubjectInstanceId,
                        Value = g.GetGradeValueInDecimal().ToString(),
                        GradeGroupId = g.GradeGroupId,
                        GradeGroupName = g.GradeGroup?.Name,
                        GradeGroupWeight = g.GradeGroup?.Weight,
                        GradeGroupAdded = g.GradeGroup?.Added,
                    };
                    gradeObjects.Add(newGrade);
                }
                return gradeObjects;
            }
        }


        /// <summary>
        /// Adds a single grade for a student
        /// </summary>
        /// <param name="grade">Expecting internal grade value (-10 to 110)</param>
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
            Grade g = new()
            {
                Added = DateTime.UtcNow,
                Name = grade.Name,
                Weight = grade.Weight,
                StudentId = studentId,
                SubjectInstanceId = grade.SubjectInstanceId
            };

            try
            {
                int internalGradeValue = Int16.Parse(grade.Value);
                try
                {
                    g.SetGradeValue(internalGradeValue);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    return BadRequest(new ErrorObject() { Message = e.Message });
                }

            }
            catch (FormatException e)
            {
                return BadRequest(new ErrorObject() { Message = e.Message });
            }

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
            List<int> idsToDelete = new();
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
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Weight { get; set; }
        public int SubjectInstanceId { get; set; }
        public int StudentId { get; set; }
        public int? GradeGroupId { get; set; }
        public string GradeGroupName { get; set; }
        public DateTime? GradeGroupAdded { get; set; }
        public int? GradeGroupWeight { get; set; }
        public DateTime Added { get; set; }
        public USERTYPE? AddedBy { get; set; }

    }
    public class GradeGroupObject
    {
        public enum USERTYPE { Teacher, Student }
        public int Id { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public int AddedById { get; set; }
        public USERTYPE AddedBy { get; set; }
    }
    public class ErrorObject
    {
        public string Message { get; set; }
    }
}
