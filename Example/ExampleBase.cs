using System;
using System.Text.RegularExpressions;
using AlmostFantastic.Messaging;

namespace Example
{
    public abstract class ExampleBase
    {
        public ExampleBase()
        {
            var title = GetType().Name.Replace("Example", "");
            title = Regex.Replace(title, "([A-Z])", " $1");
            title = "Example:" + title.ToLower();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("-==[ " + title + " ]==-");
            Console.ResetColor();
            Console.WriteLine();
            
            BroadcastU.Instance.RemoveAllSubscribers();
            
        }
    }
}