using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Enchantments
{
	// Token: 0x020008D7 RID: 2263
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Inky : EnchantmentModel
	{
		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x06006C66 RID: 27750 RVA: 0x00281D77 File Offset: 0x0027FF77
		public override bool HasExtraCardText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x06006C67 RID: 27751 RVA: 0x00281D7A File Offset: 0x0027FF7A
		public override bool ShowAmount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x06006C68 RID: 27752 RVA: 0x00281D7D File Offset: 0x0027FF7D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(1m, ValueProp.Move),
					new PowerVar<WeakPower>(1m)
				});
			}
		}

		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x06006C69 RID: 27753 RVA: 0x00281DA8 File Offset: 0x0027FFA8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x00281DC8 File Offset: 0x0027FFC8
		public override async Task OnPlay(PlayerChoiceContext choiceContext, [Nullable(2)] CardPlay cardPlay)
		{
			await PowerCmd.Apply<WeakPower>(choiceContext, (base.Card.TargetType == TargetType.AllEnemies) ? base.Card.CombatState.HittableEnemies : new <>z__ReadOnlySingleElementList<Creature>(cardPlay.Target), base.DynamicVars.Weak.BaseValue, base.Card.Owner.Creature, base.Card, false);
		}

		// Token: 0x06006C6B RID: 27755 RVA: 0x00281E1B File Offset: 0x0028001B
		public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
		{
			if (!props.IsPoweredAttack())
			{
				return 0m;
			}
			return base.DynamicVars.Damage.BaseValue;
		}
	}
}
