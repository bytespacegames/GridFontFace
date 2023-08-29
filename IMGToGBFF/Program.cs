using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace IMGToGBFF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your correctly formatted font face PNG");
            string loc = Console.ReadLine();
            string export = Path.GetDirectoryName(loc) + "\\" + Path.GetFileNameWithoutExtension(loc) + ".gff";
            Console.WriteLine(export);
            Bitmap bitmap = new Bitmap(loc);
            int glyphWidth = 0;
            Console.WriteLine("Please enter your Glyph width. This includes spacing, if you used it when creating your font.");
            glyphWidth = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter your character spacing (in pixels). 1 would be a 1 pixel gap, 0 would be no space between characters");
            int charSpacing = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter your vertical line spacing.");
            int verticalSpacing = int.Parse(Console.ReadLine());

            List<byte> fontFileBytes = new();

            if (bitmap.Width / glyphWidth != 256)
            {
                Console.WriteLine("Your Font Face must have exactly 256 glyph spaces.");
                return;
            }

            fontFileBytes.Add((byte)glyphWidth);
            fontFileBytes.Add((byte)bitmap.Height);
            fontFileBytes.Add((byte)charSpacing);
            fontFileBytes.Add((byte)verticalSpacing);

            //add glyph widths
            for (int glyph = 0; glyph < 256; glyph++)
            {
                int furthestRight = 0;
                for (int x = 0; x < glyphWidth; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (bitmap.GetPixel(x + glyphWidth * glyph,y).R == 0)
                        {
                            furthestRight = x;
                        }
                    }
                }
                fontFileBytes.Add((byte)furthestRight);
            }

            for (int glyph = 0; glyph < 256; glyph++)
            {
                List<byte> glyphLocations = new();
                for (int x = 0; x < glyphWidth; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (bitmap.GetPixel(x + glyphWidth * glyph, y).R == 0)
                        {
                            glyphLocations.Add((byte)x);
                            glyphLocations.Add((byte)y);
                        }
                    }
                }
                fontFileBytes.Add((byte)(glyphLocations.Count / 2));

                foreach (byte b in glyphLocations)
                {
                    fontFileBytes.Add(b);
                }
            }
            File.WriteAllBytes(export, fontFileBytes.ToArray());
        }
    }
}