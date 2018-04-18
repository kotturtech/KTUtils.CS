using System;
namespace KTMath
{
    public static class FpCompare
    {
        /// <summary>
        /// Utility for comparing double values, according to suggestion in:
        /// https://msdn.microsoft.com/en-us/library/ya2zha7s(v=vs.110).aspx
        /// </summary>
        /// <param name="value1">First value to compare</param>
        /// <param name="value2">Second value to compare</param>
        /// <param name="units">Number of possible floating point values between two values</param>
        /// <returns></returns>
        public static bool HasMinimalDifference(double value1, double value2, int units)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(value1);
            long lValue2 = BitConverter.DoubleToInt64Bits(value2);

            // If the signs are different, return false except for +0 and -0.
            if ((lValue1 >> 63) != (lValue2 >> 63))
            {
                if (value1 == value2)
                    return true;

                return false;
            }

            //The difference between the integer representation of two floating-point values indicates the number of possible floating-point values that separates them.
            //For example,the difference between 0.0 and Epsilon is 1, because Epsilon is the smallest representable value when working with a Double whose value is zero.
            long diff = Math.Abs(lValue1 - lValue2);

            if (diff <= units)
                return true;

            return false;
        }

        /// <summary>
        /// Smart compare of double values
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Compare(double a, double b,int differenceUnits = 10)
        {
            //Arbitrary use tolerance up to 10 differences between double values
            if (HasMinimalDifference(a, b, differenceUnits))
                return true;
            return false;
        }
    }   
}
