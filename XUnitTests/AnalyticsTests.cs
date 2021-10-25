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

            foreach (var g in testGrades)
            {
                Grade grade = new Grade();
                grade.SetGradeValue(g);
                grades.Add(grade);
            }
            

            // ACT

            // ASSERT

            Assert.Equal(3, Analytics.GetSubjectAverageForStudentAsync(grades.ToArray()));
        }
    }
}
