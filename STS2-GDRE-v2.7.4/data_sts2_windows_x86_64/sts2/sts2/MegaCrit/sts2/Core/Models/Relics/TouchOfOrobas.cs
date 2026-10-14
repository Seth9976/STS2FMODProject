using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005FF RID: 1535
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TouchOfOrobas : RelicModel
	{
		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600558B RID: 21899 RVA: 0x00248C07 File Offset: 0x00246E07
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x0600558C RID: 21900 RVA: 0x00248C0C File Offset: 0x00246E0C
		private static Dictionary<ModelId, RelicModel> RefinementUpgrades
		{
			get
			{
				return new Dictionary<ModelId, RelicModel>
				{
					{
						ModelDb.Relic<BurningBlood>().Id,
						ModelDb.Relic<BlackBlood>()
					},
					{
						ModelDb.Relic<RingOfTheSnake>().Id,
						ModelDb.Relic<RingOfTheDrake>()
					},
					{
						ModelDb.Relic<DivineRight>().Id,
						ModelDb.Relic<DivineDestiny>()
					},
					{
						ModelDb.Relic<BoundPhylactery>().Id,
						ModelDb.Relic<PhylacteryUnbound>()
					},
					{
						ModelDb.Relic<CrackedCore>().Id,
						ModelDb.Relic<InfusedCore>()
					}
				};
			}
		}

		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x0600558D RID: 21901 RVA: 0x00248C87 File Offset: 0x00246E87
		// (set) Token: 0x0600558E RID: 21902 RVA: 0x00248C90 File Offset: 0x00246E90
		[Nullable(2)]
		[SavedProperty]
		public ModelId StarterRelic
		{
			[NullableContext(2)]
			get
			{
				return this._starterRelic;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				if (this._starterRelic != null)
				{
					throw new InvalidOperationException("Recursive Core setup called twice!");
				}
				this._starterRelic = value;
				if (this._starterRelic != null)
				{
					RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._starterRelic);
					this._extraHoverTips.AddRange(relicModel.HoverTips);
					((StringVar)base.DynamicVars["StarterRelic"]).StringValue = relicModel.Title.GetFormattedText();
				}
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x0600558F RID: 21903 RVA: 0x00248D13 File Offset: 0x00246F13
		// (set) Token: 0x06005590 RID: 21904 RVA: 0x00248D1C File Offset: 0x00246F1C
		[Nullable(2)]
		[SavedProperty]
		public ModelId UpgradedRelic
		{
			[NullableContext(2)]
			get
			{
				return this._upgradedRelic;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				if (this._upgradedRelic != null)
				{
					throw new InvalidOperationException("Recursive Core setup called twice!");
				}
				this._upgradedRelic = value;
				if (this._upgradedRelic != null)
				{
					RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._upgradedRelic);
					this._extraHoverTips.AddRange(relicModel.HoverTips);
					((StringVar)base.DynamicVars["UpgradedRelic"]).StringValue = relicModel.Title.GetFormattedText();
				}
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06005591 RID: 21905 RVA: 0x00248D9F File Offset: 0x00246F9F
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return this._extraHoverTips;
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06005592 RID: 21906 RVA: 0x00248DA7 File Offset: 0x00246FA7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("StarterRelic", ""),
					new StringVar("UpgradedRelic", "")
				});
			}
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x00248DD8 File Offset: 0x00246FD8
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this._extraHoverTips = new List<IHoverTip>();
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x00248DEB File Offset: 0x00246FEB
		[return: Nullable(2)]
		private RelicModel GetStarterRelic(Player p)
		{
			return p.Relics.FirstOrDefault<RelicModel>((RelicModel r) => r.Rarity == RelicRarity.Starter);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x00248E18 File Offset: 0x00247018
		public RelicModel GetUpgradedStarterRelic(RelicModel starterRelic)
		{
			RelicModel relicModel;
			if (TouchOfOrobas.RefinementUpgrades.TryGetValue(starterRelic.Id, out relicModel))
			{
				return relicModel;
			}
			return ModelDb.Relic<Circlet>().ToMutable();
		}

		/// <summary>
		/// Sets up the upgraded starter relic based off of the player.
		/// </summary>
		/// <param name="player">The player who we are upgrading the starter relic for</param>
		/// <returns>Returns false if player doesn't have the original starter relic.</returns>
		// Token: 0x06005596 RID: 21910 RVA: 0x00248E48 File Offset: 0x00247048
		public bool SetupForPlayer(Player player)
		{
			base.AssertMutable();
			RelicModel starterRelic = this.GetStarterRelic(player);
			if (starterRelic != null)
			{
				this.StarterRelic = starterRelic.Id;
				this.UpgradedRelic = this.GetUpgradedStarterRelic(starterRelic).Id;
				return true;
			}
			return false;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x00248E87 File Offset: 0x00247087
		public void SetupForTests(ModelId starterRelic, ModelId upgradedRelic)
		{
			base.AssertMutable();
			this.StarterRelic = starterRelic;
			this.UpgradedRelic = upgradedRelic;
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x00248EA0 File Offset: 0x002470A0
		public override async Task AfterObtained()
		{
			ModelId modelId;
			if ((modelId = this.StarterRelic) == null)
			{
				modelId = base.Owner.Relics.First<RelicModel>((RelicModel r) => r.Rarity == RelicRarity.Starter).Id;
			}
			ModelId modelId2 = modelId;
			RelicModel relicById = base.Owner.GetRelicById(modelId2);
			ModelId modelId3 = this.UpgradedRelic ?? this.GetUpgradedStarterRelic(relicById).Id;
			RelicModel relicModel = ModelDb.GetById<RelicModel>(modelId3).ToMutable();
			await RelicCmd.Replace(relicById, relicModel);
		}

		// Token: 0x040024D0 RID: 9424
		private const string _starterRelicKey = "StarterRelic";

		// Token: 0x040024D1 RID: 9425
		private const string _upgradedRelicKey = "UpgradedRelic";

		// Token: 0x040024D2 RID: 9426
		[Nullable(2)]
		private ModelId _starterRelic;

		// Token: 0x040024D3 RID: 9427
		[Nullable(2)]
		private ModelId _upgradedRelic;

		// Token: 0x040024D4 RID: 9428
		private List<IHoverTip> _extraHoverTips = new List<IHoverTip>();
	}
}
