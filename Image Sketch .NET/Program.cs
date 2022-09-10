using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Sketch.NET
{
    internal class Program
    {
        static string imagePath, outputPath;
        static float fontSize;
        static char startChar,endChar;

        static void Main(string[] args)
        {
            Console.Title = "Convert Sketch to Text";
            try
            {
                
                getInputs();

                //create array using characters
                List<char> charArray = new List<char>();
                for (int i = endChar; i >= startChar; i--)
                {
                    charArray.Add((char)i);
                }
                //get character length
                int charLength = charArray.Count;


                //create image from path
                Image img = Image.FromFile(imagePath);

                //resize image using font size
                Bitmap resizedImg = resizeImage(fontSize,img); 
   
                //convert image to text
                string textImg = convertImage(resizedImg,charLength,charArray);

                //process info
                Console.Title = "Processing...";
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 20);
                
                //write to output path
                File.WriteAllText(outputPath, textImg);

                //dev info
                Console.Title = "Completed";
                Console.WriteLine("\n\nCompleted");
                Console.WriteLine("\nCreated By Venushka Dhambarage\nvdhambarage@gmail.com\ngithub.com/VenushkaD");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();

        }

        private static void getInputs()
        {
            Console.WriteLine("Enter Input Image Path:" + " \n" + @"ex: C:\Users\DELL\Desktop\sketch.png");
            imagePath = Console.ReadLine();
            Console.WriteLine("\nEnter Output File Path:" + " \n" + @"ex: C:\Users\DELL\Desktop\sketch.txt");
            outputPath = Console.ReadLine();
            Console.WriteLine("\nEnter Font Size: \nnote: smaller font size will increase quality but takes longer to process\nex:7");
            fontSize = float.Parse(Console.ReadLine());
            /*char startChar, endChar;*/
            Console.WriteLine("\nEnter Start Char:\nex:!");
            startChar = Console.ReadLine()[0];
            Console.WriteLine("\nEnter End Char:\nex:z");
            endChar = Console.ReadLine()[0];
        }
        private static Bitmap resizeImage(float fontSize, Image img)
        {
            Bitmap resizedImg = new Bitmap((int)(img.Width / fontSize), (int)(img.Height / fontSize));

            double ratioX = (double)resizedImg.Width / (double)img.Width;
            double ratioY = (double)resizedImg.Height / (double)img.Height;
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            int newHeight = Convert.ToInt32(img.Height * ratio);
            int newWidth = Convert.ToInt32(img.Width * ratio);

            using (Graphics g = Graphics.FromImage(resizedImg))
            {
                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }

            return resizedImg;
        }
        private static string convertImage(Bitmap resizedImg,int charLength, List<char> charArray)
        {
            string textImg = "";
            for (int i = 0; i < resizedImg.Height; i++)
            {
                for (int j = 0; j < resizedImg.Width; j++)
                {
                    //gets the color for each pixel
                    Color color = resizedImg.GetPixel(j, i);
                    int red = color.R;
                    int green = color.G;
                    int blue = color.B;

                    int totalColor = (red + green + blue);
                    int avgBrightness;

                    if (totalColor == 0)
                    { avgBrightness = 1; }
                    else
                    {
                        avgBrightness = (red + green + blue) / 3;
                    }

                    //add a blank space to white colours
                    if (avgBrightness > 210)
                    {
                        textImg += "  ";
                    }
                    //add a character to dark colours
                    else
                    {
                        int charIndex = (avgBrightness * charLength / 230);
                        if (charIndex == 0)
                        {
                            charIndex = 1;
                        }
                        textImg += charArray[charIndex - 1] + "" + charArray[charIndex - 1];


                    }
                    Console.Write("\u2551");

                }
                //leave a line break after for loop
                textImg = textImg + "\n";

                Console.Write(i + "/ " + (resizedImg.Height - 1));
                Console.SetCursorPosition(0, 20);

            }
            return textImg;
        }
            
        }
    }
