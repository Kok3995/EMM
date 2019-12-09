using System;

namespace Data
{
    public static class RandomExtension
    {
        public static double RandomCoordinate(this Random random, double coordinate, int randommize)
        {
            return random.Next((int)coordinate - randommize, (int)coordinate + randommize);
        }
    }
}
