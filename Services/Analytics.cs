using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class Analytics
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly GradeService gradeService;

        private SchoolContext Context { get; set; }
        public Analytics(SchoolContext context, SubjectService subjectService, StudentService studentService, GradeService gradeService)
        {
            Context = context;
            this.subjectService = subjectService;
            this.studentService = studentService;
            this.gradeService = gradeService;
        }
        //Admin
        public async Task<int> GetTeachersCountAsync()
        {
            return await Context.Teachers.CountAsync();
        }
        public async Task<int> GetStudentsCountAsync()
        {
            return await Context.Students.CountAsync();
        }
        public async Task<double> GetStudentsToTeachersRatioAsync(int decimalPlaces = 2)
        {
            double output = Math.Round(await GetStudentsCountAsync() / (double)await GetTeachersCountAsync(), decimalPlaces);
            output = output == Double.NaN ? 0 : output;
            return output;
        }

        //Teacher
        public async Task<int> GetStudentsCountInSubjectAsync(int SubjectInstanceId)
        {
            return await Context.Enrollments
                .Where(s => s.SubjectInstanceId == SubjectInstanceId)
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<int> GetStudentsCountByTeacherUserAuthIdAsync(string teacherUserAuthId)
        {
            int countOfStudents = await Context.Enrollments
                .Where(s => s.SubjectInstance.Teacher.UserAuthId == teacherUserAuthId)
                .CountAsync();
            return countOfStudents;
        }
        public async Task<int> GetSubjectsCountByTeacherIdAsync(string userId)
        {
            int countOfSubjects = await Context.SubjectInstances
                .Where(s => s.Teacher.UserAuthId == userId)
                .CountAsync();
            return countOfSubjects;
        }
        public async Task<double> GetSubjectAverageForStudentByStudentIdAsync(int studentId, int SubjectInstanceId)
        {
            double sum = 0.0;
            var grades = (await gradeService.GetAllGradesByStudentSubjectInstance(studentId, SubjectInstanceId)).Where(g => g.StudentId == studentId);

            int count = grades.Count();
            if (count == 0) //Student doesn't have any grades in the given subject
            {
                return Double.NaN;
            }
            foreach (Grade g in grades)
            {
                sum += (double)g.Value;
            }
            return Math.Round(sum / count, 2);
        }
        //Student
        public async Task<double> GetTotalAverageAsync(
            string userId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2)
        {
            Student student = Context.Students.Where(s => s.UserAuthId == userId).FirstOrDefault();

            List<SubjectInstance> subjectInstances = await subjectService.GetAllSubjectInstancesByStudentAsync(student.Id);

            double totalASumOfAverages = 0.0;
            int countOfSubjectsWithGrades = 0;
            double currentSubjectAvg = 0;
            //Iterating only through student's subject instances
            foreach (SubjectInstance subjectInstance in subjectInstances)
            {
                //Getting averages of every subject that the student has
                currentSubjectAvg = await GetSubjectAverageForStudentAsync(userId, subjectInstance.Id, maxGradeDayAge, minGradeDayAge, 5);
                if (currentSubjectAvg.CompareTo(Double.NaN) != 0) // getSubjectAverageForStudent returns 0 if student has no grades or is not enrolled in the subject
                {
                    countOfSubjectsWithGrades++;
                    totalASumOfAverages += currentSubjectAvg;
                }

            }
            //Averaging subject averages (totalASumOfAverages / subjects count)
            return Math.Round(totalASumOfAverages / countOfSubjectsWithGrades, decimalPlaces);
        }
        public async Task<double> GetSubjectAverageForStudentAsync(
            string userId,
            int SubjectInstanceId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2)
        {

            var enrollment = await Context.Enrollments
                .Where(
                    e => e.StudentGroup.StudentGroupEnrollments.Where(e => e.Student.UserAuthId == userId).Any()
                    && e.SubjectInstanceId == SubjectInstanceId
                    )
                .Include(s => s.SubjectInstance)
                    .ThenInclude(s => s.Grades)
                        .ThenInclude(s => s.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (enrollment == null) //Student is not in the given subject
            {
                return Double.NaN;
            }
            List<Grade> filtredGrades = new List<Grade>();
            foreach (Grade g in enrollment.SubjectInstance.Grades)
            {
                if (g.Student.UserAuthId == userId &&
                    (maxGradeDayAge == 0 || maxGradeDayAge >= DateTime.Now.Subtract(g.Added).TotalDays) && //if maxGradeDayAge is set, then add only grades younger than maxGradeDayAge
                    (minGradeDayAge == 0 || minGradeDayAge <= DateTime.Now.Subtract(g.Added).TotalDays) //if minGradeDayAge is set, then add only grades older than minGradeDayAge
                    )
                {
                    filtredGrades.Add(g);
                }
            }
            double sum = 0.0;
            int count = filtredGrades.Count;
            if (filtredGrades.Count == 0) //Student doesn't have any grades in the given subject
            {
                return Double.NaN;
            }
            foreach (Grade g in filtredGrades)
            {
                sum += (double)g.Value;
            }
            return Math.Round(sum / count, decimalPlaces);
        }
    }
}
