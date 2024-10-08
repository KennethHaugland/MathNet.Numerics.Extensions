using MathNet.Numerics;
using System.Diagnostics.Eventing.Reader;

namespace MathTests
{
    internal static class AssertHelpers
    {
        public static void AlmostEqualRelative(double expected, double actual, int decimalPlaces)
        {
            if (double.IsNaN(expected) && double.IsNaN(actual))
            {
                return;
            }

            if (!expected.AlmostEqualRelative(actual, decimalPlaces))
            {
                Assert.Fail("Not equal within {0} places. Expected:{1}; Actual:{2}", decimalPlaces, expected, actual);
            }
        }

        public static void AlmostEqualRelative(double[] expected, double[] actual, int decimalPlaces)
        {
            if (Enumerable.SequenceEqual(expected, actual))
                return;
            else
            {
                if (expected.Length == actual.Length)
                {

                    double error = expected.Sum() - actual.Sum();


                    Assert.Fail("Different sums. Expected:{1}; Actual:{2}", expected.Sum(), actual.Sum());
                }
                else
                    Assert.Fail("Different lenght of arrays. Expected:{1}; Actual:{2}", expected.Length, actual.Length);

            }
        }
    }
}
