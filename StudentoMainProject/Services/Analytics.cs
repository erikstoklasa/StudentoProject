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
        /// <summary>
        /// Gets the subject average for specified student and subject instance
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="SubjectInstanceId"></param>
        /// <returns>Subject average in 2 precision points (eg. 1.48)</returns>
        public async Task<double> GetSubjectAverageForStudentAsync(int studentId, int SubjectInstanceId)
        {
            int sum = 0;
            Grade[] grades = await gradeService.GetAllGradesByStudentSubjectInstance(studentId, SubjectInstanceId);

            int count = grades.Length;
            if (count == 0) //Student doesn't have any grades in the given subject
            {
                return Double.NaN;
            }
            foreach (Grade g in grades)
            {
                sum += g.GetInternalGradeValue();
            }
            int internalAverage = (int)Math.Ceiling((float)sum / count);
            return Grade.MapInnerValueToDecimalValue(internalAverage);
        }

        /// <summary>
        /// Gets the subject average in decimal (eg. 1.481)
        /// </summary>
        /// <param name="subjectInstanceId"></param>
        /// <returns>Subject average decimal number for 3 precision points</returns>
        public async Task<double> GetSubjectAverageAsync(int subjectInstanceId)
        {
            double sum = 0.0;
            Grade[] grades = await Context.Grades.Where(g => g.SubjectInstanceId == subjectInstanceId).AsNoTracking().ToArrayAsync();

            int count = grades.Length;
            if (count == 0) //Students has no grades for specified subject
            {
                return Double.NaN;
            }
            foreach (Grade g in grades)
            {
                sum += g.GetGradeValueInDecimal();
            }
            return Math.Round(sum / count, 3);
        }
        //Student
        public async Task<double> GetTotalAverageForStudentAsync(
            int studentId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2,
            ICollection<SubjectInstance> subjectInstances = null)
        {
            if (subjectInstances == null)
            {
                subjectInstances = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            }

            double totalASumOfAverages = 0.0;
            int countOfSubjectsWithGrades = 0;
            //Iterating only through student's subject instances
            foreach (SubjectInstance subjectInstance in subjectInstances)
            {
                //Getting averages of every subject that the student has
                double currentSubjectAvg = await GetSubjectAverageForStudentAsync(studentId, subjectInstance.Id, maxGradeDayAge, minGradeDayAge, 5);
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
            int studentId,
            int subjectInstanceId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2)
        {
            Grade[] grades = await gradeService.GetAllGradesByStudentSubjectInstance(studentId, subjectInstanceId);
            List<Grade> filtredGrades = new List<Grade>();
            foreach (Grade g in grades)
            {
                if (g.StudentId == studentId &&
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
                sum += g.GetGradeValueInDecimal();
            }
            return Math.Round(sum / count, decimalPlaces);
        }
    }
}
