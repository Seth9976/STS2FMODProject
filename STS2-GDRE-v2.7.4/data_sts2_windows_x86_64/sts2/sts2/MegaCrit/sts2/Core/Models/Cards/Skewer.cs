using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000ACA RID: 2762
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Skewer : CardModel
	{
		// Token: 0x06007745 RID: 30533 RVA: 0x002975A9 File Offset: 0x002957A9
		public Skewer()
			: base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002056 RID: 8278
		// (get) Token: 0x06007746 RID: 30534 RVA: 0x002975B6 File Offset: 0x002957B6
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002057 RID: 8279
		// (get) Token: 0x06007747 RID: 30535 RVA: 0x002975B9 File Offset: 0x002957B9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Move));
			}
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x002975CC File Offset: 0x002957CC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.ResolveEnergyXValue()).FromCard(this, cardPlay)
				.Targeting(cardPlay.Target)
				.WithHitVfxNode((Creature t) => NStabVfx.Create(t, true, VfxColor.Gold))
				.Execute(choiceContext);
		}

		// Token: 0x06007749 RID: 30537 RVA: 0x0029761F File Offset: 0x0029581F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}
	}
}
