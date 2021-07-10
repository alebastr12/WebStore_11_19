﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Logging;
using WebStore.Services.Database;
using WebStore.Services.Product;

namespace WebStore.ServiceHosting
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                opt =>
                {
                    opt.SwaggerDoc("v1", new Info { Title = "WebStore.API", Version = "v1" });
                    opt.IncludeXmlComments("WebStore.ServiceHosting.xml");
                    opt.IncludeXmlComments(@"bin\Debug\netcoreapp2.2\WebStore.Domain.xml");
                });

            services.AddTransient<WebStoreContextInitializer>();

            services.AddDbContext<WebStoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    //options.Lockout.AllowedForNewUsers = false;
                    // конфигурация cookies возможна здесь
                })
               .AddEntityFrameworkStores<WebStoreContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredUniqueChars = 3;

                cfg.Lockout.MaxFailedAccessAttempts = 10;
                cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                cfg.Lockout.AllowedForNewUsers = true;

                cfg.User.RequireUniqueEmail = false; // грабли!
            });

            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, CookieCartService>();
            services.AddScoped<IOrderService, SqlOrdersService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WebStoreContextInitializer db, ILoggerFactory log)
        {
            log.AddLog4Net();

            db.InitializeAsync().Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.API");
                    opt.RoutePrefix = string.Empty;
                });

            //app.Use(
            //    async (context, next) =>
            //    {
            //        context.Request.Headers.TryGetValue("secure_header", out var header_value);
            //        if (header_value != "key_value")
            //            context.Response.StatusCode = 403;
            //        else
            //            await next();
            //    });

            app.UseMvc();
        }
    }
}
