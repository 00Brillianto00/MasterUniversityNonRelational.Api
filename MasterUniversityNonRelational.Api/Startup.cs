//using MasterUniversityNonRelational.API.Services;
using MasterUniversityNonRelational.Api.Services;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterUniversityNonRelational.Api
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

            services.AddControllers();
            services.RegisterService(Configuration);
            services.AddSwaggerGen((opt) =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API | MasterUniversityNonRelational",
                    Version = "v1",
                    Description = "MasterUniversity untuk Non-Relational Database API ",
                    //Contact = new OpenApiContact()
                    //{
                    //    Name = "brillianto.oktaviga@gmail.com",
                    //    Url = new System.Uri(""),
                    //}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c => c.SerializeAsV2 = true);
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API | Master University Non Relational API"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
