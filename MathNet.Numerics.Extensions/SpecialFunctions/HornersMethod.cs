using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;


namespace Mathematics
{
    public static partial class SpecialFunctions
    {
        /// <summary>
        /// https://math.stackexchange.com/questions/2139142/how-does-horner-method-evaluate-the-derivative-of-a-function
        /// </summary>
        /// <param name="a_n"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Complex[] HornersAlgorithm(Complex[] a_n, Complex x)
        {
            Complex p = a_n.Last();
            Complex dp = 0;          

            int n = a_n.Length-1;

            for (int i = n; i >= 0; i--)
            {
                dp = dp * x + p;
                p = p * x + a_n[i];
            }

            return new Complex[2] { p, dp };
        }

        /// <summary>
        /// Horners algorithm that returns polynomial with coeffifcients a_n evalueted at x for f(x) and all possible derivatives
        /// </summary>
        /// <param name="a_n">Polynomial coefficients from a_0 + a_1*x + ... a_n*x^n </param>
        /// <param name="x">Polynomial is evalueded at x </param>
        /// <returns>Polynomial evaluedted at x and derivative 1 at 1 etc.</returns>
        public static double[] HornersMethod(double[] a_n, double x)
        {
            double[] result = new double[] { 0};

            double cnst = 1d;

            int nnd = 0;
            int nc = a_n.Length - 1;
            int nd = a_n.Length - 2;
            double[] pd = new double[nc];
                       
            pd[0] = a_n[nc];

            for (int j = 1; j < nd + 1; j++)
                pd[j] = 0;

            for (int i = nc - 1; i >= 0; i--)
            {
                nnd = (nd < (nc - i) ? nd : nc - i);
                for (int j = nnd; j > 0; j--)
                    pd[j] = pd[j] * x + pd[j - 1];

                pd[0] = pd[0] * x + a_n[i];            
            }

            for (double i = 2d; i < nd + 1d; i++)
            {
                cnst *= i;
                pd[(int)i] *= cnst;
            }

            return pd;
        }
    }
}
