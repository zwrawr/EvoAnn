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

            Net net = new Net(new int[] { 2, 4, 4, 1 });

            net.FeedForward(new int[] { 1, 0 });

            double[] outputs = net.GetResults();

            net.BackProp(new double[] { Program.and(1, 0)});

        }

        private static int and(int a, int b)
        {
            if (a > 0.5 && b > 0.5)
            {
                return 1;
            }
            return 0;
        } 
    }
}
