using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect
{
	// Token: 0x02000340 RID: 832
	[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectScreenBg.cs")]
	public class NCharacterSelectScreenBg : Control
	{
		// Token: 0x06002F52 RID: 12114 RVA: 0x0015969E File Offset: 0x0015789E
		public override void _Ready()
		{
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x001596D4 File Offset: 0x001578D4
		private void OnWindowChange()
		{
			float num = Mathf.Max(1.3333334f, (float)this._window.Size.X / (float)this._window.Size.Y);
			if (num < 1.7777778f)
			{
				float num2 = (num - 1.3333334f) / 0.44444442f;
				base.Scale = Vector2.One * Mathf.Remap(Ease.CubicOut(num2), 0f, 1f, NCharacterSelectScreenBg._defaultBgScale * NCharacterSelectScreenBg._narrowBgScale, NCharacterSelectScreenBg._defaultBgScale);
				return;
			}
			base.Scale = Vector2.One * NCharacterSelectScreenBg._defaultBgScale;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F54 RID: 12116 RVA: 0x00159770 File Offset: 0x00157970
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCharacterSelectScreenBg.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCharacterSelectScreenBg.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F55 RID: 12117 RVA: 0x001597E4 File Offset: 0x001579E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCharacterSelectScreenBg.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCharacterSelectScreenBg.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002F56 RID: 12118 RVA: 0x00159844 File Offset: 0x00157A44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCharacterSelectScreenBg.MethodName._Ready || (ref method) == NCharacterSelectScreenBg.MethodName.OnWindowChange || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002F57 RID: 12119 RVA: 0x0015986B File Offset: 0x00157A6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCharacterSelectScreenBg.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002F58 RID: 12120 RVA: 0x00159890 File Offset: 0x00157A90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCharacterSelectScreenBg.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002F59 RID: 12121 RVA: 0x001598BC File Offset: 0x00157ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCharacterSelectScreenBg.PropertyName._window, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002F5A RID: 12122 RVA: 0x001598F1 File Offset: 0x00157AF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCharacterSelectScreenBg.PropertyName._window, Variant.From<Window>(ref this._window));
		}

		/// <inheritdoc />
		// Token: 0x06002F5B RID: 12123 RVA: 0x00159910 File Offset: 0x00157B10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCharacterSelectScreenBg.PropertyName._window, ref variant))
			{
				this._window = variant.As<Window>();
			}
		}

		// Token: 0x0400122D RID: 4653
		[Nullable(1)]
		private Window _window;

		// Token: 0x0400122E RID: 4654
		private const float _sixteenByNine = 1.7777778f;

		// Token: 0x0400122F RID: 4655
		private const float _fourByThree = 1.3333334f;

		// Token: 0x04001230 RID: 4656
		private static readonly float _defaultBgScale = 1.1f;

		// Token: 0x04001231 RID: 4657
		private static readonly float _narrowBgScale = 1.153f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E6 RID: 5606
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040054CA RID: 21706
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x040054CB RID: 21707
			public static readonly StringName OnWindowChange = "OnWindowChange";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E7 RID: 5607
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x040054CC RID: 21708
			public static readonly StringName _window = "_window";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015E8 RID: 5608
		public class SignalName : Control.SignalName
		{
		}
	}
}
