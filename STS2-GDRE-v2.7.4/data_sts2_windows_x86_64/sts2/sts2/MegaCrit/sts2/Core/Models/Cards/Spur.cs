using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AE4 RID: 2788
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Spur : CardModel
	{
		// Token: 0x060077E1 RID: 30689 RVA: 0x0029891F File Offset: 0x00296B1F
		public Spur()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17002095 RID: 8341
		// (get) Token: 0x060077E2 RID: 30690 RVA: 0x0029892C File Offset: 0x00296B2C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x17002096 RID: 8342
		// (get) Token: 0x060077E3 RID: 30691 RVA: 0x00298934 File Offset: 0x00296B34
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x17002097 RID: 8343
		// (get) Token: 0x060077E4 RID: 30692 RVA: 0x00298956 File Offset: 0x00296B56
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new SummonVar(3m),
					new HealVar(5m)
				});
			}
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x00298980 File Offset: 0x00296B80
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			await CreatureCmd.Heal(base.Owner.Osty, base.DynamicVars.Heal.BaseValue, true);
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x002989CB File Offset: 0x00296BCB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(2m);
			base.DynamicVars.Heal.UpgradeValueBy(2m);
		}
	}
}
