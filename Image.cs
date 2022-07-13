using System;

namespace MyML_Lib
{
    public class Image
    {
        public byte Label { get; set; }
        public byte[,] Data { get; set; }

        public int height { get; set; }
        public int width { get; set; }

        public void Print()
        {
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (Convert.ToInt32(Data[i, j]) == 0)
                        Console.Write('0');
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(1);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
    }
}