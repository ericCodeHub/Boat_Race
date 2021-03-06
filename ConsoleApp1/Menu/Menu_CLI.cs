﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BoatRace
{
    public class Menu_CLI
    {

        public Menu_CLI()
        {

        }
        public int SelectionMenu(List<string> itemsToChooseFrom, string prompt)
        {
            
            int selection = itemsToChooseFrom.Count + 1;
            do
            {
                    
                try
                {
                    Console.WriteLine(prompt);
                    Console.WriteLine(ItemChoiceString(itemsToChooseFrom)); 
                    selection = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                        
                }
                    
            }
            while (selection > itemsToChooseFrom.Count);            
           
            return selection;
        }

        private string ItemChoiceString(List<string> itemsToChooseFrom)//can this be generic method to create list of items to choose from in menu (ListChoices())
        {
            int i = 0;
            string itemChoiceString = "";
            foreach (string item in itemsToChooseFrom)
            {
                itemChoiceString += "\n\t" + (i + 1) + ") " + item;//ListChoices()
                i++;
            }
            return itemChoiceString;

        }
        public string UserNamesBoat()
        {
            Console.Write("Okay, please enter your boat name: ");
            return Console.ReadLine();
            //boatsForRace[0].Name = Console.ReadLine();
        }
        public string OfferChanceToWager()
        {

            Console.Write("Hey, you are pretty good at this.  Want to wager some money? (y/n) ");
            return Console.ReadLine().ToLower();
        }
    }
}

