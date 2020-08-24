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

namespace SchoolGradebook.Pages.Student
{
    public class TimetableModel : PageModel
    {
        private readonly TimetableManager timetableManager;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;
        private readonly RoomService roomService;

        public string UserId { get; set; }
        public int StudentId { get; set; }
        public TimetableModel(IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, StudentService studentService, SubjectService subjectService, RoomService roomService)
        {
            this.timetableManager = timetableManager;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.roomService = roomService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimeFramesByDay = new List<TimeFrame>[5];
            for (int i = 0; i < TimeFramesByDay.Length; i++)
            {
                TimeFramesByDay[i] = new List<TimeFrame>();
            }
        }
        public Timetable Timetable { get; set; }
        public List<TimeFrame>[] TimeFramesByDay { get; set; }
        public async Task OnGetAsync()
        {
            StudentId = await studentService.GetStudentId(UserId);
            Timetable = await timetableManager.GetTimetableForStudent(StudentId, 1);
            Timetable.TimeFrames = Timetable.TimeFrames.OrderBy(tf => tf.Start.TimeOfDay).ToList();
            TimeFrame tf;
            for (int i = 0; i < Timetable.TimeFrames.Count; i++)
            {
                tf = Timetable.TimeFrames[i];
                if (tf.RoomId != null)
                {
                    tf.Room = await roomService.GetRoomById((int)tf.RoomId);
                }
                if (tf.SubjectInstanceId != null)
                {
                    tf.SubjectInstance = await subjectService.GetSubjectInstanceAsync((int)tf.SubjectInstanceId);
                }
                TimeFramesByDay[(int)tf.DayOfWeek - 1].Add(tf);
            }
        }
    }
}
