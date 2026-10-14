using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// The "clickable"
	/// </summary>
	// Token: 0x0200039D RID: 925
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NClickableControl.cs")]
	public class NClickableControl : Control
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x001A3240 File Offset: 0x001A1440
		protected virtual bool AllowFocusWhileDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x001A3243 File Offset: 0x001A1443
		// (set) Token: 0x06003784 RID: 14212 RVA: 0x001A324B File Offset: 0x001A144B
		private protected bool IsFocused { protected get; private set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x001A3254 File Offset: 0x001A1454
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x001A325C File Offset: 0x001A145C
		protected virtual void ConnectSignals()
		{
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocusHandler)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnFocusHandler)), 0U);
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHoverHandler)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhoverHandler)), 0U);
			base.Connect(NClickableControl.SignalName.MousePressed, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMousePress)), 0U);
			base.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMouseRelease)), 0U);
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
			base.AddThemeStyleboxOverride(ThemeConstants.Control.Focus, NClickableControl._blankFocusStyle);
			this._isControllerNavigable = base.FocusMode == 2L;
			if (base.HasFocus())
			{
				this.OnFocusHandler();
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x001A3369 File Offset: 0x001A1569
		private void OnVisibilityChanged()
		{
			if (!base.IsVisibleInTree())
			{
				this.OnUnFocusHandler();
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x001A3379 File Offset: 0x001A1579
		private void OnFocusHandler()
		{
			this._isControllerFocused = true;
			this.RefreshFocus();
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x001A3388 File Offset: 0x001A1588
		private void OnUnFocusHandler()
		{
			this._isControllerFocused = false;
			this.RefreshFocus();
		}

		/// <summary>
		/// Called when we start pressing down on the mouse button
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x0600378A RID: 14218 RVA: 0x001A3398 File Offset: 0x001A1598
		[NullableContext(1)]
		private void HandleMousePress(InputEvent inputEvent)
		{
			if (!this._isEnabled || !base.IsVisibleInTree() || !this.IsFocused)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L)
			{
				this._isControllerFocused = false;
				this._beginDragPosition = inputEventMouseButton.GlobalPosition;
				this.OnPressHandler();
			}
		}

		/// <summary>
		/// Called when the mouse button is released on this control (had to start the click in this Control too)
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x0600378B RID: 14219 RVA: 0x001A33EC File Offset: 0x001A15EC
		[NullableContext(1)]
		private void HandleMouseRelease(InputEvent inputEvent)
		{
			if (!this._isEnabled)
			{
				return;
			}
			if (!base.IsVisibleInTree())
			{
				return;
			}
			if (!this.IsFocused)
			{
				return;
			}
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L)
			{
				this.OnReleaseHandler();
			}
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x001A342E File Offset: 0x001A162E
		private void OnHoverHandler()
		{
			this._isHovered = true;
			if (base.GetTree().Paused && !NGame.IsReleaseGame())
			{
				return;
			}
			this.RefreshFocus();
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x001A3452 File Offset: 0x001A1652
		private void OnUnhoverHandler()
		{
			this._isHovered = false;
			if (base.GetTree().Paused && !NGame.IsReleaseGame())
			{
				return;
			}
			this.RefreshFocus();
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x001A3476 File Offset: 0x001A1676
		protected void OnPressHandler()
		{
			this._isPressed = true;
			this.OnPress();
		}

		/// <summary>
		/// Middle-man function to prevent base.OnRelease() and to
		/// allow handling of various input types simultaneously.
		/// Required because we invoke OnButtonReleased
		/// </summary>
		// Token: 0x0600378F RID: 14223 RVA: 0x001A3485 File Offset: 0x001A1685
		protected void OnReleaseHandler()
		{
			if (!this._isPressed)
			{
				return;
			}
			this._isPressed = false;
			this.OnRelease();
			base.EmitSignal(NClickableControl.SignalName.Released, new Variant[] { this });
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x001A34BC File Offset: 0x001A16BC
		private void RefreshFocus()
		{
			bool flag = (this._isEnabled || this.AllowFocusWhileDisabled) && base.IsVisibleInTree();
			bool flag2 = flag && (this._isHovered || this._isControllerFocused);
			if (this.IsFocused != flag2)
			{
				this.IsFocused = flag2;
				if (this.IsFocused)
				{
					base.EmitSignal(NClickableControl.SignalName.Focused, new Variant[] { this });
					this.OnFocus();
					return;
				}
				base.EmitSignal(NClickableControl.SignalName.Unfocused, new Variant[] { this });
				this.OnUnfocus();
			}
		}

		/// <summary>
		/// Called when a user hovers this button or navigates to/focuses on this button (controller).
		/// </summary>
		// Token: 0x06003791 RID: 14225 RVA: 0x001A355E File Offset: 0x001A175E
		protected virtual void OnFocus()
		{
		}

		/// <summary>
		/// Called when a user unhovers or navigates away from this button (controller).
		/// </summary>
		// Token: 0x06003792 RID: 14226 RVA: 0x001A3560 File Offset: 0x001A1760
		protected virtual void OnUnfocus()
		{
		}

		/// <summary>
		/// Called when a user starts a click or button press.
		/// </summary>
		// Token: 0x06003793 RID: 14227 RVA: 0x001A3562 File Offset: 0x001A1762
		protected virtual void OnPress()
		{
		}

		/// <summary>
		/// Called when the player releases a click or button press.
		/// NOTE: Doesn't trigger unless the user triggered OnPressDown on this button.
		/// </summary>
		// Token: 0x06003794 RID: 14228 RVA: 0x001A3564 File Offset: 0x001A1764
		protected virtual void OnRelease()
		{
		}

		/// <summary>
		/// WARNING: If overriding, be sure to call the base function to retain
		/// OnPressDown and OnRelease functionality.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003795 RID: 14229 RVA: 0x001A3568 File Offset: 0x001A1768
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && this._isEnabled)
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				bool flag = buttonIndex - 1L <= 1L;
				if (flag)
				{
					base.EmitSignal(inputEventMouseButton.IsPressed() ? NClickableControl.SignalName.MousePressed : NClickableControl.SignalName.MouseReleased, new Variant[] { inputEvent });
				}
			}
			if (inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				this.OnPressHandler();
				return;
			}
			if (inputEvent.IsActionReleased(MegaInput.select, false))
			{
				this.OnReleaseHandler();
			}
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x001A35F8 File Offset: 0x001A17F8
		[NullableContext(1)]
		protected void CheckMouseDragThreshold(InputEvent inputEvent)
		{
			if (this._ignoreDragThreshold <= 0f)
			{
				return;
			}
			if (this._isPressed)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion != null && inputEventMouseMotion.GlobalPosition.DistanceTo(this._beginDragPosition) >= this._ignoreDragThreshold)
				{
					this._isPressed = false;
				}
			}
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x001A3648 File Offset: 0x001A1848
		public void DebugPress()
		{
			base.EmitSignal(NClickableControl.SignalName.MousePressed, new Variant[]
			{
				new InputEventMouseButton
				{
					ButtonIndex = 1L,
					Pressed = true
				}
			});
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x001A367C File Offset: 0x001A187C
		public void DebugRelease()
		{
			base.EmitSignal(NClickableControl.SignalName.MouseReleased, new Variant[]
			{
				new InputEventMouseButton
				{
					ButtonIndex = 1L,
					Pressed = false
				}
			});
		}

		/// <summary>
		/// Force emits the Released signal and calls OnRelease(), bypassing all hover/focus/pause checks.
		/// Used by AutoSlay for automated testing.
		/// </summary>
		// Token: 0x06003799 RID: 14233 RVA: 0x001A36B0 File Offset: 0x001A18B0
		public void ForceClick()
		{
			this.OnRelease();
			base.EmitSignal(NClickableControl.SignalName.Released, new Variant[] { this });
		}

		/// <summary>
		/// Helper function to enable/disable this button similar to how Unity handles it
		/// </summary>
		// Token: 0x0600379A RID: 14234 RVA: 0x001A36D7 File Offset: 0x001A18D7
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}

		/// <summary>
		/// Helper function to enable this button similar to how Unity handles it
		/// </summary>
		// Token: 0x0600379B RID: 14235 RVA: 0x001A36EC File Offset: 0x001A18EC
		public void Enable()
		{
			if (this._isEnabled)
			{
				return;
			}
			this._isEnabled = true;
			base.FocusMode = (this._isControllerNavigable ? 2L : 0L);
			this.OnEnable();
			this.RefreshFocus();
			Callable.From(delegate
			{
				base.SetProcessInput(true);
			}).CallDeferred(Array.Empty<Variant>());
		}

		/// <summary>
		/// Helper function to disable this button similar to how Unity handles it
		/// </summary>
		// Token: 0x0600379C RID: 14236 RVA: 0x001A3747 File Offset: 0x001A1947
		public void Disable()
		{
			if (!this._isEnabled)
			{
				return;
			}
			this._isEnabled = false;
			this._isPressed = false;
			base.FocusMode = 0L;
			this.OnDisable();
			this.RefreshFocus();
			base.SetProcessInput(false);
		}

		/// <summary>
		/// Called whenever this button becomes enabled
		/// </summary>
		// Token: 0x0600379D RID: 14237 RVA: 0x001A377B File Offset: 0x001A197B
		protected virtual void OnEnable()
		{
		}

		/// <summary>
		/// Called whenever this button becomes disabled
		/// </summary>
		// Token: 0x0600379E RID: 14238 RVA: 0x001A377D File Offset: 0x001A197D
		protected virtual void OnDisable()
		{
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600379F RID: 14239 RVA: 0x001A3780 File Offset: 0x001A1980
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(25)
			{
				new MethodInfo(NClickableControl.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnVisibilityChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnFocusHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnUnFocusHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.HandleMousePress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NClickableControl.MethodName.HandleMouseRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NClickableControl.MethodName.OnHoverHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnUnhoverHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnPressHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnReleaseHandler, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.RefreshFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NClickableControl.MethodName.CheckMouseDragThreshold, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NClickableControl.MethodName.DebugPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.DebugRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.ForceClick, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.SetEnabled, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "enabled", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NClickableControl.MethodName.Enable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnEnable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClickableControl.MethodName.OnDisable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037A0 RID: 14240 RVA: 0x001A3D14 File Offset: 0x001A1F14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NClickableControl.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnVisibilityChanged && args.Count == 0)
			{
				this.OnVisibilityChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnFocusHandler && args.Count == 0)
			{
				this.OnFocusHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnUnFocusHandler && args.Count == 0)
			{
				this.OnUnFocusHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.HandleMousePress && args.Count == 1)
			{
				this.HandleMousePress(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.HandleMouseRelease && args.Count == 1)
			{
				this.HandleMouseRelease(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnHoverHandler && args.Count == 0)
			{
				this.OnHoverHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnUnhoverHandler && args.Count == 0)
			{
				this.OnUnhoverHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnPressHandler && args.Count == 0)
			{
				this.OnPressHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnReleaseHandler && args.Count == 0)
			{
				this.OnReleaseHandler();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.RefreshFocus && args.Count == 0)
			{
				this.RefreshFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.CheckMouseDragThreshold && args.Count == 1)
			{
				this.CheckMouseDragThreshold(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.DebugPress && args.Count == 0)
			{
				this.DebugPress();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.DebugRelease && args.Count == 0)
			{
				this.DebugRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.ForceClick && args.Count == 0)
			{
				this.ForceClick();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.SetEnabled && args.Count == 1)
			{
				this.SetEnabled(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.Enable && args.Count == 0)
			{
				this.Enable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnEnable && args.Count == 0)
			{
				this.OnEnable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClickableControl.MethodName.OnDisable && args.Count == 0)
			{
				this.OnDisable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060037A1 RID: 14241 RVA: 0x001A4110 File Offset: 0x001A2310
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NClickableControl.MethodName.ConnectSignals || (ref method) == NClickableControl.MethodName.OnVisibilityChanged || (ref method) == NClickableControl.MethodName.OnFocusHandler || (ref method) == NClickableControl.MethodName.OnUnFocusHandler || (ref method) == NClickableControl.MethodName.HandleMousePress || (ref method) == NClickableControl.MethodName.HandleMouseRelease || (ref method) == NClickableControl.MethodName.OnHoverHandler || (ref method) == NClickableControl.MethodName.OnUnhoverHandler || (ref method) == NClickableControl.MethodName.OnPressHandler || (ref method) == NClickableControl.MethodName.OnReleaseHandler || (ref method) == NClickableControl.MethodName.RefreshFocus || (ref method) == NClickableControl.MethodName.OnFocus || (ref method) == NClickableControl.MethodName.OnUnfocus || (ref method) == NClickableControl.MethodName.OnPress || (ref method) == NClickableControl.MethodName.OnRelease || (ref method) == NClickableControl.MethodName._GuiInput || (ref method) == NClickableControl.MethodName.CheckMouseDragThreshold || (ref method) == NClickableControl.MethodName.DebugPress || (ref method) == NClickableControl.MethodName.DebugRelease || (ref method) == NClickableControl.MethodName.ForceClick || (ref method) == NClickableControl.MethodName.SetEnabled || (ref method) == NClickableControl.MethodName.Enable || (ref method) == NClickableControl.MethodName.Disable || (ref method) == NClickableControl.MethodName.OnEnable || (ref method) == NClickableControl.MethodName.OnDisable || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060037A2 RID: 14242 RVA: 0x001A429C File Offset: 0x001A249C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NClickableControl.PropertyName.IsFocused)
			{
				this.IsFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._ignoreDragThreshold)
			{
				this._ignoreDragThreshold = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isEnabled)
			{
				this._isEnabled = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isHovered)
			{
				this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isControllerFocused)
			{
				this._isControllerFocused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isControllerNavigable)
			{
				this._isControllerNavigable = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._beginDragPosition)
			{
				this._beginDragPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isPressed)
			{
				this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060037A3 RID: 14243 RVA: 0x001A438C File Offset: 0x001A258C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NClickableControl.PropertyName.AllowFocusWhileDisabled)
			{
				bool flag = this.AllowFocusWhileDisabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName.IsFocused)
			{
				bool flag = this.IsFocused;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName.IsEnabled)
			{
				bool flag = this.IsEnabled;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._ignoreDragThreshold)
			{
				value = VariantUtils.CreateFrom<float>(ref this._ignoreDragThreshold);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isEnabled)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isEnabled);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isHovered)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isControllerFocused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isControllerFocused);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isControllerNavigable)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isControllerNavigable);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._beginDragPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._beginDragPosition);
				return true;
			}
			if ((ref name) == NClickableControl.PropertyName._isPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037A4 RID: 14244 RVA: 0x001A44EC File Offset: 0x001A26EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NClickableControl.PropertyName._ignoreDragThreshold, 0L, "", 4102L, true),
				new PropertyInfo(1L, NClickableControl.PropertyName.AllowFocusWhileDisabled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName.IsFocused, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName._isEnabled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName.IsEnabled, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName._isHovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName._isControllerFocused, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName._isControllerNavigable, 0L, "", 4096L, false),
				new PropertyInfo(5L, NClickableControl.PropertyName._beginDragPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NClickableControl.PropertyName._isPressed, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037A5 RID: 14245 RVA: 0x001A4640 File Offset: 0x001A2840
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isFocused = NClickableControl.PropertyName.IsFocused;
			bool isFocused2 = this.IsFocused;
			info.AddProperty(isFocused, Variant.From<bool>(ref isFocused2));
			info.AddProperty(NClickableControl.PropertyName._ignoreDragThreshold, Variant.From<float>(ref this._ignoreDragThreshold));
			info.AddProperty(NClickableControl.PropertyName._isEnabled, Variant.From<bool>(ref this._isEnabled));
			info.AddProperty(NClickableControl.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
			info.AddProperty(NClickableControl.PropertyName._isControllerFocused, Variant.From<bool>(ref this._isControllerFocused));
			info.AddProperty(NClickableControl.PropertyName._isControllerNavigable, Variant.From<bool>(ref this._isControllerNavigable));
			info.AddProperty(NClickableControl.PropertyName._beginDragPosition, Variant.From<Vector2>(ref this._beginDragPosition));
			info.AddProperty(NClickableControl.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
			info.AddSignalEventDelegate(NClickableControl.SignalName.Released, this.backing_Released);
			info.AddSignalEventDelegate(NClickableControl.SignalName.Focused, this.backing_Focused);
			info.AddSignalEventDelegate(NClickableControl.SignalName.Unfocused, this.backing_Unfocused);
			info.AddSignalEventDelegate(NClickableControl.SignalName.MouseReleased, this.backing_MouseReleased);
			info.AddSignalEventDelegate(NClickableControl.SignalName.MousePressed, this.backing_MousePressed);
		}

		/// <inheritdoc />
		// Token: 0x060037A6 RID: 14246 RVA: 0x001A475C File Offset: 0x001A295C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NClickableControl.PropertyName.IsFocused, ref variant))
			{
				this.IsFocused = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NClickableControl.PropertyName._ignoreDragThreshold, ref variant2))
			{
				this._ignoreDragThreshold = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NClickableControl.PropertyName._isEnabled, ref variant3))
			{
				this._isEnabled = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NClickableControl.PropertyName._isHovered, ref variant4))
			{
				this._isHovered = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NClickableControl.PropertyName._isControllerFocused, ref variant5))
			{
				this._isControllerFocused = variant5.As<bool>();
			}
			Variant variant6;
			if (info.TryGetProperty(NClickableControl.PropertyName._isControllerNavigable, ref variant6))
			{
				this._isControllerNavigable = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NClickableControl.PropertyName._beginDragPosition, ref variant7))
			{
				this._beginDragPosition = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NClickableControl.PropertyName._isPressed, ref variant8))
			{
				this._isPressed = variant8.As<bool>();
			}
			NClickableControl.ReleasedEventHandler releasedEventHandler;
			if (info.TryGetSignalEventDelegate<NClickableControl.ReleasedEventHandler>(NClickableControl.SignalName.Released, ref releasedEventHandler))
			{
				this.backing_Released = releasedEventHandler;
			}
			NClickableControl.FocusedEventHandler focusedEventHandler;
			if (info.TryGetSignalEventDelegate<NClickableControl.FocusedEventHandler>(NClickableControl.SignalName.Focused, ref focusedEventHandler))
			{
				this.backing_Focused = focusedEventHandler;
			}
			NClickableControl.UnfocusedEventHandler unfocusedEventHandler;
			if (info.TryGetSignalEventDelegate<NClickableControl.UnfocusedEventHandler>(NClickableControl.SignalName.Unfocused, ref unfocusedEventHandler))
			{
				this.backing_Unfocused = unfocusedEventHandler;
			}
			NClickableControl.MouseReleasedEventHandler mouseReleasedEventHandler;
			if (info.TryGetSignalEventDelegate<NClickableControl.MouseReleasedEventHandler>(NClickableControl.SignalName.MouseReleased, ref mouseReleasedEventHandler))
			{
				this.backing_MouseReleased = mouseReleasedEventHandler;
			}
			NClickableControl.MousePressedEventHandler mousePressedEventHandler;
			if (info.TryGetSignalEventDelegate<NClickableControl.MousePressedEventHandler>(NClickableControl.SignalName.MousePressed, ref mousePressedEventHandler))
			{
				this.backing_MousePressed = mousePressedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037A7 RID: 14247 RVA: 0x001A48C4 File Offset: 0x001A2AC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NClickableControl.SignalName.Released, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NClickableControl.SignalName.Focused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NClickableControl.SignalName.Unfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "button", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NClickableControl.SignalName.MouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NClickableControl.SignalName.MousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.ReleasedEventHandler" />
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060037A8 RID: 14248 RVA: 0x001A4AB4 File Offset: 0x001A2CB4
		// (remove) Token: 0x060037A9 RID: 14249 RVA: 0x001A4ACD File Offset: 0x001A2CCD
		public event NClickableControl.ReleasedEventHandler Released
		{
			add
			{
				this.backing_Released = (NClickableControl.ReleasedEventHandler)Delegate.Combine(this.backing_Released, value);
			}
			remove
			{
				this.backing_Released = (NClickableControl.ReleasedEventHandler)Delegate.Remove(this.backing_Released, value);
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x001A4AE6 File Offset: 0x001A2CE6
		protected void EmitSignalReleased(NClickableControl button)
		{
			base.EmitSignal(NClickableControl.SignalName.Released, new Variant[] { button });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.FocusedEventHandler" />
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060037AB RID: 14251 RVA: 0x001A4B07 File Offset: 0x001A2D07
		// (remove) Token: 0x060037AC RID: 14252 RVA: 0x001A4B20 File Offset: 0x001A2D20
		public event NClickableControl.FocusedEventHandler Focused
		{
			add
			{
				this.backing_Focused = (NClickableControl.FocusedEventHandler)Delegate.Combine(this.backing_Focused, value);
			}
			remove
			{
				this.backing_Focused = (NClickableControl.FocusedEventHandler)Delegate.Remove(this.backing_Focused, value);
			}
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x001A4B39 File Offset: 0x001A2D39
		protected void EmitSignalFocused(NClickableControl button)
		{
			base.EmitSignal(NClickableControl.SignalName.Focused, new Variant[] { button });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.UnfocusedEventHandler" />
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060037AE RID: 14254 RVA: 0x001A4B5A File Offset: 0x001A2D5A
		// (remove) Token: 0x060037AF RID: 14255 RVA: 0x001A4B73 File Offset: 0x001A2D73
		public event NClickableControl.UnfocusedEventHandler Unfocused
		{
			add
			{
				this.backing_Unfocused = (NClickableControl.UnfocusedEventHandler)Delegate.Combine(this.backing_Unfocused, value);
			}
			remove
			{
				this.backing_Unfocused = (NClickableControl.UnfocusedEventHandler)Delegate.Remove(this.backing_Unfocused, value);
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001A4B8C File Offset: 0x001A2D8C
		protected void EmitSignalUnfocused(NClickableControl button)
		{
			base.EmitSignal(NClickableControl.SignalName.Unfocused, new Variant[] { button });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.MouseReleasedEventHandler" />
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060037B1 RID: 14257 RVA: 0x001A4BAD File Offset: 0x001A2DAD
		// (remove) Token: 0x060037B2 RID: 14258 RVA: 0x001A4BC6 File Offset: 0x001A2DC6
		public event NClickableControl.MouseReleasedEventHandler MouseReleased
		{
			add
			{
				this.backing_MouseReleased = (NClickableControl.MouseReleasedEventHandler)Delegate.Combine(this.backing_MouseReleased, value);
			}
			remove
			{
				this.backing_MouseReleased = (NClickableControl.MouseReleasedEventHandler)Delegate.Remove(this.backing_MouseReleased, value);
			}
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x001A4BDF File Offset: 0x001A2DDF
		protected void EmitSignalMouseReleased(InputEvent inputEvent)
		{
			base.EmitSignal(NClickableControl.SignalName.MouseReleased, new Variant[] { inputEvent });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NClickableControl.MousePressedEventHandler" />
		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060037B4 RID: 14260 RVA: 0x001A4C00 File Offset: 0x001A2E00
		// (remove) Token: 0x060037B5 RID: 14261 RVA: 0x001A4C19 File Offset: 0x001A2E19
		public event NClickableControl.MousePressedEventHandler MousePressed
		{
			add
			{
				this.backing_MousePressed = (NClickableControl.MousePressedEventHandler)Delegate.Combine(this.backing_MousePressed, value);
			}
			remove
			{
				this.backing_MousePressed = (NClickableControl.MousePressedEventHandler)Delegate.Remove(this.backing_MousePressed, value);
			}
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x001A4C32 File Offset: 0x001A2E32
		protected void EmitSignalMousePressed(InputEvent inputEvent)
		{
			base.EmitSignal(NClickableControl.SignalName.MousePressed, new Variant[] { inputEvent });
		}

		/// <inheritdoc />
		// Token: 0x060037B7 RID: 14263 RVA: 0x001A4C54 File Offset: 0x001A2E54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NClickableControl.SignalName.Released && args.Count == 1)
			{
				NClickableControl.ReleasedEventHandler releasedEventHandler = this.backing_Released;
				if (releasedEventHandler == null)
				{
					return;
				}
				releasedEventHandler(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				return;
			}
			else if ((ref signal) == NClickableControl.SignalName.Focused && args.Count == 1)
			{
				NClickableControl.FocusedEventHandler focusedEventHandler = this.backing_Focused;
				if (focusedEventHandler == null)
				{
					return;
				}
				focusedEventHandler(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				return;
			}
			else if ((ref signal) == NClickableControl.SignalName.Unfocused && args.Count == 1)
			{
				NClickableControl.UnfocusedEventHandler unfocusedEventHandler = this.backing_Unfocused;
				if (unfocusedEventHandler == null)
				{
					return;
				}
				unfocusedEventHandler(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				return;
			}
			else if ((ref signal) == NClickableControl.SignalName.MouseReleased && args.Count == 1)
			{
				NClickableControl.MouseReleasedEventHandler mouseReleasedEventHandler = this.backing_MouseReleased;
				if (mouseReleasedEventHandler == null)
				{
					return;
				}
				mouseReleasedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NClickableControl.SignalName.MousePressed) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NClickableControl.MousePressedEventHandler mousePressedEventHandler = this.backing_MousePressed;
				if (mousePressedEventHandler == null)
				{
					return;
				}
				mousePressedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x060037B8 RID: 14264 RVA: 0x001A4D74 File Offset: 0x001A2F74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NClickableControl.SignalName.Released || (ref signal) == NClickableControl.SignalName.Focused || (ref signal) == NClickableControl.SignalName.Unfocused || (ref signal) == NClickableControl.SignalName.MouseReleased || (ref signal) == NClickableControl.SignalName.MousePressed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040015A1 RID: 5537
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		protected float _ignoreDragThreshold = -1f;

		// Token: 0x040015A3 RID: 5539
		protected bool _isEnabled = true;

		// Token: 0x040015A4 RID: 5540
		private bool _isHovered;

		// Token: 0x040015A5 RID: 5541
		private bool _isControllerFocused;

		// Token: 0x040015A6 RID: 5542
		private bool _isControllerNavigable;

		// Token: 0x040015A7 RID: 5543
		private Vector2 _beginDragPosition;

		// Token: 0x040015A8 RID: 5544
		private bool _isPressed;

		// Token: 0x040015A9 RID: 5545
		[Nullable(1)]
		private static readonly StyleBoxEmpty _blankFocusStyle = new StyleBoxEmpty();

		// Token: 0x040015AA RID: 5546
		private NClickableControl.ReleasedEventHandler backing_Released;

		// Token: 0x040015AB RID: 5547
		private NClickableControl.FocusedEventHandler backing_Focused;

		// Token: 0x040015AC RID: 5548
		private NClickableControl.UnfocusedEventHandler backing_Unfocused;

		// Token: 0x040015AD RID: 5549
		private NClickableControl.MouseReleasedEventHandler backing_MouseReleased;

		// Token: 0x040015AE RID: 5550
		private NClickableControl.MousePressedEventHandler backing_MousePressed;

		// Token: 0x02001765 RID: 5989
		// (Invoke) Token: 0x0600A69C RID: 42652
		[Signal]
		public delegate void ReleasedEventHandler(NClickableControl button);

		// Token: 0x02001766 RID: 5990
		// (Invoke) Token: 0x0600A6A0 RID: 42656
		[Signal]
		public delegate void FocusedEventHandler(NClickableControl button);

		// Token: 0x02001767 RID: 5991
		// (Invoke) Token: 0x0600A6A4 RID: 42660
		[Signal]
		public delegate void UnfocusedEventHandler(NClickableControl button);

		// Token: 0x02001768 RID: 5992
		// (Invoke) Token: 0x0600A6A8 RID: 42664
		[Signal]
		public delegate void MouseReleasedEventHandler(InputEvent inputEvent);

		// Token: 0x02001769 RID: 5993
		// (Invoke) Token: 0x0600A6AC RID: 42668
		[Signal]
		public delegate void MousePressedEventHandler(InputEvent inputEvent);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176A RID: 5994
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04005B43 RID: 23363
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnVisibilityChanged' method.
			/// </summary>
			// Token: 0x04005B44 RID: 23364
			public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

			/// <summary>
			/// Cached name for the 'OnFocusHandler' method.
			/// </summary>
			// Token: 0x04005B45 RID: 23365
			public static readonly StringName OnFocusHandler = "OnFocusHandler";

			/// <summary>
			/// Cached name for the 'OnUnFocusHandler' method.
			/// </summary>
			// Token: 0x04005B46 RID: 23366
			public static readonly StringName OnUnFocusHandler = "OnUnFocusHandler";

			/// <summary>
			/// Cached name for the 'HandleMousePress' method.
			/// </summary>
			// Token: 0x04005B47 RID: 23367
			public static readonly StringName HandleMousePress = "HandleMousePress";

			/// <summary>
			/// Cached name for the 'HandleMouseRelease' method.
			/// </summary>
			// Token: 0x04005B48 RID: 23368
			public static readonly StringName HandleMouseRelease = "HandleMouseRelease";

			/// <summary>
			/// Cached name for the 'OnHoverHandler' method.
			/// </summary>
			// Token: 0x04005B49 RID: 23369
			public static readonly StringName OnHoverHandler = "OnHoverHandler";

			/// <summary>
			/// Cached name for the 'OnUnhoverHandler' method.
			/// </summary>
			// Token: 0x04005B4A RID: 23370
			public static readonly StringName OnUnhoverHandler = "OnUnhoverHandler";

			/// <summary>
			/// Cached name for the 'OnPressHandler' method.
			/// </summary>
			// Token: 0x04005B4B RID: 23371
			public static readonly StringName OnPressHandler = "OnPressHandler";

			/// <summary>
			/// Cached name for the 'OnReleaseHandler' method.
			/// </summary>
			// Token: 0x04005B4C RID: 23372
			public static readonly StringName OnReleaseHandler = "OnReleaseHandler";

			/// <summary>
			/// Cached name for the 'RefreshFocus' method.
			/// </summary>
			// Token: 0x04005B4D RID: 23373
			public static readonly StringName RefreshFocus = "RefreshFocus";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005B4E RID: 23374
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005B4F RID: 23375
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005B50 RID: 23376
			public static readonly StringName OnPress = "OnPress";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x04005B51 RID: 23377
			public static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005B52 RID: 23378
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'CheckMouseDragThreshold' method.
			/// </summary>
			// Token: 0x04005B53 RID: 23379
			public static readonly StringName CheckMouseDragThreshold = "CheckMouseDragThreshold";

			/// <summary>
			/// Cached name for the 'DebugPress' method.
			/// </summary>
			// Token: 0x04005B54 RID: 23380
			public static readonly StringName DebugPress = "DebugPress";

			/// <summary>
			/// Cached name for the 'DebugRelease' method.
			/// </summary>
			// Token: 0x04005B55 RID: 23381
			public static readonly StringName DebugRelease = "DebugRelease";

			/// <summary>
			/// Cached name for the 'ForceClick' method.
			/// </summary>
			// Token: 0x04005B56 RID: 23382
			public static readonly StringName ForceClick = "ForceClick";

			/// <summary>
			/// Cached name for the 'SetEnabled' method.
			/// </summary>
			// Token: 0x04005B57 RID: 23383
			public static readonly StringName SetEnabled = "SetEnabled";

			/// <summary>
			/// Cached name for the 'Enable' method.
			/// </summary>
			// Token: 0x04005B58 RID: 23384
			public static readonly StringName Enable = "Enable";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04005B59 RID: 23385
			public static readonly StringName Disable = "Disable";

			/// <summary>
			/// Cached name for the 'OnEnable' method.
			/// </summary>
			// Token: 0x04005B5A RID: 23386
			public static readonly StringName OnEnable = "OnEnable";

			/// <summary>
			/// Cached name for the 'OnDisable' method.
			/// </summary>
			// Token: 0x04005B5B RID: 23387
			public static readonly StringName OnDisable = "OnDisable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176B RID: 5995
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'AllowFocusWhileDisabled' property.
			/// </summary>
			// Token: 0x04005B5C RID: 23388
			public static readonly StringName AllowFocusWhileDisabled = "AllowFocusWhileDisabled";

			/// <summary>
			/// Cached name for the 'IsFocused' property.
			/// </summary>
			// Token: 0x04005B5D RID: 23389
			public static readonly StringName IsFocused = "IsFocused";

			/// <summary>
			/// Cached name for the 'IsEnabled' property.
			/// </summary>
			// Token: 0x04005B5E RID: 23390
			public static readonly StringName IsEnabled = "IsEnabled";

			/// <summary>
			/// Cached name for the '_ignoreDragThreshold' field.
			/// </summary>
			// Token: 0x04005B5F RID: 23391
			public static readonly StringName _ignoreDragThreshold = "_ignoreDragThreshold";

			/// <summary>
			/// Cached name for the '_isEnabled' field.
			/// </summary>
			// Token: 0x04005B60 RID: 23392
			public static readonly StringName _isEnabled = "_isEnabled";

			/// <summary>
			/// Cached name for the '_isHovered' field.
			/// </summary>
			// Token: 0x04005B61 RID: 23393
			public static readonly StringName _isHovered = "_isHovered";

			/// <summary>
			/// Cached name for the '_isControllerFocused' field.
			/// </summary>
			// Token: 0x04005B62 RID: 23394
			public static readonly StringName _isControllerFocused = "_isControllerFocused";

			/// <summary>
			/// Cached name for the '_isControllerNavigable' field.
			/// </summary>
			// Token: 0x04005B63 RID: 23395
			public static readonly StringName _isControllerNavigable = "_isControllerNavigable";

			/// <summary>
			/// Cached name for the '_beginDragPosition' field.
			/// </summary>
			// Token: 0x04005B64 RID: 23396
			public static readonly StringName _beginDragPosition = "_beginDragPosition";

			/// <summary>
			/// Cached name for the '_isPressed' field.
			/// </summary>
			// Token: 0x04005B65 RID: 23397
			public static readonly StringName _isPressed = "_isPressed";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200176C RID: 5996
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Released' signal.
			/// </summary>
			// Token: 0x04005B66 RID: 23398
			public static readonly StringName Released = "Released";

			/// <summary>
			/// Cached name for the 'Focused' signal.
			/// </summary>
			// Token: 0x04005B67 RID: 23399
			public static readonly StringName Focused = "Focused";

			/// <summary>
			/// Cached name for the 'Unfocused' signal.
			/// </summary>
			// Token: 0x04005B68 RID: 23400
			public static readonly StringName Unfocused = "Unfocused";

			/// <summary>
			/// Cached name for the 'MouseReleased' signal.
			/// </summary>
			// Token: 0x04005B69 RID: 23401
			public static readonly StringName MouseReleased = "MouseReleased";

			/// <summary>
			/// Cached name for the 'MousePressed' signal.
			/// </summary>
			// Token: 0x04005B6A RID: 23402
			public static readonly StringName MousePressed = "MousePressed";
		}
	}
}
