using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreCommon.LogModule
{
    public class LogegerSetting
    {
        private IConfiguration _configuration;
        public IChangeToken _changeToken { get; private set; }

        public static string dirPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dirPath))
                {
                    _dirPath = Directory.GetCurrentDirectory();
                }
                return _dirPath;
            }


        }
        private static string _dirPath;
        public LogegerSetting(IConfiguration configuration)
        {
            _configuration = configuration;
            this._changeToken = _configuration.GetReloadToken();
            _configuration = configuration;
        }

        public LogLevel DefaultLevel
        {
            get
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), _configuration["DefaultLevel"]);

            }
        }
        /// <summary>
        /// 刷新配置
        /// </summary>
        public void Reload()
        {
            _configuration = BaseConfigure.Configuration.GetSection("FileLogging");
        }
        /// <summary>
        /// 日志默认路径
        /// </summary>
        public string DefaultPath
        {
            get
            {
                return $"{dirPath}/{ _configuration["DefaultPath"]}/";
            }
        }
        /// <summary>
        /// 文件默认最大大小
        /// </summary>
        public int DefaultMaxMB
        {
            get
            {

                return int.Parse(_configuration["DefaultMaxMB"]);
            }
        }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string DefaultFileName
        {
            get
            {
                return _configuration["DefaultFileName"];
            }
        }

        public Tuple<bool, LogLevel> GetSwitch(string name)
        {
            var section = this._configuration.GetSection("LogLevel");
            if (section != null)
            {
                LogLevel level;
                if (Enum.TryParse(section[name], true, out level))
                    return new Tuple<bool, LogLevel>(true, level);
            }
            return new Tuple<bool, LogLevel>(false, LogLevel.None);
        }
        public Tuple<bool, string> GetDiretoryPath(string name)
        {
            var section = this._configuration.GetSection("Path");
            if (section != null)
            {
                var path = section[name];
                if (!String.IsNullOrEmpty(path))
                {
                    return new Tuple<bool, string>(true, $"{dirPath}\\{path}");
                }
            }
            return new Tuple<bool, string>(false, this.DefaultPath);
        }

        public Tuple<bool, string> GetFileName(string name)
        {
            var section = this._configuration.GetSection("FileName");
            if (section != null)
            {
                var path = section[name];
                if (!String.IsNullOrEmpty(path))
                {
                    return new Tuple<bool, string>(true, name);
                }
            }
            return new Tuple<bool, string>(false, this.DefaultFileName);
        }

    }
}
