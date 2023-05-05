using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;

namespace VictorNovember.Utilities
{
    internal static class APIHandler
    {
        static string json;
        static JObject jsonParsed;

        static APIHandler()
        {
            StreamReader streamReader;

            //try to read the config file
            try
            {
                streamReader = new StreamReader("Config.json");
                json = streamReader.ReadToEnd();
                jsonParsed = JObject.Parse(json);
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Config.json not found! Exiting...");
                Environment.Exit(0);
            }
        }
        public static string ReturnSavedValue(string obj)
        {
            var valueToRetrieve = jsonParsed[obj];
            return (string)valueToRetrieve;
        }
        public static async Task<string> HttpRequestAndReturnJson(HttpWebRequest request)
        {
            string httpResponse = null;

            try
            {
                //Create request to specified url
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)(await request.GetResponseAsync()))
                {
                    //Process the response
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                        httpResponse += await reader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                return await Task.FromException<string>(e);
            }
            //And if no errors occur, return the http response
            return await Task.FromResult(httpResponse);
        }
    }
}