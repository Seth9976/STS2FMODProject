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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000650 RID: 1616
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CoveredPower : PowerModel
	{
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06005776 RID: 22390 RVA: 0x0024CE57 File Offset: 0x0024B057
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06005777 RID: 22391 RVA: 0x0024CE5A File Offset: 0x0024B05A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06005778 RID: 22392 RVA: 0x0024CE5D File Offset: 0x0024B05D
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06005779 RID: 22393 RVA: 0x0024CE60 File Offset: 0x0024B060
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("Applier", ""));
			}
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x0024CE78 File Offset: 0x0024B078
		[NullableContext(2)]
		[return: Nullable(1)]
		public override async Task AfterApplied(Creature applier, CardModel cardSource)
		{
			((StringVar)base.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, base.Applier.Player.NetId);
			InterceptPower interceptPower = base.Applier.GetPower<InterceptPower>();
			if (interceptPower == null)
			{
				InterceptPower interceptPower2 = await PowerCmd.Apply<InterceptPower>(new ThrowingPlayerChoiceContext(), base.Applier, 1m, base.Owner, null, false);
				interceptPower = interceptPower2;
			}
			interceptPower.AddCoveredCreature(base.Owner);
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x0024CEBC File Offset: 0x0024B0BC
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

		// Token: 0x0600577C RID: 22396 RVA: 0x0024CF0F File Offset: 0x0024B10F
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
			return 0m;
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x0024CF34 File Offset: 0x0024B134
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x040024F5 RID: 9461
		private const string _applierTag = "Applier";
	}
}
