using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace.RaceCourses
{
    interface IRaceCourse
    {
        public void RaceSim(Boat boatsForRace, RaceCourse boatRaceCourse, string courseSelected, int simOrActual)
        {
            int boatDirection = 0;
            int x = 0;//how else do I keep conditions for leg from printing only once?
            List<int> courseLegTypes = new List<int>();
            courseLegTypes.AddRange(boatRaceCourse.TypesOfLegsInCourse(courseSelected));

            //this list holds the cumulative leg time of the boats
            List<double> cumulativeLegTimesOfEachBoat = new List<double>();

            for (int i = 0; i < courseLegTypes.Count; i++)
            {

                cumulativeLegTimesOfEachBoat.Clear();//needs to be cleared for each leg

                foreach (Boat boat in boatsForRace)
                {
                    boatDirection = x == 0 ? boatRaceCourse.StartDirection : boatRaceCourse.NewLegDirection(boatDirection);

                    double currentBoatSpeed = Math.Round(boat.AverageSpeedInKnots * (boat.IncreaseBoatSpeed(boat.EngineHorsepower) +
                            boat.CaptainBonus + boatRaceCourse.CourseLeg(courseLegTypes[i], boatDirection, i + 1)), 2);

                    currentBoatSpeed += boat.SpeedAdjustment(currentBoatSpeed, boat.EngineHorsepower);
                    //time formula for distance traveled in a straight is 100 / currentBoatSpeed
                    //time formula for distance traveled in a curve is 25 / (currentBoatSpeed with adjustment for turn)

                    if (simOrActual == 1)//loop prints out race conditions for first leg; x condition prevents it from printing more than once
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

                    if (simOrActual == 1)
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
                Console.WriteLine("\nPositions after Leg " + (i + 1) + ": 1) " + boatPositions[0] + ", 2) " + boatPositions[1] + ", 3) " +
                boatPositions[2] + ", 4) " + boatPositions[3]);
            }//end of loop through all legs
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
