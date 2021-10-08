using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installizer
{
    class ChocoInstaller
    {
        private string[] ListOfPackages;

        public ChocoInstaller(string chocoListOfPackages)
        {
            this.ListOfPackages = System.IO.File.ReadAllLines(chocoListOfPackages);
        }
        private string GetLink(string chocoPack)
        {
            return $"https://community.chocolatey.org/packages/{chocoPack}";
        }
        private string GetPackageLink(string chocoPack)
        {
            string p = GetLink(chocoPack);
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(p);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)webRequest.GetResponse();
            System.IO.Stream stream= response.GetResponseStream();
            System.IO.StreamReader streamReader = new(stream);
            response.Close();
            stream.Close();
            string data = streamReader.ReadToEnd();
            return data;
        }
        private void DownloadPackage(string chocoPack)
        {
            string htmlData = GetPackageLink(chocoPack);
            string path = $"C:/Users/Public/Desktop/PackDown/{chocoPack}.nupkg";
            int begin = htmlData.IndexOf("https://community.chocolatey.org/api/v2/package");
            int end = htmlData.IndexOf('"', begin) - 1;
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(htmlData.Substring(begin, (end - begin)));
            System.Net.WebResponse webResponse = webRequest.GetResponse();
            System.IO.Stream stream = webRequest.GetRequestStream();
            System.IO.StreamWriter streamWriter = new(path);
            streamWriter.Write(stream);
 
        }
        public void SetContent(int idPack)
        {
            DownloadPackage(this.ListOfPackages[idPack]);
        }
    }
}
