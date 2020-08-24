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
        private readonly LessonRecordService lessonRecordService;

        public TimetableManager(TimeFrameService timeFrameService, LessonRecordService lessonRecordService)
        {
            this.timeFrameService = timeFrameService;
            this.lessonRecordService = lessonRecordService;
        }
        public async Task<Timetable> GetTimetableForStudent(int studentId, int week)
        {
            List<TimeFrame> timeFrames = await timeFrameService.GetTimeFramesByStudentId(studentId);
            timeFrames.AddRange(await timeFrameService.GetTimeFrames(tf => tf.SubjectInstanceId == null));
            return new Timetable() { TimeFrames = timeFrames, UserId = studentId, Week = week };
        }
    }
    public class Timetable
    {
        public int Week { get; set; }
        public int UserId { get; set; } //Either StudentId or TeacherId
        public List<TimeFrame> TimeFrames { get; set; }
        //public List<LessonRecord> LessonRecords { get; set; }
    }
}
