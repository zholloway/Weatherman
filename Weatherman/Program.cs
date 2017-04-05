using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace Weatherman
{
    class Program
    {       
        static void Main(string[] args)
        {
            //prompt user for Name and Zip
            Console.WriteLine("Please enter your name:");
            var userName = Console.ReadLine();
            Console.WriteLine("Please enter what zip code you would like to know the current weather conditions for:");
            var userZip = Console.ReadLine();

            //use userZip for zipcode in API url
            var url = $"http://api.openweathermap.org/data/2.5/weather?zip={userZip}&id=524901&APPID=3f619e1d5e1b959909efda2d7eae4b7e";
            Console.WriteLine(url);

            //create the WebRequest using the url
            var request = WebRequest.Create(url);

            //get a response to the request
            var response = request.GetResponse();

            //read the response and output as new weather(?)
            var rawResponse = String.Empty;
            using(var reader = new StreamReader(response.GetResponseStream()))
            {
                rawResponse = reader.ReadToEnd();
                Console.WriteLine(rawResponse);
            }

            //create new WeatherSnaphot for this request
            var currentWeather = JsonConvert.DeserializeObject<WeatherSnapshot>(rawResponse);

            //write out the current weather to user
            Console.WriteLine($"{userName}, the current temperature is {currentWeather.main.temp}. The wind speed is {currentWeather.wind.speed}. Sunset will be at {currentWeather.sys.sunset}.");

            //save userName and currentWeather to database

            Console.ReadLine();
        }
    }
}
