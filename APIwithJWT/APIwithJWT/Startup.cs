using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Hosting;  
using Microsoft.Extensions.Configuration;  
using Microsoft.Extensions.DependencyInjection;  
using Microsoft.Extensions.Logging;  
using APIwithJWT.Models;

namespace APIwithJWT
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "mysite.com",
                    ValidAudience = "mysite.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MEs5UT4CsPP33527HeapNpZfaWGTUZ8Tzpn9eSUPGsXY997YpmrPKBg7V2G9h9egu2Pan34UVDrb3uaamnv5zstVTbPBrqQDSFeskETNUfvY6pSTNKpntFuj89BnmWUsAvRrXqQcesWDagzC6utRdyN8fqz2nykQGkUgGNUdyhXxHhdHSwvQF2FKsUxzhTxtHBFCyJUMthQqDtbGQeFgQrExLRuD4ZVZ5YRH6T2UBTjA694LnqUUsgUBAy7Lp62Y"))

                };
            });

            services.Add(new ServiceDescriptor(typeof(DiamondStoriesContext), new DiamondStoriesContext(Configuration.GetConnectionString("DefaultConnection"))));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
