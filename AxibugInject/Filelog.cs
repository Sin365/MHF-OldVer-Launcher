using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AxibugLauncherInject
{
    public class FileLog
    {
        public static string logpath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\\AxibugLauncherInject.txt";
        public static void Log(string sourceStr)
        {
            try
            {
                File.AppendAllText(logpath, "\n"+DateTime.Now.ToString("yyyyMMdd HH:mm:ss: ") + sourceStr);
            }
            catch { }
        }
    }
}
