using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FayeOS.Services.Applications
{
    public class ApplicationService
    {
        private readonly Dictionary<string, string> applications = new(StringComparer.OrdinalIgnoreCase)
        {
            { "notepad", "notepad.exe" },
            { "bloc de notas", "notepad.exe"},

            { "calculadora", "calc.exe" },
            { "calc", "calc.exe"},

            { "discord", "Discord.exe" },
            { "dc", "Discord.exe" },

            { "whatsApp", "WhatsApp.exe" },
            { "wasap", "WhatsApp.exe" },

            { "YouTube", "YouTube.exe" },
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
        public bool TryGetExecutable(string appName, out string? executable)
        {
            return applications.TryGetValue(appName, out executable);
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
