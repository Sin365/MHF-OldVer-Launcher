using System;
using System.Diagnostics;
using System.Threading;

namespace MHF_OldVer_Launcher
{
    public class StaticComm
    {
        public static string HaoYue_Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //public static string strFullPath = System.Windows.Forms.Application.ExecutablePath;
        public static string strFileName => Process.GetCurrentProcess().MainModule.FileName;
        public static string strfulldir => strFileName.Substring(0, strFileName.LastIndexOf("\\")) + "\\";
        /// <summary>
        /// 当前程序名称（不包含扩展名）
        /// </summary>
        public static string ProgroamFileNameNotType = StaticComm.strFileName.Substring(0, StaticComm.strFileName.LastIndexOf("."));
        public static string GameFileName = "MHF.exe";
        public static string GameFileNameNotEXE = GameFileName.Substring(0, GameFileName.LastIndexOf('.'));
        public static bool IsCloseing = false;

        
        /// <summary>
        /// 联机娘被关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnGame_ProcessExit(object sender, EventArgs e)
        {
            if (!Main.bInGame)
                return;

            //如果是正在被执行关闭的状态 不重复触发
            if (StaticComm.IsCloseing)
                return;

            StaticComm.IsCloseing = true;

            //优先杀进程
            HaoYueHelper.KillProcessesForName(StaticComm.GameFileNameNotEXE);
            Thread.Sleep(200);
            HaoYueHelper.KillProcessesForName(StaticComm.GameFileNameNotEXE);

            Environment.Exit(-1);
            //最后杀掉自己
            HaoYueHelper.KillProcessesForName(StaticComm.ProgroamFileNameNotType);
        }
    }
}
