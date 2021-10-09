using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentoMainProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeAverages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeAverages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HumanActivationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    HumanCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanActivationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAuthId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationIdentifNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectMaterialGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectMaterialGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    AdminLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectTypes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonalIdentifNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentifCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Approbations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    SubjectTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approbations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Approbations_SubjectTypes_SubjectTypeId",
                        column: x => x.SubjectTypeId,
                        principalTable: "SubjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Approbations_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    BaseRoomId = table.Column<int>(type: "int", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_BaseRoomId",
                        column: x => x.BaseRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    SubjectTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectInstances_SubjectTypes_SubjectTypeId",
                        column: x => x.SubjectTypeId,
                        principalTable: "SubjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectInstances_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectTypeId = table.Column<int>(type: "int", nullable: true),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: false),
                    SubjectMaterialGroupId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    ToDelete = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectMaterials_SubjectMaterialGroups_SubjectMaterialGroupId",
                        column: x => x.SubjectMaterialGroupId,
                        principalTable: "SubjectMaterialGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectMaterials_SubjectTypes_SubjectTypeId",
                        column: x => x.SubjectTypeId,
                        principalTable: "SubjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectMaterials_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonalIdentifNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentifCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeFrames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeFrames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeFrames_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeFrames_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeFrames_SubjectInstances_SubjectInstanceId",
                        column: x => x.SubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentGroupId = table.Column<int>(type: "int", nullable: false),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_SubjectInstances_SubjectInstanceId",
                        column: x => x.SubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradeGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_GradeGroups_GradeGroupId",
                        column: x => x.GradeGroupId,
                        principalTable: "GradeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_SubjectInstances_SubjectInstanceId",
                        column: x => x.SubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroupEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentGroupId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroupEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroupEnrollments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGroupEnrollments_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGroupEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeFrameId = table.Column<int>(type: "int", nullable: true),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SafetyInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonRecords_SubjectInstances_SubjectInstanceId",
                        column: x => x.SubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonRecords_TimeFrames_TimeFrameId",
                        column: x => x.TimeFrameId,
                        principalTable: "TimeFrames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimetableChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeFrameId = table.Column<int>(type: "int", nullable: true),
                    StudentGroupId = table.Column<int>(type: "int", nullable: true),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Canceled = table.Column<bool>(type: "bit", nullable: true),
                    CurrentSubjectInstanceId = table.Column<int>(type: "int", nullable: true),
                    CurrentTeacherId = table.Column<int>(type: "int", nullable: true),
                    CurrentRoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableChanges_Rooms_CurrentRoomId",
                        column: x => x.CurrentRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableChanges_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableChanges_SubjectInstances_CurrentSubjectInstanceId",
                        column: x => x.CurrentSubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableChanges_Teachers_CurrentTeacherId",
                        column: x => x.CurrentTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableChanges_TimeFrames_TimeFrameId",
                        column: x => x.TimeFrameId,
                        principalTable: "TimeFrames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimetableRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectInstanceId = table.Column<int>(type: "int", nullable: true),
                    Recurrence = table.Column<int>(type: "int", nullable: false),
                    RecurrenceStart = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    TimeFrameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableRecords_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableRecords_SubjectInstances_SubjectInstanceId",
                        column: x => x.SubjectInstanceId,
                        principalTable: "SubjectInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimetableRecords_TimeFrames_TimeFrameId",
                        column: x => x.TimeFrameId,
                        principalTable: "TimeFrames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonRecordId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_LessonRecords_LessonRecordId",
                        column: x => x.LessonRecordId,
                        principalTable: "LessonRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_SchoolId",
                table: "Admins",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Approbations_SubjectTypeId",
                table: "Approbations",
                column: "SubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Approbations_TeacherId",
                table: "Approbations",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_LessonRecordId",
                table: "AttendanceRecords",
                column: "LessonRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentId",
                table: "AttendanceRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_BaseRoomId",
                table: "Classes",
                column: "BaseRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentGroupId",
                table: "Enrollments",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SubjectInstanceId",
                table: "Enrollments",
                column: "SubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeGroupId",
                table: "Grades",
                column: "GradeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectInstanceId",
                table: "Grades",
                column: "SubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonRecords_SubjectInstanceId",
                table: "LessonRecords",
                column: "SubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonRecords_TimeFrameId",
                table: "LessonRecords",
                column: "TimeFrameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SchoolId",
                table: "Rooms",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupEnrollments_ClassId",
                table: "StudentGroupEnrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupEnrollments_StudentGroupId",
                table: "StudentGroupEnrollments",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupEnrollments_StudentId",
                table: "StudentGroupEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_ClassId",
                table: "StudentGroups",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_SchoolId",
                table: "StudentGroups",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ParentId",
                table: "Students",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInstances_SubjectTypeId",
                table: "SubjectInstances",
                column: "SubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInstances_TeacherId",
                table: "SubjectInstances",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectMaterials_SubjectMaterialGroupId",
                table: "SubjectMaterials",
                column: "SubjectMaterialGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectMaterials_SubjectTypeId",
                table: "SubjectMaterials",
                column: "SubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectMaterials_TeacherId",
                table: "SubjectMaterials",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTypes_SchoolId",
                table: "SubjectTypes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeFrames_RoomId",
                table: "TimeFrames",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeFrames_SchoolId",
                table: "TimeFrames",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeFrames_SubjectInstanceId",
                table: "TimeFrames",
                column: "SubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableChanges_CurrentRoomId",
                table: "TimetableChanges",
                column: "CurrentRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableChanges_CurrentSubjectInstanceId",
                table: "TimetableChanges",
                column: "CurrentSubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableChanges_CurrentTeacherId",
                table: "TimetableChanges",
                column: "CurrentTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableChanges_StudentGroupId",
                table: "TimetableChanges",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableChanges_TimeFrameId",
                table: "TimetableChanges",
                column: "TimeFrameId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableRecords_RoomId",
                table: "TimetableRecords",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableRecords_SubjectInstanceId",
                table: "TimetableRecords",
                column: "SubjectInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableRecords_TimeFrameId",
                table: "TimetableRecords",
                column: "TimeFrameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Approbations");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "GradeAverages");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "HumanActivationCodes");

            migrationBuilder.DropTable(
                name: "StudentGroupEnrollments");

            migrationBuilder.DropTable(
                name: "SubjectMaterials");

            migrationBuilder.DropTable(
                name: "TimetableChanges");

            migrationBuilder.DropTable(
                name: "TimetableRecords");

            migrationBuilder.DropTable(
                name: "LessonRecords");

            migrationBuilder.DropTable(
                name: "GradeGroups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "SubjectMaterialGroups");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "TimeFrames");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "SubjectInstances");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "SubjectTypes");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
