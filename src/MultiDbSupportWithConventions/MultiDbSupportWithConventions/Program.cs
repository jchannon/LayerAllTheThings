namespace MultiDbSupportWithConventions
{
    using System;

    using Microsoft.Owin.Hosting;

    internal class Program
    {
        private static void Main(string[] args)
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