using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor
{
	/// <summary>
	/// Sent when a player begins drawing a line, updates the position of the line drawing, or ends drawing a line.
	/// </summary>
	// Token: 0x0200048B RID: 1163
	[NullableContext(1)]
	[Nullable(0)]
	public class MapDrawingMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x00218EA4 File Offset: 0x002170A4
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x00218EA7 File Offset: 0x002170A7
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Unreliable;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x00218EAA File Offset: 0x002170AA
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x00218EAD File Offset: 0x002170AD
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060046CF RID: 18127 RVA: 0x00218EB0 File Offset: 0x002170B0
		public IReadOnlyList<NetMapDrawingEvent> Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x00218EB8 File Offset: 0x002170B8
		public bool TryAddEvent(NetMapDrawingEvent ev)
		{
			if (this._events.Count >= MapDrawingMessage.maxEventCount)
			{
				return false;
			}
			this._events.Add(ev);
			return true;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00218EDB File Offset: 0x002170DB
		public void Serialize(PacketWriter writer)
		{
			writer.WriteList<NetMapDrawingEvent>(this._events, 4);
			writer.WriteBool(this.drawingMode != null);
			if (this.drawingMode != null)
			{
				writer.WriteEnum<DrawingMode>(this.drawingMode.Value);
			}
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00218F19 File Offset: 0x00217119
		public void Deserialize(PacketReader reader)
		{
			this._events = reader.ReadList<NetMapDrawingEvent>(4);
			if (reader.ReadBool())
			{
				this.drawingMode = new DrawingMode?(reader.ReadEnum<DrawingMode>());
			}
		}

		// Token: 0x04001BB2 RID: 7090
		public static readonly int maxEventCount = (int)Math.Pow(2.0, 4.0) - 1;

		// Token: 0x04001BB3 RID: 7091
		private const int _listBits = 4;

		// Token: 0x04001BB4 RID: 7092
		private List<NetMapDrawingEvent> _events = new List<NetMapDrawingEvent>();

		// Token: 0x04001BB5 RID: 7093
		public DrawingMode? drawingMode;
	}
}
