﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BoatRace 
{
    public abstract class Boat
    {
        /*private boat[] boatList;
        int position = -1;*/
        /******bools******/
        public bool IsTrailerable { get; set; }        
        
        /*****integers*****/
        public int Hulls { get; set; }        
        public int Length { get; set; }
        public int Captain { get; set; }
        /*captain's increase boat speed
         *if horsepower under= 30, speed increase .05 per point (50% max)
         *if horsepower over 30, speed increase .01 per point (10% max)
         */
        public int BoatPosition { get; set; }
        public int OddsToWin { get; set; }

        /******doubles******/
        public virtual double AverageSpeedInKnots { get; set; }
        public double CaptainBonus { get; set; }
        public double EngineHorsepower { get; set; }
        public double BoatTimeForDistance { get; set; }
        public double BoatTotalTime { get; set; }
        

        /******strings******/
        public string Engine { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }

        /*******Lists*******/
        private readonly List<double> outboardHorsepower = new List<double>()
        {
            10,
            15,
            18,
            22,
            25,
            30,
            33,
            36,
            39,
            42,
            45,
            50
        };
        private readonly List<double> horsePowerImpact = new List<double>() {};

        public readonly List<string> boatNames = new List<string>()
        {
            "Blind Baby","Monster Bugs", "Wave Breaker", "Awesome Tides", "Bring It", "Eat My Wake","Crunch Monkey"
        };

        /******Methods******/
        public string CanBeTowed()
        {
            if (IsTrailerable)
            {
                return "can be towed on a trailer";
            }
            return "cannot be towed on a trailer";
        }
        public double SetAverageSpeedForStartOfRace()
        {
            /*
             * takes default average speed for boat type
             * adjusts it by 0, 1, or 2 in either direction (pos or neg)
             * 
             */
            Random num = new Random();
            int adjustment;
            int posOrNeg = num.Next(10) > 4 ? 1 : -1;
            adjustment = num.Next(3) * posOrNeg;
            return adjustment;
        }
        public double Horsepower(string boatType)
        {
            Random numForHPListIndex = new Random();
            Debug.WriteLine(boatType);
            if (boatType == "SailBoat" || boatType == "Trawler")
            {
                return outboardHorsepower[numForHPListIndex.Next(outboardHorsepower.Count / 2)];//returns bottom half of horsepower choices (less horsepower)
            }            
            return outboardHorsepower[numForHPListIndex.Next((outboardHorsepower.Count / 2))+6];//returns top half of horsepower choices (more horsepower)
        }
        public double IncreaseBoatSpeed(double hpOfEngine)
        {
            Random num = new Random();
            double increase = 0;
            int posOrNeg = 0;
            

            if (hpOfEngine <= 10)
            {
                increase = 0;
            }
            else if (hpOfEngine > 10 && hpOfEngine <= 20)
            {
                posOrNeg = num.Next(10);//determines if "increase" will be positive or negative(decrease)
                increase = (num.NextDouble() * .152);
                increase*= posOrNeg < 8 ? 1 :-1;                
                //.15 is max deviation
            }
            else if (hpOfEngine > 20 && hpOfEngine <= 30)
            {
                posOrNeg = num.Next(10);//determines if "increase" will be positive or negative(decrease)
                increase = (num.NextDouble() * .253);
                increase*=posOrNeg < 8 ? 1 : -1;
                //.25 is max deviation
            }
            else if (hpOfEngine > 30 && hpOfEngine <= 40)
            {
                posOrNeg = num.Next(10);//determines if "increase" will be positive or negative(decrease)
                increase = (num.NextDouble() * .353);
                increase*= posOrNeg < 7 ? 1 : -1;
                //.35 is max deviation
            }
            else if (hpOfEngine > 40 && hpOfEngine <= 50)
            {
                posOrNeg = num.Next(10);//determines if "increase" will be positive or negative(decrease)
                increase = (num.NextDouble() * .505);
                increase*= posOrNeg < 6 ? 1 : -1;
                //.50 is max deviation
            }
            
            return Math.Round(increase,2);
        }

        //private int IncreaseSpeed();
        public double BoatSpeed(double averageSpeed, double hpOfEngine)
        {
            return IncreaseBoatSpeed(hpOfEngine);
            //return averageSpeed + (averageSpeed * IncreaseBoatSpeed(hpOfEngine));
        }
        public double SpeedAdjustment(double speed, double hp)//is this used? Yes
        {
            Random num = new Random();
            int x = num.Next(10);
            int posOrNeg = x <= 7 ? 1 : -1;
            //by modifying the percentage in the next line, the outcome of race can vary.
            return speed * ((num.NextDouble() * (.33 * posOrNeg)));
        }
        public int OddsMakingForBoat(double percentageOfVictories)
        {
            /*
             * if percent >= 50 then odds are 1:1; return 1
             * if percent >= 33 then odds are 2:1; return 2
             * if precent >=25 then odds are 3; etc.
             * if percent >=20 then odds are 4 (to 1); return 4
             * if percent >=16 then odds are 5 (to 1); return 5
             */
            int OddsToWin = 20;
            if (percentageOfVictories >= 50)
            {
                OddsToWin = 1;
            }
            else if (percentageOfVictories >= 33)
            {
                OddsToWin = 2;
            }
            else if (percentageOfVictories >= 25)
            {
                OddsToWin = 3;
            }
            else if (percentageOfVictories >= 20)
            {
                OddsToWin = 4;
            }
            else if (percentageOfVictories >= 16)
            {
                OddsToWin = 5;
            }
            else if (percentageOfVictories >= 14)
            {
                OddsToWin = 6;
            }
            else if (percentageOfVictories >= 12)
            {
                OddsToWin = 7;
            }
            else if (percentageOfVictories >= 11)
            {
                OddsToWin = 8;
            }
            else if (percentageOfVictories >= 10)
            {
                OddsToWin = 9;
            }
            return OddsToWin;
        }
    }
    
}
    

