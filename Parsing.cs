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

        byte[] pixelBytes = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixelBytes);
        
        var newPixelBytes = new byte[image.Width * image.Height * bpp / 8];


        switch (bpp)
        {
            case 8: // B&W

                for (var i = 0; i < image.Width * image.Height; i ++)
                {
                    newPixelBytes[i] = (byte)((pixelBytes[i * 4] + pixelBytes[i * 4 + 1] + pixelBytes[i * 4 + 2] + pixelBytes[i * 4 + 3]) / 4);
                }

                break;

            case 16: // TODO

                throw new NotImplementedException("16 bits per pixel is still not implemented!");

            case 32: // RBG
                for (var i = 0; i < image.Width * image.Height; i++)
                {
                    newPixelBytes[i] = pixelBytes[i * 4];
                    newPixelBytes[i + 1] = pixelBytes[i * 4 + 1];
                    newPixelBytes[i + 2] = pixelBytes[i * 4 + 2];
                }

                break;

            case 64: // RGBA
                newPixelBytes = pixelBytes;
                break;
        }
    
        if (!Directory.Exists(Path.GetDirectoryName(output))) Directory.CreateDirectory(Path.GetDirectoryName(output)!);

        var outFile = File.OpenWrite(output);

        outFile.WriteByte((byte)bpp);

        byte[] intBytes = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(intBytes, image.Width);
        outFile.Write(intBytes);
        BinaryPrimitives.WriteInt32BigEndian(intBytes, image.Height);
        outFile.Write(intBytes);

        outFile.Write(newPixelBytes);

        outFile.Close();

    }

}
