using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BoatRace
{
    
    public class SailBoat:Boat
    {

        public override double AverageSpeedInKnots { get; set; } = 4;
        public string SailPlan { get; set; }
        public List<SailBoat> RacingBoats { get; set; } = new List<SailBoat>();
        public SailBoat(int hulls, string sailPlan, string engine, int length, bool isTrailerable) : base()
        {
            this.Engine = engine;
            this.SailPlan = sailPlan;
            this.Hulls = hulls;
            this.Length = length;
            this.IsTrailerable = isTrailerable;
        }
        //public double BoatSpeed { get; set; }

        public void AddBoats(SailBoat aSailboat)
        {
            
            RacingBoats.Add(aSailboat);
        }
         
        

        
        /*public int Speed()
        {
            return AverageSpeedInKnots;
        }*/



        

    }
}
