using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent
{
	/// <summary>
	/// The items that are on the grid for the Crystal Sphere minigame
	/// </summary>
	// Token: 0x02000C4F RID: 3151
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CrystalSphereItem
	{
		// Token: 0x17002371 RID: 9073
		// (get) Token: 0x060081D9 RID: 33241
		public abstract Vector2I Size { get; }

		// Token: 0x17002372 RID: 9074
		// (get) Token: 0x060081DA RID: 33242 RVA: 0x002BF976 File Offset: 0x002BDB76
		// (set) Token: 0x060081DB RID: 33243 RVA: 0x002BF97E File Offset: 0x002BDB7E
		public Vector2I Position { get; private set; }

		// Token: 0x17002373 RID: 9075
		// (get) Token: 0x060081DC RID: 33244 RVA: 0x002BF987 File Offset: 0x002BDB87
		protected virtual string TexturePath
		{
			get
			{
				return ImageHelper.GetImagePath("events/crystal_sphere/" + StringExtensions.ToSnakeCase(base.GetType().Name) + ".png");
			}
		}

		// Token: 0x17002374 RID: 9076
		// (get) Token: 0x060081DD RID: 33245 RVA: 0x002BF9AD File Offset: 0x002BDBAD
		public Texture2D Texture
		{
			get
			{
				return PreloadManager.Cache.GetTexture2D(this.TexturePath);
			}
		}

		// Token: 0x17002375 RID: 9077
		// (get) Token: 0x060081DE RID: 33246
		public abstract bool IsGood { get; }

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x060081DF RID: 33247 RVA: 0x002BF9C0 File Offset: 0x002BDBC0
		// (remove) Token: 0x060081E0 RID: 33248 RVA: 0x002BF9F8 File Offset: 0x002BDBF8
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CrystalSphereItem> Revealed;

		/// <summary>
		/// Places an item randomly on the minigame's grid
		/// </summary>
		/// <param name="game"></param>
		/// <exception cref="T:System.InvalidOperationException"></exception>
		// Token: 0x060081E1 RID: 33249 RVA: 0x002BFA30 File Offset: 0x002BDC30
		public bool PlaceItem(CrystalSphereMinigame game)
		{
			List<Vector2I> list = new List<Vector2I>();
			for (int i = 0; i < game.GridSize.X; i++)
			{
				for (int j = 0; j < game.GridSize.Y; j++)
				{
					if (this.CanPlaceHere(game.cells, i, j))
					{
						list.Add(new Vector2I(i, j));
					}
				}
			}
			if (!list.Any<Vector2I>())
			{
				return false;
			}
			this.Position = game.Rng.NextItem<Vector2I>(list);
			for (int k = 0; k < this.Size.X; k++)
			{
				for (int l = 0; l < this.Size.Y; l++)
				{
					int num = this.Position.X + k;
					int num2 = this.Position.Y + l;
					game.cells[num, num2].SetItem(this);
				}
			}
			return true;
		}

		/// <summary>
		/// Returns true if this coordinate is a valid place to put this item
		/// </summary>
		/// <param name="grid">Grid we are doing the placement on</param>
		/// <param name="x">Potential X coord we are validating</param>
		/// <param name="y">Potential Y coord we are validating</param>
		/// <returns></returns>
		// Token: 0x060081E2 RID: 33250 RVA: 0x002BFB10 File Offset: 0x002BDD10
		private bool CanPlaceHere(CrystalSphereCell[,] grid, int x, int y)
		{
			for (int i = 0; i < this.Size.X; i++)
			{
				for (int j = 0; j < this.Size.Y; j++)
				{
					int num = x + i;
					int num2 = y + j;
					if (num < 0 || num >= grid.GetLength(0))
					{
						return false;
					}
					if (num2 < 0 || num2 >= grid.GetLength(1))
					{
						return false;
					}
					if (!grid[num, num2].IsHidden)
					{
						return false;
					}
					if (grid[num, num2].Item != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060081E3 RID: 33251 RVA: 0x002BFB94 File Offset: 0x002BDD94
		public virtual Task RevealItem(Player _)
		{
			Action<CrystalSphereItem> revealed = this.Revealed;
			if (revealed != null)
			{
				revealed(this);
			}
			return Task.CompletedTask;
		}

		// Token: 0x060081E4 RID: 33252 RVA: 0x002BFBAD File Offset: 0x002BDDAD
		[return: Nullable(2)]
		public virtual Reward ToReward(Player owner, Rng rng)
		{
			return null;
		}

		// Token: 0x060081E5 RID: 33253
		public abstract SerializableCrystalSphereItem ToSerializable();

		// Token: 0x060081E6 RID: 33254 RVA: 0x002BFBB0 File Offset: 0x002BDDB0
		public static CrystalSphereItem FromSerializable(SerializableCrystalSphereItem serializable, Player owner)
		{
			CrystalSphereItem crystalSphereItem;
			switch (serializable.type)
			{
			case CrystalSphereItemType.CardReward:
				crystalSphereItem = new CrystalSphereCardReward(serializable.cardRarity, owner);
				break;
			case CrystalSphereItemType.Curse:
				crystalSphereItem = new CrystalSphereCurse();
				break;
			case CrystalSphereItemType.Gold:
				crystalSphereItem = new CrystalSphereGold(serializable.isBigGold);
				break;
			case CrystalSphereItemType.Potion:
				crystalSphereItem = new CrystalSpherePotion(serializable.potionRarity);
				break;
			case CrystalSphereItemType.Relic:
				crystalSphereItem = new CrystalSphereRelic();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return crystalSphereItem;
		}
	}
}
