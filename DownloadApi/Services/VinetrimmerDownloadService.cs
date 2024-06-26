using System.Diagnostics;
using Common.Models;

namespace DownloadApi.Services
{
    public class VinetrimmerDownloadService : IDownloadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<VinetrimmerDownloadService> _logger;
        private string _poetryPath = null;
        private string _vtPath = null;

        public VinetrimmerDownloadService(IWebHostEnvironment environment, ILogger<VinetrimmerDownloadService> logger)
        {
            _environment = environment;
            _logger = logger;
        }
        public bool IsDownloaderAvailable()
        {
            bool isPoetryInstalled = CheckIfPoetryIsInstalled();
            bool isVinetrimmerInstalled = CheckIfVinetrimmerIsInstalled();
            if(isVinetrimmerInstalled)
            { 
                SetupDockerSpecificBinariesIfNeeded(); 
            }
            return isVinetrimmerInstalled && isPoetryInstalled;
        }

        private void SetupDockerSpecificBinariesIfNeeded()
        {
            if (File.Exists("/.dockerenv"))
            {
              
                var output = RunProcessWithStandardOutput("which","ffprobe");
                string vtffprobeFile = Path.Combine(_vtPath, "binaries", "ffprobe");

                if (!File.Exists(vtffprobeFile))
                {
                    File.Copy(output, vtffprobeFile); 
                }

                output = RunProcessWithStandardOutput("which", "aria2c");
                string aria2cpath = Path.Combine(_vtPath, "binaries", "aria2c");
                if (!File.Exists(aria2cpath))
                {
                    File.Copy(output,aria2cpath);
                }

                output = RunProcessWithStandardOutput("which", "packager");
                string packagerPath = Path.Combine(_vtPath, "binaries", "packager");
                if (!File.Exists(packagerPath))
                {
                    File.Copy(output, packagerPath);
                }

                output = RunProcessWithStandardOutput("which", "mkvmerge");
                string mkvMergePath = Path.Combine(_vtPath, "binaries", "mkvmerge");
                if (!File.Exists(mkvMergePath))
                {
                    File.Copy(output, mkvMergePath);
                }
            }
        }

        private static string RunProcessWithStandardOutput(string process, string args)
        {
            string output = "";
            Process dependencyFinder = new Process();
            dependencyFinder.StartInfo = new ProcessStartInfo(process, args);
            dependencyFinder.StartInfo.UseShellExecute = false;
            dependencyFinder.StartInfo.RedirectStandardOutput = true;
            dependencyFinder.StartInfo.RedirectStandardError = true;
            dependencyFinder.OutputDataReceived += delegate(object sender, DataReceivedEventArgs eventArgs)
            {
                output += eventArgs.Data;
            };
            dependencyFinder.Start();
            dependencyFinder.BeginErrorReadLine();
            dependencyFinder.BeginOutputReadLine();
            dependencyFinder.WaitForExit();
            return output;
        }

        private bool CheckIfPoetryIsInstalled()
        {
            if (IsPoetryInPath()) return true;
            string poetryBinPath = Environment.GetEnvironmentVariable("POETRY_BIN_PATH") ?? String.Empty;
            if (string.IsNullOrEmpty(poetryBinPath)) return false;
            if (Directory.Exists(poetryBinPath))
            {
                _poetryPath = poetryBinPath;
                _logger.LogInformation($"Poetry path is {_poetryPath}");
                return true;
            }
            return false;
        }

