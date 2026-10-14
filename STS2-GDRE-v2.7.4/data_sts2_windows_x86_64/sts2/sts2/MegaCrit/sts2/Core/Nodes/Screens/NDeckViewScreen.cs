using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// This is the screen used to view the player's deck (that's you!)
	/// </summary>
	// Token: 0x0200024D RID: 589
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NDeckViewScreen.cs")]
	public class NDeckViewScreen : NCardsViewScreen
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x000C5573 File Offset: 0x000C3773
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/deck_view_screen");
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x000C557F File Offset: 0x000C377F
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDeckViewScreen.ScenePath);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x000C558B File Offset: 0x000C378B
		public override NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.DeckView;
			}
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000C5590 File Offset: 0x000C3790
		[return: Nullable(2)]
		public static NDeckViewScreen ShowScreen(Player player)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDeckViewScreen ndeckViewScreen = PreloadManager.Cache.GetScene(NDeckViewScreen.ScenePath).Instantiate<NDeckViewScreen>(0L);
			ndeckViewScreen._player = player;
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("map_open.mp3", 1f, PitchVariance.None);
			}
			NCapstoneContainer.Instance.Open(ndeckViewScreen);
			return ndeckViewScreen;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000C55EC File Offset: 0x000C37EC
		public override void _Ready()
		{
			this._cards = this._pile.Cards.ToList<CardModel>();
			this._infoText = new LocString("gameplay_ui", "DECK_PILE_INFO");
			this._bg = base.GetNode<Control>("%SortingBg");
			this._obtainedSorter = base.GetNode<NCardViewSortButton>("%ObtainedSorter");
			this._typeSorter = base.GetNode<NCardViewSortButton>("%CardTypeSorter");
			this._costSorter = base.GetNode<NCardViewSortButton>("%CostSorter");
			this._alphabetSorter = base.GetNode<NCardViewSortButton>("%AlphabeticalSorter");
			this._obtainedSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnObtainedSort)), 0U);
			this._typeSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCardTypeSort)), 0U);
			this._costSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCostSort)), 0U);
			this._alphabetSorter.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAlphabetSort)), 0U);
			this._obtainedSorter.SetLabel(new LocString("gameplay_ui", "SORT_OBTAINED").GetRawText());
			this._typeSorter.SetLabel(new LocString("gameplay_ui", "SORT_TYPE").GetRawText());
			this._costSorter.SetLabel(new LocString("gameplay_ui", "SORT_COST").GetRawText());
			this._alphabetSorter.SetLabel(new LocString("gameplay_ui", "SORT_ALPHABET").GetRawText());
			base.GetNode<MegaLabel>("%ViewUpgradesLabel").SetTextAutoSize(new LocString("gameplay_ui", "VIEW_UPGRADES").GetFormattedText());
			ShaderMaterial shaderMaterial = (ShaderMaterial)this._player.Character.CardPool.FrameMaterial;
			this._bg.Material = shaderMaterial;
			this._obtainedSorter.SetHue(shaderMaterial);
			this._typeSorter.SetHue(shaderMaterial);
			this._costSorter.SetHue(shaderMaterial);
			this._alphabetSorter.SetHue(shaderMaterial);
			this.ConnectSignals();
			this.DisplayCards();
			Control[] array = new Control[] { this._obtainedSorter, this._typeSorter, this._costSorter, this._alphabetSorter };
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FocusNeighborTop = array[i].GetPath();
				array[i].FocusNeighborBottom = ((this._grid.DefaultFocusedControl != null) ? this._grid.DefaultFocusedControl.GetPath() : array[i].GetPath());
				array[i].FocusNeighborLeft = ((i > 0) ? array[i - 1].GetPath() : array[i].GetPath());
				array[i].FocusNeighborRight = ((i < array.Length - 1) ? array[i + 1].GetPath() : array[i].GetPath());
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000C58E8 File Offset: 0x000C3AE8
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._grid.Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>(delegate(NCardHolder h)
			{
				base.ShowCardDetail(h.CardModel);
			}), 0U);
			this._grid.Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>(delegate(NCardHolder h)
			{
				base.ShowCardDetail(h.CardModel);
			}), 0U);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000C5941 File Offset: 0x000C3B41
		public override void _EnterTree()
		{
			base._EnterTree();
			this._pile = PileType.Deck.GetPile(this._player);
			this._pile.ContentsChanged += this.OnPileContentsChanged;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000C5972 File Offset: 0x000C3B72
		public override void _ExitTree()
		{
			base._ExitTree();
			this._pile.ContentsChanged -= this.OnPileContentsChanged;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000C5991 File Offset: 0x000C3B91
		public override void AfterCapstoneClosed()
		{
			base.AfterCapstoneClosed();
			NRun instance = NRun.Instance;
			if (instance == null)
			{
				return;
			}
			instance.GlobalUi.TopBar.Deck.ToggleAnimState();
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000C59B7 File Offset: 0x000C3BB7
		private void OnPileContentsChanged()
		{
			this._cards = this._pile.Cards.ToList<CardModel>();
			this.DisplayCards();
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000C59D8 File Offset: 0x000C3BD8
		private void OnObtainedSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.Ascending);
			this._sortingPriority.Remove(SortingOrders.Descending);
			if (this._obtainedSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.Descending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.Ascending);
			}
			this.DisplayCards();
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000C5A30 File Offset: 0x000C3C30
		private void OnCardTypeSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.TypeAscending);
			this._sortingPriority.Remove(SortingOrders.TypeDescending);
			if (this._typeSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.TypeDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.TypeAscending);
			}
			this.DisplayCards();
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000C5A88 File Offset: 0x000C3C88
		private void OnCostSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.CostAscending);
			this._sortingPriority.Remove(SortingOrders.CostDescending);
			if (this._costSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.CostDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.CostAscending);
			}
			this.DisplayCards();
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000C5AE0 File Offset: 0x000C3CE0
		private void OnAlphabetSort(NButton button)
		{
			this._sortingPriority.Remove(SortingOrders.AlphabetAscending);
			this._sortingPriority.Remove(SortingOrders.AlphabetDescending);
			if (this._alphabetSorter.IsDescending)
			{
				this._sortingPriority.Insert(0, SortingOrders.AlphabetDescending);
			}
			else
			{
				this._sortingPriority.Insert(0, SortingOrders.AlphabetAscending);
			}
			this.DisplayCards();
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000C5B38 File Offset: 0x000C3D38
		private void DisplayCards()
		{
			this._grid.YOffset = 100;
			this._grid.SetCards(this._cards, this._pile.Type, this._sortingPriority, null);
			IEnumerable<NGridCardHolder> topRowOfCardNodes = this._grid.GetTopRowOfCardNodes();
			if (topRowOfCardNodes != null)
			{
				foreach (NGridCardHolder ngridCardHolder in topRowOfCardNodes)
				{
					ngridCardHolder.FocusNeighborTop = this._obtainedSorter.GetPath();
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E4B RID: 7755 RVA: 0x000C5BCC File Offset: 0x000C3DCC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(NDeckViewScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName.AfterCapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName.OnPileContentsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDeckViewScreen.MethodName.OnObtainedSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckViewScreen.MethodName.OnCardTypeSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckViewScreen.MethodName.OnCostSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckViewScreen.MethodName.OnAlphabetSort, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NDeckViewScreen.MethodName.DisplayCards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E4C RID: 7756 RVA: 0x000C5EA8 File Offset: 0x000C40A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDeckViewScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.AfterCapstoneClosed && args.Count == 0)
			{
				this.AfterCapstoneClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.OnPileContentsChanged && args.Count == 0)
			{
				this.OnPileContentsChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.OnObtainedSort && args.Count == 1)
			{
				this.OnObtainedSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.OnCardTypeSort && args.Count == 1)
			{
				this.OnCardTypeSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.OnCostSort && args.Count == 1)
			{
				this.OnCostSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.OnAlphabetSort && args.Count == 1)
			{
				this.OnAlphabetSort(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDeckViewScreen.MethodName.DisplayCards && args.Count == 0)
			{
				this.DisplayCards();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001E4D RID: 7757 RVA: 0x000C6090 File Offset: 0x000C4290
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDeckViewScreen.MethodName._Ready || (ref method) == NDeckViewScreen.MethodName.ConnectSignals || (ref method) == NDeckViewScreen.MethodName._EnterTree || (ref method) == NDeckViewScreen.MethodName._ExitTree || (ref method) == NDeckViewScreen.MethodName.AfterCapstoneClosed || (ref method) == NDeckViewScreen.MethodName.OnPileContentsChanged || (ref method) == NDeckViewScreen.MethodName.OnObtainedSort || (ref method) == NDeckViewScreen.MethodName.OnCardTypeSort || (ref method) == NDeckViewScreen.MethodName.OnCostSort || (ref method) == NDeckViewScreen.MethodName.OnAlphabetSort || (ref method) == NDeckViewScreen.MethodName.DisplayCards || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001E4E RID: 7758 RVA: 0x000C614C File Offset: 0x000C434C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDeckViewScreen.PropertyName._obtainedSorter)
			{
				this._obtainedSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._typeSorter)
			{
				this._typeSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._costSorter)
			{
				this._costSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._alphabetSorter)
			{
				this._alphabetSorter = VariantUtils.ConvertTo<NCardViewSortButton>(ref value);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._bg)
			{
				this._bg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001E4F RID: 7759 RVA: 0x000C61E8 File Offset: 0x000C43E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDeckViewScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._obtainedSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._obtainedSorter);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._typeSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._typeSorter);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._costSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._costSorter);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._alphabetSorter)
			{
				value = VariantUtils.CreateFrom<NCardViewSortButton>(ref this._alphabetSorter);
				return true;
			}
			if ((ref name) == NDeckViewScreen.PropertyName._bg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bg);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E50 RID: 7760 RVA: 0x000C62C0 File Offset: 0x000C44C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NDeckViewScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckViewScreen.PropertyName._obtainedSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckViewScreen.PropertyName._typeSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckViewScreen.PropertyName._costSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckViewScreen.PropertyName._alphabetSorter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDeckViewScreen.PropertyName._bg, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E51 RID: 7761 RVA: 0x000C639C File Offset: 0x000C459C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDeckViewScreen.PropertyName._obtainedSorter, Variant.From<NCardViewSortButton>(ref this._obtainedSorter));
			info.AddProperty(NDeckViewScreen.PropertyName._typeSorter, Variant.From<NCardViewSortButton>(ref this._typeSorter));
			info.AddProperty(NDeckViewScreen.PropertyName._costSorter, Variant.From<NCardViewSortButton>(ref this._costSorter));
			info.AddProperty(NDeckViewScreen.PropertyName._alphabetSorter, Variant.From<NCardViewSortButton>(ref this._alphabetSorter));
			info.AddProperty(NDeckViewScreen.PropertyName._bg, Variant.From<Control>(ref this._bg));
		}

		/// <inheritdoc />
		// Token: 0x06001E52 RID: 7762 RVA: 0x000C6420 File Offset: 0x000C4620
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDeckViewScreen.PropertyName._obtainedSorter, ref variant))
			{
				this._obtainedSorter = variant.As<NCardViewSortButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDeckViewScreen.PropertyName._typeSorter, ref variant2))
			{
				this._typeSorter = variant2.As<NCardViewSortButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDeckViewScreen.PropertyName._costSorter, ref variant3))
			{
				this._costSorter = variant3.As<NCardViewSortButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDeckViewScreen.PropertyName._alphabetSorter, ref variant4))
			{
				this._alphabetSorter = variant4.As<NCardViewSortButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDeckViewScreen.PropertyName._bg, ref variant5))
			{
				this._bg = variant5.As<Control>();
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000C64C0 File Offset: 0x000C46C0
		public unsafe NDeckViewScreen()
		{
			int num = 4;
			List<SortingOrders> list = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
			int num2 = 0;
			*span[num2] = SortingOrders.Ascending;
			num2++;
			*span[num2] = SortingOrders.TypeAscending;
			num2++;
			*span[num2] = SortingOrders.CostAscending;
			num2++;
			*span[num2] = SortingOrders.AlphabetAscending;
			this._sortingPriority = list;
			base..ctor();
		}

		// Token: 0x04000BA2 RID: 2978
		private Player _player;

		// Token: 0x04000BA3 RID: 2979
		private CardPile _pile;

		// Token: 0x04000BA4 RID: 2980
		private NCardViewSortButton _obtainedSorter;

		// Token: 0x04000BA5 RID: 2981
		private NCardViewSortButton _typeSorter;

		// Token: 0x04000BA6 RID: 2982
		private NCardViewSortButton _costSorter;

		// Token: 0x04000BA7 RID: 2983
		private NCardViewSortButton _alphabetSorter;

		// Token: 0x04000BA8 RID: 2984
		private Control _bg;

		// Token: 0x04000BA9 RID: 2985
		private readonly List<SortingOrders> _sortingPriority;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001263 RID: 4707
		[NullableContext(0)]
		public new class MethodName : NCardsViewScreen.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004735 RID: 18229
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004736 RID: 18230
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004737 RID: 18231
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004738 RID: 18232
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'AfterCapstoneClosed' method.
			/// </summary>
			// Token: 0x04004739 RID: 18233
			public new static readonly StringName AfterCapstoneClosed = "AfterCapstoneClosed";

			/// <summary>
			/// Cached name for the 'OnPileContentsChanged' method.
			/// </summary>
			// Token: 0x0400473A RID: 18234
			public static readonly StringName OnPileContentsChanged = "OnPileContentsChanged";

			/// <summary>
			/// Cached name for the 'OnObtainedSort' method.
			/// </summary>
			// Token: 0x0400473B RID: 18235
			public static readonly StringName OnObtainedSort = "OnObtainedSort";

			/// <summary>
			/// Cached name for the 'OnCardTypeSort' method.
			/// </summary>
			// Token: 0x0400473C RID: 18236
			public static readonly StringName OnCardTypeSort = "OnCardTypeSort";

			/// <summary>
			/// Cached name for the 'OnCostSort' method.
			/// </summary>
			// Token: 0x0400473D RID: 18237
			public static readonly StringName OnCostSort = "OnCostSort";

			/// <summary>
			/// Cached name for the 'OnAlphabetSort' method.
			/// </summary>
			// Token: 0x0400473E RID: 18238
			public static readonly StringName OnAlphabetSort = "OnAlphabetSort";

			/// <summary>
			/// Cached name for the 'DisplayCards' method.
			/// </summary>
			// Token: 0x0400473F RID: 18239
			public static readonly StringName DisplayCards = "DisplayCards";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001264 RID: 4708
		[NullableContext(0)]
		public new class PropertyName : NCardsViewScreen.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04004740 RID: 18240
			public new static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the '_obtainedSorter' field.
			/// </summary>
			// Token: 0x04004741 RID: 18241
			public static readonly StringName _obtainedSorter = "_obtainedSorter";

			/// <summary>
			/// Cached name for the '_typeSorter' field.
			/// </summary>
			// Token: 0x04004742 RID: 18242
			public static readonly StringName _typeSorter = "_typeSorter";

			/// <summary>
			/// Cached name for the '_costSorter' field.
			/// </summary>
			// Token: 0x04004743 RID: 18243
			public static readonly StringName _costSorter = "_costSorter";

			/// <summary>
			/// Cached name for the '_alphabetSorter' field.
			/// </summary>
			// Token: 0x04004744 RID: 18244
			public static readonly StringName _alphabetSorter = "_alphabetSorter";

			/// <summary>
			/// Cached name for the '_bg' field.
			/// </summary>
			// Token: 0x04004745 RID: 18245
			public static readonly StringName _bg = "_bg";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001265 RID: 4709
		[NullableContext(0)]
		public new class SignalName : NCardsViewScreen.SignalName
		{
		}
	}
}
