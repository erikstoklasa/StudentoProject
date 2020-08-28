using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TimetableManager
    {
        private readonly TimeFrameService timeFrameService;
        private readonly TimetableRecordService timetableRecordService;
        private readonly SubjectService subjectService;
        private readonly RoomService roomService;

        public TimetableManager(TimeFrameService timeFrameService, TimetableRecordService timetableRecordService, SubjectService subjectService, RoomService roomService)
        {
            this.timeFrameService = timeFrameService;
            this.timetableRecordService = timetableRecordService;
            this.subjectService = subjectService;
            this.roomService = roomService;
        }
        public async Task<Timetable> GetTimetableForStudent(int studentId, int week = 0)
        {
            List<TimeFrame> timeFrames = (await timeFrameService.GetAllTimeFrames()).OrderBy(tf => tf.Start.TimeOfDay).ToList();
            List<TimetableRecord> timetableRecords = await timetableRecordService.GetTimetableRecordsByStudentId(studentId);
            //Attaching timetableRecordProperties
            foreach (var tf in timeFrames)
            {
                var tr = timetableRecords.FirstOrDefault(tr => tr.TimeFrameId == tf.Id);
                if (tr != null && (week - tr.RecurrenceStart) >= 0 && (week - tr.RecurrenceStart) % tr.Recurrence == 0)
                {
                    tf.TimetableRecord = tr;
                    if (tf.TimetableRecord.SubjectInstanceId != null)
                    {
                        tf.TimetableRecord.SubjectInstance = await subjectService.GetSubjectInstanceAsync((int)tf.TimetableRecord.SubjectInstanceId);
                    }
                    if (tf.TimetableRecord.RoomId != null)
                    {
                        tf.TimetableRecord.Room = await roomService.GetRoomById((int)tf.TimetableRecord.RoomId);
                    }
                }
            }
            return new Timetable() { TimeFrames = timeFrames, UserId = studentId, Week = week };
        }
    }
    public class Timetable
    {
        public int Week { get; set; }
        public int UserId { get; set; } //Either StudentId or TeacherId
        public List<TimeFrame> TimeFrames { get; set; }
        public List<TimetableRecord> TimetableRecords { get; set; }
        //public List<LessonRecord> LessonRecords { get; set; }
    }
}
