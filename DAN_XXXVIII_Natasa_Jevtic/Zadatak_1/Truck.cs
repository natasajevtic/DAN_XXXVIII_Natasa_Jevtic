

namespace Zadatak_1
{
    class Truck
    {
        public int Route { get; set; }
        public int TimeOfDelivery { get; set; }
        public int TimeOfCharge { get; set; }
        public int TimeOfDischarge { get; set; }
        public string Name { get; set; }

        public void Charge() { }       

        public void Delivery() { }
        
        public void Discharge() { }
    }
}
