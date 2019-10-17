using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class ProcessStartInfoExtensions
    {
        public static async Task<int> ExecuteAsync(this ProcessStartInfo processStartInfo, Action<string> onOutputMessage = null, Action<string> onErrorMessage = null)
        {
            if (processStartInfo is null)
            {
                throw new ArgumentNullException(nameof(processStartInfo));
            }

            TaskCompletionSource<int> task = new TaskCompletionSource<int>();

            Process process = new Process { StartInfo = processStartInfo, EnableRaisingEvents = true };

            process.Exited += (sender, eventArgs) =>
            {
                process.WaitForExit();
                int code = process.ExitCode;
                process.CancelOutputRead();
                process.CancelErrorRead();
                process.Dispose();

                task.TrySetResult(code);
            };

            process.OutputDataReceived += (sender, eventArgs) =>
            {
                string data = eventArgs.Data;
                if (data is object)
                {
                    SendMesasge(data, onOutputMessage);
                }
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                string data = eventArgs.Data;
                if (data is object)
                {
                    SendMesasge(data, onErrorMessage ?? onOutputMessage);
                }
            };

            process.Start().Should().BeTrue();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return await task.Task.ConfigureAwait(false);
        }

        private static void SendMesasge(string line, Action<string> action)
        {
            action?.Invoke(string.IsNullOrWhiteSpace(line) ? line : line.TrimEnd());
        }
    }
}
