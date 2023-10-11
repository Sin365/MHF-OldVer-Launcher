using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MHF_OldVer_Launcher
{
    public static class HaoYueHelper
    {

        public static void KillProcessesForName(string ProcessesName)
        {
            System.Diagnostics.Process[] OriginProcesses = System.Diagnostics.Process.GetProcessesByName(ProcessesName);
            if (OriginProcesses != null || OriginProcesses.Count() > 0)
            {
                foreach (var p in OriginProcesses)
                {
                    p.Kill();
                }
            }
        }

        public static bool CheckHaveProcessesForName(string ProcessesName)
        {
            System.Diagnostics.Process[] OriginProcesses = System.Diagnostics.Process.GetProcessesByName(ProcessesName);
            if (OriginProcesses != null || OriginProcesses.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
