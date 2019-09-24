using System;
using System.Diagnostics;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Testing.Tools
{
    public static class ProcessStartInfoExtensions
    {
        public static void Execute(this ProcessStartInfo processStartInfo)
        {
            using (Process process = Process.Start(processStartInfo ?? throw new ArgumentNullException(nameof(processStartInfo))))
            {
                process.WaitForExit();
                process.ExitCode.Should().Be(0);
            }
        }
    }
}
