using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A49 RID: 2633
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MomentumStrike : CardModel
	{
		// Token: 0x06007499 RID: 29849 RVA: 0x00291D6F File Offset: 0x0028FF6F
		public MomentumStrike()
			: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F38 RID: 7992
		// (get) Token: 0x0600749A RID: 29850 RVA: 0x00291D7C File Offset: 0x0028FF7C
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001F39 RID: 7993
		// (get) Token: 0x0600749B RID: 29851 RVA: 0x00291D8B File Offset: 0x0028FF8B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x00291DA0 File Offset: 0x0028FFA0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			base.EnergyCost.SetThisCombat(0, false);
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x00291DF3 File Offset: 0x0028FFF3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
