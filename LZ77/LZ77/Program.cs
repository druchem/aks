using System;
using System.IO;

namespace LZ77
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var files = new[] {"czech.txt", "english.txt", "french.txt", "german.txt", "hungarian.txt"};
			var parameters = new(int WindowSize, int MaxMatch)[]  {(4096, 16), (16384, 32), (32768, 64)};

			Console.WriteLine("File\t\tTriplets\tFileSize\tWindowSize\tMax.Match\tEnc.Size\tbps");
			foreach (string file in files)
			{
				Console.WriteLine("-----------------------------------------------------------------------------------------------------");

				var input = File.ReadAllBytes(file);
				foreach (var parameter in parameters)
				{
					var lz = new Lz77(parameter.WindowSize, parameter.MaxMatch);
					var triplets = lz.Encode(input);

					int encSize = (int)(Math.Log(parameter.WindowSize, 2) + Math.Log(parameter.MaxMatch, 2) + 8) * triplets.Count;
					float bps = 1f * encSize / input.Length;

					Console.WriteLine($"{file}\t{triplets.Count}\t\t{input.Length}\t\t{parameter.WindowSize}\t\t{parameter.MaxMatch}\t\t{encSize}\t\t{bps:F3}");
				}
			}

			Console.ReadLine();
		}
	}
}