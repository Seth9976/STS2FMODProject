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
	// Token: 0x02000A22 RID: 2594
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnowThyPlace : CardModel
	{
		// Token: 0x060073B1 RID: 29617 RVA: 0x0028FEAB File Offset: 0x0028E0AB
		public KnowThyPlace()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ED6 RID: 7894
		// (get) Token: 0x060073B2 RID: 29618 RVA: 0x0028FEB8 File Offset: 0x0028E0B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<WeakPower>(1m),
					new PowerVar<VulnerablePower>(1m)
				});
			}
		}

		// Token: 0x17001ED7 RID: 7895
		// (get) Token: 0x060073B3 RID: 29619 RVA: 0x0028FEDF File Offset: 0x0028E0DF
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001ED8 RID: 7896
		// (get) Token: 0x060073B4 RID: 29620 RVA: 0x0028FEE8 File Offset: 0x0028E0E8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<WeakPower>(null),
					HoverTipFactory.FromPower<VulnerablePower>(null)
				});
			}
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x0028FF24 File Offset: 0x0028E124
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x0028FF77 File Offset: 0x0028E177
		protected override void OnUpgrade()
		{
			base.RemoveKeyword(CardKeyword.Exhaust);
		}
	}
}
