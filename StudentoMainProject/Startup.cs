using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System;
using System.IO;
using System.Reflection;

namespace SchoolGradebook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Identity/Account/Login");
                options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                options.LogoutPath = new PathString("/Identity/Account/Logout");
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAdmin", policy => policy.RequireRole("admin"));
                options.AddPolicy("OnlyTeacher", policy => policy.RequireRole("teacher"));
                options.AddPolicy("OnlyStudent", policy => policy.RequireRole("student"));
                options.AddPolicy("AdminAndTeacher", policy =>
                    policy.RequireAssertion(
                        context => context.User.IsInRole("admin") || context.User.IsInRole("teacher"))
                    );
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<EmailSender>();
            services.AddTransient<AnalyticsService>();
            services.AddTransient<TeacherAccessValidation>();
            services.AddTransient<StudentService>();
            services.AddTransient<SubjectService>();
            services.AddTransient<TeacherService>();
            services.AddTransient<GradeService>();
            services.AddTransient<StudentAccessValidation>();
            services.AddTransient<SubjectMaterialService>();
            services.AddTransient<ClassService>();
            services.AddTransient<ApprobationService>();
            services.AddTransient<StudentGroupService>();
            services.AddTransient<LessonRecordService>();
            services.AddTransient<TimeFrameService>();
            services.AddTransient<RoomService>();
            services.AddTransient<TimetableManager>();
            services.AddTransient<TimetableRecordService>();
            services.AddTransient<SchoolService>();
            services.AddTransient<AdminService>();
            services.AddTransient<TimetableChangeService>();
            services.AddTransient<AttendanceService>();
            services.AddTransient<GradeAverageService>();

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/HumanCodes", "AdminAndTeacher");
                options.Conventions.AuthorizeFolder("/Teacher", "OnlyTeacher");
                options.Conventions.AuthorizeFolder("/Admin", "OnlyAdmin");
                options.Conventions.AuthorizeFolder("/Student", "OnlyStudent");
            });
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<SchoolContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SchoolContext")));
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.CustomSchemaIds(type => type.ToString());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            app.UseSwagger();
            app.UseStatusCodePagesWithReExecute("/ErrorPage", "?code={0}");
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            const string cacheMaxAge = "604800";
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".webmanifest"] = "application/manifest+json";
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider,
                OnPrepareResponse = ctx =>
                {
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append(
                         "Cache-Control", $"public, max-age={cacheMaxAge}");
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Studento API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller=Home}/{id?}");
            });

            roleManager.CreateAsync(new IdentityRole("admin")).Wait();
            roleManager.CreateAsync(new IdentityRole("teacher")).Wait();
            roleManager.CreateAsync(new IdentityRole("student")).Wait();
        }
    }
}
