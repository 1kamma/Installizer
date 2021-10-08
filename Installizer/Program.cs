namespace Installizer
{
    class Program
    {
#nullable enable
        public static string requireInternetTasks = System.IO.Directory.GetFiles(@"C:\Users\Public\Desktop", @"RequireInternet.*", System.IO.SearchOption.AllDirectories)[0];
        public static string regularTasks = System.IO.Directory.GetFiles(@"C:\Users\Public\Desktop", @"Regular.*", System.IO.SearchOption.AllDirectories)[0];
        public static string serial = System.IO.Directory.GetFiles(@"C:\Users\Public\Desktop", @"Serial.*", System.IO.SearchOption.AllDirectories)[0];
        public static string registrySettings = System.IO.Directory.GetFiles(@"C:\Users\Public\Desktop", @"registry.json", System.IO.SearchOption.AllDirectories)[0];
        public static string[] wifis = System.IO.Directory.GetFiles(@"C:\Users\Public\Desktop", @"*.xml", System.IO.SearchOption.AllDirectories);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registryOption"></param>
        public static void SetRegistryOption(Newtonsoft.Json.Linq.JObject registryOption)
        {
            try
            {
                if ((registryOption.ContainsKey("Key")) && (registryOption.ContainsKey("Name")) && (registryOption.ContainsKey("Value")))
                {
                    if (registryOption.ContainsKey("Type"))
                    {
                        dynamic type;
                        string data = registryOption["Type"].ToString();
                        switch (data)
                        {
                            case "dword":
                                {
                                    type = Microsoft.Win32.RegistryValueKind.DWord;
                                    break;
                                }
                            case "hex":
                                {
                                    type = Microsoft.Win32.RegistryValueKind.Binary;
                                    break;
                                }
                            default:
                                {
                                    type = Microsoft.Win32.RegistryValueKind.String;
                                    break;
                                }
                        }
                        Microsoft.Win32.Registry.SetValue((string)registryOption["Key"], (string)registryOption["Name"], (string)registryOption["Value"], type);
                    }
                }
            }
            catch
            {
            }
        }
        public static void SetRegistryOptions()
        {
            Newtonsoft.Json.Linq.JArray registryOption = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(System.IO.File.ReadAllText(registrySettings));
            foreach (Newtonsoft.Json.Linq.JObject json in registryOption)
            {
                SetRegistryOption(json);
            }
        }
        public static bool Connected()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        public static void AddWifi()
        {
            ManagedNativeWifi.ProfileDocument wifi = new(wifis[0]);
        }
        public static void RequireInternet()
        {
            if (Connected())
            {
                string[] tasks = System.IO.File.ReadAllLines(requireInternetTasks);
            }
        }

        static void Main(string[] args)
        {
            //bool f = Connected();
            //Console.WriteLine("Hello World!");
            ////AddWifi();
            //RegistryCreator reg = new (@"D:\Neutral Folder\");
            //reg.RegToJsonFile();
            //ChocoInstaller chocoInstaller = new(@"D:\the Windows Deployment\Copy To Folders\Users\Public\Desktop\choco.txt");
            //chocoInstaller.SetContent(0);
            Tasker tasker = new Tasker("ab", "cd");
            int[] i = new int[] { 1, 2 };
            tasker.TaskActionsDefine(@"C:\Program Files\PowerShell\7\pwsh.exe", null, null);
            //tasker.TaskDailyTriggerrDefine("12:30");
            int[] a = new int[] { 1, 2 };
            tasker.TaskWeeklyTriggerDefine("12:30", a);
            tasker.TaskSettingsDefine();
            tasker.TaskPrincipal(user: "Feanor\\סארט");
            //Tasker.ExportJson("D:\\Neutral Folder\\t.json", tasker);
            tasker.RegisterTask();
            //string d = @"{
            //""dayly"":
            //[""12:30""]
            //}";
            //dynamic jso = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(d);
            //Tasker.GetDailyTrigger(jso);
            //System.Console.WriteLine("now");
            //PropertiesHelpers.Winget winget = new();
            //System.Console.WriteLine(winget.detect);
        }
    }
}
