using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B9 RID: 697
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsScreen.cs")]
	public class NSettingsScreen : NSubmenu
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000F997F File Offset: 0x000F7B7F
		[Nullable(1)]
		public static string[] AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new string[]
				{
					NSettingsScreen._scenePath,
					"res://images/ui/language_warning.png"
				};
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x000F9997 File Offset: 0x000F7B97
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._settingsTabManager.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Should be called before adding as child!
		/// </summary>
		// Token: 0x060024DD RID: 9437 RVA: 0x000F99A4 File Offset: 0x000F7BA4
		public void SetIsInRun(bool isInRun)
		{
			this._isInRun = isInRun;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000F99B0 File Offset: 0x000F7BB0
		public override void _Ready()
		{
			this.ConnectSignals();
			this._settingsTabManager = base.GetNode<NSettingsTabManager>("%SettingsTabManager");
			this._feedbackScreenButton = base.GetNode<NOpenFeedbackScreenButton>("%FeedbackButton");
			this._moddingScreenButton = base.GetNode<NOpenModdingScreenButton>("%ModdingButton");
			this._toast = base.GetNode<NSettingsToast>("%Toast");
			this.LocalizeLabels();
			base.ProcessMode = (base.Visible ? 0L : 4L);
			this._settingsTabManager.Connect(NSettingsTabManager.SignalName.TabChanged, Callable.From(new Action(this.OnSettingsTabChanged)), 0U);
			this._moddingScreenButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenModdingScreen)), 0U);
			this._feedbackScreenButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenFeedbackScreen)), 0U);
			if (SaveManager.Instance.SettingsSave.ModSettings != null && ModManager.Mods.Count > 0)
			{
				base.GetNode<Control>("%Modding").Visible = true;
				base.GetNode<Control>("%ModdingDivider").Visible = true;
			}
			if (PlatformUtil.GetSupportedWindowMode() == SupportedWindowMode.FullscreenOnly)
			{
				base.GetNode<Control>("%Fullscreen").Visible = false;
				base.GetNode<Control>("%FullscreenDivider").Visible = false;
			}
			if (RunManager.Instance.IsInProgress)
			{
				base.GetNode<Node>("%LanguageLine").GetNode<MegaRichTextLabel>("Label").Modulate = StsColors.gray;
				NLanguageDropdown node = base.GetNode<NLanguageDropdown>("%LanguageDropdown");
				node.Modulate = StsColors.gray;
				node.Disable();
				this._moddingScreenButton.Disable();
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000F9B7E File Offset: 0x000F7D7E
		[NullableContext(1)]
		public void ShowToast(LocString locString)
		{
			this._toast.Show(locString);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000F9B8C File Offset: 0x000F7D8C
		private void OnSettingsTabChanged()
		{
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000F9B90 File Offset: 0x000F7D90
		private void LocalizeLabels()
		{
			Node content = base.GetNode<NSettingsPanel>("%GeneralSettings").Content;
			NSettingsScreen.LocHelper(content.GetNode<Node>("LanguageLine"), new LocString("settings_ui", this._isInRun ? "LANGUAGE_IN_RUN" : "LANGUAGE"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("FastMode"), new LocString("settings_ui", "FASTMODE"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("PhobiaMode"), new LocString("settings_ui", "PHOBIA_MODE"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("Screenshake"), new LocString("settings_ui", "SCREENSHAKE"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("CommonTooltips"), new LocString("settings_ui", "COMMON_TOOLTIPS"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("ShowRunTimer"), new LocString("settings_ui", "SHOW_RUN_TIMER_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("ShowHandCardCount"), new LocString("settings_ui", "SHOW_HAND_CARD_COUNT_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("ShowMpDrawings"), new LocString("settings_ui", "SHOW_MP_DRAWINGS_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("LongPressConfirmations"), new LocString("settings_ui", "LONG_PRESS_CONFIRMATION_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("SkipIntroLogo"), new LocString("settings_ui", "SKIP_INTRO_LOGO_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("LimitFpsInBackground"), new LocString("settings_ui", "LIMIT_FPS_IN_BACKGROUND_HEADER"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("UploadGameplayData"), new LocString("settings_ui", "UPLOAD_GAMEPLAY_DATA"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("TextEffects"), new LocString("settings_ui", "TEXT_EFFECTS"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("SendFeedback"), new LocString("settings_ui", "SEND_FEEDBACK"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("ResetTutorials"), new LocString("settings_ui", "TUTORIAL_RESET"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("Credits"), new LocString("settings_ui", "CREDITS"));
			NSettingsScreen.LocHelper(content.GetNode<Node>("ResetGameplay"), new LocString("settings_ui", "RESET_DEFAULT"));
			Node content2 = base.GetNode<NSettingsPanel>("%GraphicsSettings").Content;
			NSettingsScreen.LocHelper(content2.GetNode<Node>("Fullscreen"), new LocString("settings_ui", "FULLSCREEN"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("DisplaySelection"), new LocString("settings_ui", "DISPLAY_SELECTION"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("WindowedResolution"), new LocString("settings_ui", "RESOLUTION"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("AspectRatio"), new LocString("settings_ui", "ASPECT_RATIO"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("WindowResizing"), new LocString("settings_ui", "WINDOW_RESIZING"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("VSync"), new LocString("settings_ui", "VSYNC"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("MaxFps"), new LocString("settings_ui", "FPS_CAP"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("Msaa"), new LocString("settings_ui", "MSAA"));
			NSettingsScreen.LocHelper(content2.GetNode<Node>("ResetGraphics"), new LocString("settings_ui", "RESET_DEFAULT"));
			Node content3 = base.GetNode<NSettingsPanel>("%SoundSettings").Content;
			NSettingsScreen.LocHelper(content3.GetNode<Node>("MasterVolume"), new LocString("settings_ui", "MASTER_VOLUME"));
			NSettingsScreen.LocHelper(content3.GetNode<Node>("BgmVolume"), new LocString("settings_ui", "MUSIC_VOLUME"));
			NSettingsScreen.LocHelper(content3.GetNode<Node>("SfxVolume"), new LocString("settings_ui", "SFX_VOLUME"));
			NSettingsScreen.LocHelper(content3.GetNode<Node>("AmbienceVolume"), new LocString("settings_ui", "AMBIENCE_VOLUME"));
			NSettingsScreen.LocHelper(content3.GetNode<Node>("MuteIfBackground"), new LocString("settings_ui", "BACKGROUND_MUTE"));
		}

		/// <summary>
		/// Helper class so the localize labels function doesn't look too busy.
		/// </summary>
		// Token: 0x060024E2 RID: 9442 RVA: 0x000FA04A File Offset: 0x000F824A
		[NullableContext(1)]
		private static void LocHelper(Node settingsLineNode, LocString locString)
		{
			settingsLineNode.GetNode<MegaRichTextLabel>("Label").Text = locString.GetFormattedText();
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000FA067 File Offset: 0x000F8267
		[NullableContext(1)]
		private void OpenModdingScreen(NButton _)
		{
			this._stack.PushSubmenuType<NModdingScreen>();
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000FA075 File Offset: 0x000F8275
		[NullableContext(1)]
		private void OpenFeedbackScreen(NButton _)
		{
			this._lastFocusedControl = this._feedbackScreenButton;
			TaskHelper.RunSafely(this.OpenFeedbackScreen());
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000FA090 File Offset: 0x000F8290
		[NullableContext(1)]
		public async Task OpenFeedbackScreen()
		{
			Log.Info("Opening feedback screen", 2);
			base.Visible = false;
			NMainMenu mainMenu = NGame.Instance.MainMenu;
			if (mainMenu != null)
			{
				mainMenu.DisableBackstopInstantly();
			}
			NCapstoneContainer instance = NCapstoneContainer.Instance;
			if (instance != null)
			{
				instance.DisableBackstopInstantly();
			}
			NRun instance2 = NRun.Instance;
			if (instance2 != null)
			{
				instance2.GlobalUi.RelicInventory.ShowImmediately();
			}
			NRun instance3 = NRun.Instance;
			if (instance3 != null)
			{
				instance3.GlobalUi.MultiplayerPlayerContainer.ShowImmediately();
			}
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			Image image = base.GetViewport().GetTexture().GetImage();
			base.Visible = true;
			NRun instance4 = NRun.Instance;
			if (instance4 != null)
			{
				instance4.GlobalUi.RelicInventory.HideImmediately();
			}
			NRun instance5 = NRun.Instance;
			if (instance5 != null)
			{
				instance5.GlobalUi.MultiplayerPlayerContainer.HideImmediately();
			}
			NMainMenu mainMenu2 = NGame.Instance.MainMenu;
			if (mainMenu2 != null)
			{
				mainMenu2.EnableBackstopInstantly();
			}
			NCapstoneContainer instance6 = NCapstoneContainer.Instance;
			if (instance6 != null)
			{
				instance6.EnableBackstopInstantly();
			}
			NSendFeedbackScreen orCreateFeedbackScreen = NGame.Instance.GetOrCreateFeedbackScreen();
			orCreateFeedbackScreen.SetScreenshot(image);
			orCreateFeedbackScreen.Open();
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000FA0D3 File Offset: 0x000F82D3
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			base.ProcessMode = 0L;
			base.EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
			this._settingsTabManager.ResetTabs();
			this._settingsTabManager.Enable();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000FA10A File Offset: 0x000F830A
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			base.ProcessMode = 4L;
			SaveManager.Instance.SaveSettings();
			SaveManager.Instance.SavePrefsFile();
			base.EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
			this._settingsTabManager.Disable();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000FA14A File Offset: 0x000F834A
		protected override void OnSubmenuHidden()
		{
			base.OnSubmenuClosed();
			base.ProcessMode = 4L;
			base.EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000FA16B File Offset: 0x000F836B
		protected override void OnSubmenuShown()
		{
			base.OnSubmenuShown();
			base.ProcessMode = 0L;
			base.EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024EA RID: 9450 RVA: 0x000FA18C File Offset: 0x000F838C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NSettingsScreen.MethodName.SetIsInRun, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isInRun", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSettingsScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.OnSettingsTabChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.LocalizeLabels, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.OpenModdingScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSettingsScreen.MethodName.OpenFeedbackScreen, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSettingsScreen.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.OnSubmenuHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024EB RID: 9451 RVA: 0x000FA400 File Offset: 0x000F8600
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsScreen.MethodName.SetIsInRun && args.Count == 1)
			{
				this.SetIsInRun(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OnSettingsTabChanged && args.Count == 0)
			{
				this.OnSettingsTabChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.LocalizeLabels && args.Count == 0)
			{
				this.LocalizeLabels();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OpenModdingScreen && args.Count == 1)
			{
				this.OpenModdingScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OpenFeedbackScreen && args.Count == 1)
			{
				this.OpenFeedbackScreen(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OnSubmenuHidden && args.Count == 0)
			{
				this.OnSubmenuHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsScreen.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060024EC RID: 9452 RVA: 0x000FA5B4 File Offset: 0x000F87B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsScreen.MethodName.SetIsInRun || (ref method) == NSettingsScreen.MethodName._Ready || (ref method) == NSettingsScreen.MethodName.OnSettingsTabChanged || (ref method) == NSettingsScreen.MethodName.LocalizeLabels || (ref method) == NSettingsScreen.MethodName.OpenModdingScreen || (ref method) == NSettingsScreen.MethodName.OpenFeedbackScreen || (ref method) == NSettingsScreen.MethodName.OnSubmenuOpened || (ref method) == NSettingsScreen.MethodName.OnSubmenuClosed || (ref method) == NSettingsScreen.MethodName.OnSubmenuHidden || (ref method) == NSettingsScreen.MethodName.OnSubmenuShown || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060024ED RID: 9453 RVA: 0x000FA660 File Offset: 0x000F8860
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsScreen.PropertyName._settingsTabManager)
			{
				this._settingsTabManager = VariantUtils.ConvertTo<NSettingsTabManager>(ref value);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._feedbackScreenButton)
			{
				this._feedbackScreenButton = VariantUtils.ConvertTo<NOpenFeedbackScreenButton>(ref value);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._moddingScreenButton)
			{
				this._moddingScreenButton = VariantUtils.ConvertTo<NOpenModdingScreenButton>(ref value);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._toast)
			{
				this._toast = VariantUtils.ConvertTo<NSettingsToast>(ref value);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._isInRun)
			{
				this._isInRun = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060024EE RID: 9454 RVA: 0x000FA6FC File Offset: 0x000F88FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsScreen.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._settingsTabManager)
			{
				value = VariantUtils.CreateFrom<NSettingsTabManager>(ref this._settingsTabManager);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._feedbackScreenButton)
			{
				value = VariantUtils.CreateFrom<NOpenFeedbackScreenButton>(ref this._feedbackScreenButton);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._moddingScreenButton)
			{
				value = VariantUtils.CreateFrom<NOpenModdingScreenButton>(ref this._moddingScreenButton);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._toast)
			{
				value = VariantUtils.CreateFrom<NSettingsToast>(ref this._toast);
				return true;
			}
			if ((ref name) == NSettingsScreen.PropertyName._isInRun)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isInRun);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024EF RID: 9455 RVA: 0x000FA7D4 File Offset: 0x000F89D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsScreen.PropertyName.InitialFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreen.PropertyName._settingsTabManager, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreen.PropertyName._feedbackScreenButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreen.PropertyName._moddingScreenButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsScreen.PropertyName._toast, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSettingsScreen.PropertyName._isInRun, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024F0 RID: 9456 RVA: 0x000FA8B0 File Offset: 0x000F8AB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsScreen.PropertyName._settingsTabManager, Variant.From<NSettingsTabManager>(ref this._settingsTabManager));
			info.AddProperty(NSettingsScreen.PropertyName._feedbackScreenButton, Variant.From<NOpenFeedbackScreenButton>(ref this._feedbackScreenButton));
			info.AddProperty(NSettingsScreen.PropertyName._moddingScreenButton, Variant.From<NOpenModdingScreenButton>(ref this._moddingScreenButton));
			info.AddProperty(NSettingsScreen.PropertyName._toast, Variant.From<NSettingsToast>(ref this._toast));
			info.AddProperty(NSettingsScreen.PropertyName._isInRun, Variant.From<bool>(ref this._isInRun));
			info.AddSignalEventDelegate(NSettingsScreen.SignalName.SettingsClosed, this.backing_SettingsClosed);
			info.AddSignalEventDelegate(NSettingsScreen.SignalName.SettingsOpened, this.backing_SettingsOpened);
		}

		/// <inheritdoc />
		// Token: 0x060024F1 RID: 9457 RVA: 0x000FA954 File Offset: 0x000F8B54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsScreen.PropertyName._settingsTabManager, ref variant))
			{
				this._settingsTabManager = variant.As<NSettingsTabManager>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsScreen.PropertyName._feedbackScreenButton, ref variant2))
			{
				this._feedbackScreenButton = variant2.As<NOpenFeedbackScreenButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsScreen.PropertyName._moddingScreenButton, ref variant3))
			{
				this._moddingScreenButton = variant3.As<NOpenModdingScreenButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSettingsScreen.PropertyName._toast, ref variant4))
			{
				this._toast = variant4.As<NSettingsToast>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSettingsScreen.PropertyName._isInRun, ref variant5))
			{
				this._isInRun = variant5.As<bool>();
			}
			NSettingsScreen.SettingsClosedEventHandler settingsClosedEventHandler;
			if (info.TryGetSignalEventDelegate<NSettingsScreen.SettingsClosedEventHandler>(NSettingsScreen.SignalName.SettingsClosed, ref settingsClosedEventHandler))
			{
				this.backing_SettingsClosed = settingsClosedEventHandler;
			}
			NSettingsScreen.SettingsOpenedEventHandler settingsOpenedEventHandler;
			if (info.TryGetSignalEventDelegate<NSettingsScreen.SettingsOpenedEventHandler>(NSettingsScreen.SignalName.SettingsOpened, ref settingsOpenedEventHandler))
			{
				this.backing_SettingsOpened = settingsOpenedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024F2 RID: 9458 RVA: 0x000FAA24 File Offset: 0x000F8C24
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSettingsScreen.SignalName.SettingsClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsScreen.SignalName.SettingsOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsScreen.SettingsClosedEventHandler" />
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060024F3 RID: 9459 RVA: 0x000FAA97 File Offset: 0x000F8C97
		// (remove) Token: 0x060024F4 RID: 9460 RVA: 0x000FAAB0 File Offset: 0x000F8CB0
		public event NSettingsScreen.SettingsClosedEventHandler SettingsClosed
		{
			add
			{
				this.backing_SettingsClosed = (NSettingsScreen.SettingsClosedEventHandler)Delegate.Combine(this.backing_SettingsClosed, value);
			}
			remove
			{
				this.backing_SettingsClosed = (NSettingsScreen.SettingsClosedEventHandler)Delegate.Remove(this.backing_SettingsClosed, value);
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000FAAC9 File Offset: 0x000F8CC9
		protected void EmitSignalSettingsClosed()
		{
			base.EmitSignal(NSettingsScreen.SignalName.SettingsClosed, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsScreen.SettingsOpenedEventHandler" />
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060024F6 RID: 9462 RVA: 0x000FAADC File Offset: 0x000F8CDC
		// (remove) Token: 0x060024F7 RID: 9463 RVA: 0x000FAAF5 File Offset: 0x000F8CF5
		public event NSettingsScreen.SettingsOpenedEventHandler SettingsOpened
		{
			add
			{
				this.backing_SettingsOpened = (NSettingsScreen.SettingsOpenedEventHandler)Delegate.Combine(this.backing_SettingsOpened, value);
			}
			remove
			{
				this.backing_SettingsOpened = (NSettingsScreen.SettingsOpenedEventHandler)Delegate.Remove(this.backing_SettingsOpened, value);
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000FAB0E File Offset: 0x000F8D0E
		protected void EmitSignalSettingsOpened()
		{
			base.EmitSignal(NSettingsScreen.SignalName.SettingsOpened, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x060024F9 RID: 9465 RVA: 0x000FAB24 File Offset: 0x000F8D24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NSettingsScreen.SignalName.SettingsClosed && args.Count == 0)
			{
				NSettingsScreen.SettingsClosedEventHandler settingsClosedEventHandler = this.backing_SettingsClosed;
				if (settingsClosedEventHandler == null)
				{
					return;
				}
				settingsClosedEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NSettingsScreen.SignalName.SettingsOpened) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NSettingsScreen.SettingsOpenedEventHandler settingsOpenedEventHandler = this.backing_SettingsOpened;
				if (settingsOpenedEventHandler == null)
				{
					return;
				}
				settingsOpenedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060024FA RID: 9466 RVA: 0x000FAB87 File Offset: 0x000F8D87
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NSettingsScreen.SignalName.SettingsClosed || (ref signal) == NSettingsScreen.SignalName.SettingsOpened || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000DC9 RID: 3529
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/settings_screen");

		// Token: 0x04000DCA RID: 3530
		[Nullable(1)]
		private NSettingsTabManager _settingsTabManager;

		// Token: 0x04000DCB RID: 3531
		[Nullable(1)]
		private NOpenFeedbackScreenButton _feedbackScreenButton;

		// Token: 0x04000DCC RID: 3532
		[Nullable(1)]
		private NOpenModdingScreenButton _moddingScreenButton;

		// Token: 0x04000DCD RID: 3533
		[Nullable(1)]
		private NSettingsToast _toast;

		// Token: 0x04000DCE RID: 3534
		private bool _isInRun;

		// Token: 0x04000DCF RID: 3535
		public static readonly Vector2 settingTipsOffset = new Vector2(1012f, -60f);

		// Token: 0x04000DD0 RID: 3536
		private NSettingsScreen.SettingsClosedEventHandler backing_SettingsClosed;

		// Token: 0x04000DD1 RID: 3537
		private NSettingsScreen.SettingsOpenedEventHandler backing_SettingsOpened;

		// Token: 0x020013E3 RID: 5091
		// (Invoke) Token: 0x06009FB3 RID: 40883
		[Signal]
		public delegate void SettingsClosedEventHandler();

		// Token: 0x020013E4 RID: 5092
		// (Invoke) Token: 0x06009FB7 RID: 40887
		[Signal]
		public delegate void SettingsOpenedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E5 RID: 5093
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetIsInRun' method.
			/// </summary>
			// Token: 0x04004BF0 RID: 19440
			public static readonly StringName SetIsInRun = "SetIsInRun";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BF1 RID: 19441
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSettingsTabChanged' method.
			/// </summary>
			// Token: 0x04004BF2 RID: 19442
			public static readonly StringName OnSettingsTabChanged = "OnSettingsTabChanged";

			/// <summary>
			/// Cached name for the 'LocalizeLabels' method.
			/// </summary>
			// Token: 0x04004BF3 RID: 19443
			public static readonly StringName LocalizeLabels = "LocalizeLabels";

			/// <summary>
			/// Cached name for the 'OpenModdingScreen' method.
			/// </summary>
			// Token: 0x04004BF4 RID: 19444
			public static readonly StringName OpenModdingScreen = "OpenModdingScreen";

			/// <summary>
			/// Cached name for the 'OpenFeedbackScreen' method.
			/// </summary>
			// Token: 0x04004BF5 RID: 19445
			public static readonly StringName OpenFeedbackScreen = "OpenFeedbackScreen";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004BF6 RID: 19446
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004BF7 RID: 19447
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnSubmenuHidden' method.
			/// </summary>
			// Token: 0x04004BF8 RID: 19448
			public new static readonly StringName OnSubmenuHidden = "OnSubmenuHidden";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04004BF9 RID: 19449
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E6 RID: 5094
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004BFA RID: 19450
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_settingsTabManager' field.
			/// </summary>
			// Token: 0x04004BFB RID: 19451
			public static readonly StringName _settingsTabManager = "_settingsTabManager";

			/// <summary>
			/// Cached name for the '_feedbackScreenButton' field.
			/// </summary>
			// Token: 0x04004BFC RID: 19452
			public static readonly StringName _feedbackScreenButton = "_feedbackScreenButton";

			/// <summary>
			/// Cached name for the '_moddingScreenButton' field.
			/// </summary>
			// Token: 0x04004BFD RID: 19453
			public static readonly StringName _moddingScreenButton = "_moddingScreenButton";

			/// <summary>
			/// Cached name for the '_toast' field.
			/// </summary>
			// Token: 0x04004BFE RID: 19454
			public static readonly StringName _toast = "_toast";

			/// <summary>
			/// Cached name for the '_isInRun' field.
			/// </summary>
			// Token: 0x04004BFF RID: 19455
			public static readonly StringName _isInRun = "_isInRun";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E7 RID: 5095
		public new class SignalName : NSubmenu.SignalName
		{
			/// <summary>
			/// Cached name for the 'SettingsClosed' signal.
			/// </summary>
			// Token: 0x04004C00 RID: 19456
			public static readonly StringName SettingsClosed = "SettingsClosed";

			/// <summary>
			/// Cached name for the 'SettingsOpened' signal.
			/// </summary>
			// Token: 0x04004C01 RID: 19457
			public static readonly StringName SettingsOpened = "SettingsOpened";
		}
	}
}
