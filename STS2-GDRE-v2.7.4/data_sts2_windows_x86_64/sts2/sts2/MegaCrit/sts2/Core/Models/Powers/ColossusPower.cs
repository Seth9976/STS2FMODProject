using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000645 RID: 1605
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColossusPower : PowerModel
	{
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x0600573A RID: 22330 RVA: 0x0024C7B3 File Offset: 0x0024A9B3
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600573B RID: 22331 RVA: 0x0024C7B6 File Offset: 0x0024A9B6
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600573C RID: 22332 RVA: 0x0024C7B9 File Offset: 0x0024A9B9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 0.5m));
			}
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x0600573D RID: 22333 RVA: 0x0024C7D4 File Offset: 0x0024A9D4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>(null));
			}
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x0024C7F4 File Offset: 0x0024A9F4
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (target != base.Owner)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			if (dealer == null)
			{
				return 1m;
			}
			if (!dealer.HasPower<VulnerablePower>())
			{
				return 1m;
			}
			return base.DynamicVars["DamageDecrease"].BaseValue;
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x0024C84C File Offset: 0x0024AA4C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Decrement(this);
			}
		}

		// Token: 0x040024F3 RID: 9459
		private const string _damageDecreaseKey = "DamageDecrease";
	}
}
