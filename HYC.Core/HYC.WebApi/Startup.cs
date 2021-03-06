﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HYC.Services.Implments;
using HYC.Services.Interface;
using HYC.WebApi.AuthHelper;
using HYC.WebApi.swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace HYC.WebApi
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
            //依赖注入模块(HYC.Service,HYC.Repository)
            services.AddTransient<IUserService, UserService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(jsonOption => {
                jsonOption.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //JSON返回时间格式处理
                jsonOption.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //JSON返回大小写处理
                jsonOption.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            #region==signalR跨域和配置==
            services.AddCors(options => options.AddPolicy("SignalrCore",
            builder =>
            {
                var corsurls = Configuration.GetSection("SignalrCors")["default"].Split(',');
                builder.WithOrigins(corsurls)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials();
            }));
            services.AddSignalR();
            #endregion

            services.AddCors(options => options.AddPolicy("Vue", builder =>
            {
                var corsurls = Configuration.GetSection("VueCors")["default"].Split(',');
                builder.WithOrigins(corsurls)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials();
            }));

            #region==swagger配置==
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Huang Leo",
                        Email = "281010937@qq.com",
                        Url = "http://www.cnblogs.com/RayWang"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<SwaggerFilter>();
                c.DocumentFilter<HiddenFilter>();
                c.IgnoreObsoleteActions();

                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    //默认是Bearer+空格+token,本项目用中间件把Bearer+空格去掉了
                    //默认是Bearer+空格+token,本项目用中间件把Bearer+空格去掉了
                    Description = "JWT Bearer 授权 \"Authorization:token\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
            #endregion

            #region ==jwt验证==
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });

            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    var audienceConfig = Configuration.GetSection("Audience");
                    var secret = audienceConfig["Secret"];
                    var issuer = audienceConfig["Issuer"];
                    var audience= audienceConfig["Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = audience,//Audience
                        ValidIssuer = issuer,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))//拿到SecurityKey
                    };
                });
            #endregion

            #region==Redis==
            services.AddDistributedRedisCache(option => {
                var redisip = Configuration.GetSection("Redis")["ip"];
                var redisname = Configuration.GetSection("Redis")["name"];
                option.Configuration = redisip;
                option.InstanceName = redisname;
            });
            #endregion
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
                app.UseHsts();
            }


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseMiddleware<TokenAuth>();//TokenAuth类注册为中间件

            //跨域支持
            app.UseCors("Vue");
            app.UseCors("SignalrCore");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHubs>("/chatHubs");
            });


            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
