using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player clicks on a map coordinate that they've already voted on.
	/// </summary>
	// Token: 0x0200048D RID: 1165
	[RequiredMember]
	public struct MapPingMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060046DB RID: 18139 RVA: 0x00218F9D File Offset: 0x0021719D
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060046DC RID: 18140 RVA: 0x00218FA0 File Offset: 0x002171A0
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x00218FA3 File Offset: 0x002171A3
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060046DE RID: 18142 RVA: 0x00218FA6 File Offset: 0x002171A6
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x00218FA9 File Offset: 0x002171A9
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.Write<MapCoord>(this.coord);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x00218FB7 File Offset: 0x002171B7
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.coord = reader.Read<MapCoord>();
		}

		// Token: 0x04001BB7 RID: 7095
		[RequiredMember]
		public MapCoord coord;
	}
}
