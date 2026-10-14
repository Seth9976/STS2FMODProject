using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// Selection screen for choosing a relic from out of combat generators like JewelryBox
	/// </summary>
	// Token: 0x0200024C RID: 588
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/NChooseARelicSelection.cs")]
	public class NChooseARelicSelection : Control, IOverlayScreen, IScreenContext
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x000C46F0 File Offset: 0x000C28F0
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.Rewards;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x000C46F4 File Offset: 0x000C28F4
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/choose_a_relic_selection_screen");
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x000C4700 File Offset: 0x000C2900
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NChooseARelicSelection.ScenePath);
			}
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000C470C File Offset: 0x000C290C
		[return: Nullable(2)]
		public static NChooseARelicSelection ShowScreen(IReadOnlyList<RelicModel> relics)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NChooseARelicSelection nchooseARelicSelection = PreloadManager.Cache.GetScene(NChooseARelicSelection.ScenePath).Instantiate<NChooseARelicSelection>(0L);
			nchooseARelicSelection.Name = "NChooseACardSelectionScreen";
			nchooseARelicSelection._relics = relics;
			NOverlayStack.Instance.Push(nchooseARelicSelection);
			return nchooseARelicSelection;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000C475C File Offset: 0x000C295C
		public override void _Ready()
		{
			this._banner = base.GetNode<NCommonBanner>("Banner");
			this._banner.label.SetTextAutoSize(new LocString("gameplay_ui", "CHOOSE_RELIC_HEADER").GetRawText());
			this._banner.AnimateIn();
			this._relicRow = base.GetNode<Control>("RelicRow");
			Vector2 vector = Vector2.Left * (float)(this._relics.Count - 1) * 200f * 0.5f;
			for (int i = 0; i < this._relics.Count; i++)
			{
				RelicModel relicModel = this._relics[i];
				NRelicBasicHolder holder = NRelicBasicHolder.Create(relicModel);
				holder.Scale = Vector2.One * 2f;
				this._relicRow.AddChildSafely(holder);
				holder.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
				{
					this.SelectHolder(holder);
				}), 0U);
				this._cardTween = base.CreateTween().SetParallel(true);
				this._cardTween.TweenProperty(holder, "position", holder.Position + vector + Vector2.Right * 200f * (float)i, 0.5).SetEase(1L).SetTrans(5L);
				this._cardTween.TweenProperty(holder, "modulate", Colors.White, 1.0).SetEase(1L).SetTrans(7L)
					.From(Colors.Black);
			}
			this._skipButton = base.GetNode<NChoiceSelectionSkipButton>("SkipButton");
			this._skipButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnSkipButtonReleased)), 0U);
			this._skipButton.AnimateIn();
			List<NRelicBasicHolder> list = this._relicRow.GetChildren(false).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>();
			NRelicBasicHolder nrelicBasicHolder = this._relicRow.GetChildren(false).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>()[list.Count / 2];
			this._skipButton.FocusNeighborTop = nrelicBasicHolder.GetPath();
			this._skipButton.FocusNeighborBottom = this._skipButton.GetPath();
			this._skipButton.FocusNeighborLeft = this._skipButton.GetPath();
			this._skipButton.FocusNeighborRight = this._skipButton.GetPath();
			for (int j = 0; j < this._relicRow.GetChildCount(false); j++)
			{
				Control child = this._relicRow.GetChild<Control>(j, false);
				child.FocusNeighborBottom = child.GetPath();
				child.FocusNeighborTop = child.GetPath();
				child.FocusNeighborLeft = ((j > 0) ? this._relicRow.GetChild(j - 1, false).GetPath() : this._relicRow.GetChild(this._relicRow.GetChildCount(false) - 1, false).GetPath());
				child.FocusNeighborRight = ((j < this._relicRow.GetChildCount(false) - 1) ? this._relicRow.GetChild(j + 1, false).GetPath() : this._relicRow.GetChild(0, false).GetPath());
			}
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000C4AF2 File Offset: 0x000C2CF2
		public override void _ExitTree()
		{
			if (!this._completionSource.Task.IsCompleted)
			{
				this._completionSource.SetCanceled();
			}
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000C4B14 File Offset: 0x000C2D14
		private void SelectHolder(NRelicBasicHolder relicHolder)
		{
			RelicModel model = relicHolder.Relic.Model;
			this._screenComplete = true;
			this._relicSelected = true;
			this._completionSource.SetResult(new RelicModel[] { model });
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000C4B50 File Offset: 0x000C2D50
		public async Task<IEnumerable<RelicModel>> RelicsSelected()
		{
			IEnumerable<RelicModel> enumerable = await this._completionSource.Task;
			IEnumerable<RelicModel> enumerable2 = enumerable;
			NOverlayStack.Instance.Remove(this);
			return enumerable2;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000C4B93 File Offset: 0x000C2D93
		private void OnSkipButtonReleased(NButton _)
		{
			this._screenComplete = true;
			this._completionSource.SetResult(Array.Empty<RelicModel>());
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000C4BAC File Offset: 0x000C2DAC
		public void AfterOverlayOpened()
		{
			base.Modulate = Colors.Transparent;
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this._fadeTween = base.CreateTween();
			this._fadeTween.TweenProperty(this, "modulate:a", 1f, 0.2);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000C4C0B File Offset: 0x000C2E0B
		public void AfterOverlayClosed()
		{
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this.QueueFreeSafely();
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000C4C24 File Offset: 0x000C2E24
		public void AfterOverlayShown()
		{
			base.Visible = true;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000C4C2D File Offset: 0x000C2E2D
		public void AfterOverlayHidden()
		{
			base.Visible = false;
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000C4C36 File Offset: 0x000C2E36
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x000C4C3C File Offset: 0x000C2E3C
		public Control DefaultFocusedControl
		{
			get
			{
				List<NRelicBasicHolder> list = this._relicRow.GetChildren(false).OfType<NRelicBasicHolder>().ToList<NRelicBasicHolder>();
				return list[list.Count / 2];
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E33 RID: 7731 RVA: 0x000C4C70 File Offset: 0x000C2E70
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NChooseARelicSelection.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseARelicSelection.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseARelicSelection.MethodName.SelectHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relicHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseARelicSelection.MethodName.OnSkipButtonReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NChooseARelicSelection.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E34 RID: 7732 RVA: 0x000C4E60 File Offset: 0x000C3060
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NChooseARelicSelection.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.SelectHolder && args.Count == 1)
			{
				this.SelectHolder(VariantUtils.ConvertTo<NRelicBasicHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.OnSkipButtonReleased && args.Count == 1)
			{
				this.OnSkipButtonReleased(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NChooseARelicSelection.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001E35 RID: 7733 RVA: 0x000C4FBC File Offset: 0x000C31BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NChooseARelicSelection.MethodName._Ready || (ref method) == NChooseARelicSelection.MethodName._ExitTree || (ref method) == NChooseARelicSelection.MethodName.SelectHolder || (ref method) == NChooseARelicSelection.MethodName.OnSkipButtonReleased || (ref method) == NChooseARelicSelection.MethodName.AfterOverlayOpened || (ref method) == NChooseARelicSelection.MethodName.AfterOverlayClosed || (ref method) == NChooseARelicSelection.MethodName.AfterOverlayShown || (ref method) == NChooseARelicSelection.MethodName.AfterOverlayHidden || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001E36 RID: 7734 RVA: 0x000C5048 File Offset: 0x000C3248
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NChooseARelicSelection.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._relicRow)
			{
				this._relicRow = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._skipButton)
			{
				this._skipButton = VariantUtils.ConvertTo<NChoiceSelectionSkipButton>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._screenComplete)
			{
				this._screenComplete = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._relicSelected)
			{
				this._relicSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001E37 RID: 7735 RVA: 0x000C511C File Offset: 0x000C331C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NChooseARelicSelection.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._relicRow)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._relicRow);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._skipButton)
			{
				value = VariantUtils.CreateFrom<NChoiceSelectionSkipButton>(ref this._skipButton);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._screenComplete)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._screenComplete);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._relicSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._relicSelected);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			if ((ref name) == NChooseARelicSelection.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E38 RID: 7736 RVA: 0x000C527C File Offset: 0x000C347C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName._relicRow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName._skipButton, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseARelicSelection.PropertyName._screenComplete, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseARelicSelection.PropertyName._relicSelected, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName._cardTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName._fadeTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NChooseARelicSelection.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NChooseARelicSelection.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NChooseARelicSelection.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E39 RID: 7737 RVA: 0x000C53D8 File Offset: 0x000C35D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NChooseARelicSelection.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
			info.AddProperty(NChooseARelicSelection.PropertyName._relicRow, Variant.From<Control>(ref this._relicRow));
			info.AddProperty(NChooseARelicSelection.PropertyName._skipButton, Variant.From<NChoiceSelectionSkipButton>(ref this._skipButton));
			info.AddProperty(NChooseARelicSelection.PropertyName._screenComplete, Variant.From<bool>(ref this._screenComplete));
			info.AddProperty(NChooseARelicSelection.PropertyName._relicSelected, Variant.From<bool>(ref this._relicSelected));
			info.AddProperty(NChooseARelicSelection.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
			info.AddProperty(NChooseARelicSelection.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
		}

		/// <inheritdoc />
		// Token: 0x06001E3A RID: 7738 RVA: 0x000C5488 File Offset: 0x000C3688
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._banner, ref variant))
			{
				this._banner = variant.As<NCommonBanner>();
			}
			Variant variant2;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._relicRow, ref variant2))
			{
				this._relicRow = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._skipButton, ref variant3))
			{
				this._skipButton = variant3.As<NChoiceSelectionSkipButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._screenComplete, ref variant4))
			{
				this._screenComplete = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._relicSelected, ref variant5))
			{
				this._relicSelected = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._cardTween, ref variant6))
			{
				this._cardTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NChooseARelicSelection.PropertyName._fadeTween, ref variant7))
			{
				this._fadeTween = variant7.As<Tween>();
			}
		}

		// Token: 0x04000B98 RID: 2968
		private const float _relicXSpacing = 200f;

		// Token: 0x04000B99 RID: 2969
		private NCommonBanner _banner;

		// Token: 0x04000B9A RID: 2970
		private Control _relicRow;

		// Token: 0x04000B9B RID: 2971
		private NChoiceSelectionSkipButton _skipButton;

		// Token: 0x04000B9C RID: 2972
		private readonly TaskCompletionSource<IEnumerable<RelicModel>> _completionSource = new TaskCompletionSource<IEnumerable<RelicModel>>();

		// Token: 0x04000B9D RID: 2973
		private bool _screenComplete;

		// Token: 0x04000B9E RID: 2974
		private bool _relicSelected;

		// Token: 0x04000B9F RID: 2975
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x04000BA0 RID: 2976
		[Nullable(2)]
		private Tween _fadeTween;

		// Token: 0x04000BA1 RID: 2977
		private IReadOnlyList<RelicModel> _relics;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200125E RID: 4702
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400471D RID: 18205
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400471E RID: 18206
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SelectHolder' method.
			/// </summary>
			// Token: 0x0400471F RID: 18207
			public static readonly StringName SelectHolder = "SelectHolder";

			/// <summary>
			/// Cached name for the 'OnSkipButtonReleased' method.
			/// </summary>
			// Token: 0x04004720 RID: 18208
			public static readonly StringName OnSkipButtonReleased = "OnSkipButtonReleased";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04004721 RID: 18209
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04004722 RID: 18210
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04004723 RID: 18211
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04004724 RID: 18212
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200125F RID: 4703
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04004725 RID: 18213
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004726 RID: 18214
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004727 RID: 18215
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x04004728 RID: 18216
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_relicRow' field.
			/// </summary>
			// Token: 0x04004729 RID: 18217
			public static readonly StringName _relicRow = "_relicRow";

			/// <summary>
			/// Cached name for the '_skipButton' field.
			/// </summary>
			// Token: 0x0400472A RID: 18218
			public static readonly StringName _skipButton = "_skipButton";

			/// <summary>
			/// Cached name for the '_screenComplete' field.
			/// </summary>
			// Token: 0x0400472B RID: 18219
			public static readonly StringName _screenComplete = "_screenComplete";

			/// <summary>
			/// Cached name for the '_relicSelected' field.
			/// </summary>
			// Token: 0x0400472C RID: 18220
			public static readonly StringName _relicSelected = "_relicSelected";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x0400472D RID: 18221
			public static readonly StringName _cardTween = "_cardTween";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x0400472E RID: 18222
			public static readonly StringName _fadeTween = "_fadeTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001260 RID: 4704
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
