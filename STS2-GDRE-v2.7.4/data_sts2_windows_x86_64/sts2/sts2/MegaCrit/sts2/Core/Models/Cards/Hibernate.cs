using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009FF RID: 2559
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Hibernate : CardModel
	{
		// Token: 0x060072F9 RID: 29433 RVA: 0x0028E753 File Offset: 0x0028C953
		public Hibernate()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E8A RID: 7818
		// (get) Token: 0x060072FA RID: 29434 RVA: 0x0028E760 File Offset: 0x0028C960
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001E8B RID: 7819
		// (get) Token: 0x060072FB RID: 29435 RVA: 0x0028E763 File Offset: 0x0028C963
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.Channeling, Array.Empty<DynamicVar>()),
					HoverTipFactory.FromOrb<FrostOrb>(),
					HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>())
				});
			}
		}

		// Token: 0x17001E8C RID: 7820
		// (get) Token: 0x060072FC RID: 29436 RVA: 0x0028E794 File Offset: 0x0028C994
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(2));
			}
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x0028E7A4 File Offset: 0x0028C9A4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<HibernatePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, cardPlay.Card, false);
			for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
			{
				await OrbCmd.Channel<FrostOrb>(choiceContext, base.Owner);
			}
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x0028E7F7 File Offset: 0x0028C9F7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Repeat.UpgradeValueBy(1m);
		}
	}
}
