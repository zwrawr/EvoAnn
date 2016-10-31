// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="EvoManager.cs" company="Zak R. A. West">
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
    using Ann;

    /// <summary>
    /// Class manages the population of Nets and is responsible for testing there fitness and creating the next generation
    /// </summary>
    public class EvoManager
    {

        private Net[] population;
        private int populationSize;

        private int[] networkTopology;

        public EvoManager(int populationSize, int[] networkTopology) {

            this.populationSize = populationSize;
            this.population = new Net[populationSize];

            this.networkTopology = networkTopology;
            for (int i = 0; i < this.populationSize; i++)
            {
                population[i] = new Net(this.networkTopology);
            }
        }

        public void feedGeneration(double[] inputs)
        {
            // feed forward each net
            for (int i = 0; i < this.populationSize; i++)
            {
                population[i].FeedForward(inputs);
            }
        }

        public double[][] resultsFromGeneration()
        {
            double[][] results = new double[this.populationSize][];
            // get the results from each net
            for (int i = 0; i < this.populationSize; i++)
            {
                results[i] = population[i].GetResults();
            }

            return results;
        }


    }
}
