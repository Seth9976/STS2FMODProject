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
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A16 RID: 2582
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Invoke : CardModel
	{
		// Token: 0x06007378 RID: 29560 RVA: 0x0028F62B File Offset: 0x0028D82B
		public Invoke()
			: base(1, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001EC4 RID: 7876
		// (get) Token: 0x06007379 RID: 29561 RVA: 0x0028F638 File Offset: 0x0028D838
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }),
					base.EnergyHoverTip
				});
			}
		}

		// Token: 0x17001EC5 RID: 7877
		// (get) Token: 0x0600737A RID: 29562 RVA: 0x0028F66C File Offset: 0x0028D86C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new SummonVar(2m),
					new EnergyVar(2)
				});
			}
		}

		// Token: 0x0600737B RID: 29563 RVA: 0x0028F690 File Offset: 0x0028D890
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await PowerCmd.Apply<SummonNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Summon.IntValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600737C RID: 29564 RVA: 0x0028F6DB File Offset: 0x0028D8DB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(1m);
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
