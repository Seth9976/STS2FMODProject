using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// A Base Button class which doesn't rely on Godot's Button class.
	/// </summary>
	// Token: 0x0200039C RID: 924
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NButton.cs")]
	public class NButton : NClickableControl
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x001A272C File Offset: 0x001A092C
		[Nullable(2)]
		protected virtual string ClickedSfx
		{
			[NullableContext(2)]
			get
			{
				return "event:/sfx/ui/clicks/ui_click";
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x001A2733 File Offset: 0x001A0933
		[Nullable(2)]
		protected virtual string HoveredSfx
		{
			[NullableContext(2)]
			get
			{
				return "event:/sfx/ui/clicks/ui_hover";
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x001A273A File Offset: 0x001A093A
		[Nullable(1)]
		protected virtual string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return Array.Empty<string>();
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x001A2741 File Offset: 0x001A0941
		[Nullable(2)]
		protected virtual string ControllerIconHotkey
		{
			[NullableContext(2)]
			get
			{
				if (this.Hotkeys.Length == 0)
				{
					return null;
				}
				return this.Hotkeys[0];
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x001A2756 File Offset: 0x001A0956
		private bool HasControllerHotkey
		{
			get
			{
				return this.Hotkeys.Length != 0;
			}
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x001A2764 File Offset: 0x001A0964
		public override void _Ready()
		{
			if (base.GetType() != typeof(NButton))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x001A27BC File Offset: 0x001A09BC
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			if (this.HasControllerHotkey)
			{
				this.RegisterHotkeys();
			}
			this.GetControllerIconNode();
			this.UpdateControllerButton();
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x001A27DE File Offset: 0x001A09DE
		protected virtual void GetControllerIconNode()
		{
			this._controllerHotkeyIcon = base.GetNodeOrNull<TextureRect>("%ControllerIcon");
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x001A27F8 File Offset: 0x001A09F8
		public override void _EnterTree()
		{
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
				NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			}
			if (NInputManager.Instance != null)
			{
				NInputManager.Instance.Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
			}
		}

		/// <summary>
		/// Detects input actions
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003770 RID: 14192 RVA: 0x001A2879 File Offset: 0x001A0A79
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			base.CheckMouseDragThreshold(inputEvent);
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x001A2882 File Offset: 0x001A0A82
		protected override void OnPress()
		{
			if (this.ClickedSfx != null)
			{
				SfxCmd.Play(this.ClickedSfx, 1f);
			}
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x001A289C File Offset: 0x001A0A9C
		protected override void OnFocus()
		{
			if (this.HoveredSfx != null)
			{
				SfxCmd.Play(this.HoveredSfx, 1f);
			}
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x001A28B6 File Offset: 0x001A0AB6
		protected override void OnEnable()
		{
			this.RegisterHotkeys();
			this.UpdateControllerButton();
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x001A28C4 File Offset: 0x001A0AC4
		protected override void OnDisable()
		{
			this.UnregisterHotkeys();
			this.UpdateControllerButton();
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x001A28D4 File Offset: 0x001A0AD4
		protected void UpdateControllerButton()
		{
			if (this._controllerHotkeyIcon == null)
			{
				return;
			}
			NControllerManager instance = NControllerManager.Instance;
			if (instance == null)
			{
				return;
			}
			this._controllerHotkeyIcon.Visible = instance.IsUsingController && this._isEnabled;
			if (this.ControllerIconHotkey != null)
			{
				Texture2D hotkeyIcon = NInputManager.Instance.GetHotkeyIcon(this.ControllerIconHotkey);
				if (hotkeyIcon != null)
				{
					this._controllerHotkeyIcon.Texture = hotkeyIcon;
				}
			}
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x001A2938 File Offset: 0x001A0B38
		protected void RegisterHotkeys()
		{
			if (this.HasControllerHotkey && this._isEnabled)
			{
				foreach (string text in this.Hotkeys)
				{
					NHotkeyManager.Instance.PushHotkeyPressedBinding(text, new Action(base.OnPressHandler));
					NHotkeyManager.Instance.PushHotkeyReleasedBinding(text, new Action(base.OnReleaseHandler));
				}
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x001A299C File Offset: 0x001A0B9C
		protected void UnregisterHotkeys()
		{
			if (this.HasControllerHotkey)
			{
				foreach (string text in this.Hotkeys)
				{
					NHotkeyManager.Instance.RemoveHotkeyPressedBinding(text, new Action(base.OnPressHandler));
					NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(text, new Action(base.OnReleaseHandler));
				}
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x001A29F8 File Offset: 0x001A0BF8
		public override void _ExitTree()
		{
			if (NControllerManager.Instance != null)
			{
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)));
				NControllerManager.Instance.Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)));
			}
			if (NInputManager.Instance != null)
			{
				NInputManager.Instance.Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)));
			}
			this.UnregisterHotkeys();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003779 RID: 14201 RVA: 0x001A2A7C File Offset: 0x001A0C7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(13)
			{
				new MethodInfo(NButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.GetControllerIconNode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.UpdateControllerButton, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.RegisterHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName.UnregisterHotkeys, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600377A RID: 14202 RVA: 0x001A2D28 File Offset: 0x001A0F28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.GetControllerIconNode && args.Count == 0)
			{
				this.GetControllerIconNode();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.UpdateControllerButton && args.Count == 0)
			{
				this.UpdateControllerButton();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.RegisterHotkeys && args.Count == 0)
			{
				this.RegisterHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName.UnregisterHotkeys && args.Count == 0)
			{
				this.UnregisterHotkeys();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600377B RID: 14203 RVA: 0x001A2F30 File Offset: 0x001A1130
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NButton.MethodName._Ready || (ref method) == NButton.MethodName.ConnectSignals || (ref method) == NButton.MethodName.GetControllerIconNode || (ref method) == NButton.MethodName._EnterTree || (ref method) == NButton.MethodName._Input || (ref method) == NButton.MethodName.OnPress || (ref method) == NButton.MethodName.OnFocus || (ref method) == NButton.MethodName.OnEnable || (ref method) == NButton.MethodName.OnDisable || (ref method) == NButton.MethodName.UpdateControllerButton || (ref method) == NButton.MethodName.RegisterHotkeys || (ref method) == NButton.MethodName.UnregisterHotkeys || (ref method) == NButton.MethodName._ExitTree || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600377C RID: 14204 RVA: 0x001A3007 File Offset: 0x001A1207
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NButton.PropertyName._controllerHotkeyIcon)
			{
				this._controllerHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600377D RID: 14205 RVA: 0x001A302C File Offset: 0x001A122C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NButton.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NButton.PropertyName.HoveredSfx)
			{
				string text = this.HoveredSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NButton.PropertyName.ControllerIconHotkey)
			{
				string text = this.ControllerIconHotkey;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NButton.PropertyName.HasControllerHotkey)
			{
				bool hasControllerHotkey = this.HasControllerHotkey;
				value = VariantUtils.CreateFrom<bool>(ref hasControllerHotkey);
				return true;
			}
			if ((ref name) == NButton.PropertyName._controllerHotkeyIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerHotkeyIcon);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600377E RID: 14206 RVA: 0x001A3110 File Offset: 0x001A1310
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NButton.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(4L, NButton.PropertyName.HoveredSfx, 0L, "", 4096L, false),
				new PropertyInfo(34L, NButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(4L, NButton.PropertyName.ControllerIconHotkey, 0L, "", 4096L, false),
				new PropertyInfo(1L, NButton.PropertyName.HasControllerHotkey, 0L, "", 4096L, false),
				new PropertyInfo(24L, NButton.PropertyName._controllerHotkeyIcon, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600377F RID: 14207 RVA: 0x001A31E6 File Offset: 0x001A13E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NButton.PropertyName._controllerHotkeyIcon, Variant.From<TextureRect>(ref this._controllerHotkeyIcon));
		}

		/// <inheritdoc />
		// Token: 0x06003780 RID: 14208 RVA: 0x001A3208 File Offset: 0x001A1408
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NButton.PropertyName._controllerHotkeyIcon, ref variant))
			{
				this._controllerHotkeyIcon = variant.As<TextureRect>();
			}
		}

		// Token: 0x040015A0 RID: 5536
		[Nullable(2)]
		protected TextureRect _controllerHotkeyIcon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001762 RID: 5986
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B30 RID: 23344
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04005B31 RID: 23345
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'GetControllerIconNode' method.
			/// </summary>
			// Token: 0x04005B32 RID: 23346
			public static readonly StringName GetControllerIconNode = "GetControllerIconNode";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005B33 RID: 23347
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005B34 RID: 23348
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005B35 RID: 23349
			public new static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005B36 RID: 23350
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005B37 RID: 23351
			public new static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005B38 RID: 23352
			public new static readonly StringName OnDisable = "OnDisable";

			/// <summary>
			/// Cached name for the 'UpdateControllerButton' method.
			/// </summary>
			// Token: 0x04005B39 RID: 23353
			public static readonly StringName UpdateControllerButton = "UpdateControllerButton";

			/// <summary>
			/// Cached name for the 'RegisterHotkeys' method.
			/// </summary>
			// Token: 0x04005B3A RID: 23354
			public static readonly StringName RegisterHotkeys = "RegisterHotkeys";

			/// <summary>
			/// Cached name for the 'UnregisterHotkeys' method.
			/// </summary>
			// Token: 0x04005B3B RID: 23355
			public static readonly StringName UnregisterHotkeys = "UnregisterHotkeys";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005B3C RID: 23356
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001763 RID: 5987
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04005B3D RID: 23357
			public static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the 'HoveredSfx' property.
			/// </summary>
			// Token: 0x04005B3E RID: 23358
			public static readonly StringName HoveredSfx = "HoveredSfx";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04005B3F RID: 23359
			public static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'ControllerIconHotkey' property.
			/// </summary>
			// Token: 0x04005B40 RID: 23360
			public static readonly StringName ControllerIconHotkey = "ControllerIconHotkey";

			/// <summary>
			/// Cached name for the 'HasControllerHotkey' property.
			/// </summary>
			// Token: 0x04005B41 RID: 23361
			public static readonly StringName HasControllerHotkey = "HasControllerHotkey";

			/// <summary>
			/// Cached name for the '_controllerHotkeyIcon' field.
			/// </summary>
			// Token: 0x04005B42 RID: 23362
			public static readonly StringName _controllerHotkeyIcon = "_controllerHotkeyIcon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001764 RID: 5988
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}
