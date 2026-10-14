using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001F4 RID: 500
	[ScriptPath("res://src/Core/Nodes/Vfx/NSmokyVignetteVfx.cs")]
	public class NSmokyVignetteVfx : Control
	{
		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x000A10FC File Offset: 0x0009F2FC
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn");
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000A1108 File Offset: 0x0009F308
		[NullableContext(2)]
		public static NSmokyVignetteVfx Create(Color tint, Color highlightColor)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSmokyVignetteVfx nsmokyVignetteVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn").Instantiate<NSmokyVignetteVfx>(0L);
			nsmokyVignetteVfx.Modulate = tint;
			nsmokyVignetteVfx._targetColor = tint;
			nsmokyVignetteVfx._highlightColor = highlightColor;
			return nsmokyVignetteVfx;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000A114B File Offset: 0x0009F34B
		public override void _Ready()
		{
			this._highlights = base.GetNode<Control>("Highlights");
			this._highlights.Modulate = this._highlightColor;
			TaskHelper.RunSafely(this.Animate(true));
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x000A1181 File Offset: 0x0009F381
		public void Reset(Color tint, Color highlightColor)
		{
			this._targetColor = tint;
			this._highlightColor = highlightColor;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			TaskHelper.RunSafely(this.Animate(false));
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000A11B0 File Offset: 0x0009F3B0
		[NullableContext(1)]
		private async Task Animate(bool fadeIn)
		{
			this._tween = base.CreateTween().SetParallel(true);
			if (fadeIn)
			{
				this._tween.TweenProperty(this, "modulate:a", this._targetColor.A, 0.1).From(0f);
				this._tween.TweenProperty(this._highlights, "modulate:a", this._highlightColor.A, 0.1).From(0f);
			}
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 1.0).SetEase(0L).SetTrans(7L);
			this._tween.TweenProperty(this._highlights, "modulate:a", 0f, 1.0);
			while (this._tween.IsValid() && this._tween.IsRunning())
			{
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			if (this._tween.IsValid())
			{
				this.QueueFreeSafely();
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000A11FB File Offset: 0x0009F3FB
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001994 RID: 6548 RVA: 0x000A1210 File Offset: 0x0009F410
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NSmokyVignetteVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false),
					new PropertyInfo(20L, "highlightColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSmokyVignetteVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSmokyVignetteVfx.MethodName.Reset, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false),
					new PropertyInfo(20L, "highlightColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSmokyVignetteVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001995 RID: 6549 RVA: 0x000A1380 File Offset: 0x0009F580
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSmokyVignetteVfx.MethodName.Create && args.Count == 2)
			{
				NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(VariantUtils.ConvertTo<Color>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSmokyVignetteVfx>(ref nsmokyVignetteVfx);
				return true;
			}
			if ((ref method) == NSmokyVignetteVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmokyVignetteVfx.MethodName.Reset && args.Count == 2)
			{
				this.Reset(VariantUtils.ConvertTo<Color>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSmokyVignetteVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000A1468 File Offset: 0x0009F668
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSmokyVignetteVfx.MethodName.Create && args.Count == 2)
			{
				NSmokyVignetteVfx nsmokyVignetteVfx = NSmokyVignetteVfx.Create(VariantUtils.ConvertTo<Color>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSmokyVignetteVfx>(ref nsmokyVignetteVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001997 RID: 6551 RVA: 0x000A14C4 File Offset: 0x0009F6C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSmokyVignetteVfx.MethodName.Create || (ref method) == NSmokyVignetteVfx.MethodName._Ready || (ref method) == NSmokyVignetteVfx.MethodName.Reset || (ref method) == NSmokyVignetteVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001998 RID: 6552 RVA: 0x000A1514 File Offset: 0x0009F714
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSmokyVignetteVfx.PropertyName._highlights)
			{
				this._highlights = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._targetColor)
			{
				this._targetColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._highlightColor)
			{
				this._highlightColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001999 RID: 6553 RVA: 0x000A1598 File Offset: 0x0009F798
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSmokyVignetteVfx.PropertyName._highlights)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._highlights);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._targetColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._targetColor);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._highlightColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._highlightColor);
				return true;
			}
			if ((ref name) == NSmokyVignetteVfx.PropertyName._tween)
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
		// Token: 0x0600199A RID: 6554 RVA: 0x000A1630 File Offset: 0x0009F830
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSmokyVignetteVfx.PropertyName._highlights, 0L, "", 4096L, false),
				new PropertyInfo(20L, NSmokyVignetteVfx.PropertyName._targetColor, 0L, "", 4096L, false),
				new PropertyInfo(20L, NSmokyVignetteVfx.PropertyName._highlightColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSmokyVignetteVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600199B RID: 6555 RVA: 0x000A16C8 File Offset: 0x0009F8C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSmokyVignetteVfx.PropertyName._highlights, Variant.From<Control>(ref this._highlights));
			info.AddProperty(NSmokyVignetteVfx.PropertyName._targetColor, Variant.From<Color>(ref this._targetColor));
			info.AddProperty(NSmokyVignetteVfx.PropertyName._highlightColor, Variant.From<Color>(ref this._highlightColor));
			info.AddProperty(NSmokyVignetteVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600199C RID: 6556 RVA: 0x000A1734 File Offset: 0x0009F934
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._highlights, ref variant))
			{
				this._highlights = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._targetColor, ref variant2))
			{
				this._targetColor = variant2.As<Color>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._highlightColor, ref variant3))
			{
				this._highlightColor = variant3.As<Color>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSmokyVignetteVfx.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000952 RID: 2386
		[Nullable(1)]
		private const string _path = "res://scenes/vfx/whole_screen/vfx_smoky_vignette.tscn";

		// Token: 0x04000953 RID: 2387
		[Nullable(1)]
		private Control _highlights;

		// Token: 0x04000954 RID: 2388
		private Color _targetColor;

		// Token: 0x04000955 RID: 2389
		private Color _highlightColor;

		// Token: 0x04000956 RID: 2390
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200113E RID: 4414
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004342 RID: 17218
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004343 RID: 17219
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reset' method.
			/// </summary>
			// Token: 0x04004344 RID: 17220
			public static readonly StringName Reset = "Reset";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004345 RID: 17221
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200113F RID: 4415
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_highlights' field.
			/// </summary>
			// Token: 0x04004346 RID: 17222
			public static readonly StringName _highlights = "_highlights";

			/// <summary>
			/// Cached name for the '_targetColor' field.
			/// </summary>
			// Token: 0x04004347 RID: 17223
			public static readonly StringName _targetColor = "_targetColor";

			/// <summary>
			/// Cached name for the '_highlightColor' field.
			/// </summary>
			// Token: 0x04004348 RID: 17224
			public static readonly StringName _highlightColor = "_highlightColor";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004349 RID: 17225
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001140 RID: 4416
		public class SignalName : Control.SignalName
		{
		}
	}
}
