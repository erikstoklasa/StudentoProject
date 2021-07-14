using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableServiceProject.Models;

namespace TimetableServiceProject.Data
{
    public class TimetableContext : DbContext
    {
        public TimetableContext(DbContextOptions<TimetableContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Elective> Electives { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Elective>().ToTable("Electives");
        }
    }
}
