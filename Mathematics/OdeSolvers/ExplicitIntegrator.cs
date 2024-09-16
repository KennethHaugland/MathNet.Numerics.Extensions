using System;


namespace Mathematics
{
    public static partial class OdeSolvers
    {
        public class ExplicitIntegrator
        {

            /// <summary>
            /// Position, usually from 0 to 1
            /// </summary>
            public decimal[] c { get; set; } = new decimal[0];

            /// <summary>
            /// Indicates the dependence of the stages on the derivatives found at other stages of derivatives at other stages
            /// </summary>
            public decimal[,] A { get; set; } = new decimal[0, 0];

            /// <summary>
            /// Quaderature weights
            /// </summary>
            public decimal[] b_HighOrder { get; set; } = new decimal[0];

            /// <summary>
            /// 2nd set of Quaderature weights for error estimates
            /// </summary>
            public decimal[] b_LowOrder { get; set; } = new decimal[0];

            /// <summary>
            /// Error estimation
            /// </summary>
            public decimal[] d { get; set; } = new decimal[0];


            /// <summary>
            /// Position, usually from 0 to 1
            /// </summary>
            private double[] c_double { get; set; } = new double[0];

            /// <summary>
            /// Indicates the dependence of the stages on the derivatives found at other stages of derivatives at other stages
            /// </summary>
            private double[,] A_double { get; set; } = new double[0, 0];

            /// <summary>
            /// Quaderature weights
            /// </summary>
            private double[] b_HighOrder_double { get; set; } = new double[0];

            /// <summary>
            /// 2nd set of Quaderature weights for error estimates
            /// </summary>
            private double[] b_LowOrder_double { get; set; } = new double[0];

            /// <summary>
            /// Error estimation
            /// </summary>
            private double[] d_double { get; set; } = new double[0];

            public bool LowImpactMethod { get; set; } = false;

            private IntegrationType pintegrationType;

