using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreCommon
{
   public static class BaseConfigure
    {
        private static IConfigurationBuilder _builder;

        private static object lockobj = new object(); 
        private static IConfigurationBuilder InitConfigurationBuilder(Action<IConfigurationBuilder> action=null)
        {
            lock (lockobj)
            {
                if (_builder == null)
                {
                    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
                    _builder = builder;
                }
                return _builder;

            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        public static IConfigurationRoot Configuration 
        {
            get 
            { 
            return InitConfigurationBuilder().Build();
            }
        }
        /// <summary>
        /// 读取配置文件的AppSetting
        /// </summary>
        public static IConfigurationSection AppSetting
        {
            get
            {
                return Configuration.GetSection("AppSetting");
            }
        }

    }
}
