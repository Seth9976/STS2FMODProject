using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002BA RID: 698
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsSlider.cs")]
	public abstract class NSettingsSlider : Control
	{
		// Token: 0x060024FD RID: 9469 RVA: 0x000FABDC File Offset: 0x000F8DDC
		public override void _Ready()
		{
			if (base.GetType() != typeof(NSettingsSlider))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000FAC34 File Offset: 0x000F8E34
		protected virtual void ConnectSignals()
		{
			this._slider = base.GetNode<NSlider>("Slider");
			this._valueLabel = base.GetNode<MegaLabel>("SliderValue");
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
			this._slider.Connect(Range.SignalName.ValueChanged, Callable.From<double>(new Action<double>(this.OnValueChanged)), 0U);
			MegaLabel valueLabel = this._valueLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(this._slider.Value);
			defaultInterpolatedStringHandler.AppendLiteral("%");
			valueLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000FAD1C File Offset: 0x000F8F1C
		[NullableContext(1)]
		public override void _GuiInput(InputEvent input)
		{
			base._GuiInput(input);
			if (input.IsActionPressed(MegaInput.left, false, false))
			{
				this._slider.Value -= 5.0;
			}
			if (input.IsActionPressed(MegaInput.right, false, false))
			{
				this._slider.Value += 5.0;
			}
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000FAD84 File Offset: 0x000F8F84
		private void OnValueChanged(double value)
		{
			MegaLabel valueLabel = this._valueLabel;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<double>(value);
			defaultInterpolatedStringHandler.AppendLiteral("%");
			valueLabel.SetTextAutoSize(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000FADC0 File Offset: 0x000F8FC0
		private void OnFocus()
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000FADD9 File Offset: 0x000F8FD9
		private void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002503 RID: 9475 RVA: 0x000FADE8 File Offset: 0x000F8FE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSettingsSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsSlider.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsSlider.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "input", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSettingsSlider.MethodName.OnValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSettingsSlider.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsSlider.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002504 RID: 9476 RVA: 0x000FAF6C File Offset: 0x000F916C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsSlider.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsSlider.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				this.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsSlider.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsSlider.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002505 RID: 9477 RVA: 0x000FB07C File Offset: 0x000F927C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsSlider.MethodName._Ready || (ref method) == NSettingsSlider.MethodName.ConnectSignals || (ref method) == NSettingsSlider.MethodName._GuiInput || (ref method) == NSettingsSlider.MethodName.OnValueChanged || (ref method) == NSettingsSlider.MethodName.OnFocus || (ref method) == NSettingsSlider.MethodName.OnUnfocus || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002506 RID: 9478 RVA: 0x000FB0EC File Offset: 0x000F92EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsSlider.PropertyName._slider)
			{
				this._slider = VariantUtils.ConvertTo<NSlider>(ref value);
				return true;
			}
			if ((ref name) == NSettingsSlider.PropertyName._valueLabel)
			{
				this._valueLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NSettingsSlider.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002507 RID: 9479 RVA: 0x000FB154 File Offset: 0x000F9354
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsSlider.PropertyName._slider)
			{
				value = VariantUtils.CreateFrom<NSlider>(ref this._slider);
				return true;
			}
			if ((ref name) == NSettingsSlider.PropertyName._valueLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._valueLabel);
				return true;
			}
			if ((ref name) == NSettingsSlider.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002508 RID: 9480 RVA: 0x000FB1CC File Offset: 0x000F93CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsSlider.PropertyName._slider, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsSlider.PropertyName._valueLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsSlider.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002509 RID: 9481 RVA: 0x000FB244 File Offset: 0x000F9444
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsSlider.PropertyName._slider, Variant.From<NSlider>(ref this._slider));
			info.AddProperty(NSettingsSlider.PropertyName._valueLabel, Variant.From<MegaLabel>(ref this._valueLabel));
			info.AddProperty(NSettingsSlider.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x0600250A RID: 9482 RVA: 0x000FB29C File Offset: 0x000F949C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsSlider.PropertyName._slider, ref variant))
			{
				this._slider = variant.As<NSlider>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsSlider.PropertyName._valueLabel, ref variant2))
			{
				this._valueLabel = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsSlider.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
		}

		// Token: 0x04000DD2 RID: 3538
		[Nullable(1)]
		protected NSlider _slider;

		// Token: 0x04000DD3 RID: 3539
		[Nullable(1)]
		private MegaLabel _valueLabel;

		// Token: 0x04000DD4 RID: 3540
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E9 RID: 5097
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C06 RID: 19462
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004C07 RID: 19463
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04004C08 RID: 19464
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OnValueChanged' method.
			/// </summary>
			// Token: 0x04004C09 RID: 19465
			public static readonly StringName OnValueChanged = "OnValueChanged";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004C0A RID: 19466
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004C0B RID: 19467
			public static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013EA RID: 5098
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_slider' field.
			/// </summary>
			// Token: 0x04004C0C RID: 19468
			public static readonly StringName _slider = "_slider";

			/// <summary>
			/// Cached name for the '_valueLabel' field.
			/// </summary>
			// Token: 0x04004C0D RID: 19469
			public static readonly StringName _valueLabel = "_valueLabel";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004C0E RID: 19470
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013EB RID: 5099
		public class SignalName : Control.SignalName
		{
		}
	}
}
