using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherData;
using System.IO;


namespace weatherdisp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileSystemWatcher _watch;
        public MainWindow()
        {
            _watch = new FileSystemWatcher();
            InitializeComponent();
            
        }

        ~MainWindow()
        {
            _watch.Dispose();
        }


        private void Grid_Initialized_1(object sender, EventArgs e)
        {
            WeatherData.WeatherReading r = new WeatherReading();
            this.outdoorTemperature.Value = r.OutdoorTemperature;
            this.temperatureLabel.Content = r.OutdoorTemperature;

            String risingOrFalling = "";
            if (r.WeatherTendancy == WeatherReading.WeatherTendancies.Rising) risingOrFalling = "↑";
            if (r.WeatherTendancy == WeatherReading.WeatherTendancies.Falling) risingOrFalling = "↓";



            this.pressureLabel.Content = r.PressureRelative + risingOrFalling;
            this.atmosphericPressure.Value = r.PressureRelative ;
            this.outdoorHumidityLabel.Content = r.OutdoorHumidity.ToString()  +"%";
            this.outdoorHumidity.Value = r.OutdoorHumidity ;
            this.dataUpdated.Content = r.DataRead;


            
            
            if(File.Exists(Environment.GetEnvironmentVariable("ALLUSERSPROFILE") + @"\curlst.dat")){
                _watch.Path = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
                _watch.NotifyFilter = NotifyFilters.LastWrite;
                _watch.Filter = "currdat.lst";

                _watch.Changed += new FileSystemEventHandler(watch_Changed);
                _watch.EnableRaisingEvents = true;
            }
                
            


        }

        void watch_Changed(object sender, FileSystemEventArgs e)
        {

            WeatherReading newReading = new WeatherReading();

            this.Dispatcher.Invoke((Action)(()=>
            {
                this.outdoorTemperature.Value = newReading.OutdoorTemperature;
                this.temperatureLabel.Content = newReading.OutdoorTemperature;

                String risingOrFalling = "";
                if (newReading.WeatherTendancy == WeatherReading.WeatherTendancies.Rising) risingOrFalling = "↑";
                if (newReading.WeatherTendancy == WeatherReading.WeatherTendancies.Falling) risingOrFalling = "↓";

                this.pressureLabel.Content = newReading.PressureRelative + risingOrFalling;
                this.atmosphericPressure.Value = newReading.PressureRelative;
                this.dataUpdated.Content = newReading.DataRead;
                this.outdoorHumidity.Value = newReading.OutdoorHumidity;
                this.outdoorHumidityLabel.Content = newReading.OutdoorHumidity.ToString() + "%"; 

            }));


            


        }

    }
}
