using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player has ended their turn and presses the "Ping" button to annoy other players.
	/// </summary>
	// Token: 0x0200048A RID: 1162
	public struct EndTurnPingMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x00218E94 File Offset: 0x00217094
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x00218E97 File Offset: 0x00217097
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x00218E9A File Offset: 0x0021709A
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060046C8 RID: 18120 RVA: 0x00218E9D File Offset: 0x0021709D
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x00218EA0 File Offset: 0x002170A0
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x00218EA2 File Offset: 0x002170A2
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
		}
	}
}
