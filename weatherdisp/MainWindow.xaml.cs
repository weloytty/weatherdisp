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
    public partial class MainWindow : Window, IDisposable
    {
        FileSystemWatcher _watch;
        public MainWindow()
        {
            _watch = new FileSystemWatcher();
            InitializeComponent();
        }

        ~MainWindow()
        {
            Dispose(false);
        }




        private void Grid_Initializer(object sender, EventArgs e)
        {

            updateDisplay(new WeatherReading());

            if (File.Exists(Environment.GetEnvironmentVariable("ALLUSERSPROFILE") + @"\currdat.lst"))
            {
                _watch.Path = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
                _watch.NotifyFilter = NotifyFilters.LastWrite;
                _watch.Filter = "currdat.lst";

                _watch.Changed += new FileSystemEventHandler(watch_Changed);
                _watch.EnableRaisingEvents = true;

            }

        }

        void watch_Changed(object sender, FileSystemEventArgs e)
        {


            this.Dispatcher.Invoke((Action)(() =>
            {
                updateDisplay(new WeatherReading());
            }));

        }

        void updateDisplay(WeatherReading r)
        {
            this.outdoorTemperature.Value = r.OutdoorTemperature;
            this.temperatureLabel.Content = r.OutdoorTemperature;
            this.indoorTemperatureLabel.Content = r.IndoorTemperature;
            this.indoorTemperature.Value = r.IndoorTemperature;

            String risingOrFalling = "";
            if (r.WeatherTendancy == WeatherReading.WeatherTendancies.Rising) risingOrFalling = "↑";
            if (r.WeatherTendancy == WeatherReading.WeatherTendancies.Falling) risingOrFalling = "↓";

            this.pressureLabel.Content = r.PressureRelative + risingOrFalling;
            this.atmosphericPressure.Value = r.PressureRelative;
            this.outdoorHumidityLabel.Content = r.OutdoorHumidity.ToString() + "%";
            this.outdoorHumidity.Value = r.OutdoorHumidity;
            this.indoorHumidity.Value = r.IndoorHumidity;
            this.indoorHumidityLabel.Content = r.IndoorHumidity.ToString() + "%";

            this.dataUpdated.Content = r.DataRead;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _watch != null)
            {
                _watch.Dispose();
                _watch = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
