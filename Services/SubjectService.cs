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
            Student student = await context.Students
                .Where(s => s.Id == studentId)
                .Include(s => s.StudentGroupEnrollments)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            List<SubjectInstance> subjectInstances = new List<SubjectInstance>();
            List<Enrollment> enrollments = new List<Enrollment>();
            foreach (var sge in student.StudentGroupEnrollments)
            {
                enrollments = await context.Enrollments
                    .Where(e => e.StudentGroupId == sge.StudentGroupId)
                    .Include(e => e.SubjectInstance)
                    .Include(e => e.SubjectInstance.Teacher)
                    .Include(e => e.SubjectInstance.SubjectType)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var e in enrollments)
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
        //Subject Types
        public async Task<List<SubjectType>> GetAllSubjectTypesAsync()
        {
            List<SubjectType> subjectTypes = await context.SubjectTypes
                .AsNoTracking()
                .ToListAsync();
            return subjectTypes;
        }
        //VALIDATIONS
    }
}
