using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
namespace MyML_Lib
{
    /*code from stack overflow https://stackoverflow.com/questions/49407772/reading-mnist-database*/
    public static class MnistReader
    {
        private const string TrainImages = "mnist/train-images.idx3-ubyte";
        private const string TrainLabels = "mnist/train-labels.idx1-ubyte";
        private const string TestImages = "mnist/t10k-images.idx3-ubyte";
        private const string TestLabels = "mnist/t10k-labels.idx1-ubyte";

        public static IEnumerable<Image> ReadTrainingData(int size)
        {
            int i = 0;
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                if (i == size)
                    break;
                i++;
                yield return item;
            }
        }

        public static IEnumerable<Image> ReadTestData(int size)
        {
            int i = 0;
            foreach (var item in Read(TestImages, TestLabels))
            {
                if (i == size)
                    break;
                i++;
                yield return item;
            }
        }

        private static IEnumerable<Image> Read(string imagesPath, string labelsPath)
        {
            BinaryReader labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            BinaryReader images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

            int magicNumber = images.ReadBigInt32();
            int numberOfImages = images.ReadBigInt32();
            int width = images.ReadBigInt32();
            int height = images.ReadBigInt32();

            int magicLabel = labels.ReadBigInt32();
            int numberOfLabels = labels.ReadBigInt32();

            for (int i = 0; i < numberOfImages; i++)
            {
                var bytes = images.ReadBytes(width * height);
                var arr = new byte[height, width];

                arr.ForEach((j,k) => arr[j, k] = bytes[j * height + k]);

                yield return new Image()
                {
                    Data = arr,
                    Label = labels.ReadByte(),
                    height = height,
                    width = width
                };
            }
        }
    }

    public static class Extensions
    {
        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (int w = 0; w < source.GetLength(0); w++)
            {
                for (int h = 0; h < source.GetLength(1); h++)
                {
                    action(w, h);
                }
            }
        }
    }
    
}