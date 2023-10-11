using System;
using System.Text;
using System.Runtime.InteropServices;
using EasyHook;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using static AxibugLauncherInject.ws2_32;
using System.Security.Permissions;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace AxibugLauncherInject
{
    [Serializable]
    public class HookParameter
    {
        public string Msg { get; set; }
        //public int HostProcessId { get; set; }
        public int MasterProcessId { get; set; }
        public string RedirectorArrs { get; set; }
    }

    public class Main : IEntryPoint
    {
        public LocalHook MessageBoxAHook = null;
        public LocalHook GetHostByNameHook = null;
        public LocalHook GetHostByAddrHook = null;
        public LocalHook gethostnameHook = null;
        public LocalHook connectHook = null;
        public LocalHook CreateWindowExHook = null;
        public LocalHook CopyFileAHook = null;
        public LocalHook CreateFileAHook = null;

        static List<IPPortConfig> mHostToIPCfg = new List<IPPortConfig>();

        class IPPortConfig
        {
            public string ip;
            public uint port;
        }

        public Main(
            RemoteHooking.IContext context,
            string channelName
            , HookParameter parameter
            )
        {
            ConsoleShow.Log($"_INIT");
            //ConsoleShow.Log($"_INIT P:" + parameter.RedirectorArrs);
            ConsoleShow.Log("MasterProcessId->" + parameter.MasterProcessId);
            if (string.IsNullOrEmpty(parameter.RedirectorArrs))
            {
                ConsoleShow.Log($"参数接收异常");
#if DEBUG
                MessageBox.Show("我杀我自己");
#endif
                try
                {
                    Process.GetProcessById(parameter.MasterProcessId).Kill();
                }
                catch
                {

                }
                Process.GetCurrentProcess().Kill();
                return;
            }

            //ConsoleShow.Log($"AllCfg->{parameter.RedirectorArrs}");
            string[] RedirectorArrs = parameter.RedirectorArrs.Split('|');
            try
            {
                for (int i = 0; i < RedirectorArrs.Length; i++)
                {
                    string line = RedirectorArrs[i].Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    ConsoleShow.Log($"LoopLoadCfg [{i}]->{RedirectorArrs[i]}");
                    string[] arr = RedirectorArrs[i].Trim().Split(':');
                    //ConsoleShow.Log($"LoopLoadCfg [{i}] arr lenght->{arr.Length}");
                    mHostToIPCfg.Add(new IPPortConfig
                    {
                        ip = arr[0].Trim(),
                        port = Convert.ToUInt32(arr[1].Trim())
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            ConsoleShow.Log($"_INIT_END PARAM->{mHostToIPCfg.Count}");
#if DEBUG
            //ConsoleShow.Log(parameter.Msg + ",并加载:" + mHostToIPCfg.Count + "个重定向配置", "Hooked");
#endif
        }

        public void Run(
            RemoteHooking.IContext context,
            string channelName
            , HookParameter parameter
            )
        {
            //MessageBox.Show($"Pid:"+Process.GetCurrentProcess().Id);
            //MessageBox.Show($"Pid:" + Process.GetCurrentProcess().MainModule.FileName);

            try
            {
                ConsoleShow.Log($"Hook函数user32.dll->MessageBoxA");
                MessageBoxAHook = LocalHook.Create(
                    LocalHook.GetProcAddress("user32.dll", "MessageBoxA"),
                    new DMessageBoxA(MessageBoxA_Hooked),
                    this);
                MessageBoxAHook.ThreadACL.SetExclusiveACL(new int[1]);

                ConsoleShow.Log($"Hook函数ws2_32.dll->gethostbyname");
                GetHostByNameHook = LocalHook.Create(
                    LocalHook.GetProcAddress("ws2_32.dll", "gethostbyname"),
                    new DGetHostByName(GetHostByName_Hooked),
                    this);
                GetHostByNameHook.ThreadACL.SetExclusiveACL(new int[1]);

                //ConsoleShow.Log($"Hook函数ws2_32.dll->gethostbyaddr");
                //GetHostByAddrHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("ws2_32.dll", "gethostbyaddr"),
                //    new Dgethostbyaddr(gethostbyaddr_Hooked),
                //    this);
                //GetHostByAddrHook.ThreadACL.SetExclusiveACL(new int[1]);

                //ConsoleShow.Log($"Hook函数ws2_32.dll->gethostname");
                //gethostnameHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("ws2_32.dll", "gethostname"),
                //    new Dgethostname(gethostname_Hooked),
                //    this);
                //gethostnameHook.ThreadACL.SetExclusiveACL(new int[1]);

                ConsoleShow.Log($"Hook函数ws2_32.dll->connect");
                connectHook = LocalHook.Create(
                    LocalHook.GetProcAddress("ws2_32.dll", "connect"),
                    new Dconnect(connect_Hooked),
                    this);
                connectHook.ThreadACL.SetExclusiveACL(new int[1]);

                //ConsoleShow.Log($"Hook函数KERNEL32.dll->CopyFileA");
                //CopyFileAHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("KERNEL32.dll", "CopyFileA"),
                //    new DCopyFileA(CopyFileA_Hooked),
                //    this);
                //CopyFileAHook.ThreadACL.SetExclusiveACL(new int[1]);

                ConsoleShow.Log($"Hook函数KERNEL32.dll->CreateFileA");
                CreateFileAHook = LocalHook.Create(
                    LocalHook.GetProcAddress("KERNEL32.dll", "CopyFileA"),
                    new DCreateFileA(CreateFileA_Hooked),
                    this);
                CreateFileAHook.ThreadACL.SetExclusiveACL(new int[1]);

                //ConsoleShow.Log($"Hook函数user32.dll->CreateWindowEx");
                //CreateWindowExHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("user32.dll", "CreateWindowEx"),
                //    new DCreateWindowEx(CreateWindowEx_Hooked),
                //    this);
                //CreateWindowExHook.ThreadACL.SetExclusiveACL(new int[1]);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                return;
            }

            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                }
            }
            catch
            {

            }
        }

        #region MessageBoxA

        [DllImport("user32.dll", EntryPoint = "MessageBoxA", CharSet = CharSet.Ansi)]
        public static extern IntPtr MessageBoxA(int hWnd, string text, string caption, uint type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr DMessageBoxA(int hWnd, string text, string caption, uint type);

        static IntPtr MessageBoxA_Hooked(int hWnd, string text, string caption, uint type)
        {
            return MessageBoxA(hWnd, text, "MHF:" + caption, type);
        }

        #endregion

        #region gethostbyname

        [DllImport("ws2_32.dll", EntryPoint = "gethostbyname", CharSet = CharSet.Ansi)]
        public static extern IntPtr gethostbyname(String name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr DGetHostByName(String name);
        static IntPtr GetHostByName_Hooked(
            String name)
        {
            try
            {
                ConsoleShow.Log($"gethostbyname[调用]name->{name}");
                if (name.ToLower() == "sign-mhf.capcom-networks.jp")
                {
                    ConsoleShow.Log($"Temp Value -> 255.255.255.255");
                    name = "255.255.255.255";
                }
            }
            catch
            {
            }

            // call original API...
            return gethostbyname(
                name);
        }
        #endregion

        #region gethostname

        [DllImport("ws2_32.dll", SetLastError = true)]
        static extern int gethostname(StringBuilder name, int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate int Dgethostname(StringBuilder name, int length);
        static int gethostname_Hooked(StringBuilder name, int length)
        {
            ConsoleShow.Log($"gethostname[调用]name->{name} length->{length}");
            // call original API...
            return gethostname(name, length);
        }
        #endregion

        #region gethostbyaddr

        [DllImport("ws2_32.dll", EntryPoint = "gethostbyaddr", CharSet = CharSet.Ansi)]
        public static extern IntPtr gethostbyaddr(String addr, int len, int type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr Dgethostbyaddr(String addr, int len, int type);
        static IntPtr gethostbyaddr_Hooked(String addr, int len, int type)
        {
            ConsoleShow.Log($"gethostbyaddr[调用]addr->{addr} len->{len} type->{type}");
            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                ConsoleShow.Log("gethostbyaddr[访问]：" + addr);
            }
            catch
            {
            }

            // call original API...
            return gethostbyaddr(addr, len, type);
        }
        #endregion

        #region connect

        //[StructLayout(LayoutKind.Sequential)]
        //public struct sockaddr_in6
        //{
        //    public short sin6_family;
        //    public ushort sin6_port;
        //    public uint sin6_flowinfo;
        //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        //    public byte[] sin6_addr;
        //    public uint sin6_scope_id;
        //}
        [DllImport("Ws2_32.dll")]
        public static extern int connect(IntPtr SocketHandle, ref sockaddr_in_old addr, int addrsize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate int Dconnect(IntPtr SocketHandle, ref sockaddr_in_old addr, int addrsize);
        static int connect_Hooked(IntPtr SocketHandle, ref sockaddr_in_old addr, int addrsize)
        {
            ConsoleShow.Log($"connect[调用]SocketHandle->{SocketHandle} addr->{addr} addrsize->{addrsize}");
            ConsoleShow.Log($"connect sockaddr_in 详情 :sin_family->{addr.sin_family} sin_addr->{IntToIp_descPlan(addr.sin_addr)} sin_port->{GetPort(addr.sin_port)}");

            if (IntToIp_descPlan(addr.sin_addr) == "255.255.255.255")
            {
                switch (GetPort(addr.sin_port))
                {
                    case 53312:
                        ConsoleShow.Log($"Connect MHFServer1 SetIP:{mHostToIPCfg[0].ip} Port{mHostToIPCfg[0].port}");
                        addr.sin_addr = IpToInt_descPlan(mHostToIPCfg[0].ip);
                        addr.sin_port = (ushort)GetPort_DescPlan((ushort)mHostToIPCfg[0].port);
                        break;
                    case 53322:
                        ConsoleShow.Log($"Connect MHFServer2 SetIP:{mHostToIPCfg[1].ip} Port{mHostToIPCfg[1].port}");
                        addr.sin_addr = IpToInt_descPlan(mHostToIPCfg[1].ip);
                        addr.sin_port = (ushort)GetPort_DescPlan((ushort)mHostToIPCfg[1].port);
                        break;
                    case 53332:
                        ConsoleShow.Log($"Connect MHFServer3 SetIP:{mHostToIPCfg[2].ip} Port{mHostToIPCfg[2].port}");
                        addr.sin_addr = IpToInt_descPlan(mHostToIPCfg[2].ip);
                        addr.sin_port = (ushort)GetPort_DescPlan((ushort)mHostToIPCfg[2].port);
                        break;
                }
            }
            // call original API...
            return connect(SocketHandle, ref addr, addrsize);
        }

        //数字转化为IP
        static string IntToIp_descPlan(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ipInt & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 24) & 0xFF);

            return sb.ToString();
        }
        static int GetPort(ushort Tbed)
        {
            if (Tbed < 256)
                return Tbed;

            byte gao = (byte)(Tbed >> 8);
            byte di = (byte)(Tbed & 0xff);

            ushort a = (ushort)(gao << 8);
            ushort b = (ushort)di;
            //ushort newBed = (ushort)(a | di);

            ushort newT = (ushort)(gao | di << 8);
            return newT;
        }
        static int GetPort_DescPlan(ushort Tbed)
        {
            if (Tbed < 256)
                return Tbed;
            byte di = (byte)(Tbed >> 8);
            byte gao = (byte)(Tbed & 0xff);

            ushort newT = (ushort)(gao << 8 | di);
            return newT;
        }
        //IP转数字
        static UInt32 IpToInt_descPlan(string ip)
        {
            string[] strSrc = ip.Split('.');
            ip = $"{strSrc[3]}.{strSrc[2]}.{strSrc[1]}.{strSrc[0]}";

            IPAddress ipaddress = IPAddress.Parse(ip);
            byte[] addbuffer = ipaddress.GetAddressBytes();
            Array.Reverse(addbuffer);
            return System.BitConverter.ToUInt32(addbuffer, 0);
        }

        #endregion

        #region CreateWindowEx
        [DllImport("user32.dll")]
        static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName,
   string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
   IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr DCreateWindowEx(uint dwExStyle, string lpClassName,
   string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
   IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);
        static IntPtr CreateWindowEx_Hooked(uint dwExStyle, string lpClassName,
   string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
   IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
        {
            ConsoleShow.Log($"CreateWindowEx[调用]lpClassName->{lpClassName} lpWindowName->{lpWindowName}");
            // call original API...
            return CreateWindowEx(dwExStyle, lpClassName,
   lpWindowName, dwStyle, x, y, nWidth, nHeight,
   hWndParent, hMenu, hInstance, lpParam);
        }

        #endregion

        #region CopyFileA
        [DllImport("KERNEL32.dll")]
        static extern bool CopyFileA(string lpExistingFileName,
   string lpNewFileName, bool bFailIfExists);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate bool DCopyFileA(string lpExistingFileName,
   string lpNewFileName, bool bFailIfExists);
        static bool CopyFileA_Hooked(string lpExistingFileName,
   string lpNewFileName, bool bFailIfExists)
        {
            ConsoleShow.Log($"CopyFileA[调用] lpExistingFileName->{lpExistingFileName} lpNewFileName->{lpNewFileName} bFailIfExists->{bFailIfExists}");
            // call original API...
            return CopyFileA(lpExistingFileName, lpNewFileName, bFailIfExists);
        }

        #endregion

        #region CreateFileA
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr CreateFileA(
     [MarshalAs(UnmanagedType.LPStr)] string filename,
     [MarshalAs(UnmanagedType.U4)] FileAccess access,
     [MarshalAs(UnmanagedType.U4)] FileShare share,
     IntPtr securityAttributes,
     [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
     [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
     IntPtr templateFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        delegate IntPtr DCreateFileA(
     [MarshalAs(UnmanagedType.LPStr)] string filename,
     [MarshalAs(UnmanagedType.U4)] FileAccess access,
     [MarshalAs(UnmanagedType.U4)] FileShare share,
     IntPtr securityAttributes,
     [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
     [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
     IntPtr templateFile);
        static IntPtr CreateFileA_Hooked(
     [MarshalAs(UnmanagedType.LPStr)] string filename,
     [MarshalAs(UnmanagedType.U4)] FileAccess access,
     [MarshalAs(UnmanagedType.U4)] FileShare share,
     IntPtr securityAttributes,
     [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
     [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
     IntPtr templateFile)
        {

            ConsoleShow.Log($"CreateFileA[调用] filename->{filename}");
            if (filename.ToLower() == "mhf.exe")
            {
                ConsoleShow.Log($"CreateFileA == mhf.exe 干掉CAPCOM的自己拷贝自己，客户端自重启");
#if DEBUG
                MessageBox.Show("我杀我自己");
#endif
                Process.GetCurrentProcess().Kill();
                return IntPtr.Zero;
            }
            // call original API...
            return CreateFileA(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }

        #endregion
    }
}
