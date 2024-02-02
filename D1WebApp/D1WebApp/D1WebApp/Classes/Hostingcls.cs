using System.DirectoryServices;
using System;
using Microsoft.Web.Administration;
using System.Net;
using System.IO;

namespace D1WebApp.Utilities
{


    public class HostingCls
    {
        public static void CreateAppPool(string poolname, bool enable32bitOn64, ManagedPipelineMode mode, string runtimeVersion = "v4.0")
        {
            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool newPool = serverManager.ApplicationPools.Add(poolname);
                newPool.ManagedRuntimeVersion = runtimeVersion;
                newPool.Enable32BitAppOnWin64 = true;
                newPool.ManagedPipelineMode = mode;
                serverManager.CommitChanges();
            }
        }
        public static bool ModifyHostsFile(string entry)
        {
            try
            {
                using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts")))
                {
                    w.WriteLine(entry);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "ModifyHostsFile", DateTime.Now, "ModifyHostsFile", ex.ToString(), "Account", 2);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool ReplaceHostsFile(string entry)
        {
            try
            {
                string text = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts"));
                text = text.Replace(entry, "");
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts"), text);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "ModifyHostsFile", DateTime.Now, "ModifyHostsFile", ex.ToString(), "Account", 2);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static void DeleteIISWebsite(string websiteName)
        {
            try
            {
                ServerManager iisManager = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config");
                Application aap = iisManager.Sites["Default Web Site"].Applications[websiteName];
                iisManager.Sites["Default Web Site"].Applications.Remove(aap);
                iisManager.CommitChanges();
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                ReplaceHostsFile(" " + myIP + " " + websiteName);
               
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Website Delete", DateTime.Now, "Website Delete", ed.ToString(), "Account", 2);
            }
        }

        public static void CreateIISWebsite(string websiteName, string hostname, string phyPath, string appPool)
        {
            try
            {
                //ServerManager iisManager = new ServerManager();
                ServerManager iisManager = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config");
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                ModifyHostsFile(" " + myIP + " " + websiteName);
                iisManager.Sites["Default Web Site"].Applications.Add(websiteName, phyPath);
                iisManager.Sites["Default Web Site"].Applications[websiteName].ApplicationPoolName = appPool;
                iisManager.CommitChanges();

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Website Create", DateTime.Now, "Website Create", ed.ToString(), "Account", 2);
            }
        }


    }


}