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
	// Token: 0x0200059F RID: 1439
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Orichalcum : RelicModel
	{
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x060052FD RID: 21245 RVA: 0x00243E3B File Offset: 0x0024203B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x060052FE RID: 21246 RVA: 0x00243E3E File Offset: 0x0024203E
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Unpowered));
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x060052FF RID: 21247 RVA: 0x00243E51 File Offset: 0x00242051
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x00243E63 File Offset: 0x00242063
		// (set) Token: 0x06005301 RID: 21249 RVA: 0x00243E6B File Offset: 0x0024206B
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
		// Token: 0x06005302 RID: 21250 RVA: 0x00243E7A File Offset: 0x0024207A
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

		// Token: 0x06005303 RID: 21251 RVA: 0x00243EBC File Offset: 0x002420BC
		public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
		{
			if (this.ShouldTrigger)
			{
				this.ShouldTrigger = false;
				base.Flash();
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, null, false);
			}
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x00243EFF File Offset: 0x002420FF
		public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (!participants.Contains(base.Owner.Creature))
			{
				return Task.CompletedTask;
			}
			this.ShouldTrigger = false;
			return Task.CompletedTask;
		}

		// Token: 0x04002482 RID: 9346
		private bool _shouldTrigger;
	}
}
