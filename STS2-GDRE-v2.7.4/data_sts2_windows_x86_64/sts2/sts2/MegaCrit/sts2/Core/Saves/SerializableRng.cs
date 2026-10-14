using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableRng : IPacketSerializable, IEquatable<SerializableRng>
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x000408F0 File Offset: 0x0003EAF0
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SerializableRng);
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000408FC File Offset: 0x0003EAFC
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.counter, 32);
			writer.WriteULong(this.state0, 64);
			writer.WriteULong(this.state1, 64);
			writer.WriteULong(this.state2, 64);
			writer.WriteULong(this.state3, 64);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00040950 File Offset: 0x0003EB50
		public void Deserialize(PacketReader reader)
		{
			this.counter = reader.ReadInt(32);
			this.state0 = reader.ReadULong(64);
			this.state1 = reader.ReadULong(64);
			this.state2 = reader.ReadULong(64);
			this.state3 = reader.ReadULong(64);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000409A4 File Offset: 0x0003EBA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Counter: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.counter);
			defaultInterpolatedStringHandler.AppendLiteral(" State: ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.state0);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.state1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.state2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.state3);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00040A40 File Offset: 0x0003EC40
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("counter = ");
			builder.Append(this.counter.ToString());
			builder.Append(", state0 = ");
			builder.Append(this.state0.ToString());
			builder.Append(", state1 = ");
			builder.Append(this.state1.ToString());
			builder.Append(", state2 = ");
			builder.Append(this.state2.ToString());
			builder.Append(", state3 = ");
			builder.Append(this.state3.ToString());
			return true;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00040B07 File Offset: 0x0003ED07
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(SerializableRng left, SerializableRng right)
		{
			return !(left == right);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00040B13 File Offset: 0x0003ED13
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(SerializableRng left, SerializableRng right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00040B28 File Offset: 0x0003ED28
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.counter)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state0)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state1)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state2)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state3);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00040BB8 File Offset: 0x0003EDB8
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SerializableRng);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00040BC8 File Offset: 0x0003EDC8
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(SerializableRng other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.counter, other.counter) && EqualityComparer<ulong>.Default.Equals(this.state0, other.state0) && EqualityComparer<ulong>.Default.Equals(this.state1, other.state1) && EqualityComparer<ulong>.Default.Equals(this.state2, other.state2) && EqualityComparer<ulong>.Default.Equals(this.state3, other.state3));
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00040C78 File Offset: 0x0003EE78
		[CompilerGenerated]
		protected SerializableRng(SerializableRng original)
		{
			this.counter = original.counter;
			this.state0 = original.state0;
			this.state1 = original.state1;
			this.state2 = original.state2;
			this.state3 = original.state3;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00040CC7 File Offset: 0x0003EEC7
		public SerializableRng()
		{
		}

		// Token: 0x040002D5 RID: 725
		[JsonPropertyName("counter")]
		public int counter;

		// Token: 0x040002D6 RID: 726
		[JsonPropertyName("s0")]
		public ulong state0;

		// Token: 0x040002D7 RID: 727
		[JsonPropertyName("s1")]
		public ulong state1;

		// Token: 0x040002D8 RID: 728
		[JsonPropertyName("s2")]
		public ulong state2;

		// Token: 0x040002D9 RID: 729
		[JsonPropertyName("s3")]
		public ulong state3;
	}
}
