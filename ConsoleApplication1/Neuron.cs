// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="Neuron.cs" company="Zak R. A. West">
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
    /// Class represents a single Neuron with in the Neural Net
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// a static random number generator common to all neurons
        /// </summary>
        private static Random neuronRand;

        /// <summary>
        /// this neurons position in the layer
        /// </summary>
        private uint index;

        /// <summary>
        /// gradient of learning
        /// </summary>
        private double gradient;

        /// <summary>
        ///  Initializes a new instance of the <see cref="Neuron" /> class with<see pref="numOutputs"/> outputs and in position <see pref="index"/>
        /// </summary>
        /// <param name="numOutputs">
        /// The number of outputs this Neurons has which is the same as the 
        /// number of Neurons in the next layer (excluding bias neurons) 
        /// </param>
        /// <param name="index"> This nodes position in its layer</param>
        public Neuron(uint numOutputs, uint index)
        {
            this.InitNeuronRand();
            this.OutputValue = this.RandomUnitDouble();

            this.OutputWeights = new double[numOutputs];
            for (int i = 0; i < numOutputs; i++)
            {
                this.OutputWeights[i] = this.RandomUnitDouble();
            }

            this.index = index;
        }

        /// <summary>
        /// Gets or sets this Neurons current output.
        /// </summary>
        public double OutputValue { get; protected set; }

        /// <summary>
        /// Gets or sets  this Neurons current weights.
        /// </summary>
        public double[] OutputWeights { get; protected set; }

        /// <summary>
        /// Gets or sets  this Neurons current delta weights.
        /// </summary>
        public double[] OutputDeltaWeights { get; protected set; }

        /// <summary>
        /// propagates the inputs (outputs of previous Layer) forward to the output
        /// </summary>
        /// <param name="prevLayer"> The layer before this Neurons layer in the Net </param>
        public void FeedForward(Neuron[] prevLayer)
        {
            double sum = 0.0;

            for (int i = 0; i < prevLayer.Length; i++)
            {
                sum += prevLayer[i].OutputValue * prevLayer[i].OutputWeights[this.index];
            }

            this.OutputValue = this.TransferFunction(sum);
        }

        /// <summary>
        /// Maps an input value to a value between 0 and 1
        /// </summary>
        /// <param name="value">input value for the transfer function</param>
        /// <returns> a value mapped between -1 and 1 </returns>
        private double TransferFunction(double value)
        {
            return Math.Tanh(value);
        }

        /// <summary>
        /// calculates the gradient
        /// </summary>
        /// <param name="nextLayer">the next layer of nodes in the net</param>
        private void CalcHiddenGradients(Neuron[] nextLayer)
        {
            double dow = this.SumDow(nextLayer);
            this.gradient = dow * this.TransferFunctionDerivative(this.OutputValue);
        }

        /// <summary>
        /// Maps an input value between 0 and 1 to the output.
        /// </summary>
        /// <param name="outputValue">input value for the transfer function derivative</param>
        /// <returns> the derivative of the transfer function </returns>
        private double TransferFunctionDerivative(double outputValue)
        {
            return 1.0 - (outputValue * outputValue);
        }

        /// <summary>
        /// TODO TODO TODO TODO TODO
        /// </summary>
        /// <param name="nextLayer"> the neurons in the next layer of the net </param>
        /// <returns>TODO TODO TODO TODO</returns>
        private double SumDow(Neuron[] nextLayer)
        {
            double sum = 0.0;

            // Sum our contributions of the errors at the nodes we feed.
            int numNeurons = nextLayer.Length - 1; // exclude bias neuron

            for (int i = 0; i < numNeurons; i++)
            {
                sum += this.OutputWeights[i] * nextLayer[i].gradient;
            }

            return sum;
        }

        /// <summary>
        /// Updates the previous layers output weights , i.e. this layers input weights
        /// </summary>
        /// <param name="prevLayer"> The previous layer in the Net</param>
        private void UpdateInputWeights(Neuron[] prevLayer)
        {
            for (int i = 0; i < prevLayer.Length; i++)
            {
                Neuron currNeuron = prevLayer[i];

                double oldDeltaWeight = currNeuron.OutputDeltaWeights[this.index];

                double newDeltaWeight = 
                    (AnnConfig.Eta * currNeuron.OutputValue * this.gradient) + (AnnConfig.Alpha * oldDeltaWeight);

                currNeuron.OutputDeltaWeights[this.index] = newDeltaWeight;
                currNeuron.OutputWeights[this.index] += newDeltaWeight;
            }
        }

        /// <summary>
        /// sets up the random number generator
        /// </summary>
        private void InitNeuronRand()
        {
            if (Neuron.neuronRand == null)
            {
                Neuron.neuronRand = new Random();
            }
        }

        /// <summary>
        /// method to get a random double between -1 and 1
        /// </summary>
        /// <returns>a random number between -1 and 1</returns>
        private double RandomUnitDouble()
        {
            return 2.0 * (0.5 - Neuron.neuronRand.NextDouble());
        }
    }
}
