using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    public interface IHorsePower
    {

        public double IncreaseBoatSpeed(int hpOfEngine)
        {
            double increase = 0;
            if (hpOfEngine <= 10)
            {
                increase = 0;
            }else if(hpOfEngine > 10 && hpOfEngine <= 20)
            {
                increase = .25;
            }else if (hpOfEngine > 20 && hpOfEngine<= 30)
            {
                increase = .50;
            }else if(hpOfEngine > 30 && hpOfEngine <= 4)
            {
                increase = .75;
            }else if(hpOfEngine > 40 && hpOfEngine<= 50)
            {
                increase = 1;
            }
            return increase;
        }

        //private int IncreaseSpeed();
        public double BoatSpeed(int averageSpeed,int hpOfEngine)
        {
            
            
            return averageSpeed + (averageSpeed * IncreaseBoatSpeed(hpOfEngine));
        }
    }
}
