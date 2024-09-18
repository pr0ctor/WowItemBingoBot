using DiscordBingoBot;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using System.Runtime.InteropServices;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

DiscordBingoBot.Helpers.Helpers.BuildEnvironmentVariables();

using var image1 = new FileStream(@"D:\Projects\Visual Studio\DiscordBingo\Discord Bingo Bot\BingoTests\images\yuukii1.png", FileMode.Open);
using var blockedImage = new FileStream(@"D:\Projects\Visual Studio\DiscordBingo\Discord Bingo Bot\BingoTests\images\smapleblockedtext.png", FileMode.Open);
using var blockedImageSmall = new FileStream(@"D:\Projects\Visual Studio\DiscordBingo\Discord Bingo Bot\BingoTests\images\smapleblockedtextsmall.png", FileMode.Open);

using var intstream = new MemoryStream();
image1.CopyTo(intstream);
byte[] bytes = intstream.ToArray();

using var blockstream = new MemoryStream();
blockedImage.CopyTo(blockstream);
byte[] blockbytes = blockstream.ToArray();

using var blockstreamsmall = new MemoryStream();
blockedImageSmall.CopyTo(blockstreamsmall);
byte[] blockbytessmall = blockstreamsmall.ToArray();

var ratio = 300 / 72;
using var picture = Image.Load(blockbytessmall);

picture.Mutate(o => o.Resize(picture.Width * ratio, picture.Height * ratio, KnownResamplers.Lanczos8));

using var newstream = new MemoryStream();
picture.SaveAsPng(newstream);
//picture.Mutate(o => o.Grayscale(GrayscaleMode.Bt709));


picture.SaveAsJpeg(@"D:\Projects\Visual Studio\DiscordBingo\Discord Bingo Bot\BingoTests\images\blocked-gray.png");


using var imageGray = new FileStream(@"D:\Projects\Visual Studio\DiscordBingo\Discord Bingo Bot\BingoTests\images\blocked-gray.png", FileMode.Open);

using var graystream = new MemoryStream();
imageGray.CopyTo(graystream);
byte[] bytesGray = graystream.ToArray();

var x = "";

DiscordBingoBot.OCR.OcrWorker.ReadTextFromImage(bytesGray);