using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Mathematics.Interpolation
{
    public class CatmullRom : IInterpolation
    {

        public PointCollection Points { get; set; } = new PointCollection();

        public bool IsPolygon { get; set; } = false;

        public CatmullRom(PointCollection originalPoints)
        {
            Points = originalPoints;
        }

        public PointCollection CatmullRomSpline( double InterpolationStep = 0.1, bool IsPolygon = false)
        {
            PointCollection result = new PointCollection();

            if (Points.Count <= 2)
            {
                return Points;
            }

            if (IsPolygon)
            {
                for (int i = 0; i <= Points.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        for (double k = 0; k <= (1 - InterpolationStep); k += InterpolationStep)
                        {
                            result.Add(PointOnCatmullRomCurve(Points.Last(), Points[i], Points[i + 1], Points[i + 2], k));
                        }
                    }
                    else if (i == Points.Count - 1)
                    {
                        for (double k = 0; k <= (1 - InterpolationStep); k += InterpolationStep)
                        {
                            result.Add(PointOnCatmullRomCurve(Points.Last(), Points[i], Points[0], Points[1], k));
                        }
                    }
                    else if (i == Points.Count - 2)
                    {
                        for (double k = 0; k <= (1 - InterpolationStep); k += InterpolationStep)
                        {
                            result.Add(PointOnCatmullRomCurve(Points.Last(), Points[i], Points[i + 1], Points[0], k));
                        }
                    }
                    else
                    {
                        for (double k = 0; k <= (1 - InterpolationStep); k += InterpolationStep)
                        {
                            result.Add(PointOnCatmullRomCurve(Points.Last(), Points[i], Points[i + 1], Points[i + 2], k));
                        }
                    }
                }
            }
            else
            {
                List<double> yarray = new List<double>();
                List<double> xarray = new List<double>();
                xarray.Add(Points[0].X - (Points[1].X - Points[0].X) / 2);
                yarray.Add(Points[0].Y - (Points[1].Y - Points[0].Y) / 2);

                foreach (System.Windows.Point ps in Points)
                {
                    xarray.Add(ps.X);
                    yarray.Add(ps.Y);
                }

                xarray.Add((Points[Points.Count - 1].X - (Points[Points.Count - 2].X) / 2 + Points[Points.Count - 1].X));
                yarray.Add((Points[Points.Count - 1].Y - (Points[Points.Count - 2].Y) / 2 + Points[Points.Count - 1].Y));

                PointCollection r = new PointCollection();
                for (int i = 0; i <= yarray.Count - 1; i++)
                {
                    r.Add(new System.Windows.Point(xarray[i], yarray[i]));
                }

                for (int i = 3; i <= r.Count - 1; i++)
                {
                    for (double k = 0; k <= (1 - InterpolationStep); k += InterpolationStep)
                    {
                        result.Add(PointOnCatmullRomCurve(r[i - 3], r[i - 2], r[i - 1], r[i], k));
                    }
                }
                result.Add(Points[Points.Count - 1]);
            }

            return result;
        }

        /// <summary>
        /// Calculates interpolated point between two points using Catmull-Rom Spline </summary>
        /// <remarks>
        /// Points calculated exist on the spline between points two and three. </remarks>
        /// <param name="p0">First Point</param>
        /// <param name="p1">Second Point</param>
        /// <param name="p2">Third Point</param>
        /// <param name="p3">Fourth Point</param>
        /// <param name="t">
        /// Normalised distance between second and third point where the spline point will be calculated </param>
        /// <returns>Calculated Spline Point </returns>
        public Point PointOnCatmullRomCurve(Point p0, Point p1, Point p2, Point p3, double t)
        {
            //Derivative calcualtions
            double lix1, liy1, lix2, liy2;
            lix1 = 0.5 * (p2.X - p0.X);
            lix2 = 0.5 * (p3.X - p1.X);

            liy1 = 0.5 * (p2.Y - p0.Y);
            liy2 = 0.5 * (p3.Y - p1.Y);

            // Location of Controlpoints
            PointCollection PointList = new PointCollection()
            {
                p1,
                new Point(  p1.X + (1d / 3d) * lix1, 
                            p1.Y + (1d / 3d) * liy1),
                new Point(  p2.X - (1d / 3d) * lix2, 
                            p2.Y - (1d / 3d) * liy2),
                p2
            };

            // Get the calcualted value from the 3rd degree Bezier curve
            return PointBezierFunction(PointList, t);
        }

        /// <summary>
        /// The code uses the recursive relation B_[i,n](u) = (1-u)*B_[i,n-1](u) + u*B_[i-1,n-1](u) to compute all nth-degree Bernstein polynomials
        /// </summary>
        /// <param name="n">The sum of the start point, the end point and all the knot points between. Valid range is from 2 and upwards.</param>
        /// <param name="u">Ranges from 0 to 1, and represents the current position of the curve</param>
        /// <returns></returns>
        /// <remarks>This code is translated to VB from the original C++  code given on page 21 in "The NURBS Book" by Les Piegl and Wayne Tiller </remarks>
        private double[] AllBernstein(int n, double u)
        {
            double[] B = new double[n];
            double u1, saved, temp;

            B[0] = 1;
            u1 = 1 - u;


            for (int j = 1; j <= n - 1; j++)
            {
                saved = 0;
                for (int k = 0; k <= j - 1; k++)
                {
                    temp = B[k];
                    B[k] = saved + u1 * temp;
                    saved = u * temp;
                }
                B[j] = saved;
            }

            return B;
        }

        private Point PointBezierFunction(PointCollection p, double StepSize)
        {
            PointCollection result = new PointCollection();
            double[] B;
            double CX, CY;
            double k = StepSize;

            B = AllBernstein(p.Count, k);

            CX = 0;
            CY = 0;
            for (int j = 0; j <= p.Count - 1; j++)
            {
                CX += B[j] * p[j].X;
                CY += B[j] * p[j].Y;
            }
            return new Point(CX, CY);

        }

        public bool SupportsDifferentiation => throw new NotImplementedException();

        public bool SupportsIntegration => throw new NotImplementedException();

        public double Differentiate(double t)
        {
            throw new NotImplementedException();
        }

        public double Differentiate2(double t)
        {
            throw new NotImplementedException();
        }

        public double Integrate(double t)
        {
            throw new NotImplementedException();
        }

        public double Integrate(double a, double b)
        {
            throw new NotImplementedException();
        }

        public double Interpolate(double t)
        {
            throw new NotImplementedException();
        }
    }
}
