using System;
using Xunit;

namespace XUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            SchoolGradebook.API.Grades.GradesController gradesController = new SchoolGradebook.API.Grades.GradesController(null, null,null,null,null,null);
            gradesController.TeacherGetGrades();
        }
    }
}
