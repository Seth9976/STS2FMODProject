using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C20 RID: 3104
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetEndPlayerTurnAction : INetAction, IPacketSerializable
	{
		// Token: 0x06008027 RID: 32807 RVA: 0x002B7F11 File Offset: 0x002B6111
		public GameAction ToGameAction(Player player)
		{
			return new EndPlayerTurnAction(player, this.turnNumber);
		}

		// Token: 0x06008028 RID: 32808 RVA: 0x002B7F1F File Offset: 0x002B611F
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.turnNumber, 16);
		}

		// Token: 0x06008029 RID: 32809 RVA: 0x002B7F2F File Offset: 0x002B612F
		public void Deserialize(PacketReader reader)
		{
			this.turnNumber = reader.ReadInt(16);
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x002B7F40 File Offset: 0x002B6140
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetEndPlayerTurnAction");
			defaultInterpolatedStringHandler.AppendLiteral(" turn: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.turnNumber);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002ADF RID: 10975
		public int turnNumber;
	}
}
