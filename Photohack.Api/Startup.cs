using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Photohack.Api.Extensions;
using Photohack.Models;
using Photohack.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Photohack.Api
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
            var parallelDotsSection = Configuration.GetSection("ParallelDots");
            services.Configure<ParallelDotsConfiguration>(parallelDotsSection);
            //var parallelDotsSectionSettings = parallelDotsSection.Get<ParallelDotsConfiguration>();

            services.AddScoped<IMusicService, MusicService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient("deezer", c =>
            {
                c.BaseAddress = new Uri("https://api.deezer.com/");
            });

            services.AddHttpClient("paralleldots", c =>
            {
                c.BaseAddress = new Uri("https://apis.paralleldots.com/");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CrazyFace api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/styles.css");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrazyFace");
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleWare>();

            app.UseMvc();
        }
    }
}
