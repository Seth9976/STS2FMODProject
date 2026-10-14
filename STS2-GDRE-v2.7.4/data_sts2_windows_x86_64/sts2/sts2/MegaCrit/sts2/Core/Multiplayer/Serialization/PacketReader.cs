using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x0200044E RID: 1102
	[NullableContext(1)]
	[Nullable(0)]
	public class PacketReader
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x00214955 File Offset: 0x00212B55
		// (set) Token: 0x0600450C RID: 17676 RVA: 0x0021495D File Offset: 0x00212B5D
		public int BitPosition { get; private set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600450D RID: 17677 RVA: 0x00214966 File Offset: 0x00212B66
		// (set) Token: 0x0600450E RID: 17678 RVA: 0x0021496E File Offset: 0x00212B6E
		public byte[] Buffer { get; private set; }

		// Token: 0x0600450F RID: 17679 RVA: 0x00214977 File Offset: 0x00212B77
		public void Reset(byte[] buffer)
		{
			this.BitPosition = 0;
			this.Buffer = buffer;
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00214987 File Offset: 0x00212B87
		public bool ReadBool()
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 1);
			this.BitPosition++;
			return this._tempBuffer[0] > 0;
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x002149C5 File Offset: 0x00212BC5
		public byte ReadByte(int bits = 8)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return this._tempBuffer[0];
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00214A00 File Offset: 0x00212C00
		public void ReadBytes(byte[] destinationBuffer, int byteCount)
		{
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, destinationBuffer, byteCount * 8);
			this.BitPosition += byteCount * 8;
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00214A28 File Offset: 0x00212C28
		public short ReadShort(int bits = 16)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadInt16LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x00214A80 File Offset: 0x00212C80
		public ushort ReadUShort(int bits = 16)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadUInt16LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00214AD8 File Offset: 0x00212CD8
		[NullableContext(0)]
		public T ReadEnum<T>() where T : struct, Enum
		{
			if (!typeof(int).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Trying to write enum type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(" that is not assignable to int!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num = Mathf.CeilToInt(Math.Log2((double)MaxEnumValueCache.Get<T>()) + 1.0);
			int num2 = this.ReadInt(num);
			return (T)((object)Enum.ToObject(typeof(T), num2));
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x00214B80 File Offset: 0x00212D80
		public int ReadInt(int bits = 32)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadInt32LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00214BD8 File Offset: 0x00212DD8
		public uint ReadUInt(int bits = 32)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadUInt32LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00214C30 File Offset: 0x00212E30
		public float ReadFloat(QuantizeParams? quantizeParams = null)
		{
			Array.Clear(this._tempBuffer);
			if (quantizeParams != null)
			{
				uint num = this.ReadUInt(quantizeParams.Value.bits);
				return PacketReader.Unquantize(num, quantizeParams.Value.min, quantizeParams.Value.max, quantizeParams.Value.bits);
			}
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 32);
			this.BitPosition += 32;
			return BinaryPrimitives.ReadSingleLittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x00214CCC File Offset: 0x00212ECC
		public long ReadLong(int bits = 64)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadInt64LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x00214D24 File Offset: 0x00212F24
		public ulong ReadULong(int bits = 64)
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
			this.BitPosition += bits;
			return BinaryPrimitives.ReadUInt64LittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00214D7C File Offset: 0x00212F7C
		public double ReadDouble()
		{
			Array.Clear(this._tempBuffer);
			BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 64);
			this.BitPosition += 64;
			return BinaryPrimitives.ReadDoubleLittleEndian(this._tempBuffer.AsSpan<byte>());
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00214DD4 File Offset: 0x00212FD4
		public Vector2 ReadVector2(QuantizeParams? quantizeParamsX = null, QuantizeParams? quantizeParamsY = null)
		{
			float num = this.ReadFloat(quantizeParamsX);
			float num2 = this.ReadFloat(quantizeParamsY);
			return new Vector2(num, num2);
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00214DF8 File Offset: 0x00212FF8
		public List<T> ReadList<[Nullable(0)] T>(int lengthBits = 32) where T : IPacketSerializable, new()
		{
			List<T> list = new List<T>();
			int num = this.ReadInt(lengthBits);
			for (int i = 0; i < num; i++)
			{
				list.Add(this.Read<T>());
			}
			return list;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00214E2C File Offset: 0x0021302C
		public string ReadString()
		{
			int num = this.ReadInt(32);
			if (this._stringBuffer.Length < num)
			{
				int num2 = (int)Math.Pow(2.0, Math.Ceiling(Math.Log2((double)num)));
				this._stringBuffer = new byte[num2];
			}
			this.ReadBytes(this._stringBuffer, num);
			return Encoding.UTF8.GetString(this._stringBuffer, 0, num);
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x00214E94 File Offset: 0x00213094
		public T Read<[Nullable(0)] T>() where T : IPacketSerializable, new()
		{
			T t = new T();
			t.Deserialize(this);
			return t;
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00214EB6 File Offset: 0x002130B6
		public static float Unquantize(uint value, float min, float max, int bitLength)
		{
			return (float)(value / Math.Pow(2.0, (double)bitLength) * (double)(max - min) + (double)min);
		}

		// Token: 0x04001AD8 RID: 6872
		private readonly byte[] _tempBuffer = new byte[16];

		// Token: 0x04001AD9 RID: 6873
		private byte[] _stringBuffer = new byte[64];
	}
}
