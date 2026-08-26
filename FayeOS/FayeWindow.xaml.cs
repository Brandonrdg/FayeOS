using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using FayeOS.Services.Applications;
using FayeOS.Models;
using FayeOS.Services.System;
using System.Windows.Threading;
using FayeOS.ViewModels;

namespace FayeOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FayeWindow : Window
    {
        private readonly SystemViewModel SystemViewModel = new();
        private readonly ApplicationService applicationService = new();

        private readonly SystemService systemService = new();

        private readonly DispatcherTimer systemTimer = new();

        public FayeWindow()
        {
            InitializeComponent();

            UpdateSystemStats();

            DataContext = SystemViewModel;

            systemTimer.Interval = TimeSpan.FromSeconds(1);
            systemTimer.Tick += SystemTimer_Tick;
            systemTimer.Start();
        }
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!CommandTextBox.IsMouseOver) 
            {
                Keyboard.ClearFocus();
            }
               
        }
        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string command = CommandTextBox.Text;

                if (!string.IsNullOrWhiteSpace(command)) 
                {
                    TextBlock userMessage = new TextBlock();

                    userMessage.Text = "> " + command;
                    userMessage.Foreground = Brushes.White;
                    userMessage.Margin = new Thickness(0, 5, 0, 5);

                    ConversationPanel.Children.Add(userMessage);

                    ProcessComand(command);

                    CommandTextBox.Clear();
                }
            }
        }
        private void UpdateSystemStats() 
        {
            MemoryInfo memoryInfo = systemService.GetMemoryInfo();
            double cpuUsage = systemService.GetCpuUsagePercentage();

            SystemViewModel.RamUsage = memoryInfo.UsagePercentage;
            SystemViewModel.RamUsedGB = memoryInfo.UsedGB;
            SystemViewModel.RamTotalGB = memoryInfo.TotalGB;
            SystemViewModel.CpuUsage = cpuUsage; 
        }
        private void SystemTimer_Tick(object? sender, EventArgs e)
        {
            UpdateSystemStats();
        }
        private void ProcessComand(string command)
        {
            string normalizedCommand = command.Trim().ToLower();

            string? detectedAction = applicationService.openActions.FirstOrDefault(action => normalizedCommand.Contains(action));

            

            TextBlock fayeMessage = new TextBlock();

            if (normalizedCommand == "hola")
            {
                fayeMessage.Text = "FAYE: Hola! How can I help you today?";
            }
            
            else if(detectedAction != null) 
            {
                int actionIndex = normalizedCommand.IndexOf(detectedAction);

                int appNameStartIndex = actionIndex + detectedAction.Length;

                string appName = normalizedCommand.Substring(appNameStartIndex).Trim();

                appName = applicationService.NormalizeAppName(appName);

                if (applicationService.TryGetApplication(appName, out ApplicationInfo? application))
                {
                    applicationService.OpenApplication(application);
                    fayeMessage.Text = $"FAYE: Abriendo {appName}...";
                }
                else
                {
                    fayeMessage.Text = $"FAYE: No puedo abrir {appName}.";
                }
            }
            else
            {
                fayeMessage.Text = "FAYE: No reconozco ese comando.";
            }
           
            

            fayeMessage.Foreground = Brushes.LightBlue;
            fayeMessage.Margin = new Thickness(0, 5, 0, 10);

            ConversationPanel.Children.Add(fayeMessage);
        }
       
    }
}