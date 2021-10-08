
namespace Installizer
{
    public class RegistryCreator
    {
        private string regJson = @"[]";
        private string regFolder;
        private string[] regs;
        public RegistryCreator(string Path)
        {
            this.regFolder = Path;
            RegFiles();
        }
        private void RegFiles()
        {
            this.regs = System.IO.Directory.GetFiles(this.regFolder, "*.reg", System.IO.SearchOption.AllDirectories);
        }
        private string[] _cropRegFile(string regFileContent)
        {
            string[] regSplited = regFileContent.Split('[');
            if(regSplited[0].StartsWith("Windows"))
            {
                regSplited = regSplited[1..^0];
            }
            return regSplited;
        }
        private string getKey(string regText)
        {
            System.Text.RegularExpressions.Regex regex = new(@"([A-z]|\\|{|}|[0-9])+");
            return $"\"Key\":\"{regex.Match(regText).Value}\"".Replace(@"\",@"\\").Replace("]","");
        }
        private string getName(string regText)
        {
            System.Text.RegularExpressions.Regex regex = new(@"([A-z]|""|[א-ת]|[ךםןףץ]|[0-9])+");
            string name = $"\"Name\":{regex.Match(regText.Split("=")[0]).Value}";
            if (name.EndsWith(':'))
            {
                name += "\"(Default)\"";
            }
            return name;
        }
        private string getType(string regText)
        {
            if (regText.Length < 1)
            {
                return "";
            }
            if(regText.Contains(@"""="""))
            {
                return "\"Type\":\"string\"";
            }
            else if(regText.Contains("hex"))
            {
                return "\"Type\":\"hex\"";
            }
            return "\"Type\":\"dword\"";
        }
        private string getValue(string regText, string typeReg)
        {
            string value = "\"Value\":";
            switch(typeReg)
            {
                case "\"Type\":\"dword\"":
                    {
                        System.Text.RegularExpressions.Regex regex = new(@"[0-9]+");
                        value += $"\"{regex.Match(regText.Split("dword:")[^1]).Value}\"";
                        break;
                    }
                default:
                    {
                        value += $"{regText.Split("=")[^1]}";
                        break;
                    }
            }
            if(value.EndsWith(":"))
            {
                value += "\"\"";
            }
            return value;
        }
        private string regToJson(string regText)
        {
            string jsonReg = "";
            string key = getKey(regText);
            string[] regTexts = regText.Split("\r\n")[1..^0];
            foreach (var reg in regTexts)
            {
                string type = getType(reg);
                string value = getValue(reg, type);
                string name = getName(reg);
                jsonReg += $"{(char)123}{key},{name}, {value}, {type}{(char)125},";
            }
            try
            {
                return $"{jsonReg[0..^1]}";

            }
            catch
            {

                return jsonReg;
            }
        }
        public void RegToJsonFile(string path = @"D:\Neutral Folder\registy.json")
        {
            string json = "[";
            foreach(string regFile in this.regs)
            {
                string[] regContent = _cropRegFile(System.IO.File.ReadAllText(regFile));
                foreach (string regIt in regContent)
                {
                     json += $"{regToJson(regIt)},";
                }
            }
            json += "]";
            System.IO.File.WriteAllText(path,json);
        }
    }
}
