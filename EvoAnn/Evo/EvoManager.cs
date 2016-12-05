// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="EvoManager.cs" company="Zak R. A. West">
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

    /// <summary>
    /// Class manages the population of Nets and is responsible for testing there fitness and creating the next generation
    /// </summary>
    public class EvoManager
    {
        /// <summary>
        /// The <see cref="Net"/>s that make up the population.
        /// </summary>
        private Net[] population;

        /// <summary>
        /// The number of <see cref="Net"/>s in the population.
        /// </summary>
        private int populationSize;

        /// <summary>
        /// The topology of the <see cref="Net"/>s.
        /// </summary>
        private int[] networkTopology;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvoManager" /> class.
        /// </summary>
        /// <param name="populationSize">The number of <see cref="Net"/>s in the population.</param>
        /// <param name="networkTopology">The shape of the <see cref="Net"/>s topology.</param>
        public EvoManager(int populationSize, int[] networkTopology)
        {
            this.populationSize = populationSize;
            this.population = new Net[populationSize];
            this.networkTopology = networkTopology;

            for (int i = 0; i < this.populationSize; i++)
            {
                this.population[i] = new Net(this.networkTopology);
            }
        }

        /// <summary>
        /// Feeds the inputs into the system.
        /// </summary>
        /// <param name="inputs">The inputs to the system.</param>
        public void FeedGeneration(double[] inputs)
        {
            // feed forward each net
            for (int i = 0; i < this.populationSize; i++)
            {
                this.population[i].FeedForward(inputs);
            }
        }

        /// <summary>
        /// Gets the results from the <see cref="Net"/>s.
        /// </summary>
        /// <returns>An array of the results.</returns>
        public double[][] ResultsFromGeneration()
        {
            double[][] results = new double[this.populationSize][];

            // get the results from each net
            for (int i = 0; i < this.populationSize; i++)
            {
                results[i] = this.population[i].GetResults();
            }

            return results;
        }
    }
}
