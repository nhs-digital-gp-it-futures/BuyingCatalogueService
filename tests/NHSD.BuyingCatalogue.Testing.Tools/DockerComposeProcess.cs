using System.Collections.Generic;
using System.Diagnostics;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public sealed class DockerComposeProcess
	{
        public static ProcessStartInfo Create(string currentDirectory, string arguments)
        {
            return Create(currentDirectory, arguments, new KeyValuePair<string, string>("sam", string.Empty));
        }

        public static ProcessStartInfo Create(string currentDirectory, string arguments, params KeyValuePair<string, string>[] environmentVariables)
        {
            ProcessStartInfo dockerComposeProcessStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                WorkingDirectory = currentDirectory,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Environment(environmentVariables, dockerComposeProcessStartInfo);

            return dockerComposeProcessStartInfo;
        }

        private static void Environment(KeyValuePair<string, string>[] environmentVariables, ProcessStartInfo dockerComposeProcessStartInfo)
        {
            IDictionary<string, string> environment = dockerComposeProcessStartInfo.Environment;
            environment.Add("Tag", "test");
            foreach (var item in environmentVariables)
            {
                string key = item.Key;
                if (environment.ContainsKey(key))
                {
                    environment[key] = item.Value;
                }
                else
                {
                    environment.Add(item);
                }
            }
        }
    }
}
