using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FayeOS.ViewModels
{
    public class SystemViewModel : INotifyPropertyChanged
    {
        private double ramUsage;

        public double RamUsage 
        {
            get => ramUsage;
            set 
            {
                if (ramUsage != value)
                {
                    ramUsage = value;
                    OnPropertyChanged(nameof(ramUsage));
                }
            }
        }
        private double ramUsedGB;

        public double RamUsedGB 
        {
            get => ramUsedGB;
            set 
            {
                if (ramUsedGB != value) 
                {
                    ramUsedGB = value;
                    OnPropertyChanged(nameof(ramUsedGB));
                }
               
            }
        }
        private double ramTotalGB;

        public double RamTotalGB
        {
            get => ramTotalGB;
            set
            {
                if (ramTotalGB != value)
                {
                    ramTotalGB = value;
                    OnPropertyChanged(nameof(ramTotalGB));
                }
            }
        }
        private double cpuUsage;

        public double CpuUsage 
        {
            get => cpuUsage;
            set 
            { 
                if (cpuUsage != value)
                {
                    cpuUsage = value;
                    OnPropertyChanged(nameof(CpuUsage));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName) 
        { 
                   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
