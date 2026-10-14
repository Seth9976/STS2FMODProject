using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
	/// <summary>
	/// VFX that plays when a remote player double-clicks a map node.
	/// </summary>
	// Token: 0x020001D1 RID: 465
	[ScriptPath("res://src/Core/Nodes/Vfx/NMapPingVfx.cs")]
	public class NMapPingVfx : Control
	{
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x000913E0 File Offset: 0x0008F5E0
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NMapPingVfx._scenePath);
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000913EC File Offset: 0x0008F5EC
		[NullableContext(2)]
		public static NMapPingVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMapPingVfx._scenePath).Instantiate<NMapPingVfx>(0L);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0009141A File Offset: 0x0008F61A
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayAnim());
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00091428 File Offset: 0x0008F628
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0009143C File Offset: 0x0008F63C
		[NullableContext(1)]
		private async Task PlayAnim()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", Vector2.One * 2.5f, 0.699999988079071).SetTrans(5L).SetEase(1L);
			Tween tween = this._tween;
			NodePath nodePath = "modulate";
			Color modulate = base.Modulate;
			modulate.A = 0f;
			tween.TweenProperty(this, nodePath, modulate, 0.15000000596046448).SetDelay(0.15000000596046448);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001773 RID: 6003 RVA: 0x00091480 File Offset: 0x0008F680
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapPingVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMapPingVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPingVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001774 RID: 6004 RVA: 0x00091530 File Offset: 0x0008F730
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPingVfx.MethodName.Create && args.Count == 0)
			{
				NMapPingVfx nmapPingVfx = NMapPingVfx.Create();
				ret = VariantUtils.CreateFrom<NMapPingVfx>(ref nmapPingVfx);
				return true;
			}
			if ((ref method) == NMapPingVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPingVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000915BC File Offset: 0x0008F7BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPingVfx.MethodName.Create && args.Count == 0)
			{
				NMapPingVfx nmapPingVfx = NMapPingVfx.Create();
				ret = VariantUtils.CreateFrom<NMapPingVfx>(ref nmapPingVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001776 RID: 6006 RVA: 0x000915FC File Offset: 0x0008F7FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapPingVfx.MethodName.Create || (ref method) == NMapPingVfx.MethodName._Ready || (ref method) == NMapPingVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001777 RID: 6007 RVA: 0x00091632 File Offset: 0x0008F832
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapPingVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001778 RID: 6008 RVA: 0x00091657 File Offset: 0x0008F857
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapPingVfx.PropertyName._tween)
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
		// Token: 0x06001779 RID: 6009 RVA: 0x00091684 File Offset: 0x0008F884
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapPingVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600177A RID: 6010 RVA: 0x000916B9 File Offset: 0x0008F8B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapPingVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600177B RID: 6011 RVA: 0x000916D8 File Offset: 0x0008F8D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapPingVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04000862 RID: 2146
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/map_ping_vfx");

		// Token: 0x04000863 RID: 2147
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010BB RID: 4283
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040041A5 RID: 16805
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041A6 RID: 16806
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040041A7 RID: 16807
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010BC RID: 4284
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040041A8 RID: 16808
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010BD RID: 4285
		public class SignalName : Control.SignalName
		{
		}
	}
}
