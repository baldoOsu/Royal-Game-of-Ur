using System;
using System.Collections.Generic;
using System.Linq;

namespace Royal_Game_of_Ur
{
    public class Dice
    {
        private static Random random = new Random();
        public int LastSum
        { get; private set; }
        public List<int> LastDice
        { get; private set; }

        public int Roll()
        {
            // Roll four tetrahedral dice, each with two marked corners
            List<int> rolls = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                rolls.Add(random.Next(0, 2)); // Each die has a 50% chance to add 1 to the roll
            }

            this.LastSum = rolls.Sum();
            this.LastDice = rolls;
            return LastSum;
        }
    }
}
