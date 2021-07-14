using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class StudentGroupService
    {
        private readonly SchoolContext context;

        public StudentGroupService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task AddStudentToGroup(int studentId, int studentGroupId)
        {
            if (await GroupExists(studentGroupId))
            {
                context.StudentGroupEnrollments.Add(
                    new Models.StudentGroupEnrollment() { StudentGroupId = studentGroupId, StudentId = studentId }
                    );
                await context.SaveChangesAsync();
            }
        }
        public async Task AddGroup(int classId, string groupName)
        {
            Models.StudentGroup studentGroup = new Models.StudentGroup() { ClassId = classId, Name = groupName };
            context.StudentGroups.Add(studentGroup);
            await context.SaveChangesAsync();
        }
        public async Task RemoveStudentGroupEnrollment(int studentEnrollmentId)
        {
            StudentGroupEnrollment sge = await context.StudentGroupEnrollments.FindAsync(studentEnrollmentId);
            context.StudentGroupEnrollments.Remove(sge);
            await context.SaveChangesAsync();
        }
        public async Task<List<StudentGroup>> GetAllGroupsAsync()
        {
            return await context.StudentGroups.AsNoTracking().ToListAsync();
        }
        public async Task<List<StudentGroupEnrollment>> GetAllGroupEnrollmentsByStudentAsync(int studentId)
        {
            return await context.StudentGroupEnrollments.Where(sge => sge.StudentId == studentId).AsNoTracking().ToListAsync();
        }
        public async Task<List<StudentGroup>> GetAllGroupsBySubjectInstanceAsync(int subjectInstanceId)
        {
            var enrollments = await context.Enrollments.Where(e => e.SubjectInstanceId == subjectInstanceId)
                .Include(e => e.StudentGroup)
                .AsNoTracking()
                .ToListAsync();
            var studentGroups = new List<StudentGroup>();
            foreach (var e in enrollments)
            {
                studentGroups.Add(e.StudentGroup);
            }
            return studentGroups;
        }
        public async Task<StudentGroupEnrollment> GetGroupEnrollmentAsync(int id)
        {
            return await context.StudentGroupEnrollments.Where(sge => sge.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> GroupExists(int id)
        {
            return await context.StudentGroups.Where(sg => sg.Id == id).AnyAsync();
        }
    }
}
