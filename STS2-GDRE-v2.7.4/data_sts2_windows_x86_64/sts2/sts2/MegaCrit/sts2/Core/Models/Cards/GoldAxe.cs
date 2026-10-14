using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009E6 RID: 2534
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GoldAxe : CardModel
	{
		// Token: 0x0600727C RID: 29308 RVA: 0x0028D717 File Offset: 0x0028B917
		public GoldAxe()
			: base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E57 RID: 7767
		// (get) Token: 0x0600727D RID: 29309 RVA: 0x0028D724 File Offset: 0x0028B924
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(0m);
				array[1] = new ExtraDamageVar(1m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel _, [Nullable(2)] Creature _) => CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>());
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x0028D784 File Offset: 0x0028B984
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x0028D7D7 File Offset: 0x0028B9D7
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Retain);
		}
	}
}
