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
	/// <summary>
	/// A Slider class which doesn't rely on Godot's Slider class.
	/// Advantages are that the handle interpolates between positions and you can define the slider area/handle without
	/// restrictions imposed by the theming system.
	/// </summary>
	// Token: 0x020003A4 RID: 932
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NSlider.cs")]
	public class NSlider : Range
	{
		// Token: 0x0600382A RID: 14378 RVA: 0x001A877A File Offset: 0x001A697A
		public override void _Ready()
		{
			this._handle = base.GetNode<Control>("%Handle");
		}

		/// <summary>
		/// WARNING: If overriding, be sure to call the base function to retain
		/// OnPressDown and OnRelease functionality.
		/// </summary>
		/// <param name="inputEvent"></param>
		// Token: 0x0600382B RID: 14379 RVA: 0x001A8794 File Offset: 0x001A6994
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
					base.EmitSignal(inputEventMouseButton.IsPressed() ? NSlider.SignalName.MousePressed : NSlider.SignalName.MouseReleased, new Variant[] { inputEvent });
					return;
				}
			}
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x001A8830 File Offset: 0x001A6A30
		private void SetValueBasedOnMousePosition(Vector2 mousePosition)
		{
			base.Value = (double)(mousePosition.X / base.Size.X) * base.MaxValue;
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x001A8852 File Offset: 0x001A6A52
		public void SetValueWithoutAnimation(double value)
		{
			this._currentHandlePosition = (float)value;
			base.Value = value;
			this.UpdateHandlePosition();
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x001A8869 File Offset: 0x001A6A69
		public override void _Process(double delta)
		{
			this._currentHandlePosition = MathHelper.SmoothDamp(this._currentHandlePosition, (float)base.Value, ref this._currentVelocity, 0.05f, (float)delta, float.PositiveInfinity);
			this.UpdateHandlePosition();
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x001A889C File Offset: 0x001A6A9C
		private void UpdateHandlePosition()
		{
			this._handle.Position = new Vector2(base.Size.X * (float)((double)this._currentHandlePosition / base.MaxValue) - this._handle.Size.X * 0.5f, (base.Size.Y - this._handle.Size.Y) * 0.5f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003830 RID: 14384 RVA: 0x001A8910 File Offset: 0x001A6B10
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSlider.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSlider.MethodName.SetValueBasedOnMousePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "mousePosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSlider.MethodName.SetValueWithoutAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSlider.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSlider.MethodName.UpdateHandlePosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003831 RID: 14385 RVA: 0x001A8AE0 File Offset: 0x001A6CE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlider.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlider.MethodName.SetValueBasedOnMousePosition && args.Count == 1)
			{
				this.SetValueBasedOnMousePosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlider.MethodName.SetValueWithoutAnimation && args.Count == 1)
			{
				this.SetValueWithoutAnimation(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlider.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSlider.MethodName.UpdateHandlePosition && args.Count == 0)
			{
				this.UpdateHandlePosition();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003832 RID: 14386 RVA: 0x001A8C0C File Offset: 0x001A6E0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSlider.MethodName._Ready || (ref method) == NSlider.MethodName._GuiInput || (ref method) == NSlider.MethodName.SetValueBasedOnMousePosition || (ref method) == NSlider.MethodName.SetValueWithoutAnimation || (ref method) == NSlider.MethodName._Process || (ref method) == NSlider.MethodName.UpdateHandlePosition || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003833 RID: 14387 RVA: 0x001A8C7C File Offset: 0x001A6E7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSlider.PropertyName._handle)
			{
				this._handle = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._currentHandlePosition)
			{
				this._currentHandlePosition = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._currentVelocity)
			{
				this._currentVelocity = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003834 RID: 14388 RVA: 0x001A8D00 File Offset: 0x001A6F00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSlider.PropertyName._handle)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._handle);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._currentHandlePosition)
			{
				value = VariantUtils.CreateFrom<float>(ref this._currentHandlePosition);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._currentVelocity)
			{
				value = VariantUtils.CreateFrom<float>(ref this._currentVelocity);
				return true;
			}
			if ((ref name) == NSlider.PropertyName._isDragging)
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
		// Token: 0x06003835 RID: 14389 RVA: 0x001A8D98 File Offset: 0x001A6F98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSlider.PropertyName._handle, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSlider.PropertyName._currentHandlePosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSlider.PropertyName._currentVelocity, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSlider.PropertyName._isDragging, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003836 RID: 14390 RVA: 0x001A8E30 File Offset: 0x001A7030
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSlider.PropertyName._handle, Variant.From<Control>(ref this._handle));
			info.AddProperty(NSlider.PropertyName._currentHandlePosition, Variant.From<float>(ref this._currentHandlePosition));
			info.AddProperty(NSlider.PropertyName._currentVelocity, Variant.From<float>(ref this._currentVelocity));
			info.AddProperty(NSlider.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
			info.AddSignalEventDelegate(NSlider.SignalName.MouseReleased, this.backing_MouseReleased);
			info.AddSignalEventDelegate(NSlider.SignalName.MousePressed, this.backing_MousePressed);
		}

		/// <inheritdoc />
		// Token: 0x06003837 RID: 14391 RVA: 0x001A8EC0 File Offset: 0x001A70C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSlider.PropertyName._handle, ref variant))
			{
				this._handle = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSlider.PropertyName._currentHandlePosition, ref variant2))
			{
				this._currentHandlePosition = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSlider.PropertyName._currentVelocity, ref variant3))
			{
				this._currentVelocity = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSlider.PropertyName._isDragging, ref variant4))
			{
				this._isDragging = variant4.As<bool>();
			}
			NSlider.MouseReleasedEventHandler mouseReleasedEventHandler;
			if (info.TryGetSignalEventDelegate<NSlider.MouseReleasedEventHandler>(NSlider.SignalName.MouseReleased, ref mouseReleasedEventHandler))
			{
				this.backing_MouseReleased = mouseReleasedEventHandler;
			}
			NSlider.MousePressedEventHandler mousePressedEventHandler;
			if (info.TryGetSignalEventDelegate<NSlider.MousePressedEventHandler>(NSlider.SignalName.MousePressed, ref mousePressedEventHandler))
			{
				this.backing_MousePressed = mousePressedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003838 RID: 14392 RVA: 0x001A8F74 File Offset: 0x001A7174
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NSlider.SignalName.MouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSlider.SignalName.MousePressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NSlider.MouseReleasedEventHandler" />
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06003839 RID: 14393 RVA: 0x001A9047 File Offset: 0x001A7247
		// (remove) Token: 0x0600383A RID: 14394 RVA: 0x001A9060 File Offset: 0x001A7260
		public event NSlider.MouseReleasedEventHandler MouseReleased
		{
			add
			{
				this.backing_MouseReleased = (NSlider.MouseReleasedEventHandler)Delegate.Combine(this.backing_MouseReleased, value);
			}
			remove
			{
				this.backing_MouseReleased = (NSlider.MouseReleasedEventHandler)Delegate.Remove(this.backing_MouseReleased, value);
			}
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x001A9079 File Offset: 0x001A7279
		protected void EmitSignalMouseReleased(InputEvent inputEvent)
		{
			base.EmitSignal(NSlider.SignalName.MouseReleased, new Variant[] { inputEvent });
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.GodotExtensions.NSlider.MousePressedEventHandler" />
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600383C RID: 14396 RVA: 0x001A909A File Offset: 0x001A729A
		// (remove) Token: 0x0600383D RID: 14397 RVA: 0x001A90B3 File Offset: 0x001A72B3
		public event NSlider.MousePressedEventHandler MousePressed
		{
			add
			{
				this.backing_MousePressed = (NSlider.MousePressedEventHandler)Delegate.Combine(this.backing_MousePressed, value);
			}
			remove
			{
				this.backing_MousePressed = (NSlider.MousePressedEventHandler)Delegate.Remove(this.backing_MousePressed, value);
			}
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x001A90CC File Offset: 0x001A72CC
		protected void EmitSignalMousePressed(InputEvent inputEvent)
		{
			base.EmitSignal(NSlider.SignalName.MousePressed, new Variant[] { inputEvent });
		}

		/// <inheritdoc />
		// Token: 0x0600383F RID: 14399 RVA: 0x001A90F0 File Offset: 0x001A72F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if ((ref signal) == NSlider.SignalName.MouseReleased && args.Count == 1)
			{
				NSlider.MouseReleasedEventHandler mouseReleasedEventHandler = this.backing_MouseReleased;
				if (mouseReleasedEventHandler == null)
				{
					return;
				}
				mouseReleasedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
			else
			{
				if (!((ref signal) == NSlider.SignalName.MousePressed) || args.Count != 1)
				{
					base.RaiseGodotClassSignalCallbacks(ref signal, args);
					return;
				}
				NSlider.MousePressedEventHandler mousePressedEventHandler = this.backing_MousePressed;
				if (mousePressedEventHandler == null)
				{
					return;
				}
				mousePressedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
				return;
			}
		}

		/// <inheritdoc />
		// Token: 0x06003840 RID: 14400 RVA: 0x001A916F File Offset: 0x001A736F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NSlider.SignalName.MouseReleased || (ref signal) == NSlider.SignalName.MousePressed || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040015C8 RID: 5576
		[Nullable(1)]
		private Control _handle;

		// Token: 0x040015C9 RID: 5577
		private float _currentHandlePosition;

		// Token: 0x040015CA RID: 5578
		private float _currentVelocity;

		// Token: 0x040015CB RID: 5579
		private bool _isDragging;

		// Token: 0x040015CC RID: 5580
		private NSlider.MouseReleasedEventHandler backing_MouseReleased;

		// Token: 0x040015CD RID: 5581
		private NSlider.MousePressedEventHandler backing_MousePressed;

		// Token: 0x02001784 RID: 6020
		// (Invoke) Token: 0x0600A6ED RID: 42733
		[Signal]
		public delegate void MouseReleasedEventHandler(InputEvent inputEvent);

		// Token: 0x02001785 RID: 6021
		// (Invoke) Token: 0x0600A6F1 RID: 42737
		[Signal]
		public delegate void MousePressedEventHandler(InputEvent inputEvent);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001786 RID: 6022
		public class MethodName : Range.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BD1 RID: 23505
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005BD2 RID: 23506
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'SetValueBasedOnMousePosition' method.
			/// </summary>
			// Token: 0x04005BD3 RID: 23507
			public static readonly StringName SetValueBasedOnMousePosition = "SetValueBasedOnMousePosition";

			/// <summary>
			/// Cached name for the 'SetValueWithoutAnimation' method.
			/// </summary>
			// Token: 0x04005BD4 RID: 23508
			public static readonly StringName SetValueWithoutAnimation = "SetValueWithoutAnimation";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005BD5 RID: 23509
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateHandlePosition' method.
			/// </summary>
			// Token: 0x04005BD6 RID: 23510
			public static readonly StringName UpdateHandlePosition = "UpdateHandlePosition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001787 RID: 6023
		public class PropertyName : Range.PropertyName
		{
			/// <summary>
			/// Cached name for the '_handle' field.
			/// </summary>
			// Token: 0x04005BD7 RID: 23511
			public static readonly StringName _handle = "_handle";

			/// <summary>
			/// Cached name for the '_currentHandlePosition' field.
			/// </summary>
			// Token: 0x04005BD8 RID: 23512
			public static readonly StringName _currentHandlePosition = "_currentHandlePosition";

			/// <summary>
			/// Cached name for the '_currentVelocity' field.
			/// </summary>
			// Token: 0x04005BD9 RID: 23513
			public static readonly StringName _currentVelocity = "_currentVelocity";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x04005BDA RID: 23514
			public static readonly StringName _isDragging = "_isDragging";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001788 RID: 6024
		public class SignalName : Range.SignalName
		{
			/// <summary>
			/// Cached name for the 'MouseReleased' signal.
			/// </summary>
			// Token: 0x04005BDB RID: 23515
			public static readonly StringName MouseReleased = "MouseReleased";

			/// <summary>
			/// Cached name for the 'MousePressed' signal.
			/// </summary>
			// Token: 0x04005BDC RID: 23516
			public static readonly StringName MousePressed = "MousePressed";
		}
	}
}
