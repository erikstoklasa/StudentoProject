using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class Analytics
    {
        private SchoolContext Context { get; set; }
        public Analytics(SchoolContext context)
        {
            Context = context;
        }
        //Admin
        public async Task<int> getTeachersCountAsync()
        {
            return await Context.Teachers.CountAsync();
        }
        public async Task<int> getStudentsCountAsync()
        {
            return await Context.Students.CountAsync();
        }
        public async Task<double> getStudentsToTeachersRatioAsync(int decimalPlaces = 2)
        {
            double output = Math.Round(await getStudentsCountAsync() / (double)await getTeachersCountAsync(), decimalPlaces);
            output = output == Double.NaN ? 0 : output;
            return output;
        }
        //Teacher
        public async Task<Subject[]> getAllSubjectsByTeacherUserAuthAsync(string userId)
        {
            Subject[] subjects = await Context.Subjects
                .Where(s => s.Teacher.UserAuthId == userId)
                .OrderBy(s => s.Name)
                .AsNoTracking()
                .ToArrayAsync();
            return subjects;
        }
        public async Task<Subject> getSubjectAsync(int subjectId)
        {
            //Accessing Subjects via Enrollments table => Subject
            Subject subject = await Context.Subjects
                .Where(s => s.Id == subjectId)
                .Include(s => s.Teacher)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subject;
        }
        public async Task<int> getStudentsCountInSubjectAsync(int subjectId)
        {
            return await Context.Enrollments
                .Where(s => s.SubjectId == subjectId)
                .CountAsync();
        }

        public async Task<int> getStudentsCountByTeacherUserAuthIdAsync(string teacherUserAuthId)
        {
            int countOfStudents = await Context.Enrollments
                .Where(s => s.Subject.Teacher.UserAuthId == teacherUserAuthId)
                .CountAsync();
            return countOfStudents;
        }
        public async Task<int> getSubjectsCountByTeacherIdAsync(string userId)
        {
            int countOfSubjects = await Context.Subjects
                .Where(s => s.Teacher.UserAuthId == userId)
                .CountAsync();
            return countOfSubjects;
        }
        public async Task<Student[]> getAllStudentsBySubjectIdAsync(int Id)
        {
            //Accessing Students via Enrollments table => Students
            var enrollments = await Context.Enrollments
                .Include(s => s.Student)
                .Where(s => s.Subject.Id == Id)
                .OrderBy(s => s.Subject.Name)
                .ToArrayAsync();
            Student[] output = new Student[enrollments.Length];
            for (int i = 0; i < enrollments.Length; i++)
            {
                output[i] = enrollments[i].Student;
            }
            return output;
        }
        public async Task<double> getSubjectAverageForStudentByStudentIdAsync(int studentId, int subjectId)
        {
            var enrollment = await Context.Enrollments
                .Where(e => e.StudentId == studentId && e.SubjectId == subjectId)
                .Include(e => e.Subject)
                    .ThenInclude(e => e.Grades)
                .FirstOrDefaultAsync();
            if (enrollment == null) //Student is not in the given subject
            {
                return Double.NaN;
            }

            double sum = 0.0;
            var grades = enrollment.Subject.Grades.Where(g => g.StudentId == studentId);
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
        public async Task<double> getTotalAverageAsync(
            string userId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2)
        {
            var enrollments = await Context.Enrollments
                .Include(s => s.Subject)
                .Where(s => s.Student.UserAuthId == userId)
                .ToArrayAsync();
            double totalASumOfAverages = 0.0;
            int countOfSubjectsWithGrades = 0;
            double currentSubjectAvg = 0;
            //Iterating only through student's subjects
            for (int i = 0; i < enrollments.Length; i++)
            {
                //Getting averages of every subject that the student has
                currentSubjectAvg = await getSubjectAverageForStudentAsync(userId, enrollments[i].Subject.Id, maxGradeDayAge, minGradeDayAge, 5);
                if (currentSubjectAvg.CompareTo(Double.NaN) != 0) // getSubjectAverageForStudent returns 0 if student has no grades or is not enrolled in the subject
                {
                    countOfSubjectsWithGrades++;
                    totalASumOfAverages += currentSubjectAvg;
                }

            }
            //Averaging subject averages (totalASumOfAverages / subjects count)
            return Math.Round(totalASumOfAverages / countOfSubjectsWithGrades, decimalPlaces);
        }
        public async Task<Subject[]> getAllSubjectsByStudentUserAuthAsync(string userId)
        {
            //Accessing Subjects via Enrollments table => Subject
            var enrollments = await Context.Enrollments
                .Include(s => s.Subject)
                .Include(s => s.Subject.Teacher)
                .Where(s => s.Student.UserAuthId == userId)
                .OrderBy(s => s.Subject.Name)
                .ToArrayAsync();
            Subject[] output = new Subject[enrollments.Length];
            for (int i = 0; i < enrollments.Length; i++)
            {
                output[i] = enrollments[i].Subject;
            }
            return output;
        }
        public async Task<double> getSubjectAverageForStudentAsync(
            string userId,
            int subjectId,
            int maxGradeDayAge = 0,
            int minGradeDayAge = 0,
            int decimalPlaces = 2)
        {
            //Accessing all grades in a subject via Enrollments table => Subject => Grades
            var enrollment = await Context.Enrollments
                .Where(s => s.Student.UserAuthId == userId && s.SubjectId == subjectId)
                .Include(s => s.Subject)
                    .ThenInclude(s => s.Grades)
                        .ThenInclude(s => s.Student)
                .FirstOrDefaultAsync();
            if (enrollment == null) //Student is not in the given subject
            {
                return Double.NaN;
            }
            List<Grade> filtredGrades = new List<Grade>();
            foreach (Grade g in enrollment.Subject.Grades)
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

        public async Task<Grade[]> getRecentGradesByUserIdAsync(string userId, int maxNumberOfGrades = 5)
        {
            var grades = await getGradesAsync(userId);
            if (maxNumberOfGrades > grades.Length) //If less than 5 grades are in the database for a specific student
            {
                maxNumberOfGrades = grades.Length;
            }
            Grade[] output = new Grade[maxNumberOfGrades];
            for (int i = 0; i < maxNumberOfGrades; i++)
            {
                output[i] = grades[i];
            }
            return output;
        }
        public async Task<Grade> getGradeAsync(string userId, int gradeId)
        {
            Grade grade = await Context.Grades
                .Where(g => g.Student.UserAuthId == userId && g.Id == gradeId)
                .Include(g => g.Subject)
                .ThenInclude(g => g.Teacher)
                .FirstOrDefaultAsync();

            return grade;
        }
        public async Task<Grade[]> getGradesAsync(string userId, int subjectId)
        {
            Grade[] grades = await Context.Grades
                .Where(s => s.Student.UserAuthId == userId)
                .Where(s => s.Subject.Id == subjectId)
                .OrderByDescending(s => s.Added)
                .ToArrayAsync();

            return grades;
        }

        public async Task<Grade[]> getGradesAsync(string userId)
        {
            //Accessing Subjects via Enrollments table => Subject
            var grades = await Context.Grades
                .Include(s => s.Subject)
                .Include(s => s.Subject.Teacher)
                .Where(s => s.Student.UserAuthId == userId)
                .OrderByDescending(s => s.Added)
                .ToArrayAsync();
            return grades;
        }
    }
}
