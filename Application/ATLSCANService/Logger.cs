using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ATLSCANService
{
    public static class Logger
    {
        public static void Log(Exception ex)
        {
            File.AppendAllText(
                @"D:\Cognizant Project\ZipSystem\Logs\service.log",
                $"{DateTime.Now} | {ex}\n"
            );
        }
    }

}