            public IntegrationType StoredIntegrationTypes
            {
                get { return pintegrationType; }
                set
                {
                    if (value != StoredIntegrationTypes)
                    {
                        pintegrationType = value;

                        if (pintegrationType == IntegrationType.RungeKuttaK41)
                        {
                            c = new decimal[] { 0, 1M / 2M, 1M / 2M, 1M };
                            A = new decimal[4, 4] {
                                                { 0,    0,  0,  0},
                                                {1M/2M, 0,  0,  0 },
                                                {0, 1M/2M,  0,  0 },
                                                {0,     0,  1M, 0 }
                                              };
                            b_HighOrder = new decimal[] { 1M / 6M, 1M / 3M, 1M / 3M, 1M / 6M };
                            b_LowOrder = new decimal[] { 0, 0, 0, 0 };
                            d = new decimal[] { 0, 0, 0, 0 };
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.DormandPrince56)
                        {
                            c = new decimal[] { 0, 2M / 9M, 1M / 3M, 5M / 9M, 2M / 3M, 1, 1 };
                            A = new decimal[,] {
                                            { 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 2M/9M, 0, 0, 0, 0, 0, 0, 0 },
                                            { 1M/12M, 1M/4M, 0, 0, 0, 0, 0, 0 },
                                            { 55M/324M, -25M/108M, 50M/81M, 0, 0, 0, 0, 0 },
                                            { 83M/330M, -13M/22M, 61M/66M, 9M/110M, 0, 0, 0, 0},
                                            { -19M/28M, 9M/4M, 1M/7M, -27M/7M, 22M/7M, 0, 0, 0},
                                            { 19M/200M, 0, 3M/5M, -243M/400M, 33M/40M, 7M/80M, 0,0 }
                                            };

                            b_LowOrder = new decimal[] { 19M / 200M, 0, 3M / 5M, -243M / 400M, 33M / 40M, 7M / 80M, 0 };
                            b_HighOrder = new decimal[] { 431M / 5000M, 0, 333M / 500M, -7857M / 10000M, 957M / 1000M, 193M / 2000M, -1M / 50M };
                            d = new decimal[] { -11M / 1250M, 0, 33M / 500M, -891M / 5000M, 33M / 250M, 9M / 1000M, -1M / 50M };
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.Verner56)
                        {


                        }
                        else if (pintegrationType == IntegrationType.CashKarp45)
                        {
                            c = new decimal[] { 0, 1M / 5M, 3M / 10M, 3M / 5M, 1M, 7M / 8M };
                            A = new decimal[,] {
                                            { 0, 0, 0, 0, 0, 0 },
                                            { 1M/5M, 0, 0, 0,  0,  0 },
                                            { 3M/40M, 9M/40M,  0,  0, 0, 0 },
                                            { 3M/104M, -9M/10M, 6M/5M,   0, 0, 0 },
                                            { -11M/54M, 5M/2M, -70M/27M, 35M/27M,  0, 0},
                                            { 1631M/55296M, 175M/512M, 575M/13824M, 44275M/110592M, 253M/4096M,  0}
                                            };

                            b_LowOrder = new decimal[] { 37M / 378M, 0, 250M / 621M, 125M / 594M, 0, 512M / 1771M };
                            b_HighOrder = new decimal[] { 2825M / 27648M, 0, 18575M / 48384M, 13525M / 55296M, 277M / 14336M, 1M / 4M };
                            d = new decimal[b_HighOrder.Length];

                            for (int i = 0; i < b_HighOrder.Length; i++)
                            {
                                d[i] = b_HighOrder[i] - b_LowOrder[i];
                            }
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.LSERK54)
                        {
                            c = new decimal[] { 0, -567301805773M / 1357537059087M, -2404267990393M / 2016746695238M, -3550918686646M / 2091501179385M, -1275806237668M / 842570457699M };


                            b_HighOrder = new decimal[] { 1432997174477M / 9575080441755M, 5161836677717M / 13612068292357M, 1720146321549M / 2090206949498M, 3134564353537M / 4481467310338M, 2277821191437M / 14882151754819M };
                            b_LowOrder = new decimal[b_HighOrder.Length];

                            d = new decimal[b_HighOrder.Length];
                            A = new decimal[c.Length, c.Length];
                            for (int i = 1; i < c.Length; i++)
                            {
                                A[i, i - 1] = c[i];
                            }
                            for (int i = 0; i < b_HighOrder.Length; i++)
                            {
                                b_LowOrder[i] = 0M;
                                d[i] = 0M;
                            }
                            LowImpactMethod = true;
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.LSERK74)
                        {
                            c = new decimal[] { 0M, -0.647900745934M, -2.704760863204M, -0.460080550118M, -0.500581787785M, -1.906532255913M, -1.450000000000M };
                            b_HighOrder = new decimal[] { 0.117322146869M, 0.503270262127M, 0.233663281658M, 0.283419634625M, 0.540367414023M, 0.371499414620M, 0.136670099385M };

                            b_LowOrder = new decimal[b_HighOrder.Length];

                            d = new decimal[b_HighOrder.Length];
                            A = new decimal[c.Length, c.Length];
                            for (int i = 1; i < c.Length; i++)
                            {
                                A[i, i - 1] = c[i];
                            }
                            for (int i = 0; i < b_HighOrder.Length; i++)
                            {
                                b_LowOrder[i] = 0M;
                                d[i] = 0M;
                            }
                            LowImpactMethod = true;
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.LSERK_13_4)
                        {
                            c = new decimal[] { 0M, -0.6160178650170565M, -0.4449487060774118M, -1.0952033345276178M, -1.2256030785959187M, -0.2740182222332805M, -0.0411952089052647M, -0.1797084899153560M, -1.1771530652064288M, -0.4078831463120878M, -0.8295636426191777M, -4.7895970584252288M, -0.6606671432964504M };
                            b_HighOrder = new decimal[] { 0.0271990297818803M, 0.1772488819905108M, 0.0378528418949694M, 0.6086431830142991M, 0.2154313974316100M, 0.2066152563885843M, 0.0415864076069797M, 0.0219891884310925M, 0.9893081222650993M, 0.0063199019859826M, 0.3749640721105318M, 1.6080235151003195M, 0.0961209123818189M };

                            b_LowOrder = new decimal[b_HighOrder.Length];

                            d = new decimal[b_HighOrder.Length];
                            A = new decimal[c.Length, c.Length];
                            for (int i = 1; i < c.Length; i++)
                            {
                                A[i, i - 1] = c[i];
                            }
                            for (int i = 0; i < b_HighOrder.Length; i++)
                            {
                                b_LowOrder[i] = 0M;
                                d[i] = 0M;
                            }
                            LowImpactMethod = true;
                            GenerateDoubleVariables();
                        }
                        else if (pintegrationType == IntegrationType.LRK_3_2)
                        {
                            c = new decimal[] { 0M, 11847461282814M / 36547543011857M, 3943225443063M / 7078155732230M, -346793006927M / 402990357606M };
                            b_HighOrder = new decimal[] { 1017324711453M / 9774461848756M, 8237718856693M / 13685301971492M, 57731312506979M / 19404895981398M, -101169746363290M / 37734290219643M };

                            b_LowOrder = new decimal[] { 15763415370699M / 46270243929542M, 514528521746M / 5659431552419M, 27030193851939M / 9429696342944M, -69544964788955M / 30262026368149M };

                            d = new decimal[b_HighOrder.Length];
                            A = new decimal[c.Length, c.Length];
                            for (int i = 1; i < c.Length; i++)
                            {
                                A[i, i - 1] = c[i];
                            }
                            for (int i = 0; i < b_HighOrder.Length; i++)
                            {
                                b_LowOrder[i] = 0M;
                                d[i] = b_HighOrder[i] - b_LowOrder[i];
                            }
                            LowImpactMethod = true;
                            GenerateDoubleVariables();

                        }
                        else
                        {
                            GenerateDoubleVariables();
                        }


                    }

                }
            }
            private void GenerateDoubleVariables()
            {
                c_double = new double[A.GetLength(0)];
                b_HighOrder_double = new double[A.GetLength(0)];
                b_LowOrder_double = new double[A.GetLength(0)];
                d_double = new double[A.GetLength(0)];

                A_double = new double[A.GetLength(0), A.GetLength(1)];

                for (int i = 0; i < A.GetLength(0); i++)
                {
                    c_double[i] = (double)c[i];
                    b_HighOrder_double[i] = (double)b_HighOrder[i];
                    b_LowOrder_double[i] = (double)b_LowOrder[i];
                    d_double[i] = (double)d[i];

                    for (int j = 0; j < A.GetLength(1); j++)
                        A_double[i, j] = (double)A[i, j];
                }
            }

