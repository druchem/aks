namespace LZ77
{
	internal struct Triplet
	{
		public ushort MatchOffset { get; set; }
		public byte MatchLength { get; set; }
		public byte NonMatchedByte { get; set; }

		public override string ToString() => $"({MatchOffset}, {MatchLength}, {NonMatchedByte})";
	}
}