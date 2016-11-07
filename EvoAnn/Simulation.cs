using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evo;

namespace EvoAnn
{
    class Simulation
    {

        private EvoManager manager;
        private int population;
        private int[] topology;

        public Simulation(int population) {
            this.population = population;

            this.topology = new int[] { 2, 4, 4, 2 };

            this.manager = new EvoManager(this.population,this.topology);

            runGeneration(new double[] { 0 , 1 } , new double[] { 0 , 1 });
        }

        private double[] runGeneration(double[] inputs, double[] expectedOutputs)
        {
            double[] errors = new double[population];

            manager.feedGeneration(inputs);

            double[][] obtainedOutputs = manager.resultsFromGeneration();

            for(int i = 0; i < population; i++)
            {
                errors[i] = 0.0;
                for(int j = 0; j < this.topology[this.topology.Length-1]; j++)
                {
                    errors[i] += Math.Pow(obtainedOutputs[i][j] - expectedOutputs[j], 2.0);
                }
                errors[i] = Math.Sqrt(errors[i]);
            }
            return errors;
        }
    }
}
