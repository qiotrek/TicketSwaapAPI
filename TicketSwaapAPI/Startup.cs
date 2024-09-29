using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TicketSwaapAPI.Services;
using TicketSwaapAPI.Services.Infrastructure;
using TicketSwaapAPI.Services.Logic;
using TicketSwaapAPI.Services.Repositories;
using TicketSwaapAPI.Services.Security;
using TicketSwaapAPI.StoreModels;

namespace TicketSwaapAPI
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration=configuration;
        }

        public IConfiguration Configuration { get;}

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddResponseCaching();
            services.AddSingleton(new FirestoreTableNamesConfig().WithPrefixAndSuffix(Configuration.GetValue<string>("FirestoreTableNamesConfigPrefix"), Configuration.GetValue<string>("FirestoreTableNamesConfigSuffix")));
            services.AddTransient<FireStoreService>();
            services.AddSingleton<JwtGenerator>();
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<INewActionPropositionlogic, NewActionPropositionlogic>();
            services.AddTransient<INewActionsPropositionRepository, NewActionsPropositionRepository>();
            services.AddTransient<IActiveActionsRepository, ActiveActionsRepository>();
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<IAdminLogic, AdminLogic>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IActiveActionsLogic, ActiveActionsLogic>();
            services.AddTransient<IOffertRepository, OffertRepository>();
            services.AddTransient<IAdminPanelLogic, AdminPanelLogic>();
            services.AddTransient<IUserNotificationService, UserNotificationService>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSPADevClient",
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("SPIAppURL").Get<string[]>())
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddAuthentication(options => { 
                
                options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketSwaap", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description="JWT Authorization header using the Bearer scheme (Excamle: 'Bearer 12345afwfgweghw')",
                    Name="Authorization",
                    In= ParameterLocation.Header,
                    Type=SecuritySchemeType.ApiKey,
                    Scheme="Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                //var XmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,XmlFileName));

            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>  c.SwaggerEndpoint("/swagger/v1/swagger.json","TicketSwaap v1"));
            
                app.UseHttpsRedirection();
                app.UseCors("AllowSPADevClient");
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}
