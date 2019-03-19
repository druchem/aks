using System;
using System.Collections.Generic;

namespace LZ77
{
	internal struct Triplet
	{
		public int MatchOffset { get; set; }
		public int MatchLength { get; set; }
		public byte NonMatchedByte { get; set; }

		public override string ToString() => $"({MatchOffset}, {MatchLength}, {NonMatchedByte})";
	}

	internal class Lz77
	{
		private readonly int _windowSize;
		private readonly int _maxMatchSize;
		private readonly int _maxSearchBufferSize;

		public Lz77(int windowSize, int maxMatchSize)
		{
			_windowSize = windowSize;
			_maxMatchSize = maxMatchSize;
			_maxSearchBufferSize = windowSize - maxMatchSize;
		}

		public List<Triplet> Encode(byte[] input)
		{
			Span<byte> buffer = input;

			var triplets = new List<Triplet>();
			int lookAheadIndex = 0;
			int searchOffset = 0;

			while (lookAheadIndex + _maxMatchSize < buffer.Length)
			{
				var lookAheadBuffer = buffer.Slice(lookAheadIndex, _maxMatchSize);

				int searchBufferSize = lookAheadIndex < _maxSearchBufferSize ? lookAheadIndex : _maxSearchBufferSize;
				var searchBuffer = buffer.Slice(searchOffset, searchBufferSize);

				var triplet = GetTriplet(searchBuffer, lookAheadBuffer);
				triplets.Add(triplet);

				lookAheadIndex += triplet.MatchLength + 1;

				if (lookAheadIndex > _maxSearchBufferSize)
					searchOffset = lookAheadIndex - _maxSearchBufferSize;
			}

			return triplets;
		}

		private Triplet GetTriplet(Span<byte> searchBuffer, Span<byte> lookAheadBuffer)
		{
			var triplet = new Triplet
			{
				MatchLength = 0,
				MatchOffset = 0,
				NonMatchedByte = lookAheadBuffer[0]
			};

			for (int matchLength = 1; matchLength < lookAheadBuffer.Length; matchLength++)
			{
				var searchPattern = lookAheadBuffer.Slice(0, matchLength);

				int matchIndex = searchBuffer.LastIndexOf(searchPattern);
				if (matchIndex == -1)
					break;

				triplet = new Triplet
				{
					MatchLength = matchLength,
					MatchOffset = searchBuffer.Length - matchIndex,
					NonMatchedByte = lookAheadBuffer[matchLength] // can crash
				};
			}

			return triplet;
		}
	}
}