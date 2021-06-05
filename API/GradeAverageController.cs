using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolGradebook.Services;
using SchoolGradebook.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.API.GradeAverages
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeAverageController : ControllerBase
    {
        

        private readonly TeacherService teacherService;
        private readonly GradeAverageService gradeAverageService;
        private readonly GradeGroupService gradeGroupService;
        private readonly StudentService studentService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly GradeService gradeService;


        private string UserId { get; set; }
        

        public GradeAverageController(GradeService gradeService, TeacherService teacherService, GradeAverageService gradeAverageService, GradeGroupService gradeGroupService, StudentService studentService, IHttpContextAccessor httpContextAccessor, TeacherAccessValidation teacherAccessValidation) {

            this.gradeService = gradeService;
            this.teacherService = teacherService;
            this.gradeAverageService = gradeAverageService;
            this.gradeGroupService = gradeGroupService;
            this.studentService = studentService;
            this.teacherAccessValidation = teacherAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);



        }

        /// <summary>
        /// Gets an average for subject instance
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>
        /// Returns newest grade average for subject instance
        /// </returns>
        [HttpGet("SubjectInstance")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<GradeAverage>> GetSubjectInstanceAverage(int subjectInstanceId)
        {
            
            int teacherId = await teacherService.GetTeacherId(UserId);

            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, subjectInstanceId))
            {
                return Forbid();
            }
            
            var gradeAverage = await gradeAverageService.GetGradeAverageForSubjectInstance(subjectInstanceId);
            //GradeAverageObject output;


            //output = new GradeAverageObject()
            //{
            //    Id = gradeAverage.Id,
            //    SubjectInstanceId = subjectInstanceId,
            //    Value = gradeAverage.Value,
            //    TeacherId = gradeAverage.TeacherId,
            //    StudentId = gradeAverage.StudentId,
            //    Added = gradeAverage.Added,

                

            //};
 
            return gradeAverage;
        }
        /// <summary>
        /// Gets an average for a subject for a student
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>
        /// Returns the newest grade average for subject instance/student id combination
        /// </returns>
        [HttpGet("SubjectInstanceStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<GradeAverage>> GetSubjectInstanceStudentAverage(int subjectInstanceId) {

            int studentId = await studentService.GetStudentId(UserId);

            var gradeAverage = await gradeAverageService.GetGradeAverageForStudent(subjectInstanceId, studentId);

            //GradeAverageObject output;

            //output = new GradeAverageObject() {
            //    Id = gradeAverage.Id,
            //    SubjectInstanceId = subjectInstanceId,
            //    Value = gradeAverage.Value,
            //    TeacherId = gradeAverage.TeacherId,
            //    StudentId = gradeAverage.StudentId,
            //    Added = gradeAverage.Added,



            //};


            return gradeAverage;
        }



        /// <summary>
        /// Calculates a new average for a subject instance
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>
        /// Action result
        /// </returns>
        [HttpPost("CalculateSubjectInstanceAverage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<GradeAverageObject>> CalculateGradeAverageForSubjectInstance(int subjectInstanceId)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);

            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, subjectInstanceId))
            {
                return Forbid();
            }
            var grades = await gradeService.GetAllGradesAddedByTeacherAsync(subjectInstanceId);
            double average = 0;
            int totalWeight = 0;
            IDictionary<int, int> rememberMap = new Dictionary<int, int>(); // map/dictionary to remember grade groups - first arg is id to prevent having to find gradeGroup for every grade (we find once per group)
            foreach (var g in grades) { 
                
                int weight = 0;

                if (rememberMap.TryGetValue(g.GradeGroupId.Value, out weight)) {
                    weight = rememberMap[g.GradeGroupId.Value];

                }
                else { 
                    var gradeGroup = await gradeGroupService.GetGradeGroupAsync(g.GradeGroupId.Value);
                    weight = gradeGroup.Weight;
                    rememberMap[g.GradeGroupId.Value] = weight;

                }
                
                
                average =+ g.Value;
                totalWeight =+ weight;
                

            
            }
            average /= totalWeight;
            GradeAverage gradeAverage = new GradeAverage() {
                SubjectInstanceId = subjectInstanceId,
                Value = average,
                TeacherId = teacherId,
                Added = DateTime.UtcNow,
                StudentId = -1,
            
            
            
            };
            await gradeAverageService.AddGradeAverageForSubjectInstace(gradeAverage);


            return Ok();

        }
        
        /// <summary>
        /// Calculates an average for a student for a given subjectInstance
        /// </summary>
        /// /
        /// <param name="subjectInstanceId"></param>
        /// <returns>Action result</returns>
        [HttpPost("CalculateStudentSubjectInstanceAverage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        
        [Authorize(policy : "OnlyStudent")]
        public async Task<ActionResult<GradeAverageObject>> CalculateGradeAverageForStudentsOnSubjectInstance(int subjectInstanceId) {


            int studentId = await studentService.GetStudentId(UserId);
            var grades = await gradeService.GetAllGradesByStudentSubjectInstance(studentId, subjectInstanceId);
            double average = 0;
            int totalWeight = 0;
            foreach(var g in grades) {
                var gradeGroup = await gradeGroupService.GetGradeGroupAsync(g.GradeGroupId.Value);
                int weight = gradeGroup.Weight;
                average =+ g.Value * weight;    
                totalWeight =+ weight;
    
            }
            average /= totalWeight;
            GradeAverage gradeAverage = new GradeAverage() {
                SubjectInstanceId = subjectInstanceId,
                Value = average,
                TeacherId = -1,
                Added = DateTime.UtcNow,
                StudentId = studentId,
            
            
            
            };
            await gradeAverageService.AddGradeAverageForSubjectInstace(gradeAverage);

            return Ok();
        }

    }

    public class GradeAverageObject
    {
        public enum USERTYPE { Teacher, Student }

        public int Id { get; set; }

        public int SubjectInstanceId { get; set; }
        public double Value { get; set; } //For table in the db
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public DateTime Added { get; set; }

    }

}
