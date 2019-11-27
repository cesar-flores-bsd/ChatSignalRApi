﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRSqlDependecy.Hub;
using System;

namespace SignalRSqlDependecy
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
            //services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //    .WithOrigins("http://localhost:4200", "http://ap.bsdenterprise.com:8095/ChatEjemplo");
            //}));

            services.AddSignalR();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors("CorsPolicy");
            app.UseSignalR(s =>
            {
                var desireProtocols = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                
                s.MapHub<NotificacionHub>("/NotificacionHub", (options) =>
                {
                    options.Transports = desireProtocols;
                    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(3);
                    
                });
            });
            app.UseMvc();
        }
    }
}
