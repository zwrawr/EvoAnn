// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="Simulation.cs" company="Zak R. A. West">
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
    /// This class simulates a Chromosomes response to find out it's fitness.
    /// </summary>
    public class Simulation
    {
        /// <summary>
        /// The <see cref="EvoManager"/> responsible for this simulation.
        /// </summary>
        private EvoManager manager;

        /// <summary>
        /// The population of <see cref="Chromo"/>s to be simulated
        /// </summary>
        private int population;

        /// <summary>
        /// This will be removed soon
        /// </summary>
        /// TODO: Simulation should be abstact and not care about the topology of a net
        private int[] topology;

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulation" /> class.
        /// </summary>
        /// <param name="population"> Number of <see cref="Chromo"/> it will simulate</param>
        public Simulation(int population)
        {
            this.population = population;

            this.topology = new int[] { 2, 4, 4, 2 };

            this.manager = new EvoManager(this.population, this.topology);

            this.RunGeneration(new double[] { 0, 1 }, new double[] { 0, 1 });
        }

        /// <summary>
        /// Runs a feed forward on every member of the population.
        /// </summary>
        /// <param name="inputs">the inputs to to the Simulation.</param>
        /// <param name="expectedOutputs">The desired outputs.</param>
        /// <returns>the results.</returns>
        private double[] RunGeneration(double[] inputs, double[] expectedOutputs)
        {
            double[] errors = new double[this.population];

            this.manager.FeedGeneration(inputs);

            double[][] obtainedOutputs = this.manager.ResultsFromGeneration();

            for (int i = 0; i < this.population; i++)
            {
                errors[i] = 0.0;
                for (int j = 0; j < this.topology[this.topology.Length - 1]; j++)
                {
                    errors[i] += Math.Pow(obtainedOutputs[i][j] - expectedOutputs[j], 2.0);
                }

                errors[i] = Math.Sqrt(errors[i]);
            }

            return errors;
        }
    }
}
