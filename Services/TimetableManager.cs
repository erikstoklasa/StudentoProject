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
        private readonly LessonRecordService lessonRecordService;
        private readonly TimetableChangeService timetableChangeService;

        public TimetableManager(TimeFrameService timeFrameService, TimetableRecordService timetableRecordService, SubjectService subjectService, RoomService roomService, LessonRecordService lessonRecordService, TimetableChangeService timetableChangeService)
        {
            this.timeFrameService = timeFrameService;
            this.timetableRecordService = timetableRecordService;
            this.subjectService = subjectService;
            this.roomService = roomService;
            this.lessonRecordService = lessonRecordService;
            this.timetableChangeService = timetableChangeService;
        }
        public async Task<Timetable> GetTimetableForStudent(int studentId, int week = 0)
        {
            List<TimeFrame> timeFrames = (await timeFrameService.GetAllTimeFrames()).OrderBy(tf => tf.Start.TimeOfDay).ToList();
            List<TimetableRecord> timetableRecords = await timetableRecordService.GetTimetableRecordsByStudentId(studentId);
            List<LessonRecord> lessonRecords = await lessonRecordService.GetLessonRecordsByStudentAndWeek(studentId, week);
            List<TimetableChange> timetableChanges = await timetableChangeService.GetAllTimetableChangesByStudent(studentId, week);
            //Attaching timetableRecordProperties
            foreach (var tf in timeFrames)
            {
                var tr = timetableRecords.FirstOrDefault(tr => tr.TimeFrameId == tf.Id);
                if (tr != null && (week - tr.RecurrenceStart) >= 0 && (week - tr.RecurrenceStart) % tr.Recurrence == 0)
                {
                    tf.TimetableRecord = tr;
                    var lessonRecord = lessonRecords.FirstOrDefault(lr => lr.TimeFrameId == tf.Id);
                    if (lessonRecord != null)
                    {
                        tf.LessonRecord = lessonRecord;
                    }
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
        public async Task<Timetable> GetTimetableForTeacher(int teacherId, int week = 0)
        {
            List<TimeFrame> timeFrames = (await timeFrameService.GetAllTimeFrames()).OrderBy(tf => tf.Start.TimeOfDay).ToList();
            List<TimetableRecord> timetableRecords = (await timetableRecordService.GetTimetableRecordsByTeacher(teacherId)).ToList();
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
            return new Timetable() { TimeFrames = timeFrames, UserId = teacherId, Week = week };
        }
    }
    public class Timetable
    {
        public int Week { get; set; }
        public int UserId { get; set; } //Either StudentId or TeacherId
        public List<TimeFrame> TimeFrames { get; set; }
    }
}
