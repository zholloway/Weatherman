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
        const string PathToDatabase = @"Server=localhost\SQLEXPRESS;Database=Weatherman;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            //prompt user for Name and Zip
            Console.WriteLine("Please enter your name:");
            var userName = Console.ReadLine().Trim(' ');
            //if returning user, display their past requests
            using (var connection = new SqlConnection(PathToDatabase))
            {
                connection.Open();
                var returnStatus = PastWeatherRequest.CheckForReturningUser(userName, connection);
                connection.Close();

                if (returnStatus) 
                {
                    Console.WriteLine($"Welcome back, {userName}. Your past requests were:");
                    connection.Open();
                    PastWeatherRequest.DisplayPastRequests(userName, connection);
                    connection.Close();
                }
            }

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
            }

            //create new WeatherSnaphot for this request
            var currentWeather = JsonConvert.DeserializeObject<WeatherSnapshot>(rawResponse);

            //write out the current weather to user
            Console.WriteLine($"{userName}, the current temperature is {currentWeather.main.temp}. The wind speed is {currentWeather.wind.speed}. Sunset will be at {currentWeather.sys.sunset}.");

            //save userName and currentWeather to database
            using(var connection = new SqlConnection(PathToDatabase))
            {
                connection.Open();
                PastWeatherRequest.StoreRequest(connection, userName, currentWeather);
                Console.WriteLine("Your request has been stored. Thank you.");
                connection.Close();
            }

            Console.ReadLine();
        }
    }
}
