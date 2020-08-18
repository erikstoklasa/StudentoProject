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
            foreach(StudentGroupEnrollment group in context.StudentGroupEnrollments.Where(e => e.StudentId == studentId).AsEnumerable())
            {
                if (context.Enrollments.Where(e => e.StudentGroupId == group.Id && e.SubjectInstanceId == subjectInstanceId).Any())
                    return true;
            }
            return false;
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
            List<SubjectInstance> instances = await new SubjectService(context).GetAllSubjectInstancesByStudentAsync(studentId);

            SubjectMaterial subjectMaterial = await context.SubjectMaterials
                .Where(sm => sm.Id == subjectMaterialId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return instances.Where(i => i.SubjectTypeId == subjectMaterial.SubjectTypeId).Any();
        }
    }
}
