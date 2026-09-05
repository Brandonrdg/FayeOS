using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace FayeOS.Services.Commands
{
    public class CommandService
    {
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
                "terminar",
                "finalizar"
            };

        public string? DetectOpenAction(string normalizedCommand) 
        {
            return openActions.FirstOrDefault(action => normalizedCommand.Contains(action));
        }

        public string? DetectCloseAction(string normalizedCommand)
        {
            return closeActions.FirstOrDefault(action => normalizedCommand.Contains(action));
        }

        public string normalizedCommand(string command)
        {
            return command.Trim().ToLower();
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
        public string ExtractAppName(string normalizedCommand, string detectedAction)
        {
            int actionIndex = normalizedCommand.IndexOf(detectedAction);
            int appNameStartIndex = actionIndex + detectedAction.Length;
            string appName = normalizedCommand.Substring(appNameStartIndex).Trim();
            return NormalizeAppName(appName);
        }
    }
}
