using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    class SampleCode
    {
        /*SailBoat firstBoat = new SailBoat(1, "sloop", "diesel", 34, false);
        Console.WriteLine(firstBoat.SailPlan);
            Console.WriteLine(firstBoat.Engine);
            //Boat1.Name = "Serenity";
            Console.Write("What is boat 1's name? ");
            //firstBoat.Name = Console.ReadLine();
            Console.WriteLine("Boat 1, " + firstBoat.Name +", is a" + (firstBoat.GetType().Name=="Sailboat"?firstBoat.SailPlan:"") + 
                " " + firstBoat.GetType().Name + ".\nIt is " + firstBoat.Length + " ft long with a " +
                firstBoat.Engine + " engine and " + firstBoat.CanBeTowed() + ".\n" +
                "It's average speed is " + firstBoat.AverageSpeedInKnots + " knots.");
            firstBoat.EngineHorsepower = firstBoat.Horsepower();
            Console.WriteLine(firstBoat.Name + " is traveling at " + firstBoat.BoatSpeed(firstBoat.AverageSpeedInKnots, firstBoat.EngineHorsepower) + " knots.");
            Console.WriteLine(firstBoat.Horsepower());
        public static List<SailBoat> CreateSailboatObject()
        {
            SailBoat boat = new SailBoat(1, "sloop", "diesel", 34, false);
            boat.RacingBoats.Add(boat);
            return boat.RacingBoats;
        }
        
        }
        */
        //Console.WriteLine(CreateSailboatObject());
        /*foreach(string item in theRaceBoats)
        {
            Console.WriteLine(item);
        }
        foreach (Boat item in boatsForRace)
        {
            Console.WriteLine(item.Engine);
        }

        string x = boatsForRace[1].Engine;
        Console.WriteLine(x);*/

        /*-------------------------------------
         * bool isParsable = false;
            
            while (!isParsable)
            {
                Console.WriteLine("Choose boats to race:" +
                "\n\t1) Sailboats\n\t2) Trawlers\n\t3) Pontoon Boats\n\t4) SpeedBoats");
                int boatChoiceSelection = Convert.ToInt32(Console.ReadLine());
        boatChoice = boatChoices[boatChoiceSelection - 1];
                isParsable = Int32.TryParse(boatChoiceSelection, out int num);
                
                if (num > 4)
                {
                    isParsable = false;//number selected must be between 1 and 4.
                }
                else if (isParsable)
                {
                    boatChoice = boatChoices[boatChoiceSelection - 1];
                    //boatChoice = isParsable ? (boatChoices[num - 1]) : "";   
                    
                }
            }---------------------------------------------------------------*/
    }
}
