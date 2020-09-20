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
        private readonly SubjectService subjectService;

        public StudentAccessValidation(SchoolContext context, SubjectService subjectService)
        {
            this.context = context;
            this.subjectService = subjectService;
        }

        public async Task<bool> HasAccessToSubject(int studentId, int subjectInstanceId)
        {
            List<StudentGroupEnrollment> studentGroupEnrollments = await context.StudentGroupEnrollments.Where(e => e.StudentId == studentId).AsNoTracking().ToListAsync();
            foreach (StudentGroupEnrollment sge in studentGroupEnrollments)
            {
                if (await context.Enrollments.Where(e => e.StudentGroupId == sge.Id && e.SubjectInstanceId == subjectInstanceId).AnyAsync())
                {
                    return true;
                }
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
            List<SubjectInstance> instances = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);

            SubjectMaterial subjectMaterial = await context.SubjectMaterials
                .Where(sm => sm.Id == subjectMaterialId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return instances.Where(i => i.SubjectTypeId == subjectMaterial.SubjectTypeId).Any();
        }
    }
}
