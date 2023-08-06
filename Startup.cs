using LiteDB;
using Manchu.Authentication;
using Manchu.Configurations;
using Manchu.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Manchu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtConfiguration = configuration.GetSection("JWT").Get<JwtConfiguration>();
        }

        public IConfiguration Configuration { get; }
        public JwtConfiguration JwtConfiguration { get; }

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

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
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
                        Email = string.Empty,
                        Url = new Uri("https://coderjony.com/"),
                    },
                });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "BASIC_OR_JWT";
                options.DefaultChallengeScheme = "BASIC_OR_JWT";
            })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("Basic", null)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = JwtConfiguration.ValidateIssuer,
            ValidateAudience = JwtConfiguration.ValidateAudience,
            ValidAudience = JwtConfiguration.ValidAudience,
            ValidIssuer = JwtConfiguration.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey ?? ""))
        };
    })
    .AddPolicyScheme("BASIC_OR_JWT", "BASIC_OR_JWT", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            string authorization = context.Request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Basic ", StringComparison.InvariantCultureIgnoreCase))
                return "Basic";

            return JwtBearerDefaults.AuthenticationScheme;
        };
    });
            services.AddControllersWithViews();
        }

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
    }
}