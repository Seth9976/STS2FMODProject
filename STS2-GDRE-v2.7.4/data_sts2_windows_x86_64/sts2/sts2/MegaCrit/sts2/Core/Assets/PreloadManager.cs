using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// This class manages the caching and preloading of assets for the game.
	/// </summary>
	// Token: 0x02000D93 RID: 3475
	[NullableContext(1)]
	[Nullable(0)]
	public static class PreloadManager
	{
		// Token: 0x17002660 RID: 9824
		// (get) Token: 0x06008BCD RID: 35789 RVA: 0x002EB95D File Offset: 0x002E9B5D
		public static AssetCache Cache { get; } = new AssetCache();

		/// <summary>
		/// Set this to false to disable preloading of assets. This can speed up iteration times.
		/// Assets will still be unloaded when we think they are out of use.
		/// </summary>
		// Token: 0x17002661 RID: 9825
		// (get) Token: 0x06008BCE RID: 35790 RVA: 0x002EB964 File Offset: 0x002E9B64
		// (set) Token: 0x06008BCF RID: 35791 RVA: 0x002EB96B File Offset: 0x002E9B6B
		public static bool Enabled { get; set; } = true;

		/// <summary>
		/// This method loads only the logo animation. The logo animation will be unloaded as soon as LoadMainMenuAssets
		/// is called, which is what we want.
		/// The assets are loaded synchronously.
		/// </summary>
		// Token: 0x06008BD0 RID: 35792 RVA: 0x002EB974 File Offset: 0x002E9B74
		public static async Task LoadLogoAnimation()
		{
			AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("IntroLogo", new IEnumerable<string>[] { AssetSets.IntroLogoAssets });
			AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
			await assetLoadingSession2.WaitForCompletion();
		}

		/// <summary>
		/// Loads only the essential assets needed to display the main menu.
		/// Call this while the logo animation is playing for faster main menu display.
		/// </summary>
		// Token: 0x06008BD1 RID: 35793 RVA: 0x002EB9B0 File Offset: 0x002E9BB0
		public static async Task LoadMainMenuEssentials()
		{
			if (!TestMode.IsOn)
			{
				AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("MainMenuEssentials", new IEnumerable<string>[] { AssetSets.MainMenuEssentials });
				AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
				await assetLoadingSession2.WaitForCompletion();
			}
		}

		/// <summary>
		/// Loads all common and main menu assets including compendium screens, character select, etc.
		/// Call this in background after main menu is displayed.
		/// </summary>
		// Token: 0x06008BD2 RID: 35794 RVA: 0x002EB9EC File Offset: 0x002E9BEC
		public static async Task LoadCommonAndMainMenuAssets()
		{
			PreloadManager.Cache.UnloadMissedCacheAssets();
			await PreloadManager.LoadAssetSets("Common", new IEnumerable<string>[]
			{
				AssetSets.CommonAssets,
				AssetSets.MainMenuSet
			});
		}

		/// <summary>
		/// This method is only used when starting the game, it loads the main menu assets. Otherwise, we use the
		/// LoadCommonAndMainMenuAssets method.
		/// </summary>
		// Token: 0x06008BD3 RID: 35795 RVA: 0x002EBA28 File Offset: 0x002E9C28
		public static async Task LoadMainMenuAssets()
		{
			if (!TestMode.IsOn)
			{
				AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("MainMenu", new IEnumerable<string>[] { AssetSets.MainMenuSet });
				AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
				await assetLoadingSession2.WaitForCompletion();
			}
		}

		// Token: 0x06008BD4 RID: 35796 RVA: 0x002EBA64 File Offset: 0x002E9C64
		public static async Task LoadRunAssets(IEnumerable<CharacterModel> characters)
		{
			if (!TestMode.IsOn)
			{
				List<CharacterModel> list = characters.ToList<CharacterModel>();
				bool flag = RunManager.Instance.NetService.Type.IsMultiplayer();
				AssetSets.RunSet = new HashSet<string>(PreloadManager.GetRunAssetPaths(list, flag));
				AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("characters=" + string.Join<string>(',', list.Select<CharacterModel, string>((CharacterModel c) => c.Id.Entry)), new IEnumerable<string>[]
				{
					AssetSets.CommonAssets,
					AssetSets.RunSet
				});
				AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
				await assetLoadingSession2.WaitForCompletion();
				GC.Collect();
			}
		}

		// Token: 0x06008BD5 RID: 35797 RVA: 0x002EBAA8 File Offset: 0x002E9CA8
		public static async Task LoadActAssets(ActModel act)
		{
			if (!TestMode.IsOn)
			{
				AssetSets.Act = new HashSet<string>(act.AssetPaths);
				AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("Act=" + act.Id.Entry, new IEnumerable<string>[]
				{
					AssetSets.CommonAssets,
					AssetSets.RunSet,
					AssetSets.Act
				});
				AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
				await assetLoadingSession2.WaitForCompletion();
				GC.Collect();
			}
		}

		// Token: 0x06008BD6 RID: 35798 RVA: 0x002EBAEC File Offset: 0x002E9CEC
		public static async Task LoadRoomEventAssets(EventModel eventModel, IRunState runState)
		{
			await PreloadManager.LoadRoomAssets("Event Room", eventModel.GetAssetPaths(runState));
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x002EBB38 File Offset: 0x002E9D38
		public static async Task LoadRoomCombatAssets(EncounterModel encounter, IRunState runState)
		{
			await PreloadManager.LoadRoomAssets("Combat Room", PreloadManager.GetCombatAssetPaths(encounter, runState));
		}

		// Token: 0x06008BD8 RID: 35800 RVA: 0x002EBB84 File Offset: 0x002E9D84
		public static async Task LoadRoomTreasureAssets(ActModel actModel)
		{
			string text = "Treasure Room";
			List<string> list = new List<string>();
			list.Add(actModel.ChestSpineResourcePath);
			list.AddRange(NTreasureRoom.AssetPaths);
			await PreloadManager.LoadRoomAssets(text, new <>z__ReadOnlyList<string>(list));
		}

		// Token: 0x06008BD9 RID: 35801 RVA: 0x002EBBC8 File Offset: 0x002E9DC8
		public static async Task LoadRoomMerchantAssets()
		{
			await PreloadManager.LoadRoomAssets("Merchant Room", NMerchantRoom.AssetPaths);
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x002EBC04 File Offset: 0x002E9E04
		public static async Task LoadRoomRestSite(ActModel actModel, IEnumerable<RestSiteOption> restSiteOptions)
		{
			string text = "RestSite Room";
			List<string> list = new List<string>();
			list.Add(actModel.RestSiteBackgroundPath);
			list.AddRange(restSiteOptions.SelectMany<RestSiteOption, string>((RestSiteOption s) => s.AssetPaths));
			await PreloadManager.LoadRoomAssets(text, new <>z__ReadOnlyList<string>(list));
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x002EBC50 File Offset: 0x002E9E50
		private static async Task LoadRoomAssets(string roomName, IEnumerable<string> additionalAssets)
		{
			if (!TestMode.IsOn)
			{
				PreloadManager.Cache.UnloadMissedCacheAssets();
				HashSet<string> hashSet = new HashSet<string>();
				foreach (string text in additionalAssets)
				{
					hashSet.Add(text);
				}
				HashSet<string> hashSet2 = hashSet;
				AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets(roomName, new IEnumerable<string>[]
				{
					AssetSets.CommonAssets,
					AssetSets.RunSet,
					AssetSets.Act,
					hashSet2
				});
				AssetLoadingSession assetLoadingSession2 = assetLoadingSession;
				await assetLoadingSession2.WaitForCompletion();
				GC.Collect();
			}
		}

		/// <summary>
		/// This method is the magic for preloading assets. It unloads assets that are no longer needed and loads assets
		/// that are needed.
		/// It does this through a basic set subtraction algorithm to determine what assets are not needed and can be
		/// unloaded and what assets are needed and not yet loaded (this avoids loading assets that already exist in memory).
		/// </summary>
		// Token: 0x06008BDC RID: 35804 RVA: 0x002EBC9C File Offset: 0x002E9E9C
		private static async Task<AssetLoadingSession> LoadAssetSets(string name, params IEnumerable<string>[] assetSets)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in assetSets.SelectMany<IEnumerable<string>, string>((IEnumerable<string> set) => set))
			{
				hashSet.Add(text);
			}
			HashSet<string> hashSet2 = hashSet;
			IReadOnlySet<string> loadedCacheAssets = PreloadManager.Cache.GetLoadedCacheAssets();
			IEnumerable<string> enumerable = loadedCacheAssets.Except<string>(hashSet2);
			IEnumerable<string> needLoaded = hashSet2.Except<string>(loadedCacheAssets);
			PreloadManager.Cache.UnloadAssets(enumerable);
			await Task.Yield();
			AssetLoadingSession assetLoadingSession;
			if (!PreloadManager.Enabled)
			{
				assetLoadingSession = AssetLoadingSession.Empty();
			}
			else
			{
				assetLoadingSession = PreloadManager.LoadAssets(needLoaded, name);
			}
			return assetLoadingSession;
		}

		// Token: 0x06008BDD RID: 35805 RVA: 0x002EBCE8 File Offset: 0x002E9EE8
		private static AssetLoadingSession LoadAssets(IEnumerable<string> assetPaths, string name)
		{
			AssetLoadingSession assetLoadingSession = PreloadManager.Cache.CreateSession(name, assetPaths);
			TaskHelper.RunSafely(NAssetLoader.Instance.LoadInTheBackground(assetLoadingSession));
			return assetLoadingSession;
		}

		// Token: 0x06008BDE RID: 35806 RVA: 0x002EBD14 File Offset: 0x002E9F14
		private static IEnumerable<string> GetRunAssetPaths(IEnumerable<CharacterModel> characters, bool isMultiplayer)
		{
			IEnumerable<CharacterModel> enumerable = characters.ToList<CharacterModel>();
			IEnumerable<CardPoolModel> allSharedCardPools = ModelDb.AllSharedCardPools;
			IEnumerable<CardModel> enumerable2 = allSharedCardPools.SelectMany<CardPoolModel, CardModel>((CardPoolModel pool) => pool.AllCards);
			if (!isMultiplayer)
			{
				enumerable2 = enumerable2.Where<CardModel>((CardModel card) => card.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);
			}
			IEnumerable<string>[] array = new IEnumerable<string>[10];
			array[0] = enumerable2.SelectMany<CardModel, string>((CardModel card) => card.RunAssetPaths);
			array[1] = enumerable.SelectMany<CharacterModel, string>((CharacterModel c) => c.CardPool.AllCards.SelectMany<CardModel, string>((CardModel card) => card.RunAssetPaths));
			array[2] = enumerable.SelectMany<CharacterModel, string>((CharacterModel c) => c.AssetPaths);
			array[3] = NCard.AssetPaths;
			array[4] = NMapRoom.AssetPaths;
			array[5] = NChooseACardSelectionScreen.AssetPaths;
			array[6] = NGameOverScreen.AssetPaths;
			array[7] = NRelicInventoryHolder.AssetPaths;
			array[8] = from e in ModelDb.DebugEnchantments
				select e.IconPath into p
				where p != EnchantmentModel.MissingIconPath
				select p;
			array[9] = ModelDb.AllCardPools.Select<CardPoolModel, string>((CardPoolModel p) => p.EnergyIconPath);
			return array.SelectMany<IEnumerable<string>, string>((IEnumerable<string> s) => s);
		}

		// Token: 0x06008BDF RID: 35807 RVA: 0x002EBEC8 File Offset: 0x002EA0C8
		private static IEnumerable<string> GetCombatAssetPaths(EncounterModel encounter, IRunState runState)
		{
			if (TestMode.IsOn)
			{
				return Array.Empty<string>();
			}
			return new IEnumerable<string>[]
			{
				NCombatRoom.AssetPaths,
				encounter.GetAssetPaths(runState)
			}.SelectMany<IEnumerable<string>, string>((IEnumerable<string> s) => s);
		}
	}
}
