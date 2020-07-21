using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Subjects.Materials
{
    public class DeleteModel : PageModel
    {
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly TeacherService teacherService;
        public int TeacherId { get; set; }
        public string UserId { get; set; }

        public DeleteModel(SubjectMaterialService subjectMaterialService, TeacherAccessValidation teacherAccessValidation, TeacherService teacherService, IHttpContextAccessor httpContextAccessor)
        {
            this.subjectMaterialService = subjectMaterialService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }


        [BindProperty]
        public SubjectMaterial SubjectMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            if (id == null)
            {
                return NotFound();
            }
            bool teacherHasAccessToMaterial = await teacherAccessValidation.HasAccessToSubjectMaterial(TeacherId, (Guid)id);
            if (!teacherHasAccessToMaterial)
            {
                return BadRequest();
            }
            SubjectMaterial = await subjectMaterialService.GetMaterialAsync((Guid)id);

            if (SubjectMaterial == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            if (id == null)
            {
                return NotFound();
            }
            bool teacherHasAccessToMaterial = await teacherAccessValidation.HasAccessToSubjectMaterial(TeacherId, (Guid)id);
            if (!teacherHasAccessToMaterial)
            {
                return BadRequest();
            }
            await subjectMaterialService.DeleteMaterialAsync((Guid)id);
            return LocalRedirect($"~/Teacher/Subjects/Index");
        }
    }
}
