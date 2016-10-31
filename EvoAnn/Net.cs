// =====================================================
// <summary> EvoAnn , an atempt at making a evolutionary neural network </summary>
// <copyright file="Net.cs" company="Zak R. A. West">
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
    /// Class represent the Net structure of an ANN
    /// </summary>
    public class Net
    {
        /*
         * Variables
         */

        /// <summary>
        /// number of samples to include in the moving average 
        /// </summary>
        private static int numSamplesRecentAvgError = 100;

        /// <summary>
        /// The Neurons that make up this ANN , structured as layers
        /// </summary>
        private Neuron[][] neurons;

        /// <summary>
        /// The shape of this Net
        /// </summary>
        private int[] topology;

        /// <summary>
        /// a list of recent error values used to calculate the recent average error
        /// </summary>
        private List<double> recentErrors;

        /*
         * Constructors
         */
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Net"/> class
        /// </summary>
        /// <param name="topology">the 'shape' of the net</param>
        public Net(int[] topology)
        {
            this.Error = 0;
            this.RecentAvgError = 0;

            try
            {
                this.InitTopology(topology);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Exception: Bad Net topology");
                throw e;
            }

            this.CreateNeurons();

            this.recentErrors = new List<double>();
        }

        /*
         * Methods
         */

        /// <summary>
        /// Gets or sets the current error of the Net
        /// </summary>
        public double Error { get; protected set; }

        /// <summary>
        /// Gets or sets the recent error of the net
        /// </summary>
        public double RecentAvgError { get; protected set; }

        /// <summary>
        /// feeds the inputs through the net to generate the outputs
        /// </summary>
        /// <param name="inputs">the inputs to the Input Neurons</param>
        public void FeedForward(double[] inputs)
        {
            if (inputs.Length != this.topology[0] - 1)
            {
                Console.WriteLine("Exception: number of inputs must be the same as the number of inpt Neurons");
                throw new ArgumentOutOfRangeException("inputs", "Number of inputs must be the same as the number of inpt Neurons");
            }

            // Assign (latch) the input values into the input neurons
            for (int i = 0; i < inputs.Length; i++)
            {
                this.neurons[0][i].SetInput(inputs[i]);
            }

            // forward propagate excluding input layer
            for (int i = 1; i < this.topology.Length; i++)
            {
                for (int n = 0; n < this.topology[i] - 1; n++)
                {
                    this.neurons[i][n].FeedForward(this.neurons[i - 1]);
                }
            }
        }

        /// <summary>
        /// gets the results from the net
        /// </summary>
        /// <returns>The results from the net</returns>
        public double[] GetResults()
        {
            Neuron[] outputLayer = this.neurons.Last();

            int numNeuron = this.topology.Last() - 1;
            double[] outputs = new double[numNeuron];

            // loop over every output neuron
            for (int n = 0; n < numNeuron; n++)
            {
                outputs[n] = outputLayer[n].OutputValue;
            }

            return outputs;
        }

        /// <summary>
        /// adjusts the weights of the net to make the current inputs produce outputs closer to expectedOutputs
        /// </summary>
        /// <param name="expectedOutputs"> the expected output of a net that work correctly for the current input </param>
        public void BackProp(double[] expectedOutputs)
        {
            // reset error to zero
            this.Error = 0.0;

            // Loop over every output Neuron
            for (int n = 0; n < this.topology.Last() - 1; n++)
            {
                // find the diffrence between the current output and the expected output
                double delta = expectedOutputs[n] - this.neurons.Last()[n].OutputValue;

                // square the diffrence
                this.Error += delta * delta;
            }

            this.Error = this.Error / (this.topology.Last() - 1); // find the mean error
            this.Error = Math.Sqrt(this.Error); // root the error to get rms error

            if (this.recentErrors.Count >= Net.numSamplesRecentAvgError)
            {
                this.recentErrors.RemoveAt(0);
            }

            this.recentErrors.Add(this.Error);

            // calculate the average
            this.RecentAvgError = this.recentErrors.Average();

            // Loop over evry output neuron and recalculate gradients
            for (int n = 0; n < this.topology.Last() - 1; n++)
            {
                this.neurons.Last()[n].CalcOutputGradients(expectedOutputs[n]);
            }

            // do the same for all other neurons
            for (int i = this.topology.Length - 2; i > 0; i--)
            {
                Neuron[] hiddenLayer = this.neurons[i];
                Neuron[] nextLayer = this.neurons[i + 1];

                for (int n = 0; n < hiddenLayer.Length; n++)
                {
                    hiddenLayer[n].CalcHiddenGradients(nextLayer);
                }
            }
            
            // loop over all layers in reverse order , updating there weights
            for (int i = this.topology.Length - 1; i > 0; i--)
            {
                Neuron[] currLayer = this.neurons[i];
                Neuron[] prevLayer = this.neurons[i - 1];

                for (int n = 0; n < (currLayer.Length - 1); ++n)
                {
                    currLayer[n].UpdateInputWeights(prevLayer);
                }
            }
        }

        public double[][][] getNetWeights() {
            double[][][] netWeights = new double[this.topology.Length][][];

            for (int a = 0; a < this.topology.Length; a++) {
                netWeights[a] = new double[this.topology[a]][];

                for (int b = 0; b < this.topology[a]; b++)
                {
                    netWeights[a][b] = this.neurons[a][b].OutputWeights;
                }
            }

            return netWeights;
        }

        /// <summary>
        /// instantiate all of the neurons in this net
        /// </summary>
        private void CreateNeurons()
        {
            this.neurons = new Neuron[this.topology.Length][];

            for (int i = 0; i < this.topology.Length; i++)
            {
                this.neurons[i] = new Neuron[this.topology[i]];
                for (int j = 0; j < this.topology[i]; j++)
                {
                    this.neurons[i][j] = this.CreateNeuron(i, j);
                }
            }
        }

        /// <summary>
        /// creates a new neuron with values based off its position in the net
        /// </summary>
        /// <param name="layer">Which layer this neuron is in, used to decide if this is an input, hidden or output neuron</param>
        /// <param name="index">which position this neuron is within it's layer, used to decide if this is a bias neuron</param>
        /// <returns> a new Neuron, with the correct setup to be in layer layer and position index</returns>
        private Neuron CreateNeuron(int layer, int index)
        {
            // last neuron in any layer is a bias neuron
            if (index == this.topology[layer] - 1)
            {
                // check to see if were in the last layer
                if (layer == this.topology.Length - 1)
                {
                    // were in the last layer, the putput layer so we only have one output
                    return new Neuron(1, index, Neuron.NeuronType.BIAS);
                }
                else
                {
                    // were not in the output layer so we have as many outputs as the next layer has neurons ( minus one for the BIAS )
                    return new Neuron(this.topology[layer + 1] - 1, index, Neuron.NeuronType.BIAS);
                }
            }
            else if (layer == 0)
            {
                // nodes in the first layer are inputs
                return new Neuron(this.topology[layer + 1] - 1, index, Neuron.NeuronType.INPUT);
            }
            else if (layer == this.topology.Length - 1)
            {
                // nodes in the last layer are outputs
                return new Neuron(1, index, Neuron.NeuronType.OUTPUT);
            }
            else
            {
                // must be a non-bias neuron in a hidden layer
                return new Neuron(this.topology[layer + 1] - 1, index, Neuron.NeuronType.HIDDEN);
            }
        }

        /// <summary>
        /// Creates and initializes the topology of this Net. Adds space for the bias Neurons on each layer
        /// </summary>
        /// <param name="inputTopology">the specified topology, doesn't contain space for bias neurons</param>
        private void InitTopology(int[] inputTopology)
        {
            if (inputTopology.Length < 2)
            {
                Console.WriteLine("Exception: Net topology must have at least 2 layers");
                throw new ArgumentOutOfRangeException("inputTopology", "Net topology must have at least 2 layers");
            }

            this.topology = new int[inputTopology.Length];

            for (int i = 0; i < this.topology.Length; i++)
            {
                if (inputTopology[i] < 0)
                {
                    Console.WriteLine("Exception: Net topology must have at least 1 neuron in each layer");
                    throw new ArgumentOutOfRangeException("inputTopology[" + i + "]", "Net topology must have at least 1 neuron in each layer");
                }

                this.topology[i] = inputTopology[i] + 1; // add space for bias neuron
            }
        }
    }
}
