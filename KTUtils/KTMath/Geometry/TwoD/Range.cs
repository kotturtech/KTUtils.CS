using System;

namespace KTMath.Geometry.TwoD
{
    public class Range : IComparable<Range>, IEquatable<Range>
    {
        private static int CompareToleranceUnits = 10;

        /// <summary>
        /// Constructs new instance of Range
        /// </summary>
        /// <param name="a">One end of the range</param>
        /// <param name="b">Other end of the range</param>
        public Range(double a, double b)
        {
            From = Math.Min(a, b);
            To = Math.Max(a, b);
        }

        /// <summary>
        /// Start point of the range
        /// </summary>
        public double From { get; set; }

        /// <summary>
        /// End point of the range
        /// </summary>
        public double To { get; set; }

        /// <summary>
        /// Determines whether this range overlaps with other range
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Range other)
        {
            if (other == null)
                return false;
            return other.From >= From && other.From <= To || other.To >= From && other.To <= To;
        }

        #region Comparision methods

        public int CompareTo(Range other)
        {
            if (other == null)
                return 1;

            if (FpCompare.HasMinimalDifference(From, other.From, CompareToleranceUnits))
                return From.CompareTo(other.From);
            if (FpCompare.HasMinimalDifference(To, other.To, CompareToleranceUnits))
                return To.CompareTo(other.To);

            return 0;

        }

        public bool Equals(Range other)
        {
            if (other == null)
                return false;
            return !(FpCompare.HasMinimalDifference(From, other.From, CompareToleranceUnits) ||
                     FpCompare.HasMinimalDifference(To, other.To, CompareToleranceUnits));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Range);
        }

        public override int GetHashCode()
        {
            return (int)(From + To);
        }

        #endregion

    }
}
