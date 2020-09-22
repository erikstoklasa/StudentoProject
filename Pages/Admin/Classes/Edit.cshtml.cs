using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Classes
{
    public class EditModel : PageModel
    {
        private readonly ClassService classService;
        private readonly TeacherService teacherService;
        private readonly RoomService roomService;

        public List<SelectListItem> TeachersList { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public List<SelectListItem> RoomsList { get; set; }

        public EditModel(ClassService classService, TeacherService teacherService, RoomService roomService)
        {
            this.classService = classService;
            this.teacherService = teacherService;
            this.roomService = roomService;
            TeachersList = new List<SelectListItem>();
            RoomsList = new List<SelectListItem>();
        }

        [BindProperty]
        public Class Class { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Class = await classService.GetClassAsync((int)id);

            if (Class == null)
            {
                return NotFound();
            }
            Teachers = (await teacherService.GetAllTeachersAsync()).ToList();
            foreach (Models.Teacher t in Teachers)
            {
                bool teacherSelected = Class.TeacherId == t.Id;
                TeachersList.Add(new SelectListItem(t.GetFullName(), t.Id.ToString(), teacherSelected));
            }
            var rooms = await roomService.GetAllRooms();
            foreach (var room in rooms)
            {
                bool roomSelected = room.Id == Class.BaseRoomId;
                RoomsList.Add(new SelectListItem(room.Name, room.Id.ToString(), roomSelected));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Class.SchoolId = (await classService.GetClassAsync(Class.Id)).SchoolId;
            await classService.UpdateClassAsync(Class);

            return RedirectToPage("./Index");
        }
    }
}
