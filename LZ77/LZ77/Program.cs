using System;
using System.IO;
using System.Linq;

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

					int length = triplets.Sum(triplet => triplet.MatchLength);

					Console.WriteLine($"{file}\t{triplets.Count}\t\t{input.Length}\t\t{parameter.WindowSize}\t\t{parameter.MaxMatch}\t\t{length}\t\t?????");
				}
			}

			Console.ReadLine();
		}
	}
}