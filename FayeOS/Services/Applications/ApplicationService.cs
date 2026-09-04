using FayeOS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace FayeOS.Services.Applications
{
    public class ApplicationService
    {
        private readonly static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private readonly Dictionary<string, ApplicationInfo> applications = new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "Calculador",
                new ApplicationInfo
                {
                    FileName = "calc.exe",
                    Arguments = "",
                    ProcessName = "Calculator"
                }

            },
            {
                "Calc",
                new ApplicationInfo
                {
                    FileName = "calc.exe",
                    Arguments = "",
                    ProcessName = "Calculator"
                }
            },
            {
                "Bloc de notas",
                new ApplicationInfo
                {
                    FileName = "notepad.exe",
                    Arguments = "",
                    ProcessName = "notepad"
                }
            },
            {
                "Notepad",
                new ApplicationInfo
                {
                    FileName = "notepad.exe",
                    Arguments = "",
                    ProcessName = "notepad"
                }
            },
            {   
                "Discord",
                new ApplicationInfo
                {
                    FileName = Path.Combine(localAppData, "Discord", "Update.exe"),
                    Arguments = "--processStart Discord.exe",
                    ProcessName = "Discord"
                }
            },
            {   
                "DC",
                new ApplicationInfo
                {
                    FileName = Path.Combine(localAppData, "Discord", "Update.exe"),
                    Arguments = "--processStart Discord.exe",
                    ProcessName = "Discord"
                }
            }
        };
        public void OpenApplication(ApplicationInfo application) 
        {
            Process.Start(new ProcessStartInfo(application.FileName) 
            {
                FileName = application.FileName,
                Arguments = application.Arguments,
                UseShellExecute = true 
            });
        }
        public bool CloseApplication(ApplicationInfo application)
        {
            Process[] processes = Process.GetProcessesByName(application.ProcessName);

            if (processes.Length == 0)
            {
                return false;
            }
            foreach (Process process in processes)
            {
                try 
                {
                    process.Kill(); 
                }
                catch 
                {
                    return false; 
                }
            }

            return true;
        }
        public readonly string[] openActions =
            {
                "abre",
                "ejecuta",
                "inicia",
                "abrir",
            };
        public readonly string[] closeActions =
            {
                "cierra",
                "termina",
                "finaliza",
                "cerrar",
            };
        public bool TryGetApplication(string appName, out ApplicationInfo? app)
        {
            return applications.TryGetValue(appName, out app);
        } 
        public string NormalizeAppName(string appName)
        {
            appName = appName.Trim();

            string[] articles =
            {
                "el", 
                "la",
                "los",
                "las",
                "un",
                "una",
                "unos",
                "unas"
            };

            foreach (string article in articles) 
            {
                if (appName.StartsWith(article)) 
                {
                    appName = appName.Substring(article.Length);
                    break;
                }
            }
            return appName.Trim();
        }
    }
}
