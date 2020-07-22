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
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.SubjectInstance)
                        .ThenInclude(subj => subj.SubjectType)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.SubjectInstance)
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
        public async Task<Student[]> GetAllStudentsBySubjectAsync(int subjectId)
        {
            Enrollment[] enrollments = await context.Enrollments
                .Include(s => s.Student)
                .Where(s => s.SubjectInstance.Id == subjectId)
                .AsNoTracking()
                .ToArrayAsync();
            Student[] students = new Student[enrollments.Length];
            for (int i = 0; i < enrollments.Length; i++)
            {
                students[i] = enrollments[i].Student;
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
                .Include(s => s.Student)
                .Where(s => s.SubjectInstance.TeacherId == teacherId)
                .AsNoTracking()
                .ToListAsync();
            List<Student> students = new List<Student>();
            foreach (Enrollment e in enrollments)
            {
                Student s = e.Student;
                s.Enrollments = null;
                if (!students.Where(stud => stud.Id == s.Id).Any())
                {
                    students.Add(s);
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
