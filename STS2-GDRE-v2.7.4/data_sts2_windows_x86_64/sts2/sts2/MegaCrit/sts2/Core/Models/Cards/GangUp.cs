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
	// Token: 0x020009DA RID: 2522
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GangUp : CardModel
	{
		// Token: 0x06007237 RID: 29239 RVA: 0x0028CE5B File Offset: 0x0028B05B
		public GangUp()
			: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001E3B RID: 7739
		// (get) Token: 0x06007238 RID: 29240 RVA: 0x0028CE68 File Offset: 0x0028B068
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001E3C RID: 7740
		// (get) Token: 0x06007239 RID: 29241 RVA: 0x0028CE6C File Offset: 0x0028B06C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[3];
				array[0] = new CalculationBaseVar(5m);
				array[1] = new ExtraDamageVar(5m);
				array[2] = new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature target) => CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count<DamageReceivedEntry>((DamageReceivedEntry e) => e.Receiver == target && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState) && e.Dealer != null && e.Dealer != card.Owner.Creature && e.Dealer.Side == card.Owner.Creature.Side));
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x0600723A RID: 29242 RVA: 0x0028CED0 File Offset: 0x0028B0D0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
				.Execute(choiceContext);
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x0028CF23 File Offset: 0x0028B123
		protected override void OnUpgrade()
		{
			base.DynamicVars.ExtraDamage.UpgradeValueBy(2m);
		}
	}
}
