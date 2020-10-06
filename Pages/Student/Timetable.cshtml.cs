using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student
{
    public class TimetableModel : PageModel
    {
        private readonly TimetableManager timetableManager;
        private readonly StudentService studentService;
        public string UserId { get; set; }
        public int StudentId { get; set; }
        public TimetableModel(IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, StudentService studentService)
        {
            this.timetableManager = timetableManager;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimeFramesByDay = new List<TimeFrame>[5];
            for (int i = 0; i < TimeFramesByDay.Length; i++)
            {
                TimeFramesByDay[i] = new List<TimeFrame>();
            }
        }
        public Timetable Timetable { get; set; }
        public DateTime TermStart { get; set; }
        public List<TimeFrame>[] TimeFramesByDay { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? DisplayModeRow { get; set; }
        public int WeekInTerm { get; set; }
        public async Task OnGetAsync()
        {
            StudentId = await studentService.GetStudentId(UserId);
            TermStart = DateTime.Parse("01/09/2020");
            WeekInTerm = (DateTime.Now.DayOfYear - (TermStart.DayOfYear - (int)TermStart.DayOfWeek)) / 7;
            Timetable = await timetableManager.GetTimetableForStudent(StudentId, WeekInTerm);
            TimeFrame tf;
            for (int i = 0; i < Timetable.TimeFrames.Count; i++)
            {
                tf = Timetable.TimeFrames[i];
                TimeFramesByDay[(int)tf.DayOfWeek - 1].Add(tf);
            }
        }
    }
}
