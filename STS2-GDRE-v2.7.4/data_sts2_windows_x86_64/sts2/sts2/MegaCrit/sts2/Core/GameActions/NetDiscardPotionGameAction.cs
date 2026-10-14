using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C1E RID: 3102
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetDiscardPotionGameAction : INetAction, IPacketSerializable
	{
		// Token: 0x0600801D RID: 32797 RVA: 0x002B7D16 File Offset: 0x002B5F16
		public GameAction ToGameAction(Player player)
		{
			return new DiscardPotionGameAction(player, this.potionSlotIndex, this.wasEnqueuedInCombat);
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x002B7D2A File Offset: 0x002B5F2A
		public void Serialize(PacketWriter writer)
		{
			writer.WriteUInt(this.potionSlotIndex, 4);
			writer.WriteBool(this.wasEnqueuedInCombat);
		}

		// Token: 0x0600801F RID: 32799 RVA: 0x002B7D45 File Offset: 0x002B5F45
		public void Deserialize(PacketReader reader)
		{
			this.potionSlotIndex = reader.ReadUInt(4);
			this.wasEnqueuedInCombat = reader.ReadBool();
		}

		// Token: 0x06008020 RID: 32800 RVA: 0x002B7D60 File Offset: 0x002B5F60
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 3);
			defaultInterpolatedStringHandler.AppendFormatted("NetDiscardPotionGameAction");
			defaultInterpolatedStringHandler.AppendLiteral(" slot ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.potionSlotIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" in combat: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.wasEnqueuedInCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002ADB RID: 10971
		public uint potionSlotIndex;

		// Token: 0x04002ADC RID: 10972
		public bool wasEnqueuedInCombat;
	}
}
