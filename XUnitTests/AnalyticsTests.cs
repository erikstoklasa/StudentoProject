using System;
using SchoolGradebook.Services;
using SchoolGradebook.Models;
using Xunit;
using System.Collections.Generic;

namespace XUnitTests
{
    public class UnitTest1
    {

        [Fact]
        public void GetSubjectAverageForStudentAsyncTest()
        {
            // ARRANGE  
            List<Grade> grades = new List<Grade>();

            string[] testGrades = new string[] {"4","5","1","2"};

            // ACT

            foreach (var g in testGrades)
            {
                Grade grade = new Grade();
                grade.SetGradeValue(g);
                grade.Weight = 5;
                grades.Add(grade);
            }

            // ASSERT

            Assert.Equal(3, AnalyticsService.GetSubjectAverageForStudentAsync(grades.ToArray()),2);
        }

        [Fact]
        public void GetSubjectAverageForStudentAsyncTest2()
        {
            // ARRANGE  
            List<Grade> grades = new List<Grade>();

            string[] testGrades = new string[] { "1", "1", "2", "4", "5"};
            int[] testWeights = new int[] { 8, 3, 7, 6, 2};

            // ACT

            for (int i = 0; i < testGrades.Length; i++)
            {
                Grade grade = new Grade();
                grade.SetGradeValue(testGrades[i]);
                grade.Weight = testWeights[i];
                grades.Add(grade);
            }

            // ASSERT

            Assert.Equal(2.26923076923077, AnalyticsService.GetSubjectAverageForStudentAsync(grades.ToArray()), 2);
        }

        [Fact]
        public void GetSubjectAverageAsyncTest()
        {
            // ARRANGE
            List<Grade> grades = new List<Grade>();

            string[] testGrades = new string[] { "4", "5", "1", "2" };

            // ACT

            foreach (var g in testGrades)
            {
                Grade grade = new Grade();
                grade.SetGradeValue(g);
                grade.Weight = 5;
                grades.Add(grade);
            }

            // ASSERT

            Assert.Equal(3, AnalyticsService.GetSubjectAverageAsync(grades.ToArray()), 3);
        }

        [Fact]
        public void GetSubjectAverageAsyncTest2()
        {
            // ARRANGE
            List<Grade> grades = new List<Grade>();

            string[] testGrades = new string[] { "1", "1", "2", "4", "5" };
            int[] testWeights = new int[] { 8, 3, 7, 6, 2 };

            // ACT

            for (int i = 0; i < testGrades.Length; i++)
            {
                Grade grade = new Grade();
                grade.SetGradeValue(testGrades[i]);
                grade.Weight = testWeights[i];
                grades.Add(grade);
            }

            // ASSERT

            Assert.Equal(2.26923076923077, AnalyticsService.GetSubjectAverageAsync(grades.ToArray()), 3);
        }
    }
}
