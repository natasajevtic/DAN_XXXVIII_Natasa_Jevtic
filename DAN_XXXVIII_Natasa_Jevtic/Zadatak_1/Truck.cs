using System;
using System.Threading;

namespace Zadatak_1
{
    class Truck
    {
        public int Route { get; set; }
        public int TimeOfDelivery { get; set; }
        public int TimeOfCharge { get; set; }
        public int TimeOfDischarge { get; set; }
        public string Name { get; set; }
        static SemaphoreSlim semaphor = new SemaphoreSlim(2, 2);
        static Random r = new Random();
        static Barrier barrier = new Barrier(2);
        public static ManualResetEvent canStartDelivery = new ManualResetEvent(false);
        /// <summary>
        /// This method simulates charging of trucks two by two.
        /// </summary>
        public void Charge()
        {            
            semaphor.Wait();
            //waiting for the second thread so that the charging is always done in pairs
            barrier.SignalAndWait();
            TimeOfCharge = r.Next(500, 5000);
            Console.WriteLine("{0} is charging.", Name);
            Thread.Sleep(TimeOfCharge);
            Console.WriteLine("{0} is charged and charging lasted {1} miliseconds.", Name, TimeOfCharge);
            semaphor.Release();
            //sending a signal that truck is charged
            Program.canAssignmentRoutes.Signal();
        }       

        public void Delivery() { }
        
        public void Discharge() { }
    }
}
