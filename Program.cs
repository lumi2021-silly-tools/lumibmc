using BitmapConverter;

int inputArg = -1;
string input = null!;

int outputArg = -1;
string output = null!;

int bppArg = -1;
int bpp = 32;

try
{

    if (args.Length == 0) throw new Exception("No arguments provided");

    for (var i = 0; i < args.Length; i++)
    {
        if (args[i] == "-o")
        {
            if (args.Length <= i + 1) throw new Exception($"Argument expected after -o!");
            if (!string.IsNullOrEmpty(output)) throw new Exception($"Invalid output in argument {i + 1}! (output already being received in argument {inputArg})");

            outputArg = i + 1;
            output = args[i + 1];
            i++;
        }
        else if (args[i] == "-bpp")
        {
            if (args.Length <= i + 1) throw new Exception($"Argument expected after -o!");
            if (bppArg != -1) throw new Exception($"Invalid output in argument {i + 1}! (bpp already being received in argument {inputArg})");

            bppArg = i + 1;
            if (!int.TryParse(args[i + 1], out bpp)) throw new Exception($"argument {i + 1} is not a valid integer!");

            i++;
        }

        else
        {
            if (!string.IsNullOrEmpty(input)) throw new Exception($"Invalid input in argument {i}! (input already being received in argument {inputArg})");

            inputArg = i;
            input = args[i];
        }
    }

    // Checking input and output
    if (string.IsNullOrWhiteSpace(input)) throw new Exception("No input file provided!");
    if (!File.Exists(input)) throw new Exception("Input file does not exist!");
    if (string.IsNullOrWhiteSpace(output)) throw new Exception("No output file provided!");

    // checking if bpp is valid
    if (bpp is not 8 and not 16 and not 24 and not 32) throw new Exception("Invalid bits per pixel! (try 8, 16, 24 or 32)");

    Parsing.Parse(input, output, bpp);
}
catch (Exception e)
{
    Console.WriteLine($"!!! Exception !!!\n{e.Message}");
    Console.WriteLine();
    PrintHelpAndExit();
}

static void PrintHelpAndExit()
{
    Console.WriteLine("Usage: lumibmc <input> -o <output> [-bpp <bits per pixel>]");
    Console.WriteLine();
    Console.WriteLine("-bpp:".PadRight(10) + "Bits per pixel");
    Console.WriteLine("-o:".PadRight(10) + "Output file");
    Console.WriteLine();
    Console.WriteLine("Use \"lumibmc -h bpp\" to see all the pixel formats");
    Console.WriteLine("Use \"lumibmc -h reading\" to learn how to read and parse the bitmaps");
    Console.WriteLine("Use \"lumibmc -h\" to see this message");
    Environment.Exit(1);
}
