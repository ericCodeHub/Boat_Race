using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BoatRace
{
    public class Program
    {
        //these are the kinds of boats that can be raced
        static List<string> boatChoices = new List<string>()
            {
                "Sailboat", "Trawler", "PontoonBoat", "SpeedBoat"
            };
        static Menu_CLI makeSelection = new Menu_CLI();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Boat Racing!");
            string userWantsToPlay = "y";
            while (userWantsToPlay == "y")
            {
                userWantsToPlay = RunTheRace(userWantsToPlay);
            }
            Console.WriteLine("Okay, thanks for playing!");
        }
        static string RunTheRace(string userWantsToPlay)
        { 
            //user selects a boat type to race
            Console.WriteLine();
            string openingPrompt = "\nChoose boats to race:";
            string boatChoice = boatChoices[makeSelection.SelectionMenu(boatChoices,openingPrompt) - 1];
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
            string raceCourseSelectionPrompt="Choose a race course";

            //currently courseSelected is only needed to determine the number of legs and the design of each leg(curve or straight)
            string courseSelected = rc.RaceCourseChoices()[makeSelection.SelectionMenu(rc.RaceCourseChoices(),raceCourseSelectionPrompt) - 1];

            //display the course selected
            Console.WriteLine("course selected: " + courseSelected);

            //create race course--changed rc to boatRaceCourse here to read code easier
            RaceCourse boatRaceCourse = rc;//new RaceCourse(courseSelected);//why create a new object?  Try using rc through whole race cycle
            boatRaceCourse.RaceCourseConditions();//sets conditions for course displayed below

            //****************display current conditions on the water*******************
            DisplayConditionsOnTheWater(boatRaceCourse);

            //Race Course = leg 1 + leg 2 + leg 3, etc.; number of legs depends on course selected.
            //build dictionary for holding race simulation results
            foreach (Boat boat in boatsForRace)
            {
                boatRaceCourse.RaceSimResults.Add(boat.Name, 0);
            }

            //********************race simulator************************************//
            //needs to call the same process only no results are printed out except the number of wins each boat has at the end as a percentage
            boatRaceCourse.RaceSim(boatsForRace, boatRaceCourse, courseSelected, 0, 1000);

            //ask user who they think will win the race--use the menu_cli object
            string simRacePrompt="\nBased on the results of the simulation, which boat do you think will win the race?";            
            int boatToWin = makeSelection.SelectionMenu(boatRaceCourse.RaceSimResults.Keys.ToList(),simRacePrompt);

            //handle response from user
            Console.WriteLine("Ok, " + boatsForRace[boatToWin - 1].Name + " it is.  Let's see if you're right." );
            
            //initiate race user picked a winner for
            boatRaceCourse.RaceSim(boatsForRace, boatRaceCourse, courseSelected, 1, 1);

            //check to see if user's boat won
            if (boatRaceCourse.RaceWinner == boatsForRace[boatToWin - 1].Name)
            {
                Console.WriteLine("\nCongratulations! You picked the winner!");
            }
            else
            {
                Console.WriteLine("\nSorry, the boat you chose, " + boatsForRace[boatToWin - 1].Name + ", lost.");
            }

            do
            {
                Console.Write("Play Again? (y/n)");
                userWantsToPlay = Console.ReadLine().ToLower();
                
            } 
            while (userWantsToPlay != "y" && userWantsToPlay != "n");
            return userWantsToPlay;

            //if user plays a couple games and wins then initiate gambler object and give user a chance to wager 
        } 
        

        private static void DisplayConditionsOnTheWater(RaceCourse boatRaceCourse)
        {
            Console.WriteLine("\nCurrent Conditions on the water");
            Console.WriteLine("Current: " + boatRaceCourse.WaterCurrent + "\nChop: " + boatRaceCourse.WaterChop
                + "\nWind: " + boatRaceCourse.Wind + "\nWind Direction: " + boatRaceCourse.WindDirection +
                "\nWater Chop: " + boatRaceCourse.WaterChopTextCondition + "\nWater Current Direction: " +
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

        public static void AssignBoatProperties(List<Boat> boatsForRace)
        {
            foreach (Boat boat in boatsForRace)
            {
                //assign horsepower, base movement rate, and  to boats
                boat.EngineHorsepower = boat.Horsepower(boat.GetType().Name);

                //assign captains 
                Random num = new Random();
                boat.Captain = num.Next(7) + 3;
                boat.CaptainBonus = boat.EngineHorsepower <= 30 ? boat.Captain * .05 : boat.Captain * .01;

                //assign base movement rate
                //boat.AverageSpeedInKnots += boat.SetAverageSpeedForStartOfRace();
            }
        }        
        public static List<Boat> CreateTheBoats(string boatSelection, int numberOfBoatsToCreate)
        {
            //Build the Boat--right now, builds four boats to race but could be changed to 
            //collect a user input to vary the number of boats to race
            List<Boat> list = new List<Boat>();
            for (int i = 0; i < numberOfBoatsToCreate; i++)
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

        
    }
     
        
        


    }


           
            
