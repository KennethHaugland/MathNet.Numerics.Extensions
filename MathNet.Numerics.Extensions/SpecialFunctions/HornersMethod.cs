using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Mathematics
{
    public static partial class SpecialFunctions
    {
        /// <summary>
        /// Horners evaluation of polynomial and all its derivatives
        /// </summary>
        /// <param name="a_n">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluated at f(x)</param>
        /// <returns>The value of the polynomial at x</returns>
        public static double[] HornersMethod(double[] a_n, double x)
        {

            int n = a_n.Length;

            // Stores the current calculated quotients/reminders            
            double[] q_n = new double[n];

            // Initialize first item in each of the different
            // quotient remembering that the highest power item
            // in all quotients are equal in the syntetic
            // substitution method before its zero
            // f'(x) = ((x-p)*q_1 + r_1)'
            // f''(x) = (x-p)*q_2 + r_2 = ((x-p)^2 * q_1 + r_2)''
            // etc
            for (int i = 0; i < n; i++)
                q_n[i] = a_n[0];

            for (int i = 1; i < n; i++)
            {
                // Standard summation according to Horner's method
                q_n[0] = q_n[0] * x + a_n[i];

                // Derivatives loose one coefficient
                // at each iteration since
                // (X^n)' = n! * X^(n-1)
                for (int j = 1; j < n - i; j++)
                    // The substitution of
                    // derivatives by quotients
                    q_n[j] = q_n[j] * x + q_n[j - 1];
            }

            // When completed all that is left is
            // the reminder and not the quotient
            // so r = q but for simplicity we just use q

            // Adjusting factorials.
            // Starts at 2 since 0! = 1! = 1
            double factorial = 1;
            for (int i = 2; i < n; i++)
            {
                factorial *= i;
                q_n[i] *= (double)factorial;
            }

            // Returns r*n!
            return q_n;
        }

        /// <summary>
        /// Horners evaluation of polynomial and all its derivatives
        /// </summary>
        /// <param name="a_n">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluated at f(x)</param>
        /// <returns>The value of the polynomial at x</returns>
        public static Complex[] HornersMethod(Complex[] a_n, double x)
        {

            int n = a_n.Length;

            // Stores the current calculated quotients/reminders            
            Complex[] q_n = new Complex[n];

            // Initialize first item in each of the different
            // quotient remembering that the highest power item
            // in all quotients are equal in the syntetic
            // substitution method before its zero
            // f'(x) = ((x-p)*q_1 + r_1)'
            // f''(x) = (x-p)*q_2 + r_2 = ((x-p)^2 * q_1 + r_2)''
            // etc
            for (int i = 0; i < n; i++)
                q_n[i] = a_n[0];

            for (int i = 1; i < n; i++)
            {
                // Standard summation according to Horner's method
                q_n[0] = q_n[0] * x + a_n[i];

                // Derivatives loose one coefficient
                // at each iteration since
                // (X^n)' = n! * X^(n-1)
                for (int j = 1; j < n - i; j++)
                    // The substitution of
                    // derivatives by quotients
                    q_n[j] = q_n[j] * x + q_n[j - 1];
            }

            // When completed all that is left is
            // the reminder and not the quotient
            // so r = q but for simplicity we just use q

            // Adjusting factorials.
            // Starts at 2 since 0! = 1! = 1
            double factorial = 1;
            for (int i = 2; i < n; i++)
            {
                factorial *= i;
                q_n[i] *= (double)factorial;
            }

            // Returns r*n!
            return q_n;
        }

        /// <summary>
        /// Horners evaluation of polynomial and all its derivatives
        /// </summary>
        /// <param name="a_n">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluated at f(x)</param>
        /// <returns>The value of the polynomial at x</returns>
        public static Complex[] HornersMethod(Complex[] a_n, Complex x)
        {

            int n = a_n.Length;

            // Stores the current calculated quotients/reminders            
            Complex[] q_n = new Complex[n];

            // Initialize first item in each of the different
            // quotient remembering that the highest power item
            // in all quotients are equal in the syntetic
            // substitution method before its zero
            // f'(x) = ((x-p)*q_1 + r_1)'
            // f''(x) = (x-p)*q_2 + r_2 = ((x-p)^2 * q_1 + r_2)''
            // etc
            for (int i = 0; i < n; i++)
                q_n[i] = a_n[0];

            for (int i = 1; i < n; i++)
            {
                // Standard summation according to Horner's method
                q_n[0] = q_n[0] * x + a_n[i];

                // Derivatives loose one coefficient
                // at each iteration since
                // (X^n)' = n! * X^(n-1)
                for (int j = 1; j < n - i; j++)
                    // The substitution of
                    // derivatives by quotients
                    q_n[j] = q_n[j] * x + q_n[j - 1];
            }

            // When completed all that is left is
            // the reminder and not the quotient
            // so r = q but for simplicity we just use q

            // Adjusting factorials.
            // Starts at 2 since 0! = 1! = 1
            double factorial = 1;
            for (int i = 2; i < n; i++)
            {
                factorial *= i;
                q_n[i] *= (double)factorial;
            }

            // Returns r*n!
            return q_n;
        }


        /// <summary>
        /// Horners scheme for evaluation and its first derivative
        /// </summary>
        /// <param name="a">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluation of polynomial at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static double[] HornersScheme(double[] a, double x)
        {
            double p = a[0];
            double dp = 0;

            // Start at 1 since we want the derivative as well
            for (int i = 1; i < a.Length; i++)
            {
                dp = dp * x + p;
                p = p * x + a[i];
            }

            return new double[] { p, dp };
        }

        /// <summary>
        /// Horners scheme for evaluation and its first derivative
        /// </summary>
        /// <param name="a">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluation of polynomial at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static Complex[] HornersScheme(Complex[] a, Complex x)
        {
            Complex p = a[0];
            Complex dp = 0;

            // Start at 1 since we want the derivative as well
            for (int i = 1; i < a.Length; i++)
            {
                dp = dp * x + p;
                p = p * x + a[i];
            }

            return new Complex[] { p, dp };
        }


        /// <summary>
        /// Horners scheme for evaluation and its first derivative
        /// </summary>
        /// <param name="a">Polynomial values a_n*x^n + ... + x*a_1 + a_0</param>
        /// <param name="x">Evaluation of polynomial at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static Complex[] HornersScheme(Complex[] a, double x)
        {
            Complex p = a[0];
            Complex dp = 0;

            // Start at 1 since we want the derivative as well
            for (int i = 1; i < a.Length; i++)
            {
                dp = dp * x + p;
                p = p * x + a[i];
            }

            return new Complex[] { p, dp };
        }

        /// <summary>
        /// Horner's method that takes in'n all non-zero arguments in a dictionary
        /// </summary>
        /// <param name="data">Stored power as <n,value></n></param>
        /// <param name="x">Evalueted at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static Complex[] HornersMethod(Dictionary<int, Complex> data, Complex x)
        {
            // Find highest polynomial power
            var max = data.Keys.Max();

            // Stores the polynomial
            Complex[] poly = new Complex[max + 1];

            // Used for reversing the power series
            int j = max;
            for (int i = 0; i <= max; i++)
                poly[j--] = (data.ContainsKey(i)) ? data[i] : 0;

            return HornersMethod(poly, x);
        }

        /// <summary>
        /// Horner's method that takes in'n all non-zero arguments in a dictionary
        /// </summary>
        /// <param name="data">Stored power as <n,value></n></param>
        /// <param name="x">Evalueted at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static Complex[] HornersMethod(Dictionary<int, Complex> data, double x)
        {
            // Find highest polynomial power
            var max = data.Keys.Max();

            // Stores the polynomial
            Complex[] poly = new Complex[max + 1];

            // Used for reversing the power series
            int j = max;
            for (int i = 0; i <= max; i++)
                poly[j--] = (data.ContainsKey(i)) ? data[i] : 0;

            return HornersMethod(poly, x);
        }

        /// <summary>
        /// Horner's method that takes in'n all non-zero arguments in a dictionary
        /// </summary>
        /// <param name="data">Stored power as <n,value></n></param>
        /// <param name="x">Evalueted at x</param>
        /// <returns>Polynomial evaluation and derivative at x</returns>
        public static double[] HornersMethod(Dictionary<int, double> data, double x)
        {
            // Find highest polynomial power
            var max = data.Keys.Max();

            // Stores the polynomial
            double[] poly = new double[max + 1];

            // Used for reversing the power series
            int j = max;
            for (int i = 0; i <= max; i++)
                poly[j--] = (data.ContainsKey(i)) ? data[i] : 0;

            return HornersMethod(poly, x);
        }
    }
}
