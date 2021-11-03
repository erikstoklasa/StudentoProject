using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            .Include(g => g.GradeGroup)
            .OrderByDescending(g => g.Added)
            .AsNoTracking()
            .ToArrayAsync();
            return grades;
        }
        public async Task<Grade[]> GetAllGradesBySubjectInstanceAsync(int subjectInstanceId)
        {
            Grade[] grades;
            grades = await context.Grades
            .Where(g => g.SubjectInstanceId == subjectInstanceId)
            .Include(g => g.GradeGroup)
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
        public async Task AddGradeAsync(Grade grade, USERTYPE usertype)
        {
            grade.AddedBy = usertype;
            if (GradeIsValid(grade))
            {
                await context.Grades.AddAsync(grade);
            }
            else
            {
                throw new ArgumentException("Grade is not valid.");
            }
            await context.SaveChangesAsync();
        }
        public async Task UpdateGradeAsync(Grade grade)
        {
            if (GradeIsValid(grade))
            {
                context.Attach(grade).State = EntityState.Modified;
            }
            else
            {
                throw new ArgumentException("Grade is not valid.");
            }
            await context.SaveChangesAsync();
        }

        public async Task UpdateGradesAsync(IEnumerable<Grade> grades)
        {
            foreach (var grade in grades)
            {
                if (GradeIsValid(grade))
                {
                    context.Attach(grade).State = EntityState.Modified;
                }
                else
                {
                    throw new ArgumentException("One or more grades are not valid.");
                }
            }
            await context.SaveChangesAsync();
        }
        public async Task AddGradesAsync(IEnumerable<Grade> grades)
        {
            List<Grade> gradesToAdd = new();
            foreach (var grade in grades)
            {
                if (GradeIsValid(grade))
                {
                    gradesToAdd.Add(grade);
                }
                else
                {
                    throw new ArgumentException("One or more grades are not valid.");
                }
            }
            await context.Grades.AddRangeAsync(grades);
            await context.SaveChangesAsync();
        }
        public async Task DeleteGradesAsync(IEnumerable<int> gradeIds)
        {
            List<Grade> grades = new();
            foreach (int gId in gradeIds)
            {
                grades.Add(new Grade() { Id = gId });
            }
            context.Grades.RemoveRange(grades);
            await context.SaveChangesAsync();
        }
        public static bool GradeIsValid(Grade grade)
        {
            if (grade.AddedBy == USERTYPE.Teacher)
            {
                if (grade.GradeGroupId == null || grade.GradeGroupId < 0)
                    return false;
            }
            else //grade is added by a student
            {
                if (string.IsNullOrWhiteSpace(grade.Name))
                    return false;
                if (grade.Weight == null || grade.Weight <= 0)
                    return false;
            }

            if (grade.SubjectInstanceId <= 0)
                return false;

            if (grade.StudentId <= 0)
                return false;

            if (
                    DateTime.Compare(
                        grade.Added,
                        DateTime.ParseExact("01/01/2010", "MM/dd/yyyy", CultureInfo.InvariantCulture)
                    ) <= 0
                )
                return false;

            if (grade.GetInternalGradeValue() < -10 || grade.GetInternalGradeValue() > 110)
                throw new ArgumentOutOfRangeException("Grade value");

            return true;
        }
        //-------------------
        //GradeGroup SECTION
        //-------------------
        public async Task<GradeGroup> GetGradeGroupAsync(int gradeGroupId)
        {
            GradeGroup gradeGroup = await context.GradeGroups
                .Where(s => s.Id == gradeGroupId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return gradeGroup;
        }
        public async Task<IEnumerable<GradeGroup>> GetAllGradeGroupsAsync()
        {
            IList<GradeGroup> gradeGroups = await context.GradeGroups
                .AsNoTracking().ToListAsync();
            return gradeGroups;
        }
        public async Task AddGradeGroupAsync(GradeGroup gradeGroup, bool saveChanges = true)
        {
            if (!GradeGroupIsValid(gradeGroup))
            {
                throw new ArgumentNullException("Grade Group", "Not all required properties were set");
            }
            await context.GradeGroups.AddAsync(gradeGroup);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
        public async Task UpdateGradeGroupAsync(GradeGroup gradeGroup)
        {
            if (!GradeGroupIsValid(gradeGroup))
            {
                throw new ArgumentNullException("Grade Group", "Not all required properties were set");
            }

            context.Attach(gradeGroup).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteGradeGroupsAsync(IEnumerable<int> gradeGroupIds)
        {
            List<GradeGroup> gradeGroups = new();
            foreach (int ggId in gradeGroupIds)
            {
                gradeGroups.Add(new GradeGroup() { Id = ggId });
            }
            context.GradeGroups.RemoveRange(gradeGroups);
            await context.SaveChangesAsync();
        }

        //VALIDATIONS
        public static bool GradeGroupIsValid(GradeGroup gradeGroup)
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
            if (
                    DateTime.Compare(
                        gradeGroup.Added, 
                        DateTime.ParseExact("01/01/2010", "MM/dd/yyyy", CultureInfo.InvariantCulture)
                    ) <= 0
                )
                return false;

            return true;
        }
    }
}
