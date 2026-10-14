using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens
{
	/// <summary>
	/// Allows the player to view a single relic. This allows players to see the higher resolution asset and flavor text.
	/// </summary>
	// Token: 0x02000316 RID: 790
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/InspectScreens/NInspectRelicScreen.cs")]
	public class NInspectRelicScreen : Control, IScreenContext
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x001362E1 File Offset: 0x001344E1
		public static string[] AssetPaths
		{
			get
			{
				return new string[] { NInspectRelicScreen._scenePath };
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x001362F1 File Offset: 0x001344F1
		[NullableContext(2)]
		public static NInspectRelicScreen Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NInspectRelicScreen._scenePath).Instantiate<NInspectRelicScreen>(0L);
		}

		/// <summary>
		/// Opens the Relic Inspection Screen.
		/// </summary>
		/// <param name="relics">The collection of relics this relic is a part of (used for left/right arrows)</param>
		/// <param name="relic">The relic the user clicked on to open this screen (index is automatically identified)</param>
		// Token: 0x06002BE1 RID: 11233 RVA: 0x00136314 File Offset: 0x00134514
		public void Open(IReadOnlyList<RelicModel> relics, RelicModel relic)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Inspecting Relic: ");
			defaultInterpolatedStringHandler.AppendFormatted<LocString>(relic.Title);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			this._allUnlockedRelics.Clear();
			this._allUnlockedRelics.UnionWith(unlockState.Relics);
			this._relics = relics.ToList<RelicModel>();
			this._index = relics.IndexOf(relic);
			this.SetRelic(this._index);
			base.Visible = true;
			this._popup.Modulate = StsColors.transparentBlack;
			this._leftButton.Modulate = StsColors.transparentBlack;
			this._rightButton.Modulate = StsColors.transparentBlack;
			this._leftButton.Enable();
			this._rightButton.Enable();
			this._backstop.Visible = true;
			this._backstop.MouseFilter = 0L;
			this._leftButton.MouseFilter = 0L;
			this._rightButton.MouseFilter = 0L;
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween().SetParallel(true);
			this._screenTween.TweenProperty(this._backstop, "modulate:a", 0.9f, 0.25);
			this._screenTween.TweenProperty(this._leftButton, "position:x", this._leftButtonX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._leftButtonX + 100f)
				.SetDelay(0.1);
			this._screenTween.TweenProperty(this._leftButton, "modulate", Colors.White, 0.25).SetDelay(0.1);
			this._screenTween.TweenProperty(this._rightButton, "position:x", this._rightButtonX, 0.25).SetEase(1L).SetTrans(10L)
				.From(this._rightButtonX - 100f)
				.SetDelay(0.1);
			this._screenTween.TweenProperty(this._rightButton, "modulate", Colors.White, 0.25).SetDelay(0.1);
			Tween popupTween = this._popupTween;
			if (popupTween != null)
			{
				popupTween.Kill();
			}
			this._popupTween = base.CreateTween().SetParallel(true);
			this._popupTween.TweenProperty(this._popup, "modulate", Colors.White, 0.25);
			this._popupTween.TweenProperty(this._popup, "position", this._popupPosition, 0.25).SetEase(1L).SetTrans(7L)
				.From(this._popupPosition + new Vector2(0f, 200f));
			ActiveScreenContext.Instance.Update();
			NHotkeyManager.Instance.AddBlockingScreen(this);
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.cancel, new Action(this.Close));
			NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.pauseAndBack, new Action(this.Close));
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x001366B8 File Offset: 0x001348B8
		public override void _Ready()
		{
			this._popup = base.GetNode<Control>("%Popup");
			this._backstop = base.GetNode<Control>("%Backstop");
			this._nameLabel = base.GetNode<MegaLabel>("%RelicName");
			this._rarityLabel = base.GetNode<MegaLabel>("%Rarity");
			this._description = base.GetNode<MegaRichTextLabel>("%RelicDescription");
			this._flavor = base.GetNode<MegaRichTextLabel>("%FlavorText");
			this._relicImage = base.GetNode<TextureRect>("%RelicImage");
			this._frameHsv = (ShaderMaterial)base.GetNode<Control>("%Frame").Material;
			this._leftButton = base.GetNode<NGoldArrowButton>("%LeftArrow");
			this._rightButton = base.GetNode<NGoldArrowButton>("%RightArrow");
			this._popupPosition = this._popup.Position;
			this._hoverTipRect = base.GetNode<Control>("HoverTipRect");
			this._backstop = base.GetNode<NButton>("Backstop");
			this._backstop.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnBackstopPressed)), 0U);
			this._leftButton = base.GetNode<NGoldArrowButton>("LeftArrow");
			this._leftButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnLeftButtonPressed)), 0U);
			this._rightButton = base.GetNode<NGoldArrowButton>("RightArrow");
			this._rightButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRightButtonPressed)), 0U);
			this._leftButtonX = this._leftButton.Position.X;
			this._rightButtonX = this._rightButton.Position.X;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x001368AC File Offset: 0x00134AAC
		public override void _Input(InputEvent inputEvent)
		{
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (NDevConsole.IsConsoleVisible)
			{
				return;
			}
			Control control = base.GetViewport().GuiGetFocusOwner();
			bool flag = control is TextEdit || control is LineEdit;
			if (flag)
			{
				return;
			}
			if (inputEvent.IsActionPressed(MegaInput.left, false, false))
			{
				this.OnLeftButtonPressed(this._leftButton);
			}
			if (inputEvent.IsActionPressed(MegaInput.right, false, false))
			{
				this.OnRightButtonPressed(this._rightButton);
			}
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x00136928 File Offset: 0x00134B28
		private void OnRightButtonPressed(NButton button)
		{
			this.SetRelic(this._index + 1);
			this._popup.Modulate = Colors.White;
			Tween popupTween = this._popupTween;
			if (popupTween != null)
			{
				popupTween.Kill();
			}
			this._popupTween = base.CreateTween().SetParallel(true);
			this._popupTween.TweenProperty(this._popup, "position", this._popupPosition, 0.25).SetEase(1L).SetTrans(5L)
				.From(this._popupPosition + new Vector2(100f, 0f));
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x001369D8 File Offset: 0x00134BD8
		private void OnLeftButtonPressed(NButton button)
		{
			this.SetRelic(this._index - 1);
			this._popup.Modulate = Colors.White;
			Tween popupTween = this._popupTween;
			if (popupTween != null)
			{
				popupTween.Kill();
			}
			this._popupTween = base.CreateTween().SetParallel(true);
			this._popupTween.TweenProperty(this._popup, "position", this._popupPosition, 0.25).SetEase(1L).SetTrans(5L)
				.From(this._popupPosition + new Vector2(-100f, 0f));
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00136A88 File Offset: 0x00134C88
		private void SetRelic(int index)
		{
			this._index = Math.Clamp(index, 0, this._relics.Count - 1);
			this._leftButton.Visible = this._index > 0;
			this._leftButton.MouseFilter = ((this._index > 0) ? 0L : 2L);
			this._rightButton.Visible = this._index < this._relics.Count - 1;
			this._rightButton.MouseFilter = ((this._index < this._relics.Count - 1) ? 0L : 2L);
			this.UpdateRelicDisplay();
		}

		/// <summary>
		/// Call to update the screen to match the relic we're viewing.
		/// Called on screen open and when paging left/right.
		/// </summary>
		// Token: 0x06002BE7 RID: 11239 RVA: 0x00136B2C File Offset: 0x00134D2C
		private void UpdateRelicDisplay()
		{
			RelicModel relicModel = this._relics[this._index];
			if (!this._allUnlockedRelics.Contains(relicModel.CanonicalInstance))
			{
				this._nameLabel.SetTextAutoSize(new LocString("inspect_relic_screen", "LOCKED_TITLE").GetFormattedText());
				this._rarityLabel.SetTextAutoSize(string.Empty);
				this._relicImage.SelfModulate = Colors.White;
				this._description.SetTextAutoSize(new LocString("inspect_relic_screen", "LOCKED_DESCRIPTION").GetFormattedText());
				this._flavor.Text = string.Empty;
				this.SetRarityVisuals(RelicRarity.Common);
				this._relicImage.Texture = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("packed/common_ui/locked_model.png"));
			}
			else if (!SaveManager.Instance.IsRelicSeen(relicModel))
			{
				this._nameLabel.SetTextAutoSize(new LocString("inspect_relic_screen", "UNDISCOVERED_TITLE").GetFormattedText());
				this._rarityLabel.SetTextAutoSize(string.Empty);
				this._relicImage.SelfModulate = StsColors.ninetyPercentBlack;
				this._description.SetTextAutoSize(new LocString("inspect_relic_screen", "UNDISCOVERED_DESCRIPTION").GetFormattedText());
				this._flavor.Text = string.Empty;
				this.SetRarityVisuals(relicModel.Rarity);
				this._relicImage.Texture = relicModel.BigIcon;
			}
			else
			{
				this._nameLabel.SetTextAutoSize(relicModel.Title.GetFormattedText());
				LocString locString = new LocString("gameplay_ui", "RELIC_RARITY." + relicModel.Rarity.ToString().ToUpperInvariant());
				this._rarityLabel.SetTextAutoSize(locString.GetFormattedText());
				this._relicImage.SelfModulate = Colors.White;
				this._description.SetTextAutoSize(relicModel.DynamicDescription.GetFormattedText());
				this._flavor.SetTextAutoSize(relicModel.Flavor.GetFormattedText());
				this.SetRarityVisuals(relicModel.Rarity);
				this._relicImage.Texture = relicModel.BigIcon;
			}
			NHoverTipSet.Clear();
			if (SaveManager.Instance.IsRelicSeen(relicModel))
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, relicModel.HoverTipsExcludingRelic, HoverTipAlignment.None);
				if (nhoverTipSet != null)
				{
					nhoverTipSet.SetAlignment(this._hoverTipRect, HoverTip.GetHoverTipAlignment(this, 0.75f));
				}
			}
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00136D84 File Offset: 0x00134F84
		private void SetRarityVisuals(RelicRarity rarity)
		{
			Vector3 vector;
			switch (rarity)
			{
			case RelicRarity.None:
			case RelicRarity.Starter:
			case RelicRarity.Common:
				this._rarityLabel.Modulate = StsColors.cream;
				vector..ctor(0.95f, 0.25f, 0.9f);
				break;
			case RelicRarity.Uncommon:
				this._rarityLabel.Modulate = StsColors.blue;
				vector..ctor(0.426f, 0.8f, 1.1f);
				break;
			case RelicRarity.Rare:
				this._rarityLabel.Modulate = StsColors.gold;
				vector..ctor(1f, 0.8f, 1.15f);
				break;
			case RelicRarity.Shop:
				this._rarityLabel.Modulate = StsColors.blue;
				vector..ctor(0.525f, 2.5f, 0.85f);
				break;
			case RelicRarity.Event:
				this._rarityLabel.Modulate = StsColors.green;
				vector..ctor(0.23f, 0.75f, 0.9f);
				break;
			case RelicRarity.Ancient:
				this._rarityLabel.Modulate = StsColors.red;
				vector..ctor(0.875f, 3f, 0.9f);
				break;
			default:
				Log.Error("Unspecified relic rarity: " + rarity.ToString(), 2);
				throw new ArgumentOutOfRangeException();
			}
			this._frameHsv.SetShaderParameter(NInspectRelicScreen._h, vector.X);
			this._frameHsv.SetShaderParameter(NInspectRelicScreen._s, vector.Y);
			this._frameHsv.SetShaderParameter(NInspectRelicScreen._v, vector.Z);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00136F2C File Offset: 0x0013512C
		public void Close()
		{
			if (!base.Visible)
			{
				return;
			}
			NHotkeyManager.Instance.RemoveBlockingScreen(this);
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.cancel, new Action(this.Close));
			NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.pauseAndBack, new Action(this.Close));
			this._backstop.MouseFilter = 2L;
			this._leftButton.MouseFilter = 2L;
			this._rightButton.MouseFilter = 2L;
			this._leftButton.Disable();
			this._rightButton.Disable();
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween().SetParallel(true);
			this._screenTween.TweenProperty(this._backstop, "modulate:a", 0f, 0.25);
			this._screenTween.TweenProperty(this._leftButton, "modulate:a", 0f, 0.1);
			this._screenTween.TweenProperty(this._rightButton, "modulate:a", 0f, 0.1);
			this._screenTween.TweenProperty(this._popup, "modulate", StsColors.transparentWhite, 0.1);
			this._screenTween.Chain().TweenCallback(Callable.From(delegate
			{
				base.Visible = false;
				ActiveScreenContext.Instance.Update();
			}));
			NHoverTipSet.Clear();
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x001370D0 File Offset: 0x001352D0
		private void OnBackstopPressed(NButton _)
		{
			this.Close();
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002BEB RID: 11243 RVA: 0x001370D8 File Offset: 0x001352D8
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BEC RID: 11244 RVA: 0x001370DC File Offset: 0x001352DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NInspectRelicScreen.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NInspectRelicScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectRelicScreen.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInspectRelicScreen.MethodName.OnRightButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NInspectRelicScreen.MethodName.OnLeftButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NInspectRelicScreen.MethodName.SetRelic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInspectRelicScreen.MethodName.UpdateRelicDisplay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectRelicScreen.MethodName.SetRarityVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NInspectRelicScreen.MethodName.Close, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInspectRelicScreen.MethodName.OnBackstopPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BED RID: 11245 RVA: 0x001373E0 File Offset: 0x001355E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInspectRelicScreen.MethodName.Create && args.Count == 0)
			{
				NInspectRelicScreen ninspectRelicScreen = NInspectRelicScreen.Create();
				ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref ninspectRelicScreen);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.OnRightButtonPressed && args.Count == 1)
			{
				this.OnRightButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.OnLeftButtonPressed && args.Count == 1)
			{
				this.OnLeftButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.SetRelic && args.Count == 1)
			{
				this.SetRelic(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.UpdateRelicDisplay && args.Count == 0)
			{
				this.UpdateRelicDisplay();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.SetRarityVisuals && args.Count == 1)
			{
				this.SetRarityVisuals(VariantUtils.ConvertTo<RelicRarity>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.Close && args.Count == 0)
			{
				this.Close();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInspectRelicScreen.MethodName.OnBackstopPressed && args.Count == 1)
			{
				this.OnBackstopPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x001375C4 File Offset: 0x001357C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInspectRelicScreen.MethodName.Create && args.Count == 0)
			{
				NInspectRelicScreen ninspectRelicScreen = NInspectRelicScreen.Create();
				ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref ninspectRelicScreen);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002BEF RID: 11247 RVA: 0x00137604 File Offset: 0x00135804
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInspectRelicScreen.MethodName.Create || (ref method) == NInspectRelicScreen.MethodName._Ready || (ref method) == NInspectRelicScreen.MethodName._Input || (ref method) == NInspectRelicScreen.MethodName.OnRightButtonPressed || (ref method) == NInspectRelicScreen.MethodName.OnLeftButtonPressed || (ref method) == NInspectRelicScreen.MethodName.SetRelic || (ref method) == NInspectRelicScreen.MethodName.UpdateRelicDisplay || (ref method) == NInspectRelicScreen.MethodName.SetRarityVisuals || (ref method) == NInspectRelicScreen.MethodName.Close || (ref method) == NInspectRelicScreen.MethodName.OnBackstopPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002BF0 RID: 11248 RVA: 0x001376B0 File Offset: 0x001358B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInspectRelicScreen.PropertyName._popup)
			{
				this._popup = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._backstop)
			{
				this._backstop = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._nameLabel)
			{
				this._nameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rarityLabel)
			{
				this._rarityLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._description)
			{
				this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._flavor)
			{
				this._flavor = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._relicImage)
			{
				this._relicImage = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._frameHsv)
			{
				this._frameHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._leftButton)
			{
				this._leftButton = VariantUtils.ConvertTo<NGoldArrowButton>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rightButton)
			{
				this._rightButton = VariantUtils.ConvertTo<NGoldArrowButton>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._hoverTipRect)
			{
				this._hoverTipRect = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._popupTween)
			{
				this._popupTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._popupPosition)
			{
				this._popupPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._leftButtonX)
			{
				this._leftButtonX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rightButtonX)
			{
				this._rightButtonX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._index)
			{
				this._index = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002BF1 RID: 11249 RVA: 0x00137890 File Offset: 0x00135A90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInspectRelicScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._popup)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._popup);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._backstop)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._backstop);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._nameLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameLabel);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rarityLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._rarityLabel);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._description)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._flavor)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._flavor);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._relicImage)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._relicImage);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._frameHsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._frameHsv);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._leftButton)
			{
				value = VariantUtils.CreateFrom<NGoldArrowButton>(ref this._leftButton);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rightButton)
			{
				value = VariantUtils.CreateFrom<NGoldArrowButton>(ref this._rightButton);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._hoverTipRect)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hoverTipRect);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._screenTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._popupTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._popupTween);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._popupPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._popupPosition);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._leftButtonX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._leftButtonX);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._rightButtonX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rightButtonX);
				return true;
			}
			if ((ref name) == NInspectRelicScreen.PropertyName._index)
			{
				value = VariantUtils.CreateFrom<int>(ref this._index);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BF2 RID: 11250 RVA: 0x00137AE8 File Offset: 0x00135CE8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._popup, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._backstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._nameLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._rarityLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._description, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._flavor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._relicImage, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._frameHsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._leftButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._rightButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._hoverTipRect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._screenTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName._popupTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NInspectRelicScreen.PropertyName._popupPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NInspectRelicScreen.PropertyName._leftButtonX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NInspectRelicScreen.PropertyName._rightButtonX, 0L, "", 4096L, false),
				new PropertyInfo(2L, NInspectRelicScreen.PropertyName._index, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInspectRelicScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BF3 RID: 11251 RVA: 0x00137D4C File Offset: 0x00135F4C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NInspectRelicScreen.PropertyName._popup, Variant.From<Control>(ref this._popup));
			info.AddProperty(NInspectRelicScreen.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
			info.AddProperty(NInspectRelicScreen.PropertyName._nameLabel, Variant.From<MegaLabel>(ref this._nameLabel));
			info.AddProperty(NInspectRelicScreen.PropertyName._rarityLabel, Variant.From<MegaLabel>(ref this._rarityLabel));
			info.AddProperty(NInspectRelicScreen.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
			info.AddProperty(NInspectRelicScreen.PropertyName._flavor, Variant.From<MegaRichTextLabel>(ref this._flavor));
			info.AddProperty(NInspectRelicScreen.PropertyName._relicImage, Variant.From<TextureRect>(ref this._relicImage));
			info.AddProperty(NInspectRelicScreen.PropertyName._frameHsv, Variant.From<ShaderMaterial>(ref this._frameHsv));
			info.AddProperty(NInspectRelicScreen.PropertyName._leftButton, Variant.From<NGoldArrowButton>(ref this._leftButton));
			info.AddProperty(NInspectRelicScreen.PropertyName._rightButton, Variant.From<NGoldArrowButton>(ref this._rightButton));
			info.AddProperty(NInspectRelicScreen.PropertyName._hoverTipRect, Variant.From<Control>(ref this._hoverTipRect));
			info.AddProperty(NInspectRelicScreen.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
			info.AddProperty(NInspectRelicScreen.PropertyName._popupTween, Variant.From<Tween>(ref this._popupTween));
			info.AddProperty(NInspectRelicScreen.PropertyName._popupPosition, Variant.From<Vector2>(ref this._popupPosition));
			info.AddProperty(NInspectRelicScreen.PropertyName._leftButtonX, Variant.From<float>(ref this._leftButtonX));
			info.AddProperty(NInspectRelicScreen.PropertyName._rightButtonX, Variant.From<float>(ref this._rightButtonX));
			info.AddProperty(NInspectRelicScreen.PropertyName._index, Variant.From<int>(ref this._index));
		}

		/// <inheritdoc />
		// Token: 0x06002BF4 RID: 11252 RVA: 0x00137ED8 File Offset: 0x001360D8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popup, ref variant))
			{
				this._popup = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._backstop, ref variant2))
			{
				this._backstop = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._nameLabel, ref variant3))
			{
				this._nameLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rarityLabel, ref variant4))
			{
				this._rarityLabel = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._description, ref variant5))
			{
				this._description = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._flavor, ref variant6))
			{
				this._flavor = variant6.As<MegaRichTextLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._relicImage, ref variant7))
			{
				this._relicImage = variant7.As<TextureRect>();
			}
			Variant variant8;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._frameHsv, ref variant8))
			{
				this._frameHsv = variant8.As<ShaderMaterial>();
			}
			Variant variant9;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._leftButton, ref variant9))
			{
				this._leftButton = variant9.As<NGoldArrowButton>();
			}
			Variant variant10;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rightButton, ref variant10))
			{
				this._rightButton = variant10.As<NGoldArrowButton>();
			}
			Variant variant11;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._hoverTipRect, ref variant11))
			{
				this._hoverTipRect = variant11.As<Control>();
			}
			Variant variant12;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._screenTween, ref variant12))
			{
				this._screenTween = variant12.As<Tween>();
			}
			Variant variant13;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popupTween, ref variant13))
			{
				this._popupTween = variant13.As<Tween>();
			}
			Variant variant14;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._popupPosition, ref variant14))
			{
				this._popupPosition = variant14.As<Vector2>();
			}
			Variant variant15;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._leftButtonX, ref variant15))
			{
				this._leftButtonX = variant15.As<float>();
			}
			Variant variant16;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._rightButtonX, ref variant16))
			{
				this._rightButtonX = variant16.As<float>();
			}
			Variant variant17;
			if (info.TryGetProperty(NInspectRelicScreen.PropertyName._index, ref variant17))
			{
				this._index = variant17.As<int>();
			}
		}

		// Token: 0x04001079 RID: 4217
		private static readonly StringName _v = new StringName("v");

		// Token: 0x0400107A RID: 4218
		private static readonly StringName _s = new StringName("s");

		// Token: 0x0400107B RID: 4219
		private static readonly StringName _h = new StringName("h");

		// Token: 0x0400107C RID: 4220
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/inspect_relic_screen/inspect_relic_screen");

		// Token: 0x0400107D RID: 4221
		private Control _popup;

		// Token: 0x0400107E RID: 4222
		private Control _backstop;

		// Token: 0x0400107F RID: 4223
		private MegaLabel _nameLabel;

		// Token: 0x04001080 RID: 4224
		private MegaLabel _rarityLabel;

		// Token: 0x04001081 RID: 4225
		private MegaRichTextLabel _description;

		// Token: 0x04001082 RID: 4226
		private MegaRichTextLabel _flavor;

		// Token: 0x04001083 RID: 4227
		private TextureRect _relicImage;

		// Token: 0x04001084 RID: 4228
		private ShaderMaterial _frameHsv;

		// Token: 0x04001085 RID: 4229
		private NGoldArrowButton _leftButton;

		// Token: 0x04001086 RID: 4230
		private NGoldArrowButton _rightButton;

		// Token: 0x04001087 RID: 4231
		private Control _hoverTipRect;

		// Token: 0x04001088 RID: 4232
		[Nullable(2)]
		private Tween _screenTween;

		// Token: 0x04001089 RID: 4233
		[Nullable(2)]
		private Tween _popupTween;

		// Token: 0x0400108A RID: 4234
		private Vector2 _popupPosition;

		// Token: 0x0400108B RID: 4235
		private float _leftButtonX;

		// Token: 0x0400108C RID: 4236
		private float _rightButtonX;

		// Token: 0x0400108D RID: 4237
		private const double _arrowButtonDelay = 0.1;

		// Token: 0x0400108E RID: 4238
		private IReadOnlyList<RelicModel> _relics;

		// Token: 0x0400108F RID: 4239
		private int _index;

		// Token: 0x04001090 RID: 4240
		private HashSet<RelicModel> _allUnlockedRelics = new HashSet<RelicModel>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153D RID: 5437
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005135 RID: 20789
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005136 RID: 20790
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005137 RID: 20791
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnRightButtonPressed' method.
			/// </summary>
			// Token: 0x04005138 RID: 20792
			public static readonly StringName OnRightButtonPressed = "OnRightButtonPressed";

			/// <summary>
			/// Cached name for the 'OnLeftButtonPressed' method.
			/// </summary>
			// Token: 0x04005139 RID: 20793
			public static readonly StringName OnLeftButtonPressed = "OnLeftButtonPressed";

			/// <summary>
			/// Cached name for the 'SetRelic' method.
			/// </summary>
			// Token: 0x0400513A RID: 20794
			public static readonly StringName SetRelic = "SetRelic";

			/// <summary>
			/// Cached name for the 'UpdateRelicDisplay' method.
			/// </summary>
			// Token: 0x0400513B RID: 20795
			public static readonly StringName UpdateRelicDisplay = "UpdateRelicDisplay";

			/// <summary>
			/// Cached name for the 'SetRarityVisuals' method.
			/// </summary>
			// Token: 0x0400513C RID: 20796
			public static readonly StringName SetRarityVisuals = "SetRarityVisuals";

			/// <summary>
			/// Cached name for the 'Close' method.
			/// </summary>
			// Token: 0x0400513D RID: 20797
			public static readonly StringName Close = "Close";

			/// <summary>
			/// Cached name for the 'OnBackstopPressed' method.
			/// </summary>
			// Token: 0x0400513E RID: 20798
			public static readonly StringName OnBackstopPressed = "OnBackstopPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153E RID: 5438
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x0400513F RID: 20799
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_popup' field.
			/// </summary>
			// Token: 0x04005140 RID: 20800
			public static readonly StringName _popup = "_popup";

			/// <summary>
			/// Cached name for the '_backstop' field.
			/// </summary>
			// Token: 0x04005141 RID: 20801
			public static readonly StringName _backstop = "_backstop";

			/// <summary>
			/// Cached name for the '_nameLabel' field.
			/// </summary>
			// Token: 0x04005142 RID: 20802
			public static readonly StringName _nameLabel = "_nameLabel";

			/// <summary>
			/// Cached name for the '_rarityLabel' field.
			/// </summary>
			// Token: 0x04005143 RID: 20803
			public static readonly StringName _rarityLabel = "_rarityLabel";

			/// <summary>
			/// Cached name for the '_description' field.
			/// </summary>
			// Token: 0x04005144 RID: 20804
			public static readonly StringName _description = "_description";

			/// <summary>
			/// Cached name for the '_flavor' field.
			/// </summary>
			// Token: 0x04005145 RID: 20805
			public static readonly StringName _flavor = "_flavor";

			/// <summary>
			/// Cached name for the '_relicImage' field.
			/// </summary>
			// Token: 0x04005146 RID: 20806
			public static readonly StringName _relicImage = "_relicImage";

			/// <summary>
			/// Cached name for the '_frameHsv' field.
			/// </summary>
			// Token: 0x04005147 RID: 20807
			public static readonly StringName _frameHsv = "_frameHsv";

			/// <summary>
			/// Cached name for the '_leftButton' field.
			/// </summary>
			// Token: 0x04005148 RID: 20808
			public static readonly StringName _leftButton = "_leftButton";

			/// <summary>
			/// Cached name for the '_rightButton' field.
			/// </summary>
			// Token: 0x04005149 RID: 20809
			public static readonly StringName _rightButton = "_rightButton";

			/// <summary>
			/// Cached name for the '_hoverTipRect' field.
			/// </summary>
			// Token: 0x0400514A RID: 20810
			public static readonly StringName _hoverTipRect = "_hoverTipRect";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x0400514B RID: 20811
			public static readonly StringName _screenTween = "_screenTween";

			/// <summary>
			/// Cached name for the '_popupTween' field.
			/// </summary>
			// Token: 0x0400514C RID: 20812
			public static readonly StringName _popupTween = "_popupTween";

			/// <summary>
			/// Cached name for the '_popupPosition' field.
			/// </summary>
			// Token: 0x0400514D RID: 20813
			public static readonly StringName _popupPosition = "_popupPosition";

			/// <summary>
			/// Cached name for the '_leftButtonX' field.
			/// </summary>
			// Token: 0x0400514E RID: 20814
			public static readonly StringName _leftButtonX = "_leftButtonX";

			/// <summary>
			/// Cached name for the '_rightButtonX' field.
			/// </summary>
			// Token: 0x0400514F RID: 20815
			public static readonly StringName _rightButtonX = "_rightButtonX";

			/// <summary>
			/// Cached name for the '_index' field.
			/// </summary>
			// Token: 0x04005150 RID: 20816
			public static readonly StringName _index = "_index";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200153F RID: 5439
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
