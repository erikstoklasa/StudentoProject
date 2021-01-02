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
        private readonly TimetableRecordService timetableRecordsService;
        private readonly TimetableManager timetableManager;
        private readonly StudentService studentService;

        private string UserId { get; set; }

        public TimetableController(TimetableRecordService timetableRecordsService, IHttpContextAccessor httpContextAccessor, TimetableManager timetableManager, StudentService studentService)
        {
            this.timetableRecordsService = timetableRecordsService;
            this.timetableManager = timetableManager;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [HttpGet]
        [Authorize(policy: "OnlyStudent")]
        public async Task<ActionResult<List<TimetableWeekObject>>> GetTimetable(IEnumerable<int> weeks)
        {
            int studentId = await studentService.GetStudentId(UserId);
            List<TimetableWeekObject> timetableWeeks = new List<TimetableWeekObject>();
            foreach (var week in weeks)
            {
                var tt = await timetableManager.GetTimetableForStudent(studentId, week);
                List<TimeFrameObject> timeFrameObjects = new List<TimeFrameObject>();
                foreach (var tf in tt.TimeFrames)
                {
                    if (tf.TimetableRecord != null && tf.TimetableRecord.SubjectInstanceId != null)
                    {
                        TimetableEntryObject timetableEntryNew = null;
                        //Add new timetable change if the is any change
                        if (tf.TimetableChange != null)
                        {
                            timetableEntryNew = new TimetableEntryObject();
                            if (tf.TimetableChange.CurrentRoom != null)
                            {
                                timetableEntryNew.Room = tf.TimetableChange.CurrentRoom.Name;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstanceId != null)
                            {
                                timetableEntryNew.SubjectInstanceId = (int)tf.TimetableChange.CurrentSubjectInstanceId;
                            }
                            if (tf.TimetableChange.CurrentSubjectInstance != null)
                            {

                                timetableEntryNew.SubjectInstanceName = tf.TimetableChange.CurrentSubjectInstance.SubjectType.Name;
                            }
                            if (tf.TimetableChange.CurrentTeacher != null)
                            {
                                timetableEntryNew.TeacherFirstName = tf.TimetableChange.CurrentTeacher.FirstName;
                                timetableEntryNew.TeacherLastName = tf.TimetableChange.CurrentTeacher.LastName;
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
                            TimetableEntryNew = timetableEntryNew
                        });
                    }
                }
                timetableWeeks.Add(new TimetableWeekObject()
                {
                    Week = week,
                    TimeFrames = timeFrameObjects
                });

            }
            return timetableWeeks;
        }

    }

    public class TimetableWeekObject
    {
        public int Week { get; set; }
        public IEnumerable<TimeFrameObject> TimeFrames { get; set; }
    }
    public class TimeFrameObject
    {
#nullable enable
        public int Id { set; get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimetableEntryObject TimetableEntry { get; set; }
        public TimetableEntryObject TimetableEntryNew { get; set; }
        //public LessonRecord? LessonRecord { get; set; } //Possible attachment

    }
    public class TimetableEntryObject
    {
        public string Room { get; set; }
        public int SubjectInstanceId { get; set; }
        public string SubjectInstanceName { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
    }
}
