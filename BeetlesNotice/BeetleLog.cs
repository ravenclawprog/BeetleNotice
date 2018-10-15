using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;


namespace BeetleLog
{
    public static class BugTrackingLogger
    {
       private static ILogger _log; 

       public static ILogger Logger
        {
         get { return _log; }
        }

       public static void InitLogger()
        {
            _log = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(".\\logs\\mylog.log")
            .CreateLogger();
        }

       public static void ClearTheLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
