using AxibugLauncherInject;
using EasyHook;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MHF_OldVer_Launcher
{
    public partial class Main : Form
    {
        static string mHostToIPArr;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Config.LoadCfg("MHF-OldVer-Launcher.cfg");

            if (!Config.bLoaded)
            {
                btn_start.Enabled = false;
                Application.Exit();
                return;
            }

            mHostToIPArr = "";

            foreach (var cfg in Config.TargetServerCfg)
            {
                Console.WriteLine($"{cfg.ip}->{cfg.port}");
                mHostToIPArr += $"{cfg.ip}:{cfg.port}|";
            }

            label_s1.Text = $"{Config.TargetServerCfg[0].name}->{Config.TargetServerCfg[0].ip}:{Config.TargetServerCfg[0].port}";
            label_s2.Text = $"{Config.TargetServerCfg[1].name}->{Config.TargetServerCfg[1].ip}:{Config.TargetServerCfg[1].port}";
            label_s3.Text = $"{Config.TargetServerCfg[2].name}->{Config.TargetServerCfg[2].ip}:{Config.TargetServerCfg[2].port}";

            SetRun(false);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = true;
            btn_start.Text = "Running……";
            SetRun(StartProcessWithHook(StaticComm.strfulldir + "\\" + StaticComm.GameFileName));
        }

        void SetRun(bool value)
        {
            if (value)
            {
                this.label_State.Text = "State:Running……(Please do not close the launcher)";
                this.btn_start.Enabled = false;
                this.btn_start.Text = "Running……";
            }
            else
            {
                this.label_State.Text = "State:Ready";
                this.btn_start.Enabled = true;
                this.btn_start.Text = "START GAME";
                CurrPid = 0;
                bInGame = false;
            }
        }


        #region 运行时处理
        public static int CurrPid;
        public static bool bInGame;


        public static bool GetPidForProName(string ProcessName, out int targetPid)
        {
            Process[] process = Process.GetProcessesByName(ProcessName);
            if (process.Length > 0)
            {
                targetPid = process.FirstOrDefault().Id;
                return true;
            }
            else
            {
                targetPid = -1;
                return false;
            }
        }

        public static bool StartProcessWithHook(string path)
        {
            var pro = new Process();
            try
            {
                pro.StartInfo.FileName = path;
                pro.EnableRaisingEvents = true;
                //退出函数
                pro.Exited += new EventHandler(OnGame_ProcessExit);
                //pro.TotalProcessorTime
                pro.StartInfo.UseShellExecute = true;

                //参数
                //pro.StartInfo.Arguments = StaticComm.getLink(0);
                pro.Start();
                pro.WaitForInputIdle();
                //Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                try
                {
                    pro.Kill();
                }
                catch
                {
                }
                MessageBox.Show("FAIL TO START：" + ex.ToString());
                return false;
            }
            CurrPid = pro.Id;

            bool bCantInject = false;
            string bCantInjectStr = "";
            try
            {
                Thread.Sleep(400);
                if (!DoInjectByPid(pro.Id))
                {
                    bCantInject = true;
                }
            }
            catch (Exception ex)
            {
                bCantInject = true;
                bCantInjectStr = ex.ToString();
            }

            if (bCantInject)
            {
                CurrPid = 0;
                bInGame = false;
                pro.Kill();
                MessageBox.Show("FAIL TO Inject：" + bCantInjectStr);
                return false;
            }

            bInGame = true;
            return true;
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
        private static bool RegGACAssembly()
        {
            var dllName = "EasyHook.dll";
            var dllPath = System.IO.Path.Combine(StaticComm.strfulldir, dllName);
            if (System.Runtime.InteropServices.RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
                new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            Thread.Sleep(100);
            new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
            Thread.Sleep(100);
            if (System.Runtime.InteropServices.RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
                Console.WriteLine("{0} registered to GAC successfully.", dllName);
            else
            {
                Console.WriteLine("{0} registered to GAC failed.", dllName);
                return false;
            }

            dllName = "AxibugLauncherInject.dll";
            dllPath = System.IO.Path.Combine(StaticComm.strfulldir, dllName);
            if (System.Runtime.InteropServices.RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
                new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            Thread.Sleep(100);
            new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
            Thread.Sleep(100);
            if (System.Runtime.InteropServices.RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
                Console.WriteLine("{0} registered to GAC successfully.", dllName);
            else
            {
                Console.WriteLine("{0} registered to GAC failed.", dllName);
                return false;
            }
            return true;
        }


        private static bool InstallHookInternal(int processId)
        {
            try
            {
                var parameter = new HookParameter
                {
                    Msg = "已经成功注入目标进程",
                    //HostProcessId = RemoteHooking.GetCurrentProcessId(),
                    MasterProcessId = Process.GetCurrentProcess().Id,
                    RedirectorArrs = mHostToIPArr
                };

                RemoteHooking.Inject(
                    processId,
                    InjectionOptions.Default,
                    typeof(HookParameter).Assembly.Location,
                    typeof(HookParameter).Assembly.Location,
                    string.Empty,
                    parameter
                );
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                return false;
            }

            return true;
        }
        private static bool IsWin64Emulator(int processId)
        {
            var process = Process.GetProcessById(processId);
            if (process == null)
                return false;

            if ((Environment.OSVersion.Version.Major > 5)
                || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
            {
                bool retVal;

                return !(IsWow64Process(process.Handle, out retVal) && retVal);
            }

            return false; // not on 64-bit Windows Emulator
        }
        public static bool DoInjectByPid(int Pid)
        {
            var p = Process.GetProcessById(Pid);
            if (p == null)
            {
                Console.WriteLine("指定的进程不存在!");
                return false;
            }

            if (IsWin64Emulator(p.Id) != IsWin64Emulator(Process.GetCurrentProcess().Id))
            {
                var currentPlat = IsWin64Emulator(Process.GetCurrentProcess().Id) ? 64 : 32;
                var targetPlat = IsWin64Emulator(p.Id) ? 64 : 32;
                MessageBox.Show(string.Format("当前程序是{0}位程序，目标进程是{1}位程序，请调整编译选项重新编译后重试！", currentPlat, targetPlat));
                return false;
            }

            RegGACAssembly();
            InstallHookInternal(p.Id);
            return true;
        }
        #endregion


        /// <summary>
        /// 游戏被关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnGame_ProcessExit(object sender, EventArgs e)
        {
            if (!bInGame)
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
