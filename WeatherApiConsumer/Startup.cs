using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApiConsumer.Configuration;
using WeatherApiConsumer.Services;

namespace WeatherApiConsumer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Extra security layer to restrict resource sharing to the specific domain
            services.AddCors();
            // I save all settings in AppConfig file 
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
            // Need to register IHttpClientFactory
            services.AddHttpClient();
            // Finally need to add IWeatherInterfaces services
            services.AddTransient<IWeatherServices, WeatherService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Allow all domain to access the resource
                app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // Here we inject the Exception handling services 
            // to check the bad request , this is centralize for all
            app.UseMiddleware(typeof(ExceptionHandler.ExceptionHandler));
            app.UseMvc();
        }
    }
}
