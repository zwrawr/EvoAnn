// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="Chromo.cs" company="Zak R. A. West">
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
    /// Represents a system as a set of data, and provides methods breeding them.
    /// </summary>
    public abstract class Chromo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chromo"/> class.
        /// </summary>
        public Chromo()
        {
            this.InitChromoRand();
        }

        /// <summary>
        /// Gets a static random number generator common to all NetChromos
        /// </summary>
        protected static Random ChromoRand { get; private set; }

        /// <summary>
        /// Breeds two <see cref="Chromo"/>s to create a new <see cref="Chromo"/> that shares both of there genes with some mutations.
        /// </summary>
        /// <param name="other">The other parent <see cref="Chromo"/> that the child will shared its genes with.</param>
        /// <param name="mutationChance">The chance of a mutation occurring.</param>
        /// <param name="mutationRate">How much a gene will change if it mutates.</param>
        /// <returns>A child <see cref="Chromo"/> that shares this' and other's genes.</returns>
        public abstract Chromo Breed(Chromo other, double mutationChance, double mutationRate);

        /// <summary>
        /// sets up the random number generator
        /// </summary>
        private void InitChromoRand()
        {
            if (Chromo.ChromoRand == null)
            {
                Chromo.ChromoRand = new Random();
            }
        }
    }
}
