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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// Used by Draw pile, Discard pile, and Exhaust pile screens during combat.
	/// Do not use it for anything else. You have been warned.
	/// </summary>
	// Token: 0x0200024A RID: 586
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NCardPileScreen.cs")]
	public class NCardPileScreen : Control, ICapstoneScreen, IScreenContext
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x000C3024 File Offset: 0x000C1224
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/screens/card_pile_screen");
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x000C3030 File Offset: 0x000C1230
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardPileScreen.ScenePath);
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x000C303C File Offset: 0x000C123C
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.CardPile;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x000C303F File Offset: 0x000C123F
		// (set) Token: 0x06001DFA RID: 7674 RVA: 0x000C3047 File Offset: 0x000C1247
		public CardPile Pile { get; private set; }

		// Token: 0x06001DFB RID: 7675 RVA: 0x000C3050 File Offset: 0x000C1250
		public override void _Ready()
		{
			this._bottomLabel = base.GetNode<MegaRichTextLabel>("%BottomLabel");
			switch (this.Pile.Type)
			{
			case PileType.Draw:
				this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "DRAW_PILE_INFO").GetFormattedText();
				goto IL_00D7;
			case PileType.Discard:
				this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "DISCARD_PILE_INFO").GetFormattedText();
				goto IL_00D7;
			case PileType.Exhaust:
				this._bottomLabel.Text = "[center]" + new LocString("gameplay_ui", "EXHAUST_PILE_INFO").GetFormattedText();
				goto IL_00D7;
			}
			this._bottomLabel.Visible = false;
			Log.Info("CardPileScreen has no info text.", 2);
			IL_00D7:
			this._backButton = base.GetNode<NButton>("BackButton");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnReturnButtonPressed)), 0U);
			this._backButton.Enable();
			this._grid = base.GetNode<NCardGrid>("CardGrid");
			this.OnPileContentsChanged();
			this._grid.InsetForTopBar();
			this._background = base.GetNode<ColorRect>("Background");
			this._background.Modulate = StsColors.transparentBlack;
			this._currentTween = base.CreateTween();
			this._currentTween.TweenProperty(this._background, "modulate", StsColors.screenBackdrop, 1.0).SetEase(1L).SetTrans(5L);
			base.ProcessMode = (base.Visible ? 0L : 4L);
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000C3224 File Offset: 0x000C1424
		public override void _EnterTree()
		{
			base._EnterTree();
			this.Pile.ContentsChanged += this.OnPileContentsChanged;
			foreach (string text in this._closeHotkeys)
			{
				NHotkeyManager.Instance.PushHotkeyReleasedBinding(text, new Action(NCapstoneContainer.Instance.Close));
			}
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000C3284 File Offset: 0x000C1484
		public override void _ExitTree()
		{
			base._ExitTree();
			this.Pile.ContentsChanged -= this.OnPileContentsChanged;
			foreach (string text in this._closeHotkeys)
			{
				NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(text, new Action(NCapstoneContainer.Instance.Close));
			}
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000C32E4 File Offset: 0x000C14E4
		public static NCardPileScreen ShowScreen(CardPile pile, string[] closeHotkeys)
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("map_open.mp3", 1f, PitchVariance.None);
			}
			NCardPileScreen ncardPileScreen = PreloadManager.Cache.GetScene(NCardPileScreen.ScenePath).Instantiate<NCardPileScreen>(0L);
			Node node = ncardPileScreen;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted("NCardPileScreen");
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted<PileType>(pile.Type);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			ncardPileScreen.Pile = pile;
			ncardPileScreen._closeHotkeys = closeHotkeys;
			NCapstoneContainer.Instance.Open(ncardPileScreen);
			return ncardPileScreen;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000C3380 File Offset: 0x000C1580
		private unsafe void OnPileContentsChanged()
		{
			List<CardModel> list = this.Pile.Cards.ToList<CardModel>();
			if (this.Pile.Type == PileType.Draw)
			{
				list.Sort(delegate(CardModel c1, CardModel c2)
				{
					if (c1.Rarity != c2.Rarity)
					{
						return c1.Rarity.CompareTo(c2.Rarity);
					}
					return string.Compare(c1.Id.Entry, c2.Id.Entry, StringComparison.Ordinal);
				});
			}
			NCardGrid grid = this._grid;
			IReadOnlyList<CardModel> readOnlyList = list;
			PileType type = this.Pile.Type;
			int num = 1;
			List<SortingOrders> list2 = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list2, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list2);
			int num2 = 0;
			*span[num2] = SortingOrders.Ascending;
			grid.SetCards(readOnlyList, type, list2, null);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000C340B File Offset: 0x000C160B
		private void OnReturnButtonPressed(NButton _)
		{
			NCapstoneContainer.Instance.Close();
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000C3417 File Offset: 0x000C1617
		public void AfterCapstoneOpened()
		{
			base.Visible = true;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000C3420 File Offset: 0x000C1620
		public void AfterCapstoneClosed()
		{
			base.Visible = false;
			this.QueueFreeSafely();
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x000C342F File Offset: 0x000C162F
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x000C3432 File Offset: 0x000C1632
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._grid.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E05 RID: 7685 RVA: 0x000C3440 File Offset: 0x000C1640
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NCardPileScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPileScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPileScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPileScreen.MethodName.OnPileContentsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPileScreen.MethodName.OnReturnButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardPileScreen.MethodName.AfterCapstoneOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPileScreen.MethodName.AfterCapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E06 RID: 7686 RVA: 0x000C35D0 File Offset: 0x000C17D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardPileScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName.OnPileContentsChanged && args.Count == 0)
			{
				this.OnPileContentsChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName.OnReturnButtonPressed && args.Count == 1)
			{
				this.OnReturnButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName.AfterCapstoneOpened && args.Count == 0)
			{
				this.AfterCapstoneOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPileScreen.MethodName.AfterCapstoneClosed && args.Count == 0)
			{
				this.AfterCapstoneClosed();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001E07 RID: 7687 RVA: 0x000C36F8 File Offset: 0x000C18F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardPileScreen.MethodName._Ready || (ref method) == NCardPileScreen.MethodName._EnterTree || (ref method) == NCardPileScreen.MethodName._ExitTree || (ref method) == NCardPileScreen.MethodName.OnPileContentsChanged || (ref method) == NCardPileScreen.MethodName.OnReturnButtonPressed || (ref method) == NCardPileScreen.MethodName.AfterCapstoneOpened || (ref method) == NCardPileScreen.MethodName.AfterCapstoneClosed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001E08 RID: 7688 RVA: 0x000C3778 File Offset: 0x000C1978
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardPileScreen.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._grid)
			{
				this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._bottomLabel)
			{
				this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._currentTween)
			{
				this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._closeHotkeys)
			{
				this._closeHotkeys = VariantUtils.ConvertTo<string[]>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001E09 RID: 7689 RVA: 0x000C3830 File Offset: 0x000C1A30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardPileScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._background);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._grid)
			{
				value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._bottomLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._currentTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
				return true;
			}
			if ((ref name) == NCardPileScreen.PropertyName._closeHotkeys)
			{
				value = VariantUtils.CreateFrom<string[]>(ref this._closeHotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E0A RID: 7690 RVA: 0x000C3970 File Offset: 0x000C1B70
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardPileScreen.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPileScreen.PropertyName._grid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPileScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPileScreen.PropertyName._bottomLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPileScreen.PropertyName._currentTween, 0L, "", 4096L, false),
				new PropertyInfo(34L, NCardPileScreen.PropertyName._closeHotkeys, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardPileScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardPileScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPileScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E0B RID: 7691 RVA: 0x000C3AAC File Offset: 0x000C1CAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardPileScreen.PropertyName._background, Variant.From<ColorRect>(ref this._background));
			info.AddProperty(NCardPileScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
			info.AddProperty(NCardPileScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
			info.AddProperty(NCardPileScreen.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
			info.AddProperty(NCardPileScreen.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
			info.AddProperty(NCardPileScreen.PropertyName._closeHotkeys, Variant.From<string[]>(ref this._closeHotkeys));
		}

		/// <inheritdoc />
		// Token: 0x06001E0C RID: 7692 RVA: 0x000C3B44 File Offset: 0x000C1D44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._background, ref variant))
			{
				this._background = variant.As<ColorRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._grid, ref variant2))
			{
				this._grid = variant2.As<NCardGrid>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._backButton, ref variant3))
			{
				this._backButton = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._bottomLabel, ref variant4))
			{
				this._bottomLabel = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._currentTween, ref variant5))
			{
				this._currentTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardPileScreen.PropertyName._closeHotkeys, ref variant6))
			{
				this._closeHotkeys = variant6.As<string[]>();
			}
		}

		// Token: 0x04000B8A RID: 2954
		private ColorRect _background;

		// Token: 0x04000B8B RID: 2955
		private NCardGrid _grid;

		// Token: 0x04000B8C RID: 2956
		private NButton _backButton;

		// Token: 0x04000B8D RID: 2957
		private MegaRichTextLabel _bottomLabel;

		// Token: 0x04000B8E RID: 2958
		[Nullable(2)]
		private Tween _currentTween;

		// Token: 0x04000B8F RID: 2959
		private string[] _closeHotkeys = Array.Empty<string>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001256 RID: 4694
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040046F8 RID: 18168
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040046F9 RID: 18169
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040046FA RID: 18170
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnPileContentsChanged' method.
			/// </summary>
			// Token: 0x040046FB RID: 18171
			public static readonly StringName OnPileContentsChanged = "OnPileContentsChanged";

			/// <summary>
			/// Cached name for the 'OnReturnButtonPressed' method.
			/// </summary>
			// Token: 0x040046FC RID: 18172
			public static readonly StringName OnReturnButtonPressed = "OnReturnButtonPressed";

			/// <summary>
			/// Cached name for the 'AfterCapstoneOpened' method.
			/// </summary>
			// Token: 0x040046FD RID: 18173
			public static readonly StringName AfterCapstoneOpened = "AfterCapstoneOpened";

			/// <summary>
			/// Cached name for the 'AfterCapstoneClosed' method.
			/// </summary>
			// Token: 0x040046FE RID: 18174
			public static readonly StringName AfterCapstoneClosed = "AfterCapstoneClosed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001257 RID: 4695
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x040046FF RID: 18175
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004700 RID: 18176
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004701 RID: 18177
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x04004702 RID: 18178
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_grid' field.
			/// </summary>
			// Token: 0x04004703 RID: 18179
			public static readonly StringName _grid = "_grid";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004704 RID: 18180
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_bottomLabel' field.
			/// </summary>
			// Token: 0x04004705 RID: 18181
			public static readonly StringName _bottomLabel = "_bottomLabel";

			/// <summary>
			/// Cached name for the '_currentTween' field.
			/// </summary>
			// Token: 0x04004706 RID: 18182
			public static readonly StringName _currentTween = "_currentTween";

			/// <summary>
			/// Cached name for the '_closeHotkeys' field.
			/// </summary>
			// Token: 0x04004707 RID: 18183
			public static readonly StringName _closeHotkeys = "_closeHotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001258 RID: 4696
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
