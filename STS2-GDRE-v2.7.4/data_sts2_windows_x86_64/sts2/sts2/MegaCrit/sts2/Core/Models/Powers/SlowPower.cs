using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006F4 RID: 1780
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlowPower : PowerModel
	{
		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x00253D1B File Offset: 0x00251F1B
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x00253D1E File Offset: 0x00251F1E
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06005B38 RID: 23352 RVA: 0x00253D21 File Offset: 0x00251F21
		public override int DisplayAmount
		{
			get
			{
				return base.DynamicVars["SlowAmount"].IntValue * 10;
			}
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x00253D3B File Offset: 0x00251F3B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("SlowAmount", 0m));
			}
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x00253D54 File Offset: 0x00251F54
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			DynamicVar dynamicVar = base.DynamicVars["SlowAmount"];
			decimal baseValue = dynamicVar.BaseValue;
			dynamicVar.BaseValue = baseValue + 1m;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x00253D90 File Offset: 0x00251F90
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
			return 1m + 0.1m * base.DynamicVars["SlowAmount"].BaseValue;
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x00253DE8 File Offset: 0x00251FE8
		public override Task AfterModifyingDamageAmount([Nullable(2)] CardModel cardSource)
		{
			base.Flash();
			return Task.CompletedTask;
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x00253DF5 File Offset: 0x00251FF5
		public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner))
			{
				return Task.CompletedTask;
			}
			base.DynamicVars["SlowAmount"].BaseValue = 0m;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0400251B RID: 9499
		private const string _slowAmountKey = "SlowAmount";
	}
}
