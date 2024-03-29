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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Middleware;
using CoreCommon.LogModule;
using CoreCommon.PushMessage;
using WebCore.Filter.PermissionFilter;

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

            //连接数据库
            //services.AddDbContext<CoreDbContext>(option => {
            //    option.UseSqlServer(BaseConfigure.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            //});
            
            services.AddLogging();
            //注入EF仓储
            services.AddScoped<ICoreRepository>(option => {
            if (BaseConfigure.Configuration["DataBaseType"] == "SqlServer")
            {
                    return new CoreSqlRepository(new CoreDbContext(p=> {
                        p.UseSqlServer(BaseConfigure.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
                        
                    }));
                }
                else {
                    //其它的数据库
                    return new CoreSqlRepository(option.GetRequiredService<CoreDbContext>());
                }
            });
            //注册身份验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            { 
                option.LoginPath = new PathString("/Account/Index");//身份验证未通过时的跳转页面
                option.LogoutPath = new PathString("/Account/LoginOut");//登出时跳转的页面
                option.Cookie.Name = "userCookie"; //用户登录标识的cookie名称
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                //格式化日期时间格式
                //options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //数据格式首字母小写
                //options.JsonSerializerOptions.PropertyNamingPolicy =JsonNamingPolicy.CamelCase;
                //数据格式原样输出
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //取消Unicode编码
                //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //忽略空值
               // options.JsonSerializerOptions.IgnoreNullValues = true;
                //允许额外符号
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });

            #region 业务层注入
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region 消息队列注入
            
            services.AddSingleton<IConnectionChannelPool>(p=> {
                return new ConnectionChannelPool(new RabbitMQOptions());
            });
            services.AddSingleton<IPushMessageService>(p=> {
                return new PushMessageService(p.GetRequiredService<IConnectionChannelPool>(),new PublicOptions(){Exchange="lijie_Exchange",Queue="jietest0304"});
            });
            #endregion
            #region 过滤器
            services.AddSingleton<PermissionOption>(p => new PermissionOption
            {
                LoginAction= "/Account/Index",
                NoPermissionAction= "/Account/Index",
                UserPerssions=new List<UserPermission> { 
                    new UserPermission { RoleCode = "0001", Url = "/Home/Index" }, 
                    new UserPermission { RoleCode = "0001", Url = "/Navigation/GetMenuByRole" }, 
                    new UserPermission { RoleCode = "0001", Url = "/Navigation/Index" } }
            });
          
            services.AddControllersWithViews(Options => Options.Filters.Add<PermissionFilterAttribute>());
            #endregion
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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
            //配置自定义日志
            loggerFactory.AddFile(BaseConfigure.Configuration.GetSection("FileLogging"));

            //添加身份验证中间件
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            //异常处理中间件
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
