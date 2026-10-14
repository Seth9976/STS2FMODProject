using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace MegaCrit.Sts2.Core.Multiplayer
{
	// Token: 0x02000428 RID: 1064
	[NullableContext(1)]
	[Nullable(0)]
	public static class MultiplayerDebugUtil
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0020FAC0 File Offset: 0x0020DCC0
		private static IEnumerable<BigInteger> Chunk(IReadOnlyList<byte> source, int size)
		{
			return from e in source.Select<byte, byte[]>((byte _, int index) => source.Skip<byte>(size * index).Take<byte>(size).ToArray<byte>()).TakeWhile<byte[]>((byte[] bucket) => bucket.Length != 0)
				select new BigInteger(e);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0020FB40 File Offset: 0x0020DD40
		private static byte ReplaceControlCharacterWithDot(byte character)
		{
			if (character >= 31 && character < 127)
			{
				return character;
			}
			return 46;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0020FB64 File Offset: 0x0020DD64
		private static byte[] ReplaceControlCharactersWithDots(byte[] characters)
		{
			Func<byte, byte> func;
			if ((func = MultiplayerDebugUtil.<>O.<0>__ReplaceControlCharacterWithDot) == null)
			{
				func = (MultiplayerDebugUtil.<>O.<0>__ReplaceControlCharacterWithDot = new Func<byte, byte>(MultiplayerDebugUtil.ReplaceControlCharacterWithDot));
			}
			return characters.Select<byte, byte>(func).ToArray<byte>();
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0020FB8C File Offset: 0x0020DD8C
		[NullableContext(0)]
		[return: Nullable(1)]
		public static string FormatAsHex(ReadOnlySpan<byte> data, int lineWidth = 16, int byteWidth = 1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Func<BigInteger, string> <>9__0;
			byte[] array;
			for (int i = 0; i < data.Length; i += array.Length)
			{
				array = data.Slice(i, Math.Min(lineWidth * byteWidth, data.Length - i)).ToArray();
				string text = " ";
				IEnumerable<BigInteger> enumerable = MultiplayerDebugUtil.Chunk(array, byteWidth);
				Func<BigInteger, string> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (BigInteger v) => v.ToString("X" + (byteWidth * 2).ToString(), CultureInfo.InvariantCulture));
				}
				string text2 = string.Join(text, enumerable.Select<BigInteger, string>(func));
				text2 += new string(' ', lineWidth * (byteWidth * 2 + 1) - 1 - text2.Length);
				string @string = Encoding.ASCII.GetString(MultiplayerDebugUtil.ReplaceControlCharactersWithDots(array));
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 3, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted<int>(i, "X4");
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted(text2);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted(@string);
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder3.Append(ref appendInterpolatedStringHandler);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0020FCC8 File Offset: 0x0020DEC8
		public static byte[] StringToByteArray(string hex)
		{
			if (hex.Length % 2 == 1)
			{
				throw new Exception("The binary key cannot have an odd number of digits");
			}
			byte[] array = new byte[hex.Length >> 1];
			for (int i = 0; i < hex.Length >> 1; i++)
			{
				array[i] = (byte)((MultiplayerDebugUtil.GetHexVal(hex[i << 1]) << 4) + MultiplayerDebugUtil.GetHexVal(hex[(i << 1) + 1]));
			}
			return array;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0020FD34 File Offset: 0x0020DF34
		private static int GetHexVal(char hex)
		{
			return (int)(hex - ((hex < ':') ? '0' : ((hex < 'a') ? '7' : 'W')));
		}

		// Token: 0x0200198D RID: 6541
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040064ED RID: 25837
			[Nullable(0)]
			public static Func<byte, byte> <0>__ReplaceControlCharacterWithDot;
		}
	}
}
