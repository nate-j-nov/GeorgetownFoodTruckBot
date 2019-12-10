using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTrucksApp
{
    class FoodTruck
    {
        public string SitePermit { get; set; }
        public string BusinessName { get; set; }
        public string MondayLocation { get; set; }
        public string TuesdayLocation { get; set; }
        public string WednesdayLocation { get; set; }
        public string ThursdayLocation { get; set; }
        public string FridayLocation { get; set; }

        // Constructors
        public FoodTruck() { }
        public FoodTruck(string sitePermit)
        {
            SitePermit = sitePermit;
        }
    }
}
