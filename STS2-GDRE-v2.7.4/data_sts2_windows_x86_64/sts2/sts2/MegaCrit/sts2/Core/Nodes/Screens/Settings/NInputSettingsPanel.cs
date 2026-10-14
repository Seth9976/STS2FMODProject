using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000299 RID: 665
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NInputSettingsPanel.cs")]
	public class NInputSettingsPanel : NSettingsPanel
	{
		// Token: 0x0600234E RID: 9038 RVA: 0x000F03D8 File Offset: 0x000EE5D8
		public override void _Ready()
		{
			base._Ready();
			this._resetToDefaultButton = base.GetNode<NButton>("%ResetToDefaultButton");
			this._commandHeader = base.GetNode<MegaRichTextLabel>("%CommandHeader");
			this._keyboardHeader = base.GetNode<MegaRichTextLabel>("%KeyboardHeader");
			this._controllerHeader = base.GetNode<MegaRichTextLabel>("%ControllerHeader");
			this._steamInputPrompt = base.GetNode<MegaRichTextLabel>("%SteamInputPrompt");
			this._resetToDefaultButton.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
			{
				NInputManager.Instance.ResetToDefaults();
			}), 0U);
			base.GetViewport().Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnViewportSizeChange)), 0U);
			this._commandHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.COMMAND_HEADER").GetFormattedText();
			this._keyboardHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.KEYBOARD_HEADER").GetFormattedText();
			this._controllerHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.CONTROLLER_HEADER").GetFormattedText();
			this._steamInputPrompt.Text = new LocString("settings_ui", "INPUT_SETTINGS.STEAM_INPUT_DETECTED").GetFormattedText();
			IReadOnlyList<StringName> readOnlyList = NInputManager.remappableControllerInputs.Concat<StringName>(NInputManager.remappableKeyboardInputs).Distinct<StringName>().ToList<StringName>();
			foreach (StringName stringName in readOnlyList)
			{
				NInputSettingsEntry entry = NInputSettingsEntry.Create(stringName);
				entry.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
				{
					this.SetAsListeningEntry(entry);
				}), 0U);
				base.Content.AddChildSafely(entry);
			}
			this.UpdateNavigation();
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000F05D4 File Offset: 0x000EE7D4
		private async Task RefreshSize()
		{
			await this.AwaitProcessFrame(default(CancellationToken));
			await this.AwaitProcessFrame(default(CancellationToken));
			Vector2 size = base.GetParent<Control>().Size;
			Vector2 minimumSize = base.Content.GetMinimumSize();
			if (minimumSize.Y + this._minPadding >= size.Y)
			{
				base.Size = new Vector2(base.Content.Size.X, minimumSize.Y + size.Y * 0.4f);
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000F0617 File Offset: 0x000EE817
		private void OnViewportSizeChange()
		{
			TaskHelper.RunSafely(this.RefreshSize());
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000F0625 File Offset: 0x000EE825
		protected override void OnVisibilityChange()
		{
			base.OnVisibilityChange();
			this._listeningEntry = null;
			this._steamInputPrompt.Visible = !NControllerManager.Instance.ShouldAllowControllerRebinding;
			TaskHelper.RunSafely(this.RefreshSize());
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000F0658 File Offset: 0x000EE858
		private void SetAsListeningEntry(NInputSettingsEntry entry)
		{
			this._listeningEntry = entry;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000F0664 File Offset: 0x000EE864
		public override void _UnhandledKeyInput(InputEvent inputEvent)
		{
			if (this._listeningEntry == null)
			{
				return;
			}
			if (!NInputManager.remappableKeyboardInputs.Contains(this._listeningEntry.InputName))
			{
				return;
			}
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey == null)
			{
				return;
			}
			NInputManager.Instance.ModifyShortcutKey(this._listeningEntry.InputName, inputEventKey.Keycode);
			Viewport viewport = base.GetViewport();
			if (viewport != null)
			{
				viewport.SetInputAsHandled();
			}
			this._listeningEntry = null;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000F06D0 File Offset: 0x000EE8D0
		public override void _Input(InputEvent inputEvent)
		{
			if (this._listeningEntry == null)
			{
				return;
			}
			foreach (StringName stringName in Controller.AllControllerInputs)
			{
				if (inputEvent.IsActionReleased(stringName, false))
				{
					if (NInputManager.remappableControllerInputs.Contains(this._listeningEntry.InputName) && NControllerManager.Instance.ShouldAllowControllerRebinding)
					{
						NInputManager.Instance.ModifyControllerButton(this._listeningEntry.InputName, stringName);
					}
					Viewport viewport = base.GetViewport();
					if (viewport != null)
					{
						viewport.SetInputAsHandled();
					}
					this._listeningEntry = null;
					return;
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002355 RID: 9045 RVA: 0x000F075C File Offset: 0x000EE95C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NInputSettingsPanel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsPanel.MethodName.OnViewportSizeChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsPanel.MethodName.OnVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NInputSettingsPanel.MethodName.SetAsListeningEntry, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NInputSettingsPanel.MethodName._UnhandledKeyInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NInputSettingsPanel.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002356 RID: 9046 RVA: 0x000F091C File Offset: 0x000EEB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NInputSettingsPanel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsPanel.MethodName.OnViewportSizeChange && args.Count == 0)
			{
				this.OnViewportSizeChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsPanel.MethodName.OnVisibilityChange && args.Count == 0)
			{
				this.OnVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsPanel.MethodName.SetAsListeningEntry && args.Count == 1)
			{
				this.SetAsListeningEntry(VariantUtils.ConvertTo<NInputSettingsEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsPanel.MethodName._UnhandledKeyInput && args.Count == 1)
			{
				this._UnhandledKeyInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NInputSettingsPanel.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002357 RID: 9047 RVA: 0x000F0A3C File Offset: 0x000EEC3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NInputSettingsPanel.MethodName._Ready || (ref method) == NInputSettingsPanel.MethodName.OnViewportSizeChange || (ref method) == NInputSettingsPanel.MethodName.OnVisibilityChange || (ref method) == NInputSettingsPanel.MethodName.SetAsListeningEntry || (ref method) == NInputSettingsPanel.MethodName._UnhandledKeyInput || (ref method) == NInputSettingsPanel.MethodName._Input || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002358 RID: 9048 RVA: 0x000F0AAC File Offset: 0x000EECAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NInputSettingsPanel.PropertyName._minPadding)
			{
				this._minPadding = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._listeningEntry)
			{
				this._listeningEntry = VariantUtils.ConvertTo<NInputSettingsEntry>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._resetToDefaultButton)
			{
				this._resetToDefaultButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._commandHeader)
			{
				this._commandHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._keyboardHeader)
			{
				this._keyboardHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._controllerHeader)
			{
				this._controllerHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._steamInputPrompt)
			{
				this._steamInputPrompt = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002359 RID: 9049 RVA: 0x000F0B80 File Offset: 0x000EED80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NInputSettingsPanel.PropertyName._minPadding)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minPadding);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._listeningEntry)
			{
				value = VariantUtils.CreateFrom<NInputSettingsEntry>(ref this._listeningEntry);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._resetToDefaultButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._resetToDefaultButton);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._commandHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._commandHeader);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._keyboardHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._keyboardHeader);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._controllerHeader)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._controllerHeader);
				return true;
			}
			if ((ref name) == NInputSettingsPanel.PropertyName._steamInputPrompt)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._steamInputPrompt);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600235A RID: 9050 RVA: 0x000F0C78 File Offset: 0x000EEE78
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NInputSettingsPanel.PropertyName._minPadding, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._listeningEntry, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._resetToDefaultButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._commandHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._keyboardHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._controllerHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NInputSettingsPanel.PropertyName._steamInputPrompt, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600235B RID: 9051 RVA: 0x000F0D74 File Offset: 0x000EEF74
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NInputSettingsPanel.PropertyName._minPadding, Variant.From<float>(ref this._minPadding));
			info.AddProperty(NInputSettingsPanel.PropertyName._listeningEntry, Variant.From<NInputSettingsEntry>(ref this._listeningEntry));
			info.AddProperty(NInputSettingsPanel.PropertyName._resetToDefaultButton, Variant.From<NButton>(ref this._resetToDefaultButton));
			info.AddProperty(NInputSettingsPanel.PropertyName._commandHeader, Variant.From<MegaRichTextLabel>(ref this._commandHeader));
			info.AddProperty(NInputSettingsPanel.PropertyName._keyboardHeader, Variant.From<MegaRichTextLabel>(ref this._keyboardHeader));
			info.AddProperty(NInputSettingsPanel.PropertyName._controllerHeader, Variant.From<MegaRichTextLabel>(ref this._controllerHeader));
			info.AddProperty(NInputSettingsPanel.PropertyName._steamInputPrompt, Variant.From<MegaRichTextLabel>(ref this._steamInputPrompt));
		}

		/// <inheritdoc />
		// Token: 0x0600235C RID: 9052 RVA: 0x000F0E24 File Offset: 0x000EF024
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._minPadding, ref variant))
			{
				this._minPadding = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._listeningEntry, ref variant2))
			{
				this._listeningEntry = variant2.As<NInputSettingsEntry>();
			}
			Variant variant3;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._resetToDefaultButton, ref variant3))
			{
				this._resetToDefaultButton = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._commandHeader, ref variant4))
			{
				this._commandHeader = variant4.As<MegaRichTextLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._keyboardHeader, ref variant5))
			{
				this._keyboardHeader = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._controllerHeader, ref variant6))
			{
				this._controllerHeader = variant6.As<MegaRichTextLabel>();
			}
			Variant variant7;
			if (info.TryGetProperty(NInputSettingsPanel.PropertyName._steamInputPrompt, ref variant7))
			{
				this._steamInputPrompt = variant7.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04000D80 RID: 3456
		private float _minPadding = 50f;

		// Token: 0x04000D81 RID: 3457
		[Nullable(2)]
		private NInputSettingsEntry _listeningEntry;

		// Token: 0x04000D82 RID: 3458
		private NButton _resetToDefaultButton;

		// Token: 0x04000D83 RID: 3459
		private MegaRichTextLabel _commandHeader;

		// Token: 0x04000D84 RID: 3460
		private MegaRichTextLabel _keyboardHeader;

		// Token: 0x04000D85 RID: 3461
		private MegaRichTextLabel _controllerHeader;

		// Token: 0x04000D86 RID: 3462
		private MegaRichTextLabel _steamInputPrompt;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137E RID: 4990
		[NullableContext(0)]
		public new class MethodName : NSettingsPanel.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B2F RID: 19247
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnViewportSizeChange' method.
			/// </summary>
			// Token: 0x04004B30 RID: 19248
			public static readonly StringName OnViewportSizeChange = "OnViewportSizeChange";

			/// <summary>
			/// Cached name for the 'OnVisibilityChange' method.
			/// </summary>
			// Token: 0x04004B31 RID: 19249
			public new static readonly StringName OnVisibilityChange = "OnVisibilityChange";

			/// <summary>
			/// Cached name for the 'SetAsListeningEntry' method.
			/// </summary>
			// Token: 0x04004B32 RID: 19250
			public static readonly StringName SetAsListeningEntry = "SetAsListeningEntry";

			/// <summary>
			/// Cached name for the '_UnhandledKeyInput' method.
			/// </summary>
			// Token: 0x04004B33 RID: 19251
			public static readonly StringName _UnhandledKeyInput = "_UnhandledKeyInput";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004B34 RID: 19252
			public static readonly StringName _Input = "_Input";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200137F RID: 4991
		[NullableContext(0)]
		public new class PropertyName : NSettingsPanel.PropertyName
		{
			/// <summary>
			/// Cached name for the '_minPadding' field.
			/// </summary>
			// Token: 0x04004B35 RID: 19253
			public new static readonly StringName _minPadding = "_minPadding";

			/// <summary>
			/// Cached name for the '_listeningEntry' field.
			/// </summary>
			// Token: 0x04004B36 RID: 19254
			public static readonly StringName _listeningEntry = "_listeningEntry";

			/// <summary>
			/// Cached name for the '_resetToDefaultButton' field.
			/// </summary>
			// Token: 0x04004B37 RID: 19255
			public static readonly StringName _resetToDefaultButton = "_resetToDefaultButton";

			/// <summary>
			/// Cached name for the '_commandHeader' field.
			/// </summary>
			// Token: 0x04004B38 RID: 19256
			public static readonly StringName _commandHeader = "_commandHeader";

			/// <summary>
			/// Cached name for the '_keyboardHeader' field.
			/// </summary>
			// Token: 0x04004B39 RID: 19257
			public static readonly StringName _keyboardHeader = "_keyboardHeader";

			/// <summary>
			/// Cached name for the '_controllerHeader' field.
			/// </summary>
			// Token: 0x04004B3A RID: 19258
			public static readonly StringName _controllerHeader = "_controllerHeader";

			/// <summary>
			/// Cached name for the '_steamInputPrompt' field.
			/// </summary>
			// Token: 0x04004B3B RID: 19259
			public static readonly StringName _steamInputPrompt = "_steamInputPrompt";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001380 RID: 4992
		[NullableContext(0)]
		public new class SignalName : NSettingsPanel.SignalName
		{
		}
	}
}
