using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Phonebook.Services.Report.Consumers;
using Phonebook.Services.Report.Services;
using Phonebook.Services.Report.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report
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
            #region RabbitMQ
            services.AddMassTransit(s =>
            {
                s.AddConsumer<PrepareReportDataCommandConsumer>();

                s.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ReceiveEndpoint("report-service", e =>
                    {
                        e.ConfigureConsumer<PrepareReportDataCommandConsumer>(context);
                    });
                });
            });
            services.AddMassTransitHostedService();
            #endregion

            #region DI
            services.AddScoped<IReportService, ReportService>();
            #endregion

            #region DB
            services.Configure<DBSettings>(Configuration.GetSection("DBSettings"));
            services.AddSingleton<IDBSettings>(s =>
            {
                return s.GetRequiredService<IOptions<DBSettings>>().Value;
            });
            #endregion

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Phonebook.Services.Report", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phonebook.Services.Report v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
