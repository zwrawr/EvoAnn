// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="Program.cs" company="Zak R. A. West">
// EvoAnn  Copyright (C) 2016  Zak R. A. West
// </copyright>
// <author> Zak R. A. West , zakr.a.west@gmail.com , zwrawr@gmail.com </author>
// =====================================================
namespace EvoAnn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ann;
    using System.Xml.Serialization;
    using System.IO;

    /// <summary>
    /// Entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// First method to be called when the program starts
        /// </summary>
        /// <param name="args"> command line values that were passed into the program</param>
        public static void Main(string[] args)
        {
            test1(10000);

            waitforinput();

            test2(10000);

            waitforinput();

            test3(10000);

            waitforinput();
        }

        private static void waitforinput()
        {
            Console.WriteLine("Press any key to continue");
            Console.In.ReadLine();
        }

        private static void test1(int runs)
        {
            Console.WriteLine("Test 1 AND inputs");

            Random r = new Random(0);
            Net net = new Net(new int[] { 2, 4, 4, 1 });

            for (int i = 0; i < runs; i++)
            {
                double[] inputs = new double[2];
                inputs[0] = 2.0 * (r.NextDouble() - 0.5);
                inputs[1] = 2.0 * (r.NextDouble() - 0.5);

                net.FeedForward(inputs);

                double[] results = net.GetResults();

                net.BackProp(new double[] { Program.and(inputs[0], inputs[1]) });

                if (i % (runs / 10 ) == 0) Console.WriteLine( "Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
            }

        }


        private static void test2(int runs)
        {
            Console.WriteLine("\n\rTest 2 positive or negitive ");

            Random r = new Random(0);
            Net net = new Net(new int[] { 1, 4, 4, 2 });

            for (int i = 0; i < runs; i++)
            {
                double[] inputs = new double[1];
                inputs[0] = 2.0 * (r.NextDouble() - 0.5);

                net.FeedForward(inputs);

                double[] results = net.GetResults();

                net.BackProp(Program.posneg(inputs[0]) );

                if ( i % (runs / 10) == 0  )Console.WriteLine("Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
            }
        }

        private static void test3(int runs)
        {
            Console.WriteLine("\n\rTest 3 average near zero ");

            Random r = new Random(0);
            Net net = new Net(new int[] { 10, 5, 3, 1 });

            for (int i = 0; i < runs; i++)
            {
                double[] inputs = new double[10];

                for (int j = 0; j < inputs.Length; j++)
                {
                    inputs[j] = 2.0 * (r.NextDouble() - 0.5);
                }

                net.FeedForward(inputs);

                double[] results = net.GetResults();

                net.BackProp(Program.avgzero(inputs));

                if (i % (runs / 10) == 0) Console.WriteLine("Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
            }

        }

        private static double and(double a, double b)
        {
            if (a > 0.5 && b > 0.5)
            {
                return 1;
            }
            return -1;
        }

        private static double[] posneg(double i)
        {
            if (i >= 0.5)
            {
                return new double[] { 1, -1 };
            }
            return new double[] { -1, 1 };
        }

        private static double[] avgzero(double[] inputs)
        {
            double avg = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                avg = inputs[i];
            }

            avg /= inputs.Length;

            return new double[] { 1 / (avg * avg * avg * avg) };
        }
    }
}
