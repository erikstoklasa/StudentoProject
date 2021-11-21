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
    public class AnalyticsService
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly GradeService gradeService;
        public AnalyticsService(SubjectService subjectService, StudentService studentService, GradeService gradeService)
        {
            this.subjectService = subjectService;
            this.studentService = studentService;
            this.gradeService = gradeService;
        }
        
        /// <summary>
        /// Gets the subject average for specified student and subject instance
        /// </summary>
        /// <param name="grades"></param>
        /// <returns>Subject average in 2 precision points (eg. 1.48)</returns>
        public static double GetSubjectAverageForStudentAsync(Grade[] grades)
        {
            int sum = 0;

            int weightSum = 0;
            foreach (Grade g in grades)
            {
                weightSum += g.GetWeight(); 
                sum += g.GetInternalGradeValue() * g.GetWeight();
            }
            if (weightSum == 0) //Student doesn't have any grades in the given subject
            {
                return Double.NaN;
            }
            float internalAverage = (float)sum / weightSum;
            return Grade.MapInnerValueToDecimalValue(internalAverage);
        }

        /// <summary>
        /// Gets the subject average in decimal (eg. 1.481)
        /// </summary>
        /// <param name="grades"></param>
        /// <returns>Subject average decimal number for 3 precision points</returns>
        public static double GetSubjectAverageAsync(Grade[] grades)
        {
            double sum = 0.0;

            int weightSum = 0;
            foreach (Grade g in grades)
            {
                weightSum += g.GetWeight();
                sum += g.GetGradeValueInDecimal() * g.GetWeight();
            }
            if (weightSum == 0) //Students has no grades for specified subject
            {
                return Double.NaN;
            }
            return Math.Round(sum / weightSum, 3);
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
                if (!double.IsNaN(currentSubjectAvg)) // getSubjectAverageForStudent returns 0 if student has no grades or is not enrolled in the subject
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
            List<Grade> filtredGrades = new();
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