        private bool IsPoetryInPath()
        {
            bool isPoetryInPath = false;
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo("poetry");
                process.Start();
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    isPoetryInPath = true;
                    _poetryPath = "";
                }
            }
            catch ( Exception exception)
            {
               // eat the exception because we don't care about it not existing in this case 
            }

            return isPoetryInPath;
        }

        private bool CheckIfVinetrimmerIsInstalled()
        {
            bool vtExists = false;
            string vtPath = Path.Combine(_environment.ContentRootPath,"vt");
            if (Directory.Exists(vtPath))
            {
                vtExists = true;
                _vtPath = vtPath;
            }

            vtPath = Environment.GetEnvironmentVariable("VT_INSTALL_PATH");
            if (!string.IsNullOrEmpty(vtPath))
            {
                if (Directory.Exists(vtPath))
                {
                    vtExists = true;
                    _vtPath = vtPath;
                    InstallVt();
                }
            }
            return vtExists;

        }

        private void InstallVt()
        {
			if (_vtPath is null || _poetryPath is null) return;
			Process proc = new Process();
			proc.StartInfo = new ProcessStartInfo();
			proc.StartInfo.FileName = Path.Combine(_poetryPath, "poetry");
			proc.StartInfo.WorkingDirectory = _vtPath;
			proc.StartInfo.Arguments = "config virtualenvs.in-project true";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = false;
			proc.StartInfo.RedirectStandardError = false;
			proc.Start();
			proc.WaitForExit();
			proc = new Process();
			proc.StartInfo = new ProcessStartInfo();
			proc.StartInfo.FileName = Path.Combine(_poetryPath, "poetry");
			proc.StartInfo.WorkingDirectory = _vtPath;
			proc.StartInfo.Arguments = "config virtualenvs.in-project true";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardOutput = false;
			proc.StartInfo.RedirectStandardError = false;
			proc.StartInfo.Arguments = "install";
			proc.Start();
			proc.WaitForExit();

		}

		public List<ServiceInfo> GetAvailableServices()
		{
		   var services = new List<ServiceInfo>();
            if (IsDownloaderAvailable())
            {
                if (_vtPath is null || _poetryPath is null) return services;
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo();
                proc.StartInfo.FileName = Path.Combine(_poetryPath, "poetry");
                proc.StartInfo.WorkingDirectory = _vtPath;
                proc.StartInfo.Arguments = "run vt";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                string output = "";
                bool commands = false;
                proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    if (e.Data?.Contains("Commands") == true)
                    {
                        commands = true;
                        return;
                    }

                    if (commands)
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            var split = e.Data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            string servicetag = split[0];
                            var urls = split.Skip(1).ToList();
                            for (int i = 0; i < urls.Count; i++)
                            {
                                urls[i] = urls[i].Replace(",", "").Trim();
                            }
                            services.Add(new ServiceInfo()
                            {
                                ServiceTag = servicetag.Trim(),
                                ServiceUrls = urls.ToList()
                            });
                        }
                    }
                };
                proc.Start();
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();
                proc.WaitForExit();

                List<string> configLines = GetVtConfig();
                List<string> credentialsConfigLines = GetCredentialLines(configLines);
                List<string> servicesConfiguredByCredentials = GetServicesConfiguredByCredentials(credentialsConfigLines);
                List<string> servicesConfiguredByCookies = GetServicesConfiguredByCookies();
                foreach (var serviceInfo in services)
                {
                    if (servicesConfiguredByCredentials.Any(item =>
                            item.Equals(serviceInfo.ServiceTag, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        serviceInfo.ServiceConfigurationState = ServiceConfigurationStateEnum.ConfiguredWithCredentials;
                    }


                    if (servicesConfiguredByCookies.Contains(serviceInfo.ServiceTag))
                    {
                        serviceInfo.ServiceConfigurationState = ServiceConfigurationStateEnum.ConfiguredWithCookies;
                    }
                } 
            }
            return services;
        }

        private List<string> GetServicesConfiguredByCookies()
        {
            var services = new List<string>();
            var cookieFolderPath = Path.Combine(_vtPath, "vinetrimmer","Cookies");
            var cookiesDirs = Directory.EnumerateDirectories(cookieFolderPath);
            foreach (var cookieDir in cookiesDirs)
            {
                var filepath = Path.Combine(cookieDir, "default.txt");
                if (File.Exists(filepath))
                {
                    var fileContent = File.ReadAllText(filepath);
                    if (fileContent != string.Empty)
                    {
                        services.Add(new DirectoryInfo(cookieDir).Name);
                    }
                }
            }
            return services;
        }

        private List<string> GetServicesConfiguredByCredentials(List<string> credentialsConfigLines)
        {
            var services = new List<string>();
            foreach (var credentialsConfigLine in credentialsConfigLines)
            {
                var splited = credentialsConfigLine.Split(":",2, StringSplitOptions.RemoveEmptyEntries);
                if (splited.Any())
                {
                    if (splited.Length == 2)
                    {
                        var serviceName = splited[0].Trim();
                        var credetials = splited[1].Trim();
                        if (!credetials.Contains("email:password"))
                        {
                            services.Add(serviceName);
                        }
                    }
                }
            }
            return services;
        }

        private List<string> GetCredentialLines(List<string> configLines)
        {
            var lines = new List<string>();
            var credentialsLineIndex = configLines.FindIndex(item => item.Trim().Equals("credentials:"));
            if (credentialsLineIndex < 0) return lines;
            for (int i = credentialsLineIndex+1; i < configLines.Count; i++)
            {
                if (configLines[i].Trim().EndsWith(":"))
                {
                    break; // we are in new section of ymal
                }
                lines.Add(configLines[i]);
            }
            credentialsLineIndex = configLines.FindIndex(credentialsLineIndex+1,item => item.Trim().Equals("credentials:"));
            if (credentialsLineIndex < 0) return lines;
            for (int i = credentialsLineIndex + 1; i < configLines.Count; i++)
            {
                if (configLines[i].Trim().EndsWith(":"))
                {
                    break; // we are in new section of ymal
                }
                lines.Add(configLines[i]);
            }

            return lines;
        }

        private List<string> GetVtConfig()
        {
            string vtMainConfigPath = Path.Combine(_vtPath, "vinetrimmer","vinetrimmer.yml");
            string vtServicesConfigPath = Path.Combine(_vtPath, "vinetrimmer","config","vinetrimmer.yml");
            List<string> vtConfigLines = new List<string>();
            vtConfigLines.AddRange(File.ReadAllLines(vtMainConfigPath));
            vtConfigLines.AddRange(File.ReadAllLines(vtServicesConfigPath));
            return vtConfigLines;
        }


        public int StartServiceDownload(string serviceId, string url, string quality="")
        {
            if (IsDownloaderAvailable())
            {
	            var args = $"run vt dl -al all -sl all {serviceId} {url}";
	            if (!string.IsNullOrWhiteSpace(quality))
	            {
		            args = $"run vt dl -al all -sl all -q {quality} {serviceId} {url}";

	            }


				_logger.LogInformation($"Starting download {serviceId} {url} {quality}");
                var output = RunProcessWithStandardAndErrorOutput(args, out var proc);
                _logger.LogInformation(output);

                return proc.ExitCode; 
            }

            return -1;
        }

        private string RunProcessWithStandardAndErrorOutput(string args, out Process proc)
        {
            proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = Path.Combine(_poetryPath, "poetry");
            proc.StartInfo.WorkingDirectory = _vtPath;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            string output = "";

            var services = new List<ServiceInfo>();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                output += Environment.NewLine + e.Data;
            };
            proc.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e)
            {
                output += Environment.NewLine + e.Data;
            };
            proc.Start();
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            return output;
        }
    }
}
