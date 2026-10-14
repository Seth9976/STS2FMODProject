using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// This is the screen used to view an arbitrary set of cards
	/// EX: pandoras box
	/// </summary>
	// Token: 0x02000252 RID: 594
	[ScriptPath("res://src/Core/Nodes/Screens/NSimpleCardsViewScreen.cs")]
	public class NSimpleCardsViewScreen : NCardsViewScreen
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000CAEF6 File Offset: 0x000C90F6
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("screens/simple_cards_view_screen");
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000CAF02 File Offset: 0x000C9102
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NSimpleCardsViewScreen.ScenePath);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x000CAF0E File Offset: 0x000C910E
		public override NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.SimpleCardsView;
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000CAF14 File Offset: 0x000C9114
		public unsafe override void _Ready()
		{
			this._confirmButton = base.GetNode<NButton>("ConfirmButton");
			base.GetNode<MegaLabel>("%ViewUpgradesLabel").SetTextAutoSize(new LocString("gameplay_ui", "VIEW_UPGRADES").GetFormattedText());
			this.ConnectSignals();
			NCardGrid grid = this._grid;
			IReadOnlyList<CardModel> cards = this._cards;
			PileType pileType = PileType.Deck;
			int num = 1;
			List<SortingOrders> list = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
			int num2 = 0;
			*span[num2] = SortingOrders.Ascending;
			grid.SetCards(cards, pileType, list, null);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000CAF9C File Offset: 0x000C919C
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._backButton.Disable();
			this._confirmButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(base.OnReturnButtonPressed)), 0U);
			this._confirmButton.Enable();
			this._grid.Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>(delegate(NCardHolder h)
			{
				base.ShowCardDetail(h.CardModel);
			}), 0U);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000CB00C File Offset: 0x000C920C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCardsViewScreen ShowScreen(List<CardPileAddResult> cards, LocString infoText)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSimpleCardsViewScreen nsimpleCardsViewScreen = PreloadManager.Cache.GetScene(NSimpleCardsViewScreen.ScenePath).Instantiate<NSimpleCardsViewScreen>(0L);
			nsimpleCardsViewScreen._cards = cards.Select<CardPileAddResult, CardModel>((CardPileAddResult c) => c.cardAdded).ToList<CardModel>();
			nsimpleCardsViewScreen._cardResults = cards;
			nsimpleCardsViewScreen._infoText = infoText;
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("map_open.mp3", 1f, PitchVariance.None);
			}
			NCapstoneContainer.Instance.Open(nsimpleCardsViewScreen);
			return nsimpleCardsViewScreen;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000CB09F File Offset: 0x000C929F
		protected override void OnInspectCardHidden()
		{
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000CB0A1 File Offset: 0x000C92A1
		public override void AfterCapstoneOpened()
		{
			base.AfterCapstoneOpened();
			TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000CB0B8 File Offset: 0x000C92B8
		[NullableContext(1)]
		private async Task FlashRelicsOnModifiedCards()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			using (List<CardPileAddResult>.Enumerator enumerator = this._cardResults.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CardPileAddResult result = enumerator.Current;
					NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((NGridCardHolder h) => h.CardModel == result.cardAdded);
					if (ngridCardHolder != null && result.modifyingModels != null && result.modifyingModels.Count != 0)
					{
						foreach (RelicModel relicModel in result.modifyingModels.OfType<RelicModel>())
						{
							relicModel.Flash();
							NCard cardNode = ngridCardHolder.CardNode;
							if (cardNode != null)
							{
								cardNode.FlashRelicOnCard(relicModel);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001EC1 RID: 7873 RVA: 0x000CB0FC File Offset: 0x000C92FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSimpleCardsViewScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardsViewScreen.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardsViewScreen.MethodName.OnInspectCardHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EC2 RID: 7874 RVA: 0x000CB1D0 File Offset: 0x000C93D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSimpleCardsViewScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardsViewScreen.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardsViewScreen.MethodName.OnInspectCardHidden && args.Count == 0)
			{
				this.OnInspectCardHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened && args.Count == 0)
			{
				this.AfterCapstoneOpened();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001EC3 RID: 7875 RVA: 0x000CB27C File Offset: 0x000C947C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSimpleCardsViewScreen.MethodName._Ready || (ref method) == NSimpleCardsViewScreen.MethodName.ConnectSignals || (ref method) == NSimpleCardsViewScreen.MethodName.OnInspectCardHidden || (ref method) == NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001EC4 RID: 7876 RVA: 0x000CB2CC File Offset: 0x000C94CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSimpleCardsViewScreen.PropertyName._confirmButton)
			{
				this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001EC5 RID: 7877 RVA: 0x000CB2F4 File Offset: 0x000C94F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSimpleCardsViewScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NSimpleCardsViewScreen.PropertyName._confirmButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001EC6 RID: 7878 RVA: 0x000CB34C File Offset: 0x000C954C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSimpleCardsViewScreen.PropertyName._confirmButton, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSimpleCardsViewScreen.PropertyName.ScreenType, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001EC7 RID: 7879 RVA: 0x000CB3A1 File Offset: 0x000C95A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSimpleCardsViewScreen.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
		}

		/// <inheritdoc />
		// Token: 0x06001EC8 RID: 7880 RVA: 0x000CB3C0 File Offset: 0x000C95C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSimpleCardsViewScreen.PropertyName._confirmButton, ref variant))
			{
				this._confirmButton = variant.As<NButton>();
			}
		}

		// Token: 0x04000BDE RID: 3038
		[Nullable(1)]
		private NButton _confirmButton;

		// Token: 0x04000BDF RID: 3039
		[Nullable(1)]
		private List<CardPileAddResult> _cardResults;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001276 RID: 4726
		public new class MethodName : NCardsViewScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400479F RID: 18335
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x040047A0 RID: 18336
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnInspectCardHidden' method.
			/// </summary>
			// Token: 0x040047A1 RID: 18337
			public new static readonly StringName OnInspectCardHidden = "OnInspectCardHidden";

			/// <summary>
			/// Cached name for the 'AfterCapstoneOpened' method.
			/// </summary>
			// Token: 0x040047A2 RID: 18338
			public new static readonly StringName AfterCapstoneOpened = "AfterCapstoneOpened";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001277 RID: 4727
		public new class PropertyName : NCardsViewScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x040047A3 RID: 18339
			public new static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the '_confirmButton' field.
			/// </summary>
			// Token: 0x040047A4 RID: 18340
			public static readonly StringName _confirmButton = "_confirmButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001278 RID: 4728
		public new class SignalName : NCardsViewScreen.SignalName
		{
		}
	}
}
