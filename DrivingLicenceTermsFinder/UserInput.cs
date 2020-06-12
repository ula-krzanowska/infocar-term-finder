using System;
using System.Collections.Generic;
using System.Linq;

namespace DrivingLicenceTermsFinder
{
    public class UserInput
    {
        public DateTime GetDate(string dateName)
        {
            while (true)
            {
                Console.Write($"Define {dateName} date [DD/MM/YYYY]: ");
                var userDateRange = Console.ReadLine();
                var dateSuccess = DateTime.TryParse(userDateRange, out DateTime date);
                if (dateSuccess)
                {
                    Console.WriteLine();
                    return date;
                }
                Console.WriteLine($"Wrong {dateName} format.");
                Console.WriteLine();
            }
        }

        public T Get<T>(string itemName, List<T> allowedItems)
        {
            for (int i = 0; i < allowedItems.Count; i++)
            {
                Console.WriteLine($"[{i}]: {allowedItems[i]}");
            }
            while (true)
            {
                Console.WriteLine();
                Console.Write($"Choose {itemName}: ");
                string userInput = Console.ReadLine();
                bool isSelectedIndexInt = int.TryParse(userInput, out int selectedIndex);
                if (isSelectedIndexInt && allowedItems.ElementAtOrDefault(selectedIndex) != null)
                {
                    Console.WriteLine();
                    return allowedItems[selectedIndex];
                }
                Console.WriteLine($"Wrong {itemName}.");
                Console.WriteLine();
            }
        }
    }
}