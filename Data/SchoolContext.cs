using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;

namespace SchoolGradebook.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<SubjectInstance> SubjectInstances { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<HumanActivationCode> HumanActivationCodes { get; set; }
        public DbSet<SubjectMaterial> SubjectMaterials { get; set; }
        public DbSet<SubjectType> SubjectTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentGroup> StudentGroups { set; get; }
        public DbSet<StudentGroupEnrollment> StudentGroupEnrollments { set; get; }

        public DbSet<Approbation> Approbations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<SubjectType>().ToTable("SubjectTypes");
            modelBuilder.Entity<SubjectInstance>().ToTable("SubjectInstances");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Parent>().ToTable("Parents");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<HumanActivationCode>().ToTable("HumanActivationCodes");
            modelBuilder.Entity<SubjectMaterial>().ToTable("SubjectMaterials");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Approbation>().ToTable("Approbations");
            modelBuilder.Entity<StudentGroup>().ToTable("StudentGroups");
            modelBuilder.Entity<StudentGroupEnrollment>().ToTable("StudentGroupEnrollments");
        }
    }
}
