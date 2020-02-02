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
                setup.ReturnHttpNotAcceptable = true;   //Ĭ��ֵΪ false ��ʾ�����Api���������ڷ��������ǣ�������ͷ��"Accept" ����Ϊ��Api��֧�ֵ�ý������ʱ�����᷵��406״̬�룬���Ƿ�����Ĭ��֧�ֵ��������ͣ�Ĭ�ϵ���������ΪJson�� , �������Ϊ true ��ֻ�᷵��406

                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());  //ΪApi����������������xml����  ����Ϊ��Add������������������Ĭ��ֵ����Json
                //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter()); //���Ҫ����ӵ�xml��������ΪĬ�ϵģ�����Insert����������IndexֵΪ0

            });
            //.AddXmlDataContractSerializerFormatters();  //.net 3.0�����ʹ�����ַ���

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());    //����Զ�ӳ��ķ��� ��ҪAutoMapper.Extensions.Microsoft.DependencyInjection��

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            //AddScoped���������ÿһ��Http�����������ɷ�������

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
                //�Զ���Ĵ�����
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("������һ������");
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
