using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace SchoolGradebook.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
            //if none is filled in then return
            Random r = new Random();

            string[] firstNames = { "Jan", "Jakub", "Tomáš", "Matyáš", "Adam", "Filip", "Vojtěch", "Lukáš", "Martin", "Matěj", "Eliška", "Anna", "Adéla", "Tereza", "Sofie", "Viktorie", "Ema", "Karolína", "Natálie", "Amálie" };

            string[] lastNames = { "Nováková", "Novák", "Svobodová", "Svoboda", "Novotný", "Novotná", "Dvořáková", "Dvořák", "Černá", "Černý", "Procházková", "Procházka", "Kučerová", "Kučera", "Veselá", "Veselý" };
            DateTime[] dates = { DateTime.Parse("07/09/2019"), DateTime.Parse("10/12/2019"), DateTime.Parse("11/01/2020"), DateTime.Parse("18/02/2020"), DateTime.Parse("05/03/2020"), DateTime.Parse("28/03/2020"), DateTime.Parse("11/04/2020") };
            DateTime[] birthdays = { DateTime.Parse("07/09/2002"), DateTime.Parse("10/12/2002"), DateTime.Parse("21/01/2002"), DateTime.Parse("18/02/2002"), DateTime.Parse("05/03/2002"), DateTime.Parse("28/03/2002"), DateTime.Parse("11/04/2002") };
            const int NUMBER_OF_TEACHERS = 10;
            const int NUMBER_OF_STUDENTS = 15;
            const int NUMBER_OF_GRADES = 200;
            const int NUMBER_OF_CLASSES = 20;
            string[] classesNames = { "A", "B", "C", "D" };
            int classesPerYear = classesNames.Length;

            context.Database.EnsureCreated();

            //Schools
            if (!context.Schools.Any())
            {
                List<School> schools = new List<School>

                {
                    new School() { Name = "Gymnázium Jana Keplera", CityAddress = "Praha 6", StreetAddress = "Parléřova 117", Email = "gjk@jk.cz", OrganizationIdentifNumber = "123456789", PhoneNumber = "123456789", Website = "https://gjk.cz", ZipCode = "16000" }
                };

                context.Schools.AddRange(schools);
                context.SaveChanges();
            }

            //Teachers
            if (!context.Teachers.Any())
            {
                List<Teacher> teachers = new List<Teacher>();

                for (int i = 0; i < NUMBER_OF_TEACHERS; i++)
                {
                    teachers.Add(new Teacher { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)], SchoolId = 1, Birthdate = birthdays[r.Next(birthdays.Length)], PersonalIdentifNumber = "11" });
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
                        new SubjectType{Name = "Český jazyk", SpecializationName = "Gymnázium", SchoolId=1},
                        new SubjectType{Name = "Anglický jazyk", SpecializationName = "Gymnázium", SchoolId=1},
                        new SubjectType{Name = "Německý jazyk", SpecializationName = "Gymnázium", SchoolId=1},
                        new SubjectType{Name = "Španělský jazyk", SpecializationName = "Gymnázium", SchoolId=1},
                        new SubjectType{Name = "Matematika", SpecializationName = "Gymnázium", SchoolId=1},
                        new SubjectType{Name = "Programování", SpecializationName = "Gymnázium", SchoolId=1}
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
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 2},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 2},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 3},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 4},
                        new SubjectInstance{TeacherId=3, SubjectTypeId = 5},
                        new SubjectInstance{TeacherId=4, SubjectTypeId = 5},
                        new SubjectInstance{TeacherId=4, SubjectTypeId = 6},
                        new SubjectInstance{TeacherId=1, SubjectTypeId = 1}
                    };
                foreach (SubjectInstance d in subjectInstances)
                {
                    context.SubjectInstances.Add(d);
                }
                context.SaveChanges();
            }



            // Rooms
            if (!context.Classes.Any())
            {
                var rooms = new Room[]
                {
                    new Room{Name="101", SchoolId=1},
                    new Room{Name="102", SchoolId=1},
                    new Room{Name="103", SchoolId=1},
                    new Room{Name="104", SchoolId=1},
                    new Room{Name="105", SchoolId=1}
                };

                foreach (Room room in rooms)
                    context.Rooms.Add(room);

                context.SaveChanges();
            }
            //Classes
            if (!context.Classes.Any())
            {
                List<Class> classes = new List<Class>();
                short years = (short)(NUMBER_OF_CLASSES / classesPerYear);
                for (int i = 1; i <= years; i++)
                {
                    for (int y = 0; y < classesPerYear; y++)
                    {
                        classes.Add(new Class() { Grade = (short)i, Name = classesNames[y], BaseRoomId = 1, TeacherId = 1, SchoolId = 1 });
                    }

                }
                context.Classes.AddRange(classes);
                context.SaveChanges();
            }
            //Students
            if (!context.Students.Any())
            {
                List<Student> students = new List<Student>();

                for (int i = 0; i < NUMBER_OF_STUDENTS; i++)
                {
                    students.Add(new Student { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)], ClassId = 1, SchoolId = 1, Birthdate = birthdays[r.Next(birthdays.Length)], PersonalIdentifNumber = "11" });
                }
                foreach (Student a in students)
                {
                    context.Students.Add(a);
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
                            Name = "Domácí úkol / prezentace",
                            StudentId = r.Next(1, NUMBER_OF_STUDENTS),
                            SubjectInstanceId = r.Next(1, 8),
                            Added = dates[r.Next(dates.Length)]
                        });
                }
                context.Grades.AddRange(grades);
                context.SaveChanges();
            }
            //TimeFrames
            if (!context.TimeFrames.Any())
            {
                List<TimeFrame> timeFrames = new List<TimeFrame>();
                for (int i = 1; i <= 5; i++)
                {
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("08:30:00"), End = DateTime.Parse("09:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("09:30:00"), End = DateTime.Parse("10:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("10:30:00"), End = DateTime.Parse("11:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("11:30:00"), End = DateTime.Parse("12:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("12:30:00"), End = DateTime.Parse("13:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("13:30:00"), End = DateTime.Parse("14:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("14:30:00"), End = DateTime.Parse("15:15:00"), SchoolId = 1 });
                }
                context.TimeFrames.AddRange(timeFrames);
                context.SaveChanges();
            }
            //TimetableRecords
            if (!context.TimetableRecords.Any())
            {
                List<TimetableRecord> timetableRecords = new List<TimetableRecord>();
                for (int i = 0; i < 20; i++)
                {
                    timetableRecords.Add(new TimetableRecord() { SubjectInstanceId = r.Next(1, 8), RoomId = r.Next(1, 4), Recurrence = 1, TimeFrameId = r.Next(1, 36) });
                }
                context.TimetableRecords.AddRange(timetableRecords);
                context.SaveChanges();
            }
            //StudentGroups
            if (!context.StudentGroups.Any())
            {
                var groups = new StudentGroup[]
                {
                    new StudentGroup{Name="Skupina1", SchoolId=1},
                    new StudentGroup{Name="Skupina2", SchoolId=1}
                };

                foreach (StudentGroup group in groups)
                    context.StudentGroups.Add(group);
                context.SaveChanges();
            }
            //TimetableChanges
            if (!context.TimetableChanges.Any())
            {
                List<TimetableChange> timetableChanges = new List<TimetableChange>();
                for (int i = 0; i < 20; i++)
                {
                    timetableChanges.Add(new TimetableChange() { SubjectInstanceId = r.Next(1, 8), StudentGroupId = r.Next(1, 3), Week = r.Next(1, 11), TimeFrameId = r.Next(1, 36), Canceled = r.Next(1, 3) == 1 });
                }
                context.TimetableChanges.AddRange(timetableChanges);
                context.SaveChanges();
            }
            //StudentGroupEnrollments
            if (!context.StudentGroupEnrollments.Any())
            {
                var entrollments = new StudentGroupEnrollment[]
                {
                    new StudentGroupEnrollment{StudentId=1, StudentGroupId=1}
                };

                foreach (StudentGroupEnrollment enrollment in entrollments)
                    context.StudentGroupEnrollments.Add(enrollment);
                context.SaveChanges();
            }

            //Enrollments
            if (!context.Enrollments.Any())
            {
                var enrollments = new SubjectInstanceEnrollment[]
                    {
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=1},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=2},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=3},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=4},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=5},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=6},
                    new SubjectInstanceEnrollment{StudentGroupId=1, SubjectInstanceId=7}
                    };
                foreach (SubjectInstanceEnrollment c in enrollments)
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
                new HumanActivationCode{TargetId=1, HumanCode="000000", CodeType = CodeType.Teacher},
                new HumanActivationCode{TargetId=1, HumanCode="111111", CodeType = CodeType.Student}

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
