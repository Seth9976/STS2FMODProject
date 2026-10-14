using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems
{
	// Token: 0x02000C56 RID: 3158
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSpherePotion : CrystalSphereItem
	{
		// Token: 0x1700238B RID: 9099
		// (get) Token: 0x06008228 RID: 33320 RVA: 0x002C0964 File Offset: 0x002BEB64
		protected override string TexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("events/crystal_sphere/crystal_sphere_" + this._rarity.ToString().ToLowerInvariant() + "_potion.png");
			}
		}

		// Token: 0x1700238C RID: 9100
		// (get) Token: 0x06008229 RID: 33321 RVA: 0x002C099E File Offset: 0x002BEB9E
		public override bool IsGood
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700238D RID: 9101
		// (get) Token: 0x0600822A RID: 33322 RVA: 0x002C09A1 File Offset: 0x002BEBA1
		public override Vector2I Size
		{
			get
			{
				if (this._rarity != PotionRarity.Rare)
				{
					return new Vector2I(1, 3);
				}
				return new Vector2I(2, 2);
			}
		}

		// Token: 0x0600822B RID: 33323 RVA: 0x002C09BB File Offset: 0x002BEBBB
		public CrystalSpherePotion(PotionRarity rarity)
		{
			this._rarity = rarity;
		}

		// Token: 0x0600822C RID: 33324 RVA: 0x002C09CC File Offset: 0x002BEBCC
		public override Reward ToReward(Player owner, Rng rng)
		{
			IEnumerable<PotionModel> enumerable = from p in PotionFactory.GetPotionOptions(owner)
				where p.Rarity == this._rarity
				select p;
			PotionModel potionModel = rng.NextItem<PotionModel>(enumerable).ToMutable();
			return new PotionReward(potionModel, owner).SetRng(rng);
		}

		// Token: 0x0600822D RID: 33325 RVA: 0x002C0A0B File Offset: 0x002BEC0B
		public override SerializableCrystalSphereItem ToSerializable()
		{
			return new SerializableCrystalSphereItem
			{
				type = CrystalSphereItemType.Potion,
				potionRarity = this._rarity
			};
		}

		// Token: 0x04002BA5 RID: 11173
		private readonly PotionRarity _rarity;
	}
}
