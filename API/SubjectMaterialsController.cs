using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.API.SubjectMaterials
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectMaterialsController : ControllerBase
    {
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;

        private string UserId { get; set; }

        public SubjectMaterialsController(SubjectMaterialService subjectMaterialService, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation)
        {
            this.subjectMaterialService = subjectMaterialService;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// Gets a single subject material
        /// </summary>
        /// <param name="subjectMaterialId"></param>
        /// <returns></returns>
        [HttpGet("Material/Teacher/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<SubjectMaterialObject>> GetSubjectMaterial(Guid subjectMaterialId)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (!await teacherAccessValidation.HasAccessToSubjectMaterial(teacherId, subjectMaterialId))
            {
                return Forbid();
            }
            SubjectMaterial subjectMaterial = await subjectMaterialService.GetMaterialAsync(subjectMaterialId);
            SubjectMaterialObject output;


            output = new SubjectMaterialObject()
            {
                Id = subjectMaterial.Id.ToString(),
                Name = subjectMaterial.Name,
                Description = subjectMaterial.Description,
                FileExt = subjectMaterial.FileExt,
                FileType = subjectMaterial.FileType,
                Added = subjectMaterial.Added,
                AddedBy = (SubjectMaterialObject.USERTYPE)subjectMaterial.AddedBy,
                SubjectMaterialGroupName = subjectMaterial.SubjectMaterialGroup?.Name,
                SubjectTypeId = subjectMaterial.SubjectTypeId,
                TeacherId = subjectMaterial.TeacherId,
                SubjectMaterialGroupId = subjectMaterial.SubjectMaterialGroupId,
                SubjectMaterialGroupAddedBy = (SubjectMaterialObject.USERTYPE)subjectMaterial.SubjectMaterialGroup?.AddedBy,
                SubjectMaterialGroupAddedById = subjectMaterial.SubjectMaterialGroup?.AddedById
            };

            return output;
        }


        /// <summary>
        /// Gets all subject materials in a subject
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns></returns>
        [HttpGet("Material/Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> GetSubjectMaterials(int subjectInstanceId)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, subjectInstanceId))
            {
                return Forbid();
            }
            IEnumerable<SubjectMaterial> subjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstance(subjectInstanceId);
            List<SubjectMaterialObject> output = new List<SubjectMaterialObject>();
            foreach (var sm in subjectMaterials)
            {
                output.Add(new SubjectMaterialObject()
                {
                    Id = sm.Id.ToString(),
                    Name = sm.Name,
                    Description = sm.Description,
                    FileExt = sm.FileExt,
                    FileType = sm.FileType,
                    Added = sm.Added,
                    AddedBy = (SubjectMaterialObject.USERTYPE)sm.AddedBy,
                    SubjectMaterialGroupName = sm.SubjectMaterialGroup?.Name,
                    SubjectTypeId = sm.SubjectTypeId,
                    TeacherId = sm.TeacherId,
                    SubjectMaterialGroupId = sm.SubjectMaterialGroupId,
                    SubjectMaterialGroupAddedBy = (SubjectMaterialObject.USERTYPE)sm.SubjectMaterialGroup?.AddedBy,
                    SubjectMaterialGroupAddedById = sm.SubjectMaterialGroup?.AddedById
                });
            }
            return output;
        }


        ///// <summary>
        ///// Adds subject material
        ///// </summary>
        ///// <param name="material"></param>
        ///// <returns></returns>
        //[HttpPost("Material/Teacher")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(policy: "OnlyTeacher")]
        //public async Task<ActionResult> AddSubjectMaterial(SubjectMaterialObject material)
        //{
        //    int teacherId = await teacherService.GetTeacherId(UserId);
        //    if (!await teacherAccessValidation.HasAccessToSubjectType(teacherId, material.SubjectTypeId))
        //    {
        //        return Forbid();
        //    }
        //    SubjectMaterial toAdd =
        //        new SubjectMaterial()
        //        {
        //            Added = DateTime.UtcNow,
        //            AddedBy = (SubjectMaterial.USERTYPE)material.AddedBy,
        //            Name = material.Name,
        //            Description = material.Description,
        //            FileExt = material.FileExt,
        //            FileType = material.FileType,
        //            SubjectTypeId = material.SubjectTypeId,
        //            SubjectMaterialGroupId = material.SubjectMaterialGroupId,
        //            TeacherId = material.TeacherId
        //        };
        //    bool successfullyAdded = await subjectMaterialService.AddMaterialAsync(toAdd);
        //    if (!successfullyAdded)
        //    {
        //        return BadRequest();
        //    }

        //    return CreatedAtAction("PostSubjectMaterial", new { Id = toAdd.Id });
        //}
        //[HttpGet("Teacher")]
        //public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> UpdateSubjectMaterial(int subjectInstanceId)
        //{
        //    return Ok();
        //}
        //[HttpGet("Teacher")]
        //public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> DeleteSubjectMaterial(int subjectInstanceId)
        //{
        //    return Ok();
        //}
        /// <summary>
        /// Adds subject material group
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("MaterialGroup/Teacher")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult> AddSubjectMaterialGroup(string name)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            SubjectMaterialGroup smg = new SubjectMaterialGroup()
            {
                Name = name,
                AddedById = teacherId,
                AddedBy = SubjectMaterialGroup.USERTYPE.Teacher
            };
            if (await subjectMaterialService.AddMaterialGroupAsync(smg))
            {

                return CreatedAtAction("PostSubjectMaterialGroup", new { Id = smg.Id });
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Updates subject material group
        /// </summary>
        /// <param name="subjectMaterialGroupId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("MaterialGroup/Teacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> UpdateSubjectMaterialGroup(int subjectMaterialGroupId, string name)
        {
            SubjectMaterialGroup smg = new SubjectMaterialGroup()
            {
                Id = subjectMaterialGroupId,
                Name = name
            };
            await subjectMaterialService.UpdateMaterialGroupAsync(smg);
            return Ok();
        }
        /// <summary>
        /// Deletes subject material group
        /// </summary>
        /// <param name="subjectMaterialGroupId"></param>
        /// <returns></returns>
        [HttpDelete("MaterialGroupTeacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult> DeleteSubjectMaterialGroup(int subjectMaterialGroupId)
        {
            await subjectMaterialService.DeleteMaterialGroupAsync(subjectMaterialGroupId);
            return Ok();
        }


        //[HttpGet("Student")]
        //public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> StudentDeleteSubjectMaterialGroup(int subjectInstanceId)
        //{
        //    return Ok();

        //}
    }
    public class SubjectMaterialObject
    {
        public enum USERTYPE { Teacher, Student }

        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string FileExt { get; set; }
        public string FileType { get; set; } //MIME Media type https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        public DateTime Added { get; set; }
        public USERTYPE AddedBy { get; set; }
        public string SubjectMaterialGroupName { set; get; }
        public USERTYPE SubjectMaterialGroupAddedBy { set; get; }
        public int? SubjectMaterialGroupAddedById { set; get; }

        public int SubjectTypeId { get; set; }
        public int TeacherId { set; get; }
        public int? SubjectMaterialGroupId { set; get; }
    }
}

