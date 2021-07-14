using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeTypes;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IConfiguration configuration;
        private readonly StudentService studentService;
        private readonly StudentAccessValidation studentAccessValidation;

        private string UserId { get; set; }

        public SubjectMaterialsController(SubjectMaterialService subjectMaterialService, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, IConfiguration configuration, StudentService studentService, StudentAccessValidation studentAccessValidation)
        {
            this.subjectMaterialService = subjectMaterialService;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.configuration = configuration;
            this.studentService = studentService;
            this.studentAccessValidation = studentAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        /// <summary>
        /// Gets all subject materials in a subject
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns></returns>
        [HttpGet("Teacher/Material")]
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
            IEnumerable<SubjectMaterial> subjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstanceForTeacher(subjectInstanceId);
            List<SubjectMaterialObject> output = new List<SubjectMaterialObject>();
            foreach (var sm in subjectMaterials)
            {
                SubjectMaterialObject smo = new SubjectMaterialObject
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
                    AddedById = sm.AddedById,
                    SubjectMaterialGroupId = sm.SubjectMaterialGroupId,
                    SubjectMaterialGroupAddedBy = sm.SubjectMaterialGroup != null ? (SubjectMaterialObject.USERTYPE?)sm.SubjectMaterialGroup?.AddedBy : null,
                    SubjectMaterialGroupAddedById = sm.SubjectMaterialGroup?.AddedById
                };
                output.Add(smo);
            }
            return output;
        }


        /// <summary>
        /// Adds subject material with specified details
        /// </summary>
        /// <param name="formFileObject"></param>
        /// <returns></returns>
        [HttpPost("Teacher/Material")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult> AddSubjectMaterial([FromForm] FormFileObject formFileObject)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (formFileObject.Material.SubjectTypeId != null)
            {
                if (!await teacherAccessValidation.HasAccessToSubjectType(teacherId, (int)formFileObject.Material.SubjectTypeId))
                {
                    return Forbid();
                }
            }
            if (!await teacherAccessValidation.HasAccessToSubject(teacherId, formFileObject.Material.SubjectInstanceId))
            {
                return Forbid();
            }
            if (formFileObject.FormFile.Length > 0)
            {
                string fileExt = Path.GetExtension(formFileObject.FormFile.FileName);
                Guid materialId = Guid.NewGuid();
                Response<BlobContentInfo> response = null;
                try
                {
                    //Uploading to blob storage

                    string connectionString = configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("subjectmaterials");
                    //using FileStream uploadFileStream = System.IO.File.OpenRead(filePath);
                    using Stream uploadStream = formFileObject.FormFile.OpenReadStream();
                    BlobClient blobClient = containerClient.GetBlobClient($"{materialId}{fileExt}");
                    response = await blobClient.UploadAsync(uploadStream, true);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
                if (response == null)
                {
                    return BadRequest();
                }
                //Adding to our db
                SubjectMaterial toAdd =
                        new SubjectMaterial()
                        {
                            Id = materialId,
                            Added = DateTime.UtcNow,
                            AddedBy = SubjectMaterial.USERTYPE.Teacher,
                            Name = formFileObject.Material.Name,
                            Description = formFileObject.Material.Description,
                            FileExt = fileExt,
                            FileType = MimeTypeMap.GetMimeType(fileExt),
                            SubjectTypeId = formFileObject.Material.SubjectTypeId,
                            SubjectMaterialGroupId = formFileObject.Material.SubjectMaterialGroupId,
                            AddedById = teacherId,
                            SubjectInstanceId = formFileObject.Material.SubjectInstanceId
                        };
                bool successfullyAdded = await subjectMaterialService.AddMaterialAsync(toAdd);
                if (!successfullyAdded)
                {
                    return BadRequest();
                }
                return CreatedAtAction("PostSubjectMaterial", new { Id = toAdd.Id });
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates subject material
        /// </summary>
        /// <param name="sm">Needs to have a specified id</param>
        /// <returns></returns>
        [HttpPut("Teacher/Material")]
        public async Task<ActionResult> UpdateSubjectMaterial(SubjectMaterialObject sm)
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            if (!await teacherAccessValidation.HasAccessToSubjectMaterial(teacherId, Guid.Parse(sm.Id)))
            {
                return Forbid();
            }
            try
            {
                await subjectMaterialService.UpdateMaterialAsync(new SubjectMaterial()
                {
                    Id = Guid.Parse(sm.Id),
                    Name = sm.Name,
                    Description = sm.Description,
                    FileExt = sm.FileExt,
                    FileType = sm.FileType,
                    Added = DateTime.UtcNow,
                    AddedBy = (SubjectMaterial.USERTYPE)sm.AddedBy,
                    SubjectTypeId = sm.SubjectTypeId,
                    AddedById = sm.AddedById,
                    SubjectMaterialGroupId = sm.SubjectMaterialGroupId
                });

            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Soft deletes subject material
        /// </summary>
        /// <param name="subjectMaterialId"></param>
        /// <returns></returns>
        [HttpDelete("Teacher/Material")]
        public async Task<ActionResult> DeleteSubjectMaterial(Guid subjectMaterialId)
        {
            await subjectMaterialService.SoftDeleteMaterialAsync(subjectMaterialId);
            return Ok();
        }

        /// <summary>
        /// Adds subject material group
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("Teacher/MaterialGroup")]
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
        [HttpPut("Teacher/MaterialGroup")]
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
        [HttpDelete("Teacher/MaterialGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult> DeleteSubjectMaterialGroup(int subjectMaterialGroupId)
        {
            await subjectMaterialService.DeleteMaterialGroupAsync(subjectMaterialGroupId);
            return Ok();
        }

        //----------------
        //Student section
        //----------------

        /// <summary>
        /// Gets all subject materials in a subject
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns></returns>
        [HttpGet("Student/Material")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> StudentGetSubjectMaterials(int subjectInstanceId)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (!await studentAccessValidation.HasAccessToSubject(studentId, subjectInstanceId))
            {
                return Forbid();
            }
            IEnumerable<SubjectMaterial> subjectMaterials = await subjectMaterialService.GetAllMaterialsBySubjectInstance(subjectInstanceId);
            List<SubjectMaterialObject> output = new List<SubjectMaterialObject>();
            foreach (var sm in subjectMaterials)
            {
                SubjectMaterialObject smo = new SubjectMaterialObject
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
                    AddedById = sm.AddedById,
                    SubjectMaterialGroupId = sm.SubjectMaterialGroupId,
                    SubjectMaterialGroupAddedBy = sm.SubjectMaterialGroup != null ? (SubjectMaterialObject.USERTYPE?)sm.SubjectMaterialGroup?.AddedBy : null,
                    SubjectMaterialGroupAddedById = sm.SubjectMaterialGroup?.AddedById
                };
                output.Add(smo);
            }
            return output;
        }

        /// <summary>
        /// Adds a subject material
        /// </summary>
        /// <param name="formFileObject"></param>
        /// <returns></returns>
        [HttpPost("Student/Material")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult> StudentAddSubjectMaterial([FromForm] FormFileObject formFileObject)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (formFileObject.Material.SubjectTypeId != null)
            {
                if (!await studentAccessValidation.HasAccessToSubjectType(studentId, (int)formFileObject.Material.SubjectTypeId))
                {
                    return Forbid();
                }
            }
            if (!await studentAccessValidation.HasAccessToSubject(studentId, formFileObject.Material.SubjectInstanceId))
            {
                return Forbid();
            }

            if (formFileObject.FormFile.Length > 0)
            {
                string fileExt = Path.GetExtension(formFileObject.FormFile.FileName);
                Guid materialId = Guid.NewGuid();
                Response<BlobContentInfo> response = null;
                try
                {
                    //Uploading to blob storage

                    string connectionString = configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("subjectmaterials");
                    //using FileStream uploadFileStream = System.IO.File.OpenRead(filePath);
                    using Stream uploadStream = formFileObject.FormFile.OpenReadStream();
                    BlobClient blobClient = containerClient.GetBlobClient($"{materialId}{fileExt}");
                    response = await blobClient.UploadAsync(uploadStream, true);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
                if (response == null)
                {
                    return BadRequest();
                }
                //Adding to our db
                SubjectMaterial toAdd =
                        new SubjectMaterial()
                        {
                            Id = materialId,
                            Added = DateTime.UtcNow,
                            AddedBy = SubjectMaterial.USERTYPE.Student,
                            Name = formFileObject.Material.Name,
                            Description = formFileObject.Material.Description,
                            FileExt = fileExt,
                            FileType = MimeTypeMap.GetMimeType(fileExt),
                            SubjectTypeId = formFileObject.Material.SubjectTypeId,
                            SubjectInstanceId = formFileObject.Material.SubjectInstanceId,
                            SubjectMaterialGroupId = formFileObject.Material.SubjectMaterialGroupId,
                            AddedById = studentId
                        };
                bool successfullyAdded = await subjectMaterialService.AddMaterialAsync(toAdd);
                if (!successfullyAdded)
                {
                    return BadRequest();
                }
                return CreatedAtAction("PostSubjectMaterial", new { Id = toAdd.Id });
            }
            return BadRequest();
        }

        /// <summary>
        /// Soft deletes subject material group
        /// </summary>
        /// <param name="subjectMaterialId"></param>
        /// <returns></returns>
        [HttpDelete("Student/Material")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult> StudentDeleteSubjectMaterial(Guid subjectMaterialId)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (!await studentAccessValidation.HasAccessToSubjectMaterial(studentId, subjectMaterialId))
            {
                return StatusCode(403);
            }
            await subjectMaterialService.SoftDeleteMaterialAsync(subjectMaterialId);
            return Ok();
        }

        /// <summary>
        /// Adds subject material group for student
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("Student/MaterialGroup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult> StudentAddSubjectMaterialGroup(string name)
        {
            int studentId = await studentService.GetStudentId(UserId);
            SubjectMaterialGroup smg = new SubjectMaterialGroup()
            {
                Name = name,
                AddedById = studentId,
                AddedBy = SubjectMaterialGroup.USERTYPE.Student
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
        [HttpPut("Student/MaterialGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<IEnumerable<SubjectMaterialObject>>> StudentUpdateSubjectMaterialGroup(int subjectMaterialGroupId, string name)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (!await studentAccessValidation.HasAccessToSubjectMaterialGroup(studentId, subjectMaterialGroupId))
            {
                return Forbid();
            }
            SubjectMaterialGroup smg = new SubjectMaterialGroup()
            {
                Id = subjectMaterialGroupId,
                Name = name,
                AddedBy = SubjectMaterialGroup.USERTYPE.Student,
                AddedById = studentId
            };
            await subjectMaterialService.UpdateMaterialGroupAsync(smg);
            return Ok();
        }

        /// <summary>
        /// Deletes subject material group
        /// </summary>
        /// <param name="subjectMaterialGroupId"></param>
        /// <returns></returns>
        [HttpDelete("Student/MaterialGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult> StudentDeleteSubjectMaterialGroup(int subjectMaterialGroupId)
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (!await studentAccessValidation.HasAccessToSubjectMaterialGroup(studentId, subjectMaterialGroupId))
            {
                return Forbid();
            }
            await subjectMaterialService.DeleteMaterialGroupAsync(subjectMaterialGroupId);
            return Ok();
        }
    }
    public class FormFileObject
    {
        public IFormFile FormFile { get; set; }
        public SubjectMaterialUploadObject Material { get; set; }
    }
    public class SubjectMaterialUploadObject
    {
#nullable enable
        public string Name { get; set; }
        public string? Description { get; set; }

        public int? SubjectTypeId { get; set; }
        public int SubjectInstanceId { get; set; }
        public int? SubjectMaterialGroupId { set; get; }
    }
    public class SubjectMaterialObject
    {
#nullable enable
        public enum USERTYPE { Teacher, Student }

        public string? Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? FileExt { get; set; }
        public string? FileType { get; set; } //MIME Media type https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        public DateTime? Added { get; set; }
        public USERTYPE? AddedBy { get; set; }
        public int AddedById { set; get; }
        public string? SubjectMaterialGroupName { set; get; }
        public USERTYPE? SubjectMaterialGroupAddedBy { set; get; }
        public int? SubjectMaterialGroupAddedById { set; get; }

        public int? SubjectTypeId { get; set; }
        public int SubjectInstanceId { get; set; }
        public int? SubjectMaterialGroupId { set; get; }
    }
}

