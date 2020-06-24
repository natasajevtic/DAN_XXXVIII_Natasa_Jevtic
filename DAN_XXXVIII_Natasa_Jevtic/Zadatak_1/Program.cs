using System;

namespace Zadatak_1
{
    /// <summary>
    /// This program simulates charge and discharge of trucks using threads.
    /// </summary>
    class Program
    {
        static Truck[] trucks = new Truck[10];        

        static void Main(string[] args)
        {
            //creating 10 objects of class Truck and setting their names
            for (int i = 0; i < trucks.Length; i++)
            {
                trucks[i] = new Truck() { Name = string.Format("Truck_{0}", i + 1) };
            }
        }
    }
}
