using SchoolGradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace SchoolGradebook.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(SchoolContext context)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
            //if none is filled in then return
            Random r = new();

            string[] firstNames = { "Jan", "Jakub", "Tomáš", "Matyáš", "Adam", "Filip", "Vojtěch", "Lukáš", "Martin", "Matěj", "Eliška", "Anna", "Adéla", "Tereza", "Sofie", "Viktorie", "Ema", "Karolína", "Natálie", "Amálie" };

            string[] lastNames = { "Nováková", "Novák", "Svobodová", "Svoboda", "Novotný", "Novotná", "Dvořáková", "Dvořák", "Černá", "Černý", "Procházková", "Procházka", "Kučerová", "Kučera", "Veselá", "Veselý" };
            DateTime[] dates = { DateTime.Parse("07/09/2019"), DateTime.Parse("10/12/2019"), DateTime.Parse("11/01/2020"), DateTime.Parse("18/02/2020"), DateTime.Parse("05/03/2020"), DateTime.Parse("28/03/2020"), DateTime.Parse("11/04/2020") };
            DateTime[] birthdays = { DateTime.Parse("07/09/2002"), DateTime.Parse("10/12/2002"), DateTime.Parse("21/01/2002"), DateTime.Parse("18/02/2002"), DateTime.Parse("05/03/2002"), DateTime.Parse("28/03/2002"), DateTime.Parse("11/04/2002") };
            const int NUMBER_OF_TEACHERS = 6;
            const int NUMBER_OF_STUDENTS = 15;
            const int NUMBER_OF_CLASSES = 20;
            const int NUMBER_OF_SUBJECTTYPES = 6;
            const int NUMBER_OF_SUBJECTS_PER_TEACHER = 3;
            const int NUMBER_OF_SUBJECTINSTANCES = NUMBER_OF_SUBJECTS_PER_TEACHER * NUMBER_OF_TEACHERS;
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

                await context.Schools.AddRangeAsync(schools);
                await context.SaveChangesAsync();
            }

            //Teachers
            if (!context.Teachers.Any())
            {
                List<Teacher> teachers = new();

                for (int i = 0; i < NUMBER_OF_TEACHERS; i++)
                {
                    teachers.Add(new Teacher { FirstName = firstNames[r.Next(firstNames.Length)], LastName = lastNames[r.Next(lastNames.Length)], SchoolId = 1, Birthdate = birthdays[r.Next(birthdays.Length)], PersonalIdentifNumber = "11" });
                }

                await context.Teachers.AddRangeAsync(teachers);
                await context.SaveChangesAsync();
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

                await context.SubjectTypes.AddRangeAsync(subjectTypes);
                await context.SaveChangesAsync();
            }

            //Subject instances
            if (!context.SubjectInstances.Any())
            {
                List<SubjectInstance> subjectInstances = new();
                for (int i = 1; i <= NUMBER_OF_TEACHERS; i++)
                {
                    for (int y = 0; y < NUMBER_OF_SUBJECTS_PER_TEACHER; y++)
                    {
                        subjectInstances.Add(new SubjectInstance { TeacherId = i, SubjectTypeId = r.Next(1, NUMBER_OF_SUBJECTTYPES) });
                    }
                }

                await context.SubjectInstances.AddRangeAsync(subjectInstances);
                await context.SaveChangesAsync();
            }


            // Rooms
            if (!context.Classes.Any())
            {
                var rooms = new Room[]
                {
                    new Room{Name="H1.1", SchoolId=1},
                    new Room{Name="H1.2", SchoolId=1},
                    new Room{Name="H1.3", SchoolId=1},
                    new Room{Name="H2.1", SchoolId=1},
                    new Room{Name="H2.2", SchoolId=1}
                };


                await context.Rooms.AddRangeAsync(rooms);
                await context.SaveChangesAsync();
            }
            //Classes
            if (!context.Classes.Any())
            {
                List<Class> classes = new();
                short yearCount = (short)(NUMBER_OF_CLASSES / classesPerYear);
                for (int year = 1; year <= yearCount; year++)
                {
                    for (int classInYear = 0; classInYear < classesPerYear; classInYear++)
                    {
                        classes.Add(new Class() { Grade = (short)year, Name = classesNames[classInYear], BaseRoomId = r.Next(1, 6), TeacherId = 1, SchoolId = 1 });
                    }

                }
                await context.Classes.AddRangeAsync(classes);
                await context.SaveChangesAsync();
            }
            //Students
            if (!context.Students.Any())
            {
                List<Student> students = new();

                for (int i = 0; i < NUMBER_OF_STUDENTS; i++)
                {
                    students.Add(
                        new Student
                        {
                            FirstName = firstNames[r.Next(firstNames.Length)],
                            LastName = lastNames[r.Next(lastNames.Length)],
                            ClassId = 1,
                            SchoolId = 1,
                            Birthdate = birthdays[r.Next(birthdays.Length)],
                            PersonalIdentifNumber = "11"
                        });
                }
                await context.Students.AddRangeAsync(students);
                await context.SaveChangesAsync();
            }
            //GradeGroups
            if (!context.GradeGroups.Any())
            {

                List<GradeGroup> gradeGroups = new();
                string[] gradeGroupNames = { "Domácí úkol", "Test", "Čtvrtletní práce", "Prezentace", "Aktivita při hodině" };

                List<SubjectInstance> subjectInstances = await context.SubjectInstances
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var si in subjectInstances)
                {
                    foreach (var gradeGroupName in gradeGroupNames)
                    {
                        var g = new GradeGroup
                        {
                            Name = gradeGroupName,
                            Weight = r.Next(1, 11),
                            AddedBy = GradeGroup.USERTYPE.Teacher,
                            AddedById = si.TeacherId
                        };
                        gradeGroups.Add(g);
                    }
                }

                await context.GradeGroups.AddRangeAsync(gradeGroups);
                await context.SaveChangesAsync();
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
                await context.TimeFrames.AddRangeAsync(timeFrames);
                await context.SaveChangesAsync();
            }
            //TimetableRecords
            if (!context.TimetableRecords.Any())
            {
                List<TimetableRecord> timetableRecords = new();
                for (int i = 0; i < 50; i++)
                {
                    timetableRecords.Add(new TimetableRecord() { SubjectInstanceId = r.Next(1, 8), RoomId = r.Next(1, 4), Recurrence = 1, TimeFrameId = r.Next(1, 56) });
                }
                await context.TimetableRecords.AddRangeAsync(timetableRecords);
                await context.SaveChangesAsync();
            }
            //StudentGroups
            if (!context.StudentGroups.Any())
            {
                var groups = new StudentGroup[]
                {
                    new StudentGroup{Name="Skupina1", SchoolId=1},
                    new StudentGroup{Name="Skupina2", SchoolId=1}
                };

                await context.StudentGroups.AddRangeAsync(groups);
                await context.SaveChangesAsync();
            }
            //TimetableChanges
            if (!context.TimetableChanges.Any())
            {
                List<TimetableChange> timetableChanges = new();
                for (int i = 0; i < 60; i++)
                {
                    timetableChanges.Add(new TimetableChange() { SubjectInstanceId = r.Next(1, 8), StudentGroupId = r.Next(1, 3), Week = r.Next(1, 50), TimeFrameId = r.Next(1, 36), Canceled = r.Next(1, 3) == 1 });
                }
                await context.TimetableChanges.AddRangeAsync(timetableChanges);
                await context.SaveChangesAsync();
            }
            //StudentGroupEnrollments
            if (!context.StudentGroupEnrollments.Any())
            {
                List<Student> students = await context.Students.AsNoTracking().ToListAsync();
                List<StudentGroupEnrollment> enrollments = new();
                foreach (var student in students)
                {
                    int studentGroupId = student.Id % 2 == 0 ? 1 : 2; //Distributing students into two student groups - one to the first student group and the other to the second student group

                    enrollments.Add(
                        new StudentGroupEnrollment
                        {
                            StudentId = student.Id,
                            StudentGroupId = studentGroupId
                        });

                }

                await context.StudentGroupEnrollments.AddRangeAsync(enrollments);
                await context.SaveChangesAsync();
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
                await context.Enrollments.AddRangeAsync(subjectInstanceEnrollments);
                await context.SaveChangesAsync();
            }
            //Grades
            if (!context.Grades.Any())
            {
                int[] allowedValues = { -10, 0, 10, 15, 25, 35, 40, 50, 60, 65, 75, 85, 90, 100, 110 };
                List<Grade> grades = new();
                List<SubjectInstance> subjectInstances =
                    await context.SubjectInstances.AsNoTracking().ToListAsync();
                List<Student> students =
                    await context.Students.AsNoTracking().ToListAsync();
                foreach (var si in subjectInstances)
                {
                    foreach (var student in students)
                    {
                        for (int gradeGroupId = 1; gradeGroupId <= 5; gradeGroupId++)
                        {
                            var g = new Grade
                            {
                                Name = "",
                                StudentId = student.Id,
                                SubjectInstanceId = si.Id,
                                GradeGroupId = gradeGroupId,
                                Added = dates[r.Next(dates.Length)]
                            };
                            g.SetGradeValue(
                                allowedValues[
                                    r.Next(allowedValues.Length)
                                    ]
                                );
                            grades.Add(g);
                        }

                    }

                }
                await context.Grades.AddRangeAsync(grades);
                await context.SaveChangesAsync();
            }
            //HumanActivationCodes
            if (!context.HumanActivationCodes.Any())
            {
                HumanActivationCode[] humanActivationCodes = new HumanActivationCode[]
                {
                new HumanActivationCode{TargetId=1, HumanCode="000000", CodeType = CodeType.Teacher},
                new HumanActivationCode{TargetId=1, HumanCode="111111", CodeType = CodeType.Student}

                };
                await context.HumanActivationCodes.AddRangeAsync(humanActivationCodes);
                await context.SaveChangesAsync();
            }

        }
    }
}
