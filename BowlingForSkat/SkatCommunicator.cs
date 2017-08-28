using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BowlingForSkat
{
    class SkatCommunicator
    {
        public static async Task<BowlingServerGETData> GetBowlingResults()
        {

            Uri endPoint = new Uri("http://13.74.31.101/api/points");

            using (var client = new HttpClient())
            {
                string repUrl = endPoint.ToString();
                HttpResponseMessage response = await client.GetAsync(repUrl);
                if (response.IsSuccessStatusCode)
                {                 
                    string result = await response.Content.ReadAsStringAsync();

                    BowlingServerGETData scores = (BowlingServerGETData)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(BowlingServerGETData));
                    return scores;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<string> VerifyBowlingResults(NewBowlingMatch match)
        {
            Uri endPoint = new Uri("http://13.74.31.101/api/points");

            using (var client = new HttpClient())
            {
                BowlingServerPOSTData postData = new BowlingServerPOSTData(match);
                string repUrl = endPoint.ToString();
                var serializedData = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(repUrl, serializedData);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
