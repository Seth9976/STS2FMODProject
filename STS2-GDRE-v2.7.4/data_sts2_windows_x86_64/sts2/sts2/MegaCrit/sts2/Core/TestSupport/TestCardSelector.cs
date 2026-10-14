using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.TestSupport
{
	// Token: 0x02000083 RID: 131
	[NullableContext(1)]
	[Nullable(0)]
	public class TestCardSelector : ICardSelector
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000159EE File Offset: 0x00013BEE
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x000159F6 File Offset: 0x00013BF6
		[Nullable(2)]
		public TaskCompletionSource BlockedTcs
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00015A00 File Offset: 0x00013C00
		public void Cleanup()
		{
			this._cardsToSelectTaskQueue.Clear();
			this._indicesToSelectTaskQueue.Clear();
			this._shouldBlock = false;
			this.BlockedTcs = null;
			CancellationTokenSource blockCancelSource = this._blockCancelSource;
			if (blockCancelSource != null)
			{
				blockCancelSource.Cancel();
			}
			this._blockCancelSource = null;
			this._cardRewardSelectionDelegate = null;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00015A50 File Offset: 0x00013C50
		public TaskCompletionSource<IEnumerable<CardModel>> SetupForAsyncCardSelection()
		{
			TaskCompletionSource<IEnumerable<CardModel>> taskCompletionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
			this._cardsToSelectTaskQueue.Enqueue(taskCompletionSource);
			return taskCompletionSource;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00015A70 File Offset: 0x00013C70
		public TaskCompletionSource<IEnumerable<int>> SetupForAsyncIndexSelection()
		{
			TaskCompletionSource<IEnumerable<int>> taskCompletionSource = new TaskCompletionSource<IEnumerable<int>>();
			this._indicesToSelectTaskQueue.Enqueue(taskCompletionSource);
			return taskCompletionSource;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00015A90 File Offset: 0x00013C90
		public void PrepareToSelect(IEnumerable<CardModel> cards)
		{
			TaskCompletionSource<IEnumerable<CardModel>> taskCompletionSource = new TaskCompletionSource<IEnumerable<CardModel>>();
			taskCompletionSource.SetResult(cards);
			this._cardsToSelectTaskQueue.Enqueue(taskCompletionSource);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00015AB8 File Offset: 0x00013CB8
		public void PrepareToSelect(IEnumerable<int> indices)
		{
			TaskCompletionSource<IEnumerable<int>> taskCompletionSource = new TaskCompletionSource<IEnumerable<int>>();
			taskCompletionSource.SetResult(indices);
			this._indicesToSelectTaskQueue.Enqueue(taskCompletionSource);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00015ADE File Offset: 0x00013CDE
		public void PrepareToSelectCardReward(TestCardSelector.CardRewardSelectionDelegate del)
		{
			this._cardRewardSelectionDelegate = del;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00015AE8 File Offset: 0x00013CE8
		public void PrepareToSelectCardRewardAtIndex(int index)
		{
			this.PrepareToSelectCardReward((IReadOnlyList<CardCreationResult> c, IReadOnlyList<CardRewardAlternative> _) => new CardRewardSelection
			{
				card = c[index].Card
			});
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00015B14 File Offset: 0x00013D14
		public void PrepareToSelectCardRewardAlternativeAtIndex(int index)
		{
			this.PrepareToSelectCardReward((IReadOnlyList<CardCreationResult> _, IReadOnlyList<CardRewardAlternative> a) => new CardRewardSelection
			{
				alternative = a[index]
			});
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015B40 File Offset: 0x00013D40
		public CardRewardSelection GetSelectedCardReward(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives)
		{
			if (this._cardRewardSelectionDelegate != null)
			{
				return this._cardRewardSelectionDelegate(options, alternatives);
			}
			CardRewardSelection cardRewardSelection = default(CardRewardSelection);
			CardCreationResult cardCreationResult = options.FirstOrDefault<CardCreationResult>();
			cardRewardSelection.card = ((cardCreationResult != null) ? cardCreationResult.Card : null);
			return cardRewardSelection;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015B85 File Offset: 0x00013D85
		public Task PrepareToBlock()
		{
			this._shouldBlock = true;
			this.BlockedTcs = new TaskCompletionSource();
			this._blockCancelSource = new CancellationTokenSource();
			return this.BlockedTcs.Task;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00015BB0 File Offset: 0x00013DB0
		public async Task<IEnumerable<CardModel>> GetSelectedCards(IEnumerable<CardModel> options, int minSelect, int maxSelect)
		{
			if (this._shouldBlock)
			{
				this.BlockedTcs.SetResult();
				await Task.Delay(5000, this._blockCancelSource.Token);
				throw new InvalidOperationException("Test told us to block, but it did not finish within 5 seconds!");
			}
			IEnumerable<CardModel> enumerable2;
			if (this._cardsToSelectTaskQueue.Count > 0)
			{
				IEnumerable<CardModel> enumerable = await this._cardsToSelectTaskQueue.Dequeue().Task;
				if (enumerable.Any<CardModel>((CardModel c) => !options.Contains(c)))
				{
					throw new InvalidOperationException("Selected card missing from options.");
				}
				enumerable2 = enumerable;
			}
			else if (this._indicesToSelectTaskQueue.Count > 0)
			{
				enumerable2 = (await this._indicesToSelectTaskQueue.Dequeue().Task).Select<int, CardModel>(new Func<int, CardModel>(options.ElementAt<CardModel>));
			}
			else
			{
				enumerable2 = Array.Empty<CardModel>();
			}
			return enumerable2;
		}

		// Token: 0x0400015C RID: 348
		private readonly Queue<TaskCompletionSource<IEnumerable<CardModel>>> _cardsToSelectTaskQueue = new Queue<TaskCompletionSource<IEnumerable<CardModel>>>();

		// Token: 0x0400015D RID: 349
		private readonly Queue<TaskCompletionSource<IEnumerable<int>>> _indicesToSelectTaskQueue = new Queue<TaskCompletionSource<IEnumerable<int>>>();

		// Token: 0x0400015E RID: 350
		[Nullable(2)]
		private TestCardSelector.CardRewardSelectionDelegate _cardRewardSelectionDelegate;

		// Token: 0x0400015F RID: 351
		private bool _shouldBlock;

		// Token: 0x04000160 RID: 352
		[Nullable(2)]
		private CancellationTokenSource _blockCancelSource;

		// Token: 0x02000E0D RID: 3597
		// (Invoke) Token: 0x06008E57 RID: 36439
		[NullableContext(0)]
		public delegate CardRewardSelection CardRewardSelectionDelegate(IReadOnlyList<CardCreationResult> options, IReadOnlyList<CardRewardAlternative> alternatives);
	}
}
