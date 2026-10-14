using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200093E RID: 2366
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ByrdSwoop : CardModel
	{
		// Token: 0x06006EF2 RID: 28402 RVA: 0x002866E3 File Offset: 0x002848E3
		public ByrdSwoop()
			: base(0, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001CCB RID: 7371
		// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x002866F0 File Offset: 0x002848F0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(14m, ValueProp.Move));
			}
		}

		// Token: 0x06006EF4 RID: 28404 RVA: 0x00286704 File Offset: 0x00284904
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithAttackerAnim("Attack", base.Owner.Character.AttackAnimDelay, base.Owner.PlayerCombatState.GetPet<Byrdpip>())
				.WithHitFx("vfx/vfx_attack_slash", "event:/sfx/byrdpip/byrdpip_attack", null)
				.Execute(choiceContext);
		}

		// Token: 0x06006EF5 RID: 28405 RVA: 0x00286757 File Offset: 0x00284957
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(4m);
		}

		// Token: 0x04002850 RID: 10320
		public const string attackSfx = "event:/sfx/byrdpip/byrdpip_attack";
	}
}
