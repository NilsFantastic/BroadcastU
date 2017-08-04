using System;
using System.Collections.Generic;
using AlmostFantastic.Messaging;

namespace Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new ExampleReceiveEventInLambdaFunction();
            new ExampleReceiveEventInNamedFunction();
            new ExampleRemoveNamedEventHandler();
            Console.ReadLine();
        }

        
    }
}