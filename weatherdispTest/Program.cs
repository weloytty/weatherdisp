using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;

using WeatherData;

namespace weatherdispTest
{
    
    class Program
    {

        static void Main(string[] args)
        {


            WeatherReading w = new WeatherData.WeatherReading();

            Console.WriteLine(w.DataRead);
            Console.WriteLine("Outdoor Temp: " + w.OutdoorTemperature);
            Console.WriteLine("Outdoor Humidity: " + w.OutdoorHumidity);
            Console.WriteLine("Indoor Temp: " + w.IndoorTemperature);
            Console.WriteLine("Indoor Humidity: " + w.IndoorHumidity);
            Console.WriteLine("Absolute Pressure: " + w.PressureAbsolute);
            Console.WriteLine("Relative Pressure: " + w.PressureRelative);
            Console.WriteLine("");

            Run();


        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {

            using (FileSystemWatcher watch = new FileSystemWatcher())
            {

                watch.Path = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
                watch.NotifyFilter = NotifyFilters.LastWrite;
                watch.Filter = "currdat.lst";

                watch.Changed += new FileSystemEventHandler(watch_Changed);
                watch.EnableRaisingEvents = true;

                Console.WriteLine("Press \'q\' to quit.");
                while (Console.Read() != 'q') ;
            }
        }

       static void watch_Changed(object sender, FileSystemEventArgs e)
        {
            
            WeatherReading newReading = new WeatherReading();
            Console.WriteLine("New data at " + newReading.DataRead);
            Console.WriteLine("Outdoor Temp: " + newReading.OutdoorTemperature);
            Console.WriteLine("Outdoor Humidity: " + newReading.OutdoorHumidity);
            Console.WriteLine("Indoor Temp: " + newReading.IndoorTemperature);
            Console.WriteLine("Indoor Humidity: " + newReading.IndoorHumidity);
            Console.WriteLine("Absolute Pressure: " + newReading.PressureAbsolute);
            Console.WriteLine("Relative Pressure: " + newReading.PressureRelative);
            Console.WriteLine("");
            
        }
    }
}
