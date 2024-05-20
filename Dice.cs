using System;

namespace Royal_Game_of_Ur
{
    public class Dice
    {
        private static Random random = new Random();
        public int Last
        { get; private set; }

        public int Roll()
        {
            // Roll four tetrahedral dice, each with two marked corners
            int roll = 0;
            for (int i = 0; i < 4; i++)
            {
                roll += random.Next(0, 2); // Each die has a 50% chance to add 1 to the roll
            }

            this.Last = roll;
            return roll;
        }
    }
}
