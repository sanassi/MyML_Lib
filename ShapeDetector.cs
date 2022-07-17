using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MyML_Lib
{
    public class ShapeDetector
    {
        public enum Shape
        {
            Circle = 0,
            Square = 1,
            Triangle = 2
        }

        /*img : grayscale image*/
        /*fok*/
        public static Image ConvertToByteImage(Bitmap img, Shape shapeLabel)
        {
            Image byteImg = new Image()
            {
                height = 28,
                width = 28,
                Label = (byte) shapeLabel,
            };

            byte[,] arr = new byte[img.Height, img.Width];
            
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    arr[i, j] = Convert.ToByte((img.GetPixel(j, i).R));
                }
            }

            byteImg.Data = arr;
            
            return byteImg;
        }

        public static List<Image> ReadTrainingData()
        {
            List<Image> trainingData = new List<Image>();
            int size = 100;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    Bitmap bmp = new Bitmap($"shapes/train/{(Shape) i}/drawing({j}).png");
                    Program.ReverseGrayscale(bmp);
                    trainingData.Add(ConvertToByteImage(bmp, (Shape) i));
                }
            }

            /*horrible and extremely ineficient but works for now*/
            /*shuffle training data*/
            Image[] imgAsArr = trainingData.ToArray();
            Program.Shuffle(imgAsArr);
            return imgAsArr.ToList();
        }
        
        public static List<Image> ReadTestData()
        {
            List<Image> testData = new List<Image>();
            int size = 5;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    Bitmap bmp = new Bitmap($"shapes/test/{(Shape) i}/{j}.png");
                    Program.ReverseGrayscale(bmp);
                    testData.Add(ConvertToByteImage(bmp, (Shape) i));
                }
            }

            /*horrible and extremely ineficient but works for now*/
            /*shuffle training data*/
            Image[] imgAsArr = testData.ToArray();
            Program.Shuffle(imgAsArr);
            return imgAsArr.ToList();
        }
        
        public static List<Image> ReadValidationData()
        {
            List<Image> validationData = new List<Image>();
            int size = 5;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    Bitmap bmp = new Bitmap($"shapes/validation/{(Shape) i}/{j}.png");
                    Program.ReverseGrayscale(bmp);
                    validationData.Add(ConvertToByteImage(bmp, (Shape) i));
                }
            }

            /*horrible and extremely ineficient but works for now*/
            /*shuffle training data*/
            Image[] imgAsArr = validationData.ToArray();
            Program.Shuffle(imgAsArr);
            return imgAsArr.ToList();
        }
    }
}