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
            Random r = new();

            string[] firstNames = { "Jan", "Jakub", "Tomáš", "Matyáš", "Adam", "Filip", "Vojtěch", "Lukáš", "Martin", "Matěj", "Eliška", "Anna", "Adéla", "Tereza", "Sofie", "Viktorie", "Ema", "Karolína", "Natálie", "Amálie" };

            string[] lastNames = { "Nováková", "Novák", "Svobodová", "Svoboda", "Novotný", "Novotná", "Dvořáková", "Dvořák", "Černá", "Černý", "Procházková", "Procházka", "Kučerová", "Kučera", "Veselá", "Veselý" };
            DateTime[] dates = { DateTime.Parse("07/09/2019"), DateTime.Parse("10/12/2019"), DateTime.Parse("11/01/2020"), DateTime.Parse("18/02/2020"), DateTime.Parse("05/03/2020"), DateTime.Parse("28/03/2020"), DateTime.Parse("11/04/2020") };
            DateTime[] birthdays = { DateTime.Parse("07/09/2002"), DateTime.Parse("10/12/2002"), DateTime.Parse("21/01/2002"), DateTime.Parse("18/02/2002"), DateTime.Parse("05/03/2002"), DateTime.Parse("28/03/2002"), DateTime.Parse("11/04/2002") };
            const int NUMBER_OF_TEACHERS = 10;
            const int NUMBER_OF_STUDENTS = 15;
            const int NUMBER_OF_CLASSES = 20;
            const int NUMBER_OF_SUBJECTTYPES = 6;
            const int NUMBER_OF_SUBJECTINSTANCES = NUMBER_OF_TEACHERS * NUMBER_OF_SUBJECTTYPES;
            string[] classesNames = { "A", "B", "C", "D" };
            int classesPerYear = classesNames.Length;

            context.Database.EnsureCreated();

            //Schools
            if (!context.Schools.Any())
            {
                List<School> schools = new()
                {
                    new School() { Name = "Gymnázium Jana Keplera", CityAddress = "Praha 6", StreetAddress = "Parléřova 117", Email = "gjk@jk.cz", OrganizationIdentifNumber = "123456789", PhoneNumber = "123456789", Website = "https://gjk.cz", ZipCode = "16000" }
                };

                context.Schools.AddRange(schools);
                context.SaveChanges();
            }

            //Teachers
            if (!context.Teachers.Any())
            {
                List<Teacher> teachers = new();

                for (int i = 0; i < NUMBER_OF_TEACHERS; i++)
                {
                    teachers.Add(new Teacher { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)], SchoolId = 1, Birthdate = birthdays[r.Next(birthdays.Length)], PersonalIdentifNumber = "11" });
                }

                context.Teachers.AddRange(teachers);
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

                context.SubjectTypes.AddRange(subjectTypes);
                context.SaveChanges();
            }

            //Subject instances
            if (!context.SubjectInstances.Any())
            {
                List<SubjectInstance> subjectInstances = new();
                for (int i = 1; i <= NUMBER_OF_SUBJECTTYPES; i++)
                {
                    for (int y = 1; y <= NUMBER_OF_TEACHERS; y++)
                    {
                        subjectInstances.Add(new SubjectInstance { TeacherId = y, SubjectTypeId = i });
                    }
                }

                context.SubjectInstances.AddRange(subjectInstances);
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


                context.Rooms.AddRange(rooms);
                context.SaveChanges();
            }
            //Classes
            if (!context.Classes.Any())
            {
                List<Class> classes = new();
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
                List<Student> students = new();

                for (int i = 0; i < NUMBER_OF_STUDENTS; i++)
                {
                    students.Add(new Student { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)], ClassId = 1, SchoolId = 1, Birthdate = birthdays[r.Next(birthdays.Length)], PersonalIdentifNumber = "11" });
                }
                context.Students.AddRange(students);
                context.SaveChanges();
            }
            //GradeGroups
            if (!context.GradeGroups.Any())
            {
                List<GradeGroup> gradeGroups = new();
                string[] gradeGroupNames = { "Domácí úkol", "Test", "Čtvrtletní práce", "Prezentace", "Aktivita při hodině" };
                for (int i = 0; i < gradeGroupNames.Length; i++)
                {
                    var g = new GradeGroup
                    {
                        Name = gradeGroupNames[i],
                        Weight = 5,
                    };
                    gradeGroups.Add(g);
                }
                context.GradeGroups.AddRange(gradeGroups);
                context.SaveChanges();
            }
            //Grades
            if (!context.Grades.Any())
            {
                int[] allowedValues = { -10, 0, 10, 15, 25, 35, 40, 50, 60, 65, 75, 85, 90, 100, 110 };
                List<Grade> grades = new();
                for (int i = 1; i <= NUMBER_OF_STUDENTS; i++)
                {
                    for (int y = 1; y <= NUMBER_OF_SUBJECTINSTANCES; y++)
                    {
                        for (int z = 1; z <= 5; z++)
                        {
                            var g = new Grade
                            {
                                Name = "",
                                StudentId = i,
                                SubjectInstanceId = y,
                                GradeGroupId = z,
                                Added = dates[r.Next(dates.Length)]
                            };
                            g.SetGradeValue(
                                allowedValues[
                                    r.Next(0, allowedValues.Length)
                                    ]
                                );
                            grades.Add(g);
                        }

                    }

                }
                context.Grades.AddRange(grades);
                context.SaveChanges();
            }
            //TimeFrames
            if (!context.TimeFrames.Any())
            {
                List<TimeFrame> timeFrames = new();
                for (int i = 1; i <= 5; i++)
                {
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("08:30:00"), End = DateTime.Parse("09:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("09:30:00"), End = DateTime.Parse("10:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("10:30:00"), End = DateTime.Parse("11:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("11:30:00"), End = DateTime.Parse("12:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("12:30:00"), End = DateTime.Parse("13:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("13:30:00"), End = DateTime.Parse("14:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("14:30:00"), End = DateTime.Parse("15:15:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("15:20:00"), End = DateTime.Parse("16:05:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("16:10:00"), End = DateTime.Parse("16:55:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("17:00:00"), End = DateTime.Parse("17:45:00"), SchoolId = 1 });
                    timeFrames.Add(new TimeFrame() { DayOfWeek = (DayOfWeek)i, Start = DateTime.Parse("17:50:00"), End = DateTime.Parse("18:35:00"), SchoolId = 1 });
                }
                context.TimeFrames.AddRange(timeFrames);
                context.SaveChanges();
            }
            //TimetableRecords
            if (!context.TimetableRecords.Any())
            {
                List<TimetableRecord> timetableRecords = new();
                for (int i = 0; i < 50; i++)
                {
                    timetableRecords.Add(new TimetableRecord() { SubjectInstanceId = r.Next(1, 8), RoomId = r.Next(1, 4), Recurrence = 1, TimeFrameId = r.Next(1, 56) });
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

                context.StudentGroups.AddRange(groups);
                context.SaveChanges();
            }
            //TimetableChanges
            if (!context.TimetableChanges.Any())
            {
                List<TimetableChange> timetableChanges = new();
                for (int i = 0; i < 60; i++)
                {
                    timetableChanges.Add(new TimetableChange() { SubjectInstanceId = r.Next(1, 8), StudentGroupId = r.Next(1, 3), Week = r.Next(1, 50), TimeFrameId = r.Next(1, 36), Canceled = r.Next(1, 3) == 1 });
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

                context.StudentGroupEnrollments.AddRange(entrollments);
                context.SaveChanges();
            }

            //Enrollments
            if (!context.Enrollments.Any())
            {
                List<SubjectInstanceEnrollment> subjectInstanceEnrollments = new();

                for (int i = 1; i <= NUMBER_OF_SUBJECTINSTANCES; i++)
                {
                    if (i % 2 == 0)//2 is the number of student groups to be split into the subject instances
                    {
                        subjectInstanceEnrollments.Add(new SubjectInstanceEnrollment { StudentGroupId = 1, SubjectInstanceId = i });
                    }
                    else
                    {
                        subjectInstanceEnrollments.Add(new SubjectInstanceEnrollment { StudentGroupId = 2, SubjectInstanceId = i });
                    }
                }
                context.Enrollments.AddRange(subjectInstanceEnrollments);
                context.SaveChanges();
            }
            //HumanActivationCodes
            if (!context.HumanActivationCodes.Any())
            {
                HumanActivationCode[] humanActivationCodes = new HumanActivationCode[]
                {
                new HumanActivationCode{TargetId=1, HumanCode="000000", CodeType = CodeType.Teacher},
                new HumanActivationCode{TargetId=1, HumanCode="111111", CodeType = CodeType.Student}

                };
                context.HumanActivationCodes.AddRange(humanActivationCodes);
                context.SaveChanges();
            }

        }
    }
}
