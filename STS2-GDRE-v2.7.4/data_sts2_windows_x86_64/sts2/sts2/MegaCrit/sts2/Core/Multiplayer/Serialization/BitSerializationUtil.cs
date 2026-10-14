using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000446 RID: 1094
	[NullableContext(1)]
	[Nullable(0)]
	internal static class BitSerializationUtil
	{
		// Token: 0x060044D2 RID: 17618 RVA: 0x002134DE File Offset: 0x002116DE
		public static byte GetByteMask(int bits, int startBit)
		{
			if (bits > 8 || startBit + bits > 8)
			{
				throw new InvalidOperationException();
			}
			return (byte)((1 << bits) - 1 << startBit);
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x00213500 File Offset: 0x00211700
		public static byte GetBitsAtPosition(byte[] bytes, int bitPosition, int bitsToObtain)
		{
			int num = bitPosition / 8;
			int num2 = bitPosition % 8;
			if (bitsToObtain > 8)
			{
				throw new InvalidOperationException();
			}
			if (num2 == 0)
			{
				return bytes[num] & BitSerializationUtil.GetByteMask(bitsToObtain, 0);
			}
			int num3 = 8 - num2;
			if (bitsToObtain <= num3)
			{
				return (byte)((bytes[num] >> num2) & (int)BitSerializationUtil.GetByteMask(bitsToObtain, 0));
			}
			byte b = (byte)(bytes[num] >> num2);
			byte byteMask = BitSerializationUtil.GetByteMask(num3, 0);
			byte b2 = (byte)(bytes[num + 1] << num3);
			byte byteMask2 = BitSerializationUtil.GetByteMask(bitsToObtain - num3, num3);
			byte b3 = (b & byteMask) | (b2 & byteMask2);
			return b3 & BitSerializationUtil.GetByteMask(bitsToObtain, 0);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x0021358C File Offset: 0x0021178C
		public static void WriteBytes(byte[] originBuffer, byte[] destinationBuffer, int destinationBitPosition, int totalBitsToWrite)
		{
			int num3;
			for (int i = 0; i < totalBitsToWrite; i += num3)
			{
				int num = (destinationBitPosition + i) / 8;
				int num2 = (destinationBitPosition + i) % 8;
				num3 = Mathf.Min(totalBitsToWrite - i, 8 - num2);
				byte bitsAtPosition = BitSerializationUtil.GetBitsAtPosition(originBuffer, i, num3);
				byte b = destinationBuffer[num];
				byte byteMask = BitSerializationUtil.GetByteMask(num2, 0);
				byte byteMask2 = BitSerializationUtil.GetByteMask(num3, num2);
				byte b2 = (byte)(bitsAtPosition << num2);
				byte b3 = (b & byteMask) | (b2 & byteMask2);
				destinationBuffer[num] = b3;
			}
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x00213604 File Offset: 0x00211804
		public static void ReadBits(byte[] originBuffer, int originBitPosition, byte[] destinationBuffer, int totalBitsToRead)
		{
			int num3;
			for (int i = 0; i < totalBitsToRead; i += num3)
			{
				int num = i / 8;
				int num2 = i % 8;
				num3 = Mathf.Min(totalBitsToRead - i, 8 - num2);
				byte bitsAtPosition = BitSerializationUtil.GetBitsAtPosition(originBuffer, originBitPosition + i, num3);
				byte b = destinationBuffer[num];
				byte byteMask = BitSerializationUtil.GetByteMask(num2, 0);
				byte byteMask2 = BitSerializationUtil.GetByteMask(num3, num2);
				byte b2 = (byte)(bitsAtPosition << num2);
				byte b3 = (b & byteMask) | (b2 & byteMask2);
				destinationBuffer[num] = b3;
			}
		}
	}
}
