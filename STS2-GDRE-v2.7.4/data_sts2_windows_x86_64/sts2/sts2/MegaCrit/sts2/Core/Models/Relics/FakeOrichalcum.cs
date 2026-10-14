using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x02000544 RID: 1348
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FakeOrichalcum : RelicModel
	{
		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060050A9 RID: 20649 RVA: 0x0023F746 File Offset: 0x0023D946
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Event;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060050AA RID: 20650 RVA: 0x0023F749 File Offset: 0x0023D949
		public override int MerchantCost
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060050AB RID: 20651 RVA: 0x0023F74D File Offset: 0x0023D94D
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(3m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060050AC RID: 20652 RVA: 0x0023F760 File Offset: 0x0023D960
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060050AD RID: 20653 RVA: 0x0023F772 File Offset: 0x0023D972
		// (set) Token: 0x060050AE RID: 20654 RVA: 0x0023F77A File Offset: 0x0023D97A
		private bool ShouldTrigger
		{
			get
			{
				return this._shouldTrigger;
			}
			set
			{
				base.AssertMutable();
				this._shouldTrigger = value;
			}
		}

		/// <summary>
		/// This uses the _very early_ hook because it needs to check the player's block before <see cref="T:MegaCrit.Sts2.Core.Models.Powers.PlatingPower" />
		/// triggers (otherwise, <see cref="T:MegaCrit.Sts2.Core.Models.Powers.PlatingPower" /> will prevent this relic's block gain), and
		/// <see cref="T:MegaCrit.Sts2.Core.Models.Powers.PlatingPower" /> in turn needs to run early so it can give you block before you take damage from
		/// another end-of-turn effect.
		/// </summary>
		// Token: 0x060050AF RID: 20655 RVA: 0x0023F789 File Offset: 0x0023D989
		public override Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			if (base.Owner.Creature.Block > 0)
			{
				return Task.CompletedTask;
			}
			this.ShouldTrigger = true;
			return Task.CompletedTask;
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x0023F7CC File Offset: 0x0023D9CC
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (this.ShouldTrigger)
			{
				this.ShouldTrigger = false;
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
			}
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x0023F80F File Offset: 0x0023DA0F
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.ShouldTrigger = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002445 RID: 9285
		private bool _shouldTrigger;
	}
}
