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

namespace SchoolGradebook.API.Timetable
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly TimetableManager timetableManager;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        private string UserId { get; set; }

        public TimetableController(IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, StudentService studentService, TeacherService teacherService)
        {
            this.timetableManager = timetableManager;
            this.studentService = studentService;
            this.teacherService = teacherService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// Gets timetable weeks for student
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Timetable/Student/?week=1&wantMultipleWeeks=true
        /// </remarks>
        /// <param name="week">Optional, if not set, will be automatically inferred</param>
        /// <param name="wantMultipleWeeks">True if you want one week before and after the selected week</param>
        /// <returns>Timetable week objects</returns>
        /// <response code="200">Returns timetable weeks</response>
        /// <response code="403">If the user is not a student</response>
        [HttpGet("Student")]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<List<TimetableWeekObject>>> GetTimetableForStudent(int? week, bool wantMultipleWeeks)
        {
            //Term start to be replaced by school's term start
            DateTime termStart = DateTime.ParseExact("01/09/2020", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            week ??= GetWeekInTerm(termStart, DateTime.UtcNow);
            List<int> weeks = new List<int>();
            weeks.Add((int)week);
            if (wantMultipleWeeks)
            {
                weeks.Add((int)week - 1);
                weeks.Add((int)week + 1);
            }
            int studentId = await studentService.GetStudentId(UserId);
            List<TimetableWeekObject> timetableWeeks = new List<TimetableWeekObject>();
            foreach (var w in weeks)
            {
                var tt = await timetableManager.GetTimetableForStudent(studentId, w);
                List<TimeFrameObject> timeFrameObjects = new List<TimeFrameObject>();
                foreach (var tf in tt.TimeFrames)
                {
                    if (tf.TimetableRecord != null && tf.TimetableRecord.SubjectInstanceId != null)
                    {
                        TimetableChangeObject timetableEntryChange = null;
                        //Add new timetable change if the is any change
                        if (tf.TimetableChange != null)
                        {
                            timetableEntryChange = new TimetableChangeObject();
                            if (tf.TimetableChange.Canceled != null)
                            {
                                timetableEntryChange.Cancelled = tf.TimetableChange.Canceled;
                            }
                            if (tf.TimetableChange.CurrentRoom != null)
                            {
                                timetableEntryChange.Room = tf.TimetableChange.CurrentRoom.Name;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstanceId != null)
                            {
                                timetableEntryChange.SubjectInstanceId = (int)tf.TimetableChange.CurrentSubjectInstanceId;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstance != null)
                            {

                                timetableEntryChange.SubjectInstanceName = tf.TimetableChange.CurrentSubjectInstance.SubjectType.Name;
                            }
                            if (tf.TimetableChange.CurrentTeacher != null)
                            {
                                timetableEntryChange.TeacherFirstName = tf.TimetableChange.CurrentTeacher.FirstName;
                                timetableEntryChange.TeacherLastName = tf.TimetableChange.CurrentTeacher.LastName;
                            }

                        }
                        timeFrameObjects.Add(new TimeFrameObject()
                        {
                            Id = tf.Id,
                            StartTime = tf.Start,
                            EndTime = tf.End,
                            DayOfWeek = tf.DayOfWeek,
                            TimetableEntry = new TimetableEntryObject()
                            {
                                Room = tf.TimetableRecord.Room.Name,
                                SubjectInstanceId = (int)tf.TimetableRecord.SubjectInstanceId,
                                TeacherFirstName = tf.TimetableRecord.SubjectInstance.Teacher.FirstName,
                                TeacherLastName = tf.TimetableRecord.SubjectInstance.Teacher.LastName,
                                SubjectInstanceName = tf.TimetableRecord.SubjectInstance.SubjectType.Name

                            },
                            TimetableEntryChange = timetableEntryChange
                        });
                    }
                    else //No subject in timeframe
                    {
                        timeFrameObjects.Add(new TimeFrameObject()
                        {
                            Id = tf.Id,
                            StartTime = tf.Start,
                            EndTime = tf.End,
                            DayOfWeek = tf.DayOfWeek
                        });
                    }
                }
                DateTime thisWeekStart = GetMondayFromWeek(w, termStart);
                timetableWeeks.Add(new TimetableWeekObject()
                {
                    WeekStart = thisWeekStart,
                    Week = w,
                    TimeFrames = timeFrameObjects
                });

            }
            return timetableWeeks;
        }
        /// <summary>
        /// Gets timetable weeks for teacher
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Timetable/Teacher?week=1&wantMultipleWeeks=true
        /// </remarks>
        /// <param name="week">Optional, if not set, will be automatically inferred</param>
        /// <param name="wantMultipleWeeks">True if you want one week before and after the selected week</param>
        /// <returns>Timetable week objects</returns>
        /// <response code="200">Returns timetable weeks</response>
        /// <response code="403">If the user is not a teacher</response>
        [HttpGet("Teacher")]
        [Authorize(policy: "OnlyTeacher")]
        public async Task<ActionResult<List<TimetableWeekObject>>> GetTimetableForTeacher(int? week, bool wantMultipleWeeks)
        {
            //Term start to be replaced by school's term start
            DateTime termStart = DateTime.ParseExact("01/09/2020", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            week ??= GetWeekInTerm(termStart, DateTime.UtcNow);
            List<int> weeks = new List<int>();
            weeks.Add((int)week);
            if (wantMultipleWeeks)
            {
                weeks.Add((int)week - 1);
                weeks.Add((int)week + 1);
            }
            int teacherId = await teacherService.GetTeacherId(UserId);
            List<TimetableWeekObject> timetableWeeks = new List<TimetableWeekObject>();
            foreach (var w in weeks)
            {
                var tt = await timetableManager.GetTimetableForTeacher(teacherId, w);
                List<TimeFrameObject> timeFrameObjects = new List<TimeFrameObject>();
                foreach (var tf in tt.TimeFrames)
                {
                    if (tf.TimetableRecord != null && tf.TimetableRecord.SubjectInstanceId != null)
                    {
                        TimetableChangeObject timetableEntryChange = null;
                        //Add new timetable change if the is any change
                        if (tf.TimetableChange != null)
                        {
                            timetableEntryChange = new TimetableChangeObject();
                            if (tf.TimetableChange.Canceled != null)
                            {
                                timetableEntryChange.Cancelled = tf.TimetableChange.Canceled;
                            }
                            if (tf.TimetableChange.CurrentRoom != null)
                            {
                                timetableEntryChange.Room = tf.TimetableChange.CurrentRoom.Name;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstanceId != null)
                            {
                                timetableEntryChange.SubjectInstanceId = (int)tf.TimetableChange.CurrentSubjectInstanceId;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstance != null)
                            {
                                timetableEntryChange.SubjectInstanceName = tf.TimetableChange.CurrentSubjectInstance.SubjectType.Name;
                            }
                            if (tf.TimetableChange.CurrentTeacher != null)
                            {
                                timetableEntryChange.TeacherFirstName = tf.TimetableChange.CurrentTeacher.FirstName;
                                timetableEntryChange.TeacherLastName = tf.TimetableChange.CurrentTeacher.LastName;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstance.Enrollments != null)
                            {
                                timetableEntryChange.Group = GetGroupName(tf.TimetableChange.CurrentSubjectInstance.Enrollments);
                            }

                        }
                        timeFrameObjects.Add(new TimeFrameObject()
                        {
                            Id = tf.Id,
                            StartTime = tf.Start,
                            EndTime = tf.End,
                            DayOfWeek = tf.DayOfWeek,
                            TimetableEntry = new TimetableEntryObject()
                            {
                                Room = tf.TimetableRecord.Room.Name,
                                SubjectInstanceId = (int)tf.TimetableRecord.SubjectInstanceId,
                                SubjectInstanceName = tf.TimetableRecord.SubjectInstance.SubjectType.Name,
                                Group = GetGroupName(tf.TimetableRecord.SubjectInstance.Enrollments)
                            },
                            TimetableEntryChange = timetableEntryChange,
                            LessonRecordId = tf.LessonRecord?.Id
                        });


                    }
                    else //No subject in timeframe
                    {
                        timeFrameObjects.Add(new TimeFrameObject()
                        {
                            Id = tf.Id,
                            StartTime = tf.Start,
                            EndTime = tf.End,
                            DayOfWeek = tf.DayOfWeek
                        });
                    }
                }
                DateTime thisWeekStart = GetMondayFromWeek(w, termStart);
                timetableWeeks.Add(new TimetableWeekObject()
                {
                    WeekStart = thisWeekStart,
                    Week = w,
                    TimeFrames = timeFrameObjects
                });

            }
            return timetableWeeks;
        }
        private string GetGroupName(ICollection<SubjectInstanceEnrollment> enrollments)
        {
            List<string> groupNames = new List<string>();
            foreach (var e in enrollments)
            {
                groupNames.Add(e.StudentGroup.GetName());
            }
            return String.Join(",", groupNames);
        }
        private int GetWeekInTerm(DateTime termStart, DateTime targetDate)
            => (targetDate.AddDays(1) - termStart).Days / 7;
        private DateTime GetMondayFromWeek(int week, DateTime termStart)
            => termStart.AddDays((week * 7) - 1);



    }


    public class TimetableWeekObject
    {
        public int Week { get; set; }
        public DateTime WeekStart { get; set; } //Day when the week starts (Monday)
        public IEnumerable<TimeFrameObject> TimeFrames { get; set; }
    }
    public class TimeFrameObject
    {
#nullable enable
        public int Id { set; get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimetableEntryObject? TimetableEntry { get; set; }
        public TimetableChangeObject? TimetableEntryChange { get; set; }
        public int? LessonRecordId { get; set; } //Only for teacher

    }
    public class TimetableEntryObject
    {
#nullable enable
        public string Room { get; set; }
        public int SubjectInstanceId { get; set; }
        public string SubjectInstanceName { get; set; }
        public string? TeacherFirstName { get; set; }
        public string? TeacherLastName { get; set; }
        public string? Group { get; set; } //Only for teacher
    }
    public class TimetableChangeObject
    {
#nullable enable
        public bool? Cancelled { get; set; }
        public string Room { get; set; }
        public int SubjectInstanceId { get; set; }
        public string SubjectInstanceName { get; set; }
        public string? TeacherFirstName { get; set; }
        public string? TeacherLastName { get; set; }
        public string? Group { get; set; } //Only for teacher
    }
}
