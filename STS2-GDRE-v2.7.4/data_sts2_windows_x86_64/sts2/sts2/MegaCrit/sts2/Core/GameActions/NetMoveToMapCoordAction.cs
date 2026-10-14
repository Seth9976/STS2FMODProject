using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C24 RID: 3108
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetMoveToMapCoordAction : INetAction, IPacketSerializable
	{
		// Token: 0x06008060 RID: 32864 RVA: 0x002B8998 File Offset: 0x002B6B98
		public GameAction ToGameAction(Player player)
		{
			return new MoveToMapCoordAction(player, this.destination);
		}

		// Token: 0x06008061 RID: 32865 RVA: 0x002B89A6 File Offset: 0x002B6BA6
		public void Serialize(PacketWriter writer)
		{
			writer.Write<MapCoord>(this.destination);
		}

		// Token: 0x06008062 RID: 32866 RVA: 0x002B89B4 File Offset: 0x002B6BB4
		public void Deserialize(PacketReader reader)
		{
			this.destination = reader.Read<MapCoord>();
		}

		// Token: 0x06008063 RID: 32867 RVA: 0x002B89C4 File Offset: 0x002B6BC4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendFormatted("MoveToMapCoordAction");
			defaultInterpolatedStringHandler.AppendLiteral(" to ");
			defaultInterpolatedStringHandler.AppendFormatted<MapCoord>(this.destination);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AF7 RID: 10999
		public MapCoord destination;
	}
}
