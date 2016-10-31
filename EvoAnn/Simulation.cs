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

        public Simulation(int population) {
            this.manager = new EvoManager(population, new int[] { 2, 4, 4, 1 });
            manager.feedGeneration();
        }

    }
}
