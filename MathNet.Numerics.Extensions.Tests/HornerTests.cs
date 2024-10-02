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

        [TestCase(new double[] { 1, 1, 1, 1, 1, 1, 1 }, 1, 7, 12)]
        public void HornerExactSimple(double[] n, double x, double expected, int decimalPlaces)
        {
            double[] r = Mathematics.SpecialFunctions.HornersMethod(n, x);

            Complex[] a = new Complex[r.Length];

            for (int i = 0; i < a.Length; i++)
                a[i] = new Complex(n[i], 0);

            Complex[] r2 = Mathematics.SpecialFunctions.HornersAlgorithm(a, x);
            AssertHelpers.AlmostEqualRelative(r[0], r2[0].Real, decimalPlaces);
            AssertHelpers.AlmostEqualRelative(r[1], r2[1].Real, decimalPlaces);
        }
    }


}


