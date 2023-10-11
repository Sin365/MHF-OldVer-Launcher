# MHF-OldVer-Launcher

This is a tool I created. by axibug.com **You can download it at the bottom of the article**

This is a tool that can help configure MHF older version landers,(S2 to FW5 are applicable. However, other versions require support in order to be used.)   

Tools for IP and ports of Server1, Server2, and Server3.

Very convenient to use

##***Usage:***

**1. Copy the content of [MHF OldVer Launcher v1.0.7z] to the MHF game directory.**

**2. Configure the IP addresses and ports of three servers in [MHF-OldVer-Launcher.cfg]**

For example,  eg. in MHF-OldVer-Launcher.cfg:

	Server1Name=ServerName1
	Server1IP=1.2.3.4
	Server1Port=53312
	Server2Name=OtherServerName1
	Server2IP=1.2.3.4
	Server2Port=53312
	Server3Name=OtherServerName2
	Server3IP=1.2.3.4
	Server3Port=53312

**3. Start [MHF OldVer Launcher. exe]**

##***Attention:***

1. Currently only applicable to [Season 2~Forward 5] versions.

2. It is recommended not to modify the [sign-mhf.capcom-networks.jp] domain name in the program to keep HOOK more accurate.

3. To maintain program simplicity, only include HOOK logic. However, it does not include game files. And does not include logic such as Bypass GG.

For rapid development,.Net4 is adopted. (Win7 users may need to install .Net4)

##***Function:***

Server IP and ports can be configured accordingly to maintain universality.

Other versions, future support.

Time is tight, so the UI has not been beautified yet. We will quickly provide it for you to use. Further enrichment.

The log file is located in C: /Users/<username>/Documents/AxibugLauncherInject.txt (used for troubleshooting, but I have tested it, works!)

##***Resolved issue:***

Solve the logical separation of HOOK when using three servers with the same port value.

Solve the problem of CAPCOM error pop-up and HOOK failure after automatically restarting the game after the program crashes.
