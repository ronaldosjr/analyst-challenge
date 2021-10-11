using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radix.Infra.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Radix.Api.AutoMapperProfile;
using Radix.Domain.Interfaces;
using Radix.Service;
using Radix.Api.HubConfig;
using Microsoft.OpenApi.Models;
using Radix.Infra.Data.Interfaces;
using Radix.Infra.Data.Repository;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace Radix.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://192.168.0.142:4200", "http://localhost:4200", "http://localhost:5000", "https://localhost:5001")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddAutoMapper(typeof(RadixProfile));
            services.AddMemoryCache();

            services.AddSignalR().AddNewtonsoftJsonProtocol(options => options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<IMongoConnect, RadixContext>();
            services.AddSingleton<ISensorEventService, SensorEventService>();
            services.AddSingleton<ISensorEventRepository, SensorEventRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Radix Event API",
                    Description = "Rest API para controle de eventos emitidos por sensores",
                    Contact = new OpenApiContact
                    {
                        Name = "Ronaldo Ribeiro",
                        Email = "ronaldo.ribeiro@outlook.com"
                    }
                });
            });

            services.AddDirectoryBrowser();
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist/radix-front");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Radix Event API");
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SensorEventHub>("/sensors");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSpa(o =>
            {
                o.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    o.UseAngularCliServer("start");
                    o.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
