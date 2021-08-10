using System;
using System.Collections;

namespace ASCII_ATOR
{
    public class BraillePatternsManager
    {
        public static string GetBrailleChar(BitArray bits)
        {
            int dec = GetIntFromBitArray(bits);
            int hex = Convert.ToInt32("2800", 16) + dec;

            return char.ConvertFromUtf32(hex);
        }

        private static int GetIntFromBitArray(BitArray bitArray)
        {
            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }
    }
}
