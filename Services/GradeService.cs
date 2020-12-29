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
                .OrderByDescending(g => g.Added)
                .AsNoTracking()
                .ToListAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesInArrayByStudentSubjectInstance(int studentId, int subjectInstanceId)
        {
            Grade[] grades = await context.Grades
                .Where(g => g.SubjectInstanceId == subjectInstanceId && g.StudentId == studentId)
                .OrderByDescending(g => g.Added)
                .AsNoTracking()
                .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesBySubjectInstance(int subjectInstanceId)
        {
            Grade[] grades = await context.Grades
                .Where(g => g.SubjectInstanceId == subjectInstanceId)
                .AsNoTracking()
                .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesByStudentAsync(int studentId)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId)
            .Include(g => g.SubjectInstance)
            .Include(g => g.SubjectInstance.Teacher)
            .Include(g => g.SubjectInstance.SubjectType)
            .OrderByDescending(g => g.Added)
            .AsNoTracking()
            .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetRecentGradesByStudentAsync(int studentId, int skip = 0, int take = 0)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId)
            .OrderByDescending(g => g.Added)
            .Include(g => g.SubjectInstance.SubjectType)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
            return grades;
        }
        public async Task AddGradeAsync(Grade grade, bool saveChanges = true)
        {
            if (!HasRequiredFields(grade))
            {
                throw new ArgumentNullException("Grade name");
            }

            //Grading scale is relative to the country of school
            if (grade.Value <= 0 || grade.Value > 5)
            {
                throw new ArgumentOutOfRangeException("Grade value");
            }

            await context.Grades.AddAsync(grade);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
        public async Task UpdateGradeAsync(Grade grade)
        {
            if (!HasRequiredFields(grade))
            {
                throw new ArgumentNullException("Grade name");
            }

            //Grading scale is relative to the country of school
            if (grade.Value <= 0 || grade.Value > 5)
            {
                throw new ArgumentOutOfRangeException("Grade value");
            }

            context.Attach(grade).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateGradesAsync(ICollection<Grade> grades)
        {
            foreach (var grade in grades)
            {
                if (!HasRequiredFields(grade))
                {
                    throw new ArgumentNullException("Grade name");
                }
                //Grading scale is relative to the country of school
                if (grade.Value <= 0 || grade.Value > 5)
                {
                    throw new ArgumentOutOfRangeException("Grade value");
                }
                context.Attach(grade).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
        }

        public async Task AddGradesAsync(IEnumerable<Grade> grades)
        {
            foreach (var grade in grades)
            {
                if (!HasRequiredFields(grade))
                {
                    throw new ArgumentNullException("Grade name");
                }

                //Grading scale is relative to the country of school
                if (grade.Value <= 0 || grade.Value > 5)
                {
                    throw new ArgumentOutOfRangeException("Grade value");
                }
            }
            await context.Grades.AddRangeAsync(grades);
            await context.SaveChangesAsync();
        }
        public async Task DeleteGradesAsync(ICollection<int> gradeIds)
        {
            List<Grade> grades = new List<Grade>();
            foreach (int gId in gradeIds)
            {
                grades.Add(new Grade() { Id = gId });
            }
            context.Grades.RemoveRange(grades);
            await context.SaveChangesAsync();
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