            public enum IntegrationType
            {
                RungeKuttaK41,
                Verner56,
                DormandPrince56,
                CashKarp45,
                //Low storage explicit Runge-Kutta method from Diomar Cesar Lobão
                LSERK54,
                LSERK74,
                LSERK_13_4,
                LRK_3_2
            }

            public ExplicitIntegrator()
            {
                StoredIntegrationTypes = IntegrationType.RungeKuttaK41;
            }

            /// <summary>
            /// General Runge Kutta iuntegration method from a Butcher tablau. Default loads the RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x and t as parameter and retuns a decimal value</param>
            /// <param name="x">Starting point</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public decimal CalculateStep(Func<decimal, decimal, decimal> df, decimal x, decimal t, decimal dt)
            {
                // Stores the step results
                decimal[] Increments = new decimal[c.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    decimal increment = x;

                    // Go trough all stage results in the current Butcher tableau
                    for (int j = 0; j < i; j++)
                        increment += dt * Increments[j] * A[i, j];

                    //Save the current stage 
                    Increments[i] = df(increment, t + c[i] * dt);
                }

                // Startinpoint for integration (same as the + C in integrating)
                decimal Result = x;

                // Add the integration weights (b) used for each stage result
                for (int i = 0; i < b_HighOrder.Length; i++)
                    Result += dt * b_HighOrder[i] * Increments[i];

                return Result;
            }

