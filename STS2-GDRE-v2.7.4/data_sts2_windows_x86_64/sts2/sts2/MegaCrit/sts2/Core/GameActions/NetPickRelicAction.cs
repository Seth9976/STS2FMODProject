using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C26 RID: 3110
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetPickRelicAction : INetAction, IPacketSerializable
	{
		// Token: 0x0600806C RID: 32876 RVA: 0x002B8B05 File Offset: 0x002B6D05
		public GameAction ToGameAction(Player player)
		{
			return new PickRelicAction(player, this.relicIndex);
		}

		// Token: 0x0600806D RID: 32877 RVA: 0x002B8B13 File Offset: 0x002B6D13
		public void Serialize(PacketWriter writer)
		{
			writer.WriteBool(this.relicIndex != null);
			if (this.relicIndex != null)
			{
				writer.WriteInt(this.relicIndex.Value, 8);
			}
		}

		// Token: 0x0600806E RID: 32878 RVA: 0x002B8B45 File Offset: 0x002B6D45
		public void Deserialize(PacketReader reader)
		{
			if (reader.ReadBool())
			{
				this.relicIndex = new int?(reader.ReadInt(8));
			}
		}

		// Token: 0x0600806F RID: 32879 RVA: 0x002B8B64 File Offset: 0x002B6D64
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NetPickRelicAction");
			defaultInterpolatedStringHandler.AppendLiteral(" index: ");
			defaultInterpolatedStringHandler.AppendFormatted<int?>(this.relicIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002AFB RID: 11003
		public int? relicIndex;
	}
}
