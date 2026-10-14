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
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x020006EC RID: 1772
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShrinkPower : PowerModel
	{
		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06005AFB RID: 23291 RVA: 0x00253613 File Offset: 0x00251813
		public override PowerType Type
		{
			get
			{
				return PowerType.Debuff;
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06005AFC RID: 23292 RVA: 0x00253616 File Offset: 0x00251816
		public override PowerStackType StackType
		{
			get
			{
				if (!this.IsInfinite)
				{
					return PowerStackType.Counter;
				}
				return PowerStackType.Single;
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x00253623 File Offset: 0x00251823
		public override bool AllowNegative
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06005AFE RID: 23294 RVA: 0x00253626 File Offset: 0x00251826
		private bool IsInfinite
		{
			get
			{
				return base.Amount < 0;
			}
		}

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06005AFF RID: 23295 RVA: 0x00253631 File Offset: 0x00251831
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("DamageDecrease", 30m),
					new StringVar("ApplierName", "")
				});
			}
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x00253664 File Offset: 0x00251864
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterApplied(Creature applier, CardModel cardSource)
		{
			Vantom vantom = base.Owner.Monster as Vantom;
			if (vantom != null)
			{
				vantom.ScaleTo(0.5f, 0.75f);
			}
			else
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					NCreature creatureNode = instance.GetCreatureNode(base.Owner);
					if (creatureNode != null)
					{
						creatureNode.ScaleTo(0.5f, 0.75);
					}
				}
			}
			Creature applier2 = base.Applier;
			if (applier2 != null && applier2.IsMonster)
			{
				((StringVar)base.DynamicVars["ApplierName"]).StringValue = base.Applier.Monster.Title.GetFormattedText();
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x0025370C File Offset: 0x0025190C
		public override Task AfterRemoved(Creature oldOwner)
		{
			Vantom vantom = oldOwner.Monster as Vantom;
			if (vantom != null)
			{
				vantom.ScaleTo(1f, 0.75f);
			}
			else
			{
				NCombatRoom instance = NCombatRoom.Instance;
				if (instance != null)
				{
					NCreature creatureNode = instance.GetCreatureNode(oldOwner);
					if (creatureNode != null)
					{
						creatureNode.ScaleTo(1f, 0.75);
					}
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x0025376C File Offset: 0x0025196C
		public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!this.IsInfinite)
			{
				if (participants.Contains(base.Owner))
				{
					await PowerCmd.Decrement(this);
				}
			}
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x002537B8 File Offset: 0x002519B8
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

		// Token: 0x06005B04 RID: 23300 RVA: 0x0025380C File Offset: 0x00251A0C
		[NullableContext(2)]
		public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
		{
			if (base.Owner != dealer)
			{
				return 1m;
			}
			if (!props.IsPoweredAttack())
			{
				return 1m;
			}
			return (100m - base.DynamicVars["DamageDecrease"].BaseValue) / 100m;
		}

		// Token: 0x04002515 RID: 9493
		public const decimal damageDecrease = 30m;

		// Token: 0x04002516 RID: 9494
		private const string _damageDecreaseKey = "DamageDecrease";

		// Token: 0x04002517 RID: 9495
		private const string _applierNameKey = "ApplierName";
	}
}
