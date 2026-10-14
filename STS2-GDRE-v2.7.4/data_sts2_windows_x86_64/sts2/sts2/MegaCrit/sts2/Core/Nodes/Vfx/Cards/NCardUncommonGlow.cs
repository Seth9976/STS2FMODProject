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

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000236 RID: 566
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardUncommonGlow.cs")]
	public class NCardUncommonGlow : GpuParticles2D
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000BB609 File Offset: 0x000B9809
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardUncommonGlow._scenePath);
			}
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000BB615 File Offset: 0x000B9815
		[NullableContext(2)]
		public static NCardUncommonGlow Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCardUncommonGlow._scenePath).Instantiate<NCardUncommonGlow>(0L);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000BB638 File Offset: 0x000B9838
		public override void _Ready()
		{
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0.9f, 1.0).SetEase(0L).SetTrans(7L);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000BB68C File Offset: 0x000B988C
		public void Kill()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.25);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000BB6E0 File Offset: 0x000B98E0
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
		// Token: 0x06001CCF RID: 7375 RVA: 0x000BB6F4 File Offset: 0x000B98F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardUncommonGlow.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("GPUParticles2D"), false), 33L, null, null),
				new MethodInfo(NCardUncommonGlow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardUncommonGlow.MethodName.Kill, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardUncommonGlow.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CD0 RID: 7376 RVA: 0x000BB7D4 File Offset: 0x000B99D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardUncommonGlow.MethodName.Create && args.Count == 0)
			{
				NCardUncommonGlow ncardUncommonGlow = NCardUncommonGlow.Create();
				ret = VariantUtils.CreateFrom<NCardUncommonGlow>(ref ncardUncommonGlow);
				return true;
			}
			if ((ref method) == NCardUncommonGlow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardUncommonGlow.MethodName.Kill && args.Count == 0)
			{
				this.Kill();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardUncommonGlow.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000BB884 File Offset: 0x000B9A84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardUncommonGlow.MethodName.Create && args.Count == 0)
			{
				NCardUncommonGlow ncardUncommonGlow = NCardUncommonGlow.Create();
				ret = VariantUtils.CreateFrom<NCardUncommonGlow>(ref ncardUncommonGlow);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001CD2 RID: 7378 RVA: 0x000BB8C4 File Offset: 0x000B9AC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardUncommonGlow.MethodName.Create || (ref method) == NCardUncommonGlow.MethodName._Ready || (ref method) == NCardUncommonGlow.MethodName.Kill || (ref method) == NCardUncommonGlow.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CD3 RID: 7379 RVA: 0x000BB914 File Offset: 0x000B9B14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardUncommonGlow.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CD4 RID: 7380 RVA: 0x000BB939 File Offset: 0x000B9B39
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardUncommonGlow.PropertyName._tween)
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
		// Token: 0x06001CD5 RID: 7381 RVA: 0x000BB964 File Offset: 0x000B9B64
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardUncommonGlow.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CD6 RID: 7382 RVA: 0x000BB999 File Offset: 0x000B9B99
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardUncommonGlow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001CD7 RID: 7383 RVA: 0x000BB9B8 File Offset: 0x000B9BB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardUncommonGlow.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04000B14 RID: 2836
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/uncommon_glow_vfx");

		// Token: 0x04000B15 RID: 2837
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200120B RID: 4619
		public class MethodName : GpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004605 RID: 17925
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004606 RID: 17926
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Kill' method.
			/// </summary>
			// Token: 0x04004607 RID: 17927
			public static readonly StringName Kill = "Kill";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004608 RID: 17928
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200120C RID: 4620
		public class PropertyName : GpuParticles2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004609 RID: 17929
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200120D RID: 4621
		public class SignalName : GpuParticles2D.SignalName
		{
		}
	}
}
