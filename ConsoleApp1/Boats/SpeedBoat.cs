using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    public class SpeedBoat:Boat
    {
        public override double AverageSpeedInKnots { get; set; } = 25;
        public List<SpeedBoat> racingBoats { get; set; } = new List<SpeedBoat>();
        public SpeedBoat(int hulls, string engine, int length, bool isTrailerable) 
        {
            this.Hulls = hulls;
            this.Engine = engine;
            this.Length = length;
            this.IsTrailerable = isTrailerable;
        }



    }
}
