using EllipticCurve.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
                .Select(s => new Student { Id = s.Id })
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
        public async Task<Student> GetStudentBasicInfoAsync(int studentId)
        {
            //Basic info: First Name, Last Name, Id
            Student student = await context.Students
                .Where(s => s.Id == studentId)
                .Select(s => new Student
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    SchoolId = s.SchoolId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return student;
        }
        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            Student student = await context.Students
                .Where(s => s.Email == email)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return student;
        }
        public async Task<Student> GetStudentFullProfileAsync(int studentId)
        {
            Student student = await context.Students
                .Where(s => s.Id == studentId)
                .Include(s => s.StudentGroupEnrollments)
                    .ThenInclude(sge => sge.StudentGroup)
                .Include(s => s.Class)
                .Include(s => s.Grades)
                .Include(s => s.Class.Teacher)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return student;
        }
        public async Task<Student[]> GetAllStudentsAsync()
        {
            Student[] students = await context.Students
                .Include(s => s.Class)
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ToArrayAsync();
            return students;
        }
        public async Task<Student[]> GetAllStudentsBySubjectInstanceAsync(int subjectId, bool onlyIds = false)
        {
            SubjectInstanceEnrollment[] enrollments;
            if (onlyIds)
            {
                enrollments = await context.Enrollments
                .Where(s => s.SubjectInstance.Id == subjectId)
                .Include(e => e.StudentGroup)
                .ThenInclude(g => g.StudentGroupEnrollments)
                .AsNoTracking()
                .ToArrayAsync();
                List<Student> students = new();

                foreach (SubjectInstanceEnrollment enrollment in enrollments)
                    foreach (StudentGroupEnrollment groupEnrollment in enrollment.StudentGroup.StudentGroupEnrollments)
                        if (!students.Contains(groupEnrollment.Student))
                            students.Add(new Student { Id = groupEnrollment.StudentId });

                return students.ToArray();
            }
            else
            {
                enrollments = await context.Enrollments
                .Where(s => s.SubjectInstance.Id == subjectId)
                .Include(e => e.StudentGroup)
                .ThenInclude(g => g.StudentGroupEnrollments)
                .ThenInclude(ge => ge.Student)
                .AsNoTracking()
                .ToArrayAsync();
                List<Student> students = new();

                foreach (SubjectInstanceEnrollment enrollment in enrollments)
                    foreach (StudentGroupEnrollment groupEnrollment in enrollment.StudentGroup.StudentGroupEnrollments)
                        if (!students.Contains(groupEnrollment.Student))
                            students.Add(groupEnrollment.Student);

                return students.OrderBy(s => s.LastName).ToArray();
            }



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
            List<SubjectInstanceEnrollment> enrollments = await context.Enrollments
                .Where(s => s.SubjectInstance.TeacherId == teacherId)
                .Include(s => s.StudentGroup)
                    .ThenInclude(g => g.StudentGroupEnrollments)
                        .ThenInclude(ge => ge.Student)
                .AsNoTracking()
                .ToListAsync();

            List<Student> students = new List<Student>();

            foreach (SubjectInstanceEnrollment e in enrollments)
                foreach (StudentGroupEnrollment groupEnrollment in e.StudentGroup.StudentGroupEnrollments)
                    if (!students.Where(stud => stud.Id == groupEnrollment.StudentId).Any())
                        students.Add(groupEnrollment.Student);

            return students.OrderBy(s => s.LastName).ToList();
        }
        public async Task<int> GetStudentCountAsync()
        {
            return (await GetAllStudentsAsync()).Length;
        }
        public async Task<int> GetStudentCountBySubjectAsync(int subjectId)
        {
            SubjectInstanceEnrollment[] enrollments = await context.Enrollments
              .Where(s => s.SubjectInstance.Id == subjectId)
              .Include(e => e.StudentGroup)
                 .ThenInclude(g => g.StudentGroupEnrollments)
            .AsNoTracking()
            .ToArrayAsync();

            List<int> studentIds = new List<int>(40);

            foreach (SubjectInstanceEnrollment enrollment in enrollments)
                foreach (StudentGroupEnrollment groupEnrollment in enrollment.StudentGroup.StudentGroupEnrollments)
                    if (!studentIds.Contains(groupEnrollment.StudentId))
                        studentIds.Add(groupEnrollment.StudentId);

            return studentIds.Count;
        }
        public async Task<int> GetStudentCountByTeacherAsync(int teacherId)
        {
            SubjectInstanceEnrollment[] enrollments = await context.Enrollments
              .Where(s => s.SubjectInstance.TeacherId == teacherId)
              .Include(e => e.StudentGroup)
                 .ThenInclude(g => g.StudentGroupEnrollments)
            .AsNoTracking()
            .ToArrayAsync();

            List<int> studentIds = new List<int>(40);

            foreach (SubjectInstanceEnrollment enrollment in enrollments)
                foreach (StudentGroupEnrollment groupEnrollment in enrollment.StudentGroup.StudentGroupEnrollments)
                    if (!studentIds.Contains(groupEnrollment.StudentId))
                        studentIds.Add(groupEnrollment.StudentId);

            return studentIds.Count;
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
        public async Task RemoveStudentsAsync(ICollection<int> studentIds)
        {
            List<Student> students = new();
            foreach (int studentId in studentIds)
            {
                Student s = await context.Students
                    .Where(s => s.Id == studentId)
                    .Include(s => s.Grades)
                    .Include(s => s.StudentGroupEnrollments)
                    .FirstOrDefaultAsync();
                students.Add(s);
            }
            context.RemoveRange(students);
            await context.SaveChangesAsync();
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
