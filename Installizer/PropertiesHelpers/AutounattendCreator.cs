namespace Installizer.PropertiesHelpers
{
    public class AutounattendCreator
    {

        private System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
        int drive;
        bool x64;
        bool uefi;
        string type;
        string lang;
        string computername;
        string[] keyboards;
        string language;
        public AutounattendCreator(int drive = 0, bool x64 = true, bool uefi = true, string type = "pro", string lang = "0409", string computername = null, string[] keyboards = null)
        {
            this.drive = drive;
            this.x64 = x64;
            this.uefi = uefi;
            switch (type)
            {
                case "home":
                    {
                        this.type = "home";
                        break;
                    }
                default:
                    {
                        this.type = "pro";
                        break;
                    }
            }
            if (keyboards is not null)
            {
                this.keyboards = keyboards;
            }
            if (computername is not null)
            {
                this.computername = computername;
            }
        }
        public void SetLanguage(string language)
        {
            this.lang = $"{language}:0000{language}";
        }

    }
}
