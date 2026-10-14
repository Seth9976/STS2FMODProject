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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002CE RID: 718
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NDeckHistory.cs")]
	public class NDeckHistory : VBoxContainer
	{
		// Token: 0x060025DE RID: 9694 RVA: 0x000FFB2F File Offset: 0x000FDD2F
		public override void _Ready()
		{
			this._headerLabel = base.GetNode<MegaRichTextLabel>("Header");
			this._cardContainer = base.GetNode<Control>("%CardContainer");
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000FFB60 File Offset: 0x000FDD60
		[NullableContext(1)]
		public void LoadDeck(Player player, IEnumerable<SerializableCard> cards)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<CardRarity, int> dictionary = new Dictionary<CardRarity, int>();
			foreach (CardRarity cardRarity in Enum.GetValues<CardRarity>())
			{
				dictionary.Add(cardRarity, 0);
			}
			List<SerializableCard> list = cards.ToList<SerializableCard>();
			foreach (SerializableCard serializableCard in list)
			{
				CardModel cardModel = SaveUtil.CardOrDeprecated(serializableCard.Id);
				Dictionary<CardRarity, int> dictionary2 = dictionary;
				CardRarity rarity = cardModel.Rarity;
				int num = dictionary2[rarity];
				dictionary2[rarity] = num + 1;
			}
			this._deckHeader.Add("totalCards", list.Count);
			foreach (KeyValuePair<CardRarity, int> keyValuePair in dictionary)
			{
				CardRarity rarity;
				int num;
				keyValuePair.Deconstruct(out rarity, out num);
				CardRarity cardRarity2 = rarity;
				int num2 = num;
				this._cardCategories.Add(cardRarity2.ToString() + "Cards", num2);
			}
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(20, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("[gold][b]");
			appendInterpolatedStringHandler.AppendFormatted(this._deckHeader.GetFormattedText());
			appendInterpolatedStringHandler.AppendLiteral("[/b][/gold]");
			stringBuilder3.Append(ref appendInterpolatedStringHandler);
			stringBuilder.Append(this._cardCategories.GetFormattedText().Trim(','));
			this._headerLabel.Text = stringBuilder.ToString();
			this.PopulateCards(player, list);
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000FFD14 File Offset: 0x000FDF14
		[NullableContext(1)]
		private void PopulateCards(Player player, IEnumerable<SerializableCard> cards)
		{
			foreach (Node node in this._cardContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			this._allCards.Clear();
			foreach (IGrouping<SerializableCard, SerializableCard> grouping in from x in cards
				group x by x)
			{
				CardModel cardModel = CardModel.FromSerializable(grouping.Key);
				cardModel.Owner = player;
				this._allCards.Add(cardModel);
				NDeckHistoryEntry entry = NDeckHistoryEntry.Create(cardModel, grouping.Count<SerializableCard>(), from c in grouping
					where c.FloorAddedToDeck != null
					select c.FloorAddedToDeck.Value);
				entry.Connect(NDeckHistoryEntry.SignalName.Clicked, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.ShowEntry)), 0U);
				entry.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(delegate(NClickableControl _)
				{
					this.EmitSignal(NDeckHistory.SignalName.Hovered, new Variant[] { entry });
				}), 0U);
				entry.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(delegate(NClickableControl _)
				{
					this.EmitSignal(NDeckHistory.SignalName.Unhovered, new Variant[] { entry });
				}), 0U);
				this._cardContainer.AddChildSafely(entry);
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000FFEF4 File Offset: 0x000FE0F4
		[NullableContext(1)]
		private void ShowEntry(NDeckHistoryEntry entry)
		{
			NGame.Instance.GetInspectCardScreen().Open(this._allCards, this._allCards.IndexOf(entry.Card), false);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025E2 RID: 9698 RVA: 0x000FFF20 File Offset: 0x000FE120
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDeckHistory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckHistory.MethodName.ShowEntry, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025E3 RID: 9699 RVA: 0x000FFFC4 File Offset: 0x000FE1C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckHistory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckHistory.MethodName.ShowEntry && args.Count == 1)
			{
				this.ShowEntry(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060025E4 RID: 9700 RVA: 0x00100032 File Offset: 0x000FE232
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckHistory.MethodName._Ready || (ref method) == NDeckHistory.MethodName.ShowEntry || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060025E5 RID: 9701 RVA: 0x00100059 File Offset: 0x000FE259
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckHistory.PropertyName._headerLabel)
			{
				this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NDeckHistory.PropertyName._cardContainer)
			{
				this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060025E6 RID: 9702 RVA: 0x0010009C File Offset: 0x000FE29C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckHistory.PropertyName._headerLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
				return true;
			}
			if ((ref name) == NDeckHistory.PropertyName._cardContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025E7 RID: 9703 RVA: 0x001000F4 File Offset: 0x000FE2F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDeckHistory.PropertyName._headerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckHistory.PropertyName._cardContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060025E8 RID: 9704 RVA: 0x0010014C File Offset: 0x000FE34C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckHistory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
			info.AddProperty(NDeckHistory.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
			info.AddSignalEventDelegate(NDeckHistory.SignalName.Hovered, this.backing_Hovered);
			info.AddSignalEventDelegate(NDeckHistory.SignalName.Unhovered, this.backing_Unhovered);
		}

		/// <inheritdoc />
		// Token: 0x060025E9 RID: 9705 RVA: 0x001001B0 File Offset: 0x000FE3B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckHistory.PropertyName._headerLabel, ref variant))
			{
				this._headerLabel = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckHistory.PropertyName._cardContainer, ref variant2))
			{
				this._cardContainer = variant2.As<Control>();
			}
			NDeckHistory.HoveredEventHandler hoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NDeckHistory.HoveredEventHandler>(NDeckHistory.SignalName.Hovered, ref hoveredEventHandler))
			{
				this.backing_Hovered = hoveredEventHandler;
			}
			NDeckHistory.UnhoveredEventHandler unhoveredEventHandler;
			if (info.TryGetSignalEventDelegate<NDeckHistory.UnhoveredEventHandler>(NDeckHistory.SignalName.Unhovered, ref unhoveredEventHandler))
			{
				this.backing_Unhovered = unhoveredEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060025EA RID: 9706 RVA: 0x00100228 File Offset: 0x000FE428
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDeckHistory.SignalName.Hovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "deckHistoryEntry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckHistory.SignalName.Unhovered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "deckHistoryEntry", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NDeckHistory.HoveredEventHandler" />
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060025EB RID: 9707 RVA: 0x001002FB File Offset: 0x000FE4FB
		// (remove) Token: 0x060025EC RID: 9708 RVA: 0x00100314 File Offset: 0x000FE514
		public event NDeckHistory.HoveredEventHandler Hovered
		{
			add
			{
				this.backing_Hovered = (NDeckHistory.HoveredEventHandler)Delegate.Combine(this.backing_Hovered, value);
			}
			remove
			{
				this.backing_Hovered = (NDeckHistory.HoveredEventHandler)Delegate.Remove(this.backing_Hovered, value);
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0010032D File Offset: 0x000FE52D
		protected void EmitSignalHovered(NDeckHistoryEntry deckHistoryEntry)
		{
			base.EmitSignal(NDeckHistory.SignalName.Hovered, new Variant[] { deckHistoryEntry });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NDeckHistory.UnhoveredEventHandler" />
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060025EE RID: 9710 RVA: 0x0010034E File Offset: 0x000FE54E
		// (remove) Token: 0x060025EF RID: 9711 RVA: 0x00100367 File Offset: 0x000FE567
		public event NDeckHistory.UnhoveredEventHandler Unhovered
		{
			add
			{
				this.backing_Unhovered = (NDeckHistory.UnhoveredEventHandler)Delegate.Combine(this.backing_Unhovered, value);
			}
			remove
			{
				this.backing_Unhovered = (NDeckHistory.UnhoveredEventHandler)Delegate.Remove(this.backing_Unhovered, value);
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00100380 File Offset: 0x000FE580
		protected void EmitSignalUnhovered(NDeckHistoryEntry deckHistoryEntry)
		{
			base.EmitSignal(NDeckHistory.SignalName.Unhovered, new Variant[] { deckHistoryEntry });
		}

		/// <inheritdoc />
		// Token: 0x060025F1 RID: 9713 RVA: 0x001003A4 File Offset: 0x000FE5A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NDeckHistory.SignalName.Hovered && args.Count == 1)
			{
				NDeckHistory.HoveredEventHandler hoveredEventHandler = this.backing_Hovered;
				if (hoveredEventHandler == null)
				{
					return;
				}
				hoveredEventHandler(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NDeckHistory.SignalName.Unhovered) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NDeckHistory.UnhoveredEventHandler unhoveredEventHandler = this.backing_Unhovered;
				if (unhoveredEventHandler == null)
				{
					return;
				}
				unhoveredEventHandler(VariantUtils.ConvertTo<NDeckHistoryEntry>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060025F2 RID: 9714 RVA: 0x00100423 File Offset: 0x000FE623
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NDeckHistory.SignalName.Hovered || (ref signal) == NDeckHistory.SignalName.Unhovered || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000E06 RID: 3590
		[Nullable(1)]
		private readonly LocString _deckHeader = new LocString("run_history", "DECK_HISTORY.header");

		// Token: 0x04000E07 RID: 3591
		[Nullable(1)]
		private readonly LocString _cardCategories = new LocString("run_history", "DECK_HISTORY.categories");

		// Token: 0x04000E08 RID: 3592
		[Nullable(1)]
		private MegaRichTextLabel _headerLabel;

		// Token: 0x04000E09 RID: 3593
		[Nullable(1)]
		private Control _cardContainer;

		// Token: 0x04000E0A RID: 3594
		[Nullable(1)]
		private readonly List<CardModel> _allCards = new List<CardModel>();

		// Token: 0x04000E0B RID: 3595
		private NDeckHistory.HoveredEventHandler backing_Hovered;

		// Token: 0x04000E0C RID: 3596
		private NDeckHistory.UnhoveredEventHandler backing_Unhovered;

		// Token: 0x0200141C RID: 5148
		// (Invoke) Token: 0x0600A014 RID: 40980
		[Signal]
		public delegate void HoveredEventHandler(NDeckHistoryEntry deckHistoryEntry);

		// Token: 0x0200141D RID: 5149
		// (Invoke) Token: 0x0600A018 RID: 40984
		[Signal]
		public delegate void UnhoveredEventHandler(NDeckHistoryEntry deckHistoryEntry);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200141E RID: 5150
		public class MethodName : VBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C6A RID: 19562
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ShowEntry' method.
			/// </summary>
			// Token: 0x04004C6B RID: 19563
			public static readonly StringName ShowEntry = "ShowEntry";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200141F RID: 5151
		public class PropertyName : VBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_headerLabel' field.
			/// </summary>
			// Token: 0x04004C6C RID: 19564
			public static readonly StringName _headerLabel = "_headerLabel";

			/// <summary>
			/// Cached name for the '_cardContainer' field.
			/// </summary>
			// Token: 0x04004C6D RID: 19565
			public static readonly StringName _cardContainer = "_cardContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001420 RID: 5152
		public class SignalName : VBoxContainer.SignalName
		{
			/// <summary>
			/// Cached name for the 'Hovered' signal.
			/// </summary>
			// Token: 0x04004C6E RID: 19566
			public static readonly StringName Hovered = "Hovered";

			/// <summary>
			/// Cached name for the 'Unhovered' signal.
			/// </summary>
			// Token: 0x04004C6F RID: 19567
			public static readonly StringName Unhovered = "Unhovered";
		}
	}
}
