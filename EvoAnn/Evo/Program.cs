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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

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
            Test1(10000);

            WaitForInput();

            Test2(10000);

            WaitForInput();

            Test3(10000);

            WaitForInput();
        }

        /// <summary>
        /// Waits for a key press in the console before returning.
        /// </summary>
        private static void WaitForInput()
        {
            Console.WriteLine("Press any key to continue");
            Console.In.ReadLine();
        }

        /// <summary>
        /// The first test.
        /// </summary>
        /// <param name="runs">Number of times to run.</param>
        private static void Test1(int runs)
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

                net.BackProp(new double[] { Program.And(inputs[0], inputs[1]) });

                if (i % (runs / 10) == 0)
                {
                    Console.WriteLine("Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
                }
            }
        }

        /// <summary>
        /// The second test.
        /// </summary>
        /// <param name="runs">Number of times to run.</param>
        private static void Test2(int runs)
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

                net.BackProp(Program.PosNeg(inputs[0]));

                if (i % (runs / 10) == 0)
                {
                    Console.WriteLine("Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
                }
            }
        }

        /// <summary>
        /// The third test.
        /// </summary>
        /// <param name="runs">Number of times to run.</param>
        private static void Test3(int runs)
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

                net.BackProp(Program.AvgZero(inputs));

                if (i % (runs / 10) == 0)
                {
                    Console.WriteLine("Run " + i + " Error : " + net.Error + " avgError : " + net.RecentAvgError);
                }
            }
        }

        /// <summary>
        /// Computes an and function of two doubles
        /// </summary>
        /// <param name="a">the first input.</param>
        /// <param name="b">the second input.</param>
        /// <returns>If A and B are grater than 0.5 then it returns 1 else it returns 0.</returns>
        private static double And(double a, double b)
        {
            if (a > 0.5 && b > 0.5)
            {
                return 1;
            }
            
            return -1;
        }

        /// <summary>
        /// Returns positive and negative based on if i grater than 0.5.
        /// </summary>
        /// <param name="i">The input.</param>
        /// <returns>An array of 1 and -1.</returns>
        private static double[] PosNeg(double i)
        {
            if (i >= 0.5)
            {
                return new double[] { 1, -1 };
            }

            return new double[] { -1, 1 };
        }

        /// <summary>
        /// Provides a high output when the average of the inputs is near zero.
        /// </summary>
        /// <param name="inputs">The inputs to be averaged</param>
        /// <returns>A high value when the average of the input is zero.</returns>
        private static double[] AvgZero(double[] inputs)
        {
            double avg = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                avg = inputs[i];
            }

            avg /= inputs.Length;

            return new double[] { 1 - (avg * avg) };
        }
    }
}
