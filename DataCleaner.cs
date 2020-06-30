using System;
using System.Collections.Generic;
using System.Text;
using iText;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using System.IO;
using System.Text.RegularExpressions;
namespace FoodTrucksApp
{
    class DataCleaner
    {
        // Member declarations
        public string TotalText { get; set; }
        public List<string> DividedText = new List<string>();
        public List<FoodTruck> FoodTruckList = new List<FoodTruck>();
        private string _patternSiteNumber = @"VSP-\d{5}";
        private string _patternBusinessName = @"(VSP-\d{5})(.*?)(Noma|OFF|Patriots Plaza|Georgetown|Virginia Ave \(State Dept\)|Union Station|Farragut Square 17th St|L\WEnfant Plaza|Waterfront Metro|Navy Yard/Capital River Front|Metro Center|Franklin Square)";

        //Constructors
        public DataCleaner() { }
        public DataCleaner(string filePath)
        {
            GetDataFromPDF(filePath);
            CreateArrayOfStrings();
            SetSitePermitNumber();
            SetBusinessName();
            SetDailyLocations();
            PrintSNNameAndLocations();
        }

        /// <summary>
        /// Extracts data from the foodtruck lottery pdf found online. 
        /// </summary>
        /// <param name="file"></param>
        protected void GetDataFromPDF(string file)
        {
            PdfReader pdfReader = new PdfReader(file);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                TotalText += currentText;
            }
        }

        // Separates text into an array of lines, each to represent one food truck. 
        protected void CreateArrayOfStrings()
        {
            string[] tempDividedText = TotalText.Split(new[] { "\n" }, StringSplitOptions.None);
            string patternIfMultipleLines = @"VSP-\d{5}\s+(Noma|OFF|Patriots Plaza|Georgetown|Virginia Ave \(State Dept\)|Union Station|Farragut Square 17th St|L\WEnfant Plaza|Waterfront Metro|Navy Yard/Capital River Front|Metro Center|Franklin Square)";

            for (int x = 0; x < tempDividedText.Length; x++)
            {
                //Console.WriteLine(tempDividedText[x]);
                Match matchSitePermitNumber = Regex.Match(tempDividedText[x], _patternSiteNumber);
                Match matchWhenDoubleSpace = Regex.Match(tempDividedText[x], patternIfMultipleLines);
                if (matchSitePermitNumber.Success)
                {
                    if (matchWhenDoubleSpace.Success)
                    {
                        tempDividedText[x] = tempDividedText[x].Insert(matchSitePermitNumber.Length, tempDividedText[x - 1]);
                        var matchToGetOtherPartOfName = Regex.Match(tempDividedText[x], _patternBusinessName);
                        tempDividedText[x] = tempDividedText[x].Insert(matchSitePermitNumber.Length, " " + tempDividedText[x + 1] + " / ");
                    }
                    DividedText.Add(tempDividedText[x].Replace(",", ""));
                }
            }
        }

        protected void PrintDividedText()
        {
            foreach (var s in DividedText)
            {
                Console.WriteLine(s);
            }
        }

        protected Match GetSitePermitNumber(string lineOfText)
        {
            Match sitePermitNumber = Regex.Match(lineOfText.Replace(" ", String.Empty), _patternSiteNumber);
            if (sitePermitNumber.Success)
            {
                return sitePermitNumber;
            }
            else
            {
                return null;
            }
        }

        // Turn Get and Set Permit number into one method. 
        protected void SetSitePermitNumber()
        {
            foreach (var t in DividedText)
            {
                var sitePermitNumber = GetSitePermitNumber(t);
                if (sitePermitNumber != null)
                {
                    FoodTruckList.Add(new FoodTruck(sitePermitNumber.ToString()));
                }
            }
        }

        protected Match GetBusinessName(string lineOfText)
        {
            var businessName = Regex.Match(lineOfText, _patternBusinessName);
            if (businessName.Success)
            {
                return businessName;
            }
            else
            {
                Console.WriteLine("Pattern not matched");
                return null;
            }
        }

        protected void SetBusinessName()
        {
            // Need to be sure that every business name goes to the proper number. 
            for (int x = 0; x < DividedText.Count; x++)
            {
                var foodTruckBusinessName = GetBusinessName(DividedText[x]).Groups[2].ToString();
                try
                {
                    if (foodTruckBusinessName.Contains("L'Enfant Plaza") || foodTruckBusinessName.Contains("Virginia Ave (State Dept)"))
                    {
                        Console.WriteLine("L'Enfant or Virginia Ave found. This is a test");
                        Console.WriteLine("The corresponding site permit number: {0}" + Environment.NewLine, FoodTruckList[x].SitePermit);
                    }
                    FoodTruckList[x].BusinessName = foodTruckBusinessName;
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        protected void SetDailyLocations()
        {

            string dailyLocationPattern = @"Noma|OFF|Patriots Plaza|Georgetown|Virginia Ave \(State Dept\)|Union Station|Farragut Square 17th St|L\WEnfant Plaza|Waterfront Metro|Navy Yard/Capital River Front|Metro Center|Franklin Square";
            //var dailyLocation = Regex.Match(lineOfText, dailyLocationPattern);
            for (int x = 0; x < DividedText.Count; x++)
            {

                // Sets Monday's location
                var mondayLocation = Regex.Match(DividedText[x], dailyLocationPattern);
                FoodTruckList[x].MondayLocation = mondayLocation.ToString();

                // Set Tuesday's Locations
                var tuesdayLocation = mondayLocation.NextMatch();
                FoodTruckList[x].TuesdayLocation = tuesdayLocation.ToString();

                //Etc...
                var wednesdayLocation = tuesdayLocation.NextMatch();
                FoodTruckList[x].WednesdayLocation = wednesdayLocation.ToString();

                var thursdayLocation = wednesdayLocation.NextMatch();
                FoodTruckList[x].ThursdayLocation = thursdayLocation.ToString();

                var fridayLocation = thursdayLocation.NextMatch();
                FoodTruckList[x].FridayLocation = fridayLocation.ToString();

            }
        }

        // Test functions
        protected void PrintFoodTruckSiteNumberTESTFUNCTION()
        {
            foreach (var ft in FoodTruckList)
            {
                Console.WriteLine(ft.SitePermit.ToString());
            }
        }

        protected void PrintFoodTruckSNAndNameTESTFUNCTION()
        {
            Console.WriteLine("Site Permit Business Name");
            foreach (var ft in FoodTruckList)
            {
                Console.WriteLine($"{ft.SitePermit}  {ft.BusinessName}");
            }
        }
        protected void PrintSNNameAndLocations()
        {
            Console.WriteLine("Site Permit Business Name Monday Tuesday");
            foreach (var ft in FoodTruckList)
            {
                Console.WriteLine($"{ft.SitePermit}  {ft.BusinessName}  {ft.MondayLocation}  {ft.TuesdayLocation}   {ft.WednesdayLocation}  {ft.ThursdayLocation}   {ft.FridayLocation}");
            }
        }
    }
}
