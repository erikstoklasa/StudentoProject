using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolGradebook.Services;
using SchoolGradebook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.API.GradeAverage
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeAverageController : ControllerBase
    {
        

        private readonly TeacherService teacherService;
        private readonly GradeAverageService gradeAverageService;
        private readonly GradeGroupService gradeGroupService;
        private readonly StudentService studentService;


        //constructor


        /// <summary>
        /// Gets an average for subject instance
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>
        /// Returns newest grade average for subject instance
        /// </returns>
        [HttpGet("SubjectInstance/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<GradeAverageObject>> GetSubjectInstanceAverage(Guid subjectInstanceId)
        {
            
            int teacherId = await teacherService.GetTeacherId(UserId);

            if (!await teacherAccessValidation.HasAccessToSubjectInstance(teacherId, subjectInstanceId))
            {
                return Forbid();
            }
            
            GradeAverage gradeAverage = await gradeAverageService.getGradeAverageForSubjectInstance(subjectInstanceId);
            GradeAverageObject output;


            output = new GradeAverageObject()
            {
                Id = gradeAverage.Id,
                SubjectInstanceId = subjectInstanceId,
                Value = gradeAverage.Value,
                TeacherId = gradeAverage.teacherId,
                StudentId = gradeAverage.studentId,
                Added = gradeAverage.Added;

                

            };
 
            return output;
        }

        [HttpGet("SubjectInstanceStudent/{Id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<GradeAverageObject>> GetSubjectInstanceStudentAverage(Guid subjectInstanceId) {
    
            int studentId = await StudentService.GetStudentId(UserId);

            GradeAverage gradeAverage = await GradeAverageService.getGradeAverageForStudent(subjectInstanceId, studentId);

            GradeAverageObject output;

            output = new GradeAverageObject()
                Id = gradeAverage.Id,
                SubjectInstanceId = subjectInstanceId,
                Value = gradeAverage.Value,
                TeacherId = gradeAverage.teacherId,
                StudentId = gradeAverage.studentId,
                Added = gradeAverage.Added;
    
    
    
    
    
    
    
        }



        /// <summary>
        /// Calculates a new average for a subject instance
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>
        /// Action result
        /// </returns>
        [HttpPost("CalculateSubjectInstanceAverage/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<GradeAverageObject>> CalculateGradeAverageForSubjectInstance(Guid subjectInstanceId)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);

            if (!await teacherAccessValidation.HasAccessToSubjectInstance(teacherId, subjectInstanceId))
            {
                return Forbid();
            }
            var grades = await gradeService.GetAllGradesBySubjectInstance(subjectInstanceId);
            double average = 0;
            int totalWeight = 0;
            IDictionary<int, int> rememberMap = new Dictionary<int, int>(); // map/dictionary to remember grade groups - first arg is id to prevent having to find gradeGroup for every grade (we find once per group)
            foreach (var g in grades) { 
                var newGrade = new GradeObject { Added = g.Added, Id = g.Id, Name = g.Name, StudentId = g.StudentId, SubjectInstanceId = g.SubjectInstanceId, GradeGroupId = g.GradeGroupId, GradeGroupName = g.GradeGroup?.Name };
                int weight = 0;
                if (rememberMap.Contains(g.GradeGroupId)) {
                    weight = rememberMap[g.gradeGroupId];
                
                
                }
                else {
                    var gradeGroup = await gradeGroupService.GetGradeGroupAsyncv(g.GradeGroupId);
                    weight = gradeGroup.Weight;
                    rememberMap[g.GradeGroupId] = weight;
                }
                average = average + g.Value;
                totalWeight = totalWeight + weight;
                

            
            }
            average = average / totalWeight;
            GradeAverage gradeAverage = new GradeAverage() {
                SubjectInstanceId = subjectInstanceId,
                Value = average,
                TeacherId = teacherId,
                Added = DateTime.UtcNow,
                StudentId = -1,
            
            
            
            };
            await gradeAverageService.storeGradeAverageSubjectInstance(gradeAverage);




        }
        
        /// <summary>
        /// Calculates an average for a student for a given subjectInstance
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>Action result</returns>
        [HttpPost("CalculateStudentSubjectInstanceAverage/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCose.Status)]
        [Authorize(policy : "OnlyStudent")]
        public async Task<ActionResult<GradeAverageObject>> CalculateGradeAverageForStudentsOnSubjectInstance(Guid subjectInstanceId) {


            int studentId = await studentService.GetStudentId(UserId);
            var grades = await gradeService.GetAllGradesByStudentSubjectInstance(studentId, subjectInstanceId);
            double average = 0;
            int totalWeight = 0;
            foreach(var g in grades) {
                var gradeGroup = await gradeGroupService.GetGradeGroupAsyncv(g.GradeGroupId);
                int weight = gradeGroup.Weight;
                grades = grades + g.Value * weight;    
                totalWeight = totalWeight + weight;
    
            }
            average = average / totalWeight;
            GradeAverage gradeAverage = new GradeAverage() {
                SubjectInstanceId = subjectInstanceId,
                Value = average,
                TeacherId = -1,
                Added = DateTime.UtcNow,
                StudentId = studentId,
            
            
            
            };
            await gradeAverageService.storeGradeAverageSubjectInstance(gradeAverage);

        }

    }
}
