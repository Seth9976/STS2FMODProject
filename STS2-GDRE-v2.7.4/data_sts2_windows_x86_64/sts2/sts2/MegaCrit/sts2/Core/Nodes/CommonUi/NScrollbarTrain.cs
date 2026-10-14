using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	// Token: 0x020003EB RID: 1003
	[ScriptPath("res://src/Core/Nodes/CommonUi/NScrollbarTrain.cs")]
	public class NScrollbarTrain : TextureRect
	{
		// Token: 0x06003D2E RID: 15662 RVA: 0x001D5190 File Offset: 0x001D3390
		public override void _Ready()
		{
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnMouseEntered)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnMouseExited)), 0U);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x001D51CE File Offset: 0x001D33CE
		private void OnMouseEntered()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			base.Scale = Vector2.One * 1.15f;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x001D51F8 File Offset: 0x001D33F8
		private void OnMouseExited()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.5).SetEase(1L).SetTrans(5L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D31 RID: 15665 RVA: 0x001D525C File Offset: 0x001D345C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NScrollbarTrain.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollbarTrain.MethodName.OnMouseEntered, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScrollbarTrain.MethodName.OnMouseExited, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D32 RID: 15666 RVA: 0x001D5300 File Offset: 0x001D3500
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScrollbarTrain.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbarTrain.MethodName.OnMouseEntered && args.Count == 0)
			{
				this.OnMouseEntered();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScrollbarTrain.MethodName.OnMouseExited && args.Count == 0)
			{
				this.OnMouseExited();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003D33 RID: 15667 RVA: 0x001D5385 File Offset: 0x001D3585
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScrollbarTrain.MethodName._Ready || (ref method) == NScrollbarTrain.MethodName.OnMouseEntered || (ref method) == NScrollbarTrain.MethodName.OnMouseExited || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003D34 RID: 15668 RVA: 0x001D53BB File Offset: 0x001D35BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScrollbarTrain.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003D35 RID: 15669 RVA: 0x001D53E0 File Offset: 0x001D35E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScrollbarTrain.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003D36 RID: 15670 RVA: 0x001D540C File Offset: 0x001D360C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NScrollbarTrain.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003D37 RID: 15671 RVA: 0x001D5441 File Offset: 0x001D3641
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NScrollbarTrain.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003D38 RID: 15672 RVA: 0x001D5460 File Offset: 0x001D3660
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScrollbarTrain.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x040017E1 RID: 6113
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001881 RID: 6273
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005FD2 RID: 24530
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnMouseEntered' method.
			/// </summary>
			// Token: 0x04005FD3 RID: 24531
			public static readonly StringName OnMouseEntered = "OnMouseEntered";

			/// <summary>
			/// Cached name for the 'OnMouseExited' method.
			/// </summary>
			// Token: 0x04005FD4 RID: 24532
			public static readonly StringName OnMouseExited = "OnMouseExited";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001882 RID: 6274
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005FD5 RID: 24533
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001883 RID: 6275
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}
