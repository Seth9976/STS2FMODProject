using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C2D RID: 3117
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetUndoEndPlayerTurnAction : INetAction, IPacketSerializable
	{
		// Token: 0x060080B1 RID: 32945 RVA: 0x002B9E41 File Offset: 0x002B8041
		public GameAction ToGameAction(Player player)
		{
			return new UndoEndPlayerTurnAction(player, this.turnNumber);
		}

		// Token: 0x060080B2 RID: 32946 RVA: 0x002B9E4F File Offset: 0x002B804F
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.turnNumber, 16);
		}

		// Token: 0x060080B3 RID: 32947 RVA: 0x002B9E5F File Offset: 0x002B805F
		public void Deserialize(PacketReader reader)
		{
			this.turnNumber = reader.ReadInt(16);
		}

		// Token: 0x060080B4 RID: 32948 RVA: 0x002B9E70 File Offset: 0x002B8070
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetUndoEndPlayerTurnAction");
			defaultInterpolatedStringHandler.AppendLiteral(" turn: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.turnNumber);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B10 RID: 11024
		public int turnNumber;
	}
}
