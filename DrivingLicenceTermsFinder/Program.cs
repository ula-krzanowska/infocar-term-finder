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
            Console.WriteLine("Podaj ID wordu:");
            string userID = Console.ReadLine();
            
            while (allowedWords.Contains(userID) == false)
            {
                Console.WriteLine("Błędne ID");
                Console.WriteLine("Podaj ID:");
                userID = Console.ReadLine();
            }
            
            Console.WriteLine("Podaj kategorię:");
            string userCategory = Console.ReadLine();
            
            while (allowedCategories.Contains(userCategory) == false)
            {
                Console.WriteLine("Błędna kategoria");
                Console.WriteLine("Podaj kategorię:");
                userCategory = Console.ReadLine();
            }
            
            Console.WriteLine("Podaj datę początkową w formacie DD/MM/RRRR:");
            string userDateRangeStart = Console.ReadLine();
            bool startDateSuccess = DateTime.TryParse(userDateRangeStart, out DateTime startDate);

            while (startDateSuccess == false)
            {
                Console.WriteLine("Błędny format daty");
                Console.WriteLine("Podaj datę:");
                userDateRangeStart = Console.ReadLine();
                startDateSuccess = DateTime.TryParse(userDateRangeStart, out startDate);
            }

            
            Console.WriteLine("Podaj datę końcową w formacie DD/MM/RRRR:");
            string userDateRangeEnd = Console.ReadLine();
            bool endDateSuccess = DateTime.TryParse(userDateRangeEnd, out DateTime endDate);

            while (endDateSuccess == false)
            {
                Console.WriteLine("Błędny format daty");
                Console.WriteLine("Podaj datę:");
                userDateRangeEnd = Console.ReadLine();
                endDateSuccess = DateTime.TryParse(userDateRangeEnd, out endDate);
            }

            // 1. bierzemy poczatkowa date
            //     2. w petli dodajemy po kazdym kroku 1 miesiac do DataType
            //         3. sprawdzamy czy nowo utworzona data jest w tym samym miesiacu i roku co koncowa. Jezeli tak to konczymy
            //             4. w kazdum przejsciu petli pobieramy terminy

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
                string requestUri = $"https://info-car.pl/services/word/ajax/getSchedule?wordId={userID}&examCategory={userCategory}&month={y}-{m}&_=1590592537434";
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
    }
}