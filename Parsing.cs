using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BitmapConverter;

public static class Parsing
{

    public static void Parse(string input, string output, int bpp)
    {

        using Image<Rgba32> image = Image.Load<Rgba32>(input);

        Rgba32[] pixelBytes = new Rgba32[image.Width * image.Height];
        image.CopyPixelDataTo(pixelBytes);
        
        var newPixelBytes = new byte[image.Width * image.Height * (bpp / 8)];


        switch (bpp)
        {
            case 8: // B&W

                for (var i = 0; i < image.Width * image.Height; i ++)
                {
                    newPixelBytes[i] = (byte)((pixelBytes[i].R + pixelBytes[i].G + pixelBytes[i].B + pixelBytes[i].A) / 4);
                }

                break;

            case 16: // TODO

                throw new NotImplementedException("16 bits per pixel is still not implemented!");

            case 24: // RBG
                for (var i = 0; i < image.Width * image.Height; i++)
                {
                    newPixelBytes[i + 0] = pixelBytes[i].R;
                    newPixelBytes[i + 1] = pixelBytes[i].G;
                    newPixelBytes[i + 2] = pixelBytes[i].B;
                }

                break;

            case 32: // RGBA
                for (var i = 0; i < image.Width * image.Height; i++)
                {
                    newPixelBytes[i * 4 + 0] = pixelBytes[i].R;
                    newPixelBytes[i * 4 + 1] = pixelBytes[i].G;
                    newPixelBytes[i * 4 + 2] = pixelBytes[i].B;
                    newPixelBytes[i * 4 + 3] = pixelBytes[i].A;
                }
                break;
        }
    
        if (!string.IsNullOrEmpty(Path.GetDirectoryName(output)) && !Directory.Exists(Path.GetDirectoryName(output)))
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        var outFile = File.Create(output);

        // Header
        byte[] intBytes = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(intBytes, bpp);
        outFile.Write(intBytes);
        BinaryPrimitives.WriteInt32BigEndian(intBytes, image.Width);
        outFile.Write(intBytes);
        BinaryPrimitives.WriteInt32BigEndian(intBytes, image.Height);
        outFile.Write(intBytes);

        // Data
        outFile.Position = 16; // Skip header
        outFile.Write(newPixelBytes);

        outFile.Close();

    }

}
