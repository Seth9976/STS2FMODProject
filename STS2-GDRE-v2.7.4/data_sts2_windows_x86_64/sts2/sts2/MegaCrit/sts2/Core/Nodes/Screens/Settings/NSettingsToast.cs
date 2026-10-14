using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002BE RID: 702
	[Tool]
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsToast.cs")]
	public class NSettingsToast : Control
	{
		// Token: 0x06002546 RID: 9542 RVA: 0x000FCFA0 File Offset: 0x000FB1A0
		public override void _Ready()
		{
			this._label = base.GetNode<MegaRichTextLabel>("Label");
			this._originalY = base.Position.Y;
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000FCFCC File Offset: 0x000FB1CC
		[NullableContext(1)]
		public void Show(LocString locString)
		{
			this._label.SetTextAutoSize(locString.GetFormattedText());
			base.Scale = Vector2.One;
			Vector2 position = base.Position;
			position.Y = this._originalY;
			base.Position = position;
			Color modulate = base.Modulate;
			modulate.A = 1f;
			base.Modulate = modulate;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "position:y", this._originalY - 120f, 0.25).SetTrans(10L).SetEase(1L);
			this._tween.TweenInterval(1.0);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			this._tween.TweenProperty(this, "scale", Vector2.One * 0.8f, 0.5);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002548 RID: 9544 RVA: 0x000FD0FC File Offset: 0x000FB2FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NSettingsToast.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002549 RID: 9545 RVA: 0x000FD140 File Offset: 0x000FB340
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsToast.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600254A RID: 9546 RVA: 0x000FD170 File Offset: 0x000FB370
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsToast.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600254B RID: 9547 RVA: 0x000FD188 File Offset: 0x000FB388
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsToast.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NSettingsToast.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSettingsToast.PropertyName._originalY)
			{
				this._originalY = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600254C RID: 9548 RVA: 0x000FD1F0 File Offset: 0x000FB3F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsToast.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NSettingsToast.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSettingsToast.PropertyName._originalY)
			{
				value = VariantUtils.CreateFrom<float>(ref this._originalY);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600254D RID: 9549 RVA: 0x000FD268 File Offset: 0x000FB468
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsToast.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsToast.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSettingsToast.PropertyName._originalY, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600254E RID: 9550 RVA: 0x000FD2E0 File Offset: 0x000FB4E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsToast.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NSettingsToast.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSettingsToast.PropertyName._originalY, Variant.From<float>(ref this._originalY));
		}

		/// <inheritdoc />
		// Token: 0x0600254F RID: 9551 RVA: 0x000FD338 File Offset: 0x000FB538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsToast.PropertyName._label, ref variant))
			{
				this._label = variant.As<MegaRichTextLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsToast.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsToast.PropertyName._originalY, ref variant3))
			{
				this._originalY = variant3.As<float>();
			}
		}

		// Token: 0x04000DEC RID: 3564
		[Nullable(1)]
		private MegaRichTextLabel _label;

		// Token: 0x04000DED RID: 3565
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000DEE RID: 3566
		private float _originalY;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F7 RID: 5111
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C36 RID: 19510
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F8 RID: 5112
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004C37 RID: 19511
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004C38 RID: 19512
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_originalY' field.
			/// </summary>
			// Token: 0x04004C39 RID: 19513
			public static readonly StringName _originalY = "_originalY";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F9 RID: 5113
		public class SignalName : Control.SignalName
		{
		}
	}
}
