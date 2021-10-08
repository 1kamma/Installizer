namespace Installizer.PropertiesHelpers
{
    public class Winget
    {
        public bool detect;
        private bool DetectWinget()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/c winget";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.Start();
            string g = process.StandardOutput.ReadToEnd();
            return g.Contains("Windows Package Manager");
        }
        public Winget()
        {
            this.detect = DetectWinget();
        }
    }
}
