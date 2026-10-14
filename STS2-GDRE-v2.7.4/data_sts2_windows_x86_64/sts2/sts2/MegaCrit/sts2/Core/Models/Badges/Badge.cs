using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Models.Badges
{
	/// <summary>
	/// Data class for Badges.
	/// This is the actual backing class with the data for the badge. BadgeModel only exists to listen to combat events.
	/// </summary>
	// Token: 0x02000B59 RID: 2905
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class Badge
	{
		// Token: 0x170021D3 RID: 8659
		// (get) Token: 0x06007AAC RID: 31404 RVA: 0x0029EE95 File Offset: 0x0029D095
		public string Id { get; }

		// Token: 0x170021D4 RID: 8660
		// (get) Token: 0x06007AAD RID: 31405
		public abstract BadgeRarity Rarity { get; }

		/// <summary>
		/// By default, a badge requires you to win to obtain it.
		/// </summary>
		// Token: 0x170021D5 RID: 8661
		// (get) Token: 0x06007AAE RID: 31406 RVA: 0x0029EE9D File Offset: 0x0029D09D
		public bool RequiresWin { get; }

		// Token: 0x170021D6 RID: 8662
		// (get) Token: 0x06007AAF RID: 31407 RVA: 0x0029EEA5 File Offset: 0x0029D0A5
		public bool MultiplayerOnly { get; }

		/// <summary>
		/// Given a Badge's Rarity, returns the appropriate asset that's used to hold the badge icon.
		/// </summary>
		// Token: 0x170021D7 RID: 8663
		// (get) Token: 0x06007AB0 RID: 31408 RVA: 0x0029EEB0 File Offset: 0x0029D0B0
		public Texture2D BadgeBase
		{
			get
			{
				Texture2D texture2D;
				switch (this.Rarity)
				{
				case BadgeRarity.Bronze:
					texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_bronze.png"));
					break;
				case BadgeRarity.Silver:
					texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_silver.png"));
					break;
				case BadgeRarity.Gold:
					texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_gold.png"));
					break;
				default:
					texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/power_atlas.sprites/missing_power.tres"));
					break;
				}
				return texture2D;
			}
		}

		// Token: 0x170021D8 RID: 8664
		// (get) Token: 0x06007AB1 RID: 31409 RVA: 0x0029EF38 File Offset: 0x0029D138
		public Texture2D BadgeIcon
		{
			get
			{
				if (ResourceLoader.Exists(this.IconPath, ""))
				{
					return PreloadManager.Cache.GetTexture2D(this.IconPath);
				}
				Log.Error("Badge Icon: " + this.IconPath + " doesn't exist :(", 2);
				return PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("debug/placeholder_64.png"));
			}
		}

		// Token: 0x170021D9 RID: 8665
		// (get) Token: 0x06007AB2 RID: 31410 RVA: 0x0029EF97 File Offset: 0x0029D197
		private string IconPath
		{
			get
			{
				return ImageHelper.GetImagePath("ui/game_over_screen/badge_" + this.Id.ToLowerInvariant() + ".png");
			}
		}

		// Token: 0x06007AB3 RID: 31411 RVA: 0x0029EFB8 File Offset: 0x0029D1B8
		protected Badge(SerializableRun run, bool won, ulong playerId, string id, bool requiresWin, bool multiplayerOnly)
		{
			this._run = run;
			this._won = won;
			this._localPlayer = this._run.Players.First<SerializablePlayer>((SerializablePlayer p) => p.NetId == playerId);
			this.Id = id;
			this.RequiresWin = requiresWin;
			this.MultiplayerOnly = multiplayerOnly;
		}

		// Token: 0x06007AB4 RID: 31412
		public abstract bool IsObtained();

		// Token: 0x06007AB5 RID: 31413 RVA: 0x0029F020 File Offset: 0x0029D220
		public SerializableBadge ToSerializable()
		{
			return new SerializableBadge
			{
				Id = this.Id,
				Rarity = this.Rarity
			};
		}

		// Token: 0x040028F4 RID: 10484
		protected readonly SerializableRun _run;

		// Token: 0x040028F5 RID: 10485
		protected readonly bool _won;

		// Token: 0x040028F6 RID: 10486
		protected readonly SerializablePlayer _localPlayer;
	}
}
