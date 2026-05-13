using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using Coreapi.Domain.SeedWork;
using Coreapi.Infrastructure.Audits;
using Coreapi.Infrastructure.BackgroundServices;
using Coreapi.Infrastructure.BaleBot;
using Coreapi.Infrastructure.Identity;
using Coreapi.Infrastructure.Notification;
using Coreapi.Infrastructures.BackgroundServices.BackgroundTasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUserManager, UserManagerService>();
            services.AddScoped<ISignInManager, SignInManagerService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAuditService, AuditService>();


            var dbPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory));
            services.AddDbContext<IdentityAppDbContext>(options => options.UseSqlServer(configuration["ConnectionString"]));
            services.AddDbContext<NotificationDbContext>(options => options.UseSqlite("Data Source=notifications.db"));

            // New AuditDbContext (PostGres)
            services.AddDbContext<AuditDbContext>(options => options.UseNpgsql(configuration["AuditConnectionString"]));


            services.AddAuthorization();

            services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityAppDbContext>();

            //services.AddIdentityApiEndpoints<ApplicationUser>()
            //.AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<IdentityAppDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            services.Configure<TokenManagement>(configuration.GetSection("tokenManagement"));
            var token = configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerJwt()
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                  ValidIssuer = token.Issuer,
                  ValidAudience = token.Audience,
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
              x.Events = new JwtBearerEvents
              {
                  OnAuthenticationFailed = context =>
                  {
                      context.Response.Headers.Add("IS-TOKEN-EXPIRED", context.GetType().ToString());
                      if (context.GetType() == typeof(SecurityTokenExpiredException))
                      {
                          
                          context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                      }

                      return Task.CompletedTask;
                  }
              };
          })
          .AddGoogle(options =>
          {

              options.ClientId = configuration["GoogleClientID"];
              options.ClientSecret = configuration["GoogleClientSecret"];
          })
          .AddMicrosoftAccount(options =>
          {

              options.ClientId = configuration["MicrosoftClientID"];
              options.ClientSecret = configuration["MicrosoftClientSecret"];
          });

            //services.AddSingleton<IConnectionMultiplexer>(sp =>
            //{
            //    var redisConfig = ConfigurationOptions.Parse(configuration["RedisConnection"], true);

            //    redisConfig.ResolveDns = true;
            //    redisConfig.ChannelPrefix = "Coreapi";
            //    //redisConfig.ChannelPrefix = "EhanCore";

            //    return ConnectionMultiplexer.Connect(redisConfig);
            //});

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUser, CurrentUserService>();

            services.AddScoped<IPaymentMelliService, PaymentMelliService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IElectCoService, ElectCoService>();
            services.AddScoped<IEmailService, EmailSender>();
            services.AddScoped<IRocketChatService, RocketChatService>();
            services.AddScoped<IMicrosoftService, MicrosoftService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddHostedService<QueuedHostedService>();
            services.AddHostedService<AutoOutProcessBackgroundService>();

            // Bale bot
            var baleToken = configuration["BaleService:BotToken"];
            if (!string.IsNullOrEmpty(baleToken))
            {
                services.AddHttpClient("BaleBot", client =>
                {
                    client.BaseAddress = new Uri($"https://tapi.bale.ai/bot{baleToken}/");
                    client.Timeout = TimeSpan.FromSeconds(40);
                });
                services.AddSingleton<BaleConversationStateManager>();
                services.AddScoped<IBaleService, BaleService>();
                services.AddHostedService<BalePollingService>();
            }

            //IHtmlSanitizer sanitizer = new HtmlSanitizer();
            //services.AddSingleton(sanitizer);
            services.AddSingleton<IHtmlSanitizer>(_ => new HtmlSanitizer());

            return services;
        }
    }
}