            /// <summary>
            /// General Runge Kutta integration method for a system of differential equations from a Butcher tablau. By default it also loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a decimal[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public decimal[,] CalculateStep(Func<decimal[,], decimal, decimal[,]> df, decimal[,] x, decimal t, decimal dt)
            {

                // Stores the step results
                decimal[,,] Increments = new decimal[c.Length, x.GetLength(0), x.GetLength(1)];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    decimal[,] increment = x;


                    for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                    {
                        for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                        {
                            // Low impact ERK method or not?
                            if (LowImpactMethod)
                            {
                                // For low impact Butcher tablau c[i] is equal to
                                // A[i,i-1] since all other elements in row i is zero
                                // Low impact saves 1 loop
                                increment[x_n, y_n] += dt * Increments[i, x_n, y_n] * c[i];
                            }
                            else
                            {
                                // Go trough all stage results in the current Butcher tableau
                                for (int j = 0; j < i; j++)
                                    increment[x_n, y_n] += dt * Increments[j, x_n, y_n] * A[i, j];
                            }

                        }

                    }
                    //Save the current stage 
                    decimal[,] differential = df(increment, t + c[i] * dt);

                    for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                    {
                        for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                            Increments[i, x_n, y_n] = differential[x_n, y_n];
                    }
                }
                // Startinpoint for integration (same as the + C in integrating)
                decimal[,] Result = x;
                for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                {
                    for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                    {
                        // Add the integration weights (b) used for each stage result
                        for (int i = 0; i < b_HighOrder.Length; i++)
                            Result[x_n, y_n] += dt * b_HighOrder[i] * Increments[i, x_n, y_n];
                    }
                }


                return Result;
            }

            /// <summary>
            /// General Runge Kutta integration method for a system of differential equations from a Butcher tablau. By default it also loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a decimal[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public decimal[] CalculateStep(Func<decimal[], decimal, decimal[]> df, decimal[] x, decimal t, decimal dt)
            {
                // Startinpoint for integration (same as the + C in integrating)
                decimal[] Result = x;

                // Stores the step results
                decimal[,] Increments = new decimal[c.Length, x.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    decimal[] increment = x;
                    for (int x_n = 0; x_n < x.Length; x_n++)
                    {
                        // Low impact ERK method or not?
                        if (LowImpactMethod)
                        {
                            // For low impact Butcher tablau c[i] is equal to
                            // A[i,i-1] since all other elements in row i is zero
                            // Low impact saves 1 loop
                            increment[x_n] += dt * Increments[i, x_n] * c[i];
                        }
                        else
                        {
                            // Go trough all stage results in the current Butcher tableau
                            for (int j = 0; j < i; j++)
                                increment[x_n] += dt * Increments[j, x_n] * A[i, j];
                        }
                    }
                    //Save the current stage 
                    decimal[] differential = df(increment, t + c[i] * dt);

                    for (int x_n = 0; x_n < x.Length; x_n++)
                        Increments[i, x_n] = differential[x_n];
                }

                for (int x_n = 0; x_n < x.Length; x_n++)
                {
                    // Add the integration weights (b) used for each stage result
                    for (int i = 0; i < b_HighOrder.Length; i++)
                        Result[x_n] += dt * b_HighOrder[i] * Increments[i, x_n];
                }

                return Result;
            }

