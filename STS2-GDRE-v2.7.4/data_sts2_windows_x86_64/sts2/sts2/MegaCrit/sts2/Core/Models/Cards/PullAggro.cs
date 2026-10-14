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
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A86 RID: 2694
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PullAggro : CardModel
	{
		// Token: 0x060075CF RID: 30159 RVA: 0x0029463F File Offset: 0x0029283F
		public PullAggro()
			: base(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001FB6 RID: 8118
		// (get) Token: 0x060075D0 RID: 30160 RVA: 0x0029464C File Offset: 0x0029284C
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001FB7 RID: 8119
		// (get) Token: 0x060075D1 RID: 30161 RVA: 0x0029464F File Offset: 0x0029284F
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new SummonVar(4m),
					new BlockVar(7m, ValueProp.Move)
				});
			}
		}

		// Token: 0x17001FB8 RID: 8120
		// (get) Token: 0x060075D2 RID: 30162 RVA: 0x00294679 File Offset: 0x00292879
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x0029469C File Offset: 0x0029289C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x002946EF File Offset: 0x002928EF
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(1m);
			base.DynamicVars.Block.UpgradeValueBy(2m);
		}
	}
}
