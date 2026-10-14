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
	// Token: 0x02000677 RID: 1655
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlankingPower : PowerModel
	{
		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x0600584C RID: 22604 RVA: 0x0024E637 File Offset: 0x0024C837
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x0600584D RID: 22605 RVA: 0x0024E63A File Offset: 0x0024C83A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x0600584E RID: 22606 RVA: 0x0024E63D File Offset: 0x0024C83D
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x0600584F RID: 22607 RVA: 0x0024E640 File Offset: 0x0024C840
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("Applier", ""));
			}
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x0024E658 File Offset: 0x0024C858
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, base.Applier.Player.NetId);
			return Task.CompletedTask;
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x0024E6A8 File Offset: 0x0024C8A8
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

		// Token: 0x06005852 RID: 22610 RVA: 0x0024E6E4 File Offset: 0x0024C8E4
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (participants.Contains(base.Owner))
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x040024F7 RID: 9463
		private const string _applierTagKey = "Applier";
	}
}
