using LiteDB;
using Manchu.Authentication;
using Manchu.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Manchu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manchu API");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ConnectionString(Configuration.GetConnectionString("Manchu")));
            services.AddSwaggerGen(options =>
            {
                var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "Basic",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Basic",
                    Description = "Provide your username and password.",

                    Reference = new OpenApiReference
                    {
                        Id = "Basic",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                options.SchemaFilter<EnumSchemaFilter>();
                options.OperationFilter<AuthorizeHeaderOperationFilter>();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Manchu API",
                    Version = "v1",
                    Description = "Description for the API goes here.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sven Thiel",
                        Email = "m6thsv2@googlemail.com",
                        Url = new Uri("https://infinite-trajectory.de/"),
                    },
                });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "BASIC";
                options.DefaultChallengeScheme = "BASIC";
            })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("Basic", null)
    .AddPolicyScheme("BASIC", "BASIC", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            return "Basic";
        };
    });
            services.AddControllersWithViews();
        }
    }
}