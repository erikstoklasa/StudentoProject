using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using StudentoMainProject.Models;

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
        public DbSet<GradeGroup> GradeGroups { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SubjectInstanceEnrollment> Enrollments { get; set; }
        public DbSet<HumanActivationCode> HumanActivationCodes { get; set; }
        public DbSet<SubjectMaterial> SubjectMaterials { get; set; }
        public DbSet<SubjectMaterialGroup> SubjectMaterialGroups { get; set; }
        public DbSet<SubjectType> SubjectTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentGroup> StudentGroups { set; get; }
        public DbSet<StudentGroupEnrollment> StudentGroupEnrollments { set; get; }
        public DbSet<TimeFrame> TimeFrames { set; get; }
        public DbSet<Room> Rooms { set; get; }
        public DbSet<LessonRecord> LessonRecords { set; get; }
        public DbSet<AttendanceRecord> AttendanceRecords { set; get; }
        public DbSet<TimetableRecord> TimetableRecords { set; get; }
        public DbSet<Approbation> Approbations { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<TimetableChange> TimetableChanges { get; set; }
        public DbSet<GradeAverage> GradeAverages { get; set; }
        public DbSet<LogItem> LogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().ToTable("Schools");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<SubjectType>().ToTable("SubjectTypes");
            modelBuilder.Entity<SubjectInstance>().ToTable("SubjectInstances");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Parent>().ToTable("Parents");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<SubjectInstanceEnrollment>().ToTable("Enrollments");
            modelBuilder.Entity<GradeGroup>().ToTable("GradeGroups");
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<HumanActivationCode>().ToTable("HumanActivationCodes");
            modelBuilder.Entity<SubjectMaterial>().ToTable("SubjectMaterials");
            modelBuilder.Entity<SubjectMaterialGroup>().ToTable("SubjectMaterialGroups");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Approbation>().ToTable("Approbations");
            modelBuilder.Entity<StudentGroup>().ToTable("StudentGroups");
            modelBuilder.Entity<StudentGroupEnrollment>().ToTable("StudentGroupEnrollments");
            modelBuilder.Entity<TimeFrame>().ToTable("TimeFrames");
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<GradeAverage>().ToTable("GradeAverages");
            modelBuilder.Entity<LogItem>().ToTable("LogItems");
            modelBuilder.Entity<LessonRecord>().ToTable("LessonRecords").HasIndex(tr => tr.TimeFrameId).IsUnique(false);
            modelBuilder.Entity<AttendanceRecord>().ToTable("AttendanceRecords");
            modelBuilder.Entity<TimetableRecord>().ToTable("TimetableRecords").HasIndex(tr => tr.TimeFrameId).IsUnique(false);
            modelBuilder.Entity<TimetableChange>().ToTable("TimetableChanges").HasIndex(tr => tr.TimeFrameId).IsUnique(false);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Grades)
                .WithOne(e => e.Student)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudentGroupEnrollments)
                .WithOne(e => e.Student)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<SubjectInstance>()
                .HasOne(e => e.Teacher)
                .WithMany(e => e.SubjectInstances)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<SubjectInstance>()
                .HasMany(e => e.Enrollments)
                .WithOne(e => e.SubjectInstance)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<SubjectInstance>()
                .HasMany(e => e.Grades)
                .WithOne(e => e.SubjectInstance)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<SubjectInstance>()
                .HasMany(e => e.TimetableRecords)
                .WithOne(e => e.SubjectInstance)
                .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
