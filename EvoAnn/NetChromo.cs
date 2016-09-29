// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="NetChromo.cs" company="Zak R. A. West">
// EvoAnn  Copyright (C) 2016  Zak R. A. West
// </copyright>
// <author> Zak R. A. West , zakr.a.west@gmail.com , zwrawr@gmail.com </author>
// =====================================================
namespace Evo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class represent the Chromosome information of a Net and is responsible for breading chromosomes
    /// </summary>
    public class NetChromo
    {
        /// <summary>
        /// a static random number generator common to all NetChromos
        /// </summary>
        private static Random chromoRand;

        /// <summary>
        /// The chromosome data that represents a Net
        /// </summary>
        private double[][][] chromoData;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetChromo"/> class. a 3D array of doubles represents the  
        /// </summary>
        /// <param name="data">Chromosome data</param>
        public NetChromo(double[][][] data)
        {
            this.chromoData = data;
            this.InitChromoRand();
        }

        /// <summary>
        /// breads the two NetChromo objects to create a new child NetChromo that inherits traits from both parent NetChromos
        /// </summary>
        /// <param name="mother">One of the parent NetChromo s</param>
        /// <param name="father">The other of the parent NetChromo s</param>
        /// <param name="mutationChance">The chance of a mutation happening. [0..1] should be a small value</param>
        /// <param name="mutationRate">How large of a chance will happen to a trait if it mutates [0..1] should be a small value</param>
        /// <returns>A new NetChromo based of the two parent NetChromos</returns>
        public static NetChromo Breed(NetChromo mother, NetChromo father, double mutationChance, double mutationRate)
        {
            NetChromo child = new NetChromo(mother.chromoData);

            for (int a = 0; a < child.chromoData.Length; a++)
            {
                for (int b = 0; b < child.chromoData[a].Length; b++)
                {
                    for (int c = 0; c < child.chromoData[a][b].Length; c++)
                    {
                        if (chromoRand.NextDouble() < 0.5)
                        {
                            child.chromoData[a][b][c] = father.chromoData[a][b][c];
                        }

                        if (chromoRand.NextDouble() < mutationChance)
                        {
                            child.chromoData[a][b][c] += mutationRate - (2.0 * mutationRate * chromoRand.NextDouble());
                        }
                    }
                }
            }

            return child;
        }

        /// <summary>
        /// sets up the random number generator
        /// </summary>
        private void InitChromoRand()
        {
            if (NetChromo.chromoRand == null)
            {
                NetChromo.chromoRand = new Random();
            }
        }
    }
}