            /// <summary>
            /// General Runge Kutta integration method from a Butcher tablau. By default it loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a decimal[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public decimal[] CalculateStep(Func<decimal[], decimal, decimal[]> df, decimal[] x, decimal t, decimal dt, Action<decimal[]> error)
            {

                // Stores the step results
                decimal[,] Increments = new decimal[c.Length, x.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    decimal[] increment = x;

                    for (int x_n = 0; x_n < x.Length; x_n++)
                    {
                        // Go trough all stage results in the current Butcher tableau
                        for (int j = 0; j < i; j++)
                            increment[x_n] += dt * Increments[j, x_n] * A[i, j];
                    }
                    //Save the current stage 
                    decimal[] differential = df(increment, t + c[i] * dt);
                    for (int x_n = 0; x_n < x.Length; x_n++)
                        Increments[i, x_n] = differential[x_n];
                }
                // Startinpoint for integration (same as the + C in integrating)
                decimal[] Result = x;
                decimal[] dE = new decimal[x.Length];
                for (int x_n = 0; x_n < x.Length; x_n++)
                {
                    // Add the integration weights (b) used for each stage result
                    for (int i = 0; i < b_HighOrder.Length; i++)
                    {
                        Result[x_n] += dt * b_HighOrder[i] * Increments[i, x_n];
                        dE[x_n] += dt * d[i] * Increments[i, x_n];
                    }
                }

                // Calculate relative error for each integration parameter
                for (int i = 0; i < x.Length; i++)
                    dE[i] /= Result[i];

                // Return the error for each integration, you can alternative take the List.Max to get the worst error
                error(dE);

                return Result;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="df"></param>
            /// <param name="x"></param>
            /// <param name="t"></param>
            /// <param name="dt"></param>
            /// <param name="error"></param>
            /// <returns></returns>
            public decimal CalculateStep(Func<decimal, decimal, decimal> df, decimal x, decimal t, decimal dt, Action<decimal> error)
            {
                // Stores the step results
                decimal[] Increments = new decimal[c.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    decimal increment = x;

                    // Go trough all stage results in the current Butcher tableau
                    for (int j = 0; j < i; j++)
                        increment += dt * Increments[j] * A[i, j];

                    //Save the current stage integration result
                    Increments[i] = df(increment, t + c[i] * dt);
                }

                // Startinpoint for integration for both orders (same as the + C in integrating)
                decimal result = x;

                // The error estimated from the difference 
                decimal dE = 0;
                // Add the weights (b) used for each stage result and each integration order
                for (int i = 0; i < b_HighOrder.Length; i++)
                {
                    result += dt * b_HighOrder[i] * Increments[i];
                    dE += dt * d[i] * Increments[i];
                }

                // Report back the estimation of the relative error. Lower dt will give a better estimation obviously due to Taylor series  
                error(dE / result);

                // Returns the integration of the highest order
                return result;
            }
            /// <summary>
            /// General Runge Kutta iuntegration method from a Butcher tablau. Default loads the RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x and t as parameter and retuns a double value</param>
            /// <param name="x">Starting point</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public double CalculateStep(Func<double, double, double> df, double x, double t, double dt)
            {
                // Stores the step results
                double[] Increments = new double[c.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    double increment = x;

                    // Go trough all stage results in the current Butcher tableau
                    for (int j = 0; j < i; j++)
                        increment += dt * Increments[j] * A_double[i, j];

                    //Save the current stage 
                    Increments[i] = df(increment, t + c_double[i] * dt);
                }

                // Startinpoint for integration (same as the + C in integrating)
                double Result = x;

                // Add the integration weights (b) used for each stage result
                for (int i = 0; i < b_HighOrder.Length; i++)
                    Result += dt * b_HighOrder_double[i] * Increments[i];

                return Result;
            }

            /// <summary>
            /// General Runge Kutta integration method for a system of differential equations from a Butcher tablau. By default it also loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a double[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public double[,] CalculateStep(Func<double[,], double, double[,]> df, double[,] x, double t, double dt)
            {

                // Stores the step results
                double[,,] Increments = new double[c.Length, x.GetLength(0), x.GetLength(1)];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value 
                    double[,] increment = x;


                    for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                    {
                        for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                        {
                            // Go trough all stage results in the current Butcher tableau
                            for (int j = 0; j < i; j++)
                                increment[x_n, y_n] += dt * Increments[j, x_n, y_n] * A_double[i, j];
                        }

                    }
                    //Save the current stage 
                    double[,] differential = df(increment, t + c_double[i] * dt);

                    for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                    {
                        for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                            Increments[i, x_n, y_n] = differential[x_n, y_n];
                    }
                }
                // Startinpoint for integration (same as the + C in integrating)
                double[,] Result = x;
                for (int x_n = 0; x_n < x.GetLength(0); x_n++)
                {
                    for (int y_n = 0; y_n < x.GetLength(1); y_n++)
                    {
                        // Add the integration weights (b) used for each stage result
                        for (int i = 0; i < b_HighOrder.Length; i++)
                            Result[x_n, y_n] += dt * b_HighOrder_double[i] * Increments[i, x_n, y_n];
                    }
                }


                return Result;
            }

