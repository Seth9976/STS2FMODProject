using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x02000721 RID: 1825
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToricToughnessPower : PowerModel
	{
		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06005C51 RID: 23633 RVA: 0x00256117 File Offset: 0x00254317
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06005C52 RID: 23634 RVA: 0x0025611A File Offset: 0x0025431A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06005C53 RID: 23635 RVA: 0x0025611D File Offset: 0x0025431D
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06005C54 RID: 23636 RVA: 0x0025612F File Offset: 0x0025432F
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06005C55 RID: 23637 RVA: 0x00256132 File Offset: 0x00254332
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(0m, ValueProp.Unpowered));
			}
		}

		/// <summary>
		/// Set the amount of block that this instance of Toric Toughness will deal.
		/// This is necessary because Amount is used to track the number of turns left.
		/// </summary>
		/// <param name="block">Amount of block to gain</param>
		// Token: 0x06005C56 RID: 23638 RVA: 0x00256144 File Offset: 0x00254344
		public void SetBlock(decimal block)
		{
			base.AssertMutable();
			base.DynamicVars.Block.BaseValue = block;
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x00256160 File Offset: 0x00254360
		public override async Task AfterBlockCleared(Creature creature)
		{
			if (creature == base.Owner)
			{
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner, base.DynamicVars.Block, null, false);
				await PowerCmd.Decrement(this);
			}
		}
	}
}
