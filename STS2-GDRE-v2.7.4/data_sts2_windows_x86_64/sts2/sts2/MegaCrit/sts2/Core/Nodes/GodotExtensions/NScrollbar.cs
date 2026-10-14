using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	// Token: 0x020003A3 RID: 931
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NScrollbar.cs")]
	public class NScrollbar : Range
	{
		// Token: 0x06003812 RID: 14354 RVA: 0x001A7D45 File Offset: 0x001A5F45
		public override void _Ready()
		{
			this._handle = base.GetNode<Control>("%Handle");
		}

		/// <summary>
		/// WARNING: If overriding, be sure to call the base function to retain
		/// OnPressDown and OnRelease functionality.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x06003813 RID: 14355 RVA: 0x001A7D60 File Offset: 0x001A5F60
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			base._GuiInput(inputEvent);
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton == null)
			{
				InputEventMouseMotion inputEventMouseMotion = inputEvent as InputEventMouseMotion;
				if (inputEventMouseMotion == null)
				{
					return;
				}
				if (this._isDragging)
				{
					this.SetValueBasedOnMousePosition(inputEventMouseMotion.Position);
				}
			}
			else
			{
				MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
				bool flag = buttonIndex - 1L <= 1L;
				if (flag)
				{
					this._isDragging = inputEventMouseButton.IsPressed();
					this.SetValueBasedOnMousePosition(inputEventMouseButton.Position);
					base.EmitSignal(inputEventMouseButton.IsPressed() ? NScrollbar.SignalName.MousePressed : NScrollbar.SignalName.MouseReleased, new Variant[] { inputEvent });
					return;
				}
			}
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x001A7DFC File Offset: 0x001A5FFC
		private void SetValueBasedOnMousePosition(Vector2 mousePosition)
		{
			base.Value = (double)(mousePosition.Y / base.Size.Y) * base.MaxValue;
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x001A7E1E File Offset: 0x001A601E
		public void SetValueWithoutAnimation(double value)
		{
			this._currentHandlePosition = (float)value;
			base.Value = value;
			this.UpdateHandlePosition();
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x001A7E35 File Offset: 0x001A6035
		public override void _Process(double delta)
		{
			this._currentHandlePosition = (float)base.Value;
			this.UpdateHandlePosition();
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x001A7E4C File Offset: 0x001A604C
		private void UpdateHandlePosition()
		{
			float num = MathHelper.SmoothDamp(this._handle.Position.Y, base.Size.Y * (float)((double)this._currentHandlePosition / base.MaxValue) - this._handle.Size.Y * 0.5f, ref this._currentVelocity, 0.05f, (float)base.GetProcessDeltaTime(), float.PositiveInfinity);
			this._handle.Position = new Vector2((base.Size.X - this._handle.Size.X) * 0.5f, num);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003818 RID: 14360 RVA: 0x001A7EEC File Offset: 0x001A60EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NScrollbar.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollbar.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollbar.MethodName.SetValueBasedOnMousePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "mousePosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollbar.MethodName.SetValueWithoutAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollbar.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScrollbar.MethodName.UpdateHandlePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003819 RID: 14361 RVA: 0x001A80BC File Offset: 0x001A62BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScrollbar.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbar.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbar.MethodName.SetValueBasedOnMousePosition && args.Count == 1)
			{
				this.SetValueBasedOnMousePosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbar.MethodName.SetValueWithoutAnimation && args.Count == 1)
			{
				this.SetValueWithoutAnimation(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbar.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbar.MethodName.UpdateHandlePosition && args.Count == 0)
			{
				this.UpdateHandlePosition();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600381A RID: 14362 RVA: 0x001A81E8 File Offset: 0x001A63E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScrollbar.MethodName._Ready || (ref method) == NScrollbar.MethodName._GuiInput || (ref method) == NScrollbar.MethodName.SetValueBasedOnMousePosition || (ref method) == NScrollbar.MethodName.SetValueWithoutAnimation || (ref method) == NScrollbar.MethodName._Process || (ref method) == NScrollbar.MethodName.UpdateHandlePosition || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600381B RID: 14363 RVA: 0x001A8258 File Offset: 0x001A6458
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScrollbar.PropertyName._handle)
			{
				this._handle = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._currentHandlePosition)
			{
				this._currentHandlePosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._currentVelocity)
			{
				this._currentVelocity = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600381C RID: 14364 RVA: 0x001A82DC File Offset: 0x001A64DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScrollbar.PropertyName._handle)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._handle);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._currentHandlePosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._currentHandlePosition);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._currentVelocity)
			{
				value = VariantUtils.CreateFrom<float>(ref this._currentVelocity);
				return true;
			}
			if ((ref name) == NScrollbar.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600381D RID: 14365 RVA: 0x001A8374 File Offset: 0x001A6574
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NScrollbar.PropertyName._handle, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollbar.PropertyName._currentHandlePosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScrollbar.PropertyName._currentVelocity, 0L, "", 4096L, false),
				new PropertyInfo(1L, NScrollbar.PropertyName._isDragging, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600381E RID: 14366 RVA: 0x001A840C File Offset: 0x001A660C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NScrollbar.PropertyName._handle, Variant.From<Control>(ref this._handle));
			info.AddProperty(NScrollbar.PropertyName._currentHandlePosition, Variant.From<float>(ref this._currentHandlePosition));
			info.AddProperty(NScrollbar.PropertyName._currentVelocity, Variant.From<float>(ref this._currentVelocity));
			info.AddProperty(NScrollbar.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddSignalEventDelegate(NScrollbar.SignalName.MouseReleased, this.backing_MouseReleased);
			info.AddSignalEventDelegate(NScrollbar.SignalName.MousePressed, this.backing_MousePressed);
		}

		/// <inheritdoc />
		// Token: 0x0600381F RID: 14367 RVA: 0x001A849C File Offset: 0x001A669C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScrollbar.PropertyName._handle, ref variant))
			{
				this._handle = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NScrollbar.PropertyName._currentHandlePosition, ref variant2))
			{
				this._currentHandlePosition = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NScrollbar.PropertyName._currentVelocity, ref variant3))
			{
				this._currentVelocity = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NScrollbar.PropertyName._isDragging, ref variant4))
			{
				this._isDragging = variant4.As<bool>();
			}
			NScrollbar.MouseReleasedEventHandler mouseReleasedEventHandler;
			if (info.TryGetSignalEventDelegate<NScrollbar.MouseReleasedEventHandler>(NScrollbar.SignalName.MouseReleased, ref mouseReleasedEventHandler))
			{
				this.backing_MouseReleased = mouseReleasedEventHandler;
			}
			NScrollbar.MousePressedEventHandler mousePressedEventHandler;
			if (info.TryGetSignalEventDelegate<NScrollbar.MousePressedEventHandler>(NScrollbar.SignalName.MousePressed, ref mousePressedEventHandler))
			{
				this.backing_MousePressed = mousePressedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003820 RID: 14368 RVA: 0x001A8550 File Offset: 0x001A6750
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NScrollbar.SignalName.MouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NScrollbar.SignalName.MousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NScrollbar.MouseReleasedEventHandler" />
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06003821 RID: 14369 RVA: 0x001A8623 File Offset: 0x001A6823
		// (remove) Token: 0x06003822 RID: 14370 RVA: 0x001A863C File Offset: 0x001A683C
		public event NScrollbar.MouseReleasedEventHandler MouseReleased
		{
			add
			{
				this.backing_MouseReleased = (NScrollbar.MouseReleasedEventHandler)Delegate.Combine(this.backing_MouseReleased, value);
			}
			remove
			{
				this.backing_MouseReleased = (NScrollbar.MouseReleasedEventHandler)Delegate.Remove(this.backing_MouseReleased, value);
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x001A8655 File Offset: 0x001A6855
		protected void EmitSignalMouseReleased(InputEvent inputEvent)
		{
			base.EmitSignal(NScrollbar.SignalName.MouseReleased, new Variant[] { inputEvent });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NScrollbar.MousePressedEventHandler" />
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06003824 RID: 14372 RVA: 0x001A8676 File Offset: 0x001A6876
		// (remove) Token: 0x06003825 RID: 14373 RVA: 0x001A868F File Offset: 0x001A688F
		public event NScrollbar.MousePressedEventHandler MousePressed
		{
			add
			{
				this.backing_MousePressed = (NScrollbar.MousePressedEventHandler)Delegate.Combine(this.backing_MousePressed, value);
			}
			remove
			{
				this.backing_MousePressed = (NScrollbar.MousePressedEventHandler)Delegate.Remove(this.backing_MousePressed, value);
			}
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x001A86A8 File Offset: 0x001A68A8
		protected void EmitSignalMousePressed(InputEvent inputEvent)
		{
			base.EmitSignal(NScrollbar.SignalName.MousePressed, new Variant[] { inputEvent });
		}

		/// <inheritdoc />
		// Token: 0x06003827 RID: 14375 RVA: 0x001A86CC File Offset: 0x001A68CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NScrollbar.SignalName.MouseReleased && args.Count == 1)
			{
				NScrollbar.MouseReleasedEventHandler mouseReleasedEventHandler = this.backing_MouseReleased;
				if (mouseReleasedEventHandler == null)
				{
					return;
				}
				mouseReleasedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NScrollbar.SignalName.MousePressed) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NScrollbar.MousePressedEventHandler mousePressedEventHandler = this.backing_MousePressed;
				if (mousePressedEventHandler == null)
				{
					return;
				}
				mousePressedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003828 RID: 14376 RVA: 0x001A874B File Offset: 0x001A694B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NScrollbar.SignalName.MouseReleased || (ref signal) == NScrollbar.SignalName.MousePressed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040015C2 RID: 5570
		[Nullable(1)]
		private Control _handle;

		// Token: 0x040015C3 RID: 5571
		private float _currentHandlePosition;

		// Token: 0x040015C4 RID: 5572
		private float _currentVelocity;

		// Token: 0x040015C5 RID: 5573
		private bool _isDragging;

		// Token: 0x040015C6 RID: 5574
		private NScrollbar.MouseReleasedEventHandler backing_MouseReleased;

		// Token: 0x040015C7 RID: 5575
		private NScrollbar.MousePressedEventHandler backing_MousePressed;

		// Token: 0x0200177F RID: 6015
		// (Invoke) Token: 0x0600A6DF RID: 42719
		[Signal]
		public delegate void MouseReleasedEventHandler(InputEvent inputEvent);

		// Token: 0x02001780 RID: 6016
		// (Invoke) Token: 0x0600A6E3 RID: 42723
		[Signal]
		public delegate void MousePressedEventHandler(InputEvent inputEvent);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001781 RID: 6017
		public class MethodName : Range.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BC5 RID: 23493
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005BC6 RID: 23494
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'SetValueBasedOnMousePosition' method.
			/// </summary>
			// Token: 0x04005BC7 RID: 23495
			public static readonly StringName SetValueBasedOnMousePosition = "SetValueBasedOnMousePosition";

			/// <summary>
			/// Cached name for the 'SetValueWithoutAnimation' method.
			/// </summary>
			// Token: 0x04005BC8 RID: 23496
			public static readonly StringName SetValueWithoutAnimation = "SetValueWithoutAnimation";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005BC9 RID: 23497
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateHandlePosition' method.
			/// </summary>
			// Token: 0x04005BCA RID: 23498
			public static readonly StringName UpdateHandlePosition = "UpdateHandlePosition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001782 RID: 6018
		public class PropertyName : Range.PropertyName
		{
			/// <summary>
			/// Cached name for the '_handle' field.
			/// </summary>
			// Token: 0x04005BCB RID: 23499
			public static readonly StringName _handle = "_handle";

			/// <summary>
			/// Cached name for the '_currentHandlePosition' field.
			/// </summary>
			// Token: 0x04005BCC RID: 23500
			public static readonly StringName _currentHandlePosition = "_currentHandlePosition";

			/// <summary>
			/// Cached name for the '_currentVelocity' field.
			/// </summary>
			// Token: 0x04005BCD RID: 23501
			public static readonly StringName _currentVelocity = "_currentVelocity";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x04005BCE RID: 23502
			public static readonly StringName _isDragging = "_isDragging";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001783 RID: 6019
		public class SignalName : Range.SignalName
		{
			/// <summary>
			/// Cached name for the 'MouseReleased' signal.
			/// </summary>
			// Token: 0x04005BCF RID: 23503
			public static readonly StringName MouseReleased = "MouseReleased";

			/// <summary>
			/// Cached name for the 'MousePressed' signal.
			/// </summary>
			// Token: 0x04005BD0 RID: 23504
			public static readonly StringName MousePressed = "MousePressed";
		}
	}
}
