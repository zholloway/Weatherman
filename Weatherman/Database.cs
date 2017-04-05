﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Weatherman
{
    class PastWeatherRequest
    {
        const string PathToDatabase = @"Server=localhost\SQLEXPRESS;Database=Weathermen;Trusted_Connection=True;";

        public static void StoreRequest(SqlConnection connection, String userName, WeatherSnapshot snapshot)
        {
            var query = @"INSERT INTO PastWeatherRequests (UserName, MainTemp, WindSpeed, SysSunset)"
                        + "Values (@PastWeatherRequests, @MainTemp, @WindSpeed, @SysSunset)";

            var cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@MainTemp", snapshot.main.temp);
            cmd.Parameters.AddWithValue("@WindSpeed", snapshot.wind.speed);
            cmd.Parameters.AddWithValue("@SysSunset", snapshot.sys.sunset);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}