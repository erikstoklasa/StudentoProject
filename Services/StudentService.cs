using EllipticCurve.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class StudentService
    {
        private readonly SchoolContext context;

        public StudentService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<int> GetStudentId(string userAuthId)
        {
            Student student = await context.Students
                .Where(t => t.UserAuthId == userAuthId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (student == null) //Has role student but no assigned userAuthId - can happen after db resets
            {
                return -1;
            }
            return student.Id;
        }
        public async Task<bool> AddStudentAsync(Student student)
        {
            if (!HasRequiredFields(student))
            {
                return false;
            }
            if (!ValidationUtils.PersonalIdentifNumberIsValid(student.PersonalIdentifNumber))
            {
                return false;
            }
            try
            {
                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<Student> GetStudentAsync(int studentId)
        {
            Student student = await context.Students
                .Where(s => s.Id == studentId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return student;
        }
        public async Task<Student> GetStudentFullProfileAsync(int studentId)
        {
            Student student = await context.Students
                .Where(s => s.Id == studentId)
                .Include(s => s.GroupEnrollments)
                    .ThenInclude(g => g.Enrollments)
                        .ThenInclude(subj => subj.SubjectInstance)
                            .ThenInclude(subj => subj.Teacher)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return student;
        }
        public async Task<Student[]> GetAllStudentsAsync()
        {
            Student[] students = await context.Students
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ToArrayAsync();
            return students;
        }
        public async Task<Student[]> GetAllStudentsBySubjectInstanceAsync(int subjectId)
        {
            Enrollment[] enrollments = await context.Enrollments
                .Where(s => s.SubjectInstance.Id == subjectId)
                .Include(e => e.StudentGroup)
                    .ThenInclude(g => g.Students)
                .AsNoTracking()
                .ToArrayAsync();
            List<Student> students = new List<Student>();
            foreach(Enrollment enrollment in enrollments)
            {
                foreach(Student student in enrollment.StudentGroup.Students)
                {
                    students.Add(student);
                }
            }
            
            return students.OrderBy(s => s.LastName).ToArray();
        }
        public async Task<List<Student>> GetAllStudentsByClassAsync(int classId)
        {
            List<Student> students = await context.Students
                .Where(s => s.ClassId == classId)
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ToListAsync();
            return students;
        }
        public async Task<List<Student>> GetAllStudentsByTeacherAsync(int teacherId)
        {
            List<Enrollment> enrollments = await context.Enrollments
                .Where(s => s.SubjectInstance.TeacherId == teacherId)
                .Include(s => s.StudentGroup)
                    .ThenInclude(g => g.Students)
                .AsNoTracking()
                .ToListAsync();
            List<Student> students = new List<Student>();
            foreach (Enrollment e in enrollments)
            {
                foreach (Student s in e.StudentGroup.Students)
                {
                    if (!students.Where(stud => stud.Id == s.Id).Any())
                    {
                        students.Add(s);
                    }
                }
            }
            return students.OrderBy(s => s.LastName).ToList();
        }
        public async Task<int> GetStudentCountAsync()
        {
            return (await GetAllStudentsAsync()).Length;
        }
        public async Task<int> GetStudentCountBySubjectAsync(int subjectId)
        {
            return await context.Enrollments
                .Where(s => s.SubjectInstanceId == subjectId)
                .AsNoTracking()
                .CountAsync();
        }
        public async Task<int> GetStudentCountByTeacherAsync(int teacherId)
        {
            int studentCount = await context.Enrollments
                .Where(s => s.SubjectInstance.Teacher.Id == teacherId)
                .CountAsync();
            return studentCount;
        }
        public async Task<bool> UpdateStudentAsync(Student student)
        {
            if (!HasRequiredFields(student))
            {
                return false;
            }
            if (!ValidationUtils.PersonalIdentifNumberIsValid(student.PersonalIdentifNumber))
            {
                return false;
            }
            context.Attach(student).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> AddStudentToClassAsync(int studentId, int classId)
        {
            Student student = await GetStudentAsync(studentId);
            student.ClassId = classId;
            context.Attach(student).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        //VALIDATIONS
        public static bool HasRequiredFields(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(student.PersonalIdentifNumber))
            {
                return false;
            }
            if (!student.Birthdate.HasValue)
            {
                return false;
            }
            return true;
        }
    }
}
