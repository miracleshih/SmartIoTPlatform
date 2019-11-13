using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public class WInfluxDB
    {
        InfluxDbClient influxDbClient;  // = new InfluxDbClient();   //"http://localhost:8086/", "admin", "pass.123", InfluxDbVersion.Latest);

        public WInfluxDB(string url, string user, string password)
        {
            influxDbClient = new InfluxDbClient(url, user, password, InfluxDbVersion.Latest);
        }

        public async void CreateDB(string DBName)
        {
            try
            {
                var response = await influxDbClient.Database.CreateDatabaseAsync(DBName);
            }
            catch (Exception)
            { }
        }


        public async void Insert(string DBName, Point PointData)
        {
            var response = await influxDbClient.Client.WriteAsync(PointData, DBName);
        }

        public async void Select(string DBName, string query)
        {
            // var query = "SELECT * FROM yowkoTest WHERE SensorState = 'active' ";
            var response = await influxDbClient.Client.QueryAsync(query, DBName);
        }

        public async void Delete(string DBName, string query)
        {
            var response = await influxDbClient.Client.QueryAsync(query, DBName);
        }

    }
}
