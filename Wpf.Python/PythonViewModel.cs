using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Python
{
    public partial class PythonViewModel : ObservableRecipient
    {
        [ObservableProperty]
        string testText;
        [ObservableProperty]
        string logText;

        public PythonViewModel()
        {
            IsActive = true;
            TestText = "Python";
        }

        [RelayCommand]
        async void StartFunc()
        {
            try
            {
                LogText = string.Empty;
                var output = await RunAutomationExeAsync();
                LogText = output;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Automation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> RunAutomationExeAsync()
        {
            // bin폴더
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var exePath = Path.Combine(baseDir, "Tools", "SimpleTestAutomation", "SimpleTestAutomation.exe");
            if (!File.Exists(exePath))
                throw new FileNotFoundException("Automation EXE not found. Ensure it is copied to output directory.", exePath);

            var workingDir = Path.GetDirectoryName(exePath)!;

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var p = new Process { StartInfo = psi };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            p.OutputDataReceived += (_, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
            p.ErrorDataReceived += (_, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            await p.WaitForExitAsync();

            if (p.ExitCode != 0)
                throw new Exception($"SimpleTestAutomation failed (ExitCode={p.ExitCode}).\n\n{stderr}");

            return stdout.ToString();
        }
    }
}
