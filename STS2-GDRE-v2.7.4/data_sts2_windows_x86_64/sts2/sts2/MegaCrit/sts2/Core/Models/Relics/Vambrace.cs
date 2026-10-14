using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x0200060C RID: 1548
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class Vambrace : RelicModel
	{
		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060055F1 RID: 22001 RVA: 0x0024993B File Offset: 0x00247B3B
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Uncommon;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060055F2 RID: 22002 RVA: 0x0024993E File Offset: 0x00247B3E
		// (set) Token: 0x060055F3 RID: 22003 RVA: 0x00249946 File Offset: 0x00247B46
		private CardModel TriggeringCard
		{
			get
			{
				return this._triggeringCard;
			}
			set
			{
				base.AssertMutable();
				this._triggeringCard = value;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060055F4 RID: 22004 RVA: 0x00249955 File Offset: 0x00247B55
		// (set) Token: 0x060055F5 RID: 22005 RVA: 0x0024995D File Offset: 0x00247B5D
		private bool BlockGainedThisCombat
		{
			get
			{
				return this._blockGainedThisCombat;
			}
			set
			{
				base.AssertMutable();
				this._blockGainedThisCombat = value;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0024996C File Offset: 0x00247B6C
		[Nullable(1)]
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x0024997E File Offset: 0x00247B7E
		[NullableContext(1)]
		public override Task BeforeCombatStart()
		{
			this.TriggeringCard = null;
			this.BlockGainedThisCombat = false;
			base.Status = RelicStatus.Active;
			return Task.CompletedTask;
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x0024999C File Offset: 0x00247B9C
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsCardOrMonsterMove())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			if (this.TriggeringCard != null && this.TriggeringCard != cardSource)
			{
				return 1m;
			}
			if (cardSource.Owner != base.Owner)
			{
				return 1m;
			}
			if (this.BlockGainedThisCombat)
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x00249A02 File Offset: 0x00247C02
		[return: Nullable(1)]
		public override Task AfterModifyingBlockAmount(decimal modifiedAmount, CardModel cardSource, CardPlay cardPlay)
		{
			if (modifiedAmount <= 0m)
			{
				return Task.CompletedTask;
			}
			if (cardSource == null)
			{
				return Task.CompletedTask;
			}
			base.Flash();
			base.Status = RelicStatus.Normal;
			this.TriggeringCard = cardSource;
			return Task.CompletedTask;
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x00249A3C File Offset: 0x00247C3C
		[NullableContext(1)]
		public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (cardPlay.Card.Owner != base.Owner)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card != this.TriggeringCard)
			{
				return Task.CompletedTask;
			}
			if (this.BlockGainedThisCombat)
			{
				return Task.CompletedTask;
			}
			this.BlockGainedThisCombat = true;
			return Task.CompletedTask;
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x00249A90 File Offset: 0x00247C90
		[NullableContext(1)]
		public override Task AfterCombatEnd(CombatRoom room)
		{
			this.TriggeringCard = null;
			this.BlockGainedThisCombat = false;
			base.Status = RelicStatus.Normal;
			return Task.CompletedTask;
		}

		// Token: 0x040024E0 RID: 9440
		private CardModel _triggeringCard;

		// Token: 0x040024E1 RID: 9441
		private bool _blockGainedThisCombat;
	}
}
