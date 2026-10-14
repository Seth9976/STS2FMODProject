using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen
{
	// Token: 0x020002E8 RID: 744
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModMenuRow.cs")]
	public class NModMenuRow : NClickableControl
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x001118D5 File Offset: 0x0010FAD5
		private string Hotkey
		{
			get
			{
				return MegaInput.accept;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x001118E1 File Offset: 0x0010FAE1
		// (set) Token: 0x060027F5 RID: 10229 RVA: 0x001118E9 File Offset: 0x0010FAE9
		[Nullable(2)]
		public Mod Mod
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x001118F4 File Offset: 0x0010FAF4
		[return: Nullable(2)]
		public static NModMenuRow Create(NModdingScreen screen, Mod mod)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NModMenuRow nmodMenuRow = PreloadManager.Cache.GetScene(NModMenuRow._scenePath).Instantiate<NModMenuRow>(0L);
			nmodMenuRow.Mod = mod;
			nmodMenuRow._screen = screen;
			return nmodMenuRow;
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x00111930 File Offset: 0x0010FB30
		public override void _Ready()
		{
			if (this.Mod == null)
			{
				return;
			}
			this._selectionHighlight = base.GetNode<Panel>("SelectionHighlight");
			this._tickbox = base.GetNode<NTickbox>("Tickbox");
			MegaRichTextLabel node = base.GetNode<MegaRichTextLabel>("Title");
			TextureRect node2 = base.GetNode<TextureRect>("PlatformIcon");
			this._controllerIcon = base.GetNode<TextureRect>("ControllerIcon");
			this._tickbox = base.GetNode<NTickbox>("Tickbox");
			CanvasItem selectionHighlight = this._selectionHighlight;
			Color color = this._selectionHighlight.Modulate;
			color.A = 0f;
			selectionHighlight.Modulate = color;
			NTickbox tickbox = this._tickbox;
			ModSettings modSettings = SaveManager.Instance.SettingsSave.ModSettings;
			tickbox.IsTicked = modSettings == null || !modSettings.IsModDisabled(this.Mod);
			this._tickbox.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.OnTickboxToggled)), 0U);
			MegaRichTextLabel megaRichTextLabel = node;
			ModManifest manifest = this.Mod.manifest;
			string text;
			if ((text = ((manifest != null) ? manifest.name : null)) == null)
			{
				ModManifest manifest2 = this.Mod.manifest;
				text = ((manifest2 != null) ? manifest2.id : null) ?? "<null>";
			}
			megaRichTextLabel.Text = text;
			node2.Texture = NModMenuRow.GetPlatformIcon(this.Mod.modSource);
			ModLoadState state = this.Mod.state;
			switch (state)
			{
			case ModLoadState.None:
				color = StsColors.purple;
				break;
			case ModLoadState.Loaded:
				color = Colors.White;
				break;
			case ModLoadState.Failed:
				color = StsColors.red;
				break;
			case ModLoadState.Disabled:
				color = StsColors.gray;
				break;
			case ModLoadState.DisabledDuplicate:
				color = StsColors.gray;
				break;
			case ModLoadState.AddedAtRuntime:
				color = StsColors.gray;
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(state);
				break;
			}
			Color color2 = color;
			node.Modulate = color2;
			node2.Modulate = color2;
			this.ConnectSignals();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			this.UpdateControllerButton();
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x00111B79 File Offset: 0x0010FD79
		public override void _GuiInput(InputEvent inputEvent)
		{
			base._GuiInput(inputEvent);
			if (inputEvent.IsActionPressed(this.Hotkey, false, false) && this._isSelected)
			{
				this._tickbox.ForceToggleTick();
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x00111BAC File Offset: 0x0010FDAC
		protected override void OnFocus()
		{
			if (this._isSelected)
			{
				return;
			}
			CanvasItem selectionHighlight = this._selectionHighlight;
			Color darkBlue = StsColors.darkBlue;
			darkBlue.A = 0.25f;
			selectionHighlight.Modulate = darkBlue;
			this.UpdateControllerButton();
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x00111BE6 File Offset: 0x0010FDE6
		private void UpdateControllerButton()
		{
			this._controllerIcon.SetVisible(this._isSelected && NControllerManager.Instance.IsUsingController);
			this._controllerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.Hotkey);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x00111C23 File Offset: 0x0010FE23
		protected override void OnUnfocus()
		{
			if (this._isSelected)
			{
				return;
			}
			this._selectionHighlight.Modulate = Colors.Transparent;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x00111C3E File Offset: 0x0010FE3E
		protected override void OnRelease()
		{
			this._screen.OnRowSelected(this);
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x00111C4C File Offset: 0x0010FE4C
		public void SetSelected(bool isSelected)
		{
			if (this._isSelected != isSelected)
			{
				this._isSelected = isSelected;
				if (this._isSelected)
				{
					CanvasItem selectionHighlight = this._selectionHighlight;
					Color color = StsColors.blue;
					color.A = 0.25f;
					selectionHighlight.Modulate = color;
				}
				else if (base.IsFocused)
				{
					CanvasItem selectionHighlight2 = this._selectionHighlight;
					Color color = StsColors.darkBlue;
					color.A = 0.25f;
					selectionHighlight2.Modulate = color;
				}
				else
				{
					this._selectionHighlight.Modulate = Colors.Transparent;
				}
			}
			this.UpdateControllerButton();
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x00111CD0 File Offset: 0x0010FED0
		private void OnTickboxToggled(NTickbox tickbox)
		{
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (settingsSave.ModSettings == null)
			{
				settingsSave.ModSettings = new ModSettings();
			}
			foreach (SettingsSaveMod settingsSaveMod in SaveManager.Instance.SettingsSave.ModSettings.ModList)
			{
				string id = settingsSaveMod.Id;
				Mod mod = this.Mod;
				string text;
				if (mod == null)
				{
					text = null;
				}
				else
				{
					ModManifest manifest = mod.manifest;
					text = ((manifest != null) ? manifest.id : null);
				}
				if (id == text && settingsSaveMod.Source == this.Mod.modSource)
				{
					settingsSaveMod.IsEnabled = tickbox.IsTicked;
				}
			}
			this._screen.OnModEnabledOrDisabled();
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00111DA0 File Offset: 0x0010FFA0
		public static Texture2D GetPlatformIcon(ModSource modSource)
		{
			AssetCache cache = PreloadManager.Cache;
			string text;
			if (modSource != ModSource.ModsDirectory)
			{
				if (modSource != ModSource.SteamWorkshop)
				{
					throw new ArgumentOutOfRangeException("modSource", modSource, null);
				}
				text = ImageHelper.GetImagePath("ui/mods/steam_logo.png");
			}
			else
			{
				text = ImageHelper.GetImagePath("ui/mods/folder.png");
			}
			return cache.GetTexture2D(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002800 RID: 10240 RVA: 0x00111DF0 File Offset: 0x0010FFF0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NModMenuRow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuRow.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NModMenuRow.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuRow.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuRow.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuRow.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NModMenuRow.MethodName.SetSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isSelected", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NModMenuRow.MethodName.OnTickboxToggled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NModMenuRow.MethodName.GetPlatformIcon, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "modSource", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002801 RID: 10241 RVA: 0x00112064 File Offset: 0x00110264
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModMenuRow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.SetSelected && args.Count == 1)
			{
				this.SetSelected(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.OnTickboxToggled && args.Count == 1)
			{
				this.OnTickboxToggled(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NModMenuRow.MethodName.GetPlatformIcon && args.Count == 1)
			{
				Texture2D platformIcon = NModMenuRow.GetPlatformIcon(VariantUtils.ConvertTo<ModSource>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref platformIcon);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x00112208 File Offset: 0x00110408
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NModMenuRow.MethodName.GetPlatformIcon && args.Count == 1)
			{
				Texture2D platformIcon = NModMenuRow.GetPlatformIcon(VariantUtils.ConvertTo<ModSource>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref platformIcon);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002803 RID: 10243 RVA: 0x00112258 File Offset: 0x00110458
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NModMenuRow.MethodName._Ready || (ref method) == NModMenuRow.MethodName._GuiInput || (ref method) == NModMenuRow.MethodName.OnFocus || (ref method) == NModMenuRow.MethodName.UpdateControllerButton || (ref method) == NModMenuRow.MethodName.OnUnfocus || (ref method) == NModMenuRow.MethodName.OnRelease || (ref method) == NModMenuRow.MethodName.SetSelected || (ref method) == NModMenuRow.MethodName.OnTickboxToggled || (ref method) == NModMenuRow.MethodName.GetPlatformIcon || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002804 RID: 10244 RVA: 0x001122F4 File Offset: 0x001104F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NModMenuRow.PropertyName._controllerIcon)
			{
				this._controllerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._selectionHighlight)
			{
				this._selectionHighlight = VariantUtils.ConvertTo<Panel>(ref value);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._tickbox)
			{
				this._tickbox = VariantUtils.ConvertTo<NTickbox>(ref value);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._screen)
			{
				this._screen = VariantUtils.ConvertTo<NModdingScreen>(ref value);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._isSelected)
			{
				this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002805 RID: 10245 RVA: 0x00112390 File Offset: 0x00110590
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NModMenuRow.PropertyName.Hotkey)
			{
				string hotkey = this.Hotkey;
				value = VariantUtils.CreateFrom<string>(ref hotkey);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._controllerIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerIcon);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._selectionHighlight)
			{
				value = VariantUtils.CreateFrom<Panel>(ref this._selectionHighlight);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._tickbox)
			{
				value = VariantUtils.CreateFrom<NTickbox>(ref this._tickbox);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._screen)
			{
				value = VariantUtils.CreateFrom<NModdingScreen>(ref this._screen);
				return true;
			}
			if ((ref name) == NModMenuRow.PropertyName._isSelected)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002806 RID: 10246 RVA: 0x00112468 File Offset: 0x00110668
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NModMenuRow.PropertyName.Hotkey, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuRow.PropertyName._controllerIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuRow.PropertyName._selectionHighlight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuRow.PropertyName._tickbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NModMenuRow.PropertyName._screen, 0L, "", 4096L, false),
				new PropertyInfo(1L, NModMenuRow.PropertyName._isSelected, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002807 RID: 10247 RVA: 0x00112540 File Offset: 0x00110740
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NModMenuRow.PropertyName._controllerIcon, Variant.From<TextureRect>(ref this._controllerIcon));
			info.AddProperty(NModMenuRow.PropertyName._selectionHighlight, Variant.From<Panel>(ref this._selectionHighlight));
			info.AddProperty(NModMenuRow.PropertyName._tickbox, Variant.From<NTickbox>(ref this._tickbox));
			info.AddProperty(NModMenuRow.PropertyName._screen, Variant.From<NModdingScreen>(ref this._screen));
			info.AddProperty(NModMenuRow.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
		}

		/// <inheritdoc />
		// Token: 0x06002808 RID: 10248 RVA: 0x001125C4 File Offset: 0x001107C4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NModMenuRow.PropertyName._controllerIcon, ref variant))
			{
				this._controllerIcon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NModMenuRow.PropertyName._selectionHighlight, ref variant2))
			{
				this._selectionHighlight = variant2.As<Panel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NModMenuRow.PropertyName._tickbox, ref variant3))
			{
				this._tickbox = variant3.As<NTickbox>();
			}
			Variant variant4;
			if (info.TryGetProperty(NModMenuRow.PropertyName._screen, ref variant4))
			{
				this._screen = variant4.As<NModdingScreen>();
			}
			Variant variant5;
			if (info.TryGetProperty(NModMenuRow.PropertyName._isSelected, ref variant5))
			{
				this._isSelected = variant5.As<bool>();
			}
		}

		// Token: 0x04000ED5 RID: 3797
		private TextureRect _controllerIcon;

		// Token: 0x04000ED6 RID: 3798
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/modding/modding_screen_row");

		// Token: 0x04000ED7 RID: 3799
		private const float _selectedAlpha = 0.25f;

		// Token: 0x04000ED8 RID: 3800
		private Panel _selectionHighlight;

		// Token: 0x04000ED9 RID: 3801
		private NTickbox _tickbox;

		// Token: 0x04000EDA RID: 3802
		private NModdingScreen _screen;

		// Token: 0x04000EDC RID: 3804
		private bool _isSelected;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148D RID: 5261
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004DF7 RID: 19959
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04004DF8 RID: 19960
			public new static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004DF9 RID: 19961
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x04004DFA RID: 19962
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004DFB RID: 19963
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04004DFC RID: 19964
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'SetSelected' method.
			/// </summary>
			// Token: 0x04004DFD RID: 19965
			public static readonly StringName SetSelected = "SetSelected";

			/// <summary>
			/// Cached name for the 'OnTickboxToggled' method.
			/// </summary>
			// Token: 0x04004DFE RID: 19966
			public static readonly StringName OnTickboxToggled = "OnTickboxToggled";

			/// <summary>
			/// Cached name for the 'GetPlatformIcon' method.
			/// </summary>
			// Token: 0x04004DFF RID: 19967
			public static readonly StringName GetPlatformIcon = "GetPlatformIcon";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148E RID: 5262
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkey' property.
			/// </summary>
			// Token: 0x04004E00 RID: 19968
			public static readonly StringName Hotkey = "Hotkey";

			/// <summary>
			/// Cached name for the '_controllerIcon' field.
			/// </summary>
			// Token: 0x04004E01 RID: 19969
			public static readonly StringName _controllerIcon = "_controllerIcon";

			/// <summary>
			/// Cached name for the '_selectionHighlight' field.
			/// </summary>
			// Token: 0x04004E02 RID: 19970
			public static readonly StringName _selectionHighlight = "_selectionHighlight";

			/// <summary>
			/// Cached name for the '_tickbox' field.
			/// </summary>
			// Token: 0x04004E03 RID: 19971
			public static readonly StringName _tickbox = "_tickbox";

			/// <summary>
			/// Cached name for the '_screen' field.
			/// </summary>
			// Token: 0x04004E04 RID: 19972
			public static readonly StringName _screen = "_screen";

			/// <summary>
			/// Cached name for the '_isSelected' field.
			/// </summary>
			// Token: 0x04004E05 RID: 19973
			public static readonly StringName _isSelected = "_isSelected";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200148F RID: 5263
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
