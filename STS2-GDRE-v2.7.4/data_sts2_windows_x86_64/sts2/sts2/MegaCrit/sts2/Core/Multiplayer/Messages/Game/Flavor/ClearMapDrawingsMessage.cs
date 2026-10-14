using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player clears their drawings at the map screen.
	/// </summary>
	// Token: 0x02000489 RID: 1161
	public struct ClearMapDrawingsMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060046BF RID: 18111 RVA: 0x00218E84 File Offset: 0x00217084
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060046C0 RID: 18112 RVA: 0x00218E87 File Offset: 0x00217087
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x00218E8A File Offset: 0x0021708A
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060046C2 RID: 18114 RVA: 0x00218E8D File Offset: 0x0021708D
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x00218E90 File Offset: 0x00217090
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x00218E92 File Offset: 0x00217092
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
