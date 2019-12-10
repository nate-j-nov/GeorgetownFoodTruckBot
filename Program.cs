using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace FoodTrucksApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PDF Extraction");
            string file = "C:\\Users\\natej\\Documents\\C#\\FoodTrucks.pdf";
            DataCleaner cleaner = new DataCleaner(file);
            TruckDisplayer Displayer = new TruckDisplayer(cleaner.FoodTruckList);
        }
    }
}

