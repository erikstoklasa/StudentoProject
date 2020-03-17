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
        public int getTeachersCount()
        {
            return Context.Teachers.Count();
        }
        //Teacher
        public int getStudentsCount()
        {
            return Context.Students.Count();
        }
        public int getStudentsCountInSubject(int subjectId)
        {
            return Context.Enrollments
                .Where(s => s.SubjectId == subjectId)
                .Count();
        }
        public double getStudentsToTeachersRatio(int decimalPlaces = 2)
        {
            double output = Math.Round(getStudentsCount() / (double)getTeachersCount(), decimalPlaces);
            output = output == Double.NaN ? 0 : output;
            return output;
        }
        public async Task<int> getStudentsCountByTeacherId(string userId)
        {
            int countOfStudents = await Context.Enrollments
                .Where(s => s.Subject.Teacher.UserAuthId == userId)
                .CountAsync();
            return countOfStudents;
        }
        public async Task<int> getSubjectsCountByTeacherId(string userId)
        {
            int countOfSubjects = await Context.Subjects
                .Where(s => s.Teacher.UserAuthId == userId)
                .CountAsync();
            return countOfSubjects;
        }
        //Student
        public double getTotalAverage(string userId, int decimalPlaces = 2)
        {
            var enrollments = Context.Enrollments
                .Include(s => s.Subject)
                .Where(s => s.Student.UserAuthId == userId)
                .ToArray();
            double totalASumOfAverages = 0.0;
            int countOfSubjectsWithGrades = 0;
            //Iterating only through student's subjects
            for (int i = 0; i < enrollments.Length; i++)
            {
                //Getting averages of every subject that the student has
                double currentlyAdded = getSubjectAverageForStudent(userId, enrollments[i].Subject.Id, decimalPlaces);
                if(currentlyAdded.CompareTo(Double.NaN) != 0) // getSubjectAverageForStudent returns 0 if student has no grades or is not enrolled in the subject
                {
                    countOfSubjectsWithGrades++;
                    totalASumOfAverages += currentlyAdded;
                }
                
            }
            //Averaging subject averages (totalASumOfAverages / subjects count)
            return Math.Round(totalASumOfAverages / countOfSubjectsWithGrades, decimalPlaces);
        }
        public Subject[] getAllSubjectsByUserId(string userId)
        {
            //Accessing Subjects via Enrollments table => Subject
            var enrollments = Context.Enrollments
                .Include(s => s.Subject)
                .Include(s => s.Subject.Teacher)
                .Where(s => s.Student.UserAuthId == userId)
                .OrderBy(s => s.Subject.Name)
                .ToArray();
            Subject[] output = new Subject[enrollments.Length];
            for (int i = 0; i < enrollments.Length; i++)
            {
                output[i] = enrollments[i].Subject;
            }
            return output;
        }
        public double getSubjectAverageForStudent(string userId, int subjectId, int maxGradeDayAge = 0, int minGradeDayAge = 0, int decimalPlaces = 2)
        {
            //Accessing all grades in a subject via Enrollments table => Subject => Grades
            var enrollment = Context.Enrollments
                .Where(s => s.Student.UserAuthId == userId && s.SubjectId == subjectId)
                .Include(s => s.Subject)
                    .ThenInclude(s => s.Grades)
                        .ThenInclude(s => s.Student)
                .FirstOrDefault();
            if (enrollment == null) //Student is not in the given subject
            {
                return Double.NaN;
            }
            List<Grade> filtredGrades = new List<Grade>();
            foreach (Grade g in enrollment.Subject.Grades)
            {
                if(g.Student.UserAuthId == userId && 
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
                sum += (double) g.Value;
            }
            return Math.Round(sum / count, decimalPlaces);
        }

        public Grade[] getRecentGradesByUserId(string userId, int maxNumberOfGrades = 5)
        {
            var grades = Context.Grades
                .Include(s => s.Subject)
                .Include(s => s.Subject.Teacher)
                .Where(s => s.Student.UserAuthId == userId)
                .OrderByDescending(s => s.Added)
                .ToArray();
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

        public Grade[] getSubjectGradesByUserId(string userId, int subjectId)
        {
            var grades = Context.Grades
                .Where(s => s.Student.UserAuthId == userId)
                .Where(s => s.Subject.Id == subjectId)
                .OrderByDescending(s => s.Added)
                .ToArray();

            return grades;
        }

        public Grade[] getAllGradesByUserId(string userId)
        {
            //Accessing Subjects via Enrollments table => Subject
            var grades = Context.Grades
                .Include(s => s.Subject)
                .Include(s => s.Subject.Teacher)
                .Where(s => s.Student.UserAuthId == userId)
                .OrderByDescending(s => s.Added)
                .ToArray();
            return grades;
        }
    }
}
