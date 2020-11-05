using BoatRace.RaceCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoatRace
{
    public class RaceCourse : IRaceCourse
    {
        /*
         * RaceCourse currently is a parent class for the different race courses
         * **no longer a parent class--10.15.20
         * but it's becoming clear that the different courses don't need a class.
         * It's possible that their elements could be contained in a dictionary.
         * They need a number of legs and whether each leg is a curve or a straight.
         * Example: An oval consists of straight, curve, curve, straight curve, curve;
         * a triangle: straight, curve, straight, curve, straight (final curve is start and finish line).
         * A square or rectangle could be straight, curve, straight, curve, straight, curve, straight.
         * 
         * A private dictionary in this class might be able to hold the legs of each course. A number could be used to represent leg.
         * so dictionary would have a key of oval, with values: 1, 2, 2, 1 2, 2, where 1 = straight and 2 = curve.
         * Values could be a list or an array.  I think it would be a list since lengths would vary.
        */
        public string Test { get; set; } = "hello";

        /****integers*******/
        public int Straight { get; set; }
        public int Curve { get; set; }
        public int StartDirection { get; set; }
        public int PresentBoatDirection { get; set; }

        public int Finish { get; set; }
        public int WaterCurrent { get; set; }
        public int WaterCurrentDirection { get; set; }
        public int WaterChop { get; set; }
        public int Wind { get; set; }
        public int WindDirection { get; set; }

        /******doubles*********/
        public double StraightDistanceOfLeg = 10000;
        public double CurvedDistanceOfLeg = 2500;
        /*
         * turns will incur speed penalties
         * more experienced captains will make a better turn
         * higher boat speeds going into the turn will impact quality of the turn
         * see curveLegSpeedEffect for more details
         */
        private double ChopEffect //why can't this be in parent class RaceCourse?**now it is.
        {
            get
            {
                return WaterChopIndex[WaterChop] * -1;
            }
        }
        /*******strings*******/
        public string RaceWinner { get; set; }
        public string WindReport { get; set; }
        public string WaterCurrentReport { get; set; }
        public string waterChopTextCondition
        {
            get
            {
                switch (WaterChop)
                {
                    case int x when WaterChop < 2:
                        return "none";

                    case 2:
                        return "light";

                    case 3:
                        return "moderate";

                    case 4:
                        return "heavy";
                    default:
                        return "chop condition not set.";

                }
            }
        }

        
        /******Lists******/
        //percentages to increase or decrease speed of respective conditions are in collections below
        public List<double> WaterCurrentIndex = new List<double> { 0, .05, .10, .20, .30 };
        public List<double> WaterChopIndex = new List<double> { 0, 0, .05, .10, .20 };
        public List<double> WindIndex = new List<double> { 0, 0, .05, .10, .20 };

        /*****Dictionaries*****/
        //this dictionary holds integer values in a list that represent the two different possible legs 
        //in the race course (curve = 1 and straight = 0)
        private Dictionary<string, List<int>> BoatLegs = new Dictionary<string, List<int>>
        {
            {"Oval", new List<int>() {0, 1, 1, 0, 1, 1} },
            {"Straight", new List<int>{0, 0, 0, 0} },
            {"Triangle", new List<int>(){0, 1, 0, 1, 0} }

        };
        public Dictionary<string, int> RaceSimResults = new Dictionary<string, int>();

        /*****Class Constructors*****/
        //additional notes for this class are below the constructors
        public RaceCourse() { }
        public RaceCourse(string raceCourseSelection)//why is the raceCourseSelection being passed through here?
        {

            RaceCourseConditions();
        }
        //race courses have four to five legs
        //boats run through each leg
        //boat speed through each leg determines winner of that leg.
        //highest sum of boat speed after all legs wins race

        /******Methods******/

        public int NewLegDirection(int boatDirection)//changes boat direction
        {
            //if direction is four then must reset to 1
            return boatDirection == 4 ? 1 : boatDirection++;
        }
        public double CourseLeg(int legShape, int boatDirection, int legCount)  
        {
            //Each boat average speed should be adjusted based on conditions
            //conditions are in Race Course Parent Class
            //water chop is negative speed
            //last leg of triangle is opposite conditions of first leg

            double currentEffect = WaterCurrentIndex[WaterCurrent];


            //returns a percentage of how boatspeed is affected

            return WindCondition(boatDirection) + WaterCurrentCondition(boatDirection) + ChopEffect;

        }
        //factors that increase boat speed: captain experience, engine horsepower, positive current, strong wind speed
        //factors that decrease boat speed: negative current, strong wind speed, waterchop, course curves/turns
        //additionally engines with high horsepower break down more frequently than lower hp engines.

        public void RaceCourseConditions()//can conditions be set in constructor instead?  I'm thinking no.
        {
            Random num = new Random();//random number generators to set conditions for course
            int waterCurrent = num.Next(WaterCurrentIndex.Count);
            int waterChop = num.Next(WaterChopIndex.Count);
            int wind = num.Next(WindIndex.Count);
            int waterCurrentDirection = num.Next(4) + 1;//direction is currently represented by four whole values(1,2,3,4)
            int windDirection = num.Next(4) + 1;
            int startDirection = num.Next(4) + 1;

            //now set properties to variables created above
            //that way they stay constant through the race
            WaterCurrent = waterCurrent;
            WaterChop = waterChop;
            Wind = wind;
            WaterCurrentDirection = waterCurrent > 0 ? waterCurrentDirection : 0;
            WindDirection = wind > 1 ? windDirection : 0;
            StartDirection = startDirection;
        }

        public List<string> RaceCourseChoices()
        {            
            return BoatLegs.Keys.ToList<string>();
        }
        public List<int> TypesOfLegsInCourse(string courseSelection)
        {
            return BoatLegs[courseSelection];
        }

        public double CurvedLegSpeedEffect(Boat boat)
        {
            /*
             * A curved leg can take longer to navigate base on how wide a turn is made
             * Boat speed is affected by hp, captain, and speed heading into the turn
             * currently going with max impact of 40% based on captain rating and speed going into turn
             * 
             * all boat's speeds should be altered somewhat, say 10%; possible formula:
             * captains < 5, 20% to 40%; >5 10% to 20%
             * speed
            */
            Random num = new Random();
            double curveEffect;

            if (boat.Captain <= 5)
            {
                curveEffect = num.NextDouble() * .404;
                if (curveEffect < .20)
                    curveEffect = .20;
            }
            else
            {
                curveEffect = num.NextDouble() * .202;
                if (curveEffect < .10) { curveEffect = .10; }
            }
            return curveEffect;

        }
        public double WindCondition(int boatDirection)
        {
            //check if boat is into the wind (*-1 *windIndex), alongside the wind(-.5 or .5), or with the wind (WindIndex)
            //string windReport;
            double windEffect = WindIndex[Wind];
            if (Wind < 2)
            {
                WindReport = "Wind has no effect on Boat movement.";
            }
            else if (WindDirection == boatDirection)
            {
                //return (WindDirection + " " + StartDirection + " Wind helps movement in first leg of race");
                WindReport = "Boats are moving with the wind";

            }
            else if ((WindDirection - boatDirection) == 1) //|| StartDirection-WindDirection == 1)
            {
                //return (WindDirection + " " + StartDirection + " Boat is angled into the wind in leg 1");
                WindReport = "Boats are angled into the wind.";
                windEffect *= -.5;
            }
            else if (Math.Abs(WindDirection - boatDirection) == 2) //|| StartDirection-WindDirection==2){
            {
                ////return (WindDirection + " " + StartDirection + " Boat is heading into the wind in first leg");
                WindReport = "Boats are heading into the wind";
                windEffect *= -1;
            }
            else if (boatDirection - WindDirection == 1)
            {
                //return (WindDirection + " " + StartDirection + " Boat is angled away from the wind");
                WindReport = "Boats are angled away from the wind.";
                windEffect *= .5;
            }
            else
            {
                WindReport = "No condition calculated";
                
            }
            //Console.WriteLine(WindReport);
            return windEffect;

        }
        public double WaterCurrentCondition(int boatDirection)
        {
            //check if boat is into the current (*-1 *WaterCurrentIndex), alongside the wind(-.5 or .5), or with the wind (CurrentIndex)
            //string windReport;
            double waterCurrentEffect = WaterCurrentIndex[WaterCurrent];
            
            if (WaterCurrent < 1)
            {
                WaterCurrentReport = boatDirection + "Current has no effect on Boat movement.";
            }
            else if (WaterCurrentDirection == boatDirection)
            {
                //return (WindDirection + " " + StartDirection + " Wind helps movement in first leg of race");
                WaterCurrentReport = boatDirection + "Boats are moving with the current";

            }
            else if ((WaterCurrentDirection - boatDirection) == 1 || WaterCurrent - boatDirection == 3) 
            {
                //return (WindDirection + " " + StartDirection + " Boat is angled into the wind in leg 1");
                WaterCurrentReport = boatDirection + "Boats are angled into the current.";
                waterCurrentEffect *= -.5;
            }
            else if (Math.Abs(WaterCurrentDirection - boatDirection) == 2) //|| StartDirection-WindDirection==2){
            {
                ////return (WindDirection + " " + StartDirection + " Boat is heading into the wind in first leg");
                WaterCurrentReport = boatDirection + "Boats are heading into the current";
                waterCurrentEffect *= -1;
            }
            else if (boatDirection - WaterCurrentDirection == 1)
            {
                //return (WindDirection + " " + StartDirection + " Boat is angled away from the wind");
                WaterCurrentReport = boatDirection + "Boats are angled away from the current.";
                waterCurrentEffect *= .5;
            }
            else
            {
                WaterCurrentReport = "No condition calculated";

            }
            //Console.WriteLine(CurrentReport);
            return waterCurrentEffect;
        }
        public void RaceSim(List<Boat> boatsForRace, RaceCourse boatRaceCourse, string courseSelected, int simOrActual, int numOfRacesToSim)
        {
            for (int a = 0; a < numOfRacesToSim; a++)
            {
                RaceWinner = "";
                int boatDirection = 0;
                int x = 0;//how else do I keep conditions for leg from printing only once?
                List<int> courseLegTypes = new List<int>();
                courseLegTypes.AddRange(boatRaceCourse.TypesOfLegsInCourse(courseSelected));

                //this list holds the cumulative leg time of the boats
                List<double> cumulativeLegTimesOfEachBoat = new List<double>();
                ClearBoatTimesForNextRace(boatsForRace);
                for (int i = 0; i < courseLegTypes.Count; i++)//this for loop compiles each leg of the race
                {
                    if (simOrActual == 1)
                    {
                        Console.WriteLine("\nLeg " + (i + 1) + ":  " + (courseLegTypes[i] == 0 ? "straight" : "curve"));
                    }
                    cumulativeLegTimesOfEachBoat.Clear();//needs to be cleared/reset for each leg

                    foreach (Boat boat in boatsForRace)
                    {
                        boatDirection = x == 0 ? boatRaceCourse.StartDirection : boatRaceCourse.NewLegDirection(boatDirection);
                        double currentBoatSpeed = CurrentBoatSpeed(boatRaceCourse, boatDirection, courseLegTypes, i, boat);
                        //time formula for distance traveled in a straight is 100 / currentBoatSpeed
                        //time formula for distance traveled in a curve is 25 / (currentBoatSpeed with adjustment for turn)

                        if (simOrActual == 1 & x == 0)//prints out wind and water current conditions for each leg
                        {
                            //****condition reports for present leg of race
                            Console.WriteLine("Water Current Report: " + boatRaceCourse.WaterCurrentReport);
                            Console.WriteLine("Wind Report: " + boatRaceCourse.WindReport);
                            //******condition reports

                            //header for leg data
                            Console.WriteLine("\nName\t\tHP\tCapt.\tLg Sp\tLg T\tTot T\tPlace\n");
                        }
                        x++;
                        //leg results

                        double thisLegTime = ThisLegTime(boat, boatRaceCourse, currentBoatSpeed,courseLegTypes[i]);
                        boat.BoatTimeForDistance += thisLegTime;

                        //*******print out results for leg*********************************//
                        if (simOrActual == 1)//if not a sim
                        {
                            Console.WriteLine(boat.Name + "\t" + boat.EngineHorsepower +
                                "\t" + boat.Captain + "\t" + Math.Round(currentBoatSpeed, 2) +
                                "\t" + thisLegTime + "\t" + Math.Round(boat.BoatTimeForDistance / 10, 2) +
                                "\t" + boat.AverageSpeedInKnots);
                        }

                        cumulativeLegTimesOfEachBoat.Add(boat.BoatTimeForDistance);
                    }//end of leg

                    List<string> boatPositions = new List<string>();

                    boatPositions.AddRange(CalculateBoatPositionsAfterEachLeg(cumulativeLegTimesOfEachBoat, boatsForRace, boatsForRace.Count));
                    if (simOrActual == 1)
                    {
                        Console.WriteLine("\nPositions after Leg " + (i + 1) + ": 1) " + boatPositions[0] + ", 2) " + boatPositions[1] + ", 3) " +
                        boatPositions[2] + ", 4) " + boatPositions[3]);
                    }
                    if (i == courseLegTypes.Count - 1)
                    {
                        boatRaceCourse.RaceSimResults[boatPositions[0]] += 1;
                        if (simOrActual == 1)
                        {
                            RaceWinner = boatPositions[0];
                        }
                    }

                }//end of loop through all legs


            }
            if (simOrActual == 0)
            {
                Console.WriteLine("\nHere are the results based on a simulation of " + numOfRacesToSim + " races:");
                foreach (KeyValuePair<string, int> result in boatRaceCourse.RaceSimResults)
                {
                    Console.WriteLine("\t" + result.Key + ": " + "\t" + result.Value + "\t" + Math.Round(((double)result.Value / numOfRacesToSim) * 100, 0) + "%");
                }
            }
        }

        private static void ClearBoatTimesForNextRace(List<Boat> boatsForRace)
        {
            foreach (Boat boat in boatsForRace)
            {
                boat.BoatTimeForDistance = 0;
            }
        }

        double ThisLegTime(Boat boat, RaceCourse boatRaceCourse, double currentBoatSpeed, int courseLegType)
        {
            double legDistance = courseLegType == 0 ? boatRaceCourse.StraightDistanceOfLeg : CurvedDistanceOfLeg;
            currentBoatSpeed = courseLegType == 0 ? currentBoatSpeed : currentBoatSpeed += currentBoatSpeed * CurvedLegSpeedEffect(boat);
            return Math.Round((legDistance / currentBoatSpeed) / 10, 2);
        }

        double CurrentBoatSpeed(RaceCourse boatRaceCourse, int boatDirection, List<int> courseLegTypes, int i, Boat boat)
        {
            double currentBoatSpeed = Math.Round(boat.AverageSpeedInKnots * (boat.IncreaseBoatSpeed(boat.EngineHorsepower) +
                                        boatRaceCourse.CourseLeg(courseLegTypes[i], boatDirection, i + 1)), 2);
            currentBoatSpeed += (currentBoatSpeed * boat.CaptainBonus);//add captain bonus
            currentBoatSpeed += boat.SpeedAdjustment(currentBoatSpeed, boat.EngineHorsepower);
            return currentBoatSpeed;
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
