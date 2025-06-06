﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TeacherAccessValidation
    {
        private readonly SchoolContext context;

        public TeacherAccessValidation(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<bool> HasAccessToSubject(int teacherId, int subjectInstanceId)
        {
            SubjectInstance subject = await context.SubjectInstances
                .Where(si => si.Id == subjectInstanceId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (subject == null) {
                return false;
            }
            return subject.TeacherId == teacherId;
        }
        
        public async Task<bool> HasAccessToSubjectType(int teacherId, int subjectTypeId)
        {
            SubjectType subjectType = await context.SubjectTypes
                .Where(st => st.Id == subjectTypeId)
                .Include(st => st.SubjectInstances)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectType.SubjectInstances.Where(si => si.TeacherId == teacherId).Any();
        }
        public async Task<bool> HasAccessToGrade(int teacherId, int gradeId)
        {
            Grade grade = await context.Grades
                .Where(g => g.Id == gradeId)
                .Select(g => new Grade()
                {
                    SubjectInstanceId = g.SubjectInstanceId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return await HasAccessToSubject(teacherId, grade.SubjectInstanceId);
        }
        public async Task<bool> HasAccessToGradeGroup(int teacherId, int gradeGroupId)
        {
            GradeGroup gradeGroup = await context.GradeGroups
                .Where(g => g.Id == gradeGroupId)
                .Select(g => new GradeGroup()
                {
                    AddedById = g.AddedById,
                    AddedBy = g.AddedBy
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            bool isAddedByTeacher = gradeGroup.AddedBy == GradeGroup.USERTYPE.Teacher;
            bool isAddedBySpecifiedTeacherId = gradeGroup.AddedById == teacherId;
            return isAddedBySpecifiedTeacherId && isAddedByTeacher;
        }
        public async Task<bool> HasAccessToStudent(int teacherId, int studentId)
        {
            List<SubjectInstance> instances = await context.GetService<SubjectService>().GetAllSubjectInstancesByStudentAsync(studentId);

            return instances.Where(i => i.TeacherId == teacherId).Any();
        }
        public async Task<bool> HasAccessToSubjectMaterial(int teacherId, Guid subjectMaterialId)
        {
            SubjectMaterial subjectMaterial = await context.SubjectMaterials
                .Where(sm => sm.Id == subjectMaterialId && sm.AddedById == teacherId && sm.AddedBy == SubjectMaterial.USERTYPE.Teacher)
                .Select(sm => new SubjectMaterial()
                {
                    Id = sm.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectMaterial != null;
        }
    }
}
