using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Classes
{
    public class CreateModel : PageModel
    {
        private readonly ClassService classService;
        private readonly TeacherService teacherService;
        private readonly AdminService adminService;
        private readonly RoomService roomService;

        public List<SelectListItem> TeachersList { get; set; }
        public List<SelectListItem> RoomsList { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public string UserId { get; set; }

        public CreateModel(ClassService classService, TeacherService teacherService, IHttpContextAccessor httpContextAccessor, AdminService adminService, RoomService roomService)
        {
            this.classService = classService;
            this.teacherService = teacherService;
            this.adminService = adminService;
            this.roomService = roomService;
            TeachersList = new List<SelectListItem>();
            RoomsList = new List<SelectListItem>();
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task<IActionResult> OnGet()
        {
            Teachers = (await teacherService.GetAllTeachersAsync()).ToList();
            foreach (Models.Teacher t in Teachers)
            {
                TeachersList.Add(new SelectListItem(t.GetFullName(), t.Id.ToString()));
            }
            var rooms = await roomService.GetAllRooms();
            foreach (var room in rooms)
            {
                RoomsList.Add(new SelectListItem(room.Name, room.Id.ToString()));
            }
            return Page();
        }

        [BindProperty]
        public Class Class { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Class.SchoolId = await adminService.GetAdminId(UserId);
            await classService.AddClassAsync(Class);

            return RedirectToPage("./Index");
        }
    }
}
