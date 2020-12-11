using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student
{
    public class TimetableModel : PageModel
    {
        private readonly TimetableManager timetableManager;
        private readonly StudentService studentService;
        private readonly ILogger logger;

        public string UserId { get; set; }
        public int StudentId { get; set; }
        public System.Globalization.CultureInfo Provider { get; set; }
        public TimetableModel(IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, StudentService studentService, ILogger<TimetableModel> logger)
        {
            this.timetableManager = timetableManager;
            this.studentService = studentService;
            this.logger = logger;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimeFramesByDay = new List<TimeFrame>[5];
            for (int i = 0; i < TimeFramesByDay.Length; i++)
            {
                TimeFramesByDay[i] = new List<TimeFrame>();
            }
            Provider = System.Globalization.CultureInfo.InvariantCulture;
        }
        public Timetable Timetable { get; set; }
        public DateTime TermStart { get; set; }
        public List<TimeFrame>[] TimeFramesByDay { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? DisplayModeRow { get; set; }
        public int WeekInTerm { get; set; }
        public async Task OnGetAsync()
        {
            var timer = new Stopwatch();
            timer.Start();

            StudentId = await studentService.GetStudentId(UserId);
            TermStart = DateTime.ParseExact("01/09/2020", "dd/MM/yyyy", Provider);
            WeekInTerm = (DateTime.Now.DayOfYear - (TermStart.DayOfYear - (int)TermStart.DayOfWeek)) / 7;
            Timetable = await timetableManager.GetTimetableForStudent(StudentId, WeekInTerm);
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            logger.LogDebug($"Time taken: {timeTaken:ss\\.fff}");

            TimeFrame tf;
            for (int i = 0; i < Timetable.TimeFrames.Count; i++)
            {
                tf = Timetable.TimeFrames[i];
                TimeFramesByDay[(int)tf.DayOfWeek - 1].Add(tf);
            }
        }
        public string GetColorByItemId(int id)
        {
            string[] colors = {
                "#473CB1",
                "#FEB13C",
                "#B56F04",
                "#4F81B9",
                "#EF950B",
                "#79A3D2",
                "#FED33C",
                "#FFE58A",
                "#feb23c",
                "#B58E04",
                "#FFC060",
                "#6359C2",
                "#FFD18A",
                "#3168A6",
                "#13549C",
                "#0C3E76"
            };
            return colors[id % colors.Length];
        }
    }
}
