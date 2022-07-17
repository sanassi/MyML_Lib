using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Drawing;
using static MyML_Lib.ShapeDetector;

namespace MyML_Lib
{
    class Program
    {
        public static Vector<double> VectorizeImage(Image img)
        {
            Vector<double> v = new Vector<double>(img.height * img.width);

            for (int i = 0; i < img.height; i++)
            {
                for (int j = 0; j < img.width; j++)
                {
                    v[i * img.height + j] = Convert.ToDouble(img.Data[i, j]) / 255;
                }
            }
            
            return v;
        }
        
        public static Vector<double> VectorizeBMP(Bitmap img)
        {
            Vector<double> v = new Vector<double>(img.Height * img.Width);

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    v[i * img.Height + j] = Convert.ToDouble(img.GetPixel(j, i).R) / 255;
                }
            }
            
            return v;
        }

        public static void ReverseGrayscale(Bitmap img)
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    img.SetPixel(i, j, Color.FromArgb(255 - r, 255 - g, 255 - b));
                }
            }
        }
        
        public static Vector<double> GetTargetVector(int vectorSize, double target)
        {
            Vector<double> res = new Vector<double>(vectorSize, 0);
            res[(int)target] = 1;
            return res;
        }

        public static void Shuffle<T>(T[] arr)  
        {  
            Random rng = new Random();  
            int n = arr.Length;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (arr[k], arr[n]) = (arr[n], arr[k]);
            }  
        }

        public static void Grayscale(Bitmap img)
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    double r = pixel.R;
                    double g = pixel.G;
                    double b = pixel.B;

                    double res = 21 * r / 100 + 72 * g / 100 + 7 * b / 100;

                    img.SetPixel(i, j, Color.FromArgb((int)res, (int)res, (int)res));
                }
            }
        }

        public static double GetValidationError(Network net, List<Image> validSet)
        {
            double cost = 0;

            foreach (var x in validSet)
            {
                net.FeedForward(VectorizeImage(x));
                double y = Matrix<double>.ArgMax(net.output).Item1;
                cost += (x.Label - y) * (x.Label - y);
                
                /*//
                net.FeedForward(VectorizeImage(x));

                Matrix<double> sub = GetTargetVector(net.sizeOutput, x.Label) - net.output;
                Matrix<double> sq = sub ^ sub;
                cost += Matrix<double>.Sum(sq);
                */
            }
            
            return cost / validSet.Count;
        }


        public static void Train(List<Image> trainingData, List<Image> validationData, double eta)
        {
            Network net = new Network(784, 50, 3)
            {
                eta = eta
            };

            double minError = Double.MaxValue;

            /*list to store results and then plot error*/
            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();

            for (int epoch = 0; epoch < 50; epoch++)
            {
                for (int i = 0; i < trainingData.Count; i++)
                {
                    net.Train(VectorizeImage(trainingData[i]), GetTargetVector(3, trainingData[i].Label), eta);
                }
                
                
                if (epoch % 10 == 0)
                {
                    dataX.Add(epoch);
                    double validationError = GetValidationError(net, validationData);
                    dataY.Add(validationError);
                    var plt = new ScottPlot.Plot(400, 300);
                    plt.AddScatter(dataX.ToArray(), dataY.ToArray());
                    new ScottPlot.FormsPlotViewer(plt).ShowDialog();

                    /*early exit*/
                    if (validationError < minError)
                    {
                        minError = validationError;
                        net.Save("networkData2/");
                    }
                }
            }
            //net.Save("networkData2/");
        }

        public static void TestNetwork(Network net, List<Image> data)
        {
            foreach (var x in data)
            {
                net.FeedForward(VectorizeImage(x));
                int prediction = Matrix<double>.ArgMax(net.output).Item1;
                int expected = x.Label;

                if (prediction == expected)
                {
                    Console.Write($"expected : {expected}  ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"got : {prediction}");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"expected : {expected}  ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"got : {prediction}");
                    Console.ResetColor();
                }
            }
        }
        
        static void Main(string[] args)
        {
            /*
            List<Image> trainingData = MnistReader.ReadTrainingData(10000).ToList();
            List<Image> validSet = trainingData.GetRange(9000, 1000);
            List<Image> testData = MnistReader.ReadTestData(100).ToList();

            Network net = new Network("NetworkData/");

            foreach (var x in testData)
            {
                net.FeedForward(VectorizeImage(x));
                Console.WriteLine($"Got : {Matrix<double>.ArgMax(net.output)}, Expected : {x.Label}");
            }
            */
            /*
            Network net = new Network(784, 50, 10)
            {
                eta = 0.05
            };

            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();

            for (int epoch = 0; epoch < 50; epoch++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    net.Train(VectorizeImage(trainingData[i]), GetTargetVector(10, trainingData[i].Label), 0.05);
                }
                
                
                if (epoch % 10 == 0)
                {
                    dataX.Add(epoch);
                    dataY.Add(GetValidationError(net, validSet));
                    var plt = new ScottPlot.Plot(400, 300);
                    plt.AddScatter(dataX.ToArray(), dataY.ToArray());
                    new ScottPlot.FormsPlotViewer(plt).ShowDialog();
                }
                
            }
            net.Save("networkData/");
            */
            /*
            Network net = new Network("networkData/");
            for (int i = 0; i < 27; i++)
            {
                Bitmap img = new Bitmap($"images/{i}.bmp");
                Grayscale(img);
            
                ReverseGrayscale(img);
                
                net.FeedForward(VectorizeBMP(img));
            
                Console.WriteLine("-----------" + Matrix<double>.ArgMax(net.output).Item1);
                net.output.Print();
                Console.WriteLine("------------------------");
            }
            
            //img.Save("gray.png");
            */
            /*
            double accuracy = 0;
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine();
                net.FeedForward(VectorizeImage(validSet[i]));
                //testData[i].Print();
                if (Matrix<double>.ArgMax(net.output).Item1 == validSet[i].Label)
                {
                    accuracy += 1;
                }
                //Console.WriteLine(Matrix<double>.ArgMax(net.output));
            }
            
            accuracy = (accuracy / 1000) * 100;
            Console.WriteLine(accuracy);
            
            net.Save("networkData/");
            */
            /*
            double[] dataX = new double[] { 1, 2, 3, 4, 5};
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(dataX, dataY);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
            */

            /*
            Bitmap img = new Bitmap("shapes/circle/drawing(1).png");
            //Grayscale(img);
            img.Save("test.png");
            Image byteImg = ShapeDetector.ConvertToByteImage(img, ShapeDetector.Shape.Circle);

            byteImg.Print();
            */
            
            
            List<Image> trainingData = ShapeDetector.ReadTrainingData();
            List<Image> testData = ShapeDetector.ReadTestData();
            List<Image> validationData = ShapeDetector.ReadValidationData();
            
            //Train(trainingData, validationData, 0.5);
            TestNetwork(new Network("networkData2/"), trainingData);

            /*
            foreach (var img in validationData)
            {
                Console.WriteLine((Shape) img.Label);
                img.Print();
            }
            */
        }
    }
}