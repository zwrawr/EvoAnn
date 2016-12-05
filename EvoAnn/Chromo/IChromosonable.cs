// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="IChromosonable.cs" company="Zak R. A. West">
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
    /// An interface for classes that can be converted into <see cref="Chromo"/>'s.
    /// </summary>
    public interface IChromosonable
    {
        /// <summary>
        /// Converts a class that implements this interface to a <see cref="Chromo"/>.
        /// </summary>
        /// <returns>a <see cref="Chromo"/> that represents the class that implements this interface.</returns>
        Chromo ToChromo();

        /// <summary>
        /// Creates and instance of the class that implements this interface from a <see cref="Chromo"/>.
        /// </summary>
        /// <param name="data">A <see cref="Chromo"/>.</param>
        /// <returns> A instance of the class that implements the interface.</returns>
        object FromChromo(Chromo data);
    }
}
