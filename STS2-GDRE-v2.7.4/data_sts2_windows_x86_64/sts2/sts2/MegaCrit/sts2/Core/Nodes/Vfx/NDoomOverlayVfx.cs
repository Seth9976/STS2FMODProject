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
	// Token: 0x0200019F RID: 415
	[ScriptPath("res://src/Core/Nodes/Vfx/NDoomOverlayVfx.cs")]
	public class NDoomOverlayVfx : BackBufferCopy
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x0007B9C4 File Offset: 0x00079BC4
		[NullableContext(2)]
		public static NDoomOverlayVfx GetOrCreate()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (NDoomOverlayVfx._instance != null)
			{
				NDoomOverlayVfx._instance.PlayVfx();
			}
			else
			{
				NDoomOverlayVfx._instance = PreloadManager.Cache.GetScene(NDoomOverlayVfx._scenePath).Instantiate<NDoomOverlayVfx>(0L);
			}
			if (!GodotObject.IsInstanceValid(NDoomOverlayVfx._instance))
			{
				return null;
			}
			return NDoomOverlayVfx._instance;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0007BA1C File Offset: 0x00079C1C
		public override void _Ready()
		{
			base.Modulate = Colors.Transparent;
			base.GetNode<Control>("%Rect").SetDeferred(Control.PropertyName.Size, base.GetViewportRect().Size);
			this.PlayVfx();
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0007BA68 File Offset: 0x00079C68
		private void PlayVfx()
		{
			if (!GodotObject.IsInstanceValid(this))
			{
				return;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 1f, 1.0).SetEase(1L).SetTrans(5L);
			this._tween.TweenInterval(1.2000000476837158);
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			this._tween.TweenCallback(Callable.From(delegate
			{
				NDoomOverlayVfx._instance = null;
			}));
			this._tween.Finished += this.OnTweenFinished;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0007BB59 File Offset: 0x00079D59
		private void OnTweenFinished()
		{
			this.QueueFreeSafely();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0007BB61 File Offset: 0x00079D61
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
		// Token: 0x06001477 RID: 5239 RVA: 0x0007BB94 File Offset: 0x00079D94
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NDoomOverlayVfx.MethodName.GetOrCreate, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("BackBufferCopy"), false), 33L, null, null),
				new MethodInfo(NDoomOverlayVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomOverlayVfx.MethodName.PlayVfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomOverlayVfx.MethodName.OnTweenFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomOverlayVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001478 RID: 5240 RVA: 0x0007BCA0 File Offset: 0x00079EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDoomOverlayVfx.MethodName.GetOrCreate && args.Count == 0)
			{
				NDoomOverlayVfx orCreate = NDoomOverlayVfx.GetOrCreate();
				ret = VariantUtils.CreateFrom<NDoomOverlayVfx>(ref orCreate);
				return true;
			}
			if ((ref method) == NDoomOverlayVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomOverlayVfx.MethodName.PlayVfx && args.Count == 0)
			{
				this.PlayVfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomOverlayVfx.MethodName.OnTweenFinished && args.Count == 0)
			{
				this.OnTweenFinished();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomOverlayVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0007BD78 File Offset: 0x00079F78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDoomOverlayVfx.MethodName.GetOrCreate && args.Count == 0)
			{
				NDoomOverlayVfx orCreate = NDoomOverlayVfx.GetOrCreate();
				ret = VariantUtils.CreateFrom<NDoomOverlayVfx>(ref orCreate);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600147A RID: 5242 RVA: 0x0007BDB8 File Offset: 0x00079FB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDoomOverlayVfx.MethodName.GetOrCreate || (ref method) == NDoomOverlayVfx.MethodName._Ready || (ref method) == NDoomOverlayVfx.MethodName.PlayVfx || (ref method) == NDoomOverlayVfx.MethodName.OnTweenFinished || (ref method) == NDoomOverlayVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600147B RID: 5243 RVA: 0x0007BE17 File Offset: 0x0007A017
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDoomOverlayVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600147C RID: 5244 RVA: 0x0007BE3C File Offset: 0x0007A03C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDoomOverlayVfx.PropertyName._tween)
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
		// Token: 0x0600147D RID: 5245 RVA: 0x0007BE68 File Offset: 0x0007A068
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDoomOverlayVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600147E RID: 5246 RVA: 0x0007BE9D File Offset: 0x0007A09D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDoomOverlayVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600147F RID: 5247 RVA: 0x0007BEBC File Offset: 0x0007A0BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDoomOverlayVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04000748 RID: 1864
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/doom_overlay_vfx");

		// Token: 0x04000749 RID: 1865
		[Nullable(2)]
		private static NDoomOverlayVfx _instance;

		// Token: 0x0400074A RID: 1866
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001003 RID: 4099
		public class MethodName : BackBufferCopy.MethodName
		{
			/// <summary>
			/// Cached name for the 'GetOrCreate' method.
			/// </summary>
			// Token: 0x04003F87 RID: 16263
			public static readonly StringName GetOrCreate = "GetOrCreate";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F88 RID: 16264
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PlayVfx' method.
			/// </summary>
			// Token: 0x04003F89 RID: 16265
			public static readonly StringName PlayVfx = "PlayVfx";

			/// <summary>
			/// Cached name for the 'OnTweenFinished' method.
			/// </summary>
			// Token: 0x04003F8A RID: 16266
			public static readonly StringName OnTweenFinished = "OnTweenFinished";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F8B RID: 16267
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001004 RID: 4100
		public class PropertyName : BackBufferCopy.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F8C RID: 16268
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001005 RID: 4101
		public class SignalName : BackBufferCopy.SignalName
		{
		}
	}
}
