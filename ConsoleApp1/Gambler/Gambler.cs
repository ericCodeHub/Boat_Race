using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BoatRace
{
    public class Gambler
    {
        public int Picks { get; set; }
        public int Wager { get; set; }
        public int Winnings { get; set; }
        public int Payout { get; set; }
        public static int StarterWins {get;set;}
        public string GambleOnBoats { get; set; } = "y";
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
        public int PlaceWager()
        {
            //player can wager $0 if they don't want to put any money down on race
            //
            //tell player how much money they have to wager
            //ask player for wager
            //don't let wager exceed player.winnings
            //
            Console.WriteLine($"You have ${Winnings} to wager.\nHow much would you like to wager?");
            do
            {
                if ((Wager = int.Parse(Console.ReadLine())) > Winnings)
                {
                    Console.WriteLine($"Your wager must be less than the funds you have available (${Winnings}).");
                }                
            
            } while(Wager > Winnings);

            return Wager;
            
        }
    }
}
