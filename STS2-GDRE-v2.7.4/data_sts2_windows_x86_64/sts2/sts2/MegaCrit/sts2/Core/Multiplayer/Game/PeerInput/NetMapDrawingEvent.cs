using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	// Token: 0x020004B0 RID: 1200
	public struct NetMapDrawingEvent : IPacketSerializable
	{
		// Token: 0x06004813 RID: 18451 RVA: 0x002207C8 File Offset: 0x0021E9C8
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<MapDrawingEventType>(this.type);
			writer.WriteBool(this.overrideDrawingMode != null);
			if (this.overrideDrawingMode != null)
			{
				writer.WriteEnum<DrawingMode>(this.overrideDrawingMode.Value);
			}
			if (this.type != MapDrawingEventType.EndLine)
			{
				writer.WriteVector2(this.position, new QuantizeParams?(NetMapDrawingEvent._quantizeParamsX), new QuantizeParams?(NetMapDrawingEvent._quantizeParamsY));
			}
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x0022083C File Offset: 0x0021EA3C
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<MapDrawingEventType>();
			if (reader.ReadBool())
			{
				this.overrideDrawingMode = new DrawingMode?(reader.ReadEnum<DrawingMode>());
			}
			if (this.type != MapDrawingEventType.EndLine)
			{
				this.position = reader.ReadVector2(new QuantizeParams?(NetMapDrawingEvent._quantizeParamsX), new QuantizeParams?(NetMapDrawingEvent._quantizeParamsY));
			}
		}

		// Token: 0x04001C58 RID: 7256
		private static readonly QuantizeParams _quantizeParamsX = new QuantizeParams(-3f, 3f, 16);

		// Token: 0x04001C59 RID: 7257
		private static readonly QuantizeParams _quantizeParamsY = new QuantizeParams(-2f, 2f, 24);

		// Token: 0x04001C5A RID: 7258
		public MapDrawingEventType type;

		// Token: 0x04001C5B RID: 7259
		public DrawingMode? overrideDrawingMode;

		// Token: 0x04001C5C RID: 7260
		public Vector2 position;
	}
}
