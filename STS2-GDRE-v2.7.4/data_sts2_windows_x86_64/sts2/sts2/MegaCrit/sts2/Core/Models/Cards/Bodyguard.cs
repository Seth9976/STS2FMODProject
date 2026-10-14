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
	// Token: 0x02000925 RID: 2341
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Bodyguard : CardModel
	{
		// Token: 0x06006E70 RID: 28272 RVA: 0x0028558F File Offset: 0x0028378F
		public Bodyguard()
			: base(1, CardType.Skill, CardRarity.Basic, TargetType.Self, true)
		{
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x06006E71 RID: 28273 RVA: 0x0028559C File Offset: 0x0028379C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new SummonVar(5m));
			}
		}

		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x06006E72 RID: 28274 RVA: 0x002855AE File Offset: 0x002837AE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, new DynamicVar[] { base.DynamicVars.Summon }));
			}
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x002855D0 File Offset: 0x002837D0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(base.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(base.Owner.Character));
			await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars.Summon.BaseValue, this);
		}

		// Token: 0x06006E74 RID: 28276 RVA: 0x0028561B File Offset: 0x0028381B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Summon.UpgradeValueBy(2m);
		}
	}
}
