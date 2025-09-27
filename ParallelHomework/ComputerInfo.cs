using System.Diagnostics;
using System.Management;

namespace ParallelHomework;

public static class ComputerInfo
{
    public static string GetSummaryInfo()
    {
        string info = $"""
            OS: {GetOSInfo()}
            Processor Count: {GetProcessorCount()}
            RAM: {GetRamInfo()}
            Processor: {GetProcessorName()}
            """;
        return info;
    }
    public static string GetOSInfo()
    {
        string os = string.Empty;
        if (OperatingSystem.IsWindows())
        {
            var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject item in searcher.Get())
            {
                os = item["Caption"]?.ToString() ?? "Unknown Windows OS";
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            try
            {
                // Попробуем прочитать /etc/os-release (стандарт для большинства дистрибутивов)
                if (File.Exists("/etc/os-release"))
                {
                    var lines = File.ReadAllLines("/etc/os-release");
                    string name = null;
                    string version = null;

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("PRETTY_NAME="))
                        {
                            name = line.Split('=')[1].Trim('"', '\'');
                            break;
                        }
                        else if (line.StartsWith("NAME=") && name == null)
                        {
                            name = line.Split('=')[1].Trim('"', '\'');
                        }
                        else if (line.StartsWith("VERSION_ID="))
                        {
                            version = line.Split('=')[1].Trim('"', '\'');
                        }
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        os = version != null ? $"{name} {version}" : name;
                    }
                }

                // Альтернативные методы для разных дистрибутивов
                else if (File.Exists("/etc/redhat-release"))
                {
                    os = File.ReadAllText("/etc/redhat-release").Trim();
                }

                else if (File.Exists("/etc/lsb-release"))
                {
                    var lines = File.ReadAllLines("/etc/lsb-release");
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("DISTRIB_DESCRIPTION="))
                        {
                            os = line.Split('=')[1].Trim('"', '\'');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения информации Linux: {ex.Message}");
            }

        }
        else if (OperatingSystem.IsMacOS())
        {
            try
            {
                // Используем system_profiler для получения информации о macOS
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "sw_vers",
                        Arguments = "-productVersion",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string version = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                os = $"macOS {version}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения информации macOS: {ex.Message}");
            }
        }
        else if (string.IsNullOrEmpty(os))
            os = Environment.OSVersion.ToString();
        return os;
    }

    public static int GetProcessorCount()
    {
        return Environment.ProcessorCount;
    }

    public static string GetRamInfo()
    {
        return $"{Math.Ceiling(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1073741824.0)} GB";
    }

    public static string GetProcessorName()
    {
        string processorName = string.Empty;
        if (OperatingSystem.IsWindows())
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        processorName = obj["Name"]?.ToString() ?? "Unknown";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                processorName = $"Error: ({ex.Message})";
            }
        }
        else if (OperatingSystem.IsLinux())
        {
            try
            {
                if (File.Exists("/proc/cpuinfo"))
                {
                    var lines = File.ReadAllLines("/proc/cpuinfo");
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("model name"))
                        {
                            processorName = line.Split(':')[1].Trim();
                        }
                    }
                }
                else
                {
                    // Альтернативный способ через команду
                    try
                    {
                        var process = new Process()
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "lscpu",
                                Arguments = "--json",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            }
                        };
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        if (!string.IsNullOrEmpty(output))
                        {
                            // Парсим JSON вывод (упрощенно)
                            if (output.Contains("Model name:"))
                            {
                                int start = output.IndexOf("Model name:") + "Model name:".Length;
                                int end = output.IndexOf("\n", start);
                                if (end > start)
                                    processorName = output.Substring(start, end - start).Trim();
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                processorName = $"Error reading CPU info: {ex.Message}";
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sysctl",
                        Arguments = "-n machdep.cpu.brand_string",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string procName = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                processorName = !string.IsNullOrEmpty(procName) ? procName : "Unknown Processor";
            }
            catch (Exception ex)
            {
                processorName = $"Error reading CPU info: {ex.Message}";
            }
        }
        else if (string.IsNullOrEmpty(processorName))
            processorName = "Unknown Processor";

        return processorName;
    }
}
