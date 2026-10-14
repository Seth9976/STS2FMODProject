using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player presses the pencil button at the bottom-left of the map screen.
	/// </summary>
	// Token: 0x0200048C RID: 1164
	public struct MapDrawingModeChangedMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060046D5 RID: 18133 RVA: 0x00218F75 File Offset: 0x00217175
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060046D6 RID: 18134 RVA: 0x00218F78 File Offset: 0x00217178
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060046D7 RID: 18135 RVA: 0x00218F7B File Offset: 0x0021717B
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060046D8 RID: 18136 RVA: 0x00218F7E File Offset: 0x0021717E
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x00218F81 File Offset: 0x00217181
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<DrawingMode>(this.drawingMode);
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x00218F8F File Offset: 0x0021718F
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.drawingMode = reader.ReadEnum<DrawingMode>();
		}

		// Token: 0x04001BB6 RID: 7094
		public DrawingMode drawingMode;
	}
}
