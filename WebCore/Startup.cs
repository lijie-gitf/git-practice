using CoreBll.UserService;
using CoreCommon;
using CoreEntirty;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Middleware;

namespace WebCore
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
            
            //�������ݿ�
            //services.AddDbContext<CoreDbContext>(option => {
            //    option.UseSqlServer(BaseConfigure.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            //});

            //ע��EF�ִ�
            services.AddScoped<ICoreRepository>(option => {
            if (BaseConfigure.Configuration["DataBaseType"] == "SqlServer")
            {
                    return new CoreSqlRepository(new CoreDbContext(p=> {
                        p.UseSqlServer(BaseConfigure.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
                        
                    }));
                }
                else {
                    //���������ݿ�
                    return new CoreSqlRepository(option.GetRequiredService<CoreDbContext>());
                }
            });
            //ע�������֤
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = new PathString("/Account/Login");//�����֤δͨ��ʱ����תҳ��
                option.LogoutPath = new PathString("/Account/LoginOut");//�ǳ�ʱ��ת��ҳ��
                option.Cookie.Name = "userCookie"; //�û���¼��ʶ��cookie����
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                //��ʽ������ʱ���ʽ
                //options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //���ݸ�ʽ����ĸСд
                //options.JsonSerializerOptions.PropertyNamingPolicy =JsonNamingPolicy.CamelCase;
                //���ݸ�ʽԭ�����
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //ȡ��Unicode����
                //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //���Կ�ֵ
               // options.JsonSerializerOptions.IgnoreNullValues = true;
                //����������
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                //�����л����������������Ƿ�ʹ�ò����ִ�Сд�ıȽ�
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

            #region ҵ���ע��
            services.AddTransient<IUserService, UserService>();
            #endregion

            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
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
            //��������֤�м��
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //�쳣�����м��
            app.UseExecptionMiddleware();
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
