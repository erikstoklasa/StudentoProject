﻿using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class SubjectMaterialService
    {
        private readonly SchoolContext context;

        public SubjectMaterialService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<SubjectMaterial> GetMaterialAsync(Guid subjectMaterialId)
        {
            SubjectMaterial subjectMaterial = await context.SubjectMaterials
                .Where(sm => sm.Id == subjectMaterialId)
                .Include(sm => sm.SubjectType)
                .Include(sm => sm.SubjectMaterialGroup)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return subjectMaterial;
        }
        public async Task<List<SubjectMaterial>> GetAllMaterialsBySubjectInstance(int subjectInstanceId, int take = 0)
        {
            if (take <= 0)
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == false)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .ToListAsync();
                return subjectMaterials;
            }
            else
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == false)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .Take(take)
                    .ToListAsync();
                return subjectMaterials;
            }

        }
        public async Task<List<SubjectMaterial>> GetAllMaterialsBySubjectInstanceForTeacher(int subjectInstanceId, int take = 0)
        {
            if (take <= 0)
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == false && sm.AddedBy == SubjectMaterial.USERTYPE.Teacher)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .ToListAsync();
                return subjectMaterials;
            }
            else
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == false && sm.AddedBy == SubjectMaterial.USERTYPE.Teacher)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .Take(take)
                    .ToListAsync();
                return subjectMaterials;
            }

        }
        public async Task<bool> AddMaterialAsync(SubjectMaterial subjectMaterial)
        {
            if (!HasRequiredFields(subjectMaterial))
            {
                return false;
            }
            try
            {
                await context.SubjectMaterials.AddAsync(subjectMaterial);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task UpdateMaterialAsync(SubjectMaterial subjectMaterial)
        {
            context.Attach(subjectMaterial).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<List<SubjectMaterial>> GetAllDeletedMaterials(int subjectInstanceId, int take = 0)
        {
            if (take <= 0)
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == true)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .ToListAsync();
                return subjectMaterials;
            }
            else
            {
                List<SubjectMaterial> subjectMaterials = await context.SubjectMaterials
                    .Where(sm => sm.SubjectInstanceId == subjectInstanceId && sm.ToDelete == true)
                    .Include(sm => sm.SubjectMaterialGroup)
                    .AsNoTracking()
                    .OrderByDescending(sm => sm.Added)
                    .Take(take)
                    .ToListAsync();
                return subjectMaterials;
            }

        }
        public async Task<bool> AddMaterialGroupAsync(SubjectMaterialGroup subjectMaterialGroup)
        {
            if (subjectMaterialGroup.Name == null)
            {
                return false;
            }
            try
            {
                await context.SubjectMaterialGroups.AddAsync(subjectMaterialGroup);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task UpdateMaterialGroupAsync(SubjectMaterialGroup subjectMaterialGroup)
        {
            context.Attach(subjectMaterialGroup).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<bool> DeleteMaterialGroupAsync(int subjectMaterialGroupId)
        {
            SubjectMaterialGroup SubjectMaterialGroup = await context.SubjectMaterialGroups.FindAsync(subjectMaterialGroupId);

            if (SubjectMaterialGroup != null)
            {
                context.SubjectMaterialGroups.Remove(SubjectMaterialGroup);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteMaterialAsync(Guid subjectMaterialId)
        {
            SubjectMaterial subjectMaterial = await context.SubjectMaterials.FindAsync(subjectMaterialId);

            if (subjectMaterial != null)
            {
                context.SubjectMaterials.Remove(subjectMaterial);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task SoftDeleteMaterialAsync(Guid subjectMaterialId)
        {
            SubjectMaterial subjectMaterial = await context.SubjectMaterials.FindAsync(subjectMaterialId);
            subjectMaterial.ToDelete = true;
            await context.SaveChangesAsync();
        }
        //VALIDATIONS
        public static bool HasRequiredFields(SubjectMaterial subjectMaterial)
        {
            if (string.IsNullOrWhiteSpace(subjectMaterial.Name))
            {
                return false;
            }
            return true;
        }
    }
}
