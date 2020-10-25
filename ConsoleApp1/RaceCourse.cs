using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoatRace
{
    public class RaceCourse
    {
        /*
         * RaceCourse currently is a parent class for the different race courses
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
        public double StraightDistanceOfLeg = 100;
        public double CurvedDistanceOfLeg = 25;
        private double ChopEffect //why can't this be in parent class RaceCourse?
        {
            get
            {
                return WaterChopIndex[WaterChop] * -1;
            }
        }
        /*******strings*******/
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
            //that way they stat constant through the race
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

        public double CurvedLegSpeedEffect()
        {
            //A curved leg can take longer to navigate base on how wide a turn is made
            //Boat speed is affected by hp, captain, and speed heading into the turn
            return 0;

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
    }
}
