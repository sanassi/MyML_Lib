using System;
using System.IO;
using System.Threading.Tasks;

namespace MyML_Lib
{
    public class Network
    {
        public int sizeInput;
        public int sizeHidden;
        public int sizeOutput;
        
        //public Matrix<double> input;
        public Matrix<double> hidden;
        public Matrix<double> output;

        public Matrix<double> w_i_h;
        public Matrix<double> w_h_o;

        public Matrix<double> b_h;
        public Matrix<double> b_o;

        public double eta;
        
        
        /*3 layer neural network*/
        public Network(int sizeInput, int sizeHidden, int sizeOutput)
        {
            this.sizeInput = sizeInput;
            this.sizeHidden = sizeHidden;
            this.sizeOutput = sizeOutput;
            
            //this.input = new Vector<double>(sizeInput);
            this.hidden = new Vector<double>(sizeHidden, -1, 1);
            this.output = new Vector<double>(sizeOutput, -1, 1);
            
            this.b_h = new Vector<double>(sizeHidden, -1, 1);
            this.b_o = new Vector<double>(sizeOutput, -1, 1);

            this.w_i_h = new Matrix<double>(sizeHidden, sizeInput, -1, 1);
            this.w_h_o = new Matrix<double>(sizeOutput, sizeHidden, -1, 1);
        }

        public Network(string path)
        {
            this.Load(path);
        }

        public void FeedForward(Vector<double> x)
        {
            this.hidden = Matrix<double>.Sigmoid(w_i_h * x + b_h);
            this.output = Matrix<double>.Sigmoid(w_h_o * hidden + b_o);
        }

        public void Train(Vector<double> x, Vector<double> y, double alpha)
        {
            FeedForward(x);

            Matrix<double> outputErrors = y - output;

            /*
            Matrix<double> outputGradients = Matrix<double>.SigmoidPrime(output);
            Matrix<double> delta_WHO = alpha * (outputErrors ^ outputGradients) * (~hidden);

            Matrix<double> w_h_o_t = ~w_h_o;
            Matrix<double> hiddenErrors = w_h_o_t * outputErrors;
            Matrix<double> hiddenGradients = Matrix<double>.SigmoidPrime(hidden);
            Matrix<double> delta_WIH = alpha * (hiddenErrors ^ hiddenGradients) * (~x);
            */
            /*
            Matrix<double> w_h_o_t = ~w_h_o;
            Matrix<double> hiddenErrors = w_h_o_t * outputErrors;

            Matrix<double> delta_WHO = alpha * ((outputErrors ^ Matrix<double>.SigmoidPrime(output)) * (~hidden));
            Matrix<double> delta_WIH = alpha * ((hiddenErrors ^ Matrix<double>.SigmoidPrime(hidden)) * (~x));
            */

            /*9:28*/

            Matrix<double> gradients = Matrix<double>.SigmoidPrime(output) ^ outputErrors * alpha;

            Matrix<double> hiddenT = ~hidden;
            Matrix<double> delta_WHO = gradients * hiddenT;

            w_h_o += delta_WHO;
            b_o += gradients;

            Matrix<double> WHO_T = ~w_h_o;
            Matrix<double> hiddenError = WHO_T * outputErrors;

            Matrix<double> hiddenGradient = Matrix<double>.SigmoidPrime(hidden) ^ hiddenError * alpha;

            Matrix<double> inputTranspose = ~x;
            Matrix<double> delta_WIH = hiddenGradient * inputTranspose;

            w_i_h += delta_WIH;
            b_h += hiddenGradient;
        }
        
        // Save and Load NN
        public void Save(string folderPath)
        {
            using (StreamWriter sr =
                new StreamWriter(folderPath + "netInfo.txt"))
            {
                sr.WriteLine(sizeInput);
                sr.WriteLine(sizeHidden);
                sr.WriteLine(sizeOutput);
                
                sr.WriteLine(eta);
            }
            
            using (StreamWriter sw =
                   new StreamWriter(folderPath + "w_i_h.txt"))
            {
                for (int i = 0; i < sizeHidden; i++)
                {
                    for (int j = 0; j < sizeInput; j++)
                    {
                        sw.WriteLine(w_i_h[i,j]);
                    }
                }
            }
            
            using (StreamWriter sw =
                new StreamWriter(folderPath + "w_h_o.txt"))
            {
                for (int i = 0; i < sizeOutput; i++)
                {
                    for (int j = 0; j < sizeHidden; j++)
                    {
                        sw.WriteLine(w_h_o[i, j]);
                    }
                }
            }
            
            using (StreamWriter sw =
                new StreamWriter(folderPath + "b_h.txt"))
            {
                for (int i = 0; i < sizeHidden; i++)
                {
                    sw.WriteLine(b_h[i, 0]);
                    
                }
            }
            
            using (StreamWriter sw =
                new StreamWriter(folderPath + "b_o.txt"))
            {
                for (int i = 0; i < sizeOutput; i++)
                {
                    sw.WriteLine(b_o[i, 0]);
                    
                }
            }
        }
        public void Load(string folderPath)
        {
            using (StreamReader sr = 
                   new StreamReader(folderPath + "netInfo.txt"))
            {
                this.sizeInput = Convert.ToInt32(sr.ReadLine());
                this.sizeHidden = Convert.ToInt32(sr.ReadLine());
                this.sizeOutput = Convert.ToInt32(sr.ReadLine());
            }
            
            this.hidden = new Vector<double>(sizeHidden);
            this.output = new Vector<double>(sizeOutput);
            
            this.b_h = new Vector<double>(sizeHidden);
            this.b_o = new Vector<double>(sizeOutput);

            this.w_i_h = new Matrix<double>(sizeHidden, sizeInput);
            this.w_h_o = new Matrix<double>(sizeOutput, sizeHidden);
            
            using (StreamReader sr = 
                new StreamReader(folderPath + "w_i_h.txt"))
            {
                for (int i = 0; i < sizeHidden; i++)
                {
                    for (int j = 0; j < sizeInput; j++)
                    {
                        w_i_h[i, j] = Convert.ToDouble(sr.ReadLine());
                    }
                }
            }
            
            using (StreamReader sr = 
                new StreamReader(folderPath + "w_h_o.txt"))
            {
                for (int i = 0; i < sizeOutput; i++)
                {
                    for (int j = 0; j < sizeHidden; j++)
                    {
                        w_h_o[i, j] = Convert.ToDouble(sr.ReadLine());
                    }
                }
            }
            
            using (StreamReader sr = 
                new StreamReader(folderPath + "b_h.txt"))
            {
                for (int i = 0; i < sizeHidden; i++)
                {
                    b_h[i, 0] = Convert.ToDouble(sr.ReadLine());
                }
            }
            
            using (StreamReader sr = 
                new StreamReader(folderPath + "b_o.txt"))
            {
                for (int i = 0; i < sizeOutput; i++)
                {
                    b_o[i, 0] = Convert.ToDouble(sr.ReadLine());
                }
            }
        }
    }
}