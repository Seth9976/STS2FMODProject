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
	// Token: 0x02000A44 RID: 2628
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MinionStrike : CardModel
	{
		// Token: 0x0600747F RID: 29823 RVA: 0x00291A3F File Offset: 0x0028FC3F
		public MinionStrike()
			: base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F2D RID: 7981
		// (get) Token: 0x06007480 RID: 29824 RVA: 0x00291A4C File Offset: 0x0028FC4C
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag>
				{
					CardTag.Strike,
					CardTag.Minion
				};
			}
		}

		// Token: 0x17001F2E RID: 7982
		// (get) Token: 0x06007481 RID: 29825 RVA: 0x00291A63 File Offset: 0x0028FC63
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17001F2F RID: 7983
		// (get) Token: 0x06007482 RID: 29826 RVA: 0x00291A6B File Offset: 0x0028FC6B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x06007483 RID: 29827 RVA: 0x00291A90 File Offset: 0x0028FC90
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007484 RID: 29828 RVA: 0x00291AE3 File Offset: 0x0028FCE3
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
