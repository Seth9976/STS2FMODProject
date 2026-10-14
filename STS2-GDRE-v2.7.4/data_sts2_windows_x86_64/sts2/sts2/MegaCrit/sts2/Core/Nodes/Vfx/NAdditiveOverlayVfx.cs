using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// A simple vfx which renders a colorRect to tint the entire screen using an additive blend.
	/// </summary>
	// Token: 0x0200017E RID: 382
	[ScriptPath("res://src/Core/Nodes/Vfx/NAdditiveOverlayVfx.cs")]
	public class NAdditiveOverlayVfx : ColorRect
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x0006C31C File Offset: 0x0006A51C
		[NullableContext(2)]
		public static NAdditiveOverlayVfx Create(VfxColor vfxColor = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NAdditiveOverlayVfx nadditiveOverlayVfx = PreloadManager.Cache.GetScene(NAdditiveOverlayVfx._scenePath).Instantiate<NAdditiveOverlayVfx>(0L);
			nadditiveOverlayVfx._vfxColor = vfxColor;
			return nadditiveOverlayVfx;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0006C354 File Offset: 0x0006A554
		public override void _Ready()
		{
			this.SetVfxColor();
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0.1f, 0.5).SetEase(1L).SetTrans(5L);
			this._tween.TweenInterval(0.5);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			this._tween.Finished += this.OnTweenFinished;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0006C404 File Offset: 0x0006A604
		private void SetVfxColor()
		{
			switch (this._vfxColor)
			{
			case VfxColor.Red:
			case VfxColor.Black:
				return;
			case VfxColor.Green:
				base.Modulate = new Color("00ff1500");
				return;
			case VfxColor.Blue:
				base.Modulate = new Color("001aff00");
				return;
			case VfxColor.Purple:
				base.Modulate = new Color("b300ff00");
				return;
			case VfxColor.White:
				base.Modulate = new Color("ffffff00");
				return;
			case VfxColor.Cyan:
				base.Modulate = new Color("00fffb00");
				return;
			case VfxColor.Gold:
				base.Modulate = new Color("b17e0000");
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0006C4AC File Offset: 0x0006A6AC
		private void OnTweenFinished()
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0006C4B4 File Offset: 0x0006A6B4
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			if (this._tween != null)
			{
				this._tween.Finished -= this.OnTweenFinished;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001262 RID: 4706 RVA: 0x0006C4E8 File Offset: 0x0006A6E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NAdditiveOverlayVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("ColorRect"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "vfxColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAdditiveOverlayVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAdditiveOverlayVfx.MethodName.SetVfxColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAdditiveOverlayVfx.MethodName.OnTweenFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAdditiveOverlayVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001263 RID: 4707 RVA: 0x0006C61C File Offset: 0x0006A81C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAdditiveOverlayVfx.MethodName.Create && args.Count == 1)
			{
				NAdditiveOverlayVfx nadditiveOverlayVfx = NAdditiveOverlayVfx.Create(VariantUtils.ConvertTo<VfxColor>(args[0]));
				ret = VariantUtils.CreateFrom<NAdditiveOverlayVfx>(ref nadditiveOverlayVfx);
				return true;
			}
			if ((ref method) == NAdditiveOverlayVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAdditiveOverlayVfx.MethodName.SetVfxColor && args.Count == 0)
			{
				this.SetVfxColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAdditiveOverlayVfx.MethodName.OnTweenFinished && args.Count == 0)
			{
				this.OnTweenFinished();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAdditiveOverlayVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0006C700 File Offset: 0x0006A900
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAdditiveOverlayVfx.MethodName.Create && args.Count == 1)
			{
				NAdditiveOverlayVfx nadditiveOverlayVfx = NAdditiveOverlayVfx.Create(VariantUtils.ConvertTo<VfxColor>(args[0]));
				ret = VariantUtils.CreateFrom<NAdditiveOverlayVfx>(ref nadditiveOverlayVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001265 RID: 4709 RVA: 0x0006C750 File Offset: 0x0006A950
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAdditiveOverlayVfx.MethodName.Create || (ref method) == NAdditiveOverlayVfx.MethodName._Ready || (ref method) == NAdditiveOverlayVfx.MethodName.SetVfxColor || (ref method) == NAdditiveOverlayVfx.MethodName.OnTweenFinished || (ref method) == NAdditiveOverlayVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001266 RID: 4710 RVA: 0x0006C7AF File Offset: 0x0006A9AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAdditiveOverlayVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			if ((ref name) == NAdditiveOverlayVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001267 RID: 4711 RVA: 0x0006C7F0 File Offset: 0x0006A9F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAdditiveOverlayVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			if ((ref name) == NAdditiveOverlayVfx.PropertyName._tween)
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
		// Token: 0x06001268 RID: 4712 RVA: 0x0006C848 File Offset: 0x0006AA48
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NAdditiveOverlayVfx.PropertyName._vfxColor, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAdditiveOverlayVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001269 RID: 4713 RVA: 0x0006C89D File Offset: 0x0006AA9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAdditiveOverlayVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
			info.AddProperty(NAdditiveOverlayVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600126A RID: 4714 RVA: 0x0006C8D4 File Offset: 0x0006AAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAdditiveOverlayVfx.PropertyName._vfxColor, ref variant))
			{
				this._vfxColor = variant.As<VfxColor>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAdditiveOverlayVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
		}

		// Token: 0x04000679 RID: 1657
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/additive_overlay_vfx");

		// Token: 0x0400067A RID: 1658
		private VfxColor _vfxColor;

		// Token: 0x0400067B RID: 1659
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F88 RID: 3976
		public class MethodName : ColorRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003DFB RID: 15867
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003DFC RID: 15868
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetVfxColor' method.
			/// </summary>
			// Token: 0x04003DFD RID: 15869
			public static readonly StringName SetVfxColor = "SetVfxColor";

			/// <summary>
			/// Cached name for the 'OnTweenFinished' method.
			/// </summary>
			// Token: 0x04003DFE RID: 15870
			public static readonly StringName OnTweenFinished = "OnTweenFinished";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003DFF RID: 15871
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F89 RID: 3977
		public class PropertyName : ColorRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x04003E00 RID: 15872
			public static readonly StringName _vfxColor = "_vfxColor";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003E01 RID: 15873
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F8A RID: 3978
		public class SignalName : ColorRect.SignalName
		{
		}
	}
}
