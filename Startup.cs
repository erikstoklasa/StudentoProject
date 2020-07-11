using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using SchoolGradebook.Services;


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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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
            services.AddTransient<Analytics>();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/HumanCodes", "AdminAndTeacher");
                options.Conventions.AuthorizeFolder("/Teacher", "OnlyTeacher");
                options.Conventions.AuthorizeFolder("/Admin", "OnlyAdmin");
                options.Conventions.AuthorizeFolder("/Student", "OnlyStudent");
            });
            services.AddDbContext<SchoolContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SchoolContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
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
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            roleManager.CreateAsync(new IdentityRole("admin")).Wait();
            roleManager.CreateAsync(new IdentityRole("teacher")).Wait();
            roleManager.CreateAsync(new IdentityRole("student")).Wait();
            IdentityUser user;
            if (userManager.FindByEmailAsync("admin@studento.cz").Result != null)
            {
                user = userManager.FindByEmailAsync("admin@studento.cz").Result;
                userManager.AddToRoleAsync(user, "admin").Wait();
            }




        }
    }
}
