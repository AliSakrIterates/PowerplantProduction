using PowerplantProduction.Hosting.Middlewares;
using PowerplantProduction.Services.ProductionPlan;
using Serilog;
using Microsoft.OpenApi.Models;
using AspNetCore.Serilog.RequestLoggingMiddleware;
using System.Reflection;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace PowerplantProduction.Hosting
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var handlerAssemblies = new[] { typeof(PowerPlanHandler).Assembly };
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(PowerPlanHandler).Assembly);
            });
            services.AddSingleton<LoggingExceptionHandler>();
            services.AddControllers()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
               });
            services.AddControllers();
            services.AddSingleton(Log.Logger);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "PowerplantProduction", Version = "v1" }); });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PowerplantProduction v1"));

            app.UseSerilogRequestLogging();
            app.UseMiddleware<LoggingExceptionHandler>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
