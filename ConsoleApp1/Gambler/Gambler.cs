using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BoatRace
{
    public class Gambler
    {
        public int Picks { get; set; }
        public decimal Winnings { get; set; }
        public static int StarterWins {get;set;}
        public Gambler()
        {

        }

        public static string WantToGamble(Menu_CLI menu)
        {
            do
            {
                return menu.OfferChanceToWager();
            } 
            while (menu.OfferChanceToWager() != "y" && menu.OfferChanceToWager() != "n");
        }
    }
}
