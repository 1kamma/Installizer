using Newtonsoft.Json.Linq;
namespace Installizer
{
    class Env
    {
        public static void MachineEnv(string varia, string path)
        {
            System.Environment.SetEnvironmentVariable(varia, path, System.EnvironmentVariableTarget.Machine);
        }
        public static void UserEnv(string varia, string path)
        {
            System.Environment.SetEnvironmentVariable(varia, path, System.EnvironmentVariableTarget.User);
        }
        public static void SetEnv(string path)
        {
            string data = System.IO.File.ReadAllText(path);
            var js = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(data);

        }
    }
}
