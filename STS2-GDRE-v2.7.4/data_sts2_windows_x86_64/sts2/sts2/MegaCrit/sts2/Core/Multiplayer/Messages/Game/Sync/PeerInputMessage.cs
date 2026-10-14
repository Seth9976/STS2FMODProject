using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	/// <summary>
	/// Message sent to update the input state of a remote peer.
	/// Input state includes mouse position, mouse down state, and hover info.
	/// </summary>
	// Token: 0x02000482 RID: 1154
	public class PeerInputMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x00218784 File Offset: 0x00216984
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x00218787 File Offset: 0x00216987
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x0021878A File Offset: 0x0021698A
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600468A RID: 18058 RVA: 0x0021878D File Offset: 0x0021698D
		public bool ShouldBuffer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x00218790 File Offset: 0x00216990
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.mouseDown);
			writer.WriteBool(this.isTargeting);
			writer.WriteBool(this.netMousePos != null);
			if (this.netMousePos != null)
			{
				writer.WriteVector2(this.netMousePos.Value, new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams));
			}
			writer.WriteEnum<NetScreenType>(this.screenType);
			writer.Write<HoveredModelData>(this.hoveredModelData);
			writer.WriteBool(this.isUsingController);
			writer.WriteBool(this.controllerFocusPosition != null);
			if (this.controllerFocusPosition != null)
			{
				writer.WriteVector2(this.controllerFocusPosition.Value, new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams));
			}
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x00218860 File Offset: 0x00216A60
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.mouseDown = reader.ReadBool();
			this.isTargeting = reader.ReadBool();
			if (reader.ReadBool())
			{
				this.netMousePos = new Vector2?(reader.ReadVector2(new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams)));
			}
			this.screenType = reader.ReadEnum<NetScreenType>();
			this.hoveredModelData = reader.Read<HoveredModelData>();
			this.isUsingController = reader.ReadBool();
			if (reader.ReadBool())
			{
				this.controllerFocusPosition = new Vector2?(reader.ReadVector2(new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams)));
			}
		}

		// Token: 0x04001B97 RID: 7063
		private static readonly QuantizeParams _quantizeParams = new QuantizeParams(-3f, 3f, 16);

		// Token: 0x04001B98 RID: 7064
		public bool mouseDown;

		// Token: 0x04001B99 RID: 7065
		public bool isTargeting;

		// Token: 0x04001B9A RID: 7066
		public Vector2? netMousePos;

		// Token: 0x04001B9B RID: 7067
		public NetScreenType screenType;

		// Token: 0x04001B9C RID: 7068
		public HoveredModelData hoveredModelData;

		// Token: 0x04001B9D RID: 7069
		public bool isUsingController;

		// Token: 0x04001B9E RID: 7070
		public Vector2? controllerFocusPosition;
	}
}
