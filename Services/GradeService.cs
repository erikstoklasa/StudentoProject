using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class GradeService
    {
        private readonly SchoolContext context;

        public GradeService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<Grade> GetGradeAsync(int gradeId)
        {
            Grade grade = await context.Grades
                .Where(s => s.Id == gradeId)
                .Include(g => g.SubjectInstance)
                .Include(g => g.Student)
                .Include(g => g.SubjectInstance.Teacher)
                .Include(g => g.SubjectInstance.SubjectType)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return grade;
        }
        public async Task<List<Grade>> GetAllGradesByStudentSubjectInstance(int studentId, int subjectInstanceId)
        {
            List<Grade> grades = await context.Grades
                .Where(g => g.SubjectInstanceId == subjectInstanceId && g.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync();
            return grades;
        }
        public async Task<List<Grade>> GetAllGradesByStudentAsync(int studentId, int skip = 0, int take = 0)
        {
            List<Grade> grades;
            if (take > 0)
            {
                grades = await context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.SubjectInstance)
                .Include(g => g.SubjectInstance.Teacher)
                .Include(g => g.SubjectInstance.SubjectType)
                .OrderByDescending(g => g.Added)
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            } else
            {
                grades = await context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.SubjectInstance)
                .Include(g => g.SubjectInstance.Teacher)
                .Include(g => g.SubjectInstance.SubjectType)
                .OrderByDescending(g => g.Added)
                .AsNoTracking()
                .Skip(skip)
                .ToListAsync();
            }
            
            return grades;
        }
        public async Task<bool> AddGradeAsync(Grade grade)
        {
            if (!HasRequiredFields(grade))
            {
                return false;
            }
            //Grading scale is relative to the country of school
            if (grade.Value <= 0 && grade.Value > 5)
            {
                return false;
            }
            try
            {
                await context.Grades.AddAsync(grade);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        //VALIDATIONS
        public static bool HasRequiredFields(Grade grade)
        {
            if (string.IsNullOrWhiteSpace(grade.Name))
            {
                return false;
            }
            return true;
        }
    }
}
