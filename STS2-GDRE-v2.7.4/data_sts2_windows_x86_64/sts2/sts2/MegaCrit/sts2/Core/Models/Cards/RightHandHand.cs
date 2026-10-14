using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA3 RID: 2723
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RightHandHand : CardModel
	{
		// Token: 0x0600766E RID: 30318 RVA: 0x00295AEE File Offset: 0x00293CEE
		public RightHandHand()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001FFA RID: 8186
		// (get) Token: 0x0600766F RID: 30319 RVA: 0x00295AFB File Offset: 0x00293CFB
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.OstyAttack };
			}
		}

		// Token: 0x17001FFB RID: 8187
		// (get) Token: 0x06007670 RID: 30320 RVA: 0x00295B0A File Offset: 0x00293D0A
		protected override bool ShouldGlowRedInternal
		{
			get
			{
				return base.Owner.IsOstyMissing;
			}
		}

		// Token: 0x17001FFC RID: 8188
		// (get) Token: 0x06007671 RID: 30321 RVA: 0x00295B17 File Offset: 0x00293D17
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x17001FFD RID: 8189
		// (get) Token: 0x06007672 RID: 30322 RVA: 0x00295B24 File Offset: 0x00293D24
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new OstyDamageVar(4m, ValueProp.Move),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x00295B4C File Offset: 0x00293D4C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			if (!Osty.CheckMissingWithAnim(base.Owner))
			{
				await DamageCmd.Attack(base.DynamicVars.OstyDamage.BaseValue).FromOsty(base.Owner.Osty, this, cardPlay).Targeting(cardPlay.Target)
					.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
					.Execute(choiceContext);
			}
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x00295BA0 File Offset: 0x00293DA0
		public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner == base.Owner)
			{
				if (cardPlay.Resources.EnergyValue >= base.DynamicVars.Energy.IntValue)
				{
					CardPile pile = base.Pile;
					if (pile != null && pile.Type == PileType.Discard)
					{
						await CardPileCmd.Add(this, PileType.Hand, CardPilePosition.Bottom, null, false);
					}
				}
			}
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x00295BEB File Offset: 0x00293DEB
		protected override void OnUpgrade()
		{
			base.DynamicVars.OstyDamage.UpgradeValueBy(2m);
		}
	}
}
