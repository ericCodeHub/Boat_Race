using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Transactions;

namespace BoatRace
{
    public class TriangleCourse : RaceCourse
    {
        private double ChopEffect //why can't this be in parent class RaceCourse?
        {
            get
            {
                return WaterChopIndex[WaterChop] * -1;
            }
        }
        private int BoatDirection { get; set; }//why isn't this in parent class, RaceCourse?
        
        private int CourseStartingDirection//is this being used? No.
        {
            get
            {
                Random num = new Random();
                return num.Next(3) + 1;
            }
        }
        
        public TriangleCourse()
        {
            RaceCourseConditions();            

        }
        public double FirstLeg()//why are boat legs in sub class instead of parent class?   
        {
            //Each boat average speed should be adjusted based on conditions
            //conditions are in Race Course Parent Class
            //water chop is negative speed
            //last leg of triangle is opposite conditions of first leg

            double currentEffect = WaterCurrentIndex[WaterCurrent];
            
         
            //returns a percentage of how boatspeed is affected

            return WindCondition(StartDirection) + WaterCurrentCondition(StartDirection) + ChopEffect;

        }
        public double SecondLeg(int boatDirection)
        {
            //BoatDirection = boatDirection < 4 ? boatDirection + 1 : 1;
            //Console.WriteLine(boatDirection);
            return WindCondition(boatDirection) + WaterCurrentCondition(boatDirection) + ChopEffect;
        }

        public double ThirdLeg()
        {
            Console.WriteLine(BoatDirection);
            //BoatDirection = BoatDirection < 4 ? BoatDirection + 1 : 1;
            Console.WriteLine(BoatDirection);
            return WindCondition(BoatDirection) + WaterCurrentCondition(BoatDirection) + ChopEffect;
        }
        


    }
}
