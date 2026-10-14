using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C33 RID: 3123
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetVoteToMoveToNextActAction : INetAction, IPacketSerializable
	{
		// Token: 0x060080D9 RID: 32985 RVA: 0x002BA770 File Offset: 0x002B8970
		public GameAction ToGameAction(Player player)
		{
			return new VoteToMoveToNextActAction(player, this.currentActIndex);
		}

		// Token: 0x060080DA RID: 32986 RVA: 0x002BA77E File Offset: 0x002B897E
		public void Serialize(PacketWriter writer)
		{
			writer.WriteInt(this.currentActIndex, 32);
		}

		// Token: 0x060080DB RID: 32987 RVA: 0x002BA78E File Offset: 0x002B898E
		public void Deserialize(PacketReader reader)
		{
			this.currentActIndex = reader.ReadInt(32);
		}

		// Token: 0x060080DC RID: 32988 RVA: 0x002BA7A0 File Offset: 0x002B89A0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetVoteToMoveToNextActAction");
			defaultInterpolatedStringHandler.AppendLiteral(" act ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.currentActIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B22 RID: 11042
		public int currentActIndex;
	}
}
