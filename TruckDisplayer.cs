using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTrucksApp
{
    class TruckDisplayer
    {
        // Class members
        public List<FoodTruck> TrucksInGeorgetown = new List<FoodTruck>();
        public List<FoodTruck> FoodTrucks = new List<FoodTruck>();
        public DayOfWeek Today = DateTime.Today.DayOfWeek;

        //Constructors
        public TruckDisplayer() { }
        public TruckDisplayer(IEnumerable<FoodTruck> foodtruckList)
        {
            FoodTrucks.AddRange(foodtruckList);
            GetGeorgetownFoodTrucks();
            PrintGeorgetownFoodTrucks();
        }

        public void GetGeorgetownFoodTrucks()
        {
            switch (Today)
            {
                case DayOfWeek.Monday:
                    foreach(var ft in FoodTrucks)
                    {
                        if(ft.MondayLocation == "Georgetown")
                            TrucksInGeorgetown.Add(ft);
                        
                    }
                    break;

                case DayOfWeek.Tuesday:
                    foreach(var ft in FoodTrucks)
                    {
                        if (ft.TuesdayLocation == "Georgetown")
                            TrucksInGeorgetown.Add(ft);
                    }
                    break;

                case DayOfWeek.Wednesday:
                    foreach(var ft in FoodTrucks)
                    {
                        if (ft.WednesdayLocation == "Georgetown")
                            TrucksInGeorgetown.Add(ft);
                    }
                    break;

                case DayOfWeek.Thursday:
                    foreach(var ft in FoodTrucks)
                    {
                        if (ft.ThursdayLocation == "Georgetown")
                            TrucksInGeorgetown.Add(ft);
                    }
                    break;

                case DayOfWeek.Friday:
                    foreach(var ft in FoodTrucks)
                    {
                        if (ft.FridayLocation == "Georgetown")
                            TrucksInGeorgetown.Add(ft);
                    }
                    break;

                default:
                    Console.WriteLine("Foodtrucks aren't in Georgetown over the weekends :(");
                    break;
            }
        } 

        public void PrintGeorgetownFoodTrucks()
        {
            foreach (var ft in TrucksInGeorgetown)
                Console.WriteLine(ft.BusinessName);
        }
    }
}
