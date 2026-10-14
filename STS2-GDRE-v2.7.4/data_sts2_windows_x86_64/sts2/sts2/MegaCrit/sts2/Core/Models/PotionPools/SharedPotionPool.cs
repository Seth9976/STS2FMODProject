using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Models.PotionPools
{
	// Token: 0x0200078B RID: 1931
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SharedPotionPool : PotionPoolModel
	{
		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06005EEF RID: 24303 RVA: 0x00259B1B File Offset: 0x00257D1B
		public override string EnergyColorName
		{
			get
			{
				return "colorless";
			}
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x00259B24 File Offset: 0x00257D24
		protected override IEnumerable<PotionModel> GenerateAllPotions()
		{
			return new <>z__ReadOnlyArray<PotionModel>(new PotionModel[]
			{
				ModelDb.Potion<AttackPotion>(),
				ModelDb.Potion<BeetleJuice>(),
				ModelDb.Potion<BlessingOfTheForge>(),
				ModelDb.Potion<BlockPotion>(),
				ModelDb.Potion<BottledPotential>(),
				ModelDb.Potion<Clarity>(),
				ModelDb.Potion<ColorlessPotion>(),
				ModelDb.Potion<CureAll>(),
				ModelDb.Potion<DexterityPotion>(),
				ModelDb.Potion<DistilledChaos>(),
				ModelDb.Potion<DropletOfPrecognition>(),
				ModelDb.Potion<Duplicator>(),
				ModelDb.Potion<EnergyPotion>(),
				ModelDb.Potion<EntropicBrew>(),
				ModelDb.Potion<ExplosiveAmpoule>(),
				ModelDb.Potion<FairyInABottle>(),
				ModelDb.Potion<FirePotion>(),
				ModelDb.Potion<FlexPotion>(),
				ModelDb.Potion<Fortifier>(),
				ModelDb.Potion<FruitJuice>(),
				ModelDb.Potion<FyshOil>(),
				ModelDb.Potion<GamblersBrew>(),
				ModelDb.Potion<GigantificationPotion>(),
				ModelDb.Potion<HeartOfIron>(),
				ModelDb.Potion<LiquidBronze>(),
				ModelDb.Potion<LiquidMemories>(),
				ModelDb.Potion<LuckyTonic>(),
				ModelDb.Potion<MazalethsGift>(),
				ModelDb.Potion<OrobicAcid>(),
				ModelDb.Potion<PotionOfBinding>(),
				ModelDb.Potion<PowderedDemise>(),
				ModelDb.Potion<PowerPotion>(),
				ModelDb.Potion<RadiantTincture>(),
				ModelDb.Potion<RegenPotion>(),
				ModelDb.Potion<ShacklingPotion>(),
				ModelDb.Potion<ShipInABottle>(),
				ModelDb.Potion<SkillPotion>(),
				ModelDb.Potion<SneckoOil>(),
				ModelDb.Potion<SpeedPotion>(),
				ModelDb.Potion<StableSerum>(),
				ModelDb.Potion<StrengthPotion>(),
				ModelDb.Potion<SwiftPotion>(),
				ModelDb.Potion<TouchOfInsanity>(),
				ModelDb.Potion<VulnerablePotion>(),
				ModelDb.Potion<WeakPotion>()
			});
		}

		/// <summary>
		/// Returns a list of Potions but prunes out any potions if the associated Epochs aren't revealed.
		/// </summary>
		// Token: 0x06005EF1 RID: 24305 RVA: 0x00259CCC File Offset: 0x00257ECC
		public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
		{
			List<PotionModel> list = base.AllPotions.ToList<PotionModel>();
			if (!unlockState.IsEpochRevealed<Potion1Epoch>())
			{
				foreach (PotionModel potionModel in Potion1Epoch.Potions)
				{
					list.Remove(potionModel);
				}
			}
			if (!unlockState.IsEpochRevealed<Potion2Epoch>())
			{
				foreach (PotionModel potionModel2 in Potion2Epoch.Potions)
				{
					list.Remove(potionModel2);
				}
			}
			return list;
		}
	}
}
