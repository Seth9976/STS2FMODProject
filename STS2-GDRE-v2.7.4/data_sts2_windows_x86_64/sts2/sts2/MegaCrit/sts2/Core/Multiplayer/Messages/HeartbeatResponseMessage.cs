using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages
{
	// Token: 0x0200045D RID: 1117
	public struct HeartbeatResponseMessage : INetMessage, IPacketSerializable, IEquatable<HeartbeatResponseMessage>
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06004590 RID: 17808 RVA: 0x0021754E File Offset: 0x0021574E
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06004591 RID: 17809 RVA: 0x00217551 File Offset: 0x00215751
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06004592 RID: 17810 RVA: 0x00217554 File Offset: 0x00215754
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06004593 RID: 17811 RVA: 0x00217557 File Offset: 0x00215757
		public bool ShouldBuffer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x0021755A File Offset: 0x0021575A
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.counter, 32);
			writer.WriteBool(this.isLoading);
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x00217576 File Offset: 0x00215776
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.counter = reader.ReadInt(32);
			this.isLoading = reader.ReadBool();
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00217594 File Offset: 0x00215794
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("HeartbeatResponseMessage");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x002175E0 File Offset: 0x002157E0
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
			builder.Append(", isLoading = ");
			builder.Append(this.isLoading.ToString());
			return true;
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x002176D2 File Offset: 0x002158D2
		[CompilerGenerated]
		public static bool operator !=(HeartbeatResponseMessage left, HeartbeatResponseMessage right)
		{
			return !(left == right);
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x002176DE File Offset: 0x002158DE
		[CompilerGenerated]
		public static bool operator ==(HeartbeatResponseMessage left, HeartbeatResponseMessage right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x002176E8 File Offset: 0x002158E8
		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return EqualityComparer<int>.Default.GetHashCode(this.counter) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.isLoading);
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00217711 File Offset: 0x00215911
		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is HeartbeatResponseMessage && this.Equals((HeartbeatResponseMessage)obj);
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00217729 File Offset: 0x00215929
		[CompilerGenerated]
		public readonly bool Equals(HeartbeatResponseMessage other)
		{
			return EqualityComparer<int>.Default.Equals(this.counter, other.counter) && EqualityComparer<bool>.Default.Equals(this.isLoading, other.isLoading);
		}

		// Token: 0x04001B52 RID: 6994
		public int counter;

		// Token: 0x04001B53 RID: 6995
		public bool isLoading;
	}
}
