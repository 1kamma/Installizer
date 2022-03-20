using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Installizer.PropertiesHelpers
{
    public class Winget
    {
        public bool detect;

        private bool DetectWinget()
        {
            Process process = new();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/c winget";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.Start();
            string g = process.StandardOutput.ReadToEnd();
            return g.Contains("Windows Package Manager");
        }

        public string? JsonApps
        {
            get
            {
                var a = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.AllDirectories);
                foreach (string s in a)
                {
                    if (Regex.IsMatch(s, "winget") /*|| (() => new ThreadStart(ReadFile(s)))*/)
                    {
                        return s;
                    }
                }

                return null;
            }
        }

        public Winget()
        {
            this.detect = DetectWinget();

        }

        private static ParameterizedThreadStart ReadFile(string path)
        {
            return (new ParameterizedThreadStart(x => File.ReadAllText(path).Contains("winget")));

        }
    }
}
