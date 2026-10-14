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
	// Token: 0x02000A1D RID: 2589
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KinglyKick : CardModel
	{
		// Token: 0x06007397 RID: 29591 RVA: 0x0028FA64 File Offset: 0x0028DC64
		public KinglyKick()
			: base(4, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001ECE RID: 7886
		// (get) Token: 0x06007398 RID: 29592 RVA: 0x0028FA71 File Offset: 0x0028DC71
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(27m, ValueProp.Move));
			}
		}

		// Token: 0x06007399 RID: 29593 RVA: 0x0028FA88 File Offset: 0x0028DC88
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, "heavy_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x0600739A RID: 29594 RVA: 0x0028FADB File Offset: 0x0028DCDB
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(8m);
		}

		// Token: 0x0600739B RID: 29595 RVA: 0x0028FAF3 File Offset: 0x0028DCF3
		public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			base.EnergyCost.AddThisCombat(-1, false);
			return Task.CompletedTask;
		}
	}
}
