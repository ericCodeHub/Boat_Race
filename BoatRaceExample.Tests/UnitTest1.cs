using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using BoatRace;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoatRaceExample.Tests
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void WindConditionTest_NoWind()
        {
            TriangleCourse rc = new TriangleCourse();
            rc.Wind = 0;
            rc.WindDirection = 1;            
            
            Assert.AreEqual(0, rc.WindCondition(2));
        }

        [TestMethod]
        public void WindConditionTest_WithWind()
        {
            TriangleCourse rc = new TriangleCourse();
            rc.Wind = 3;
            rc.WindDirection = 1;            
            
            Assert.AreEqual(-0.1, rc.WindCondition(3));
        }
        [TestMethod]
        public void CurrentConditionTest1()
        {
            RaceCourse rc = new RaceCourse();
            rc.WaterCurrent = 3;
            rc.WaterCurrentDirection = 1;
            Assert.AreEqual(0.1, rc.WaterCurrentCondition(2));               

        }
        [TestMethod]
        public void CurrentConditionTest2()
        {
            RaceCourse rc = new RaceCourse();
            rc.WaterCurrent = 3;
            rc.WaterCurrentDirection = 4;
            
            Assert.AreEqual(-0.1, rc.WaterCurrentCondition(1));
        }
        [TestMethod]
        public void BoatSpeedTest()
        {
            SpeedBoat bSpeed = new SpeedBoat(1,"gas",30,true);
            double expected = bSpeed.BoatSpeed(bSpeed.AverageSpeedInKnots, 25);
            //double actual = bSpeed.BoatSpeed(bSpeed.AverageSpeedInKnots, 45);
            Assert.IsTrue(expected < .25);
            
                

        }
        [TestMethod]
        public void RaceCourseTestForZeroWind()
        {

            RaceCourse rc = new RaceCourse();
            rc.Wind = 0;
            rc.WindDirection = 1;
            rc.StartDirection = 2;
            rc.CourseLeg(1, 3, 1);

            //Console.WriteLine(rc.StartDirection);
            Assert.AreEqual("Wind has no effect on Boat movement.", rc.WindReport);

        }
        [TestMethod]
        public void HorsepowerTest()
        {
            SailBoat sb = new SailBoat(1, "sloop", "diesel", 30, true);
            Assert.IsTrue(sb.Horsepower(sb.GetType().Name) <= 30);
        }
        [TestMethod]
        public void BoatSelectionMenuTest()
        {
            Console.WriteLine("");
        }
        [TestMethod]
        public void RaceSimResultsTest()
        {
            RaceCourse rc = new RaceCourse();
            List<Boat> rBoats = BoatRace.Program.CreateTheBoats("SpeedBoat",4);
            BoatRace.Program.AssignBoatProperties(rBoats);
            for(int i = 0; i < rBoats.Count; i++)
            {
                rBoats[i].Name = "boat" + (i+1);
                rc.RaceSimResults.Add(rBoats[i].Name, 0);
            }
            rc.RaceSim(rBoats, rc, "Straight", 0, 1000);
            
        }
        [TestMethod]
        public void SingleRaceTest()
        {
            RaceCourse rc = new RaceCourse();
            List<Boat> rBoats = BoatRace.Program.CreateTheBoats("SpeedBoat", 4);
            BoatRace.Program.AssignBoatProperties(rBoats);
            for (int i = 0; i < rBoats.Count; i++)
            {
                rBoats[i].Name = "boat" + (i + 1);
                rc.RaceSimResults.Add(rBoats[i].Name, 0);
            }
            rc.RaceSim(rBoats, rc, "Triangle",1, 1);
        }
        [TestMethod]
        public void CurrentBoatSpeedTest()
        {
            RaceCourse rc = new RaceCourse();
            List<Boat> rBoats = BoatRace.Program.CreateTheBoats("SpeedBoat", 4);
            rBoats[0].EngineHorsepower = rBoats[0].Horsepower(rBoats[0].GetType().Name);
            double result = rc.CurrentBoatSpeed(rc, 1, 0, 1, rBoats[0]);
            Console.WriteLine(result);
            Assert.IsTrue(result > 0);
        }
        [TestMethod]
        public void NewLegDirectionTest()
        {
            //boat changes direction whenever it goes into a turn
            RaceCourse rc = new RaceCourse();
            int newDirection = rc.NewLegDirection(4);
            Assert.AreEqual(1, newDirection,"direction should change from 4 to 1");
        }
        [TestMethod]
        public void OddsMakingTest()
        {
            RaceCourse rc = new RaceCourse();
            //OddsMakingForBoat now in Boat Class after testing
            Assert.AreEqual(4, rc.OddsMakingForBoat(22));
        }
        [TestMethod]
        public void CheckWinningsTest()
        {
            RaceCourse rc = new RaceCourse();
            List<Boat> rBoats = BoatRace.Program.CreateTheBoats("SpeedBoat", 4);
            Gambler player = new Gambler();
            player.Winnings = 500;
            player.Wager = 500;
            //OddsToWin moved to Boat Class after testing
            rc.OddsToWin = 2;
            BoatRace.Program.CheckWinnings(rBoats,rc,3,player);
            Assert.IsTrue(player.Winnings == 1000);
        }
    }
}
