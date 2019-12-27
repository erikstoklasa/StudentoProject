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
        public int getTeachersCount()
        {
            return Context.Teachers.Count();
        }
        public int getStudentsCount()
        {
            return Context.Students.Count();
        }
        public double getStudentsToTeachersRatio(int decimalPlaces)
        {
            return Math.Round(getStudentsCount() / (double) getTeachersCount(), decimalPlaces);
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
    }
}
