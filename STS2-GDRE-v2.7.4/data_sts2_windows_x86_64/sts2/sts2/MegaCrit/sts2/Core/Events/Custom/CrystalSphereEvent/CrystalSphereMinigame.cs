using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent
{
	// Token: 0x02000C50 RID: 3152
	[NullableContext(1)]
	[Nullable(0)]
	public class CrystalSphereMinigame
	{
		// Token: 0x17002376 RID: 9078
		// (get) Token: 0x060081E8 RID: 33256 RVA: 0x002BFC2A File Offset: 0x002BDE2A
		// (set) Token: 0x060081E9 RID: 33257 RVA: 0x002BFC32 File Offset: 0x002BDE32
		public Rng Rng { get; private set; }

		// Token: 0x17002377 RID: 9079
		// (get) Token: 0x060081EA RID: 33258 RVA: 0x002BFC3B File Offset: 0x002BDE3B
		// (set) Token: 0x060081EB RID: 33259 RVA: 0x002BFC43 File Offset: 0x002BDE43
		public int DivinationCount
		{
			get
			{
				return this._divinationCount;
			}
			set
			{
				this._divinationCount = value;
				Action divinationCountChanged = this.DivinationCountChanged;
				if (divinationCountChanged == null)
				{
					return;
				}
				divinationCountChanged();
			}
		}

		// Token: 0x17002378 RID: 9080
		// (get) Token: 0x060081EC RID: 33260 RVA: 0x002BFC5C File Offset: 0x002BDE5C
		public Vector2I GridSize
		{
			get
			{
				return new Vector2I(this.cells.GetLength(0), this.cells.GetLength(1));
			}
		}

		// Token: 0x17002379 RID: 9081
		// (get) Token: 0x060081ED RID: 33261 RVA: 0x002BFC7B File Offset: 0x002BDE7B
		public bool IsFinished
		{
			get
			{
				return this.DivinationCount == 0;
			}
		}

		// Token: 0x1700237A RID: 9082
		// (get) Token: 0x060081EE RID: 33262 RVA: 0x002BFC86 File Offset: 0x002BDE86
		// (set) Token: 0x060081EF RID: 33263 RVA: 0x002BFC8E File Offset: 0x002BDE8E
		public bool PlacedAllItems { get; private set; }

		// Token: 0x1700237B RID: 9083
		// (get) Token: 0x060081F0 RID: 33264 RVA: 0x002BFC97 File Offset: 0x002BDE97
		// (set) Token: 0x060081F1 RID: 33265 RVA: 0x002BFC9F File Offset: 0x002BDE9F
		public CrystalSphereMinigame.CrystalSphereToolType CrystalSphereTool { get; private set; }

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x060081F2 RID: 33266 RVA: 0x002BFCA8 File Offset: 0x002BDEA8
		// (remove) Token: 0x060081F3 RID: 33267 RVA: 0x002BFCE0 File Offset: 0x002BDEE0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action DivinationCountChanged;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x060081F4 RID: 33268 RVA: 0x002BFD18 File Offset: 0x002BDF18
		// (remove) Token: 0x060081F5 RID: 33269 RVA: 0x002BFD50 File Offset: 0x002BDF50
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action Finished;

		// Token: 0x1700237C RID: 9084
		// (get) Token: 0x060081F6 RID: 33270 RVA: 0x002BFD85 File Offset: 0x002BDF85
		public IReadOnlyList<CrystalSphereItem> Items
		{
			get
			{
				return this._items;
			}
		}

		/// <summary>
		/// The cell that we are currently hovered over.
		/// </summary>
		// Token: 0x1700237D RID: 9085
		// (get) Token: 0x060081F7 RID: 33271 RVA: 0x002BFD8D File Offset: 0x002BDF8D
		// (set) Token: 0x060081F8 RID: 33272 RVA: 0x002BFD95 File Offset: 0x002BDF95
		[Nullable(2)]
		private CrystalSphereCell HoveredCell
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		/// <summary>
		/// The cells that are highlighted on the board. This is separate from HoveredCell
		/// because we can highlight multiple cells at once depending on the tool
		/// </summary>
		// Token: 0x1700237E RID: 9086
		// (get) Token: 0x060081F9 RID: 33273 RVA: 0x002BFD9E File Offset: 0x002BDF9E
		// (set) Token: 0x060081FA RID: 33274 RVA: 0x002BFDA6 File Offset: 0x002BDFA6
		private List<CrystalSphereCell> HighlightedCells { get; set; } = new List<CrystalSphereCell>();

		// Token: 0x060081FB RID: 33275 RVA: 0x002BFDB0 File Offset: 0x002BDFB0
		public unsafe CrystalSphereMinigame(Player owner, Rng rng, int divinationCount)
		{
			this._owner = owner;
			this.Rng = rng;
			this.cells = new CrystalSphereCell[11, 11];
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					this.cells[i, j] = new CrystalSphereCell(i, j);
				}
			}
			int num = 4;
			List<Vector2I> list = new List<Vector2I>(num);
			CollectionsMarshal.SetCount<Vector2I>(list, num);
			Span<Vector2I> span = CollectionsMarshal.AsSpan<Vector2I>(list);
			int num2 = 0;
			*span[num2] = new Vector2I(0, 0);
			num2++;
			*span[num2] = new Vector2I(this.cells.GetLength(0) - 1, 0);
			num2++;
			*span[num2] = new Vector2I(this.cells.GetLength(0) - 1, this.cells.GetLength(0) - 1);
			num2++;
			*span[num2] = new Vector2I(0, this.cells.GetLength(0) - 1);
			List<Vector2I> list2 = list;
			for (int k = 0; k < 2; k++)
			{
				List<Vector2I> list3 = list2.Concat<Vector2I>(list2.SelectMany<Vector2I, Vector2I>((Vector2I c) => this.GetHorizontalCells(c.X, c.Y))).Concat<Vector2I>(list2.SelectMany<Vector2I, Vector2I>((Vector2I c) => this.GetVerticalCells(c.X, c.Y))).ToList<Vector2I>();
				list2 = list3;
			}
			foreach (Vector2I vector2I in list2)
			{
				TaskHelper.RunSafely(this.ClearCell(vector2I.X, vector2I.Y));
			}
			int num3 = 0;
			do
			{
				this.PlacedAllItems = this.PopulateItems();
				num3++;
			}
			while (!this.PlacedAllItems && num3 < 10);
			this.DivinationCount = divinationCount;
			this.CrystalSphereTool = CrystalSphereMinigame.CrystalSphereToolType.Big;
		}

		/// <summary>
		/// Ends the minigames early. Used if the event is force closed via save/quit or closing the application
		/// </summary>
		// Token: 0x060081FC RID: 33276 RVA: 0x002BFFC0 File Offset: 0x002BE1C0
		public void ForceMinigameEnd()
		{
			this._revealed.Clear();
			if (!this._completionSource.Task.IsCompleted)
			{
				this._completionSource.SetCanceled();
			}
		}

		// Token: 0x060081FD RID: 33277 RVA: 0x002BFFEC File Offset: 0x002BE1EC
		public async Task PlayMinigame()
		{
			if (LocalContext.IsMe(this._owner))
			{
				NCrystalSphereScreen.ShowScreen(this);
				await this._completionSource.Task;
				await this.CompleteMinigame();
			}
		}

		// Token: 0x060081FE RID: 33278 RVA: 0x002C0030 File Offset: 0x002BE230
		private bool PopulateItems()
		{
			bool flag = true;
			CrystalSphereItem crystalSphereItem = new CrystalSphereRelic();
			flag = flag && crystalSphereItem.PlaceItem(this);
			this._items.Add(crystalSphereItem);
			for (int i = 0; i < 2; i++)
			{
				CrystalSphereItem crystalSphereItem2 = new CrystalSpherePotion(PotionRarity.Common);
				flag = flag && crystalSphereItem2.PlaceItem(this);
				this._items.Add(crystalSphereItem2);
			}
			CrystalSphereItem crystalSphereItem3 = new CrystalSpherePotion(PotionRarity.Rare);
			flag = flag && crystalSphereItem3.PlaceItem(this);
			this._items.Add(crystalSphereItem3);
			CrystalSphereItem crystalSphereItem4 = new CrystalSphereCardReward(CardRarity.Common, this._owner);
			flag = flag && crystalSphereItem4.PlaceItem(this);
			this._items.Add(crystalSphereItem4);
			CrystalSphereItem crystalSphereItem5 = new CrystalSphereCardReward(CardRarity.Uncommon, this._owner);
			flag = flag && crystalSphereItem5.PlaceItem(this);
			this._items.Add(crystalSphereItem5);
			CrystalSphereItem crystalSphereItem6 = new CrystalSphereCardReward(CardRarity.Rare, this._owner);
			flag = flag && crystalSphereItem6.PlaceItem(this);
			this._items.Add(crystalSphereItem6);
			CrystalSphereItem crystalSphereItem7 = new CrystalSphereCurse();
			flag = flag && crystalSphereItem7.PlaceItem(this);
			this._items.Add(crystalSphereItem7);
			for (int j = 0; j < 5; j++)
			{
				CrystalSphereItem crystalSphereItem8 = new CrystalSphereGold(false);
				flag = flag && crystalSphereItem8.PlaceItem(this);
				this._items.Add(crystalSphereItem8);
			}
			for (int k = 0; k < 2; k++)
			{
				CrystalSphereItem crystalSphereItem9 = new CrystalSphereGold(true);
				flag = flag && crystalSphereItem9.PlaceItem(this);
				this._items.Add(crystalSphereItem9);
			}
			foreach (CrystalSphereItem crystalSphereItem10 in this._items)
			{
				crystalSphereItem10.Revealed += this.OnItemRevealed;
			}
			return flag;
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x002C0210 File Offset: 0x002BE410
		private void OnItemRevealed(CrystalSphereItem item)
		{
			this._revealed.Add(item);
		}

		// Token: 0x06008200 RID: 33280 RVA: 0x002C0220 File Offset: 0x002BE420
		public unsafe void SetHoveredCell(CrystalSphereCell cell)
		{
			if (this.HoveredCell != null)
			{
				this.UnsetHoveredCell();
			}
			this.HoveredCell = cell;
			this.HoveredCell.IsHovered = true;
			if (this.CrystalSphereTool == CrystalSphereMinigame.CrystalSphereToolType.Big)
			{
				this.HighlightedCells = (from c in this.GetAdjacentCells(cell.X, cell.Y)
					select this.cells[c.X, c.Y]).ToList<CrystalSphereCell>();
			}
			else
			{
				int num = 1;
				List<CrystalSphereCell> list = new List<CrystalSphereCell>(num);
				CollectionsMarshal.SetCount<CrystalSphereCell>(list, num);
				Span<CrystalSphereCell> span = CollectionsMarshal.AsSpan<CrystalSphereCell>(list);
				int num2 = 0;
				*span[num2] = this.cells[cell.X, cell.Y];
				this.HighlightedCells = list;
			}
			foreach (CrystalSphereCell crystalSphereCell in this.HighlightedCells)
			{
				crystalSphereCell.IsHighlighted = true;
			}
		}

		// Token: 0x06008201 RID: 33281 RVA: 0x002C0310 File Offset: 0x002BE510
		public void UnsetHoveredCell()
		{
			foreach (CrystalSphereCell crystalSphereCell in this.HighlightedCells)
			{
				crystalSphereCell.IsHighlighted = false;
				crystalSphereCell.IsHovered = false;
			}
			this.HighlightedCells = new List<CrystalSphereCell>();
			this.HoveredCell = null;
		}

		// Token: 0x06008202 RID: 33282 RVA: 0x002C037C File Offset: 0x002BE57C
		public void SetTool(CrystalSphereMinigame.CrystalSphereToolType tool)
		{
			this.CrystalSphereTool = tool;
			if (this.HoveredCell != null)
			{
				this.SetHoveredCell(this.HoveredCell);
			}
		}

		// Token: 0x06008203 RID: 33283 RVA: 0x002C039C File Offset: 0x002BE59C
		public async Task CellClicked(CrystalSphereCell clickedCell)
		{
			int divinationCount = this.DivinationCount;
			this.DivinationCount = divinationCount - 1;
			if (this.CrystalSphereTool == CrystalSphereMinigame.CrystalSphereToolType.Big)
			{
				List<Vector2I> adjacentCells = this.GetAdjacentCells(clickedCell.X, clickedCell.Y);
				foreach (Vector2I vector2I in adjacentCells)
				{
					await this.ClearCell(vector2I.X, vector2I.Y);
				}
				List<Vector2I>.Enumerator enumerator = default(List<Vector2I>.Enumerator);
			}
			else
			{
				await this.ClearCell(clickedCell.X, clickedCell.Y);
			}
			if (this.DivinationCount == 0)
			{
				this._completionSource.SetResult();
			}
		}

		/// <summary>
		/// Decreases the FogValue of a CrystalSphereCell at the given x,y coordinate by amount
		/// </summary>
		/// <param name="x">X coordinate the cell we want to affect is at</param>
		/// <param name="y">Y coordinate the cell we want to affect is at</param>
		// Token: 0x06008204 RID: 33284 RVA: 0x002C03E8 File Offset: 0x002BE5E8
		private async Task ClearCell(int x, int y)
		{
			if (x < 0 || x >= this.GridSize.X)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(x);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendFormatted<int>(y);
				defaultInterpolatedStringHandler.AppendLiteral("] is not a valid cell on this grid");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (y < 0 || y >= this.GridSize.Y)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(x);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendFormatted<int>(y);
				defaultInterpolatedStringHandler.AppendLiteral("] is not a valid cell on this grid");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this.cells[x, y].IsHidden)
			{
				this.cells[x, y].IsHidden = false;
				if (this.cells[x, y].Item != null)
				{
					CrystalSphereItem item = this.cells[x, y].Item;
					if (this.AreAllOccupiedCellsClear(item))
					{
						await item.RevealItem(this._owner);
					}
				}
			}
		}

		/// <summary>
		/// Returns if all the cells an item occupies are visible
		/// </summary>
		/// <param name="item">The item we are checking is fully visible</param>
		/// <returns></returns>
		// Token: 0x06008205 RID: 33285 RVA: 0x002C043C File Offset: 0x002BE63C
		private bool AreAllOccupiedCellsClear(CrystalSphereItem item)
		{
			for (int i = 0; i < item.Size.X; i++)
			{
				for (int j = 0; j < item.Size.Y; j++)
				{
					int num = item.Position.X + i;
					int num2 = item.Position.Y + j;
					if (this.cells[num, num2].IsHidden)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008206 RID: 33286 RVA: 0x002C04A8 File Offset: 0x002BE6A8
		private async Task CompleteMinigame()
		{
			await Cmd.Wait(0.75f, false);
			await RunManager.Instance.OneOffSynchronizer.DoLocalCrystalSphereRewards(this._owner, this.Rng, this._revealed);
			Action finished = this.Finished;
			if (finished != null)
			{
				finished();
			}
		}

		// Token: 0x06008207 RID: 33287 RVA: 0x002C04EB File Offset: 0x002BE6EB
		private List<Vector2I> GetAdjacentCells(int x, int y)
		{
			return this.GetHorizontalCells(x, y).Concat<Vector2I>(this.GetVerticalCells(x, y)).Concat<Vector2I>(this.GetDiagonalCells(x, y))
				.Concat<Vector2I>(new <>z__ReadOnlySingleElementList<Vector2I>(new Vector2I(x, y)))
				.ToList<Vector2I>();
		}

		// Token: 0x06008208 RID: 33288 RVA: 0x002C0528 File Offset: 0x002BE728
		private List<Vector2I> GetHorizontalCells(int x, int y)
		{
			List<Vector2I> list = new List<Vector2I>();
			for (int i = -1; i <= 1; i += 2)
			{
				int num = x + i;
				if (num >= 0 && num < 11)
				{
					list.Add(new Vector2I(num, y));
				}
			}
			return list;
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x002C0564 File Offset: 0x002BE764
		private List<Vector2I> GetVerticalCells(int x, int y)
		{
			List<Vector2I> list = new List<Vector2I>();
			for (int i = -1; i <= 1; i += 2)
			{
				int num = y + i;
				if (num >= 0 && num < 11)
				{
					list.Add(new Vector2I(x, num));
				}
			}
			return list;
		}

		// Token: 0x0600820A RID: 33290 RVA: 0x002C05A0 File Offset: 0x002BE7A0
		private List<Vector2I> GetDiagonalCells(int x, int y)
		{
			List<Vector2I> list = new List<Vector2I>();
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					int num = x + i;
					int num2 = y + j;
					if (num >= 0 && num < 11 && num2 >= 0 && num2 < 11)
					{
						list.Add(new Vector2I(num, num2));
					}
				}
			}
			return list;
		}

		// Token: 0x04002B88 RID: 11144
		private const int _defaultWidth = 11;

		// Token: 0x04002B89 RID: 11145
		private const int _defaultHeight = 11;

		// Token: 0x04002B8A RID: 11146
		private int _divinationCount;

		// Token: 0x04002B8F RID: 11151
		private readonly TaskCompletionSource _completionSource = new TaskCompletionSource();

		// Token: 0x04002B90 RID: 11152
		private readonly Player _owner;

		// Token: 0x04002B91 RID: 11153
		public CrystalSphereCell[,] cells;

		/// <summary>
		/// Set of items on the grid
		/// </summary>
		// Token: 0x04002B92 RID: 11154
		private readonly List<CrystalSphereItem> _items = new List<CrystalSphereItem>();

		/// <summary>
		/// Items revealed by the player.
		/// Rewards for these items are given to the player once the minigame is completed
		/// </summary>
		// Token: 0x04002B93 RID: 11155
		private readonly List<CrystalSphereItem> _revealed = new List<CrystalSphereItem>();

		// Token: 0x02002389 RID: 9097
		[NullableContext(0)]
		public enum CrystalSphereToolType
		{
			// Token: 0x04009682 RID: 38530
			None,
			// Token: 0x04009683 RID: 38531
			Small,
			// Token: 0x04009684 RID: 38532
			Big
		}
	}
}
