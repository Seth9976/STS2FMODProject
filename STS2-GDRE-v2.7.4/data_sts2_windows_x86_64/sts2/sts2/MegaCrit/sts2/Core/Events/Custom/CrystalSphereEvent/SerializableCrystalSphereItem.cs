using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent
{
	// Token: 0x02000C51 RID: 3153
	public class SerializableCrystalSphereItem : IPacketSerializable
	{
		// Token: 0x0600820E RID: 33294 RVA: 0x002C0638 File Offset: 0x002BE838
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<CrystalSphereItemType>(this.type);
			if (this.type == CrystalSphereItemType.CardReward)
			{
				writer.WriteEnum<CardRarity>(this.cardRarity);
				return;
			}
			if (this.type == CrystalSphereItemType.Potion)
			{
				writer.WriteEnum<PotionRarity>(this.potionRarity);
				return;
			}
			if (this.type == CrystalSphereItemType.Gold)
			{
				writer.WriteBool(this.isBigGold);
			}
		}

		// Token: 0x0600820F RID: 33295 RVA: 0x002C0694 File Offset: 0x002BE894
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<CrystalSphereItemType>();
			if (this.type == CrystalSphereItemType.CardReward)
			{
				this.cardRarity = reader.ReadEnum<CardRarity>();
				return;
			}
			if (this.type == CrystalSphereItemType.Potion)
			{
				this.potionRarity = reader.ReadEnum<PotionRarity>();
				return;
			}
			if (this.type == CrystalSphereItemType.Gold)
			{
				this.isBigGold = reader.ReadBool();
			}
		}

		// Token: 0x04002B96 RID: 11158
		public CrystalSphereItemType type;

		// Token: 0x04002B97 RID: 11159
		public CardRarity cardRarity;

		// Token: 0x04002B98 RID: 11160
		public PotionRarity potionRarity;

		// Token: 0x04002B99 RID: 11161
		public bool isBigGold;
	}
}
