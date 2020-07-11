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
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<SubjectInstance> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<HumanActivationCode> HumanActivationCodes { get; set; }
        public DbSet<SubjectMaterial> SubjectMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<SubjectInstance>().ToTable("Subjects");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<HumanActivationCode>().ToTable("HumanActivationCodes");
            modelBuilder.Entity<SubjectMaterial>().ToTable("SubjectMaterials");
        }
    }
}
