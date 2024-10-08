using MathTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathNet
{

    [TestFixture, Category("Functions")]
    public class HornerTests
    {
        [TestCase(new double[] { 1, 1, 1, 1, 1, 1, 1 }, 1, 7, 12)]
        public void HornerExact(double[] n, double x, double expected, int decimalPlaces)
        {
            double[] r = Mathematics.SpecialFunctions.HornersMethod(n, x);
            AssertHelpers.AlmostEqualRelative(expected, r[0], decimalPlaces);
        }

        [TestCase(new double[] { 1, 0, -7, 1, 10, 1 }, 2, new double[] { 1,10,78,198,240,120}, 12)]
        public void HornerExactSimple(double[] n, double x, double[] expected, int decimalPlaces)
        {
            double[] r = Mathematics.SpecialFunctions.HornersMethod(n, x);
            AssertHelpers.AlmostEqualRelative(expected, r, decimalPlaces);            
        }
    }


}


