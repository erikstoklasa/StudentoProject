using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class TeacherService
    {
        private readonly SchoolContext context;

        public TeacherService(SchoolContext context)
        {
            this.context = context;
        }
        public async Task<bool> AddTeacherAsync(Teacher teacher, List<int> approbations)
        {
            //approbations list represents SubjectType Ids for which the teacher has access to teach
            if (!HasRequiredFields(teacher))
            {
                return false;
            }
            if (!ValidationUtils.PersonalIdentifNumberIsValid(teacher.PersonalIdentifNumber))
            {
                return false;
            }
            try
            {
                await context.Teachers.AddAsync(teacher);
                await context.SaveChangesAsync();
                foreach(var a in approbations)
                {
                    await context.Approbations.AddAsync(new Approbation { TeacherId = teacher.Id, SubjectTypeId = a });
                }
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<int> GetTeacherId(string userAuthId)
        {
            Teacher teacher = await context.Teachers
                .Where(t => t.UserAuthId == userAuthId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if(teacher == null) //Has role teacher but no assigned userAuthId - can happen after db resets
            {
                return -1;
            }
            return teacher.Id;
        }
        public async Task<Teacher> GetTeacherAsync(int teacherId)
        {
            Teacher teacher = await context.Teachers
                .Where(s => s.Id == teacherId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return teacher;
        }
        public async Task<Teacher> GetTeacherByEmailAsync(string email)
        {
            Teacher teacher = await context.Teachers
                .Where(s => s.Email == email)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return teacher;
        }
        public async Task<Teacher[]> GetAllTeachersAsync()
        {
            Teacher[] teacher = await context.Teachers
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ToArrayAsync();
            return teacher;
        }
        public async Task<int> GetTeacherCountAsync()
        {
            return (await GetAllTeachersAsync()).Length;
        }
        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            Teacher teacher = await context.Teachers.FindAsync(teacherId);

            if (teacher != null)
            {
                context.Teachers.Remove(teacher);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateTeacherAsync(Teacher teacher, List<int> newApprobations)
        {
            if (!HasRequiredFields(teacher))
            {
                return false;
            }
            if (!ValidationUtils.PersonalIdentifNumberIsValid(teacher.PersonalIdentifNumber))
            {
                return false;
            }
            context.Attach(teacher).State = EntityState.Modified;
            List<Approbation> oldApprobations = await context.Approbations
                .Where(a => a.TeacherId == teacher.Id)
                .AsNoTracking()
                .ToListAsync();
            try
            {
                //If not exists and want to add
                foreach(int i in newApprobations)
                {
                    if(!oldApprobations.Where(a => a.SubjectTypeId == i).Any())
                    {
                        await context.Approbations.AddAsync(new Approbation { SubjectTypeId = i, TeacherId = teacher.Id});
                    }
                }
                //If not exists but want to remove
                foreach (var a in oldApprobations)
                {
                    if(!newApprobations.Where(app => app == a.SubjectTypeId).Any())
                    {
                        Approbation toRemove = await context.Approbations.Where(app => app.SubjectTypeId == a.SubjectTypeId).FirstOrDefaultAsync();
                        context.Approbations.Remove(toRemove);
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
        //VALIDATIONS
        public static bool HasRequiredFields(Teacher teacher)
        {
            if (string.IsNullOrWhiteSpace(teacher.FirstName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(teacher.LastName))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(teacher.PersonalIdentifNumber))
            {
                return false;
            }
            if (!teacher.Birthdate.HasValue)
            {
                return false;
            }
            return true;
        }
    }
}
