using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C27 RID: 3111
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayCardAction : GameAction
	{
		// Token: 0x17002326 RID: 8998
		// (get) Token: 0x06008070 RID: 32880 RVA: 0x002B8BA6 File Offset: 0x002B6DA6
		public override ulong OwnerId
		{
			get
			{
				return this.Player.NetId;
			}
		}

		// Token: 0x17002327 RID: 8999
		// (get) Token: 0x06008071 RID: 32881 RVA: 0x002B8BB3 File Offset: 0x002B6DB3
		public override GameActionType ActionType
		{
			get
			{
				return GameActionType.CombatPlayPhaseOnly;
			}
		}

		// Token: 0x17002328 RID: 9000
		// (get) Token: 0x06008072 RID: 32882 RVA: 0x002B8BB6 File Offset: 0x002B6DB6
		public Player Player { get; }

		// Token: 0x17002329 RID: 9001
		// (get) Token: 0x06008073 RID: 32883 RVA: 0x002B8BBE File Offset: 0x002B6DBE
		public NetCombatCard NetCombatCard { get; }

		// Token: 0x1700232A RID: 9002
		// (get) Token: 0x06008074 RID: 32884 RVA: 0x002B8BC6 File Offset: 0x002B6DC6
		public ModelId CardModelId { get; }

		// Token: 0x1700232B RID: 9003
		// (get) Token: 0x06008075 RID: 32885 RVA: 0x002B8BCE File Offset: 0x002B6DCE
		public uint? TargetId { get; }

		// Token: 0x1700232C RID: 9004
		// (get) Token: 0x06008076 RID: 32886 RVA: 0x002B8BD6 File Offset: 0x002B6DD6
		// (set) Token: 0x06008077 RID: 32887 RVA: 0x002B8BDE File Offset: 0x002B6DDE
		[Nullable(2)]
		public PlayerChoiceContext PlayerChoiceContext
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06008078 RID: 32888 RVA: 0x002B8BE8 File Offset: 0x002B6DE8
		public PlayCardAction(CardModel cardModel, [Nullable(2)] Creature target)
		{
			if (target != null && target.CombatId == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot target card against target ");
				defaultInterpolatedStringHandler.AppendFormatted<Creature>(target);
				defaultInterpolatedStringHandler.AppendLiteral(" with no combat ID!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Player = cardModel.Owner;
			this.NetCombatCard = NetCombatCard.FromModel(cardModel);
			this.CardModelId = cardModel.Id;
			this.TargetId = ((target != null) ? target.CombatId : null);
		}

		// Token: 0x06008079 RID: 32889 RVA: 0x002B8C83 File Offset: 0x002B6E83
		public PlayCardAction(Player player, NetCombatCard netCombatCard, ModelId cardModelId, uint? targetId)
		{
			this.Player = player;
			this.NetCombatCard = netCombatCard;
			this.CardModelId = cardModelId;
			this.TargetId = targetId;
		}

		/// <summary>
		/// Get the creature that this action is targeting.
		/// Null for un-targeted cards.
		/// </summary>
		// Token: 0x1700232D RID: 9005
		// (get) Token: 0x0600807A RID: 32890 RVA: 0x002B8CA8 File Offset: 0x002B6EA8
		[Nullable(2)]
		public Creature Target
		{
			[NullableContext(2)]
			get
			{
				ICombatState combatState = this.Player.Creature.CombatState;
				if (combatState == null)
				{
					return null;
				}
				return combatState.GetCreature(this.TargetId);
			}
		}

		// Token: 0x0600807B RID: 32891 RVA: 0x002B8CCC File Offset: 0x002B6ECC
		protected override async Task ExecuteAction()
		{
			this._card = this.NetCombatCard.ToCardModel();
			NCardPlayQueue instance = NCardPlayQueue.Instance;
			if (instance != null)
			{
				instance.UpdateCardBeforeExecution(this);
			}
			Creature creature = await this.Player.Creature.CombatState.GetCreatureAsync(this.TargetId, 10.0);
			Creature target = creature;
			CardPile pile = this._card.Pile;
			if (pile == null || pile.Type != PileType.Hand)
			{
				NCardPlayQueue instance2 = NCardPlayQueue.Instance;
				if (instance2 != null)
				{
					instance2.RemoveCardFromQueueForCancellation(this);
				}
			}
			else
			{
				bool flag = target == null;
				if (flag)
				{
					TargetType targetType = this._card.TargetType;
					flag = targetType == TargetType.AnyEnemy || targetType == TargetType.AnyAlly;
				}
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(104, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Attempted to play card ");
					defaultInterpolatedStringHandler.AppendFormatted<CardModel>(this._card);
					defaultInterpolatedStringHandler.AppendLiteral(" with TargetType of type 'Any', but no target was passed to the play card action!");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				UnplayableReason unplayableReason;
				AbstractModel abstractModel;
				if (!this._card.CanPlay(out unplayableReason, out abstractModel) || !this._card.IsValidTarget(target))
				{
					base.Cancel();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					string text;
					if (target == null)
					{
						text = "no target";
					}
					else
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
						defaultInterpolatedStringHandler.AppendLiteral("targeting ");
						defaultInterpolatedStringHandler.AppendFormatted(target.LogName);
						defaultInterpolatedStringHandler.AppendLiteral(" (index ");
						ICombatState combatState = this.Player.Creature.CombatState;
						defaultInterpolatedStringHandler.AppendFormatted<int?>((combatState != null) ? new int?(combatState.Creatures.IndexOf(target)) : null);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						text = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					string text2 = text;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Player ");
					defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._card.Owner.NetId);
					defaultInterpolatedStringHandler.AppendLiteral(" playing card ");
					defaultInterpolatedStringHandler.AppendFormatted(this._card.Id.Entry);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted(text2);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					ValueTuple<int, int> valueTuple = await this._card.SpendResources();
					int item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					ResourceInfo resourceInfo = new ResourceInfo
					{
						EnergySpent = item,
						EnergyValue = item,
						StarsSpent = item2,
						StarValue = item2
					};
					this.PlayerChoiceContext = new GameActionPlayerChoiceContext(this);
					await this._card.OnPlayWrapper(this.PlayerChoiceContext, target, false, resourceInfo, false);
				}
			}
		}

		/// <summary>
		/// We override this to handle the case where some external action (like showing the hand selection screen) needs to
		/// cancel queued card plays.
		/// </summary>
		// Token: 0x0600807C RID: 32892 RVA: 0x002B8D10 File Offset: 0x002B6F10
		protected override void CancelAction()
		{
			if (TestMode.IsOn && !RunManager.Instance.IsInProgress)
			{
				return;
			}
			if (this._card == null)
			{
				this._card = this.NetCombatCard.ToCardModelOrNull();
			}
			if (this._card != null)
			{
				NCardPlayQueue instance = NCardPlayQueue.Instance;
				if (instance != null)
				{
					instance.RemoveCardFromQueueForCancellation(this);
				}
				if (LocalContext.IsMe(this.Player))
				{
					NPlayerHand instance2 = NPlayerHand.Instance;
					if (instance2 == null)
					{
						return;
					}
					instance2.TryCancelCardPlay(this._card);
				}
			}
		}

		// Token: 0x0600807D RID: 32893 RVA: 0x002B8D88 File Offset: 0x002B6F88
		public override INetAction ToNetAction()
		{
			NetPlayCardAction netPlayCardAction = new NetPlayCardAction
			{
				card = this.NetCombatCard,
				modelId = this.CardModelId,
				targetId = this.TargetId
			};
			return netPlayCardAction;
		}

		// Token: 0x0600807E RID: 32894 RVA: 0x002B8DCC File Offset: 0x002B6FCC
		public override string ToString()
		{
			CardModel cardModel = this.NetCombatCard.ToCardModelOrNull();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 4);
			defaultInterpolatedStringHandler.AppendFormatted("PlayCardAction");
			defaultInterpolatedStringHandler.AppendLiteral(" card: ");
			defaultInterpolatedStringHandler.AppendFormatted<CardModel>(cardModel);
			defaultInterpolatedStringHandler.AppendLiteral(" index: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.NetCombatCard.CombatCardIndex);
			defaultInterpolatedStringHandler.AppendLiteral(" targetid: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint?>(this.TargetId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002B01 RID: 11009
		[Nullable(2)]
		private CardModel _card;
	}
}
