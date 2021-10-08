namespace Installizer
{
    class Registrayzier
    {
        private string regJson;
        private string[] regFiles;
        private System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> listOfRegistries = new();
        public Registrayzier(string Folder)
        {
            this.regFiles = System.IO.Directory.GetFiles(Folder, "*.reg", System.IO.SearchOption.AllDirectories);
        }
        private string[] CropLocations(string regFileText)
        {
            return regFileText.Split("[")[1..^0];
        }
        private string GetKey(string regFileText)
        {
            regFileText = regFileText.Replace(@"\", @"\\");
            if (regFileText.StartsWith("hk"))
            {
                regFileText = regFileText.Replace("hklm", "HKEY_LOCAL_MACHINE").Replace("hkcu", "HKEY_CURRENT_USER").Replace("hkcr", @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes");
            }
            if (regFileText.Contains(']'))
            {
                return regFileText.Split("]")[0];
            }
            else if (regFileText.Contains("\r\n"))
            {
                return regFileText.Split("\r\n")[0];
            }
            else if (regFileText.Contains("\n"))
            {
                return regFileText.Split("\n")[0];
            }
            return regFileText;
        }
        private string[]? SplitReg(string regFileText)
        {
            if (regFileText.Contains("="))
            {
                return regFileText.Split("=");
            }
            return null;
        }
        private string GetName(string regFileText)
        {
            string name = SplitReg(regFileText)[0] ?? "";
            return name;
        }
        private string GetType(string regFileText)
        {
            string type = SplitReg(regFileText)[1] ?? "";
            if (type.Length == 0)
            {
                return "None";
            }
            else if (type.StartsWith("\""))
            {
                return "string";
            }
            else if (type.StartsWith("hex"))
            {
                return "hex";
            }
            return "dword";
        }
        private string GetValue(string regFileText)
        {
            string value;
            string tempValue = SplitReg(regFileText)[1];
            string type = GetType(regFileText);
            if(type == "string")
            {
                value = tempValue;
            }
            else if(type == "dword")
            {
                value = tempValue.Split("dword:")[1];
            }
            else
            {
                value = "";
            }
            return value;
        }
        //private string 
    }
}
