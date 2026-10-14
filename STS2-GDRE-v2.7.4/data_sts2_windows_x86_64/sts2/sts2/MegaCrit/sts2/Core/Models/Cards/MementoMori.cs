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
	// Token: 0x02000A3A RID: 2618
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MementoMori : CardModel
	{
		// Token: 0x06007444 RID: 29764 RVA: 0x002912C8 File Offset: 0x0028F4C8
		public MementoMori()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F12 RID: 7954
		// (get) Token: 0x06007445 RID: 29765 RVA: 0x002912D8 File Offset: 0x0028F4D8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(9m);
				array[1] = new ExtraDamageVar(4m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => CombatManager.Instance.History.Entries.OfType<CardDiscardedEntry>().Count<CardDiscardedEntry>((CardDiscardedEntry e) => e.HappenedThisTurn(card.CombatState) && e.Actor == card.Owner.Creature));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x0029133C File Offset: 0x0028F53C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x0029138F File Offset: 0x0028F58F
		protected override void OnUpgrade()
		{
			base.DynamicVars.CalculationBase.UpgradeValueBy(2m);
			base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
		}
	}
}