            /// <summary>
            /// General Runge Kutta integration method for a system of differential equations from a Butcher tablau. By default it also loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a double[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public double[] CalculateStep(Func<double[], double, double[]> df, double[] x, double t, double dt)
            {

                // Stores the step results
                double[,] Increments = new double[c.Length, x.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    double[] increment = x;

                    for (int x_n = 0; x_n < x.Length; x_n++)
                    {
                        // Go trough all stage results in the current Butcher tableau
                        for (int j = 0; j < i; j++)
                            increment[x_n] += dt * Increments[j, x_n] * A_double[i, j];
                    }
                    //Save the current stage 
                    double[] differential = df(increment, t + c_double[i] * dt);
                    for (int x_n = 0; x_n < x.Length; x_n++)
                        Increments[i, x_n] = differential[x_n];
                }
                // Startinpoint for integration (same as the + C in integrating)
                double[] Result = x;
                for (int x_n = 0; x_n < x.Length; x_n++)
                {
                    // Add the integration weights (b) used for each stage result
                    for (int i = 0; i < b_HighOrder.Length; i++)
                        Result[x_n] += dt * b_HighOrder_double[i] * Increments[i, x_n];
                }
                return Result;

            }

            /// <summary>
            /// General Runge Kutta integration method from a Butcher tablau. By default it loads the classical RK41
            /// </summary>
            /// <param name="df">The derivative of the function that takes x[] and t as parameter and retuns a double[] value</param>
            /// <param name="x">Initial conditions for integration</param>
            /// <param name="t">Current time</param>
            /// <param name="dt">Time step used for numerical integration</param>
            /// <returns></returns>
            public double[] CalculateStep(Func<double[], double, double[]> df, double[] x, double t, double dt, Action<double[]> error)
            {

                // Stores the step results
                double[,] Increments = new double[c.Length, x.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    // Initial value
                    double[] increment = x;

                    for (int x_n = 0; x_n < x.Length; x_n++)
                    {
                        // Go trough all stage results in the current Butcher tableau
                        for (int j = 0; j < i; j++)
                            increment[x_n] += dt * Increments[j, x_n] * A_double[i, j];
                    }
                    //Save the current stage 
                    double[] differential = df(increment, t + c_double[i] * dt);
                    for (int x_n = 0; x_n < x.Length; x_n++)
                        Increments[i, x_n] = differential[x_n];
                }
                // Startinpoint for integration (same as the + C in integrating)
                double[] Result = x;
                double[] dE = new double[x.Length];
                for (int x_n = 0; x_n < x.Length; x_n++)
                {
                    // Add the integration weights (b) used for each stage result
                    for (int i = 0; i < b_HighOrder.Length; i++)
                    {
                        Result[x_n] += dt * b_HighOrder_double[i] * Increments[i, x_n];
                        dE[x_n] += dt * d_double[i] * Increments[i, x_n];
                    }
                }

                // Calculate relative error for each integration parameter
                for (int i = 0; i < x.Length; i++)
                    dE[i] /= Result[i];

                // Return the error for each integration, you can alternative take the List.Max to get the worst error
                error(dE);

                return Result;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="df"></param>
            /// <param name="x"></param>
            /// <param name="t"></param>
            /// <param name="dt"></param>
            /// <param name="error"></param>
            /// <returns></returns>
            public double CalculateStep(Func<double, double, double> df, double x, double t, double dt, Action<double> error)
            {
                // Stores the step results
                double[] Increments = new double[c.Length];

                // Go through each step of the Butcher tablau
                for (int i = 0; i < c.Length; i++)
                {
                    double increment = x;

                    // Go trough all stage results in the current Butcher tableau
                    for (int j = 0; j < i; j++)
                        increment += dt * Increments[j] * A_double[i, j];

                    //Save the current stage integration result
                    Increments[i] = df(increment, t + c_double[i] * dt);
                }

                // Startinpoint for integration for both orders (same as the + C in integrating)
                double result = x;

                // The error estimated from the difference 
                double dE = 0;
                // Add the weights (b) used for each stage result and each integration order
                for (int i = 0; i < b_HighOrder.Length; i++)
                {
                    result += dt * b_HighOrder_double[i] * Increments[i];
                    dE += dt * d_double[i] * Increments[i];
                }

                // Report back the estimation of the relative error. Lower dt will give a better estimation obviously due to Taylor series  
                error(dE / result);

                // Returns the integration of the highest order
                return result;
            }
        }
    }

}
