using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Debug
{
	// Token: 0x020003C8 RID: 968
	[ScriptPath("res://src/Core/Nodes/Debug/NFpsVisualizer.cs")]
	public class NFpsVisualizer : TextureRect
	{
		// Token: 0x06003AA5 RID: 15013 RVA: 0x001BFFF8 File Offset: 0x001BE1F8
		public override void _Ready()
		{
			if (!OS.HasFeature("editor"))
			{
				this.QueueFreeSafely();
				return;
			}
			this._label = base.GetNode<Label>("Label");
			base.Connect(NFpsVisualizer.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMouseRelease)), 0U);
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x001C004C File Offset: 0x001BE24C
		[NullableContext(1)]
		private void HandleMouseRelease(InputEvent inputEvent)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001C0054 File Offset: 0x001BE254
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && inputEventMouseButton.IsReleased())
			{
				base.EmitSignal(NFpsVisualizer.SignalName.MouseReleased, new Variant[] { inputEvent });
			}
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x001C009C File Offset: 0x001BE29C
		public override void _Process(double delta)
		{
			if (this._label == null)
			{
				return;
			}
			double framesPerSecond = Engine.GetFramesPerSecond();
			Texture2D texture2D;
			if (framesPerSecond < 58.0)
			{
				if (framesPerSecond < 50.0)
				{
					if (framesPerSecond < 30.0)
					{
						texture2D = this._sad;
					}
					else
					{
						texture2D = this._neutral;
					}
				}
				else
				{
					texture2D = this._content;
				}
			}
			else
			{
				texture2D = this._happy;
			}
			base.Texture = texture2D;
			this._label.Text = framesPerSecond.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AA9 RID: 15017 RVA: 0x001C0120 File Offset: 0x001BE320
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFpsVisualizer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFpsVisualizer.MethodName.HandleMouseRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NFpsVisualizer.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NFpsVisualizer.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003AAA RID: 15018 RVA: 0x001C0278 File Offset: 0x001BE478
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFpsVisualizer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFpsVisualizer.MethodName.HandleMouseRelease && args.Count == 1)
			{
				this.HandleMouseRelease(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFpsVisualizer.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFpsVisualizer.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003AAB RID: 15019 RVA: 0x001C034C File Offset: 0x001BE54C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFpsVisualizer.MethodName._Ready || (ref method) == NFpsVisualizer.MethodName.HandleMouseRelease || (ref method) == NFpsVisualizer.MethodName._GuiInput || (ref method) == NFpsVisualizer.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003AAC RID: 15020 RVA: 0x001C039C File Offset: 0x001BE59C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFpsVisualizer.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<Label>(ref value);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._happy)
			{
				this._happy = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._content)
			{
				this._content = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._neutral)
			{
				this._neutral = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._sad)
			{
				this._sad = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003AAD RID: 15021 RVA: 0x001C0438 File Offset: 0x001BE638
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFpsVisualizer.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<Label>(ref this._label);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._happy)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._happy);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._content)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._content);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._neutral)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._neutral);
				return true;
			}
			if ((ref name) == NFpsVisualizer.PropertyName._sad)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._sad);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AAE RID: 15022 RVA: 0x001C04F0 File Offset: 0x001BE6F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFpsVisualizer.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFpsVisualizer.PropertyName._happy, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NFpsVisualizer.PropertyName._content, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NFpsVisualizer.PropertyName._neutral, 17L, "Texture2D", 4102L, true),
				new PropertyInfo(24L, NFpsVisualizer.PropertyName._sad, 17L, "Texture2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003AAF RID: 15023 RVA: 0x001C05B0 File Offset: 0x001BE7B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFpsVisualizer.PropertyName._label, Variant.From<Label>(ref this._label));
			info.AddProperty(NFpsVisualizer.PropertyName._happy, Variant.From<Texture2D>(ref this._happy));
			info.AddProperty(NFpsVisualizer.PropertyName._content, Variant.From<Texture2D>(ref this._content));
			info.AddProperty(NFpsVisualizer.PropertyName._neutral, Variant.From<Texture2D>(ref this._neutral));
			info.AddProperty(NFpsVisualizer.PropertyName._sad, Variant.From<Texture2D>(ref this._sad));
			info.AddSignalEventDelegate(NFpsVisualizer.SignalName.MouseReleased, this.backing_MouseReleased);
		}

		/// <inheritdoc />
		// Token: 0x06003AB0 RID: 15024 RVA: 0x001C0644 File Offset: 0x001BE844
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFpsVisualizer.PropertyName._label, ref variant))
			{
				this._label = variant.As<Label>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFpsVisualizer.PropertyName._happy, ref variant2))
			{
				this._happy = variant2.As<Texture2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFpsVisualizer.PropertyName._content, ref variant3))
			{
				this._content = variant3.As<Texture2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NFpsVisualizer.PropertyName._neutral, ref variant4))
			{
				this._neutral = variant4.As<Texture2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NFpsVisualizer.PropertyName._sad, ref variant5))
			{
				this._sad = variant5.As<Texture2D>();
			}
			NFpsVisualizer.MouseReleasedEventHandler mouseReleasedEventHandler;
			if (info.TryGetSignalEventDelegate<NFpsVisualizer.MouseReleasedEventHandler>(NFpsVisualizer.SignalName.MouseReleased, ref mouseReleasedEventHandler))
			{
				this.backing_MouseReleased = mouseReleasedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003AB1 RID: 15025 RVA: 0x001C06FC File Offset: 0x001BE8FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NFpsVisualizer.SignalName.MouseReleased, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Debug.NFpsVisualizer.MouseReleasedEventHandler" />
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06003AB2 RID: 15026 RVA: 0x001C0770 File Offset: 0x001BE970
		// (remove) Token: 0x06003AB3 RID: 15027 RVA: 0x001C0789 File Offset: 0x001BE989
		public event NFpsVisualizer.MouseReleasedEventHandler MouseReleased
		{
			add
			{
				this.backing_MouseReleased = (NFpsVisualizer.MouseReleasedEventHandler)Delegate.Combine(this.backing_MouseReleased, value);
			}
			remove
			{
				this.backing_MouseReleased = (NFpsVisualizer.MouseReleasedEventHandler)Delegate.Remove(this.backing_MouseReleased, value);
			}
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x001C07A2 File Offset: 0x001BE9A2
		protected void EmitSignalMouseReleased(InputEvent inputEvent)
		{
			base.EmitSignal(NFpsVisualizer.SignalName.MouseReleased, new Variant[] { inputEvent });
		}

		/// <inheritdoc />
		// Token: 0x06003AB5 RID: 15029 RVA: 0x001C07C3 File Offset: 0x001BE9C3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NFpsVisualizer.SignalName.MouseReleased) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NFpsVisualizer.MouseReleasedEventHandler mouseReleasedEventHandler = this.backing_MouseReleased;
			if (mouseReleasedEventHandler == null)
			{
				return;
			}
			mouseReleasedEventHandler(VariantUtils.ConvertTo<InputEvent>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003AB6 RID: 15030 RVA: 0x001C0802 File Offset: 0x001BEA02
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NFpsVisualizer.SignalName.MouseReleased || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040016E6 RID: 5862
		[Nullable(2)]
		private Label _label;

		// Token: 0x040016E7 RID: 5863
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _happy;

		// Token: 0x040016E8 RID: 5864
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _content;

		// Token: 0x040016E9 RID: 5865
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _neutral;

		// Token: 0x040016EA RID: 5866
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Texture2D _sad;

		// Token: 0x040016EB RID: 5867
		private NFpsVisualizer.MouseReleasedEventHandler backing_MouseReleased;

		// Token: 0x020017FF RID: 6143
		// (Invoke) Token: 0x0600A7D3 RID: 42963
		[Signal]
		public delegate void MouseReleasedEventHandler(InputEvent inputEvent);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001800 RID: 6144
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005DCB RID: 24011
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'HandleMouseRelease' method.
			/// </summary>
			// Token: 0x04005DCC RID: 24012
			public static readonly StringName HandleMouseRelease = "HandleMouseRelease";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005DCD RID: 24013
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005DCE RID: 24014
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001801 RID: 6145
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005DCF RID: 24015
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_happy' field.
			/// </summary>
			// Token: 0x04005DD0 RID: 24016
			public static readonly StringName _happy = "_happy";

			/// <summary>
			/// Cached name for the '_content' field.
			/// </summary>
			// Token: 0x04005DD1 RID: 24017
			public static readonly StringName _content = "_content";

			/// <summary>
			/// Cached name for the '_neutral' field.
			/// </summary>
			// Token: 0x04005DD2 RID: 24018
			public static readonly StringName _neutral = "_neutral";

			/// <summary>
			/// Cached name for the '_sad' field.
			/// </summary>
			// Token: 0x04005DD3 RID: 24019
			public static readonly StringName _sad = "_sad";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001802 RID: 6146
		public class SignalName : TextureRect.SignalName
		{
			/// <summary>
			/// Cached name for the 'MouseReleased' signal.
			/// </summary>
			// Token: 0x04005DD4 RID: 24020
			public static readonly StringName MouseReleased = "MouseReleased";
		}
	}
}
