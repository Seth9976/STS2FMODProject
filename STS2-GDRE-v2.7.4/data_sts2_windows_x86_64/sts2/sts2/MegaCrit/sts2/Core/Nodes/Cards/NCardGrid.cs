using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	// Token: 0x02000415 RID: 1045
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Cards/NCardGrid.cs")]
	public class NCardGrid : Control
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x00201DA4 File Offset: 0x001FFFA4
		private Dictionary<SortingOrders, Func<CardModel, CardModel, int>> SortingAlgorithms
		{
			get
			{
				Dictionary<SortingOrders, Func<CardModel, CardModel, int>> dictionary;
				if ((dictionary = this._sortingAlgorithms) == null)
				{
					Dictionary<SortingOrders, Func<CardModel, CardModel, int>> dictionary2 = new Dictionary<SortingOrders, Func<CardModel, CardModel, int>>();
					dictionary2.Add(SortingOrders.RarityAscending, (CardModel a, CardModel b) => this.GetCardRarityComparisonValue(a).CompareTo(this.GetCardRarityComparisonValue(b)));
					dictionary2.Add(SortingOrders.CostAscending, (CardModel a, CardModel b) => a.EnergyCost.GetResolved().CompareTo(b.EnergyCost.GetResolved()));
					dictionary2.Add(SortingOrders.TypeAscending, (CardModel a, CardModel b) => a.Type.CompareTo(b.Type));
					dictionary2.Add(SortingOrders.AlphabetAscending, (CardModel a, CardModel b) => string.Compare(a.Title, b.Title, LocManager.Instance.CultureInfo, CompareOptions.None));
					dictionary2.Add(SortingOrders.RarityDescending, (CardModel a, CardModel b) => -this.GetCardRarityComparisonValue(a).CompareTo(this.GetCardRarityComparisonValue(b)));
					dictionary2.Add(SortingOrders.CostDescending, (CardModel a, CardModel b) => -a.EnergyCost.GetResolved().CompareTo(b.EnergyCost.GetResolved()));
					dictionary2.Add(SortingOrders.TypeDescending, (CardModel a, CardModel b) => -a.Type.CompareTo(b.Type));
					dictionary2.Add(SortingOrders.AlphabetDescending, (CardModel a, CardModel b) => -string.Compare(a.Title, b.Title, LocManager.Instance.CultureInfo, CompareOptions.None));
					dictionary2.Add(SortingOrders.Ascending, (CardModel a, CardModel b) => this._cards.IndexOf(a).CompareTo(this._cards.IndexOf(b)));
					dictionary2.Add(SortingOrders.Descending, (CardModel a, CardModel b) => -this._cards.IndexOf(a).CompareTo(this._cards.IndexOf(b)));
					Dictionary<SortingOrders, Func<CardModel, CardModel, int>> dictionary3 = dictionary2;
					this._sortingAlgorithms = dictionary2;
					dictionary = dictionary3;
				}
				return dictionary;
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00201F00 File Offset: 0x00200100
		private int CompareCardVisibility(CardModel a, CardModel b)
		{
			bool flag = this.GetCardVisibility(a) == ModelVisibility.Locked;
			bool flag2 = this.GetCardVisibility(b) == ModelVisibility.Locked;
			return flag.CompareTo(flag2);
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00201F2C File Offset: 0x0020012C
		private int GetCardRarityComparisonValue(CardModel a)
		{
			if (a.Rarity <= CardRarity.Ancient)
			{
				return (int)a.Rarity;
			}
			int num;
			switch (a.Rarity)
			{
			case CardRarity.Event:
				num = 8;
				break;
			case CardRarity.Token:
				num = 10;
				break;
			case CardRarity.Status:
				num = 6;
				break;
			case CardRarity.Curse:
				num = 7;
				break;
			case CardRarity.Quest:
				num = 9;
				break;
			default:
				throw new ArgumentOutOfRangeException("a", a, null);
			}
			return num;
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x0600422D RID: 16941 RVA: 0x00201F92 File Offset: 0x00200192
		private bool CanScroll
		{
			get
			{
				return this._scrollingEnabled && base.Visible;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x00201FA4 File Offset: 0x002001A4
		// (set) Token: 0x0600422F RID: 16943 RVA: 0x00201FAC File Offset: 0x002001AC
		private int DisplayedRows { get; set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x00201FB5 File Offset: 0x002001B5
		protected int Columns
		{
			get
			{
				return (int)((this._scrollContainer.Size.X + this.CardPadding) / (this._cardSize.X + this.CardPadding));
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06004231 RID: 16945 RVA: 0x00201FE2 File Offset: 0x002001E2
		protected float CardPadding
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x00201FE9 File Offset: 0x002001E9
		protected virtual bool IsCardLibrary
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06004233 RID: 16947 RVA: 0x00201FEC File Offset: 0x002001EC
		private float ScrollLimitBottom
		{
			get
			{
				if (base.Size.Y <= this._scrollContainer.Size.Y)
				{
					return base.Size.Y - this._scrollContainer.Size.Y;
				}
				return (base.Size.Y - this._scrollContainer.Size.Y) * 0.5f;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x00202058 File Offset: 0x00200258
		protected float ScrollLimitTop
		{
			get
			{
				if (base.Size.Y <= this._scrollContainer.Size.Y || !this.CenterGrid)
				{
					return 0f;
				}
				return (base.Size.Y - this._scrollContainer.Size.Y) * 0.5f;
			}
		}

		/// <summary>
		/// Returns the currently displayed cards.
		/// Since the grid only displays chunks of cards at a time, remember that this may not hold all the cards in the grid.
		/// </summary>
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06004235 RID: 16949 RVA: 0x002020B2 File Offset: 0x002002B2
		public IEnumerable<NGridCardHolder> CurrentlyDisplayedCardHolders
		{
			get
			{
				return this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> r) => r);
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x002020DE File Offset: 0x002002DE
		public IEnumerable<CardModel> CurrentlyDisplayedCards
		{
			get
			{
				return this.CurrentlyDisplayedCardHolders.Select<NGridCardHolder, CardModel>((NGridCardHolder h) => h.CardModel);
			}
		}

		/// <summary>
		/// True when the card library is animating or has animated out all cards that were displayed.
		/// </summary>
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06004237 RID: 16951 RVA: 0x0020210C File Offset: 0x0020030C
		public bool IsAnimatingOut
		{
			get
			{
				Task animatingOutTask = this._animatingOutTask;
				return animatingOutTask != null && !animatingOutTask.IsCompleted;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x0020212E File Offset: 0x0020032E
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x00202138 File Offset: 0x00200338
		public bool IsShowingUpgrades
		{
			get
			{
				return this._isShowingUpgrades;
			}
			set
			{
				this._isShowingUpgrades = value;
				foreach (List<NGridCardHolder> list in this._cardRows)
				{
					foreach (NGridCardHolder ngridCardHolder in list)
					{
						if ((this._isShowingUpgrades || ngridCardHolder.CardModel.CanonicalInstance.IsUpgradable) && (!this._isShowingUpgrades || ngridCardHolder.CardModel.IsUpgradable))
						{
							ngridCardHolder.SetIsPreviewingUpgrade(this._isShowingUpgrades);
						}
					}
				}
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x002021FC File Offset: 0x002003FC
		// (set) Token: 0x0600423B RID: 16955 RVA: 0x00202204 File Offset: 0x00200404
		public int YOffset { get; set; }

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x0020220D File Offset: 0x0020040D
		protected virtual bool CenterGrid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00202210 File Offset: 0x00200410
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCardGrid))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00202268 File Offset: 0x00200468
		protected virtual void ConnectSignals()
		{
			this._scrollContainer = base.GetNode<Control>("%ScrollContainer");
			this._scrollbar = base.GetNode<NScrollbar>("Scrollbar");
			this._cardSize = NCard.defaultSize * NCardHolder.smallScale;
			this._scrollContainer.Connect(CanvasItem.SignalName.ItemRectChanged, Callable.From(new Action(this.UpdateScrollLimitBottom)), 0U);
			this._scrollbar.Visible = false;
			this._scrollbar.Connect(NScrollbar.SignalName.MousePressed, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = true;
			}), 0U);
			this._scrollbar.Connect(NScrollbar.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
			{
				this._scrollbarPressed = false;
			}), 0U);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x0020232B File Offset: 0x0020052B
		public override void _EnterTree()
		{
			base._EnterTree();
			this._cts = new CancellationTokenSource();
			base.GetViewport().Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)), 0U);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00202364 File Offset: 0x00200564
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cts.Cancel();
			CancellationTokenSource setCardsCancellation = this._setCardsCancellation;
			if (setCardsCancellation != null)
			{
				setCardsCancellation.Cancel();
			}
			base.GetViewport().Disconnect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>(new Action<Control>(this.ProcessGuiFocus)));
			foreach (List<NGridCardHolder> list in this._cardRows)
			{
				foreach (NGridCardHolder ngridCardHolder in list)
				{
					ngridCardHolder.QueueFreeSafely();
				}
			}
			this._cardRows.Clear();
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0020243C File Offset: 0x0020063C
		private void UpdateScrollLimitBottom()
		{
			float num = base.Size.Y + 320f;
			this._scrollbar.Visible = this._scrollContainer.Size.Y > num && this.CanScroll;
			this._scrollbar.MouseFilter = ((this._scrollContainer.Size.Y > num && this.CanScroll) ? 0L : 2L);
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x002024AE File Offset: 0x002006AE
		public override void _GuiInput(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			this.ProcessMouseEvent(inputEvent);
			this.ProcessScrollEvent(inputEvent);
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x002024C7 File Offset: 0x002006C7
		public override void _Process(double delta)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this.CanScroll)
			{
				return;
			}
			this.UpdateScrollPosition(delta);
			if (this._needsReinit)
			{
				this.InitGrid();
			}
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x002024F0 File Offset: 0x002006F0
		public override void _Notification(int what)
		{
			base._Notification(what);
			if ((long)what == 40L && base.IsNodeReady())
			{
				this._needsReinit = true;
			}
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0020250F File Offset: 0x0020070F
		public void SetScrollPosition(float scrollY)
		{
			this._targetDrag = scrollY;
			this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, scrollY);
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00202539 File Offset: 0x00200739
		public void SetCanScroll(bool canScroll)
		{
			this._scrollingEnabled = canScroll;
			if (!this.CanScroll)
			{
				this._isDragging = false;
			}
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x00202551 File Offset: 0x00200751
		public void InsetForTopBar()
		{
			base.SetAnchorAndOffset(1L, 0f, 80f, false);
		}

		/// <summary>
		/// Detects mouse click up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06004248 RID: 16968 RVA: 0x00202568 File Offset: 0x00200768
		private void ProcessMouseEvent(InputEvent inputEvent)
		{
			if (this._isDragging)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion != null)
				{
					this._targetDrag += inputEventMouseMotion.Relative.Y;
					return;
				}
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null)
			{
				if (inputEventMouseButton.ButtonIndex == 1L)
				{
					if (inputEventMouseButton.Pressed)
					{
						this._isDragging = true;
						this._startDrag = this._scrollContainer.Position.Y;
						this._targetDrag = this._startDrag;
						return;
					}
					this._isDragging = false;
					return;
				}
				else if (!inputEventMouseButton.Pressed)
				{
					this._isDragging = false;
				}
			}
		}

		/// <summary>
		/// Detects mouse wheel up/down and updates our scroll target accordingly
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06004249 RID: 16969 RVA: 0x002025FD File Offset: 0x002007FD
		private void ProcessScrollEvent(InputEvent inputEvent)
		{
			this._targetDrag += ScrollHelper.GetDragForScrollEvent(inputEvent);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x00202614 File Offset: 0x00200814
		private void ProcessGuiFocus(Control focusedControl)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this.CanScroll)
			{
				return;
			}
			if (!NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			if (focusedControl.GetParent() == this._scrollContainer)
			{
				float num = -focusedControl.Position.Y + base.Size.Y * 0.5f;
				float num2 = Math.Min(Math.Min(this.ScrollLimitTop, this.ScrollLimitBottom), 0f);
				float num3 = Math.Max(Math.Min(this.ScrollLimitTop, this.ScrollLimitBottom), 0f);
				num = Math.Clamp(num, num2, num3);
				this._targetDrag = num;
			}
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x002026B8 File Offset: 0x002008B8
		private void UpdateScrollPosition(double delta)
		{
			float num = this._scrollContainer.Position.Y;
			if (Math.Abs(num - this._targetDrag) > 0.1f)
			{
				float num2 = (float)Mathf.Sign(num - this._targetDrag);
				num = Mathf.Lerp(num, this._targetDrag, Mathf.Clamp((float)delta * 15f, 0f, 1f));
				float num3 = (float)Mathf.Sign(num - this._targetDrag);
				if (Math.Abs(num - this._targetDrag) < 0.5f || !Mathf.IsEqualApprox(num2, num3))
				{
					num = this._targetDrag;
				}
				this.AllocateCardHolders();
				if (!this._scrollbarPressed && this.CanScroll)
				{
					this._scrollbar.SetValueWithoutAnimation((double)(Mathf.Clamp(this._scrollContainer.Position.Y / this.ScrollLimitBottom, 0f, 1f) * 100f));
				}
			}
			if (this._scrollbarPressed)
			{
				this._targetDrag = Mathf.Lerp(0f, this.ScrollLimitBottom, (float)this._scrollbar.Value / 100f);
				this.AllocateCardHolders();
			}
			if (!this._isDragging)
			{
				if (this._targetDrag < Mathf.Min(this.ScrollLimitBottom, this.ScrollLimitTop))
				{
					this._targetDrag = Mathf.Lerp(this._targetDrag, Mathf.Min(this.ScrollLimitBottom, this.ScrollLimitTop), (float)delta * 12f);
				}
				else if (this._targetDrag > Mathf.Max(this.ScrollLimitTop, this.ScrollLimitBottom))
				{
					this._targetDrag = Mathf.Lerp(this._targetDrag, Mathf.Max(this.ScrollLimitTop, this.ScrollLimitBottom), (float)delta * 12f);
				}
			}
			this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, num);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0020288B File Offset: 0x00200A8B
		public void ClearGrid()
		{
			this._cardsCache.Clear();
			this._cards.Clear();
			TaskHelper.RunSafely(this.InitGrid(null));
		}

		/// <summary>
		/// Sets the cards to display in the grid.
		/// If IsCardLibrary is true, then the old cards will be animated out and the new cards will be animated in.
		/// </summary>
		/// <param name="cardsToDisplay">The set of cards to display.</param>
		/// <param name="pileType">The type of pile we are displaying. Used when updating the text on the cards.</param>
		/// <param name="sortingPriority">The sorting applied to the cards.</param>
		/// <param name="taskToWaitOn">If non-null, the new cards will not be displayed until this task is finished. Prefer
		/// passing it over awaiting it, as the old cards can still animate out while the task is running and hide loading.</param>
		// Token: 0x0600424D RID: 16973 RVA: 0x002028B0 File Offset: 0x00200AB0
		public void SetCards(IReadOnlyList<CardModel> cardsToDisplay, PileType pileType, List<SortingOrders> sortingPriority, [Nullable(2)] Task taskToWaitOn = null)
		{
			this._cardsCache.Clear();
			this._cardsCache.AddRange(cardsToDisplay);
			if (sortingPriority[0] == SortingOrders.Descending)
			{
				this._cardsCache.Reverse();
			}
			else if (sortingPriority[0] != SortingOrders.Ascending)
			{
				this._cardsCache.Sort(delegate(CardModel x, CardModel y)
				{
					foreach (SortingOrders sortingOrders in sortingPriority)
					{
						int num = this.SortingAlgorithms[sortingOrders](x, y);
						if (num != 0)
						{
							return num;
						}
					}
					return x.Id.CompareTo(y.Id);
				});
			}
			if (this.IsCardLibrary)
			{
				this._sortedCardsCache.Clear();
				this._sortedCardsCache.AddRange(this._cardsCache.OrderBy<CardModel, CardModel>((CardModel c) => c, Comparer<CardModel>.Create(new Comparison<CardModel>(this.CompareCardVisibility))));
				this._cardsCache.Clear();
				this._cardsCache.AddRange(this._sortedCardsCache);
			}
			this._cards.Clear();
			this._cards.AddRange(this._cardsCache);
			this._pileType = pileType;
			if (this._cardsAnimatingOutForSetCards)
			{
				return;
			}
			TaskHelper.RunSafely(this.InitGrid(taskToWaitOn));
		}

		/// <summary>
		/// Animate the grid out.
		/// If this grid is not the card library grid, then this does nothing.
		/// If this is called before SetCards, then SetCards will wait for this animation to be completed before the cards
		/// animate back in. Otherwise, SetCards itself will call this.
		/// </summary>
		// Token: 0x0600424E RID: 16974 RVA: 0x002029DA File Offset: 0x00200BDA
		public Task AnimateOut()
		{
			this._animatingOutTask = this.AnimateOutInternal();
			return this._animatingOutTask;
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x002029F0 File Offset: 0x00200BF0
		private async Task AnimateOutInternal()
		{
			if (this.IsCardLibrary)
			{
				List<NGridCardHolder> list = this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> c) => c).ToList<NGridCardHolder>();
				if (list.Count > 0)
				{
					Tween tween = base.CreateTween().SetParallel(true);
					foreach (NGridCardHolder ngridCardHolder in list)
					{
						tween.TweenProperty(ngridCardHolder, "position:y", ngridCardHolder.Position.Y + 40f, 0.2).SetEase(1L).SetTrans(5L);
						tween.TweenProperty(ngridCardHolder, "modulate:a", 0f, 0.2);
					}
					bool flag = await tween.AwaitFinished(this);
				}
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x00202A34 File Offset: 0x00200C34
		private async Task AnimateIn()
		{
			if (this.IsCardLibrary)
			{
				List<NGridCardHolder> list = this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> c) => c).ToList<NGridCardHolder>();
				if (list.Count > 0)
				{
					Tween tween = base.CreateTween().SetParallel(true);
					for (int i = 0; i < list.Count; i++)
					{
						NGridCardHolder ngridCardHolder = list[i];
						float num = (float)i / (float)list.Count * 0.2f;
						float y = ngridCardHolder.Position.Y;
						Control control = ngridCardHolder;
						Vector2 position = ngridCardHolder.Position;
						position.Y = ngridCardHolder.Position.Y + 40f;
						control.Position = position;
						CanvasItem canvasItem = ngridCardHolder;
						Color modulate = ngridCardHolder.Modulate;
						modulate.A = 0f;
						canvasItem.Modulate = modulate;
						tween.TweenProperty(ngridCardHolder, "position:y", y, 0.4).SetEase(1L).SetTrans(10L)
							.SetDelay((double)num);
						tween.TweenProperty(ngridCardHolder, "modulate:a", 1f, 0.4).SetEase(1L).SetTrans(5L)
							.SetDelay((double)num);
					}
					this._setCardsCancellation = new CancellationTokenSource();
					while (tween.IsRunning())
					{
						if (this._setCardsCancellation.IsCancellationRequested)
						{
							tween.Kill();
						}
						await this.AwaitProcessFrame(this._cts.Token);
					}
					tween = null;
				}
			}
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x00202A78 File Offset: 0x00200C78
		private async Task InitGrid([Nullable(2)] Task taskToWaitOn)
		{
			if (this._setCardsCancellation != null)
			{
				await this._setCardsCancellation.CancelAsync();
			}
			this._cardsAnimatingOutForSetCards = true;
			Task animatingOutTask = this._animatingOutTask;
			if (animatingOutTask != null && !animatingOutTask.IsCompleted)
			{
				await this._animatingOutTask;
			}
			else
			{
				await this.AnimateOut();
			}
			if (taskToWaitOn != null)
			{
				await taskToWaitOn;
			}
			this._cardsAnimatingOutForSetCards = false;
			this.InitGrid();
			this.SetScrollPosition(this.ScrollLimitTop);
			await this.AnimateIn();
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x00202AC4 File Offset: 0x00200CC4
		private int CalculateRowsNeeded()
		{
			int num = Mathf.CeilToInt((base.Size.Y + this.CardPadding) / (this._cardSize.Y + this.CardPadding)) + 2;
			return Mathf.Min(num, this.GetTotalRowCount());
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x00202B0C File Offset: 0x00200D0C
		protected virtual void InitGrid()
		{
			this._scrollContainer.Position = new Vector2(this._scrollContainer.Position.X, this.ScrollLimitTop);
			this._slidingWindowCardIndex = 0;
			this._scrollbar.Value = 0.0;
			this.DisplayedRows = this.CalculateRowsNeeded();
			foreach (List<NGridCardHolder> list in this._cardRows)
			{
				foreach (NGridCardHolder ngridCardHolder in list)
				{
					NGridCardHolder ngridCardHolder2 = ngridCardHolder;
					ngridCardHolder2.Name += "-OLD";
					ngridCardHolder.QueueFreeSafely();
				}
			}
			this._cardRows.Clear();
			if (this._cards.Count != 0)
			{
				int num = 0;
				for (int i = 0; i < this.DisplayedRows; i++)
				{
					List<NGridCardHolder> list2 = new List<NGridCardHolder>();
					int num2 = 0;
					while (num2 < this.Columns && num < this._cards.Count)
					{
						CardModel cardModel = this._cards[num];
						NCard ncard = NCard.Create(cardModel, this.GetCardVisibility(cardModel));
						NGridCardHolder ngridCardHolder3 = NGridCardHolder.Create(ncard);
						list2.Add(ngridCardHolder3);
						ngridCardHolder3.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderPressed)), 0U);
						ngridCardHolder3.Connect(NCardHolder.SignalName.AltPressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.OnHolderAltPressed)), 0U);
						ngridCardHolder3.Visible = true;
						ngridCardHolder3.MouseFilter = 1L;
						ngridCardHolder3.Scale = ngridCardHolder3.SmallScale;
						this._scrollContainer.AddChildSafely(ngridCardHolder3);
						ncard.UpdateVisuals(this._pileType, CardPreviewMode.Normal);
						if (ngridCardHolder3.CardModel.IsUpgradable)
						{
							ngridCardHolder3.SetIsPreviewingUpgrade(this.IsShowingUpgrades);
						}
						num++;
						num2++;
					}
					this._cardRows.Add(list2);
				}
			}
			this._scrollContainer.SetDeferred(Control.PropertyName.Size, new Vector2(this._scrollContainer.Size.X, this.GetContainedCardsSize().Y + 80f + 320f + (float)this.YOffset));
			this.UpdateGridPositions(0);
			this.UpdateGridNavigation();
			this._needsReinit = false;
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x00202DA4 File Offset: 0x00200FA4
		private Vector2 GetContainedCardsSize()
		{
			int totalRowCount = this.GetTotalRowCount();
			return new Vector2((float)this.Columns, (float)totalRowCount) * this._cardSize + new Vector2((float)(this.Columns - 1), (float)(totalRowCount - 1)) * this.CardPadding;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x00202DF3 File Offset: 0x00200FF3
		private void ReflowColumns()
		{
			if (this._cards.Count == 0)
			{
				return;
			}
			this.InitGrid();
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00202E0C File Offset: 0x0020100C
		private void UpdateGridPositions(int index)
		{
			Vector2 vector = new Vector2((this._scrollContainer.Size.X - this.GetContainedCardsSize().X) * 0.5f, (float)this.YOffset + 80f) + this._cardSize * 0.5f;
			foreach (List<NGridCardHolder> list in this._cardRows)
			{
				foreach (NGridCardHolder ngridCardHolder in list)
				{
					int num = index / this.Columns;
					int num2 = index % this.Columns;
					ngridCardHolder.Position = vector + new Vector2((float)num2 * (this._cardSize.X + this.CardPadding), (float)num * (this._cardSize.Y + this.CardPadding));
					index++;
				}
			}
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x00202F38 File Offset: 0x00201138
		[return: Nullable(2)]
		public NGridCardHolder GetCardHolder(CardModel model)
		{
			return this._cardRows.SelectMany<List<NGridCardHolder>, NGridCardHolder>((List<NGridCardHolder> row) => row).FirstOrDefault<NGridCardHolder>((NGridCardHolder h) => h.CardModel == model);
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00202F8F File Offset: 0x0020118F
		[return: Nullable(2)]
		public NCard GetCardNode(CardModel model)
		{
			NGridCardHolder cardHolder = this.GetCardHolder(model);
			if (cardHolder == null)
			{
				return null;
			}
			return cardHolder.CardNode;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00202FA3 File Offset: 0x002011A3
		[return: Nullable(new byte[] { 2, 1 })]
		public IEnumerable<NGridCardHolder> GetTopRowOfCardNodes()
		{
			return this._cardRows.FirstOrDefault<List<NGridCardHolder>>();
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x00202FB0 File Offset: 0x002011B0
		private void OnHolderPressed(NCardHolder holder)
		{
			this._lastFocusedHolder = holder;
			base.EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[] { holder });
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00202FD8 File Offset: 0x002011D8
		private void OnHolderAltPressed(NCardHolder holder)
		{
			this._lastFocusedHolder = holder;
			base.EmitSignal(NCardGrid.SignalName.HolderAltPressed, new Variant[] { holder });
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x00203000 File Offset: 0x00201200
		private int GetTotalRowCount()
		{
			return Mathf.CeilToInt((float)this._cards.Count / (float)this.Columns);
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0020301C File Offset: 0x0020121C
		private void AllocateCardHolders()
		{
			if (this._cardRows.Count == 0)
			{
				return;
			}
			float y = base.GetViewportRect().Size.Y;
			float num = y;
			List<NGridCardHolder> list = this._cardRows[0];
			float y2 = list[0].GlobalPosition.Y;
			List<List<NGridCardHolder>> cardRows = this._cardRows;
			List<NGridCardHolder> list2 = cardRows[cardRows.Count - 1];
			float y3 = list2[0].GlobalPosition.Y;
			if (Mathf.Abs(y2 - 0f) > base.Size.Y * 2f)
			{
				this.ReallocateAll();
				return;
			}
			if (y2 > 0f)
			{
				List<List<NGridCardHolder>> cardRows2 = this._cardRows;
				this.ReallocateAbove(cardRows2[cardRows2.Count - 1]);
				return;
			}
			if (y3 < num)
			{
				this.ReallocateBelow(this._cardRows[0]);
			}
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x002030F8 File Offset: 0x002012F8
		private void ReallocateAll()
		{
			List<NGridCardHolder> list = this._cardRows[0];
			float y = list[0].GlobalPosition.Y;
			float num = y - 0f;
			int num2 = Mathf.RoundToInt(num / (this._cardSize.Y + this.CardPadding));
			int num3 = Mathf.Max(0, this._slidingWindowCardIndex - this.Columns * num2);
			this._slidingWindowCardIndex = num3;
			int count = this._cardRows.Count;
			for (int i = 0; i < count; i++)
			{
				this.AssignCardsToRow(this._cardRows[i], this._slidingWindowCardIndex + i * this.Columns);
			}
			this.UpdateGridPositions(this._slidingWindowCardIndex);
			this.UpdateGridNavigation();
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x002031B8 File Offset: 0x002013B8
		private void ReallocateAbove(List<NGridCardHolder> row)
		{
			int num = this._slidingWindowCardIndex - this.Columns;
			if (num < 0)
			{
				return;
			}
			this._slidingWindowCardIndex = num;
			this._cardRows.RemoveAt(this._cardRows.Count - 1);
			this.AssignCardsToRow(row, this._slidingWindowCardIndex);
			this._cardRows.Insert(0, row);
			List<NGridCardHolder> list = this._cardRows[1];
			float y = list[0].Position.Y;
			foreach (NGridCardHolder ngridCardHolder in row)
			{
				Control control = ngridCardHolder;
				Vector2 position = ngridCardHolder.Position;
				position.Y = y - this._cardSize.Y - this.CardPadding;
				control.Position = position;
			}
			this.UpdateGridNavigation();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x002032A0 File Offset: 0x002014A0
		private void ReallocateBelow(List<NGridCardHolder> row)
		{
			int num = this.Columns * this.DisplayedRows;
			int num2 = this._slidingWindowCardIndex + num;
			if (num2 >= this._cards.Count)
			{
				return;
			}
			this._slidingWindowCardIndex += this.Columns;
			this._cardRows.RemoveAt(0);
			this.AssignCardsToRow(row, num2);
			this._cardRows.Add(row);
			List<List<NGridCardHolder>> cardRows = this._cardRows;
			List<NGridCardHolder> list = cardRows[cardRows.Count - 2];
			float y = list[0].Position.Y;
			foreach (NGridCardHolder ngridCardHolder in row)
			{
				Control control = ngridCardHolder;
				Vector2 position = ngridCardHolder.Position;
				position.Y = y + this._cardSize.Y + this.CardPadding;
				control.Position = position;
			}
			this.UpdateGridNavigation();
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0020339C File Offset: 0x0020159C
		public void HighlightCard(CardModel card)
		{
			this._highlightedCards.Add(card);
			NCard cardNode = this.GetCardNode(card);
			if (cardNode != null)
			{
				cardNode.CardHighlight.AnimShow();
			}
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x002033CC File Offset: 0x002015CC
		public void UnhighlightCard(CardModel card)
		{
			this._highlightedCards.Remove(card);
			NCard cardNode = this.GetCardNode(card);
			if (cardNode != null)
			{
				cardNode.CardHighlight.AnimHide();
			}
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x002033FC File Offset: 0x002015FC
		protected virtual void AssignCardsToRow(List<NGridCardHolder> row, int startIndex)
		{
			for (int i = 0; i < row.Count; i++)
			{
				NGridCardHolder ngridCardHolder = row[i];
				if (startIndex + i >= this._cards.Count)
				{
					ngridCardHolder.Visible = false;
				}
				else
				{
					CardModel cardModel = this._cards[startIndex + i];
					ngridCardHolder.ReassignToCard(cardModel, PileType.None, null, this.GetCardVisibility(cardModel));
					ngridCardHolder.Visible = true;
					if (this._highlightedCards.Contains(cardModel))
					{
						ngridCardHolder.CardNode.CardHighlight.AnimShow();
					}
					else
					{
						ngridCardHolder.CardNode.CardHighlight.AnimHide();
					}
					if (this._isShowingUpgrades && cardModel.IsUpgradable)
					{
						ngridCardHolder.SetIsPreviewingUpgrade(true);
					}
				}
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x002034AE File Offset: 0x002016AE
		protected virtual ModelVisibility GetCardVisibility(CardModel card)
		{
			return ModelVisibility.Visible;
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06004265 RID: 16997 RVA: 0x002034B1 File Offset: 0x002016B1
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._lastFocusedHolder != null)
				{
					return this._lastFocusedHolder;
				}
				if (this._cards.Count == 0)
				{
					return null;
				}
				return this._cardRows[0][0];
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x002034E3 File Offset: 0x002016E3
		[Nullable(2)]
		public Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._cards.Count != 0)
				{
					return this._cardRows[0][0];
				}
				return null;
			}
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x00203508 File Offset: 0x00201708
		protected virtual void UpdateGridNavigation()
		{
			for (int i = 0; i < this._cardRows.Count; i++)
			{
				for (int j = 0; j < this._cardRows[i].Count; j++)
				{
					NCardHolder ncardHolder = this._cardRows[i][j];
					ncardHolder.FocusNeighborLeft = ((j > 0) ? this._cardRows[i][j - 1].GetPath() : this._cardRows[i][this._cardRows[i].Count - 1].GetPath());
					ncardHolder.FocusNeighborRight = ((j < this._cardRows[i].Count - 1) ? this._cardRows[i][j + 1].GetPath() : this._cardRows[i][0].GetPath());
					ncardHolder.FocusNeighborTop = ((i > 0) ? this._cardRows[i - 1][j].GetPath() : this._cardRows[i][j].GetPath());
					ncardHolder.FocusNeighborBottom = ((i < this._cardRows.Count - 1 && j < this._cardRows[i + 1].Count) ? this._cardRows[i + 1][j].GetPath() : this._cardRows[i][j].GetPath());
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004268 RID: 17000 RVA: 0x0020369C File Offset: 0x0020189C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(27)
			{
				new MethodInfo(NCardGrid.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.UpdateScrollLimitBottom, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.SetScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "scrollY", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.SetCanScroll, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "canScroll", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.InsetForTopBar, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.ProcessMouseEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.ProcessScrollEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.ProcessGuiFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusedControl", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.UpdateScrollPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.ClearGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.CalculateRowsNeeded, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.InitGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.GetContainedCardsSize, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.ReflowColumns, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.UpdateGridPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.OnHolderPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.OnHolderAltPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardGrid.MethodName.GetTotalRowCount, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.AllocateCardHolders, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.ReallocateAll, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGrid.MethodName.UpdateGridNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004269 RID: 17001 RVA: 0x00203DA8 File Offset: 0x00201FA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardGrid.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.UpdateScrollLimitBottom && args.Count == 0)
			{
				this.UpdateScrollLimitBottom();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.SetScrollPosition && args.Count == 1)
			{
				this.SetScrollPosition(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.SetCanScroll && args.Count == 1)
			{
				this.SetCanScroll(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.InsetForTopBar && args.Count == 0)
			{
				this.InsetForTopBar();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ProcessMouseEvent && args.Count == 1)
			{
				this.ProcessMouseEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ProcessScrollEvent && args.Count == 1)
			{
				this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ProcessGuiFocus && args.Count == 1)
			{
				this.ProcessGuiFocus(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.UpdateScrollPosition && args.Count == 1)
			{
				this.UpdateScrollPosition(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ClearGrid && args.Count == 0)
			{
				this.ClearGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.CalculateRowsNeeded && args.Count == 0)
			{
				int num = this.CalculateRowsNeeded();
				ret = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.InitGrid && args.Count == 0)
			{
				this.InitGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.GetContainedCardsSize && args.Count == 0)
			{
				Vector2 containedCardsSize = this.GetContainedCardsSize();
				ret = VariantUtils.CreateFrom<Vector2>(ref containedCardsSize);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ReflowColumns && args.Count == 0)
			{
				this.ReflowColumns();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.UpdateGridPositions && args.Count == 1)
			{
				this.UpdateGridPositions(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.OnHolderPressed && args.Count == 1)
			{
				this.OnHolderPressed(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.OnHolderAltPressed && args.Count == 1)
			{
				this.OnHolderAltPressed(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.GetTotalRowCount && args.Count == 0)
			{
				int totalRowCount = this.GetTotalRowCount();
				ret = VariantUtils.CreateFrom<int>(ref totalRowCount);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.AllocateCardHolders && args.Count == 0)
			{
				this.AllocateCardHolders();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.ReallocateAll && args.Count == 0)
			{
				this.ReallocateAll();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGrid.MethodName.UpdateGridNavigation && args.Count == 0)
			{
				this.UpdateGridNavigation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600426A RID: 17002 RVA: 0x00204264 File Offset: 0x00202464
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardGrid.MethodName._Ready || (ref method) == NCardGrid.MethodName.ConnectSignals || (ref method) == NCardGrid.MethodName._EnterTree || (ref method) == NCardGrid.MethodName._ExitTree || (ref method) == NCardGrid.MethodName.UpdateScrollLimitBottom || (ref method) == NCardGrid.MethodName._GuiInput || (ref method) == NCardGrid.MethodName._Process || (ref method) == NCardGrid.MethodName._Notification || (ref method) == NCardGrid.MethodName.SetScrollPosition || (ref method) == NCardGrid.MethodName.SetCanScroll || (ref method) == NCardGrid.MethodName.InsetForTopBar || (ref method) == NCardGrid.MethodName.ProcessMouseEvent || (ref method) == NCardGrid.MethodName.ProcessScrollEvent || (ref method) == NCardGrid.MethodName.ProcessGuiFocus || (ref method) == NCardGrid.MethodName.UpdateScrollPosition || (ref method) == NCardGrid.MethodName.ClearGrid || (ref method) == NCardGrid.MethodName.CalculateRowsNeeded || (ref method) == NCardGrid.MethodName.InitGrid || (ref method) == NCardGrid.MethodName.GetContainedCardsSize || (ref method) == NCardGrid.MethodName.ReflowColumns || (ref method) == NCardGrid.MethodName.UpdateGridPositions || (ref method) == NCardGrid.MethodName.OnHolderPressed || (ref method) == NCardGrid.MethodName.OnHolderAltPressed || (ref method) == NCardGrid.MethodName.GetTotalRowCount || (ref method) == NCardGrid.MethodName.AllocateCardHolders || (ref method) == NCardGrid.MethodName.ReallocateAll || (ref method) == NCardGrid.MethodName.UpdateGridNavigation || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600426B RID: 17003 RVA: 0x00204410 File Offset: 0x00202610
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardGrid.PropertyName.DisplayedRows)
			{
				this.DisplayedRows = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.IsShowingUpgrades)
			{
				this.IsShowingUpgrades = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.YOffset)
			{
				this.YOffset = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._startDrag)
			{
				this._startDrag = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._targetDrag)
			{
				this._targetDrag = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollingEnabled)
			{
				this._scrollingEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollContainer)
			{
				this._scrollContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollbarPressed)
			{
				this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollbar)
			{
				this._scrollbar = VariantUtils.ConvertTo<NScrollbar>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._slidingWindowCardIndex)
			{
				this._slidingWindowCardIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._pileType)
			{
				this._pileType = VariantUtils.ConvertTo<PileType>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._cardSize)
			{
				this._cardSize = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._cardsAnimatingOutForSetCards)
			{
				this._cardsAnimatingOutForSetCards = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._isShowingUpgrades)
			{
				this._isShowingUpgrades = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._lastFocusedHolder)
			{
				this._lastFocusedHolder = VariantUtils.ConvertTo<NCardHolder>(ref value);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._needsReinit)
			{
				this._needsReinit = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600426C RID: 17004 RVA: 0x002045F0 File Offset: 0x002027F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardGrid.PropertyName.CanScroll)
			{
				bool flag = this.CanScroll;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.DisplayedRows)
			{
				int num = this.DisplayedRows;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.Columns)
			{
				int num = this.Columns;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.CardPadding)
			{
				float num2 = this.CardPadding;
				value = VariantUtils.CreateFrom<float>(ref num2);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.IsCardLibrary)
			{
				bool flag = this.IsCardLibrary;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.ScrollLimitBottom)
			{
				float num2 = this.ScrollLimitBottom;
				value = VariantUtils.CreateFrom<float>(ref num2);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.ScrollLimitTop)
			{
				float num2 = this.ScrollLimitTop;
				value = VariantUtils.CreateFrom<float>(ref num2);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.IsAnimatingOut)
			{
				bool flag = this.IsAnimatingOut;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.IsShowingUpgrades)
			{
				bool flag = this.IsShowingUpgrades;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.YOffset)
			{
				int num = this.YOffset;
				value = VariantUtils.CreateFrom<int>(ref num);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.CenterGrid)
			{
				bool flag = this.CenterGrid;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._startDrag)
			{
				value = VariantUtils.CreateFrom<float>(ref this._startDrag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._targetDrag)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetDrag);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollingEnabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scrollingEnabled);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._scrollContainer);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollbarPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._scrollbar)
			{
				value = VariantUtils.CreateFrom<NScrollbar>(ref this._scrollbar);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._slidingWindowCardIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._slidingWindowCardIndex);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._pileType)
			{
				value = VariantUtils.CreateFrom<PileType>(ref this._pileType);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._cardSize)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._cardSize);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._cardsAnimatingOutForSetCards)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._cardsAnimatingOutForSetCards);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._isShowingUpgrades)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isShowingUpgrades);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._lastFocusedHolder)
			{
				value = VariantUtils.CreateFrom<NCardHolder>(ref this._lastFocusedHolder);
				return true;
			}
			if ((ref name) == NCardGrid.PropertyName._needsReinit)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._needsReinit);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600426D RID: 17005 RVA: 0x0020498C File Offset: 0x00202B8C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NCardGrid.PropertyName._startDrag, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardGrid.PropertyName._targetDrag, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._isDragging, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._scrollingEnabled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName.CanScroll, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGrid.PropertyName.DisplayedRows, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGrid.PropertyName.Columns, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardGrid.PropertyName.CardPadding, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName.IsCardLibrary, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGrid.PropertyName._scrollContainer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardGrid.PropertyName.ScrollLimitBottom, 0L, "", 4096L, false),
				new PropertyInfo(3L, NCardGrid.PropertyName.ScrollLimitTop, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._scrollbarPressed, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGrid.PropertyName._scrollbar, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGrid.PropertyName._slidingWindowCardIndex, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGrid.PropertyName._pileType, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardGrid.PropertyName._cardSize, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._cardsAnimatingOutForSetCards, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._isShowingUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGrid.PropertyName._lastFocusedHolder, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName.IsAnimatingOut, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName.IsShowingUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName._needsReinit, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGrid.PropertyName.YOffset, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGrid.PropertyName.CenterGrid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGrid.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGrid.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600426E RID: 17006 RVA: 0x00204D08 File Offset: 0x00202F08
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName displayedRows = NCardGrid.PropertyName.DisplayedRows;
			int num = this.DisplayedRows;
			info.AddProperty(displayedRows, Variant.From<int>(ref num));
			StringName isShowingUpgrades = NCardGrid.PropertyName.IsShowingUpgrades;
			bool isShowingUpgrades2 = this.IsShowingUpgrades;
			info.AddProperty(isShowingUpgrades, Variant.From<bool>(ref isShowingUpgrades2));
			StringName yoffset = NCardGrid.PropertyName.YOffset;
			num = this.YOffset;
			info.AddProperty(yoffset, Variant.From<int>(ref num));
			info.AddProperty(NCardGrid.PropertyName._startDrag, Variant.From<float>(ref this._startDrag));
			info.AddProperty(NCardGrid.PropertyName._targetDrag, Variant.From<float>(ref this._targetDrag));
			info.AddProperty(NCardGrid.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddProperty(NCardGrid.PropertyName._scrollingEnabled, Variant.From<bool>(ref this._scrollingEnabled));
			info.AddProperty(NCardGrid.PropertyName._scrollContainer, Variant.From<Control>(ref this._scrollContainer));
			info.AddProperty(NCardGrid.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
			info.AddProperty(NCardGrid.PropertyName._scrollbar, Variant.From<NScrollbar>(ref this._scrollbar));
			info.AddProperty(NCardGrid.PropertyName._slidingWindowCardIndex, Variant.From<int>(ref this._slidingWindowCardIndex));
			info.AddProperty(NCardGrid.PropertyName._pileType, Variant.From<PileType>(ref this._pileType));
			info.AddProperty(NCardGrid.PropertyName._cardSize, Variant.From<Vector2>(ref this._cardSize));
			info.AddProperty(NCardGrid.PropertyName._cardsAnimatingOutForSetCards, Variant.From<bool>(ref this._cardsAnimatingOutForSetCards));
			info.AddProperty(NCardGrid.PropertyName._isShowingUpgrades, Variant.From<bool>(ref this._isShowingUpgrades));
			info.AddProperty(NCardGrid.PropertyName._lastFocusedHolder, Variant.From<NCardHolder>(ref this._lastFocusedHolder));
			info.AddProperty(NCardGrid.PropertyName._needsReinit, Variant.From<bool>(ref this._needsReinit));
			info.AddSignalEventDelegate(NCardGrid.SignalName.HolderPressed, this.backing_HolderPressed);
			info.AddSignalEventDelegate(NCardGrid.SignalName.HolderAltPressed, this.backing_HolderAltPressed);
		}

		/// <inheritdoc />
		// Token: 0x0600426F RID: 17007 RVA: 0x00204EC0 File Offset: 0x002030C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardGrid.PropertyName.DisplayedRows, ref variant))
			{
				this.DisplayedRows = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardGrid.PropertyName.IsShowingUpgrades, ref variant2))
			{
				this.IsShowingUpgrades = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardGrid.PropertyName.YOffset, ref variant3))
			{
				this.YOffset = variant3.As<int>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardGrid.PropertyName._startDrag, ref variant4))
			{
				this._startDrag = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardGrid.PropertyName._targetDrag, ref variant5))
			{
				this._targetDrag = variant5.As<float>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardGrid.PropertyName._isDragging, ref variant6))
			{
				this._isDragging = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCardGrid.PropertyName._scrollingEnabled, ref variant7))
			{
				this._scrollingEnabled = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCardGrid.PropertyName._scrollContainer, ref variant8))
			{
				this._scrollContainer = variant8.As<Control>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCardGrid.PropertyName._scrollbarPressed, ref variant9))
			{
				this._scrollbarPressed = variant9.As<bool>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCardGrid.PropertyName._scrollbar, ref variant10))
			{
				this._scrollbar = variant10.As<NScrollbar>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCardGrid.PropertyName._slidingWindowCardIndex, ref variant11))
			{
				this._slidingWindowCardIndex = variant11.As<int>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCardGrid.PropertyName._pileType, ref variant12))
			{
				this._pileType = variant12.As<PileType>();
			}
			Variant variant13;
			if (info.TryGetProperty(NCardGrid.PropertyName._cardSize, ref variant13))
			{
				this._cardSize = variant13.As<Vector2>();
			}
			Variant variant14;
			if (info.TryGetProperty(NCardGrid.PropertyName._cardsAnimatingOutForSetCards, ref variant14))
			{
				this._cardsAnimatingOutForSetCards = variant14.As<bool>();
			}
			Variant variant15;
			if (info.TryGetProperty(NCardGrid.PropertyName._isShowingUpgrades, ref variant15))
			{
				this._isShowingUpgrades = variant15.As<bool>();
			}
			Variant variant16;
			if (info.TryGetProperty(NCardGrid.PropertyName._lastFocusedHolder, ref variant16))
			{
				this._lastFocusedHolder = variant16.As<NCardHolder>();
			}
			Variant variant17;
			if (info.TryGetProperty(NCardGrid.PropertyName._needsReinit, ref variant17))
			{
				this._needsReinit = variant17.As<bool>();
			}
			NCardGrid.HolderPressedEventHandler holderPressedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardGrid.HolderPressedEventHandler>(NCardGrid.SignalName.HolderPressed, ref holderPressedEventHandler))
			{
				this.backing_HolderPressed = holderPressedEventHandler;
			}
			NCardGrid.HolderAltPressedEventHandler holderAltPressedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardGrid.HolderAltPressedEventHandler>(NCardGrid.SignalName.HolderAltPressed, ref holderAltPressedEventHandler))
			{
				this.backing_HolderAltPressed = holderAltPressedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004270 RID: 17008 RVA: 0x002050E0 File Offset: 0x002032E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardGrid.SignalName.HolderPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardGrid.SignalName.HolderAltPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "card", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.NCardGrid.HolderPressedEventHandler" />
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06004271 RID: 17009 RVA: 0x002051B3 File Offset: 0x002033B3
		// (remove) Token: 0x06004272 RID: 17010 RVA: 0x002051CC File Offset: 0x002033CC
		[Nullable(0)]
		public event NCardGrid.HolderPressedEventHandler HolderPressed
		{
			[NullableContext(0)]
			add
			{
				this.backing_HolderPressed = (NCardGrid.HolderPressedEventHandler)Delegate.Combine(this.backing_HolderPressed, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_HolderPressed = (NCardGrid.HolderPressedEventHandler)Delegate.Remove(this.backing_HolderPressed, value);
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x002051E5 File Offset: 0x002033E5
		[NullableContext(0)]
		protected void EmitSignalHolderPressed(NCardHolder card)
		{
			base.EmitSignal(NCardGrid.SignalName.HolderPressed, new Variant[] { card });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.NCardGrid.HolderAltPressedEventHandler" />
		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06004274 RID: 17012 RVA: 0x00205206 File Offset: 0x00203406
		// (remove) Token: 0x06004275 RID: 17013 RVA: 0x0020521F File Offset: 0x0020341F
		[Nullable(0)]
		public event NCardGrid.HolderAltPressedEventHandler HolderAltPressed
		{
			[NullableContext(0)]
			add
			{
				this.backing_HolderAltPressed = (NCardGrid.HolderAltPressedEventHandler)Delegate.Combine(this.backing_HolderAltPressed, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_HolderAltPressed = (NCardGrid.HolderAltPressedEventHandler)Delegate.Remove(this.backing_HolderAltPressed, value);
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x00205238 File Offset: 0x00203438
		[NullableContext(0)]
		protected void EmitSignalHolderAltPressed(NCardHolder card)
		{
			base.EmitSignal(NCardGrid.SignalName.HolderAltPressed, new Variant[] { card });
		}

		/// <inheritdoc />
		// Token: 0x06004277 RID: 17015 RVA: 0x0020525C File Offset: 0x0020345C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NCardGrid.SignalName.HolderPressed && args.Count == 1)
			{
				NCardGrid.HolderPressedEventHandler holderPressedEventHandler = this.backing_HolderPressed;
				if (holderPressedEventHandler == null)
				{
					return;
				}
				holderPressedEventHandler(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NCardGrid.SignalName.HolderAltPressed) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NCardGrid.HolderAltPressedEventHandler holderAltPressedEventHandler = this.backing_HolderAltPressed;
				if (holderAltPressedEventHandler == null)
				{
					return;
				}
				holderAltPressedEventHandler(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06004278 RID: 17016 RVA: 0x002052DB File Offset: 0x002034DB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardGrid.SignalName.HolderPressed || (ref signal) == NCardGrid.SignalName.HolderAltPressed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040019DB RID: 6619
		[Nullable(new byte[] { 2, 1, 1, 1 })]
		private Dictionary<SortingOrders, Func<CardModel, CardModel, int>> _sortingAlgorithms;

		// Token: 0x040019DC RID: 6620
		private float _startDrag;

		// Token: 0x040019DD RID: 6621
		private float _targetDrag;

		// Token: 0x040019DE RID: 6622
		private bool _isDragging;

		// Token: 0x040019DF RID: 6623
		private bool _scrollingEnabled = true;

		// Token: 0x040019E1 RID: 6625
		private const float _topMargin = 80f;

		// Token: 0x040019E2 RID: 6626
		private const float _bottomMargin = 320f;

		// Token: 0x040019E3 RID: 6627
		protected Control _scrollContainer;

		// Token: 0x040019E4 RID: 6628
		private bool _scrollbarPressed;

		// Token: 0x040019E5 RID: 6629
		private NScrollbar _scrollbar;

		// Token: 0x040019E6 RID: 6630
		private int _slidingWindowCardIndex;

		// Token: 0x040019E7 RID: 6631
		private PileType _pileType;

		// Token: 0x040019E8 RID: 6632
		protected Vector2 _cardSize;

		// Token: 0x040019E9 RID: 6633
		protected readonly List<CardModel> _cards = new List<CardModel>();

		// Token: 0x040019EA RID: 6634
		protected readonly List<List<NGridCardHolder>> _cardRows = new List<List<NGridCardHolder>>();

		// Token: 0x040019EB RID: 6635
		private readonly List<CardModel> _highlightedCards = new List<CardModel>();

		// Token: 0x040019EC RID: 6636
		[Nullable(2)]
		private Task _animatingOutTask;

		// Token: 0x040019ED RID: 6637
		private bool _cardsAnimatingOutForSetCards;

		// Token: 0x040019EE RID: 6638
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x040019EF RID: 6639
		[Nullable(2)]
		private CancellationTokenSource _setCardsCancellation;

		// Token: 0x040019F0 RID: 6640
		private bool _isShowingUpgrades;

		// Token: 0x040019F1 RID: 6641
		[Nullable(2)]
		private NCardHolder _lastFocusedHolder;

		// Token: 0x040019F2 RID: 6642
		private readonly List<CardModel> _cardsCache = new List<CardModel>();

		// Token: 0x040019F3 RID: 6643
		private readonly List<CardModel> _sortedCardsCache = new List<CardModel>();

		// Token: 0x040019F4 RID: 6644
		private bool _needsReinit;

		// Token: 0x040019F6 RID: 6646
		[Nullable(0)]
		private NCardGrid.HolderPressedEventHandler backing_HolderPressed;

		// Token: 0x040019F7 RID: 6647
		[Nullable(0)]
		private NCardGrid.HolderAltPressedEventHandler backing_HolderAltPressed;

		// Token: 0x02001940 RID: 6464
		// (Invoke) Token: 0x0600AA56 RID: 43606
		[NullableContext(0)]
		[Signal]
		public delegate void HolderPressedEventHandler(NCardHolder card);

		// Token: 0x02001941 RID: 6465
		// (Invoke) Token: 0x0600AA5A RID: 43610
		[NullableContext(0)]
		[Signal]
		public delegate void HolderAltPressedEventHandler(NCardHolder card);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001942 RID: 6466
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400639B RID: 25499
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x0400639C RID: 25500
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400639D RID: 25501
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400639E RID: 25502
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateScrollLimitBottom' method.
			/// </summary>
			// Token: 0x0400639F RID: 25503
			public static readonly StringName UpdateScrollLimitBottom = "UpdateScrollLimitBottom";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x040063A0 RID: 25504
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040063A1 RID: 25505
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x040063A2 RID: 25506
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'SetScrollPosition' method.
			/// </summary>
			// Token: 0x040063A3 RID: 25507
			public static readonly StringName SetScrollPosition = "SetScrollPosition";

			/// <summary>
			/// Cached name for the 'SetCanScroll' method.
			/// </summary>
			// Token: 0x040063A4 RID: 25508
			public static readonly StringName SetCanScroll = "SetCanScroll";

			/// <summary>
			/// Cached name for the 'InsetForTopBar' method.
			/// </summary>
			// Token: 0x040063A5 RID: 25509
			public static readonly StringName InsetForTopBar = "InsetForTopBar";

			/// <summary>
			/// Cached name for the 'ProcessMouseEvent' method.
			/// </summary>
			// Token: 0x040063A6 RID: 25510
			public static readonly StringName ProcessMouseEvent = "ProcessMouseEvent";

			/// <summary>
			/// Cached name for the 'ProcessScrollEvent' method.
			/// </summary>
			// Token: 0x040063A7 RID: 25511
			public static readonly StringName ProcessScrollEvent = "ProcessScrollEvent";

			/// <summary>
			/// Cached name for the 'ProcessGuiFocus' method.
			/// </summary>
			// Token: 0x040063A8 RID: 25512
			public static readonly StringName ProcessGuiFocus = "ProcessGuiFocus";

			/// <summary>
			/// Cached name for the 'UpdateScrollPosition' method.
			/// </summary>
			// Token: 0x040063A9 RID: 25513
			public static readonly StringName UpdateScrollPosition = "UpdateScrollPosition";

			/// <summary>
			/// Cached name for the 'ClearGrid' method.
			/// </summary>
			// Token: 0x040063AA RID: 25514
			public static readonly StringName ClearGrid = "ClearGrid";

			/// <summary>
			/// Cached name for the 'CalculateRowsNeeded' method.
			/// </summary>
			// Token: 0x040063AB RID: 25515
			public static readonly StringName CalculateRowsNeeded = "CalculateRowsNeeded";

			/// <summary>
			/// Cached name for the 'InitGrid' method.
			/// </summary>
			// Token: 0x040063AC RID: 25516
			public static readonly StringName InitGrid = "InitGrid";

			/// <summary>
			/// Cached name for the 'GetContainedCardsSize' method.
			/// </summary>
			// Token: 0x040063AD RID: 25517
			public static readonly StringName GetContainedCardsSize = "GetContainedCardsSize";

			/// <summary>
			/// Cached name for the 'ReflowColumns' method.
			/// </summary>
			// Token: 0x040063AE RID: 25518
			public static readonly StringName ReflowColumns = "ReflowColumns";

			/// <summary>
			/// Cached name for the 'UpdateGridPositions' method.
			/// </summary>
			// Token: 0x040063AF RID: 25519
			public static readonly StringName UpdateGridPositions = "UpdateGridPositions";

			/// <summary>
			/// Cached name for the 'OnHolderPressed' method.
			/// </summary>
			// Token: 0x040063B0 RID: 25520
			public static readonly StringName OnHolderPressed = "OnHolderPressed";

			/// <summary>
			/// Cached name for the 'OnHolderAltPressed' method.
			/// </summary>
			// Token: 0x040063B1 RID: 25521
			public static readonly StringName OnHolderAltPressed = "OnHolderAltPressed";

			/// <summary>
			/// Cached name for the 'GetTotalRowCount' method.
			/// </summary>
			// Token: 0x040063B2 RID: 25522
			public static readonly StringName GetTotalRowCount = "GetTotalRowCount";

			/// <summary>
			/// Cached name for the 'AllocateCardHolders' method.
			/// </summary>
			// Token: 0x040063B3 RID: 25523
			public static readonly StringName AllocateCardHolders = "AllocateCardHolders";

			/// <summary>
			/// Cached name for the 'ReallocateAll' method.
			/// </summary>
			// Token: 0x040063B4 RID: 25524
			public static readonly StringName ReallocateAll = "ReallocateAll";

			/// <summary>
			/// Cached name for the 'UpdateGridNavigation' method.
			/// </summary>
			// Token: 0x040063B5 RID: 25525
			public static readonly StringName UpdateGridNavigation = "UpdateGridNavigation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001943 RID: 6467
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CanScroll' property.
			/// </summary>
			// Token: 0x040063B6 RID: 25526
			public static readonly StringName CanScroll = "CanScroll";

			/// <summary>
			/// Cached name for the 'DisplayedRows' property.
			/// </summary>
			// Token: 0x040063B7 RID: 25527
			public static readonly StringName DisplayedRows = "DisplayedRows";

			/// <summary>
			/// Cached name for the 'Columns' property.
			/// </summary>
			// Token: 0x040063B8 RID: 25528
			public static readonly StringName Columns = "Columns";

			/// <summary>
			/// Cached name for the 'CardPadding' property.
			/// </summary>
			// Token: 0x040063B9 RID: 25529
			public static readonly StringName CardPadding = "CardPadding";

			/// <summary>
			/// Cached name for the 'IsCardLibrary' property.
			/// </summary>
			// Token: 0x040063BA RID: 25530
			public static readonly StringName IsCardLibrary = "IsCardLibrary";

			/// <summary>
			/// Cached name for the 'ScrollLimitBottom' property.
			/// </summary>
			// Token: 0x040063BB RID: 25531
			public static readonly StringName ScrollLimitBottom = "ScrollLimitBottom";

			/// <summary>
			/// Cached name for the 'ScrollLimitTop' property.
			/// </summary>
			// Token: 0x040063BC RID: 25532
			public static readonly StringName ScrollLimitTop = "ScrollLimitTop";

			/// <summary>
			/// Cached name for the 'IsAnimatingOut' property.
			/// </summary>
			// Token: 0x040063BD RID: 25533
			public static readonly StringName IsAnimatingOut = "IsAnimatingOut";

			/// <summary>
			/// Cached name for the 'IsShowingUpgrades' property.
			/// </summary>
			// Token: 0x040063BE RID: 25534
			public static readonly StringName IsShowingUpgrades = "IsShowingUpgrades";

			/// <summary>
			/// Cached name for the 'YOffset' property.
			/// </summary>
			// Token: 0x040063BF RID: 25535
			public static readonly StringName YOffset = "YOffset";

			/// <summary>
			/// Cached name for the 'CenterGrid' property.
			/// </summary>
			// Token: 0x040063C0 RID: 25536
			public static readonly StringName CenterGrid = "CenterGrid";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040063C1 RID: 25537
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x040063C2 RID: 25538
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_startDrag' field.
			/// </summary>
			// Token: 0x040063C3 RID: 25539
			public static readonly StringName _startDrag = "_startDrag";

			/// <summary>
			/// Cached name for the '_targetDrag' field.
			/// </summary>
			// Token: 0x040063C4 RID: 25540
			public static readonly StringName _targetDrag = "_targetDrag";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x040063C5 RID: 25541
			public static readonly StringName _isDragging = "_isDragging";

			/// <summary>
			/// Cached name for the '_scrollingEnabled' field.
			/// </summary>
			// Token: 0x040063C6 RID: 25542
			public static readonly StringName _scrollingEnabled = "_scrollingEnabled";

			/// <summary>
			/// Cached name for the '_scrollContainer' field.
			/// </summary>
			// Token: 0x040063C7 RID: 25543
			public static readonly StringName _scrollContainer = "_scrollContainer";

			/// <summary>
			/// Cached name for the '_scrollbarPressed' field.
			/// </summary>
			// Token: 0x040063C8 RID: 25544
			public static readonly StringName _scrollbarPressed = "_scrollbarPressed";

			/// <summary>
			/// Cached name for the '_scrollbar' field.
			/// </summary>
			// Token: 0x040063C9 RID: 25545
			public static readonly StringName _scrollbar = "_scrollbar";

			/// <summary>
			/// Cached name for the '_slidingWindowCardIndex' field.
			/// </summary>
			// Token: 0x040063CA RID: 25546
			public static readonly StringName _slidingWindowCardIndex = "_slidingWindowCardIndex";

			/// <summary>
			/// Cached name for the '_pileType' field.
			/// </summary>
			// Token: 0x040063CB RID: 25547
			public static readonly StringName _pileType = "_pileType";

			/// <summary>
			/// Cached name for the '_cardSize' field.
			/// </summary>
			// Token: 0x040063CC RID: 25548
			public static readonly StringName _cardSize = "_cardSize";

			/// <summary>
			/// Cached name for the '_cardsAnimatingOutForSetCards' field.
			/// </summary>
			// Token: 0x040063CD RID: 25549
			public static readonly StringName _cardsAnimatingOutForSetCards = "_cardsAnimatingOutForSetCards";

			/// <summary>
			/// Cached name for the '_isShowingUpgrades' field.
			/// </summary>
			// Token: 0x040063CE RID: 25550
			public static readonly StringName _isShowingUpgrades = "_isShowingUpgrades";

			/// <summary>
			/// Cached name for the '_lastFocusedHolder' field.
			/// </summary>
			// Token: 0x040063CF RID: 25551
			public static readonly StringName _lastFocusedHolder = "_lastFocusedHolder";

			/// <summary>
			/// Cached name for the '_needsReinit' field.
			/// </summary>
			// Token: 0x040063D0 RID: 25552
			public static readonly StringName _needsReinit = "_needsReinit";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001944 RID: 6468
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'HolderPressed' signal.
			/// </summary>
			// Token: 0x040063D1 RID: 25553
			public static readonly StringName HolderPressed = "HolderPressed";

			/// <summary>
			/// Cached name for the 'HolderAltPressed' signal.
			/// </summary>
			// Token: 0x040063D2 RID: 25554
			public static readonly StringName HolderAltPressed = "HolderAltPressed";
		}
	}
}
