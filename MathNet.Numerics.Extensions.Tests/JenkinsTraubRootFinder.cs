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
    public class JenkinsTraubRootFinderTEst
    {
        [TestCase(new double[] { 1,5,6 }, -3, 12)]
        public void JenkinsTraubRootFinderExact(double[] n, double expected, int decimalPlaces)
        {
            List<Complex> r = Mathematics.RootFinders.FindRoots(n);
            AssertHelpers.AlmostEqualRelative(expected, r[0].Real, decimalPlaces);
        }


    }

}
