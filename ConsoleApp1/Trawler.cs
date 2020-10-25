using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    class Trawler:Boat
    {
        public override double AverageSpeedInKnots { get; set; } = 8;
        public List<Trawler> racingBoats { get; set; } = new List<Trawler>();

        public Trawler(int hulls, string engine, int length, bool isTrailerable)
        {
            this.Hulls = hulls;
            this.Engine = engine;
            this.Length = length;
            this.IsTrailerable = isTrailerable;
        }
    }
}
