using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000997 RID: 2455
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Dominate : CardModel
	{
		// Token: 0x060070C0 RID: 28864 RVA: 0x00289F23 File Offset: 0x00288123
		public Dominate()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001D94 RID: 7572
		// (get) Token: 0x060070C1 RID: 28865 RVA: 0x00289F30 File Offset: 0x00288130
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001D95 RID: 7573
		// (get) Token: 0x060070C2 RID: 28866 RVA: 0x00289F38 File Offset: 0x00288138
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<VulnerablePower>(1m),
					new DynamicVar("StrengthPerVulnerable", 1m)
				});
			}
		}

		// Token: 0x17001D96 RID: 7574
		// (get) Token: 0x060070C3 RID: 28867 RVA: 0x00289F64 File Offset: 0x00288164
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x00289FA0 File Offset: 0x002881A0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars["VulnerablePower"].BaseValue, base.Owner.Creature, this, false);
			VulnerablePower power = cardPlay.Target.GetPower<VulnerablePower>();
			int num = ((power != null) ? power.Amount : 0);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, num, base.Owner.Creature, this, false);
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x00289FF3 File Offset: 0x002881F3
		protected override void OnUpgrade()
		{
			base.DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
		}

		// Token: 0x0400285E RID: 10334
		private const string _strengthPerVulnerableKey = "StrengthPerVulnerable";
	}
}
