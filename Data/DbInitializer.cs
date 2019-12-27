
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
                new Student{FirstName="Jan", LastName="Novák"},
                new Student{FirstName="Monika", LastName="Boháčková", UserAuthId="98e49d8c-a908-425d-a197-337cd033dfac"},
                new Student{FirstName="Tomáš", LastName="Kopecký"},
                new Student{FirstName="Anna", LastName="Plíšková"}
            };
            foreach(Student a in students)
            {
                context.Students.Add(a);
            }
            context.SaveChanges();

            var teachers = new Teacher[]
            {
                new Teacher{FirstName="Mgr. Jan", LastName="Novák"},
                new Teacher{FirstName="Mgr. Monika", LastName="Boháčková"},
                new Teacher{FirstName="PhD. Tomáš", LastName="Kopecký", UserAuthId="f19c611a-b3aa-40cb-a2e3-1f0d9fee6598"},
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
                new Subject{TeacherId=2, Name="Informatika"}
            };
            foreach (Subject d in subjects)
            {
                context.Subjects.Add(d);
            }
            context.SaveChanges();

            var grades = new Grade[]
            {
                new Grade{Value=2, StudentId=4, SubjectId=1, Added=DateTime.Parse("11-11-2019")},
                new Grade{Value=3, StudentId=3, SubjectId=2, Added=DateTime.Parse("12-11-2019")},
                new Grade{Value=1, StudentId=2, SubjectId=3, Added=DateTime.Parse("10-11-2019")},
                new Grade{Value=5, StudentId=1, SubjectId=4, Added=DateTime.Parse("01-11-2019")}
            };
            foreach (Grade e in grades)
            {
                context.Grades.Add(e);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentId=1, SubjectId=4},
                new Enrollment{StudentId=1, SubjectId=3},
                new Enrollment{StudentId=1, SubjectId=2},
                new Enrollment{StudentId=2, SubjectId=3},
                new Enrollment{StudentId=3, SubjectId=2},
                new Enrollment{StudentId=4, SubjectId=1}
            };
            foreach (Enrollment c in enrollments)
            {
                context.Enrollments.Add(c);
            }
            context.SaveChanges();

            var humanActivationCodes = new HumanActivationCode[]
            {
                new HumanActivationCode{Id=1, HumanCode="123456", CodeType = CodeType.Student},
                new HumanActivationCode{Id=2, HumanCode="hatrat", CodeType = CodeType.Student},
                new HumanActivationCode{Id=3, HumanCode="000111", CodeType = CodeType.Student}
                
            };
            foreach (HumanActivationCode d in humanActivationCodes)
            {
                context.HumanActivationCodes.Add(d);
            }
            context.SaveChanges();
        }
    }
}
