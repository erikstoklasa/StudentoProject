using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TeacherAccessValidation
    {
        private readonly SchoolContext context;

        public TeacherAccessValidation(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<bool> HasAccessToSubject(int teacherId, int subjectId)
        {
            Subject subject = await context.Subjects.FindAsync(subjectId);
            return subject.TeacherId == teacherId;
        }
        public async Task<bool> HasAccessToGrade(int teacherId, int gradeId)
        {
            Grade grade = await context.Grades.FindAsync(gradeId);
            return await HasAccessToSubject(teacherId, grade.SubjectId);
        }
        public async Task<bool> HasAccessToStudent(int teacherId, int studentId)
        {
            return await context.Enrollments.Where(e => e.StudentId == studentId && e.Subject.TeacherId == teacherId).AnyAsync();
        }
        public async Task<bool> HasAccessToSubjectMaterial(int teacherId, Guid subjectMaterialId)
        {
            SubjectMaterial subjectMaterial = await context.SubjectMaterials.FindAsync(subjectMaterialId);
            return await HasAccessToSubject(teacherId, subjectMaterial.SubjectId);
        }
    }
}
