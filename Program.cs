using System;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

class img2ascii
{
    const string Chars = "@%#*+=-:.";

    static void Main()
    {
        string? input = "";
        while (input != "-1")
        {
            Console.WriteLine("Enter image path (-1 to exit)");
            input = Console.ReadLine();
            if (input == "-1") break;

            Console.WriteLine("Write to file? (y Yes, n No)");
            string? writeInput = Console.ReadLine();
            bool write = writeInput != null && (writeInput.ToLower()[0] == 'y');

            string? writePath = "";
            if (write)
            {
                Console.WriteLine("Path to write:");
                writePath = Console.ReadLine();
                if (!(writePath.EndsWith(".txt") || writePath.EndsWith(".text")))
                {
                    writePath += ".txt";
                }
                if (!string.IsNullOrEmpty(writePath))
                    File.WriteAllText(writePath, "");
            }

            string ext = Path.GetExtension(input).ToLower();
            if (File.Exists(input) && (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp"))
            {
                using var image = Image.Load<Rgba32>(input);
                image.Mutate(x => x.Resize(100, 50));

                for (int i = 0; i < image.Height; i++)
                {
                    var sb = new StringBuilder();
                    for (int j = 0; j < image.Width; j++)
                    {
                        int gray = (image[j, i].R + image[j, i].G + image[j, i].B) / 3;
                        char toAdd = Chars[(gray * (Chars.Length - 1)) / 255];
                        sb.Append(toAdd);
                    }

                    string result = sb.ToString();
                    Console.WriteLine(result);

                    if (write && !string.IsNullOrEmpty(writePath))
                        File.AppendAllText(writePath, result + '\n');
                }
            }
            else
            {
                Console.WriteLine("Invalid path or unsupported format!");
            }
        }
    }
}
