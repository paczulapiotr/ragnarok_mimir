using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mimir.Database;
using Microsoft.EntityFrameworkCore;
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
            var connectionString = Environment.IsDevelopment()
                ? Configuration.GetConnectionString("Local")
                : Configuration.GetConnectionString("Azure");
            services.AddDbContext<MimirDbContext>(opts 
                => opts.UseSqlServer(connectionString));

            var authority = Environment.IsDevelopment() 
                ? Configuration["Security:AuthorityUrl:Local"] 
                : Configuration["Security:AuthorityUrl:Azure"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = !Environment.IsDevelopment();
                options.Audience = Configuration["Security:ApiName"];
            });
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                var clientOrigin = Environment.IsDevelopment()
                                ? Configuration["Security:ClientUrl:Local"]
                                : Configuration["Security:ClientUrl:Azure"];
                options.AddPolicy("default", policy =>
                {
                    policy
                        .WithOrigins(clientOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
                // this defines a CORS policy for IdentityServer requests
                var is4Origin = Environment.IsDevelopment()
                                ? Configuration["Security:AuthorityUrl:Local"]
                                : Configuration["Security:AuthorityUrl:Azure"];
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //using (var scope = app.ApplicationServices
                //    .GetRequiredService<IServiceScopeFactory>()
                //    .CreateScope())
                //{
                //    DataSeeder.Seed(scope.ServiceProvider);
                //}
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
                endpoints.MapControllers();
            });
        }
    }
}
