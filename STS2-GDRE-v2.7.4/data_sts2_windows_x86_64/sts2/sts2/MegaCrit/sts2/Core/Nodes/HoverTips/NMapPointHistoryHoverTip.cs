using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.HoverTips
{
	// Token: 0x0200039B RID: 923
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/HoverTips/NMapPointHistoryHoverTip.cs")]
	public class NMapPointHistoryHoverTip : MarginContainer
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x001A0ADB File Offset: 0x0019ECDB
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/ui/map_point_history_hover_tip.tscn");
			}
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x001A0AE8 File Offset: 0x0019ECE8
		public static NMapPointHistoryHoverTip Create(int floorNum, ulong playerId, MapPointHistoryEntry historyEntry)
		{
			NMapPointHistoryHoverTip nmapPointHistoryHoverTip = PreloadManager.Cache.GetScene("res://scenes/ui/map_point_history_hover_tip.tscn").Instantiate<NMapPointHistoryHoverTip>(0L);
			nmapPointHistoryHoverTip._entry = historyEntry;
			nmapPointHistoryHoverTip._floorNum = floorNum;
			nmapPointHistoryHoverTip._playerId = playerId;
			return nmapPointHistoryHoverTip;
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x001A0B24 File Offset: 0x0019ED24
		public override void _Ready()
		{
			this._titleLabel = base.GetNode<MegaLabel>("%Title");
			this._playerStats = base.GetNode<RichTextLabel>("%PlayerStats");
			this._roomStats = base.GetNode<RichTextLabel>("%RoomStats");
			this._actionStats = base.GetNode<RichTextLabel>("%CardStats");
			this._rewardStatsContainer = base.GetNode<Control>("%RewardStats");
			this._skippedStatsContainer = base.GetNode<Control>("%SkippedStats");
			this._rewardRows = base.GetNode<Control>("%RewardRows").GetChildren(false).OfType<RichTextLabel>()
				.ToList<RichTextLabel>();
			this._skippedRows = base.GetNode<Control>("%SkippedRows").GetChildren(false).OfType<RichTextLabel>()
				.ToList<RichTextLabel>();
			this._rewardStatsContainer.GetNode<MegaLabel>("Header").SetTextAutoSize(this._rewardsHeaderLoc.GetFormattedText());
			this._skippedStatsContainer.GetNode<MegaLabel>("Header").SetTextAutoSize(this._skippedHeaderLoc.GetFormattedText());
			this._floorTitle.Add("FloorNum", this._floorNum);
			this._titleLabel.SetTextAutoSize(this._floorTitle.GetFormattedText());
			string text;
			switch (this._entry.MapPointType)
			{
			case MapPointType.Shop:
				text = "ROOM_MERCHANT";
				break;
			case MapPointType.Treasure:
				text = "ROOM_TREASURE";
				break;
			case MapPointType.RestSite:
				text = "ROOM_REST";
				break;
			case MapPointType.Monster:
				text = "ROOM_ENEMY";
				break;
			case MapPointType.Elite:
				text = "ROOM_ELITE";
				break;
			case MapPointType.Boss:
				text = "ROOM_BOSS";
				break;
			case MapPointType.Ancient:
				text = "ROOM_ANCIENT";
				break;
			default:
				text = null;
				break;
			}
			string text2 = text;
			if (this._entry.MapPointType == MapPointType.Unknown)
			{
				switch (this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType)
				{
				case RoomType.Monster:
					text = "ROOM_UNKNOWN_ENEMY";
					goto IL_022B;
				case RoomType.Elite:
					text = "ROOM_UNKNOWN_ELITE";
					goto IL_022B;
				case RoomType.Treasure:
					text = "ROOM_UNKNOWN_TREASURE";
					goto IL_022B;
				case RoomType.Shop:
					text = "ROOM_UNKNOWN_MERCHANT";
					goto IL_022B;
				case RoomType.Event:
					text = "ROOM_EVENT";
					goto IL_022B;
				}
				text = null;
				IL_022B:
				text2 = text;
			}
			this._mapPointRoomStatsLoc.Add("MapPointType", new LocString("static_hover_tips", text2 + ".title"));
			switch (this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType)
			{
			case RoomType.Treasure:
			case RoomType.Shop:
			case RoomType.RestSite:
				this._mapPointRoomStatsLoc.Add("ModelTitle", "");
				break;
			case RoomType.Event:
				this._mapPointRoomStatsLoc.Add("ModelTitle", SaveUtil.EventOrDeprecated(this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId).Title);
				break;
			default:
				this._mapPointRoomStatsLoc.Add("ModelTitle", SaveUtil.EncounterOrDeprecated(this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId).Title);
				break;
			}
			this._roomStats.Text = this._mapPointRoomStatsLoc.GetFormattedText();
			PlayerMapPointHistoryEntry playerMapPointHistoryEntry = this._entry.PlayerStats.FirstOrDefault<PlayerMapPointHistoryEntry>((PlayerMapPointHistoryEntry e) => e.PlayerId == this._playerId);
			if (playerMapPointHistoryEntry == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Player with ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(this._playerId);
				defaultInterpolatedStringHandler.AppendLiteral(" not found in player stats for this run history!");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._mapPointPlayerStatsLoc.Add("HP", playerMapPointHistoryEntry.CurrentHp);
			this._mapPointPlayerStatsLoc.Add("MaxHP", playerMapPointHistoryEntry.MaxHp);
			this._mapPointPlayerStatsLoc.Add("Gold", playerMapPointHistoryEntry.CurrentGold);
			this._playerStats.Text = this._mapPointPlayerStatsLoc.GetFormattedText();
			this.PopulateActionStats(playerMapPointHistoryEntry);
			this.PopulateRewardAndSkippedEntries(playerMapPointHistoryEntry);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x001A0F14 File Offset: 0x0019F114
		private void PopulateActionStats(PlayerMapPointHistoryEntry playerEntry)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._entry.MapPointType == MapPointType.Ancient)
			{
				LocString ancientPickedChoiceLoc = playerEntry.GetAncientPickedChoiceLoc();
				if (ancientPickedChoiceLoc != null)
				{
					this._chose.Add("Choice", ancientPickedChoiceLoc);
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder3 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\t");
					appendInterpolatedStringHandler.AppendFormatted(this._chose.GetFormattedText());
					appendInterpolatedStringHandler.AppendLiteral("\n");
					stringBuilder3.Append(ref appendInterpolatedStringHandler);
				}
				using (List<LocString>.Enumerator enumerator = playerEntry.GetAncientSkippedChoiceLoc().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LocString locString = enumerator.Current;
						this._skipped.Add("Choice", locString);
						StringBuilder stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder4 = stringBuilder2;
						StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("\t");
						appendInterpolatedStringHandler.AppendFormatted(this._skipped.GetFormattedText());
						appendInterpolatedStringHandler.AppendLiteral("\n");
						stringBuilder4.Append(ref appendInterpolatedStringHandler);
					}
					goto IL_0227;
				}
			}
			MapPointRoomHistoryEntry mapPointRoomHistoryEntry = this._entry.FirstRoomOfType(RoomType.Event);
			ModelId modelId = ((mapPointRoomHistoryEntry != null) ? mapPointRoomHistoryEntry.ModelId : null);
			EventModel eventModel = ((modelId != null) ? SaveUtil.EventOrDeprecated(modelId) : null);
			if (eventModel != null)
			{
				foreach (EventOptionHistoryEntry eventOptionHistoryEntry in playerEntry.EventChoices)
				{
					eventModel.DynamicVars.AddTo(eventOptionHistoryEntry.Title);
					if (eventOptionHistoryEntry.Variables != null)
					{
						foreach (KeyValuePair<string, object> keyValuePair in eventOptionHistoryEntry.Variables)
						{
							eventOptionHistoryEntry.Title.AddObj(keyValuePair.Key, keyValuePair.Value);
						}
					}
					this._chose.Add("Choice", eventOptionHistoryEntry.Title.GetFormattedText());
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder5 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\t");
					appendInterpolatedStringHandler.AppendFormatted(this._chose.GetFormattedText());
					appendInterpolatedStringHandler.AppendLiteral("\n");
					stringBuilder5.Append(ref appendInterpolatedStringHandler);
				}
			}
			IL_0227:
			foreach (string text in playerEntry.RestSiteChoices)
			{
				LocString locString2 = new LocString("rest_site_ui", "OPTION_" + text + ".name");
				this._chose.Add("Choice", locString2.GetFormattedText() ?? "");
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder6 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._chose.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder6.Append(ref appendInterpolatedStringHandler);
			}
			MapPointRoomHistoryEntry mapPointRoomHistoryEntry2 = this._entry.Rooms.FirstOrDefault<MapPointRoomHistoryEntry>((MapPointRoomHistoryEntry r) => r.RoomType.IsCombatRoom());
			if (playerEntry.IsAffectedByFurCoat)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder7 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(FurCoat.HistoryEntry.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder7.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.MaxHpLost > 0)
			{
				this._maxHpLost.Add("HP", playerEntry.MaxHpLost);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder8 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._maxHpLost.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder8.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.DamageTaken > 0 || mapPointRoomHistoryEntry2 != null)
			{
				this._damaged.Add("Damage", playerEntry.DamageTaken);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder9 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._damaged.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder9.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.MaxHpGained > 0)
			{
				this._maxHpGained.Add("HP", playerEntry.MaxHpGained);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder10 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._maxHpGained.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder10.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.HpHealed > 0)
			{
				this._healed.Add("HP", playerEntry.HpHealed);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder11 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._healed.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder11.Append(ref appendInterpolatedStringHandler);
			}
			if (mapPointRoomHistoryEntry2 != null)
			{
				this._turns.Add("Turns", mapPointRoomHistoryEntry2.TurnsTaken);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder12 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._turns.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder12.Append(ref appendInterpolatedStringHandler);
			}
			foreach (ModelId modelId2 in playerEntry.CompletedQuests)
			{
				this._quests.Add("Quest", SaveUtil.CardOrDeprecated(modelId2).Title);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder13 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				appendInterpolatedStringHandler.AppendFormatted(this._quests.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder13.Append(ref appendInterpolatedStringHandler);
			}
			foreach (ModelId modelId3 in playerEntry.PotionUsed)
			{
				this._used.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
				this._used.Add("Title", SaveUtil.PotionOrDeprecated(modelId3).Title);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder14 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(this._used.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder14.Append(ref appendInterpolatedStringHandler);
			}
			foreach (ModelId modelId4 in playerEntry.PotionDiscarded)
			{
				this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
				this._removed.Add("Title", SaveUtil.PotionOrDeprecated(modelId4).Title);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder15 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(this._removed.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder15.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.GoldSpent > 0)
			{
				this._goldSpent.Add("Amount", playerEntry.GoldSpent);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder16 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(this._goldSpent.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder16.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.GoldLost > 0)
			{
				this._goldLost.Add("Amount", playerEntry.GoldLost);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder17 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(this._goldLost.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder17.Append(ref appendInterpolatedStringHandler);
			}
			if (playerEntry.GoldStolen > 0)
			{
				this._goldStolen.Add("Icon", "[img=top]res://images/packed/sprite_fonts/gold_icon.png[/img]");
				this._goldStolen.Add("Amount", playerEntry.GoldStolen);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder18 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(this._goldStolen.GetFormattedText());
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder18.Append(ref appendInterpolatedStringHandler);
			}
			this._actionStats.Text = stringBuilder.ToString().Trim('\n');
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x001A17A0 File Offset: 0x0019F9A0
		private void PopulateRewardAndSkippedEntries(PlayerMapPointHistoryEntry playerEntry)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (playerEntry.GoldGained > 0)
			{
				this._goldGained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/gold_icon.png[/img]");
				this._goldGained.Add("Amount", playerEntry.GoldGained);
				list.Add(this._goldGained.GetFormattedText());
			}
			foreach (SerializableCard serializableCard in playerEntry.CardsGained)
			{
				this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._obtained.Add("Title", CardModel.FromSerializable(serializableCard).Title);
				list.Add(this._obtained.GetFormattedText());
			}
			foreach (CardChoiceHistoryEntry cardChoiceHistoryEntry in playerEntry.CardChoices)
			{
				if (!cardChoiceHistoryEntry.wasPicked)
				{
					this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
					this._obtained.Add("Title", CardModel.FromSerializable(cardChoiceHistoryEntry.Card).Title);
					list2.Add(this._obtained.GetFormattedText());
				}
			}
			foreach (ModelChoiceHistoryEntry modelChoiceHistoryEntry in playerEntry.RelicChoices)
			{
				this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/chest_icon.png[/img]");
				this._obtained.Add("Title", SaveUtil.RelicOrDeprecated(modelChoiceHistoryEntry.choice).Title);
				if (modelChoiceHistoryEntry.wasPicked)
				{
					list.Add(this._obtained.GetFormattedText());
				}
				else
				{
					list2.Add(this._obtained.GetFormattedText());
				}
			}
			foreach (ModelChoiceHistoryEntry modelChoiceHistoryEntry2 in playerEntry.PotionChoices)
			{
				this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
				this._obtained.Add("Title", SaveUtil.PotionOrDeprecated(modelChoiceHistoryEntry2.choice).Title);
				if (modelChoiceHistoryEntry2.wasPicked)
				{
					list.Add(this._obtained.GetFormattedText());
				}
				else
				{
					list2.Add(this._obtained.GetFormattedText());
				}
			}
			foreach (SerializableCard serializableCard2 in playerEntry.CardsRemoved)
			{
				this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._removed.Add("Title", CardModel.FromSerializable(serializableCard2).Title);
				list.Add(this._removed.GetFormattedText() ?? "");
			}
			foreach (ModelId modelId in playerEntry.RelicsRemoved)
			{
				this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/chest_icon.png[/img]");
				this._removed.Add("Title", SaveUtil.RelicOrDeprecated(modelId).Title);
				list.Add(this._removed.GetFormattedText() ?? "");
			}
			foreach (ModelId modelId2 in playerEntry.UpgradedCards)
			{
				this._upgraded.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._upgraded.Add("Title", SaveUtil.CardOrDeprecated(modelId2).Title);
				list.Add(this._upgraded.GetFormattedText() ?? "");
			}
			foreach (ModelId modelId3 in playerEntry.DowngradedCards)
			{
				this._downgraded.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._downgraded.Add("Title", SaveUtil.CardOrDeprecated(modelId3).Title);
				list.Add(this._downgraded.GetFormattedText() ?? "");
			}
			foreach (CardEnchantmentHistoryEntry cardEnchantmentHistoryEntry in playerEntry.CardsEnchanted)
			{
				this._enchanted.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._enchanted.Add("Title1", CardModel.FromSerializable(cardEnchantmentHistoryEntry.Card).Title);
				this._enchanted.Add("Title2", SaveUtil.EnchantmentOrDeprecated(cardEnchantmentHistoryEntry.Enchantment).Title);
				list.Add(this._enchanted.GetFormattedText() ?? "");
			}
			foreach (CardTransformationHistoryEntry cardTransformationHistoryEntry in playerEntry.CardsTransformed)
			{
				this._transformed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
				this._transformed.Add("Title1", CardModel.FromSerializable(cardTransformationHistoryEntry.OriginalCard).Title);
				this._transformed.Add("Title2", CardModel.FromSerializable(cardTransformationHistoryEntry.FinalCard).Title);
				list.Add(this._transformed.GetFormattedText() ?? "");
			}
			int num = Mathf.Max(5, Mathf.CeilToInt((float)list.Count / 2f));
			this._rewardRows[1].Visible = list.Count > 5;
			this._rewardStatsContainer.Visible = list.Count > 0;
			for (int i = 0; i < list.Count; i++)
			{
				RichTextLabel richTextLabel = this._rewardRows[i / num];
				richTextLabel.Text = richTextLabel.Text + "\t" + list[i] + "\n";
			}
			int num2 = Mathf.Max(5, Mathf.CeilToInt((float)list2.Count / 2f));
			this._skippedRows[1].Visible = list2.Count > 5;
			this._skippedStatsContainer.Visible = list2.Count > 0;
			for (int j = 0; j < list2.Count; j++)
			{
				RichTextLabel richTextLabel2 = this._skippedRows[j / num2];
				richTextLabel2.Text = richTextLabel2.Text + "\t" + list2[j] + "\n";
			}
			foreach (RichTextLabel richTextLabel3 in this._rewardRows)
			{
				richTextLabel3.Text = richTextLabel3.Text.Trim('\n');
			}
			foreach (RichTextLabel richTextLabel4 in this._skippedRows)
			{
				richTextLabel4.Text = richTextLabel4.Text.Trim('\n');
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600375D RID: 14173 RVA: 0x001A1F94 File Offset: 0x001A0194
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMapPointHistoryHoverTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600375E RID: 14174 RVA: 0x001A1FD8 File Offset: 0x001A01D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPointHistoryHoverTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600375F RID: 14175 RVA: 0x001A2008 File Offset: 0x001A0208
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapPointHistoryHoverTip.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003760 RID: 14176 RVA: 0x001A2020 File Offset: 0x001A0220
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._floorNum)
			{
				this._floorNum = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._playerId)
			{
				this._playerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._titleLabel)
			{
				this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._playerStats)
			{
				this._playerStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._roomStats)
			{
				this._roomStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer)
			{
				this._rewardStatsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer)
			{
				this._skippedStatsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._actionStats)
			{
				this._actionStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003761 RID: 14177 RVA: 0x001A2110 File Offset: 0x001A0310
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._floorNum)
			{
				value = VariantUtils.CreateFrom<int>(ref this._floorNum);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._playerId)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._playerId);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._titleLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._playerStats)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._playerStats);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._roomStats)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._roomStats);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rewardStatsContainer);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._skippedStatsContainer);
				return true;
			}
			if ((ref name) == NMapPointHistoryHoverTip.PropertyName._actionStats)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._actionStats);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003762 RID: 14178 RVA: 0x001A2228 File Offset: 0x001A0428
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NMapPointHistoryHoverTip.PropertyName._floorNum, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapPointHistoryHoverTip.PropertyName._playerId, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._titleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._playerStats, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._roomStats, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryHoverTip.PropertyName._actionStats, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003763 RID: 14179 RVA: 0x001A2344 File Offset: 0x001A0544
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._floorNum, Variant.From<int>(ref this._floorNum));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._playerId, Variant.From<ulong>(ref this._playerId));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._playerStats, Variant.From<RichTextLabel>(ref this._playerStats));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._roomStats, Variant.From<RichTextLabel>(ref this._roomStats));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, Variant.From<Control>(ref this._rewardStatsContainer));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, Variant.From<Control>(ref this._skippedStatsContainer));
			info.AddProperty(NMapPointHistoryHoverTip.PropertyName._actionStats, Variant.From<RichTextLabel>(ref this._actionStats));
		}

		/// <inheritdoc />
		// Token: 0x06003764 RID: 14180 RVA: 0x001A2408 File Offset: 0x001A0608
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._floorNum, ref variant))
			{
				this._floorNum = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._playerId, ref variant2))
			{
				this._playerId = variant2.As<ulong>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._titleLabel, ref variant3))
			{
				this._titleLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._playerStats, ref variant4))
			{
				this._playerStats = variant4.As<RichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._roomStats, ref variant5))
			{
				this._roomStats = variant5.As<RichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, ref variant6))
			{
				this._rewardStatsContainer = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, ref variant7))
			{
				this._skippedStatsContainer = variant7.As<Control>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._actionStats, ref variant8))
			{
				this._actionStats = variant8.As<RichTextLabel>();
			}
		}

		// Token: 0x04001577 RID: 5495
		private readonly LocString _mapPointRoomStatsLoc = new LocString("run_history", "MAP_POINT_HISTORY.room_stats");

		// Token: 0x04001578 RID: 5496
		private readonly LocString _mapPointPlayerStatsLoc = new LocString("run_history", "MAP_POINT_HISTORY.player_stats");

		// Token: 0x04001579 RID: 5497
		private readonly LocString _combatStats = new LocString("run_history", "MAP_POINT_HISTORY.combatStats");

		// Token: 0x0400157A RID: 5498
		private readonly LocString _floorTitle = new LocString("run_history", "MAP_POINT_HISTORY.header");

		// Token: 0x0400157B RID: 5499
		private readonly LocString _chose = new LocString("run_history", "MAP_POINT_HISTORY.chose");

		// Token: 0x0400157C RID: 5500
		private readonly LocString _skipped = new LocString("run_history", "MAP_POINT_HISTORY.skipped");

		// Token: 0x0400157D RID: 5501
		private readonly LocString _rewardsHeaderLoc = new LocString("run_history", "HISTORY_ENTRY.rewardsHeader");

		// Token: 0x0400157E RID: 5502
		private readonly LocString _skippedHeaderLoc = new LocString("run_history", "HISTORY_ENTRY.skippedHeader");

		// Token: 0x0400157F RID: 5503
		private readonly LocString _enchanted = new LocString("run_history", "HISTORY_ENTRY.enchanted");

		// Token: 0x04001580 RID: 5504
		private readonly LocString _obtained = new LocString("run_history", "HISTORY_ENTRY.obtained");

		// Token: 0x04001581 RID: 5505
		private readonly LocString _goldGained = new LocString("run_history", "HISTORY_ENTRY.goldGained");

		// Token: 0x04001582 RID: 5506
		private readonly LocString _goldSpent = new LocString("run_history", "HISTORY_ENTRY.goldSpent");

		// Token: 0x04001583 RID: 5507
		private readonly LocString _goldLost = new LocString("run_history", "HISTORY_ENTRY.goldLost");

		// Token: 0x04001584 RID: 5508
		private readonly LocString _goldStolen = new LocString("run_history", "HISTORY_ENTRY.goldStolen");

		// Token: 0x04001585 RID: 5509
		private readonly LocString _used = new LocString("run_history", "HISTORY_ENTRY.used");

		// Token: 0x04001586 RID: 5510
		private readonly LocString _removed = new LocString("run_history", "HISTORY_ENTRY.removed");

		// Token: 0x04001587 RID: 5511
		private readonly LocString _transformed = new LocString("run_history", "HISTORY_ENTRY.transformed");

		// Token: 0x04001588 RID: 5512
		private readonly LocString _upgraded = new LocString("run_history", "HISTORY_ENTRY.upgraded");

		// Token: 0x04001589 RID: 5513
		private readonly LocString _downgraded = new LocString("run_history", "HISTORY_ENTRY.downgraded");

		// Token: 0x0400158A RID: 5514
		private readonly LocString _damaged = new LocString("run_history", "MAP_POINT_HISTORY.damageTaken");

		// Token: 0x0400158B RID: 5515
		private readonly LocString _healed = new LocString("run_history", "MAP_POINT_HISTORY.healed");

		// Token: 0x0400158C RID: 5516
		private readonly LocString _maxHpGained = new LocString("run_history", "MAP_POINT_HISTORY.maxHpGained");

		// Token: 0x0400158D RID: 5517
		private readonly LocString _maxHpLost = new LocString("run_history", "MAP_POINT_HISTORY.maxHpLost");

		// Token: 0x0400158E RID: 5518
		private readonly LocString _turns = new LocString("run_history", "MAP_POINT_HISTORY.turnsTaken");

		// Token: 0x0400158F RID: 5519
		private readonly LocString _quests = new LocString("run_history", "MAP_POINT_HISTORY.questCompleted");

		// Token: 0x04001590 RID: 5520
		private const string _goldIconPath = "res://images/packed/sprite_fonts/gold_icon.png";

		// Token: 0x04001591 RID: 5521
		private const string _cardIconPath = "res://images/packed/sprite_fonts/card_icon.png";

		// Token: 0x04001592 RID: 5522
		private const string _chestIconPath = "res://images/packed/sprite_fonts/chest_icon.png";

		// Token: 0x04001593 RID: 5523
		private const string _potionIconPath = "res://images/packed/sprite_fonts/potion_icon.png";

		// Token: 0x04001594 RID: 5524
		private const string _roomHistoryTipScenePath = "res://scenes/ui/map_point_history_hover_tip.tscn";

		// Token: 0x04001595 RID: 5525
		private MapPointHistoryEntry _entry;

		// Token: 0x04001596 RID: 5526
		private int _floorNum;

		// Token: 0x04001597 RID: 5527
		private ulong _playerId;

		// Token: 0x04001598 RID: 5528
		private MegaLabel _titleLabel;

		// Token: 0x04001599 RID: 5529
		private RichTextLabel _playerStats;

		// Token: 0x0400159A RID: 5530
		private RichTextLabel _roomStats;

		// Token: 0x0400159B RID: 5531
		private Control _rewardStatsContainer;

		// Token: 0x0400159C RID: 5532
		private Control _skippedStatsContainer;

		// Token: 0x0400159D RID: 5533
		private List<RichTextLabel> _rewardRows;

		// Token: 0x0400159E RID: 5534
		private List<RichTextLabel> _skippedRows;

		// Token: 0x0400159F RID: 5535
		private RichTextLabel _actionStats;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200175E RID: 5982
		[NullableContext(0)]
		public class MethodName : MarginContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B25 RID: 23333
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200175F RID: 5983
		[NullableContext(0)]
		public class PropertyName : MarginContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_floorNum' field.
			/// </summary>
			// Token: 0x04005B26 RID: 23334
			public static readonly StringName _floorNum = "_floorNum";

			/// <summary>
			/// Cached name for the '_playerId' field.
			/// </summary>
			// Token: 0x04005B27 RID: 23335
			public static readonly StringName _playerId = "_playerId";

			/// <summary>
			/// Cached name for the '_titleLabel' field.
			/// </summary>
			// Token: 0x04005B28 RID: 23336
			public static readonly StringName _titleLabel = "_titleLabel";

			/// <summary>
			/// Cached name for the '_playerStats' field.
			/// </summary>
			// Token: 0x04005B29 RID: 23337
			public static readonly StringName _playerStats = "_playerStats";

			/// <summary>
			/// Cached name for the '_roomStats' field.
			/// </summary>
			// Token: 0x04005B2A RID: 23338
			public static readonly StringName _roomStats = "_roomStats";

			/// <summary>
			/// Cached name for the '_rewardStatsContainer' field.
			/// </summary>
			// Token: 0x04005B2B RID: 23339
			public static readonly StringName _rewardStatsContainer = "_rewardStatsContainer";

			/// <summary>
			/// Cached name for the '_skippedStatsContainer' field.
			/// </summary>
			// Token: 0x04005B2C RID: 23340
			public static readonly StringName _skippedStatsContainer = "_skippedStatsContainer";

			/// <summary>
			/// Cached name for the '_actionStats' field.
			/// </summary>
			// Token: 0x04005B2D RID: 23341
			public static readonly StringName _actionStats = "_actionStats";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001760 RID: 5984
		[NullableContext(0)]
		public class SignalName : MarginContainer.SignalName
		{
		}
	}
}
