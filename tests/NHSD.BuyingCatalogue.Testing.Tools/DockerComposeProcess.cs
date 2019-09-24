using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public class DockerComposeProcess
    {
        private readonly string _workingDirectory;
        private readonly IEnumerable<string> _fileList;
        private readonly IDictionary<string, string> _enivronmentVariables;
        private readonly string _composeFileListArgument = null;

        public DockerComposeProcess(string workingDirectory, IEnumerable<string> fileList) : this(workingDirectory, fileList, new Dictionary<string, string>())
        {
        }

        public DockerComposeProcess(string workingDirectory, IEnumerable<string> fileList, IDictionary<string, string> enivronmentVariables)
        {
            _workingDirectory = workingDirectory ?? throw new ArgumentNullException(nameof(workingDirectory));
            _fileList = fileList ?? throw new ArgumentNullException(nameof(fileList));
            _enivronmentVariables = enivronmentVariables ?? throw new ArgumentNullException(nameof(enivronmentVariables));

            _composeFileListArgument = string.Join(" ", _fileList.Select(item => $"-f {item}")).Trim();
        }

        public int Start()
        {
            string argument = $"{_composeFileListArgument} up -d";

            return ExecuteProcess(CreateDockerComposeProcess(argument));
        }

        public int Stop()
        {
            string argument = $"{_composeFileListArgument} down -v -rmi \"all\"";

            return ExecuteProcess(CreateDockerComposeProcess(argument));
        }

        private ProcessStartInfo CreateDockerComposeProcess(string arguments)
        {
            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            ProcessStartInfo dockerComposeProcessStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                WorkingDirectory = _workingDirectory,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            IDictionary<string, string> environment = dockerComposeProcessStartInfo.Environment;
            foreach (var item in _enivronmentVariables)
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

            return dockerComposeProcessStartInfo;
        }

        private static int ExecuteProcess(ProcessStartInfo processStartInfo)
        {
            if (processStartInfo is null)
            {
                throw new ArgumentNullException(nameof(processStartInfo));
            }

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                return process.ExitCode;
            }
        }
    }
}
