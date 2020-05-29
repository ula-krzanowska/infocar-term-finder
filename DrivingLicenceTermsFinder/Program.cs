using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DrivingLicenceTermsFinder
{
    class Program
    {
        private static List<string> allowedCategories = new List<string>()
        {
            "A",
            "A1",
            "A2",
            "AM",
            "B",
            "B1",
            "BE",
            "C",
            "C1",
            "CE",
            "C1E",
            "D",
            "D1",
            "DE",
            "D1E",
            "T",
            "PT"
        };

        private static List<string> allowedWords = new List<string>()
        {
            "16",
            "15"
        };
    
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Podaj ID wordu:");
            string pobraneId = Console.ReadLine();
            
            while (allowedWords.Contains(pobraneId) == false)
            {
                Console.WriteLine("Błędne ID");
                Console.WriteLine("Podaj ID:");
                pobraneId = Console.ReadLine();
            }
            
            Console.WriteLine("Podaj kategorię:");
            string pobranaKat = Console.ReadLine();
            
            while (allowedCategories.Contains(pobranaKat) == false)
            {
                Console.WriteLine("Błędna kategoria");
                Console.WriteLine("Podaj kategorię:");
                pobranaKat = Console.ReadLine();
            }
            
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(
                $"https://info-car.pl/services/word/ajax/getSchedule?wordId={pobraneId}&examCategory={pobranaKat}&month=2020-06&_=1590592537434");
            String content = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(content);
            
            JObject o = JObject.Parse(content);
            // Console.WriteLine(o);
            
            IEnumerable<JToken> daty = o.SelectTokens("$..date");
            List<DateTime> sortedDates = new List<DateTime>();
            
            foreach (JToken token in daty)
            {
                DateTime data = DateTime.Parse(token.ToString(), CultureInfo.CreateSpecificCulture("pl-PL"));
                sortedDates.Add(data);
            }
            sortedDates.Sort();
            sortedDates.ForEach(d => Console.WriteLine(d));
            



            // while (true)
            // {
            //    
            //     Thread.Sl
        }
    }
}