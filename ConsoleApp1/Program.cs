using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BoatRace
{
    class Program
    {
        //these are the kinds of boats that can be raced
        static List<string> boatChoices = new List<string>()
            {
                "Sailboat", "Trawler", "PontoonBoat", "SpeedBoat"
            };
        static Menu_CLI makeSelection = new Menu_CLI();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Boat Racing");

            //user selects a boat
            Console.WriteLine("\nChoose boats to race:");// + ListBoatChoices())

            string boatChoice = boatChoices[makeSelection.SelectionMenu(boatChoices) - 1];
            //determines boat selected by calling menu_cli
            //sends boatchoices to the menu and returns the index for the boat selected
            //string is completed here to set which type of boat is being raced
            //actual boats are created below                                                                    

            //Create a list to hold boats and their properties
            //Build the Boat--right now, builds four boats to race but could be changed to 
            //collect a user input to vary the number of boats to race
            List<Boat> boatsForRace = CreateTheBoats(boatChoice, boatChoices.Count);

            //name the boats
            Console.Write("Would you like to name your boat? (y or n) ");
            string answer = Console.ReadLine();

            if (answer == "y")
            {
                boatsForRace[0].Name = makeSelection.UserNamesBoat();
            }

            //name rest of boats
            NameTheBoats(answer, boatsForRace);

            //assign horsepower, base movement rate, and captains to engines
            AssignBoatProperties(boatsForRace);

            Console.WriteLine("\nBoat Captains improve your boat's speed.\n" +
                "They are ranked from 1 to 10, 10 being the most experienced");

            //show the boat hp, captain, and avg speed            
            Console.WriteLine(DisplayBoatProperties(boatsForRace));

            //******************select race course***************************//
            //choose a course
            RaceCourse rc = new RaceCourse();
            Console.WriteLine("Choose a race course");

            //currently courseSelected is only needed to determine the number of legs and the design of each leg(curve or straight)
            string courseSelected = rc.RaceCourseChoices()[makeSelection.SelectionMenu(rc.RaceCourseChoices()) - 1];

            //display the course selected
            Console.WriteLine("course selected: " + courseSelected);

            //create race course--changed rc to boatRaceCourse here to read code easier
            RaceCourse boatRaceCourse = rc;//new RaceCourse(courseSelected);//why create a new object?  Try using rc through whole race cycle
            boatRaceCourse.RaceCourseConditions();//sets conditions for course displayed below

            //****************display current conditions on the water*******************
            DisplayConditionsOnTheWater(boatRaceCourse);

            //Race Course = leg 1 + leg 2 + leg 3, etc.; number of legs depends on course selected.

            //********************race simulator************************************//
            //needs to call the same process only no results are printed out except the number of wins each boat has at the end as a percentage

            //*******print out results for first leg*********************************//
        } 
        private static void RaceSim(Boat boatsForRace, RaceCourse boatRaceCourse, string courseSelected, int simOrActual)
        { 
            int boatDirection = 0;
            int x = 0;//how else do I keep conditions for leg from printing only once?
            List<int> courseLegTypes = new List<int>();
            courseLegTypes.AddRange(boatRaceCourse.TypesOfLegsInCourse(courseSelected));

            //this list holds the cumulative leg time of the boats
            List<double> cumulativeLegTimesOfEachBoat = new List<double>();

            for (int i = 0; i < courseLegTypes.Count; i++)
            {
                Console.WriteLine("\nLeg " + (i + 1) + ":  " + (courseLegTypes[i] == 0 ? "straight" : "curve")); ;

                cumulativeLegTimesOfEachBoat.Clear();//needs to be cleared for each leg

                foreach (Boat boat in boatsForRace)
                {
                    //calculate first leg speed
                    /*
                     * get average speed in knots,
                     * increase/decrease boat speed based on hp
                     * calculate captain bonus
                     * calculate water condition bonus (based on Leg() method)  (negative or positive)
                     * calculate final speed adjustment
                     * write out results
                     * NOTE: current boat speed can be calculated in Boat class because it uses Boat properties and methods to calculate speed
                     */

                    //boat speed takes into account current, wind, chop, captain and engine horsepower
                    //the more horsepower, the greater deviation in speed as larger engines are more prone to break down
                    /*boat speed formula = 
                     * average speed
                     * (average speed * deviation)  
                     * (average speed * engineHorspower bonus) Increase Boat Speed based on Horsepower
                     * average speed * captain bonus
                     * average speed * racecourse effects
                     * */

                    //double currentBoatSpeed = boat.AverageSpeedInKnots;                
                    //currentBoatSpeed += boat.IncreaseBoatSpeed(boat.EngineHorsepower);

                    boatDirection = x == 0 ? boatRaceCourse.StartDirection : boatRaceCourse.NewLegDirection(boatDirection);
                    double currentBoatSpeed = Math.Round(boat.AverageSpeedInKnots * (boat.IncreaseBoatSpeed(boat.EngineHorsepower) +
                        boat.CaptainBonus + boatRaceCourse.CourseLeg(courseLegTypes[i], boatDirection, i + 1)), 2);

                    currentBoatSpeed += boat.SpeedAdjustment(currentBoatSpeed, boat.EngineHorsepower);
                    //time formula for distance traveled in a straight is 100 / currentBoatSpeed
                    //time formula for distance traveled in a curve is 25 / (currentBoatSpeed with adjustment for turn)



                    if (x == 0)//loop prints out race conditions for first leg; x condition prevents it from printing more than once
                    {
                        //****condition reports for present leg of race
                        
                        Console.WriteLine("Water Current Report: " + boatRaceCourse.WaterCurrentReport);
                        Console.WriteLine("Wind Report: " + boatRaceCourse.WindReport);
                        //******condition reports

                        //header for leg data
                        Console.WriteLine("\nName\t\tHP\tCapt.\tLg Sp\tLg T\tTot T\tPlace\n");
                    }
                    //leg results

                    double thisLegTime = Math.Round((boatRaceCourse.StraightDistanceOfLeg / currentBoatSpeed) / 10, 2);
                    boat.BoatTimeForDistance += thisLegTime;
                    Console.WriteLine(boat.Name + "\t" + boat.EngineHorsepower +
                        "\t" + boat.Captain + "\t" + Math.Round(currentBoatSpeed, 2) +
                        "\t" + thisLegTime + "\t" + Math.Round(boat.BoatTimeForDistance / 10, 2) +
                        "\t" + boat.AverageSpeedInKnots);

                    cumulativeLegTimesOfEachBoat.Add(boat.BoatTimeForDistance);
                    /* how to determine position of each boat
                     * use x counter (x % 4 = 0 then determine positions for the specific leg)
                     * the BoatTimeForDistance must be determined for each boat in leg before determining position
                     * 
                     */
                    if (x % 4 == 0)
                    {
                        //call method to get positions and send cumulativeLegTimesOfEachBoat as argument
                    }
                    x++;//counter for foreach loop
                }//end of leg
                List<string> boatPositions = new List<string>();
                
                boatPositions.AddRange(CalculateBoatPositionsAfterEachLeg(cumulativeLegTimesOfEachBoat, boatsForRace, boatsForRace.Count));
                Console.WriteLine("\nPositions after Leg " + (i + 1) + ": 1) " + boatPositions[0] + ", 2) " + boatPositions[1] + ", 3) " +
                    boatPositions[2] + ", 4) " + boatPositions[3]);
            }//end of loop through all legs


        }

        private static void DisplayConditionsOnTheWater(RaceCourse boatRaceCourse)
        {
            Console.WriteLine("\nCurrent Conditions on the water");
            Console.WriteLine("Current: " + boatRaceCourse.WaterCurrent + "\nChop: " + boatRaceCourse.WaterChop
                + "\nWind: " + boatRaceCourse.Wind + "\nWind Direction: " + boatRaceCourse.WindDirection +
                "\nWater Chop: " + boatRaceCourse.waterChopTextCondition + "\nWater Current Direction: " +
                boatRaceCourse.WaterCurrentDirection + "\nStarting Direction: " + boatRaceCourse.StartDirection);
        }

        private static string DisplayBoatProperties(List<Boat> boatsForRace)
        {
            string boatInfo = "\nName\t\tHP\tCapt.\tAvg Speed\n";//header
            foreach (Boat boat in boatsForRace)
            {
                boatInfo += boat.Name + "\t" + boat.EngineHorsepower +
                    "\t" + boat.Captain + "\t" + boat.AverageSpeedInKnots + "\n";
            }
            return boatInfo;
        }

        private static void AssignBoatProperties(List<Boat> boatsForRace)
        {
            foreach (Boat boat in boatsForRace)
            {
                //assign horsepower, base movement rate, and  to boats
                boat.EngineHorsepower = boat.Horsepower(boat.GetType().Name);

                //assign captains 
                Random num = new Random();
                boat.Captain = num.Next(9) + 1;
                boat.CaptainBonus = boat.EngineHorsepower <= 30 ? boat.Captain * .05 : boat.Captain * .01;

                //assign base movement rate
                boat.AverageSpeedInKnots += boat.SetAverageSpeedForStartOfRace();
            }
        }

        private static string ListBoatChoices()//this needs to go into Menu_CLI I think
        {
            int i = 0;
            string theBoatChoiceString = "";
            foreach (string boat in boatChoices)
            {
                theBoatChoiceString += "\n\t" + (i+1) + ") " + boat;
                i++;
            }
            return theBoatChoiceString;
        }
        private static List<Boat> CreateTheBoats(string boatSelection, int numberOfBoatsToCreate)
        {
            //Build the Boat--right now, builds four boats to race but could be changed to 
            //collect a user input to vary the number of boats to race
            List<Boat> list = new List<Boat>();
            for (int i = 0; i < boatChoices.Count; i++)
            {                
                if (boatSelection == "Sailboat")
                {
                    //Create a sailboat
                    SailBoat boat = new SailBoat(1, "sloop", "diesel", 34, false);
                    list.Add(boat);//this the Boat Type list

                    /*
                        * referencing a sailplan is an issue. 
                        * Might be solved through a dictionary once name is given.  
                        * Name could be key.
                    */
                    //next line is probably not necessary
                    boat.RacingBoats.Add(boat);//do I need this?
                }
                else if (boatSelection == "Trawler")
                {
                    //Create a Trawler
                    Trawler boat = new Trawler(1, "diesel", 38, false);
                    list.Add(boat);//this the Boat Type list

                    boat.racingBoats.Add(boat);//do I need this?

                }
                else if (boatSelection == "PontoonBoat")
                {
                    //Create a Pontoon Boat
                    PontoonBoat boat = new PontoonBoat(2, "gas", 30, true);
                    list.Add(boat);//this the Boat Type list

                    boat.racingBoats.Add(boat);//do I need this?

                }
                else if (boatSelection == "SpeedBoat")
                {
                    //Create a speedboat
                    SpeedBoat boat = new SpeedBoat(1, "gas", 35, true);
                    list.Add(boat);//this the Boat Type list

                    boat.racingBoats.Add(boat);//do I need this?

                }
                else
                {
                    Console.WriteLine("You have not selected an available boat.");
                };
                
            }
            return list;
        }
        private static void NameTheBoats(string nameTheBoatAnswer, List<Boat> boatsForRace)
        {
            List<int> boatNameIndexes = new List<int>();//collects random generated numbers to make sure name is not duplicated
            for (int i = nameTheBoatAnswer == "y" ? 1 : 0; i < boatsForRace.Count; i++)
            {
                Random num = new Random();
                int nameIndex;
                do//makes sure duplicate numbers aren't used so each boat name is unique
                {
                    nameIndex = num.Next(boatsForRace[i].boatNames.Count());
                }
                while (boatNameIndexes.Contains(nameIndex));

                boatsForRace[i].Name = boatsForRace[i].boatNames[nameIndex];
                boatNameIndexes.Add(nameIndex);

            }
        }

        private static List<string> CalculateBoatPositionsAfterEachLeg(List<double> legTimes, List<Boat> boats, int numOfBoats)
        {
            List<string> list = new List<string>();
            string[] array = new string[numOfBoats];
            legTimes.Sort();
            foreach (Boat boat in boats)
            {
                int boatTimeInList = legTimes.IndexOf(boat.BoatTimeForDistance);
                array[boatTimeInList] = boat.Name;
            }
            return array.ToList();
        }
    }
     
        
        


    }


           
            
