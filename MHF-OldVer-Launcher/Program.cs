using System;
using System.Threading;
using System.Windows.Forms;

namespace MHF_OldVer_Launcher
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Mutex mutex;
            bool ret;
            mutex = new System.Threading.Mutex(true, "MHF_OldVer_Launcher", out ret);

            if (!ret)
            {
                MessageBox.Show("Do not run MHF_OldVer_Launcher.exe repeatedly!", "MHF_OldVer_Launcher");
                Application.Exit();
                return;
            }
             
            System.Diagnostics.Process[] mhfProcesses = System.Diagnostics.Process.GetProcessesByName("mhf");
            if (mhfProcesses.Length > 0)
            {
                MessageBox.Show("Please close mhf.exe first", "MHF_OldVer_Launcher");
                Application.Exit();
                return;
            }

            if (!System.IO.File.Exists(StaticComm.strfulldir + "\\" + StaticComm.GameFileName))
            {
                MessageBox.Show("[MHF-OldVer-Launcher.exe]Must be in the game root directory and ensure the presence of[" + StaticComm.GameFileName + "]", "MHF_OldVer_Launcher");
                Environment.Exit(-1);
                return;
            }

            //指定 进程退出卸载的事件
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(StaticComm.OnGame_ProcessExit);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        /// <summary>
        /// 联机娘被关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnGame_ProcessExit(object sender, EventArgs e)
        {
            if (!MHF_OldVer_Launcher.Main.bInGame)
                return;

            //如果是正在被执行关闭的状态 不重复触发
            if (StaticComm.IsCloseing)
                return;

            StaticComm.IsCloseing = true;

            //优先杀进程
            HaoYueHelper.KillProcessesForName(StaticComm.GameFileNameNotEXE);
            Thread.Sleep(200);
            HaoYueHelper.KillProcessesForName(StaticComm.GameFileNameNotEXE);

            try
            {
            }
            catch
            {

            }

            Environment.Exit(-1);
            //最后杀掉自己
            HaoYueHelper.KillProcessesForName(StaticComm.ProgroamFileNameNotType);
        }
    }
}
