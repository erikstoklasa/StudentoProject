
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
            //if none is filled in then return
            Random r = new Random();

            string[] firstNames = { "Jan", "Jakub", "Tomáš", "Matyáš", "Adam", "Filip", "Vojtěch", "Lukáš", "Martin", "Matěj", "Eliška", "Anna", "Adéla", "Tereza", "Sofie", "Viktorie", "Ema", "Karolína", "Natálie", "Amálie" };

            string[] lastNames = { "Nováková", "Novák", "Svobodová", "Svoboda", "Novotný", "Novotná", "Dvořáková", "Dvořák", "Černá", "Černý", "Procházková", "Procházka", "Kučerová", "Kučera", "Veselá", "Veselý" };
            DateTime[] dates = { DateTime.Parse("07-09-2019"), DateTime.Parse("10-12-2019"), DateTime.Parse("21-01-2020"), DateTime.Parse("18-02-2020"), DateTime.Parse("05-03-2020"), DateTime.Parse("28-03-2020"), DateTime.Parse("11-04-2020") };
            const int NUMBER_OF_TEACHERS = 10;
            const int NUMBER_OF_STUDENTS = 15;
            const int NUMBER_OF_GRADES = 200;
            context.Database.EnsureCreated();
            //Students
            if (!context.Students.Any())
            {
                List<Student> students = new List<Student>();

                for (int i = 0; i < NUMBER_OF_STUDENTS; i++)
                {
                    students.Add(new Student { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)] });
                }
                foreach (Student a in students)
                {
                    context.Students.Add(a);
                }
                context.SaveChanges();
            }


            //Teachers
            if (!context.Teachers.Any())
            {
                List<Teacher> teachers = new List<Teacher>();

                for (int i = 0; i < NUMBER_OF_TEACHERS; i++)
                {
                    teachers.Add(new Teacher { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)] });
                }

                foreach (Teacher b in teachers)
                {
                    context.Teachers.Add(b);
                }
                context.SaveChanges();
            }
            //Subject types
            if (!context.SubjectTypes.Any())
            {
                var subjectTypes = new SubjectType[]
                    {
                        new SubjectType{Name = "Český jazyk", SpecializationName = "Gymnázium"},
                        new SubjectType{Name = "Anglický jazyk", SpecializationName = "Gymnázium"},
                        new SubjectType{Name = "Německý jazyk", SpecializationName = "Gymnázium"},
                        new SubjectType{Name = "Španělský jazyk", SpecializationName = "Gymnázium"},
                        new SubjectType{Name = "Matematika", SpecializationName = "Gymnázium"},
                        new SubjectType{Name = "Programování", SpecializationName = "Gymnázium"}
                    };
                foreach (SubjectType d in subjectTypes)
                {
                    context.SubjectTypes.Add(d);
                }
                context.SaveChanges();
            }

            //Subject instances
            if (!context.SubjectInstances.Any())
            {
                var subjectInstances = new SubjectInstance[]
                    {
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 1},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 2},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 3},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 4},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 5},
                        new SubjectInstance{TeacherId=4, SubjectTypeId = 1},
                        new SubjectInstance{TeacherId=4, SubjectTypeId = 2},
                        new SubjectInstance{TeacherId=1, SubjectTypeId = 1}
                    };
                foreach (SubjectInstance d in subjectInstances)
                {
                    context.SubjectInstances.Add(d);
                }
                context.SaveChanges();
            }

            //Grades
            if (!context.Grades.Any())
            {
                List<Grade> grades = new List<Grade>();
                for (int i = 0; i < NUMBER_OF_GRADES; i++)
                {
                    grades.Add(
                        new Grade
                        {
                            Value = r.Next(1, 6),
                            Name = "Domácí úkol - prezentace",
                            StudentId = r.Next(1, NUMBER_OF_STUDENTS + 1),
                            SubjectInstanceId = r.Next(1, 8),
                            Added = dates[r.Next(dates.Length)]
                        });
                }
                foreach (Grade e in grades)
                {
                    context.Grades.Add(e);
                }
                context.SaveChanges();
            }

            //Enrollments
            if (!context.Enrollments.Any())
            {
                var enrollments = new Enrollment[]
                    {
                    new Enrollment{StudentId=1, SubjectInstanceId=1},
                    new Enrollment{StudentId=1, SubjectInstanceId=2},
                    new Enrollment{StudentId=1, SubjectInstanceId=3},
                    new Enrollment{StudentId=1, SubjectInstanceId=4},
                    new Enrollment{StudentId=1, SubjectInstanceId=5},
                    new Enrollment{StudentId=1, SubjectInstanceId=6},
                    new Enrollment{StudentId=1, SubjectInstanceId=7}
                    };
                foreach (Enrollment c in enrollments)
                {
                    context.Enrollments.Add(c);
                }
                context.SaveChanges();
            }
            //HumanActivationCodes
            if (!context.HumanActivationCodes.Any())
            {
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
}
