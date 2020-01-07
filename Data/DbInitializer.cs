
using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();
            if (context.Grades.Any())
            {
                return;
            }
            var students = new Student[]
            {
                new Student{FirstName="Jan", LastName="Studený"},
                new Student{FirstName="Monika", LastName="Boháčková"},
                new Student{FirstName="Antonín", LastName="Kovář"},
                new Student{FirstName="Žaneta", LastName="Plíšková"}
            };
            foreach(Student a in students)
            {
                context.Students.Add(a);
            }
            context.SaveChanges();

            var teachers = new Teacher[]
            {
                new Teacher{FirstName="Mgr. Jan", LastName="Novák"},
                new Teacher{FirstName="Mgr. Eliška", LastName="Boháčková"},
                new Teacher{FirstName="PhD. Tomáš", LastName="Kopecký"},
                new Teacher{FirstName="Bc. Anna", LastName="Plíšková"}
            };
            foreach (Teacher b in teachers)
            {
                context.Teachers.Add(b);
            }
            context.SaveChanges();

            var subjects = new Subject[]
            {
                new Subject{TeacherId=2, Name="Matematika"},
                new Subject{TeacherId=2, Name="Fyzika"},
                new Subject{TeacherId=2, Name="Český jazyk"},
                new Subject{TeacherId=2, Name="Chemie"},
                new Subject{TeacherId=2, Name="Anglický jazyk"},
                new Subject{TeacherId=4, Name="Německý jazyk"},
                new Subject{TeacherId=3, Name="Informatika"}
            };
            foreach (Subject d in subjects)
            {
                context.Subjects.Add(d);
            }
            context.SaveChanges();

            var grades = new Grade[]
            {
                new Grade{Value=1, StudentId=3, SubjectId=1, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=2, StudentId=3, SubjectId=2, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=3, StudentId=3, SubjectId=3, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=1, StudentId=3, SubjectId=3, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=2, StudentId=3, SubjectId=3, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=1, StudentId=3, SubjectId=4, Added=DateTime.Parse("11-11-2019")},
                new Grade{Value=1, StudentId=3, SubjectId=4, Added=DateTime.Parse("11-12-2019")},
                new Grade{Value=5, StudentId=3, SubjectId=4, Added=DateTime.Parse("01-12-2019")}
            };
            foreach (Grade e in grades)
            {
                context.Grades.Add(e);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentId=3, SubjectId=1},
                new Enrollment{StudentId=3, SubjectId=2},
                new Enrollment{StudentId=3, SubjectId=3},
                new Enrollment{StudentId=3, SubjectId=4},
                new Enrollment{StudentId=3, SubjectId=5},
                new Enrollment{StudentId=3, SubjectId=6},
                new Enrollment{StudentId=3, SubjectId=7}
            };
            foreach (Enrollment c in enrollments)
            {
                context.Enrollments.Add(c);
            }
            context.SaveChanges();

            var humanActivationCodes = new HumanActivationCode[]
            {
                new HumanActivationCode{TargetId=2, HumanCode="000000", CodeType = CodeType.Teacher},
                new HumanActivationCode{TargetId=3, HumanCode="111111", CodeType = CodeType.Student},
                new HumanActivationCode{TargetId=4, HumanCode="000111", CodeType = CodeType.Student}
                
            };
            foreach (HumanActivationCode d in humanActivationCodes)
            {
                context.HumanActivationCodes.Add(d);
            }
            context.SaveChanges();
        }
    }
}
