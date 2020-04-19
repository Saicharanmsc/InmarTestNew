using System;
using log4net;

namespace InmarTest.DataAccess
{
    public class Logger
    {
        private ILog _Log;

        public Logger(string className)
        {
            log4net.Config.XmlConfigurator.Configure();
            _Log = LogManager.GetLogger(className);
        }

        public void Info(object message)
        {
            _Log.Info(message);
        }

        public void Debug(object message)
        {
            _Log.Debug(message);
        }

        public void Error(object message)
        {
            _Log.Error(message);
        }

        public void Fatal(object message)
        {
            _Log.Fatal(message);
        }
        public void LogException(Exception ex, object ActionName, object Action)
        {
            _Log.Error("-----------------------------------");
            _Log.Fatal(string.Format("Error in {0} {1}", ActionName, Action));
            _Log.Error("Inner Exception:" + ex.InnerException);
            _Log.Error("Exception:" + ex.Message);
            _Log.Error("Stack Trace:" + ex.StackTrace);
            _Log.Error("-----------------------------------");
        }
    }
}