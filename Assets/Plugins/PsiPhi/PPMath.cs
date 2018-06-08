using System;

namespace PsiPhi
{
    public static class PPMath
    {
        public static double SafeSum( this double value1, double value2, int decimals = 2 )
        {
            return Math.Round( value1 + value2, decimals );
        }

        public static float Sum( this float value1, float value2, int decimals = 2 )
        {
            return (float)Math.Round( value1 + value2, decimals );
        }

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

        public static float Round( float value, int decimals = 2 )
        {
            return (float)Math.Round( value, decimals );
        }

        public static T Clamp<T>( T value, T min, T max ) where T : System.IComparable<T>
        {
            T result = value;
            if( value.CompareTo( max ) > 0 )
                result = max;
            if( value.CompareTo( min ) < 0 )
                result = min;
            return result;
        }
    }
}
