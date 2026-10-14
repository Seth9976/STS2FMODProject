using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006A4 RID: 1700
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class KnockdownPower : PowerModel
	{
		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06005952 RID: 22866 RVA: 0x002504D9 File Offset: 0x0024E6D9
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06005953 RID: 22867 RVA: 0x002504DC File Offset: 0x0024E6DC
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06005954 RID: 22868 RVA: 0x002504DF File Offset: 0x0024E6DF
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06005955 RID: 22869 RVA: 0x002504E2 File Offset: 0x0024E6E2
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("Applier", ""));
			}
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x002504F8 File Offset: 0x0024E6F8
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, base.Applier.Player.NetId);
			return Task.CompletedTask;
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x00250548 File Offset: 0x0024E748
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
			if (dealer == base.Applier)
			{
				return 1m;
			}
			return base.Amount;
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x00250584 File Offset: 0x0024E784
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x04002505 RID: 9477
		private const string _applierTag = "Applier";
	}
}
