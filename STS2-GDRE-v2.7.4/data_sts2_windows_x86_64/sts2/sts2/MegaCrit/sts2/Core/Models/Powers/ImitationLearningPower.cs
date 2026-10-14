using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Powers
{
	// Token: 0x0200069A RID: 1690
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImitationLearningPower : PowerModel
	{
		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06005911 RID: 22801 RVA: 0x0024FC57 File Offset: 0x0024DE57
		public override PowerType Type
		{
			get
			{
				return PowerType.Buff;
			}
		}

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06005912 RID: 22802 RVA: 0x0024FC5A File Offset: 0x0024DE5A
		public override PowerStackType StackType
		{
			get
			{
				return PowerStackType.Counter;
			}
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06005913 RID: 22803 RVA: 0x0024FC5D File Offset: 0x0024DE5D
		public override PowerInstanceType InstanceType
		{
			get
			{
				return PowerInstanceType.Instanced;
			}
		}

		/// <summary>
		/// The player whose powers we are copying.
		/// This is different than <see cref="P:MegaCrit.Sts2.Core.Models.PowerModel.Target" />, which is used specifically for the case in which an
		/// enemy has multiple of the same power targeting different players. If this kind of concept becomes common, we
		/// can make it first class in PowerModel, or combine it with Target in some way.
		/// </summary>
		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06005914 RID: 22804 RVA: 0x0024FC60 File Offset: 0x0024DE60
		// (set) Token: 0x06005915 RID: 22805 RVA: 0x0024FC74 File Offset: 0x0024DE74
		public Player PlayerTarget
		{
			get
			{
				Player playerTarget = this._playerTarget;
				if (playerTarget == null)
				{
					throw new InvalidOperationException();
				}
				return playerTarget;
			}
			set
			{
				base.AssertMutable();
				this._playerTarget = value;
				((StringVar)base.DynamicVars["TargetPlayer"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, this._playerTarget.NetId);
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06005916 RID: 22806 RVA: 0x0024FCC7 File Offset: 0x0024DEC7
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new StringVar("TargetPlayer", ""));
			}
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x0024FCDD File Offset: 0x0024DEDD
		protected override object InitInternalData()
		{
			return new ImitationLearningPower.Data();
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x0024FCE4 File Offset: 0x0024DEE4
		public override Task BeforeCardPlayed(CardPlay cardPlay)
		{
			if (this._playerTarget == null)
			{
				throw new InvalidOperationException("ImitationLearningPower applied without a player target!");
			}
			if (cardPlay.Card.Owner != this._playerTarget)
			{
				return Task.CompletedTask;
			}
			if (cardPlay.Card.Type != CardType.Power)
			{
				return Task.CompletedTask;
			}
			CardModel cardModel = cardPlay.Card.CreateCloneForPlayer(base.Owner.Player);
			base.GetInternalData<ImitationLearningPower.Data>().cardsAndClones.Add(cardPlay.Card, cardModel);
			return Task.CompletedTask;
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x0024FD64 File Offset: 0x0024DF64
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			CardModel clone;
			if (base.GetInternalData<ImitationLearningPower.Data>().cardsAndClones.TryGetValue(cardPlay.Card, out clone))
			{
				base.Flash();
				await PowerCmd.Decrement(this);
				await CardCmd.AutoPlay(choiceContext, clone, null, AutoPlayType.Default, false, false);
			}
		}

		// Token: 0x04002501 RID: 9473
		private const string _targetPlayerKey = "TargetPlayer";

		// Token: 0x04002502 RID: 9474
		[Nullable(2)]
		private Player _playerTarget;

		// Token: 0x02001BF3 RID: 7155
		[NullableContext(0)]
		private class Data
		{
			// Token: 0x04006F7E RID: 28542
			[Nullable(1)]
			public readonly Dictionary<CardModel, CardModel> cardsAndClones = new Dictionary<CardModel, CardModel>();
		}
	}
}
