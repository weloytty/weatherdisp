using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WeatherData
{

           
    public class WeatherReading
    {

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);

        const double ERROR_VALUE = -9999;


        public enum WeatherPicture{Rain,Cloud,Sun}
        public enum WeatherTendancies{Falling,Rising,NoChange}

        public string ProgramName { get; set; }
        public double ProgramVersion { get; set; }
        public double FileFormatVersion { get; set; }
        public double OutdoorTemperature { get; set; }
        public double OutdoorHumidity { get; set; } 
        public bool StormAlarm{get;set;}
        public double IndoorTemperature{get;set;}
        public double IndoorHumidity { get; set; }
        public double DewPoint { get; set; }
        public double WindChill { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double RainTotal { get; set; }
        public double TwentyFourHourRain { get; set; }
        public double OneHourRain { get; set; }
        public double PressureAbsolute { get; set; }
        public double PressureRelative { get; set; }
        public string WindDirectionName { get; set; }
        public DateTime DataRead { get; private set; }
        public WeatherTendancies WeatherTendancy{get; private set;}


        public WeatherReading()
        {
            string fileName = Environment.GetEnvironmentVariable("ALLUSERSPROFILE") + "\\currdat.lst";
            LoadData(fileName);
        }

        public WeatherReading(string fileName)
        {
            LoadData(fileName);
        }

        

        internal void LoadData(string fileName)
        {

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException();

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Can't find " + fileName);

            this.ProgramName = GetStringValue("header", "programm_name", fileName);
            this.ProgramVersion =GetDoubleValue("header","programm_version",fileName);
            this.FileFormatVersion =GetDoubleValue("header","file_format_version",fileName);
            this.OutdoorTemperature =GetDoubleValue("outdoor_temperature","deg_F",fileName);
            this.OutdoorHumidity = GetDoubleValue("outdoor_humidity", "percent", fileName);
            this.StormAlarm =GetBoolValue("outdoor_temperature","deg_F",fileName);
            this.IndoorTemperature = GetDoubleValue("indoor_temperature", "deg_F", fileName);
            this.IndoorHumidity = GetDoubleValue("indoor_humidity", "percent", fileName);
            this.DewPoint = GetDoubleValue("dewpoint", "deg_F", fileName);
            this.WindChill = GetDoubleValue("windchill", "deg_F", fileName);
            this.WindSpeed = GetDoubleValue("wind_speed", "mph", fileName);
            this.WindDirectionName = GetStringValue("wind_direction", "name",fileName);
            this.WindDirection = GetDoubleValue("wind_direction", "deg", fileName);
            this.RainTotal = GetDoubleValue("rain_total", "inch", fileName);
            this.TwentyFourHourRain = GetDoubleValue("rain_24h", "inch", fileName);
            this.OneHourRain = GetDoubleValue("rain_1h", "inch", fileName);
            this.PressureAbsolute  =GetDoubleValue("outdoor_temperature","deg_F",fileName);
            this.PressureRelative = GetDoubleValue("pressure_relative", "inHg", fileName);
            this.DataRead = GetDateTimeValue("time", "last_actualisation", fileName);
            this.WeatherTendancy = GetWeatherTendancy("weather_tendency","number",fileName);

            
        }
        private WeatherTendancies GetWeatherTendancy(string sectionName,string keyName,string fileName)
        {
            StringBuilder sb = new StringBuilder(1024);
            WeatherTendancies returnValue = WeatherTendancies.NoChange;
            uint result = GetPrivateProfileString(sectionName,keyName,"",sb,(uint)sb.Capacity,fileName);
            if(result >0)
            {
                switch(Int32.Parse(sb.ToString()))
                {
                    case 0:
                        break;
                    case 1:
                        returnValue = WeatherTendancies.Rising;
                        break;
                    case 2:
                        returnValue = WeatherTendancies.Falling;
                        break;
                    default:
                        break;
                }
            }
            return returnValue;



        }
        private DateTime GetDateTimeValue(string sectionName, string keyName, string fileName)
        {
            DateTime returnValue = DateTime.MinValue;

            StringBuilder sb = new StringBuilder(1024);
            uint result = GetPrivateProfileString(sectionName, keyName, "", sb, (uint)sb.Capacity, fileName);
            if (result > 0)
            {
                double timestamp = ERROR_VALUE;
                DateTime origin = new DateTime(1900, 1, 1, 0, 0, 0, 0);
                double.TryParse(sb.ToString(), out timestamp);
                returnValue = origin.AddSeconds(timestamp).ToLocalTime();
            }
            Debug.Assert(returnValue != DateTime.MinValue);

            return returnValue;
        }

        private bool GetBoolValue(string sectionName, string keyName, string fileName)
        {
            bool returnValue = false;

            StringBuilder sb = new StringBuilder(1024);
            uint result = GetPrivateProfileString(sectionName, keyName, "", sb, (uint)sb.Capacity, fileName);
            if (result > 0)
            {
                returnValue = sb.ToString() == "1" ? true : false;
            }
            Debug.Assert(result > 0);

            return returnValue;
        }

        private string GetStringValue(string sectionName, string keyName, string fileName)
        {
            string returnValue = "";

            StringBuilder sb = new StringBuilder(1024);
            uint result = GetPrivateProfileString(sectionName, keyName, "", sb, (uint)sb.Capacity, fileName);
            if (result > 0)
            {
                returnValue = sb.ToString();
            }

            Debug.Assert(returnValue != "");

            return returnValue;
        }

        private double GetDoubleValue(string sectionName, string keyName,string fileName)
        {
            double returnValue =ERROR_VALUE;
            StringBuilder sb = new StringBuilder(1024);

            uint result = GetPrivateProfileString(sectionName, keyName, "", sb,(uint) sb.Capacity, fileName);
            if (result > 0)
            {
                double temp = ERROR_VALUE;
                double.TryParse(sb.ToString(), out temp);
                returnValue = temp;
            }

            Debug.Assert(returnValue != ERROR_VALUE);
            return returnValue;
        }











    }
}
