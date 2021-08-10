using System;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;

namespace ASCII_ATOR
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) Environment.Exit(0);
            Image sourceImage = Image.FromFile(args[0]);
            Bitmap sourceBitmap = new Bitmap(sourceImage);
            string imageFileName = Path.GetFileName(args[0]);
            string imageDirectoryName = Path.GetDirectoryName(args[0]);

            Console.WriteLine("Width : ");
            int width = int.Parse(Console.ReadLine());
            Console.WriteLine("Height : ");
            int height = int.Parse(Console.ReadLine());
            Console.WriteLine("Contrast threshold : ");
            int threshold = int.Parse(Console.ReadLine());

            if (width % 2 != 0) { width += 1; }
            if (height % 4 != 0) { height -= height % 4; }
            sourceBitmap = ImageEditor.ResizeImage(sourceImage, width, height);
            sourceBitmap = ImageEditor.SetContrast(sourceBitmap, threshold);
            sourceBitmap = ImageEditor.SetBlackAndWhiteFilter(sourceBitmap);

            File.WriteAllText(imageDirectoryName + "\\" + imageFileName + ".txt", GetASCIIArt(sourceBitmap, width, height));

            Console.WriteLine("Successful");
            Console.ReadKey();
        }

        static string GetASCIIArt(Bitmap sourceBitmap, int bitmapWidth, int bitmapHeight)
        {
            string resultASCII = "";
            for (int height = 0; height < bitmapHeight; height += 4)
            {
                for (int width = 0; width < bitmapWidth; width += 2)
                {
                    BitArray brailleBitArray = new BitArray(8);
                    int brailleIndex = 0;

                    for (int brailleWidth = 0; brailleWidth <= 1; brailleWidth++)
                    {
                        for (int brailleHeight = 0; brailleHeight < 3; brailleHeight++)
                        {
                            brailleBitArray.Set(brailleIndex, ImageEditor.GetBrightness(sourceBitmap.GetPixel(brailleWidth + width, brailleHeight + height)) > 127 ? true : false);
                            brailleIndex++;
                        }
                    }
                    brailleBitArray.Set(6, ImageEditor.GetBrightness(sourceBitmap.GetPixel(0 + width, 3 + height)) > 127 ? true : false);
                    brailleBitArray.Set(7, ImageEditor.GetBrightness(sourceBitmap.GetPixel(1 + width, 3 + height)) > 127 ? true : false);

                    resultASCII += BraillePatternsManager.GetBrailleChar(brailleBitArray);
                }
                resultASCII += Environment.NewLine;
            }

            return resultASCII;
        }
    }
}
