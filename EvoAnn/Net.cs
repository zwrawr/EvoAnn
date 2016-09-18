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
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class represent the Net structure of an ANN
    /// </summary>
    public class Net
    {
        private static int numSamplesRecentAvgError = 100;

        private Neuron[][] neurons;

        private int[] topology;

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
        }

        public double Error { get; protected set; }

        public double RecentAvgError { get; protected set; }

        public void FeedForward(int[] inputs)
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

            // TODO improve this recent average
            // a simplitic moveing average
            this.RecentAvgError =
                            ((this.RecentAvgError * Net.numSamplesRecentAvgError) + this.Error)
                            / (Net.numSamplesRecentAvgError + 1.0);

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
