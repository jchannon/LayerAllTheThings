using System;
using System.Reflection;
using Microsoft.Owin.Hosting;

namespace TraditionalLayering
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://+:5678"))
            {
                Console.WriteLine("Running on http://localhost:5678");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
