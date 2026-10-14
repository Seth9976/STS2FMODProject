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
	// Token: 0x02000A93 RID: 2707
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Reanimate : CardModel
	{
		// Token: 0x06007614 RID: 30228 RVA: 0x00294FD7 File Offset: 0x002931D7
		public Reanimate()
			: base(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001FD2 RID: 8146
		// (get) Token: 0x06007615 RID: 30229 RVA: 0x00294FE4 File Offset: 0x002931E4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(20m));
			}
		}

		// Token: 0x17001FD3 RID: 8147
		// (get) Token: 0x06007616 RID: 30230 RVA: 0x00294FF7 File Offset: 0x002931F7
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001FD4 RID: 8148
		// (get) Token: 0x06007617 RID: 30231 RVA: 0x00294FFF File Offset: 0x002931FF
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x00295024 File Offset: 0x00293224
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x0029506F File Offset: 0x0029326F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(5m);
		}
	}
}
