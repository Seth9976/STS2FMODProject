using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Rewards;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// The asset sets tend to get really large so this class just holds the big statically defined sets so they don't
	/// clutter the PreloadManager class.
	/// </summary>
	// Token: 0x02000D90 RID: 3472
	[NullableContext(1)]
	[Nullable(0)]
	public static class AssetSets
	{
		/// <summary>
		/// Minimal assets needed to display main menu immediately after logo animation.
		/// Does NOT include compendium screens, character select, or other submenus.
		/// </summary>
		// Token: 0x17002659 RID: 9817
		// (get) Token: 0x06008B9E RID: 35742 RVA: 0x002E9A01 File Offset: 0x002E7C01
		public static IReadOnlySet<string> MainMenuEssentials { get; }

		// Token: 0x1700265A RID: 9818
		// (get) Token: 0x06008B9F RID: 35743 RVA: 0x002E9A08 File Offset: 0x002E7C08
		public static IReadOnlySet<string> IntroLogoAssets { get; }

		// Token: 0x1700265B RID: 9819
		// (get) Token: 0x06008BA0 RID: 35744 RVA: 0x002E9A0F File Offset: 0x002E7C0F
		public static IReadOnlySet<string> CommonAssets { get; }

		// Token: 0x1700265C RID: 9820
		// (get) Token: 0x06008BA1 RID: 35745 RVA: 0x002E9A16 File Offset: 0x002E7C16
		public static IReadOnlySet<string> MainMenuSet { get; }

		// Token: 0x1700265D RID: 9821
		// (get) Token: 0x06008BA2 RID: 35746 RVA: 0x002E9A1D File Offset: 0x002E7C1D
		// (set) Token: 0x06008BA3 RID: 35747 RVA: 0x002E9A24 File Offset: 0x002E7C24
		public static IReadOnlySet<string> RunSet { get; set; }

		// Token: 0x1700265E RID: 9822
		// (get) Token: 0x06008BA4 RID: 35748 RVA: 0x002E9A2C File Offset: 0x002E7C2C
		// (set) Token: 0x06008BA5 RID: 35749 RVA: 0x002E9A33 File Offset: 0x002E7C33
		public static IReadOnlySet<string> Act { get; set; }

		// Token: 0x1700265F RID: 9823
		// (get) Token: 0x06008BA6 RID: 35750 RVA: 0x002E9A3C File Offset: 0x002E7C3C
		private static IEnumerable<string> CardMaterialPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					"res://materials/cards/banners/card_banner_common_mat.tres", "res://materials/cards/banners/card_banner_uncommon_mat.tres", "res://materials/cards/banners/card_banner_rare_mat.tres", "res://materials/cards/banners/card_banner_curse_mat.tres", "res://materials/cards/banners/card_banner_status_mat.tres", "res://materials/cards/banners/card_banner_event_mat.tres", "res://materials/cards/banners/card_banner_quest_mat.tres", "res://materials/cards/banners/card_banner_ancient_mat.tres", "res://materials/cards/frames/card_frame_red_mat.tres", "res://materials/cards/frames/card_frame_green_mat.tres",
					"res://materials/cards/frames/card_frame_blue_mat.tres", "res://materials/cards/frames/card_frame_pink_mat.tres", "res://materials/cards/frames/card_frame_orange_mat.tres", "res://materials/cards/frames/card_frame_colorless_mat.tres", "res://materials/cards/frames/card_frame_curse_mat.tres", "res://materials/cards/frames/card_frame_quest_mat.tres"
				});
			}
		}

		// Token: 0x06008BA7 RID: 35751 RVA: 0x002E9ADC File Offset: 0x002E7CDC
		// Note: this type is marked as 'beforefieldinit'.
		static AssetSets()
		{
			List<string> list = new List<string>();
			list.AddRange(NMainMenu.AssetPaths);
			list.AddRange(NTransition.AssetPaths);
			list.AddRange(NSettingsScreen.AssetPaths);
			list.Add("res://shaders/hsv.gdshader");
			list.Add("res://shaders/dark_blur.gdshader");
			AssetSets.MainMenuEssentials = new HashSet<string>(new <>z__ReadOnlyList<string>(list));
			List<string> list2 = new List<string>();
			list2.AddRange(NTransition.AssetPaths);
			list2.AddRange(NLogoAnimation.AssetPaths);
			AssetSets.IntroLogoAssets = new HashSet<string>(new <>z__ReadOnlyList<string>(list2));
			AssetSets.CommonAssets = new HashSet<string>(new IEnumerable<string>[]
			{
				NActBanner.AssetPaths,
				NActHistoryEntry.AssetPaths,
				NMultiplayerVoteContainer.AssetPaths,
				NAncientMapPoint.AssetPaths,
				NNormalMapPoint.AssetPaths,
				NAncientNameBanner.AssetPaths,
				NBossMapPoint.AssetPaths,
				NInspectCardScreen.AssetPaths,
				NInspectRelicScreen.AssetPaths,
				NSettingsScreen.AssetPaths,
				new <>z__ReadOnlySingleElementList<string>(NDevConsole.assetPath),
				NCardPileScreen.AssetPaths,
				NCardRewardSelectionScreen.AssetPaths,
				NChooseABundleSelectionScreen.AssetPaths,
				NChooseACardSelectionScreen.AssetPaths,
				NCombatPileCardSelectScreen.AssetPaths,
				NCombatStartBanner.AssetPaths,
				NCreature.AssetPaths,
				NDailyRunLeaderboard.AssetPaths,
				NDeckCardSelectScreen.AssetPaths,
				NDeckEnchantSelectScreen.AssetPaths,
				NDeckHistoryEntry.AssetPaths,
				NDeckUpgradeSelectScreen.AssetPaths,
				NEndTurnButton.AssetPaths,
				NEnemyTurnBanner.AssetPaths,
				NEnergyCounter.AssetPaths,
				NEventOptionButton.AssetPaths,
				NEventRoom.AssetPaths,
				NHandCardHolder.AssetPaths,
				NHoverTipSet.AssetPaths,
				NIntent.AssetPaths,
				NLinkedRewardSet.AssetPaths,
				NMapScreen.AssetPaths,
				NOrb.AssetPaths,
				NOrbManager.AssetPaths,
				NPlayerTurnBanner.AssetPaths,
				NPotion.AssetPaths,
				NPotionHolder.AssetPaths,
				NPotionPopup.AssetPaths,
				NPower.AssetPaths,
				NRelic.AssetPaths,
				NMultiplayerPlayerState.AssetPaths,
				NRestSiteButton.AssetPaths,
				NRestSiteRoom.AssetPaths,
				NRewardButton.AssetPaths,
				NRewardsScreen.AssetPaths,
				NMapPointHistoryEntry.AssetPaths,
				NRun.AssetPaths,
				NSelectedHandCardHolder.AssetPaths,
				NSimpleCardSelectScreen.AssetPaths,
				NSimpleCardsViewScreen.AssetPaths,
				NSpeechBubbleVfx.AssetPaths,
				NStarCounter.AssetPaths,
				NErrorPopup.AssetPaths,
				NTargetingArrow.AssetPaths,
				NThoughtBubbleVfx.AssetPaths,
				NVerticalPopup.AssetPaths,
				NBlockBrokenVfx.AssetPaths,
				NBlockSparkVfx.AssetPaths,
				NCardBundle.AssetPaths,
				NHorizontalLinesVfx.AssetPaths,
				NCardFlyPowerVfx.AssetPaths,
				NCardFlyShuffleVfx.AssetPaths,
				NCardFlyVfx.AssetPaths,
				NCardRareGlow.AssetPaths,
				NCardSmithVfx.AssetPaths,
				NCardTransformVfx.AssetPaths,
				NCardUncommonGlow.AssetPaths,
				NCardUpgradeVfx.AssetPaths,
				NDamageBlockedVfx.AssetPaths,
				NDamageNumVfx.AssetPaths,
				NDeckViewScreen.AssetPaths,
				NDoomVfx.AssetPaths,
				NGainEpochVfx.AssetPaths,
				NGridCardHolder.AssetPaths,
				NHitSparkVfx.AssetPaths,
				NMapCircleVfx.AssetPaths,
				NMapNodeSelectVfx.AssetPaths,
				NMonsterDeathVfx.AssetPaths,
				NPowerAppliedVfx.AssetPaths,
				NPowerFlashVfx.AssetPaths,
				NPowerRemovedVfx.AssetPaths,
				NPowerUpVfx.AssetPaths,
				NPreviewCardHolder.AssetPaths,
				NRelicFlashVfx.AssetPaths,
				NSmokyVignetteVfx.AssetPaths,
				NStunnedVfx.AssetPaths,
				NUiFlashVfx.AssetPaths,
				VfxCmd.AssetPaths,
				ControllerConfig.AllAssetPaths,
				NTransition.AssetPaths,
				Enum.GetValues<RewardType>().SelectMany<RewardType, string>((RewardType t) => t.GetAssetPaths()),
				TmpSfx.assetPaths,
				NPowerAppliedBuffVfx.AssetPaths,
				NPowerAppliedDebuffVfx.AssetPaths,
				PlayerFullscreenHealVfx.AssetPaths,
				NFireSmokePuffVfx.AssetPaths,
				NThinSliceVfx.AssetPaths,
				new <>z__ReadOnlySingleElementList<string>("res://materials/vfx/hsv.tres")
			}.SelectMany<IEnumerable<string>, string>((IEnumerable<string> s) => s).Concat<string>(AssetSets.CardMaterialPaths));
			AssetSets.MainMenuSet = new HashSet<string>(new IEnumerable<string>[]
			{
				NCharacterSelectScreen.AssetPaths,
				NMainMenu.AssetPaths,
				NAbandonRunConfirmPopup.AssetPaths,
				NGenericPopup.AssetPaths,
				NMultiplayerWarningPopup.AssetPaths,
				NCard.AssetPaths,
				NMultiplayerLoadGameScreen.AssetPaths,
				NBestiary.AssetPaths,
				NRelicCollection.AssetPaths,
				NPotionLab.AssetPaths,
				NCardLibrary.AssetPaths,
				NRunHistory.AssetPaths,
				NStatsScreen.AssetPaths,
				NTimelineScreen.AssetPaths,
				NDailyRunScreen.AssetPaths,
				NDailyRunLoadScreen.AssetPaths,
				NCustomRunScreen.AssetPaths,
				NCustomRunLoadScreen.AssetPaths,
				NModdingScreen.AssetPaths,
				NBestiaryEntry.AssetPaths,
				NProfileScreen.AssetPaths,
				NAchievementsGrid.AssetPaths,
				ModelDb.AllCharacters.SelectMany<CharacterModel, string>((CharacterModel character) => character.AssetPathsCharacterSelect)
			}.SelectMany<IEnumerable<string>, string>((IEnumerable<string> s) => s));
			AssetSets.RunSet = null;
			AssetSets.Act = null;
		}
	}
}
