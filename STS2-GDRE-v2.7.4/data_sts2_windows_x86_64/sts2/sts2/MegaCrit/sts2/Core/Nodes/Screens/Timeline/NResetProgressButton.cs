using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	// Token: 0x02000262 RID: 610
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NResetProgressButton.cs")]
	public class NResetProgressButton : Control
	{
		// Token: 0x06001FE8 RID: 8168 RVA: 0x000D7560 File Offset: 0x000D5760
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnMouseEntered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnMouseExited)), 0U);
			base.Connect(Control.SignalName.GuiInput, Callable.From<InputEvent>(new Action<InputEvent>(this.OnGuiInput)), 0U);
			this._disclaimer = base.GetNode<MegaLabel>("%CloseScreenDisclaimer");
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000D75DD File Offset: 0x000D57DD
		private void OnMouseExited()
		{
			base.Scale = Vector2.One;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000D75EA File Offset: 0x000D57EA
		private void OnMouseEntered()
		{
			base.Scale = Vector2.One * 1.1f;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000D7604 File Offset: 0x000D5804
		[NullableContext(1)]
		private void OnGuiInput(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && !inputEventMouseButton.Pressed)
			{
				SaveManager.Instance.ResetTimelineProgress();
				this._disclaimer.Visible = true;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FEC RID: 8172 RVA: 0x000D7644 File Offset: 0x000D5844
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NResetProgressButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetProgressButton.MethodName.OnMouseExited, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetProgressButton.MethodName.OnMouseEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NResetProgressButton.MethodName.OnGuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FED RID: 8173 RVA: 0x000D7748 File Offset: 0x000D5948
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NResetProgressButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetProgressButton.MethodName.OnMouseExited && args.Count == 0)
			{
				this.OnMouseExited();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetProgressButton.MethodName.OnMouseEntered && args.Count == 0)
			{
				this.OnMouseEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NResetProgressButton.MethodName.OnGuiInput && args.Count == 1)
			{
				this.OnGuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001FEE RID: 8174 RVA: 0x000D7800 File Offset: 0x000D5A00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NResetProgressButton.MethodName._Ready || (ref method) == NResetProgressButton.MethodName.OnMouseExited || (ref method) == NResetProgressButton.MethodName.OnMouseEntered || (ref method) == NResetProgressButton.MethodName.OnGuiInput || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001FEF RID: 8175 RVA: 0x000D7850 File Offset: 0x000D5A50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NResetProgressButton.PropertyName._disclaimer)
			{
				this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001FF0 RID: 8176 RVA: 0x000D7875 File Offset: 0x000D5A75
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NResetProgressButton.PropertyName._disclaimer)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FF1 RID: 8177 RVA: 0x000D78A0 File Offset: 0x000D5AA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NResetProgressButton.PropertyName._disclaimer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FF2 RID: 8178 RVA: 0x000D78D5 File Offset: 0x000D5AD5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NResetProgressButton.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
		}

		/// <inheritdoc />
		// Token: 0x06001FF3 RID: 8179 RVA: 0x000D78F4 File Offset: 0x000D5AF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NResetProgressButton.PropertyName._disclaimer, ref variant))
			{
				this._disclaimer = variant.As<MegaLabel>();
			}
		}

		// Token: 0x04000C7F RID: 3199
		[Nullable(1)]
		private MegaLabel _disclaimer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B7 RID: 4791
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040048E5 RID: 18661
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnMouseExited' method.
			/// </summary>
			// Token: 0x040048E6 RID: 18662
			public static readonly StringName OnMouseExited = "OnMouseExited";

			/// <summary>
			/// Cached name for the 'OnMouseEntered' method.
			/// </summary>
			// Token: 0x040048E7 RID: 18663
			public static readonly StringName OnMouseEntered = "OnMouseEntered";

			/// <summary>
			/// Cached name for the 'OnGuiInput' method.
			/// </summary>
			// Token: 0x040048E8 RID: 18664
			public static readonly StringName OnGuiInput = "OnGuiInput";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B8 RID: 4792
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_disclaimer' field.
			/// </summary>
			// Token: 0x040048E9 RID: 18665
			public static readonly StringName _disclaimer = "_disclaimer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B9 RID: 4793
		public class SignalName : Control.SignalName
		{
		}
	}
}
