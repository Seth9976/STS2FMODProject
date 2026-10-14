using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
	// Token: 0x02000688 RID: 1672
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GuardedPower : PowerModel
	{
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x060058A6 RID: 22694 RVA: 0x0024F0AF File Offset: 0x0024D2AF
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x060058A7 RID: 22695 RVA: 0x0024F0B2 File Offset: 0x0024D2B2
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x060058A8 RID: 22696 RVA: 0x0024F0B5 File Offset: 0x0024D2B5
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x060058A9 RID: 22697 RVA: 0x0024F0B8 File Offset: 0x0024D2B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("Applier", ""),
					new DynamicVar("DamageDecrease", 0.5m)
				});
			}
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x0024F0FC File Offset: 0x0024D2FC
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, base.Applier.Player.NetId);
			return Task.CompletedTask;
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x0024F14C File Offset: 0x0024D34C
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
		{
			if (!wasRemovalPrevented)
			{
				if (creature == base.Applier)
				{
					await PowerCmd.Remove(this);
				}
			}
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x0024F19F File Offset: 0x0024D39F
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
			return base.DynamicVars["DamageDecrease"].BaseValue;
		}

		// Token: 0x040024F9 RID: 9465
		private const string _damageDecrease = "DamageDecrease";

		// Token: 0x040024FA RID: 9466
		private const string _applierTag = "Applier";
	}
}
