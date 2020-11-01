using Microsoft.Extensions.Logging;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
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
        private readonly ILogger<TimetableManager> logger;

        public TimetableManager(TimeFrameService timeFrameService,
                                TimetableRecordService timetableRecordService,
                                SubjectService subjectService,
                                RoomService roomService,
                                LessonRecordService lessonRecordService,
                                TimetableChangeService timetableChangeService,
                                ILogger<TimetableManager> logger)
        {
            this.timeFrameService = timeFrameService;
            this.timetableRecordService = timetableRecordService;
            this.subjectService = subjectService;
            this.roomService = roomService;
            this.lessonRecordService = lessonRecordService;
            this.timetableChangeService = timetableChangeService;
            this.logger = logger;
        }
        public async Task<Timetable> GetTimetableForStudent(int studentId, int week = 0)
        {
            var timer = new Stopwatch();
            timer.Start();
            List<TimeFrame> timeFrames = (await timeFrameService.GetAllTimeFrames()).OrderBy(tf => tf.Start.TimeOfDay).ToList();
            List<TimetableRecord> timetableRecords = await timetableRecordService.GetTimetableRecordsByStudentId(studentId);
            List<LessonRecord> lessonRecords = await lessonRecordService.GetLessonRecordsByStudentAndWeek(studentId, week);
            List<TimetableChange> timetableChanges = (await timetableChangeService.GetAllTimetableChangesByStudent(studentId, week)).ToList();
            timer.Stop();
            logger.LogDebug($"First timer: {timer.ElapsedMilliseconds}");
            //Attaching timetableRecordProperties
            timer.Reset();
            timer.Start();
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
                }
                if (timetableChanges.Count() > 0)
                {
                    tf.TimetableChange = timetableChanges.Where(tch => tch.TimeFrameId == tf.Id).FirstOrDefault();
                }
            }
            timer.Stop();
            logger.LogDebug($"Second timer: {timer.ElapsedMilliseconds}");
            return new Timetable() { TimeFrames = timeFrames, UserId = studentId, Week = week };
        }
        public async Task<Timetable> GetTimetableForTeacher(int teacherId, int week = 0)
        {
            List<TimeFrame> timeFrames = (await timeFrameService.GetAllTimeFrames()).OrderBy(tf => tf.Start.TimeOfDay).ToList();
            List<TimetableRecord> timetableRecords = (await timetableRecordService.GetTimetableRecordsByTeacher(teacherId)).ToList();
            List<LessonRecord> lessonRecords = await lessonRecordService.GetLessonRecordsByTeacherAndWeek(teacherId, week);
            List<TimetableChange> timetableChanges = (await timetableChangeService.GetAllTimetableChangesByTeacher(teacherId, week)).ToList();
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
                }
                if (timetableChanges.Count() > 0)
                {
                    tf.TimetableChange = timetableChanges.Where(tch => tch.TimeFrameId == tf.Id).FirstOrDefault();
                }
            }
            return new Timetable() { TimeFrames = timeFrames, UserId = teacherId, Week = week };
        }
        public async Task<List<LessonRecord>> GetLessonRecordsNeededToBeCompleted(int teacherId, DateTime today)
        {
            int thisWeek = today.Date.DayOfYear / 7;
            List<TimetableRecord> timetableRecords = (await timetableRecordService.GetTimetableRecordsByTeacher(teacherId)).ToList();
            List<TimetableChange> timetableChanges =
                await timetableChangeService.GetAllTimetableChangesByTeacherUntilThisWeek(teacherId, thisWeek);
            List<LessonRecord> completedLessonRecords = await lessonRecordService.GetAllLessonRecordsByTeacher(teacherId);
            List<LessonRecord> lessonRecordsToBeCompleted = new List<LessonRecord>();
            //Go through each timetable record and timetable change and check if there is a lesson record for that
            foreach (var tr in timetableRecords)
            {
                for (int currWeek = 0; currWeek < thisWeek; currWeek++)
                {
                    if (tr != null && (currWeek - tr.RecurrenceStart) >= 0 && (currWeek - tr.RecurrenceStart) % tr.Recurrence == 0)
                    {
                        IEnumerable<TimetableChange> thisWeeksChanges = timetableChanges.Where(tch => tch.Week == currWeek);
                        for (int i = 0; i < thisWeeksChanges.Count(); i++)
                        {
                            //Implement timetableChanges to alter the output
                        }
                        lessonRecordsToBeCompleted.Add(new LessonRecord() { SubjectInstanceId = tr.SubjectInstanceId, Week = currWeek, TimeFrameId = tr.TimeFrameId });
                    }
                }
            }
            foreach (var i in completedLessonRecords)
            {
                lessonRecordsToBeCompleted
                    .RemoveAll(lr => lr.SubjectInstanceId == i.SubjectInstanceId && lr.Week == i.Week && lr.TimeFrameId == i.TimeFrameId);
            }
            return lessonRecordsToBeCompleted;
        }
    }
    public class Timetable
    {
        public int Week { get; set; }
        public int UserId { get; set; } //Either StudentId or TeacherId
        public List<TimeFrame> TimeFrames { get; set; }
    }
}
