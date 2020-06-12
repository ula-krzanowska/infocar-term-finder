using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace DrivingLicenceTermsFinder
{
    public class InfoCarApi
    {
        private static HttpClient client = new HttpClient();

        public async Task<List<DateTime>> FetchAvailableDates(string wordId, string userCategory, int year, int month)
        {
            string requestUri = $"https://info-car.pl/services/word/ajax/getSchedule?wordId={wordId}&examCategory={userCategory}&month={year}-{month}&_=1590592537434";

            HttpResponseMessage response = await client.GetAsync(requestUri);
            String content = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(content);

            IEnumerable<JToken> daty = o.SelectTokens("$..date");
            List<DateTime> sortedDates = new List<DateTime>();
            foreach (JToken token in daty)
            {
                DateTime data = DateTime.Parse(token.ToString(), CultureInfo.CreateSpecificCulture("pl-PL"));
                sortedDates.Add(data);
            }
            sortedDates.Sort();
            return sortedDates;
        }

        public async Task<List<Word>> FetchWords()
        {
            string requestUri = $"https://info-car.pl/services/word/ajax/getData";
            HttpResponseMessage response = await client.GetAsync(requestUri);
            String content = await response.Content.ReadAsStringAsync();
            return JObject
                .Parse(content)["words"]
                .Values<JProperty>()
                .Select(jsonWord => new Word(jsonWord.Name, (string) jsonWord.Value["name"]))
                .ToList();
        }
        
        public async Task<List<string>> FetchCaegories()
        {
            string requestUri = $"https://info-car.pl/services/word/ajax/getData";
            HttpResponseMessage response = await client.GetAsync(requestUri);
            String content = await response.Content.ReadAsStringAsync();
            return JObject
                .Parse(content)["categories"]
                .Select(c => (string) c)
                .ToList();
        }
    }
}