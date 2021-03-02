using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.LogModule
{
    /// <summary>
    /// 日志容器扩展
    /// </summary>
   public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// 扩展方法，在日志工厂添加自定义配置
        /// </summary>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="configuration">配置信息</param>
        /// <returns></returns>
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddFile(new LogegerSetting(configuration));
            return loggerFactory;
        }
        /// <summary>
        /// 注册LoggerProvider注册到日志工厂
        /// </summary>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="logegerSetting">日志配置信息</param>
        /// <returns></returns>
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, LogegerSetting logegerSetting)
        {
            loggerFactory.AddProvider(new LoggerProvider(logegerSetting));
            return loggerFactory;

        }
    }
}
