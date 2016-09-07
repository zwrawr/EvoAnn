// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="AnnConfig.cs" company="Zak R. A. West">
// EvoAnn  Copyright (C) 2016  Zak R. A. West
// </copyright>
// <author> Zak R. A. West , zakr.a.west@gmail.com , zwrawr@gmail.com </author>
// =====================================================
namespace Ann
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class holds config info for the ANN such as training rate
    /// </summary>
    public static class AnnConfig
    {
        /// <summary>
        /// Initializes static members of the <see cref="AnnConfig" /> class.
        /// Sets Eta and Alpha to default values
        /// </summary>
        static AnnConfig()
        {
            Eta = 0.1;
            Alpha = 0.5;
        }

        /// <summary>
        /// Gets Net training weight
        /// </summary>
        public static double Eta { get; private set; }

        /// <summary>
        /// Gets Neuron training momentum
        /// </summary>
        public static double Alpha { get; private set; }

        /// <summary>
        /// Sets Eta to value and ensure it is between 0 and 1
        /// </summary>
        /// <param name="value"> Eta is set to value (clamped between 0 and 1)</param>
        public static void SetEta(double value)
        {
            Eta = value;

            if (Eta < 0)
            {
                Eta = 0;
            }

            if (Eta > 1)
            {
                Eta = 1;
            }
        }

        /// <summary>
        /// Sets Eta to value and ensures it is positive
        /// </summary>
        /// <param name="value"> Eta is set to value (clamped above 0)</param>
        public static void SetAlpha(double value)
        {
            Alpha = value;

            if (Alpha < 0)
            {
                Alpha = 0;
            }
        }
    }
}
