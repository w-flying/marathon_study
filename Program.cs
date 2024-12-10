using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Marathon.Data;
using Marathon.Common;
using NLog;
using NLog.Web;

namespace Marathon
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var logger = LogManager.Setup().LoadConfigurationFromFile("CfgFile/nlog.config").GetCurrentClassLogger();
            var builder = WebApplication.CreateBuilder(args);
            try
            {
            builder.Services.AddDbContextFactory<MarathonContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MarathonContext") ?? throw new InvalidOperationException("Connection string 'MarathonContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //注册Session服务
            builder.Services.AddSession();

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomActionFilter));
                options.Filters.Add(typeof(CustomActionFilterAttribute));
            });
                // 配置使用NLog
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                builder.Host.UseNLog();
                //添加services.AddHttpContextAccessor();
                builder.Services.AddHttpContextAccessor();


                var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");
            //使用Session
            app.UseSession();

            app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "由于异常而停止程序");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
