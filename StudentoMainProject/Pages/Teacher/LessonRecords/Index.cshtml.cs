using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.LessonRecords
{
    public class IndexModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly LessonRecordService lessonRecordService;
        private readonly StudentGroupService studentGroupService;
        private readonly StudentService studentService;
        private readonly TimeFrameService timeFrameService;
        private readonly AttendanceService attendanceService;

        public string ErrorMessage { get; set; }
        public List<Models.Student> Students { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        public LessonRecord LessonRecord { get; set; }
        public IndexModel(SubjectService subjectService,
                          LessonRecordService lessonRecordService,
                          StudentGroupService studentGroupService,
                          StudentService studentService,
                          TimeFrameService timeFrameService,
                          AttendanceService attendanceService)
        {
            this.subjectService = subjectService;
            this.lessonRecordService = lessonRecordService;
            this.studentGroupService = studentGroupService;
            this.studentService = studentService;
            this.timeFrameService = timeFrameService;
            this.attendanceService = attendanceService;
        }
        [BindProperty(SupportsGet = true)]
        public int? Week { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? TimeframeId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SubjectInstanceId { get; set; }
        public string LessonDescription { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (SubjectInstanceId != null && Week != null && TimeframeId != null)
            {
                LessonRecord = await lessonRecordService.GetLessonRecord((int)Week, (int)SubjectInstanceId, (int)TimeframeId);
                if (LessonRecord == null)
                {
                    LessonRecord = new LessonRecord()
                    {
                        SubjectInstance = await subjectService.GetSubjectInstanceAsync((int)SubjectInstanceId),
                        TimeFrame = await timeFrameService.GetTimeFrameById((int)TimeframeId)
                    };
                }
                else
                {
                    LessonRecord.Attendance = (await attendanceService.GetAttendanceRecordsByLessonRecordId(LessonRecord.Id)).ToList();
                }

                StudentGroups = await studentGroupService.GetAllGroupsBySubjectInstanceAsync((int)SubjectInstanceId);
                Students = (await studentService.GetAllStudentsBySubjectInstanceAsync((int)SubjectInstanceId)).OrderBy(s => s.LastName).ToList();
                return Page();
            }
            else
            {
                return BadRequest();
            }
        }
        public class LessonRec
        {
            public string LessonDescription { get; set; }
            public int[] AbsentStudents { get; set; }
            public int? Week { get; set; }
            public int? TimeframeId { get; set; }
            public int? SubjectInstanceId { get; set; }
        }
        public async Task<ActionResult> OnPostAsync([FromBody] LessonRec lr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (lr.SubjectInstanceId == null || lr.Week == null || lr.TimeframeId == null)
            {
                return BadRequest();
            }
            LessonRecord = await lessonRecordService.GetLessonRecord((int)lr.Week, (int)lr.SubjectInstanceId, (int)lr.TimeframeId);
            if (LessonRecord == null) //Creating a new lessonRecord
            {
                LessonRecord = new LessonRecord()
                {
                    SubjectInstanceId = (int)lr.SubjectInstanceId,
                    TimeFrameId = (int)lr.TimeframeId,
                    Week = (int)lr.Week,
                    Description = lr.LessonDescription
                };
                Students = (await studentService.GetAllStudentsBySubjectInstanceAsync((int)lr.SubjectInstanceId, onlyIds: true)).ToList();
                List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();
                foreach (int i in lr.AbsentStudents)
                {
                    if (!Students.Where(s => s.Id == i).Any())
                    {
                        return BadRequest();
                    }
                }
                await lessonRecordService.AddLessonRecord(LessonRecord);

                foreach (int i in lr.AbsentStudents)
                {
                    attendanceRecords.Add(new AttendanceRecord()
                    {
                        StudentId = i,
                        Reason = AttendanceRecord.REASON.Absent,
                        LessonRecordId = LessonRecord.Id
                    });
                }
                await attendanceService.RemoveOrCreateAttendenceRecords(attendanceRecords, LessonRecord.Id);
                return StatusCode(201);
            }
            else //Updating an existing lessonRecord
            {
                LessonRecord.Description = lr.LessonDescription;
                Students = (await studentService.GetAllStudentsBySubjectInstanceAsync((int)lr.SubjectInstanceId, onlyIds: true)).ToList();
                foreach (int i in lr.AbsentStudents)
                {
                    if (!Students.Where(s => s.Id == i).Any())
                    {
                        return BadRequest();
                    }
                }
                await lessonRecordService.UpdateLessonRecord(LessonRecord);
                List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();
                foreach (int i in lr.AbsentStudents)
                {
                    attendanceRecords.Add(new AttendanceRecord()
                    {
                        StudentId = i,
                        Reason = AttendanceRecord.REASON.Absent,
                        LessonRecordId = LessonRecord.Id
                    });
                }
                await attendanceService.RemoveOrCreateAttendenceRecords(attendanceRecords, LessonRecord.Id);
                return StatusCode(200);
            }
            //TODO: 2) Validate if teacher has access to this lessonrecord
            //TODO: 3) Validate if timeFrameId and subjectInstanceId and week are valid
        }
    }
}
