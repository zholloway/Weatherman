using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;

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
            var url = $"api.openweathermap.org/data/2.5/weather?zip={userZip}&APPID=274e4207010e8ea9bc2a679922db5b8d";
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

            Console.ReadLine();
        }
    }
}
