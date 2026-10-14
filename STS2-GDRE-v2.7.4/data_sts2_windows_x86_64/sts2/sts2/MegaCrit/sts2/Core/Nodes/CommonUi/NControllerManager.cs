using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003D7 RID: 983
	[ScriptPath("res://src/Core/Nodes/CommonUi/NControllerManager.cs")]
	public class NControllerManager : Node
	{
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x001C765F File Offset: 0x001C585F
		[Nullable(2)]
		public static NControllerManager Instance
		{
			[NullableContext(2)]
			get
			{
				if (NGame.Instance == null)
				{
					return null;
				}
				return NGame.Instance.InputManager.ControllerManager;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x001C7679 File Offset: 0x001C5879
		public bool ShouldAllowControllerRebinding
		{
			get
			{
				IControllerInputStrategy inputStrategy = this._inputStrategy;
				return inputStrategy == null || inputStrategy.ShouldAllowControllerRebinding;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x001C768C File Offset: 0x001C588C
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x001C7694 File Offset: 0x001C5894
		public bool IsUsingController { get; private set; }

		// Token: 0x06003B88 RID: 15240 RVA: 0x001C76A0 File Offset: 0x001C58A0
		[NullableContext(1)]
		public async Task Init()
		{
			ActiveScreenContext.Instance.Updated += this.OnScreenContextChanged;
			this._label = base.GetNode<MegaLabel>("Label");
			this._label.Modulate = Colors.Transparent;
			this._inputStrategy = new SteamControllerInputStrategy();
			await this._inputStrategy.Init();
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x001C76E3 File Offset: 0x001C58E3
		public override void _ExitTree()
		{
			ActiveScreenContext.Instance.Updated -= this.OnScreenContextChanged;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x001C76FB File Offset: 0x001C58FB
		public override void _Process(double delta)
		{
			if (this._skipMouseCheckFrames > 0)
			{
				this._skipMouseCheckFrames--;
			}
			if (!NGame.IsGameFocusedWindow())
			{
				return;
			}
			IControllerInputStrategy inputStrategy = this._inputStrategy;
			if (inputStrategy == null)
			{
				return;
			}
			inputStrategy.ProcessInput();
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x001C772C File Offset: 0x001C592C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			if (this.IsUsingController)
			{
				this.CheckForMouseInput(inputEvent);
				return;
			}
			this.CheckForControllerInput(inputEvent);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x001C7745 File Offset: 0x001C5945
		public void OnControllerTypeChanged()
		{
			this.EmitSignalControllerTypeChanged();
		}

		/// <summary>
		/// Checks if the input event is from a mouse and notifies the ui that we are now using mouse input
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003B8D RID: 15245 RVA: 0x001C7750 File Offset: 0x001C5950
		[NullableContext(1)]
		private void CheckForMouseInput(InputEvent inputEvent)
		{
			bool flag = inputEvent is InputEventMouseButton;
			InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
			bool flag2 = inputEventMouseMotion != null && inputEventMouseMotion.Velocity.LengthSquared() > 100f && this._skipMouseCheckFrames <= 0 && inputEventMouseMotion.Relative.LengthSquared() <= 250000f;
			Viewport viewport = base.GetViewport();
			if (flag || flag2)
			{
				this.IsUsingController = false;
				Input.WarpMouse(this._lastMousePosition);
				if (viewport != null)
				{
					viewport.GuiReleaseFocus();
				}
				base.EmitSignal(NControllerManager.SignalName.MouseDetected, Array.Empty<Variant>());
				this.ControlModeChanged();
			}
		}

		/// <summary>
		/// Checks if the input event is from a controller and notifies the ui that we are now using controller input
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003B8E RID: 15246 RVA: 0x001C77EC File Offset: 0x001C59EC
		[NullableContext(1)]
		private void CheckForControllerInput(InputEvent inputEvent)
		{
			if (!NGame.IsGameFocusedWindow())
			{
				return;
			}
			bool flag = Controller.AllControllerInputs.Any<StringName>((StringName i) => inputEvent.IsActionPressed(i, false, false));
			if (flag)
			{
				this.IsUsingController = true;
				Viewport viewport = base.GetViewport();
				if (viewport != null)
				{
					Vector2I vector2I = DisplayServer.MouseGetPosition();
					Vector2I vector2I2 = DisplayServer.WindowGetPosition(0);
					this._lastMousePosition = new Vector2((float)(vector2I.X - vector2I2.X), (float)(vector2I.Y - vector2I2.Y));
					viewport.WarpMouse(NControllerManager._offscreenPos);
					this._skipMouseCheckFrames = 2;
				}
				ActiveScreenContext.Instance.FocusOnDefaultControl();
				base.EmitSignal(NControllerManager.SignalName.ControllerDetected, Array.Empty<Variant>());
				this.ControlModeChanged();
				if (viewport != null)
				{
					viewport.SetInputAsHandled();
				}
			}
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x001C78B0 File Offset: 0x001C5AB0
		private void ControlModeChanged()
		{
			Tween notifyTween = this._notifyTween;
			if (notifyTween != null)
			{
				notifyTween.Kill();
			}
			this._notifyTween = base.CreateTween();
			this._notifyTween.TweenProperty(this._label, "modulate", Colors.White, 0.25);
			this._notifyTween.TweenInterval(0.5);
			this._notifyTween.TweenProperty(this._label, "modulate", Colors.Transparent, 0.75);
			if (this.IsUsingController)
			{
				this._label.SetTextAutoSize(new LocString("main_menu_ui", "CONTROLLER_DETECTED").GetFormattedText());
				Log.Info("CONTROLLER DETECTED: " + ((this._inputStrategy != null) ? this._inputStrategy.GetControllerName() : "NONE"), 2);
				return;
			}
			this._label.SetTextAutoSize(new LocString("main_menu_ui", "MOUSE_DETECTED").GetFormattedText());
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x001C79C0 File Offset: 0x001C5BC0
		private void OnScreenContextChanged()
		{
			if (this.IsUsingController)
			{
				Callable.From(delegate
				{
					ActiveScreenContext.Instance.FocusOnDefaultControl();
				}).CallDeferred(Array.Empty<Variant>());
				return;
			}
			Vector2 mousePosition = base.GetViewport().GetMousePosition();
			using (InputEventMouseMotion inputEventMouseMotion = new InputEventMouseMotion())
			{
				inputEventMouseMotion.Position = mousePosition;
				inputEventMouseMotion.GlobalPosition = mousePosition;
				Input.ParseInputEvent(inputEventMouseMotion);
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x001C7A4C File Offset: 0x001C5C4C
		[NullableContext(1)]
		[return: Nullable(2)]
		public Texture2D GetHotkeyIcon(string hotkey)
		{
			IControllerInputStrategy inputStrategy = this._inputStrategy;
			if (inputStrategy == null)
			{
				return null;
			}
			return inputStrategy.GetHotkeyIcon(hotkey);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x001C7A60 File Offset: 0x001C5C60
		public Vector2 GetLeftAnalogStickDirection()
		{
			IControllerInputStrategy inputStrategy = this._inputStrategy;
			if (inputStrategy == null)
			{
				return Vector2.Zero;
			}
			return inputStrategy.GetLeftAnalogStickDirection();
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x001C7A77 File Offset: 0x001C5C77
		[Nullable(1)]
		public Dictionary<StringName, StringName> GetDefaultControllerInputMap
		{
			[NullableContext(1)]
			get
			{
				if (this._inputStrategy == null)
				{
					return new SteamControllerConfig().DefaultControllerInputMap;
				}
				return this._inputStrategy.GetDefaultControllerInputMap;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003B94 RID: 15252 RVA: 0x001C7A97 File Offset: 0x001C5C97
		public ControllerMappingType ControllerMappingType
		{
			get
			{
				if (this._inputStrategy == null)
				{
					return ControllerMappingType.Default;
				}
				return this._inputStrategy.ControllerConfig.ControllerMappingType;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B95 RID: 15253 RVA: 0x001C7AB4 File Offset: 0x001C5CB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(10)
			{
				new MethodInfo(NControllerManager.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NControllerManager.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NControllerManager.MethodName.OnControllerTypeChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.MethodName.CheckForMouseInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NControllerManager.MethodName.CheckForControllerInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NControllerManager.MethodName.ControlModeChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.MethodName.OnScreenContextChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.MethodName.GetHotkeyIcon, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "hotkey", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NControllerManager.MethodName.GetLeftAnalogStickDirection, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B96 RID: 15254 RVA: 0x001C7D88 File Offset: 0x001C5F88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NControllerManager.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.OnControllerTypeChanged && args.Count == 0)
			{
				this.OnControllerTypeChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.CheckForMouseInput && args.Count == 1)
			{
				this.CheckForMouseInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.CheckForControllerInput && args.Count == 1)
			{
				this.CheckForControllerInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.ControlModeChanged && args.Count == 0)
			{
				this.ControlModeChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.OnScreenContextChanged && args.Count == 0)
			{
				this.OnScreenContextChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.GetHotkeyIcon && args.Count == 1)
			{
				Texture2D hotkeyIcon = this.GetHotkeyIcon(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref hotkeyIcon);
				return true;
			}
			if ((ref method) == NControllerManager.MethodName.GetLeftAnalogStickDirection && args.Count == 0)
			{
				Vector2 leftAnalogStickDirection = this.GetLeftAnalogStickDirection();
				ret = VariantUtils.CreateFrom<Vector2>(ref leftAnalogStickDirection);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B97 RID: 15255 RVA: 0x001C7F64 File Offset: 0x001C6164
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NControllerManager.MethodName._ExitTree || (ref method) == NControllerManager.MethodName._Process || (ref method) == NControllerManager.MethodName._Input || (ref method) == NControllerManager.MethodName.OnControllerTypeChanged || (ref method) == NControllerManager.MethodName.CheckForMouseInput || (ref method) == NControllerManager.MethodName.CheckForControllerInput || (ref method) == NControllerManager.MethodName.ControlModeChanged || (ref method) == NControllerManager.MethodName.OnScreenContextChanged || (ref method) == NControllerManager.MethodName.GetHotkeyIcon || (ref method) == NControllerManager.MethodName.GetLeftAnalogStickDirection || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003B98 RID: 15256 RVA: 0x001C8010 File Offset: 0x001C6210
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NControllerManager.PropertyName.IsUsingController)
			{
				this.IsUsingController = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._lastMousePosition)
			{
				this._lastMousePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._skipMouseCheckFrames)
			{
				this._skipMouseCheckFrames = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._notifyTween)
			{
				this._notifyTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003B99 RID: 15257 RVA: 0x001C80AC File Offset: 0x001C62AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NControllerManager.PropertyName.ShouldAllowControllerRebinding)
			{
				bool flag = this.ShouldAllowControllerRebinding;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName.IsUsingController)
			{
				bool flag = this.IsUsingController;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName.ControllerMappingType)
			{
				ControllerMappingType controllerMappingType = this.ControllerMappingType;
				value = VariantUtils.CreateFrom<ControllerMappingType>(ref controllerMappingType);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._lastMousePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._lastMousePosition);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._skipMouseCheckFrames)
			{
				value = VariantUtils.CreateFrom<int>(ref this._skipMouseCheckFrames);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NControllerManager.PropertyName._notifyTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._notifyTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B9A RID: 15258 RVA: 0x001C81AC File Offset: 0x001C63AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NControllerManager.PropertyName.ShouldAllowControllerRebinding, 0L, "", 4096L, false),
				new PropertyInfo(5L, NControllerManager.PropertyName._lastMousePosition, 0L, "", 4096L, false),
				new PropertyInfo(2L, NControllerManager.PropertyName._skipMouseCheckFrames, 0L, "", 4096L, false),
				new PropertyInfo(24L, NControllerManager.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NControllerManager.PropertyName._notifyTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NControllerManager.PropertyName.IsUsingController, 0L, "", 4096L, false),
				new PropertyInfo(2L, NControllerManager.PropertyName.ControllerMappingType, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B9B RID: 15259 RVA: 0x001C82A4 File Offset: 0x001C64A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isUsingController = NControllerManager.PropertyName.IsUsingController;
			bool isUsingController2 = this.IsUsingController;
			info.AddProperty(isUsingController, Variant.From<bool>(ref isUsingController2));
			info.AddProperty(NControllerManager.PropertyName._lastMousePosition, Variant.From<Vector2>(ref this._lastMousePosition));
			info.AddProperty(NControllerManager.PropertyName._skipMouseCheckFrames, Variant.From<int>(ref this._skipMouseCheckFrames));
			info.AddProperty(NControllerManager.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NControllerManager.PropertyName._notifyTween, Variant.From<Tween>(ref this._notifyTween));
			info.AddSignalEventDelegate(NControllerManager.SignalName.ControllerDetected, this.backing_ControllerDetected);
			info.AddSignalEventDelegate(NControllerManager.SignalName.MouseDetected, this.backing_MouseDetected);
			info.AddSignalEventDelegate(NControllerManager.SignalName.ControllerTypeChanged, this.backing_ControllerTypeChanged);
		}

		/// <inheritdoc />
		// Token: 0x06003B9C RID: 15260 RVA: 0x001C835C File Offset: 0x001C655C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NControllerManager.PropertyName.IsUsingController, ref variant))
			{
				this.IsUsingController = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NControllerManager.PropertyName._lastMousePosition, ref variant2))
			{
				this._lastMousePosition = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NControllerManager.PropertyName._skipMouseCheckFrames, ref variant3))
			{
				this._skipMouseCheckFrames = variant3.As<int>();
			}
			Variant variant4;
			if (info.TryGetProperty(NControllerManager.PropertyName._label, ref variant4))
			{
				this._label = variant4.As<MegaLabel>();
			}
			Variant variant5;
			if (info.TryGetProperty(NControllerManager.PropertyName._notifyTween, ref variant5))
			{
				this._notifyTween = variant5.As<Tween>();
			}
			NControllerManager.ControllerDetectedEventHandler controllerDetectedEventHandler;
			if (info.TryGetSignalEventDelegate<NControllerManager.ControllerDetectedEventHandler>(NControllerManager.SignalName.ControllerDetected, ref controllerDetectedEventHandler))
			{
				this.backing_ControllerDetected = controllerDetectedEventHandler;
			}
			NControllerManager.MouseDetectedEventHandler mouseDetectedEventHandler;
			if (info.TryGetSignalEventDelegate<NControllerManager.MouseDetectedEventHandler>(NControllerManager.SignalName.MouseDetected, ref mouseDetectedEventHandler))
			{
				this.backing_MouseDetected = mouseDetectedEventHandler;
			}
			NControllerManager.ControllerTypeChangedEventHandler controllerTypeChangedEventHandler;
			if (info.TryGetSignalEventDelegate<NControllerManager.ControllerTypeChangedEventHandler>(NControllerManager.SignalName.ControllerTypeChanged, ref controllerTypeChangedEventHandler))
			{
				this.backing_ControllerTypeChanged = controllerTypeChangedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B9D RID: 15261 RVA: 0x001C8444 File Offset: 0x001C6644
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NControllerManager.SignalName.ControllerDetected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.SignalName.MouseDetected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NControllerManager.SignalName.ControllerTypeChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NControllerManager.ControllerDetectedEventHandler" />
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06003B9E RID: 15262 RVA: 0x001C84E6 File Offset: 0x001C66E6
		// (remove) Token: 0x06003B9F RID: 15263 RVA: 0x001C84FF File Offset: 0x001C66FF
		public event NControllerManager.ControllerDetectedEventHandler ControllerDetected
		{
			add
			{
				this.backing_ControllerDetected = (NControllerManager.ControllerDetectedEventHandler)Delegate.Combine(this.backing_ControllerDetected, value);
			}
			remove
			{
				this.backing_ControllerDetected = (NControllerManager.ControllerDetectedEventHandler)Delegate.Remove(this.backing_ControllerDetected, value);
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001C8518 File Offset: 0x001C6718
		protected void EmitSignalControllerDetected()
		{
			base.EmitSignal(NControllerManager.SignalName.ControllerDetected, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NControllerManager.MouseDetectedEventHandler" />
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06003BA1 RID: 15265 RVA: 0x001C852B File Offset: 0x001C672B
		// (remove) Token: 0x06003BA2 RID: 15266 RVA: 0x001C8544 File Offset: 0x001C6744
		public event NControllerManager.MouseDetectedEventHandler MouseDetected
		{
			add
			{
				this.backing_MouseDetected = (NControllerManager.MouseDetectedEventHandler)Delegate.Combine(this.backing_MouseDetected, value);
			}
			remove
			{
				this.backing_MouseDetected = (NControllerManager.MouseDetectedEventHandler)Delegate.Remove(this.backing_MouseDetected, value);
			}
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001C855D File Offset: 0x001C675D
		protected void EmitSignalMouseDetected()
		{
			base.EmitSignal(NControllerManager.SignalName.MouseDetected, Array.Empty<Variant>());
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.CommonUi.NControllerManager.ControllerTypeChangedEventHandler" />
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06003BA4 RID: 15268 RVA: 0x001C8570 File Offset: 0x001C6770
		// (remove) Token: 0x06003BA5 RID: 15269 RVA: 0x001C8589 File Offset: 0x001C6789
		public event NControllerManager.ControllerTypeChangedEventHandler ControllerTypeChanged
		{
			add
			{
				this.backing_ControllerTypeChanged = (NControllerManager.ControllerTypeChangedEventHandler)Delegate.Combine(this.backing_ControllerTypeChanged, value);
			}
			remove
			{
				this.backing_ControllerTypeChanged = (NControllerManager.ControllerTypeChangedEventHandler)Delegate.Remove(this.backing_ControllerTypeChanged, value);
			}
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x001C85A2 File Offset: 0x001C67A2
		protected void EmitSignalControllerTypeChanged()
		{
			base.EmitSignal(NControllerManager.SignalName.ControllerTypeChanged, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06003BA7 RID: 15271 RVA: 0x001C85B8 File Offset: 0x001C67B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NControllerManager.SignalName.ControllerDetected && args.Count == 0)
			{
				NControllerManager.ControllerDetectedEventHandler controllerDetectedEventHandler = this.backing_ControllerDetected;
				if (controllerDetectedEventHandler == null)
				{
					return;
				}
				controllerDetectedEventHandler();
				return;
			}
			else if ((ref signal) == NControllerManager.SignalName.MouseDetected && args.Count == 0)
			{
				NControllerManager.MouseDetectedEventHandler mouseDetectedEventHandler = this.backing_MouseDetected;
				if (mouseDetectedEventHandler == null)
				{
					return;
				}
				mouseDetectedEventHandler();
				return;
			}
			else
			{
				if (!((ref signal) == NControllerManager.SignalName.ControllerTypeChanged) || args.Count != 0)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NControllerManager.ControllerTypeChangedEventHandler controllerTypeChangedEventHandler = this.backing_ControllerTypeChanged;
				if (controllerTypeChangedEventHandler == null)
				{
					return;
				}
				controllerTypeChangedEventHandler();
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003BA8 RID: 15272 RVA: 0x001C8642 File Offset: 0x001C6842
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NControllerManager.SignalName.ControllerDetected || (ref signal) == NControllerManager.SignalName.MouseDetected || (ref signal) == NControllerManager.SignalName.ControllerTypeChanged || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04001747 RID: 5959
		[Nullable(2)]
		private IControllerInputStrategy _inputStrategy;

		/// <summary>
		/// The position we warp the mouse to when we switch to controller mode. This is so it no
		/// longer hovers over the last control it ws positioned at
		/// </summary>
		// Token: 0x04001748 RID: 5960
		private static readonly Vector2 _offscreenPos = Vector2.One * -1000f;

		/// <summary>
		/// Used to reset the mouse position to the last place it was before we swapped to controller mode
		/// </summary>
		// Token: 0x04001749 RID: 5961
		private Vector2 _lastMousePosition;

		/// <summary>
		/// Number of frames to ignore mouse motion events after warping the cursor offscreen.
		/// WarpMouse generates a synthetic InputEventMouseMotion (via OS event queue, arriving next
		/// frame) that would otherwise immediately flip us back to mouse mode.
		/// </summary>
		// Token: 0x0400174A RID: 5962
		private int _skipMouseCheckFrames;

		/// <summary>
		/// Minimum relative displacement (squared) to consider a mouse motion event as a warp artifact
		/// rather than real user input. No human mouse movement covers 500+ pixels in a single frame.
		/// </summary>
		// Token: 0x0400174B RID: 5963
		private const float _warpDisplacementThresholdSq = 250000f;

		// Token: 0x0400174C RID: 5964
		[Nullable(1)]
		private MegaLabel _label;

		// Token: 0x0400174D RID: 5965
		[Nullable(2)]
		private Tween _notifyTween;

		// Token: 0x0400174F RID: 5967
		private NControllerManager.ControllerDetectedEventHandler backing_ControllerDetected;

		// Token: 0x04001750 RID: 5968
		private NControllerManager.MouseDetectedEventHandler backing_MouseDetected;

		// Token: 0x04001751 RID: 5969
		private NControllerManager.ControllerTypeChangedEventHandler backing_ControllerTypeChanged;

		// Token: 0x0200182D RID: 6189
		// (Invoke) Token: 0x0600A829 RID: 43049
		[Signal]
		public delegate void ControllerDetectedEventHandler();

		// Token: 0x0200182E RID: 6190
		// (Invoke) Token: 0x0600A82D RID: 43053
		[Signal]
		public delegate void MouseDetectedEventHandler();

		/// <summary>
		/// Fires when we detect that the controller type has changed (ie xbox to ps4).
		/// </summary>
		// Token: 0x0200182F RID: 6191
		// (Invoke) Token: 0x0600A831 RID: 43057
		[Signal]
		public delegate void ControllerTypeChangedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001830 RID: 6192
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005E9F RID: 24223
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005EA0 RID: 24224
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005EA1 RID: 24225
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'OnControllerTypeChanged' method.
			/// </summary>
			// Token: 0x04005EA2 RID: 24226
			public static readonly StringName OnControllerTypeChanged = "OnControllerTypeChanged";

			/// <summary>
			/// Cached name for the 'CheckForMouseInput' method.
			/// </summary>
			// Token: 0x04005EA3 RID: 24227
			public static readonly StringName CheckForMouseInput = "CheckForMouseInput";

			/// <summary>
			/// Cached name for the 'CheckForControllerInput' method.
			/// </summary>
			// Token: 0x04005EA4 RID: 24228
			public static readonly StringName CheckForControllerInput = "CheckForControllerInput";

			/// <summary>
			/// Cached name for the 'ControlModeChanged' method.
			/// </summary>
			// Token: 0x04005EA5 RID: 24229
			public static readonly StringName ControlModeChanged = "ControlModeChanged";

			/// <summary>
			/// Cached name for the 'OnScreenContextChanged' method.
			/// </summary>
			// Token: 0x04005EA6 RID: 24230
			public static readonly StringName OnScreenContextChanged = "OnScreenContextChanged";

			/// <summary>
			/// Cached name for the 'GetHotkeyIcon' method.
			/// </summary>
			// Token: 0x04005EA7 RID: 24231
			public static readonly StringName GetHotkeyIcon = "GetHotkeyIcon";

			/// <summary>
			/// Cached name for the 'GetLeftAnalogStickDirection' method.
			/// </summary>
			// Token: 0x04005EA8 RID: 24232
			public static readonly StringName GetLeftAnalogStickDirection = "GetLeftAnalogStickDirection";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001831 RID: 6193
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ShouldAllowControllerRebinding' property.
			/// </summary>
			// Token: 0x04005EA9 RID: 24233
			public static readonly StringName ShouldAllowControllerRebinding = "ShouldAllowControllerRebinding";

			/// <summary>
			/// Cached name for the 'IsUsingController' property.
			/// </summary>
			// Token: 0x04005EAA RID: 24234
			public static readonly StringName IsUsingController = "IsUsingController";

			/// <summary>
			/// Cached name for the 'ControllerMappingType' property.
			/// </summary>
			// Token: 0x04005EAB RID: 24235
			public static readonly StringName ControllerMappingType = "ControllerMappingType";

			/// <summary>
			/// Cached name for the '_lastMousePosition' field.
			/// </summary>
			// Token: 0x04005EAC RID: 24236
			public static readonly StringName _lastMousePosition = "_lastMousePosition";

			/// <summary>
			/// Cached name for the '_skipMouseCheckFrames' field.
			/// </summary>
			// Token: 0x04005EAD RID: 24237
			public static readonly StringName _skipMouseCheckFrames = "_skipMouseCheckFrames";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005EAE RID: 24238
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_notifyTween' field.
			/// </summary>
			// Token: 0x04005EAF RID: 24239
			public static readonly StringName _notifyTween = "_notifyTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001832 RID: 6194
		public class SignalName : Node.SignalName
		{
			/// <summary>
			/// Cached name for the 'ControllerDetected' signal.
			/// </summary>
			// Token: 0x04005EB0 RID: 24240
			public static readonly StringName ControllerDetected = "ControllerDetected";

			/// <summary>
			/// Cached name for the 'MouseDetected' signal.
			/// </summary>
			// Token: 0x04005EB1 RID: 24241
			public static readonly StringName MouseDetected = "MouseDetected";

			/// <summary>
			/// Cached name for the 'ControllerTypeChanged' signal.
			/// </summary>
			// Token: 0x04005EB2 RID: 24242
			public static readonly StringName ControllerTypeChanged = "ControllerTypeChanged";
		}
	}
}
