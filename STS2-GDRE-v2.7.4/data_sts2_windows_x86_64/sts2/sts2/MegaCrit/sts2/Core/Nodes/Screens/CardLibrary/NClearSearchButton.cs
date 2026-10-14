using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary
{
	// Token: 0x02000358 RID: 856
	[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NClearSearchButton.cs")]
	public class NClearSearchButton : NButton
	{
		// Token: 0x06003189 RID: 12681 RVA: 0x001703F1 File Offset: 0x0016E5F1
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<Control>("Image");
			this._image.PivotOffset = this._image.Size / 2f;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x00170430 File Offset: 0x0016E630
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", Vector2.One * 1.1f, 0.05);
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x001704A0 File Offset: 0x0016E6A0
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "scale", Vector2.One, 0.3);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600318C RID: 12684 RVA: 0x00170508 File Offset: 0x0016E708
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NClearSearchButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClearSearchButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NClearSearchButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600318D RID: 12685 RVA: 0x001705AC File Offset: 0x0016E7AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NClearSearchButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClearSearchButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NClearSearchButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600318E RID: 12686 RVA: 0x00170631 File Offset: 0x0016E831
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NClearSearchButton.MethodName._Ready || (ref method) == NClearSearchButton.MethodName.OnFocus || (ref method) == NClearSearchButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600318F RID: 12687 RVA: 0x00170667 File Offset: 0x0016E867
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NClearSearchButton.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NClearSearchButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003190 RID: 12688 RVA: 0x001706A8 File Offset: 0x0016E8A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NClearSearchButton.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NClearSearchButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003191 RID: 12689 RVA: 0x00170700 File Offset: 0x0016E900
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NClearSearchButton.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NClearSearchButton.PropertyName._image, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003192 RID: 12690 RVA: 0x00170756 File Offset: 0x0016E956
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NClearSearchButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NClearSearchButton.PropertyName._image, Variant.From<Control>(ref this._image));
		}

		/// <inheritdoc />
		// Token: 0x06003193 RID: 12691 RVA: 0x0017078C File Offset: 0x0016E98C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NClearSearchButton.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NClearSearchButton.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<Control>();
			}
		}

		// Token: 0x04001351 RID: 4945
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001352 RID: 4946
		[Nullable(1)]
		private Control _image;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001646 RID: 5702
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040056C7 RID: 22215
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040056C8 RID: 22216
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040056C9 RID: 22217
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001647 RID: 5703
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040056CA RID: 22218
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x040056CB RID: 22219
			public static readonly StringName _image = "_image";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001648 RID: 5704
		public new class SignalName : NButton.SignalName
		{
		}
	}
}
