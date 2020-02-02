using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Routine.Api.Data;
using Routine.Api.Services;
using System;

namespace Routine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup => {
                setup.ReturnHttpNotAcceptable = true;   //默认值为 false 表示如果此Api的消费者在发送请求是，在请求头的"Accept" 设置为本Api不支持的媒体类型时，不会返回406状态码，而是返回他默认支持的数据类型（默认的数据类型为Json） , 如果设置为 true 就只会返回406

                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());  //为Api的输出数据类型添加xml类型  ，因为是Add操作，返回数据类型默认值还是Json
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter()); //如果要把添加的xml类型设置为默认的，就用Insert方法，设置Index值为0

            });
            //.AddXmlDataContractSerializerFormatters();  //.net 3.0后可以使用这种方法

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());    //添加自动映射的服务 需要AutoMapper.Extensions.Microsoft.DependencyInjection包

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            //AddScoped这种是针对每一次Http请求都重新生成服务的情况

            services.AddDbContext<RoutineDbContext>(option =>
            {
                option.UseSqlite("Data Source=routine.db");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //自定义的错误处理
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("发生了一个错误！");
                    });
                });
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
