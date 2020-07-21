using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class StudentAccessValidation
    {
        private readonly SchoolContext context;

        public StudentAccessValidation(SchoolContext context)
        {
            this.context = context;
        }
        
        public bool HasAccessToSubject(int studentId, int subjectInstanceId)
        {
            bool enrollmentWithPassedValuesExists = context.Enrollments
                .Where(e => e.StudentId == studentId && e.SubjectInstanceId == subjectInstanceId)
                .Any();
            return enrollmentWithPassedValuesExists;
        }
        public async Task<bool> HasAccessToGrade(int studentId, int gradeId)
        {
            Grade grade = await context.Grades.Where(g => g.StudentId == studentId && g.Id == gradeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return grade != null;
        }
        public async Task<bool> HasAccessToSubjectMaterial(int studentId, Guid subjectMaterialId)
        {
            List<Enrollment> enrollments = await context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.SubjectInstance)
                .AsNoTracking()
                .ToListAsync();
            SubjectMaterial subjectMaterial = await context.SubjectMaterials
                .Where(sm => sm.Id == subjectMaterialId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            bool studentHasAccessToMaterial = enrollments
                .Where(e => e.SubjectInstance.SubjectTypeId == subjectMaterial.SubjectTypeId)
                .Any();
            return studentHasAccessToMaterial;
        }
    }
}
