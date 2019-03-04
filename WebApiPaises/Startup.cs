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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore; //DB
using WebApiPaises.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer; //Autenticacion
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApiPaises
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080"
                                        ).AllowAnyHeader().AllowAnyMethod();
                });
            });


            services.AddDbContext<Models.ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));//creamos db pasamos confiuraciones ApplicationDb...
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //sistema de usuarios por defecto ASP
            services.AddIdentity<ApplicationUsercs, IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>().
                AddDefaultTokenProviders();

            //configurar el servicio de autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options => 
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "tudominio.com",
                    ValidAudience = "tudominio.com",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Llave_secreta"])),
                    ClockSkew = TimeSpan.Zero//no permite ajustes temporales en el algorithmo que determinja si ha expirado
                });

            services.AddMvc().AddJsonOptions(ConfigureJson);
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                //app.UseCors("AllowAll");
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            if (!context.Paises.Any())
            {
                context.Paises.AddRange(new List<Pais>()
                {
                    new Pais(){Name = "China", Provincias = new List<Provincia>()
                    {
                        new Provincia(){Nombre = "Shenzhen"},
                        new Provincia(){Nombre = "hunan"}
                    } },

                    new Pais(){Name = "Mexico", Provincias = new List<Provincia>()
                    {
                        new Provincia(){Nombre = "Michoacan"},
                        new Provincia(){Nombre = "Nuevo Leon"}
                    } },
                    new Pais(){Name = "Argentina"}
                });
                context.SaveChanges();
            }



        }

        //private void ConfigureServicesForCors(IServiceCollection services)
        //{
        //    services.AddCors(options => options.AddPolicy("AllowAll", builder =>
        //    {
        //        builder
        //            //.WithOrigins("http://localhost:8080") //AllowSpecificOrigins;  
        //            //.WithOrigins("http://localhost:4456", "http://localhost:4457") //AllowMultipleOrigins;  
        //            .AllowAnyOrigin() //AllowAllOrigins;  
        //                              //.WithMethods("GET") //AllowSpecificMethods;  
        //                              //.WithMethods("GET", "PUT") //AllowSpecificMethods;  
        //                              //.WithMethods("GET", "PUT", "POST") //AllowSpecificMethods;  
        //                              //.WithMethods("GET", "PUT", "POST", "DELETE") //AllowSpecificMethods;  
        //            .AllowAnyMethod() //AllowAllMethods;  
        //                              //.WithHeaders("Accept", "Content-type", "Origin", "X-Custom-Header"); //AllowSpecificHeaders;  
        //            .AllowCredentials()
        //            .AllowAnyHeader(); //AllowAllHeaders;  
        //    })
        //    );



        //}

        }
}
