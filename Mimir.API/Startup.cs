using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mimir.Database;
using Microsoft.EntityFrameworkCore;
using Mimir.API.Hubs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mimir.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceManager.RegisterServices(services);
            services.AddControllers();
            services.AddSignalR();
            var connectionString = Configuration.GetConnectionString("Data");
            services.AddDbContext<MimirDbContext>(opts 
                => opts.UseSqlServer(connectionString));

            var authority = Configuration["Security:AuthorityUrl"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = !Environment.IsDevelopment();
                options.Audience = Configuration["Security:ApiName"];

                // SignalR auth event
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrWhiteSpace(accessToken))
                            context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                var clientOrigin = Configuration["Security:ClientUrl"];
                options.AddPolicy("default", policy =>
                {
                    policy
                        .WithOrigins(clientOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
                // this defines a CORS policy for IdentityServer requests
                var is4Origin = Configuration["Security:AuthorityUrl"];
                options.AddPolicy("IdentityServer", policy =>
                {
                    policy
                        .WithOrigins(is4Origin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.MigrateDatabase(logger);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BoardSynchronizationHub>("/ws/synchronize");
                endpoints.MapControllers();
            });
        }
    }
}
