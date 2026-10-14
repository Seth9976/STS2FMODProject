using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages
{
	// Token: 0x0200045C RID: 1116
	public struct HeartbeatRequestMessage : INetMessage, IPacketSerializable, IEquatable<HeartbeatRequestMessage>
	{
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x002173B0 File Offset: 0x002155B0
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x002173B3 File Offset: 0x002155B3
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x002173B6 File Offset: 0x002155B6
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x002173B9 File Offset: 0x002155B9
		public bool ShouldBuffer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x002173BC File Offset: 0x002155BC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.counter, 32);
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x002173CC File Offset: 0x002155CC
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.counter = reader.ReadInt(32);
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x002173DC File Offset: 0x002155DC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("HeartbeatRequestMessage");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00217428 File Offset: 0x00215628
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("ShouldBroadcast = ");
			builder.Append(this.ShouldBroadcast.ToString());
			builder.Append(", Mode = ");
			builder.Append(this.Mode.ToString());
			builder.Append(", LogLevel = ");
			builder.Append(this.LogLevel.ToString());
			builder.Append(", ShouldBuffer = ");
			builder.Append(this.ShouldBuffer.ToString());
			builder.Append(", counter = ");
			builder.Append(this.counter.ToString());
			return true;
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x002174F6 File Offset: 0x002156F6
		[CompilerGenerated]
		public static bool operator !=(HeartbeatRequestMessage left, HeartbeatRequestMessage right)
		{
			return !(left == right);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00217502 File Offset: 0x00215702
		[CompilerGenerated]
		public static bool operator ==(HeartbeatRequestMessage left, HeartbeatRequestMessage right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0021750C File Offset: 0x0021570C
		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return EqualityComparer<int>.Default.GetHashCode(this.counter);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x0021751E File Offset: 0x0021571E
		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is HeartbeatRequestMessage && this.Equals((HeartbeatRequestMessage)obj);
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00217536 File Offset: 0x00215736
		[CompilerGenerated]
		public readonly bool Equals(HeartbeatRequestMessage other)
		{
			return EqualityComparer<int>.Default.Equals(this.counter, other.counter);
		}

		// Token: 0x04001B51 RID: 6993
		public int counter;
	}
}
