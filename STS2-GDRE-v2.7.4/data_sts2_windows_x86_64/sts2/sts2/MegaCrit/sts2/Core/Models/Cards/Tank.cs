using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B06 RID: 2822
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tank : CardModel
	{
		// Token: 0x06007889 RID: 30857 RVA: 0x0029A07B File Offset: 0x0029827B
		public Tank()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x170020D4 RID: 8404
		// (get) Token: 0x0600788A RID: 30858 RVA: 0x0029A088 File Offset: 0x00298288
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x170020D5 RID: 8405
		// (get) Token: 0x0600788B RID: 30859 RVA: 0x0029A08C File Offset: 0x0029828C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("DamageIncrease", 1.5m),
					new DynamicVar("DamageDecrease", 0.5m)
				});
			}
		}

		// Token: 0x0600788C RID: 30860 RVA: 0x0029A0D4 File Offset: 0x002982D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<TankPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x0600788D RID: 30861 RVA: 0x0029A11F File Offset: 0x0029831F
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x040028CA RID: 10442
		private const string _damageIncreaseKey = "DamageIncrease";

		// Token: 0x040028CB RID: 10443
		private const string _damageDecreaseKey = "DamageDecrease";
	}
}
