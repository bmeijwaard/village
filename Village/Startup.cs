using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Village.Config;
using Village.Data;
using Village.Data.Domain;
using Village.Data.Identity;

namespace Village
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ServiceProvider ServiceProvider { get; private set; }
        public RoleSeeder RoleSeeder { get; private set; }
        public UserSeeder UserSeeder { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // This needs to be enabled to use DI for IOptions<> to map appsettings
            services.AddOptions();
            //services.Configure<ExampleSettings>(opt => Configuration.GetSection("ExampleSettings").Bind(opt));

            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });;

            services.AddIdentity<User, Role>(options =>
            {
                options.Lockout = new LockoutOptions
                {
                    AllowedForNewUsers = true,
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30),
                    MaxFailedAccessAttempts = 5
                };
            })
            .AddEntityFrameworkStores<SqlContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<User, Role, SqlContext, Guid>>()
            .AddRoleStore<RoleStore<Role, SqlContext, Guid>>()
            .AddUserManager<ApplicationUserManager>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = true;
            });

            services.AddScoped<ApplicationUserManager>();
            services.AddScoped<ApplicationSignInManager>();
            services.AddScoped<SqlContext>();
            
            AutoMapperConfig.Register();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<RoleSeeder>();
            services.AddTransient<UserSeeder>();

            ServiceProvider = services.BuildServiceProvider();
            RoleSeeder = ServiceProvider.GetService<RoleSeeder>();
            UserSeeder = ServiceProvider.GetService<UserSeeder>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                RoleSeeder.Seed().Wait();
                UserSeeder.Seed().Wait();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
