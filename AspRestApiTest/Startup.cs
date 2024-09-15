using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AspRestApiTest
{
    using AspRestApiTest.Data;
    using AspRestApiTest.Features.Exceptions;
    using AspRestApiTest.Features.Logger;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.ListenAnyIP(Configuration.GetValue<int>("Kestrel:Port"));
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddProvider(new CustomLoggerProvider());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AspRestApiTest",
                    Version = "v1"
                });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddTransient<ExceptionHandlingMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspRestApiTest API V1");
                });
            }

            app.UseRouting();
            app.UseAuthorization();

            dbContext.Database.Migrate();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "api/{area:exists}/{controller}/{action=Index}/{id?}"
                );
            });
        }
    }
}
