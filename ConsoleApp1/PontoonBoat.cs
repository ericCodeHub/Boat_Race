using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    class PontoonBoat:Boat
    {
        public override double AverageSpeedInKnots { get; set; } = 15;
        public List<PontoonBoat> racingBoats { get; set; } = new List<PontoonBoat>();
        public PontoonBoat(int hulls,string engine, int length, bool isTrailerable)
            {
                this.Hulls = hulls;
                this.Engine = engine;
                this.Length = length;
                this.IsTrailerable = isTrailerable;
            }


    }
}
