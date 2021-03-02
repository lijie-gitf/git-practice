using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.LogModule
{
    public class LoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// 存放日志实例的键值对集合
        /// </summary>
        private static Dictionary<string, FileLogger> _loggerKeys = new Dictionary<string, FileLogger>();

        /// <summary>
        /// 日志配置信息
        /// </summary>
        private LogegerSetting _configure;
        public LoggerProvider(LogegerSetting configure)
        {
            _configure = configure;
            //当配置变更，重新加载配置信息
            _configure._changeToken.RegisterChangeCallback(p =>
            {

                _configure.Reload();
                foreach (var item in LoggerProvider._loggerKeys.Keys)
                {

                    initLogSetting(item);

                }

            }, null);
        }
        /// <summary>
        /// 初始化log配置
        /// </summary>
        /// <param name="categoryName"></param>
        public void initLogSetting(string categoryName)
        {
            var loggerkeys = _loggerKeys.Keys;
            foreach (var key in loggerkeys)
            {
                var model = _loggerKeys[key];
                var switchV = _configure.GetSwitch(key);
                model._minlevel = _configure.DefaultLevel;
                if (switchV.Item1)
                {
                    model._minlevel = switchV.Item2;

                }
                var swtchpath = _configure.GetDiretoryPath(key);
                model.FileDiretoryPath = _configure.DefaultPath;
                if (swtchpath.Item1)
                {
                    model.FileNameTemplate = swtchpath.Item2;

                }
                var swtchTempName = _configure.GetFileName(key);
                model.FileNameTemplate = _configure.DefaultFileName;
                if (swtchTempName.Item1)
                {
                    model.FileNameTemplate = swtchTempName.Item2;

                }
            }
        }
        /// <summary>
        /// 创建一个日志对象
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            if (!LoggerProvider._loggerKeys.ContainsKey(categoryName))
            {
                var model = new FileLogger(categoryName);
                _loggerKeys.Add(categoryName, model);
                initLogSetting(categoryName);
            }

            return _loggerKeys[categoryName];
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
