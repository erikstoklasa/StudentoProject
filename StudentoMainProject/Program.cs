using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System;
using System.Threading.Tasks;

namespace SchoolGradebook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await CreateDbIfNotExists(host);

            host.Run();
        }

        private static async Task CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<SchoolContext>();
                var studentService = services.GetRequiredService<StudentService>();
                var gradeService = services.GetRequiredService<GradeService>();
                /* 
                 * Note: 
                 * Uncomment these in case you need to forcefully recreate the tables.
                 * Don't forget to query db_init.sql afterwards and comment these back. 
                 */
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //context.Database.Migrate();
                await DbInitializer.Initialize(context, studentService, gradeService);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
