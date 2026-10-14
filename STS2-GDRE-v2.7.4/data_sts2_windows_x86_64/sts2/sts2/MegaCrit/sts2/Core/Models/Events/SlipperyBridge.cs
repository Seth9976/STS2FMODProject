using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000850 RID: 2128
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SlipperyBridge : EventModel
	{
		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x060068BF RID: 26815 RVA: 0x0027917C File Offset: 0x0027737C
		// (set) Token: 0x060068C0 RID: 26816 RVA: 0x00279184 File Offset: 0x00277384
		private int NumberOfHoldOns
		{
			get
			{
				return this._numberOfHoldOns;
			}
			set
			{
				base.AssertMutable();
				this._numberOfHoldOns = value;
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x060068C1 RID: 26817 RVA: 0x00279193 File Offset: 0x00277393
		// (set) Token: 0x060068C2 RID: 26818 RVA: 0x0027919B File Offset: 0x0027739B
		[Nullable(2)]
		private CardModel RandomCardToLose
		{
			[NullableContext(2)]
			get
			{
				return this._randomCardToLose;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._randomCardToLose = value;
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x060068C3 RID: 26819 RVA: 0x002791AA File Offset: 0x002773AA
		// (set) Token: 0x060068C4 RID: 26820 RVA: 0x002791B2 File Offset: 0x002773B2
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<CardModel> SkippedRemovals
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._skippedRemovals;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				base.AssertMutable();
				this._skippedRemovals = value;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x060068C5 RID: 26821 RVA: 0x002791C1 File Offset: 0x002773C1
		private int CurrentHpLoss
		{
			get
			{
				return 3 + this.NumberOfHoldOns;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x060068C6 RID: 26822 RVA: 0x002791CB File Offset: 0x002773CB
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new StringVar("RandomCard", ""),
					new DynamicVar("HpLoss", this.CurrentHpLoss)
				});
			}
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x00279202 File Offset: 0x00277402
		public override bool IsAllowed(IRunState runState)
		{
			if (runState.TotalFloor > 6)
			{
				return runState.Players.All<Player>((Player p) => p.Deck.Cards.Any<CardModel>((CardModel c) => c.IsRemovable));
			}
			return false;
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x0027923C File Offset: 0x0027743C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			this.GetNewRandomCard();
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Overcome), "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME", new IHoverTip[] { HoverTipFactory.FromCard(this.RandomCardToLose, false) }),
				new EventOption(this, new Func<Task>(this.HoldOn), "SLIPPERY_BRIDGE.pages.INITIAL.options.HOLD_ON_0", Array.Empty<IHoverTip>()).ThatDoesDamage(this.CurrentHpLoss)
			});
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x002792B8 File Offset: 0x002774B8
		public override void OnRoomEnter()
		{
			NEventRoom instance = NEventRoom.Instance;
			if (instance == null)
			{
				return;
			}
			Control vfxContainer = instance.VfxContainer;
			if (vfxContainer == null)
			{
				return;
			}
			vfxContainer.AddChildSafely(NRainVfx.Create());
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x002792D8 File Offset: 0x002774D8
		private void GetNewRandomCard()
		{
			List<CardModel> list;
			if (this.RandomCardToLose == null)
			{
				list = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.Rarity != CardRarity.Basic).ToList<CardModel>();
			}
			else
			{
				if (this.SkippedRemovals == null)
				{
					this.SkippedRemovals = new HashSet<CardModel>();
				}
				this.SkippedRemovals.Add(this.RandomCardToLose);
				list = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.GetType() != this.RandomCardToLose.GetType()).ToList<CardModel>();
			}
			list.RemoveAll(delegate(CardModel c)
			{
				if (c.IsRemovable)
				{
					HashSet<CardModel> skippedRemovals = this.SkippedRemovals;
					return skippedRemovals != null && skippedRemovals.Contains(c);
				}
				return true;
			});
			if (list.Count == 0)
			{
				list = base.Owner.Deck.Cards.Where<CardModel>((CardModel c) => c.IsRemovable).ToList<CardModel>();
			}
			this.RandomCardToLose = base.Rng.NextItem<CardModel>(list);
			StringVar stringVar = (StringVar)base.DynamicVars["RandomCard"];
			stringVar.StringValue = this.RandomCardToLose.Title;
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x00279408 File Offset: 0x00277608
		private async Task Overcome()
		{
			await CardPileCmd.RemoveFromDeck(this.RandomCardToLose, true);
			base.SetEventFinished(base.L10NLookup("SLIPPERY_BRIDGE.pages.OVERCOME.description"));
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x0027944C File Offset: 0x0027764C
		private async Task HoldOn()
		{
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, this.CurrentHpLoss, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			this.NumberOfHoldOns++;
			base.DynamicVars["HpLoss"].BaseValue = this.CurrentHpLoss;
			this.GetNewRandomCard();
			string holdOnSuffix = this.GetHoldOnSuffix(this.NumberOfHoldOns - 1);
			string holdOnSuffix2 = this.GetHoldOnSuffix(this.NumberOfHoldOns);
			string text = "SLIPPERY_BRIDGE.pages.HOLD_ON_" + holdOnSuffix + ".options.HOLD_ON_" + holdOnSuffix2;
			this.SetEventState(base.L10NLookup("SLIPPERY_BRIDGE.pages.HOLD_ON_" + holdOnSuffix + ".description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Overcome), "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME", new IHoverTip[] { HoverTipFactory.FromCard(this.RandomCardToLose, false) }),
				new EventOption(this, new Func<Task>(this.HoldOn), text, Array.Empty<IHoverTip>()).ThatDoesDamage(this.CurrentHpLoss)
			}));
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x0027948F File Offset: 0x0027768F
		private string GetHoldOnSuffix(int holdOnNumber)
		{
			if (holdOnNumber >= 7)
			{
				return "LOOP";
			}
			return holdOnNumber.ToString();
		}

		// Token: 0x040027B0 RID: 10160
		private const string _randomCardKey = "RandomCard";

		// Token: 0x040027B1 RID: 10161
		private const string _hpLossKey = "HpLoss";

		// Token: 0x040027B2 RID: 10162
		private const string _overcomeLocKey = "SLIPPERY_BRIDGE.pages.INITIAL.options.OVERCOME";

		// Token: 0x040027B3 RID: 10163
		private const int _initialHpLoss = 3;

		// Token: 0x040027B4 RID: 10164
		private int _numberOfHoldOns;

		// Token: 0x040027B5 RID: 10165
		[Nullable(2)]
		private CardModel _randomCardToLose;

		// Token: 0x040027B6 RID: 10166
		[Nullable(new byte[] { 2, 1 })]
		private HashSet<CardModel> _skippedRemovals;
	}
}
