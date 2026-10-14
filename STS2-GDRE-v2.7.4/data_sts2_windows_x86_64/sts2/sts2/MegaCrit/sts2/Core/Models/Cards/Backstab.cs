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
	// Token: 0x0200090A RID: 2314
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Backstab : CardModel
	{
		// Token: 0x06006DE4 RID: 28132 RVA: 0x0028433B File Offset: 0x0028253B
		public Backstab()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x06006DE5 RID: 28133 RVA: 0x00284348 File Offset: 0x00282548
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Exhaust,
					CardKeyword.Innate
				});
			}
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x06006DE6 RID: 28134 RVA: 0x0028435D File Offset: 0x0028255D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(11m, ValueProp.Move));
			}
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x00284374 File Offset: 0x00282574
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x06006DE8 RID: 28136 RVA: 0x002843C7 File Offset: 0x002825C7
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}
	}
}
