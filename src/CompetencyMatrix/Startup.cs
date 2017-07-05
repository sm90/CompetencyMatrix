using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompetencyMatrix.Data;
using CompetencyMatrix.Infrastructure;
using CompetencyMatrix.Infrastructure.XLS.Formatters;
using CompetencyMatrix.Models;
using CompetencyMatrix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using CompetencyMatrix.Infrastructure.ModelBinders;
using CompetencyMatrix.Infrastructure.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CompetencyMatrix
{
    public class Startup
    {
        public static IHostingEnvironment HostingEnvironment { get; private set; }

        public static MailSettings MailSettings { get; private set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);

                var launchConfiguration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(@"Properties\launchSettings.json")
                .Build();
                // During development we won't be using port 443.
                _sslPort = launchConfiguration.GetValue<int>("iisSettings:iisExpress:sslPort");
            }

            Configuration = builder.Build();

            foreach (var c in Configuration.GetSection("ErrorPages").GetChildren())
            {
                var key = Convert.ToInt32(c.Key);
                if (!ErrorPages.Keys.Contains(key))
                {
                    ErrorPages.Add(key, c.Value);
                }
            }

            MailSettings = new MailSettings(Configuration.GetSection("MailSettings").GetChildren());

            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        private int _sslPort;

        internal static IDictionary<int, string> ErrorPages { get; } = new Dictionary<int, string>();
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(240);
            });

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddUserManager<ActiveDirectoryUserManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddOptions();
            services.Configure<DomainOptions>(Configuration);

            var csvFormatterOptions = new CsvFormatterOptions();
            services
                .AddMvc(config =>
                {
                    config.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
                    config.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
                    //config.Filters.Add(typeof(CustomExceptionFilter));
                })
                .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Formatting.Indented)
                .AddJsonOptions(o => o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None);

            services.AddDbContext<CompetencyMatrixContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CompetencyMatrix")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CompetencyMatrix")));

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IServerVariables, ServerVariables>();
            services.AddTransient<ICompetencyMatrixContext, CompetencyMatrixContext>();
            services.AddTransient<CompetencyMatrixContext>();

            services.AddTransient<IPositionMatrixService, PositionMatrixService>();
            services.AddTransient<IPositionMatrixInheritanceService, PositionMatrixInheritanceService>();
            services.AddTransient<IViewModelValidationService, ViewModelValidationService>();
            services.AddTransient<ILogsReportService, LogsReportService>();

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ITemplateService, TemplateService>();

            AddAuthorization(services);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SkillAccess",
                                  policy => policy.Requirements.Add(new SkillLevelRequirement(21)));
                
            });

            services.AddSingleton<IAuthorizationHandler, SkillAccessHandler>();
            services.AddSingleton<IAuthorizationHandler, PositionMatrixAuthorizationHandler>();
            
            //services.AddSingleton<ViewRenderService>();
            services.AddScoped<ViewRender, ViewRender>();

            services.AddMvc().AddMvcOptions(options => {
                options.ModelBinderProviders.Insert(0, new TrimmingModelBinderProvider());
                options.SslPort = _sslPort;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();// for NLog
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministrationPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("adminlevel");
                });
                options.AddPolicy("EmployeePolicy", policy =>
                {
                    policy.RequireRole("Employees");
                });
                options.AddPolicy("HRPolicy", policy =>
                {
                    policy.RequireRole("HR");
                });
                options.AddPolicy("ManagerPolicy", policy =>
                {
                    policy.RequireRole("Manager");
                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(GlobalExceptionHandlerCreator.CreateGlobalExceptionHandler(loggerFactory));
            app.UseIdentity();
            app.UseSession();

            ConfigureLoggers(app, env, loggerFactory);
            
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseCustomErrorPages();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureLoggers(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //add NLog to .NET Core
            loggerFactory.AddNLog();
            //Enable ASP.NET Core features (NLog.web)
            app.AddNLogWeb();
            //configure nlog.config in your project root
            env.ConfigureNLog("nlog.config");
        }
    }
}
