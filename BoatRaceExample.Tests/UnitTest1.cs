using System;
using System.ComponentModel.Design;
using BoatRace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoatRaceExample.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FirstLegTriangleTestForZeroWind()
        {
                      
            TriangleCourse rc = new TriangleCourse();
            rc.Wind = 0; 
            rc.WindDirection = 1;
            rc.StartDirection = 2;
            
            //Console.WriteLine(rc.StartDirection);
            Assert.AreEqual("Boat is not affected by wind", rc.FirstLeg());                     

        }

        [TestMethod]
        public void FirstLegTriangleTestWindEqualsStart()
        {
            TriangleCourse rcourse = new TriangleCourse();
            rcourse.Wind = 3;
            rcourse.WindDirection = 2;
            rcourse.StartDirection = 2;
            //Console.WriteLine(rc.StartDirection);
            Assert.AreEqual(rcourse.WindDirection + " " + rcourse.StartDirection + " Wind helps movement in first leg of race", rcourse.FirstLeg());
        }
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
        public void CurrentConditionTest()
        {
            TriangleCourse rc = new TriangleCourse();
            rc.WaterCurrent = 3;
            rc.WaterCurrentDirection = 1;
            Assert.AreEqual(-0.1, rc.WaterCurrentCondition(2));               

        }
        [TestMethod]
        public void BoatSpeedTest()
        {
            SpeedBoat bSpeed = new SpeedBoat(1,"gas",30,true);
            double expected = bSpeed.AverageSpeedInKnots + (bSpeed.AverageSpeedInKnots * bSpeed.IncreaseBoatSpeed(25));
            double actual = bSpeed.BoatSpeed(bSpeed.AverageSpeedInKnots, 25);
            Assert.AreEqual(expected, actual);
                

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

    }
}
