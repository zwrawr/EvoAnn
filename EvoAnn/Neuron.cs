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
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class represents a single Neuron with in the Neural Net
    /// </summary>
    [Serializable]
    public class Neuron
    {
        /*
         * Variables
         */

        /// <summary>
        /// a static random number generator common to all neurons
        /// </summary>
        private static Random neuronRand;

        /// <summary>
        /// this neurons position in the layer
        /// </summary>
        private int index;

        /// <summary>
        /// gradient of learning
        /// </summary>
        private double gradient;

        /// <summary>
        /// Which type of neuron this is.
        /// </summary>
        private NeuronType neuronType;

        /*
         * constructors
         */

        /// <summary>
        ///  Initializes a new instance of the <see cref="Neuron" /> class with<see pref="numOutputs"/> outputs and in position <see pref="index"/>
        /// </summary>
        /// <param name="numOutputs">
        /// The number of outputs this Neurons has which is the same as the 
        /// number of Neurons in the next layer (excluding bias neurons) 
        /// </param>
        /// <param name="index"> This nodes position in its layer</param>
        /// <param name="neuronType"> Weather this Neuron is an input, output, bias or hidden neuron</param>
        public Neuron(int numOutputs, int index, NeuronType neuronType)
        {
            this.neuronType = neuronType;

            this.InitNeuronRand();
            this.OutputValue = this.RandomUnitDouble();

            this.OutputWeights = new double[numOutputs];
            for (int i = 0; i < numOutputs; i++)
            {
                this.OutputWeights[i] = this.RandomUnitDouble();
            }

            this.OutputDeltaWeights = new double[numOutputs];

            this.index = index;
        }

        /*
         * ENums
         */

        /// <summary>
        /// Represents the different types of Neurons
        /// </summary>
        public enum NeuronType
        {
            /// <summary>
            /// Represents a Neuron that receives direct input.
            /// </summary>
            INPUT,

            /// <summary>
            /// Represents a Neuron that acts as a bias and always has an output of 1.
            /// </summary>
            BIAS,

            /// <summary>
            /// Represents a hidden neuron which is neither an input ,output or bias.
            /// </summary>
            HIDDEN,

            /// <summary>
            /// Represents a Output neuron which will be read for output.
            /// </summary>
            OUTPUT
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

        /*
         * Methods
         */

        /// <summary>
        /// Allows the inputs of Neurons of NeuronType.INPUT to be set
        /// </summary>
        /// <param name="input">value to set this neuron to</param>
        public void SetInput(double input)
        {
            if (this.neuronType == NeuronType.INPUT)
            {
                this.OutputValue = input;
            }
            else
            {
                Console.WriteLine("Cannot set the input value of neurons that are not NeuronType.INPUT Neurons");
            }
        }

        /// <summary>
        /// propagates the inputs (outputs of previous Layer) forward to the output
        /// </summary>
        /// <param name="prevLayer"> The layer before this Neurons layer in the Net </param>
        public void FeedForward(Neuron[] prevLayer)
        {
            double sum = 0.0;

            for (int i = 0; i < prevLayer.Length; i++)
            {
                double output = prevLayer[i].OutputValue;
                double weight = prevLayer[i].OutputWeights[this.index];
                sum += output * weight;
            }

            this.OutputValue = this.TransferFunction(sum);
        }

        /// <summary>
        /// calculates the gradient of a hidden layer
        /// </summary>
        /// <param name="nextLayer">the next layer of nodes in the net</param>
        public void CalcHiddenGradients(Neuron[] nextLayer)
        {
            double dow = this.SumDow(nextLayer);
            this.gradient = dow * this.TransferFunctionDerivative(this.OutputValue);
        }

        /// <summary>
        /// calculates the gradient of the output layer
        /// </summary>
        /// <param name="targetVal">the expected output value</param>
        public void CalcOutputGradients(double targetVal)
        {
            double delta = targetVal - this.OutputValue;
            this.gradient = delta * this.TransferFunctionDerivative(this.OutputValue);
        }

        /// <summary>
        /// Updates the previous layers output weights , i.e. this layers input weights
        /// </summary>
        /// <param name="prevLayer"> The previous layer in the Net</param>
        public void UpdateInputWeights(Neuron[] prevLayer)
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
        /// Maps an input value to a value between 0 and 1
        /// </summary>
        /// <param name="value">input value for the transfer function</param>
        /// <returns> a value mapped between -1 and 1 </returns>
        private double TransferFunction(double value)
        {
            return Math.Tanh(value);
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
