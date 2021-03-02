using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreCommon.LogModule
{
    public class FileLogger : ILogger
    {


        public LogLevel _minlevel;


        public string FileDiretoryPath { get; set; }
        public string FileNameTemplate { get; set; }

        public string _categoryName { get; private set; }

        class Disposable : IDisposable
        {
            public void Dispose()
            {

            }
        }
        Disposable _DisposableInstance = new Disposable();
        public FileLogger(string categoryName)
        {

            this._categoryName = categoryName;

        }

        /// <summary>
        /// 作用域，管理同一个逻辑下的日志
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {

            return _DisposableInstance;
        }

        /// <summary>
        /// 开启的记录日志最小等级
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return this._minlevel <= logLevel;
        }
        /// <summary>
        /// 消息写入
        /// </summary>
        /// <typeparam name="TState">写入对象的类型</typeparam>
        /// <param name="logLevel">写入对象的等级</param>
        /// <param name="eventId">事件的ID</param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
                return;

            var msg = formatter(state, exception);
            if (exception != null)
            {
                msg = $"{msg}:details:{exception.Message}";
            }
            Writ(logLevel, eventId, exception, msg);
        }
        private void Writ(LogLevel level, EventId eventId, Exception ex, string message)
        {
            DateTime now = DateTime.Now;
            initFile(now);

            File.AppendAllText($"{FileDiretoryPath}{now.ToString(FileNameTemplate)}.log", $"{now}\t{_categoryName}:{message}\r\n");
        }

        private void initFile(DateTime now)
        {
            if (!Directory.Exists(FileDiretoryPath))
            {
                Directory.CreateDirectory(FileDiretoryPath);
            }
            string fileName = $"{FileDiretoryPath}{now.ToString(FileNameTemplate)}.log";
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();

            }
        }

    }
}
