using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Multiplayer.Serialization
{
	// Token: 0x02000450 RID: 1104
	[NullableContext(1)]
	[Nullable(0)]
	public class PacketWriter
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x002150CD File Offset: 0x002132CD
		// (set) Token: 0x0600452B RID: 17707 RVA: 0x002150D5 File Offset: 0x002132D5
		public int BitPosition { get; private set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600452C RID: 17708 RVA: 0x002150DE File Offset: 0x002132DE
		public int BytePosition
		{
			get
			{
				return Mathf.CeilToInt((float)this.BitPosition / 8f);
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600452D RID: 17709 RVA: 0x002150F2 File Offset: 0x002132F2
		// (set) Token: 0x0600452E RID: 17710 RVA: 0x002150FA File Offset: 0x002132FA
		public byte[] Buffer { get; private set; } = new byte[1024];

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600452F RID: 17711 RVA: 0x00215103 File Offset: 0x00213303
		// (set) Token: 0x06004530 RID: 17712 RVA: 0x0021510B File Offset: 0x0021330B
		public bool WarnOnGrow { get; set; } = true;

		// Token: 0x06004531 RID: 17713 RVA: 0x00215114 File Offset: 0x00213314
		public void Reset()
		{
			this.BitPosition = 0;
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x0021511D File Offset: 0x0021331D
		public void WriteBool(bool val)
		{
			this._tempBuffer[0] = ((val > false) ? 1 : 0);
			this.ResizeBufferIfNecessary(1);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 1);
			this.BitPosition++;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x00215159 File Offset: 0x00213359
		public void WriteByte(byte val, int bits = 8)
		{
			this._tempBuffer[0] = val;
			this.ResizeBufferIfNecessary(bits);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00215191 File Offset: 0x00213391
		public void WriteBytes(byte[] bytes, int byteCount)
		{
			this.ResizeBufferIfNecessary(byteCount * 8);
			BitSerializationUtil.WriteBytes(bytes, this.Buffer, this.BitPosition, byteCount * 8);
			this.BitPosition += byteCount * 8;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x002151C1 File Offset: 0x002133C1
		public void WriteShort(short val, int bits = 16)
		{
			this.ResizeBufferIfNecessary(bits);
			BinaryPrimitives.WriteInt16LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00215201 File Offset: 0x00213401
		public void WriteUShort(ushort val, int bits = 16)
		{
			this.ResizeBufferIfNecessary(bits);
			BinaryPrimitives.WriteUInt16LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x00215244 File Offset: 0x00213444
		[NullableContext(0)]
		public void WriteEnum<T>(T val) where T : struct, Enum
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
			int num2 = Convert.ToInt32(val);
			this.WriteInt(num2, num);
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x002152E0 File Offset: 0x002134E0
		public void WriteInt(int val, int bits = 32)
		{
			this.ResizeBufferIfNecessary(bits);
			BinaryPrimitives.WriteInt32LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x00215320 File Offset: 0x00213520
		public void WriteUInt(uint val, int bits = 32)
		{
			this.ResizeBufferIfNecessary(bits);
			BinaryPrimitives.WriteUInt32LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00215360 File Offset: 0x00213560
		public void WriteFloat(float val, QuantizeParams? quantizeParams = null)
		{
			if (quantizeParams != null)
			{
				this.ResizeBufferIfNecessary(quantizeParams.Value.bits);
				uint num = PacketWriter.Quantize(val, quantizeParams.Value.min, quantizeParams.Value.max, quantizeParams.Value.bits);
				this.WriteUInt(num, quantizeParams.Value.bits);
				return;
			}
			this.ResizeBufferIfNecessary(32);
			BinaryPrimitives.WriteSingleLittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 32);
			this.BitPosition += 32;
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00215408 File Offset: 0x00213608
		public void WriteLong(long val, int bits = 64)
		{
			this.ResizeBufferIfNecessary(64);
			BinaryPrimitives.WriteInt64LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x00215454 File Offset: 0x00213654
		public void WriteULong(ulong val, int bits = 64)
		{
			this.ResizeBufferIfNecessary(64);
			BinaryPrimitives.WriteUInt64LittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
			this.BitPosition += bits;
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x002154A0 File Offset: 0x002136A0
		public void WriteDouble(double val)
		{
			this.ResizeBufferIfNecessary(64);
			BinaryPrimitives.WriteDoubleLittleEndian(this._tempBuffer.AsSpan<byte>(), val);
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 64);
			this.BitPosition += 64;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x002154EE File Offset: 0x002136EE
		public void WriteVector2(Vector2 val, QuantizeParams? quantizeParamsX = null, QuantizeParams? quantizeParamsY = null)
		{
			this.WriteFloat(val.X, quantizeParamsX);
			this.WriteFloat(val.Y, quantizeParamsY);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0021550C File Offset: 0x0021370C
		public void WriteList<[Nullable(0)] T>(IReadOnlyList<T> list, int lengthBits = 32) where T : IPacketSerializable, new()
		{
			if ((long)list.Count >= 1L << lengthBits)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 2);
				defaultInterpolatedStringHandler.AppendLiteral("List length ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" is too large to fit in bit size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(lengthBits);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new IndexOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.WriteInt(list.Count, lengthBits);
			foreach (T t in list)
			{
				this.Write<T>(t);
			}
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x002155C4 File Offset: 0x002137C4
		public void WriteString(string str)
		{
			int byteCount = Encoding.UTF8.GetByteCount(str);
			if (this._stringBuffer.Length < byteCount)
			{
				int num = (int)Math.Pow(2.0, Math.Ceiling(Math.Log2((double)byteCount)));
				this._stringBuffer = new byte[num];
			}
			int bytes = Encoding.UTF8.GetBytes(str, this._stringBuffer);
			this.WriteInt(bytes, 32);
			this.WriteBytes(this._stringBuffer, bytes);
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x00215642 File Offset: 0x00213842
		public void Write<[Nullable(0)] T>(T val) where T : IPacketSerializable
		{
			val.Serialize(this);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x00215654 File Offset: 0x00213854
		public void ZeroByteRemainder()
		{
			int num = 8 - this.BitPosition % 8;
			if (num == 8)
			{
				return;
			}
			this._tempBuffer[0] = 0;
			BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, num);
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x00215694 File Offset: 0x00213894
		private void ResizeBufferIfNecessary(int bitsBeingWritten)
		{
			int i = Mathf.CeilToInt((float)(this.BitPosition + bitsBeingWritten) / 8f);
			int num = this.Buffer.Length;
			while (i >= num)
			{
				num *= 2;
			}
			if (num == this.Buffer.Length)
			{
				return;
			}
			byte[] array = new byte[num];
			Array.Copy(this.Buffer, array, this.Buffer.Length);
			if (this.WarnOnGrow)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Warning: Packet writer is growing from ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Buffer.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(array.Length);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes!");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			this.Buffer = array;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x00215753 File Offset: 0x00213953
		public static uint Quantize(float value, float min, float max, int bitLength)
		{
			return (uint)((double)((value - min) / (max - min)) * Math.Pow(2.0, (double)bitLength));
		}

		// Token: 0x04001ADD RID: 6877
		private readonly byte[] _tempBuffer = new byte[16];

		// Token: 0x04001ADE RID: 6878
		private byte[] _stringBuffer = new byte[64];
	}
}
