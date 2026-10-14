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
	// Token: 0x020008F8 RID: 2296
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Afterlife : CardModel
	{
		// Token: 0x06006D8B RID: 28043 RVA: 0x002838BC File Offset: 0x00281ABC
		public Afterlife()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x06006D8C RID: 28044 RVA: 0x002838C9 File Offset: 0x00281AC9
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x06006D8D RID: 28045 RVA: 0x002838D1 File Offset: 0x00281AD1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(6m));
			}
		}

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x06006D8E RID: 28046 RVA: 0x002838E3 File Offset: 0x00281AE3
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x00283908 File Offset: 0x00281B08
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x00283953 File Offset: 0x00281B53
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(3m);
		}
	}
}
