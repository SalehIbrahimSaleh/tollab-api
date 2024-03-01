using SuperKotob.Admin.Utils.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;

namespace SuperKotob.Admin.Utils.Logging
{
    public class SeriLogger : ILogger
    {
        Logger InnerLogger { get; set; }

        public SeriLogger(IAppConfigurations appConfigurations)
        {
            var rollingFilePath = Task.Run(appConfigurations.LoggerRollingFilePath.GetValueAsync).Result;

            this.InnerLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(rollingFilePath)
                .CreateLogger();
        }


        public async Task LogErrorAsync(string message)
        {
            InnerLogger.Error(message);
        }

        public async Task LogAsync(Exception ex)
        {
            var message = ex.GetBaseException().Message;
            InnerLogger.ForContext<Exception>().Error(message);
        }


        public bool IsDebugEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return true;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return true;
            }
        }

        public ILogger CreateChildLogger(string loggerName)
        {
            return null;
        }

        public void Debug(Func<string> messageFactory)
        {

        }

        public void Debug(string message)
        {

        }

        public void Debug(string message, Exception exception)
        {

        }

        public void DebugFormat(string format, params object[] args)
        {

        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {

        }

        public void DebugFormat(Exception exception, string format, params object[] args)
        {

        }

        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void Error(Func<string> messageFactory)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
        }

        public void ErrorFormat(string format, params object[] args)
        {
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
        }

        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void Fatal(Func<string> messageFactory)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Fatal(string message, Exception exception)
        {
        }

        public void FatalFormat(string format, params object[] args)
        {
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
        }

        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void Info(Func<string> messageFactory)
        {
        }

        public void Info(string message)
        {
        }

        public void Info(string message, Exception exception)
        {
        }

        public void InfoFormat(string format, params object[] args)
        {
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void InfoFormat(Exception exception, string format, params object[] args)
        {
        }

        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void Warn(Func<string> messageFactory)
        {
        }

        public void Warn(string message)
        {
        }

        public void Warn(string message, Exception exception)
        {
        }

        public void WarnFormat(string format, params object[] args)
        {
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
        }

        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
        }
    }
}
