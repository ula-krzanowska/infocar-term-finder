using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace DrivingLicenceTermsFinder
{
    class Program
    {
        static UserInput userInput = new UserInput();
        static InfoCarApi infoCarApi = new InfoCarApi();
        
        static async Task Main(string[] args)
        {
            var words = await infoCarApi.FetchWords();
            var categories = await infoCarApi.FetchCaegories();
            
            var word = userInput.Get("word", words);
            var category = userInput.Get("category", categories);
            var startDate = userInput.GetDate("start");
            var endDate = userInput.GetDate("end");

            while (true)
            {
                Console.WriteLine($"Looking for free terms ({category} {startDate.ToShortDateString()}-{endDate.ToShortDateString()})");
                var freeTerms = await lookForFreeTerms(startDate, endDate, word, category);
                if (freeTerms.Any())
                {
                    freeTerms.ForEach(d => Console.WriteLine($"* {d}"));
                    SystemSounds.Beep.Play();
                    Console.WriteLine();
                }
                Thread.Sleep(3000);
            }
        }

        private static async Task<List<DateTime>> lookForFreeTerms(DateTime startDate, DateTime endDate, Word word, string category)
        {
            int monthsBetween = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);

            var foundDates = new List<DateTime>();
            for (int i = 0; i <= monthsBetween; i++)
            {
                DateTime dateWithNewMonth = startDate.AddMonths(i);
                var month = dateWithNewMonth.Month;
                var year = dateWithNewMonth.Year;
                var dates = (await infoCarApi.FetchAvailableDates(word.Id, category, year, month))
                    .Where(d => d.Ticks >= startDate.Ticks && d.Ticks <= endDate.Ticks);
                foundDates.AddRange(dates);
            }
            return foundDates;
        }
    }
}