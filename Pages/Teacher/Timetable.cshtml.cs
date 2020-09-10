using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Teacher
{
    public class TimetableModel : PageModel
    {
        private readonly TimetableManager timetableManager;
        private readonly TeacherService teacherService;
        public string UserId { get; set; }
        public int TeacherId { get; set; }
        public TimetableModel(IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, TeacherService teacherService)
        {
            this.timetableManager = timetableManager;
            this.teacherService = teacherService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimeFramesByDay = new List<TimeFrame>[5];
            for (int i = 0; i < TimeFramesByDay.Length; i++)
            {
                TimeFramesByDay[i] = new List<TimeFrame>();
            }
        }
        public Timetable Timetable { get; set; }
        public List<TimeFrame>[] TimeFramesByDay { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? DisplayModeRow { get; set; }
        public DateTime TermStart { get; set; }
        public async Task OnGetAsync()
        {
            TeacherId = await teacherService.GetTeacherId(UserId);

            TermStart = DateTime.Parse("01/09/2020");
            int weekInTerm = (DateTime.Now.DayOfYear - (TermStart.DayOfYear - (int)TermStart.DayOfWeek)) / 7;

            Timetable = await timetableManager.GetTimetableForTeacher(TeacherId, weekInTerm);
            TimeFrame tf;
            for (int i = 0; i < Timetable.TimeFrames.Count; i++)
            {
                tf = Timetable.TimeFrames[i];
                TimeFramesByDay[(int)tf.DayOfWeek - 1].Add(tf);
            }
        }
    }
}
