using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Zadatak_1
{
    /// <summary>
    /// This program simulates charge and discharge of trucks using threads.
    /// </summary>
    class Program
    {
        static Truck[] trucks = new Truck[10];
        static int[] routes;
        static List<int> numbersDivisibleBy3;
        static int[] choosenRoutes;
        static object locker = new object();
        static EventWaitHandle canStartChoosingRoutes = new AutoResetEvent(false);
        static EventWaitHandle canStartCharging = new AutoResetEvent(false);

        /// <summary>
        /// This method generates random numbers that represents possible routes of trucks and writes them in file.
        /// </summary>
        static void GenerateRoutes()
        {
            //locking block of code, that only one thread can access this object at the same time  
            lock (locker)
            {
                //initialization of array of routes
                routes = new int[1000];
                //filling array of routes with random numbers
                Random random = new Random();
                for (int i = 0; i < routes.Length; i++)
                {
                    routes[i] = random.Next(1, 5001);
                }
                //writing every number from array of routes to file
                StreamWriter writer = new StreamWriter(@"../../Routes.txt");
                foreach (int route in routes)
                {
                    writer.WriteLine(route);
                }
                writer.Close();
                Console.WriteLine("Marks of possible routes are generated.");
                //sending signal to another thread that routes are generated  
                canStartChoosingRoutes.Set();
            }
        }
        /// <summary>
        /// This method reads possible routes from file, and choose ten distinct, for every truck one route.
        /// </summary>
        static void ChooseRoutes()
        {
            //if routes are not generated, wait for it
            if (routes == null)
            {
                //if cannot generate routes for 3 seconds
                canStartChoosingRoutes.WaitOne(3000);
            }
            //locking block of code, that only one thread can access this object at the same time
            lock (locker)
            {

                //initialization of list of numbers that are divisible by 3
                numbersDivisibleBy3 = new List<int>();
                //reading all lines from file
                string[] lines = File.ReadAllLines(@"../../Routes.txt");
                //converting array of string to array of int
                int[] numbers = Array.ConvertAll(lines, x => Int32.Parse(x));
                //finding numbers that are divisible by 3
                numbersDivisibleBy3 = numbers.Where(x => x % 3 == 0).ToList();
                //sorting list
                numbersDivisibleBy3.Sort();
                //finding 10 distinct smallest number and putting them in new array
                choosenRoutes = new int[10];
                List<int> distinctRoutes = numbersDivisibleBy3.Distinct().ToList();
                choosenRoutes = distinctRoutes.GetRange(0, 10).ToArray();
                Console.Write("Choosen routes are: ");
                foreach (int route in choosenRoutes)
                {
                    Console.Write(route + " ");
                }
                Console.WriteLine("\nCharge of trucks can start.");
                //sending a signal that routes are chosen and trucks can start with charging
                canStartCharging.Set();
            }
        }
        static void Main(string[] args)
        {
            //creating 10 objects of class Truck and setting their names
            for (int i = 0; i < trucks.Length; i++)
            {
                trucks[i] = new Truck() { Name = string.Format("Truck_{0}", i + 1) };
            }
            Thread menager = new Thread(ChooseRoutes);
            Thread generateRoutes = new Thread(GenerateRoutes);
            menager.Start();
            generateRoutes.Start();
            //waiting for signal that routes are chosen and can start with charging of trucks
            canStartCharging.WaitOne();
            Console.ReadLine();
        }
    }
}
