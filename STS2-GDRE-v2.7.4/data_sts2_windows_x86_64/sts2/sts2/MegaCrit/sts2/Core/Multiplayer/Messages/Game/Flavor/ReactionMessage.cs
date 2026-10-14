using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player uses the reaction wheel to send a reaction.
	/// </summary>
	// Token: 0x0200048E RID: 1166
	public struct ReactionMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060046E1 RID: 18145 RVA: 0x00218FC5 File Offset: 0x002171C5
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060046E2 RID: 18146 RVA: 0x00218FC8 File Offset: 0x002171C8
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x00218FCB File Offset: 0x002171CB
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060046E4 RID: 18148 RVA: 0x00218FCE File Offset: 0x002171CE
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x00218FD1 File Offset: 0x002171D1
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<ReactionType>(this.type);
			writer.WriteVector2(this.normalizedPosition, new QuantizeParams?(ReactionMessage._quantizeParams), new QuantizeParams?(ReactionMessage._quantizeParams));
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00218FFF File Offset: 0x002171FF
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<ReactionType>();
			this.normalizedPosition = reader.ReadVector2(new QuantizeParams?(ReactionMessage._quantizeParams), new QuantizeParams?(ReactionMessage._quantizeParams));
		}

		// Token: 0x04001BB8 RID: 7096
		private static readonly QuantizeParams _quantizeParams = new QuantizeParams(-3f, 3f, 16);

		// Token: 0x04001BB9 RID: 7097
		public ReactionType type;

		// Token: 0x04001BBA RID: 7098
		public Vector2 normalizedPosition;
	}
}
