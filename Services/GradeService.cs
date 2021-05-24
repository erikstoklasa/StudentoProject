using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolGradebook.Models.Grade;

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
                .Include(g => g.GradeGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return grade;
        }

        public async Task<Grade> GetPlainGradeAsync(int gradeId)
        {
            Grade grade = await context.Grades
                .Where(s => s.Id == gradeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return grade;
        }

        public async Task<Grade[]> GetAllGradesByStudentSubjectInstance(int studentId, int subjectInstanceId)
        {
            Grade[] grades = await context.Grades
                .Where(g => g.SubjectInstanceId == subjectInstanceId && g.StudentId == studentId)
                .Include(g => g.GradeGroup)
                .OrderByDescending(g => g.Added)
                .AsNoTracking()
                .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesAddedByTeacherAsync(int subjectInstanceId)
        {
            Grade[] grades = await context.Grades
                .Where(g => g.SubjectInstanceId == subjectInstanceId && g.AddedBy == USERTYPE.Teacher)
                .Include(g => g.GradeGroup)
                .AsNoTracking()
                .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesAsync(int studentId)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId)
            .Include(g => g.SubjectInstance)
            .Include(g => g.SubjectInstance.Teacher)
            .Include(g => g.SubjectInstance.SubjectType)
            .Include(g => g.GradeGroup)
            .OrderByDescending(g => g.Added)
            .AsNoTracking()
            .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetGradesAddedByStudentAsync(int studentId, int subjectInstanceId)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId && g.AddedBy == USERTYPE.Student && g.SubjectInstanceId == subjectInstanceId)
            .Include(g => g.SubjectInstance)
            .Include(g => g.SubjectInstance.Teacher)
            .Include(g => g.SubjectInstance.SubjectType)
            .OrderByDescending(g => g.Added)
            .AsNoTracking()
            .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetGradesAddedByTeacherAsync(int studentId, int subjectInstanceId)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId && g.AddedBy == USERTYPE.Teacher && g.SubjectInstanceId == subjectInstanceId)
            .Include(g => g.SubjectInstance)
            .Include(g => g.SubjectInstance.Teacher)
            .Include(g => g.SubjectInstance.SubjectType)
            .Include(g => g.GradeGroup)
            .OrderByDescending(g => g.Added)
            .AsNoTracking()
            .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetRecentGradesAsync(int studentId, int skip = 0, int take = 0)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.StudentId == studentId && g.AddedBy == USERTYPE.Teacher)
            .OrderByDescending(g => g.Added)
            .Include(g => g.SubjectInstance.SubjectType)
            .Include(g => g.GradeGroup)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToArrayAsync();
            return grades;
        }
        public async Task AddGradeAsync(Grade grade, USERTYPE usertype, bool saveChanges = true)
        {
            grade.Name ??= "";
            //Grading scale is relative to the country of school
            if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
            {
                throw new ArgumentOutOfRangeException("Grade value");
            }
            grade.AddedBy = usertype;
            await context.Grades.AddAsync(grade);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
        public async Task UpdateGradeAsync(Grade grade)
        {

            //Grading scale is relative to the country of school
            if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
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
                grade.Name ??= ""; 
                //Grading scale is relative to the country of school
                if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
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
                grade.Name ??= "";
                //Grading scale is relative to the country of school
                if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
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
        //-------------------
        //GradeGroup SECTION
        //-------------------

        public async Task AddGradeGroupAsync(GradeGroup gradeGroup, bool saveChanges = true)
        {
            if (!HasRequiredFields(gradeGroup))
            {
                throw new ArgumentNullException("Grade Group","Not all required properties were set");
            }
            await context.GradeGroups.AddAsync(gradeGroup);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
        public async Task UpdateGradeGroupAsync(GradeGroup gradeGroup)
        {
            if (!HasRequiredFields(gradeGroup))
            {
                throw new ArgumentNullException("Grade Group", "Not all required properties were set");
            }

            context.Attach(gradeGroup).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteGradeGroupsAsync(ICollection<int> gradeGroupIds)
        {
            List<GradeGroup> gradeGroups = new List<GradeGroup>();
            foreach (int ggId in gradeGroupIds)
            {
                gradeGroups.Add(new GradeGroup() { Id = ggId });
            }
            context.GradeGroups.RemoveRange(gradeGroups);
            await context.SaveChangesAsync();
        }

        //VALIDATIONS
        public static bool HasRequiredFields(GradeGroup gradeGroup)
        {
            if (string.IsNullOrWhiteSpace(gradeGroup.Name))
            {
                return false;
            }
            if (gradeGroup.Weight <= 0)
            {
                return false;
            }
            if (gradeGroup.AddedById <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
