using System;

namespace PsiPhi
{
    public static class DoubleExtension
    {
        public static bool AlmostEqualTo( this double value1, double value2, double precision = 0.000001 )
        {
            return Math.Abs( value1 - value2 ) < precision;
        }

        public static bool IsGreaterEqualThan( this double value1, double value2, double precision = 0.000001 )
        {
            return value1 > value2 || value1.AlmostEqualTo( value2, precision );
        }

        public static bool IsLowerEqualThan( this double value1, double value2, double precision = 0.000001 )
        {
            return value1 < value2 || value1.AlmostEqualTo( value2, precision );
        }
    }
}
