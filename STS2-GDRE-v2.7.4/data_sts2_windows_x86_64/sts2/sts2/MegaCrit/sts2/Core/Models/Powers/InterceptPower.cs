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
	// Token: 0x020006A0 RID: 1696
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InterceptPower : PowerModel
	{
		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06005939 RID: 22841 RVA: 0x0025015F File Offset: 0x0024E35F
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x0600593A RID: 22842 RVA: 0x00250162 File Offset: 0x0024E362
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Single;
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x0600593B RID: 22843 RVA: 0x00250165 File Offset: 0x0024E365
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("Covering", ""));
			}
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x0025017B File Offset: 0x0024E37B
		protected override object InitInternalData()
		{
			return new InterceptPower.Data();
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x00250184 File Offset: 0x0024E384
		public void AddCoveredCreature(Creature c)
		{
			List<Creature> coveredCreatures = base.GetInternalData<InterceptPower.Data>().coveredCreatures;
			if (!base.GetInternalData<InterceptPower.Data>().coveredCreatures.Contains(c))
			{
				coveredCreatures.Add(c);
			}
			StringVar stringVar = (StringVar)base.DynamicVars["Covering"];
			stringVar.StringValue = "";
			for (int i = 0; i < coveredCreatures.Count; i++)
			{
				StringVar stringVar2 = stringVar;
				stringVar2.StringValue += PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, coveredCreatures[i].Player.NetId);
				if (i == coveredCreatures.Count - 2)
				{
					StringVar stringVar3 = stringVar;
					stringVar3.StringValue += ", and ";
				}
				else if (i < coveredCreatures.Count - 2)
				{
					StringVar stringVar4 = stringVar;
					stringVar4.StringValue += ", ";
				}
			}
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x00250266 File Offset: 0x0024E466
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
			return base.GetInternalData<InterceptPower.Data>().coveredCreatures.Count + 1;
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x0025029C File Offset: 0x0024E49C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (side == CombatSide.Enemy)
			{
				await PowerCmd.Remove(this);
			}
		}

		// Token: 0x04002504 RID: 9476
		private const string _coveringKey = "Covering";

		// Token: 0x02001BFC RID: 7164
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006FAF RID: 28591
			[Nullable(1)]
			public readonly List<Creature> coveredCreatures = new List<Creature>();
		}
	}
}
