using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Drawing;

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
            }
            
            return cost / validSet.Count;
        }

        static void Main(string[] args)
        {
            List<Image> trainingData = MnistReader.ReadTrainingData(2000).ToList();
            List<Image> validSet = trainingData.GetRange(1000, 1000);
            List<Image> testData = MnistReader.ReadTestData(10000).ToList();
            
            /*
            Network net = new Network(784, 50, 10);
            
            for (int epoch = 0; epoch < 50; epoch++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    net.eta = 0.4;
                    net.Train(VectorizeImage(trainingData[i]), GetTargetVector(10, trainingData[i].Label), 0.4);
                }
                Console.WriteLine(GetValidationError(net, validSet));
            }
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
        }
    }
}