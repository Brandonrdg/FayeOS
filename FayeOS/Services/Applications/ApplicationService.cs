using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FayeOS.Models;

namespace FayeOS.Services.Applications
{
    public class ApplicationService
    {
        private readonly Dictionary<string, ApplicationInfo> applications = new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "Calculador",
                new ApplicationInfo
                {
                    FileName = "calc.exe",
                    Arguments = ""
                }

            },
            {
                "Calc",
                new ApplicationInfo
                {
                    FileName = "calc.exe",
                    Arguments = ""
                }
            },
            {
                "Bloc de notas",
                new ApplicationInfo
                {
                    FileName = "notepad.exe",
                    Arguments = ""
                }
            },
            {
                "Notepad",
                new ApplicationInfo
                {
                    FileName = "notepad.exe",
                    Arguments = ""
                }
            }
        };
        public void OpenApplication(string fileName) 
        {
            Process.Start(new ProcessStartInfo(fileName) 
            {
                FileName = fileName,
                UseShellExecute = true 
            });
        }
        public readonly string[] openActions =
            {
                "abre",
                "ejecuta",
                "inicia",
                "abrir",
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
