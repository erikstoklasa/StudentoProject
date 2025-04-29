# Studento School System

Welcome to the repository of the modern classification system Studento :blush:

We are innovating the outdated systems currently used in schools. The goal is to simplify the work for teachers, students, and administrators in managing grades, schedules, study materials, and other school agendas.

[![Build Status](https://dev.azure.com/Studento/WebApp/_apis/build/status/WebApp?branchName=master)](https://dev.azure.com/Studento/WebApp/_build/latest?definitionId=2&branchName=master)

## Key Features

*   **User Management:** Records of students, teachers, parents, and administrators.
*   **Account Activation:** Process for activating newly created user accounts.
*   **Class and Subject Management:** Creating and managing school classes and taught subjects.
*   **Grading:** Entering grades, calculating averages, managing grading groups.
*   **Timetable:** Displaying schedules, recording lessons, and potential changes.
*   **Study Materials:** Ability to upload and share materials for subjects.
*   **Attendance:** Recording student attendance (functionality indicated in models).
*   **Logging:** Recording important events in the system.

## Project Structure

*   **`StudentoMainProject/`**: Main web application built on ASP.NET Core. Contains the user interface (Razor Pages), API controllers, data models (Entity Framework Core), and business logic.
*   **`TimetableServiceProject/`**: Separate service, likely intended for processing or generating timetables.
*   **`XUnitTests/`**: Project containing unit tests to verify application functionality.

## Technologies Used

*   **.NET Core / ASP.NET Core:** Backend framework
*   **Entity Framework Core:** ORM for the database
*   **C#:** Main programming language
*   **SQL Server:** Database
*   **JavaScript:** Frontend logic
*   **Webpack:** JavaScript bundler
*   **Azure Pipelines:** CI/CD

## Project Setup

### Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or newer)
*   [Node.js and npm](https://nodejs.org/)
*   [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) instance

### Steps

1.  Clone the repository: `git clone <repository_URL> StudentoWeb`
2.  Navigate to the project folder: `cd StudentoWeb`
3.  Configure the connection strings (`DefaultConnection`, `SchoolContext`) in the `StudentoMainProject/appsettings.json` file (and potentially `StudentoMainProject/appsettings.Development.json`) for your SQL Server instance.
4.  Navigate to the main project folder: `cd StudentoMainProject`
5.  Install frontend dependencies: `npm install`
6.  Run the application: `dotnet run`
    *   On the first run, the database should be automatically created and migrated (thanks to `DbInitializer`). Alternatively, you can run the migration manually using `dotnet ef database update`.
    *   *Note: For full functionality, you might also need to run the `TimetableServiceProject`.* 

## Screenshots

This is what our application looks like so far ->
![Studento dashboard](/StudentoMainProject/github_readme_images/dashboard_v4.jpg)

## Our Milestones:

- [x] Student grade management and simple analysis
- [x] Simple mobile application
- [ ] Public release version
- [ ] More detailed analysis, grade development predictions

## Task List Before Next Milestone:

- [x] Detailed student data management
- [x] Demo version
- [ ] Mid-term report card
- [ ] More role types (principal, vice-principal)

## Contributing

If you encounter a bug or have a suggestion for improvement, feel free to create an [Issue](https://github.com/your-username/StudentoWeb/issues) (replace `your-username/StudentoWeb` with the correct repository path).

## License

This project is licensed under the license specified in the [LICENSE](StudentoMainProject/LICENSE) file.

## Notes

Every API key in the repo is invalid (this is an archive repo, nowadays we would never post secrects to a repo)
