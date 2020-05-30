using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var wordId = ReadStringFromUser("wordId", allowedWords);
            var userCategory = ReadStringFromUser("category", allowedCategories);
            var startDate = ReadDateFromUser("start");
            var endDate = ReadDateFromUser("end");
            
            
            int monthsBetween = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);
            
            for (int i = 0; i <= monthsBetween; i++)
            {
                Console.WriteLine("******");
                Console.WriteLine("******");
                Console.WriteLine("******");
                DateTime dateWithNewMonth = startDate.AddMonths(i);
                Console.WriteLine(dateWithNewMonth);
                Console.WriteLine("******");
                Console.WriteLine("******");
                Console.WriteLine("******");

                int m = dateWithNewMonth.Month;
                int y = dateWithNewMonth.Year;
                    
                HttpClient client = new HttpClient();
                string requestUri = $"https://info-car.pl/services/word/ajax/getSchedule?wordId={wordId}&examCategory={userCategory}&month={y}-{m}&_=1590592537434";
                //Console.WriteLine(requestUri);

                HttpResponseMessage response = await client.GetAsync(requestUri);
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
            }
            
            // Console.WriteLine(endDate.Subtract(startDate));

           
            
            
          
            



            // while (true)
            // {
            //    
            //     Thread.Sl
        }

        private static DateTime ReadDateFromUser(string dateName)
        {
            Console.WriteLine($"Define {dateName} date [DD/MM/YYYY]:");
            string userDateRange = Console.ReadLine();
            bool dateSuccess = DateTime.TryParse(userDateRange, out DateTime date);

            while (dateSuccess == false)
            {
                Console.WriteLine($"Wrong {dateName} format.");
                Console.WriteLine($"Define {dateName} date [DD/MM/YYYY]:");
                userDateRange = Console.ReadLine();
                dateSuccess = DateTime.TryParse(userDateRange, out date);
            }

            return date;
        }

        private static string ReadStringFromUser(string itemName, List<string> allowedItems)
        {
            Console.WriteLine($"Define {itemName}:");
            string userID = Console.ReadLine();

            while (allowedItems.Contains(userID) == false)
            {
                Console.WriteLine($"Wrong {itemName}.");
                Console.WriteLine($"Define {itemName}:");
                userID = Console.ReadLine();
            }

            return userID;
        }
    }
}