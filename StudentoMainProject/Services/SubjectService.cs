using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Pages.Admin.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class SubjectService
    {
        private readonly SchoolContext context;

        public SubjectService(SchoolContext context)
        {
            this.context = context;
        }
        //Subject Instances
        public async Task<bool> AddSubjectInstanceAsync(SubjectInstance subjectInstance)
        {
            bool teacherExists = context.Teachers.Where(t => t.Id == subjectInstance.TeacherId).Any();
            bool subjectTypeExists = context.SubjectTypes.Where(st => st.Id == subjectInstance.SubjectTypeId).Any();
            if (!teacherExists)
            {
                return false;
            }
            if (!subjectTypeExists)
            {
                return false;
            }
            try
            {
                await context.SubjectInstances.AddAsync(subjectInstance);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<SubjectInstance> GetSubjectInstanceAsync(int subjectInstanceId)
        {
            SubjectInstance subjectInstance = await context.SubjectInstances
                .Where(s => s.Id == subjectInstanceId)
                .Include(s => s.SubjectType)
                .Include(s => s.Teacher)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.StudentGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectInstance;
        }
        public async Task<SubjectInstance> GetSubjectInstanceByTeacherAndSubjectTypeAsync(int teacherId, int subjectTypeId)
        {
            SubjectInstance subjectInstance = await context.SubjectInstances
                .Where(s => s.TeacherId == teacherId && s.SubjectTypeId == subjectTypeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectInstance;
        }
        public async Task<SubjectInstance> GetSubjectInstanceFullAsync(int subjectInstanceId)
        {
            SubjectInstance subjectInstance = await context.SubjectInstances
                .Where(s => s.Id == subjectInstanceId)
                .Include(s => s.SubjectType)
                .Include(s => s.Teacher)
                .Include(s => s.Enrollments)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectInstance;
        }
        public async Task<List<SubjectInstance>> GetAllSubjectInstancesFullAsync()
        {
            List<SubjectInstance> subjectInstances = await context.SubjectInstances
                .Include(s => s.SubjectType)
                .Include(s => s.Teacher)
                .Include(s => s.Enrollments)
                .AsNoTracking()
                .ToListAsync();
            return subjectInstances;
        }
        public async Task<List<SubjectInstance>> GetAllSubjectInstancesAsync()
        {
            List<SubjectInstance> subjectInstances = await context.SubjectInstances
                .Include(s => s.SubjectType)
                .Include(s => s.Teacher)
                .AsNoTracking()
                .ToListAsync();
            return subjectInstances;
        }
        public async Task<List<SubjectInstance>> GetAllSubjectInstancesByStudentAsync(int studentId)
        {
            StudentGroupEnrollment[] studentGroupEnrollments = await context.StudentGroupEnrollments
                .Where(s => s.StudentId == studentId)
                .Include(sge => sge.StudentGroup.Enrollments)
                .ThenInclude(e => e.SubjectInstance)
                .AsNoTracking()
                .ToArrayAsync();

            List<SubjectInstance> subjectInstances = new List<SubjectInstance>();
            SubjectInstanceEnrollment[] enrollments;
            foreach (StudentGroupEnrollment sge in studentGroupEnrollments)
            {
                enrollments = await context.Enrollments
                    .Where(e => e.StudentGroupId == sge.StudentGroupId)
                    .Include(e => e.SubjectInstance)
                    .Include(e => e.SubjectInstance.Teacher)
                    .Include(e => e.SubjectInstance.SubjectType)
                    .OrderBy(e => e.SubjectInstance.SubjectType.Name)
                    .AsNoTracking()
                    .ToArrayAsync();
                foreach (SubjectInstanceEnrollment e in enrollments)
                {
                    subjectInstances.Add(e.SubjectInstance);
                }
            }

            return subjectInstances;
        }
        public async Task<List<SubjectInstance>> GetAllSubjectInstancesByTeacherAsync(int teacherId)
        {
            List<SubjectInstance> subjectInstances = await context.SubjectInstances
                .Where(s => s.TeacherId == teacherId)
                .Include(s => s.SubjectType)
                .Include(s => s.Teacher)
                .OrderBy(s => s.SubjectType.Name)
                .AsNoTracking()
                .ToListAsync();
            return subjectInstances;
        }
        public async Task<bool> UpdateSubjectInstance(SubjectInstance subjectInstance)
        {
            bool teacherExists = context.Teachers.Where(t => t.Id == subjectInstance.TeacherId).Any();
            bool subjectTypeExists = context.SubjectTypes.Where(st => st.Id == subjectInstance.SubjectTypeId).Any();
            if (!teacherExists)
            {
                return false;
            }
            if (!subjectTypeExists)
            {
                return false;
            }
            context.Attach(subjectInstance).State = EntityState.Modified;
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
        public async Task<bool> DeleteSubjectInstanceAsync(int subjectInstanceId)
        {
            SubjectInstance subjectInstance = await context.SubjectInstances.FindAsync(subjectInstanceId);

            if (subjectInstance != null)
            {
                context.SubjectInstances.Remove(subjectInstance);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<SubjectInstance[]> GetSubjectInstancesByGroupId(int groupId)
            => await context.SubjectInstances
                    .Where(si => si.Enrollments.Where(e => e.StudentGroupId == groupId).Any())
                    .AsNoTracking()
                    .ToArrayAsync();

        //Subject Types
        public async Task<List<SubjectType>> GetAllSubjectTypesAsync()
        {
            List<SubjectType> subjectTypes = await context.SubjectTypes
                .AsNoTracking()
                .ToListAsync();
            return subjectTypes;
        }
        public async Task AddSubjectTypeAsync(SubjectType st)
        {
            await context.SubjectTypes.AddAsync(st);
            await context.SaveChangesAsync();
        }
        public async Task<SubjectType> GetSubjectTypeAsync(int id)
        {
            SubjectType subjectType = await context.SubjectTypes
                .Where(st => st.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectType;
        }
        public async Task UpdateSubjectTypeAsync(SubjectType st)
        {
            context.Attach(st).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { }
        }
        public async Task DeleteSubjectTypeAsync(int id)
        {
            SubjectType st = await context.SubjectTypes
                .Where(st => st.Id == id)
                .FirstOrDefaultAsync();
            if (st != null)
            {
                context.SubjectTypes.Remove(st);
                await context.SaveChangesAsync();
            }
        }
    }
}
