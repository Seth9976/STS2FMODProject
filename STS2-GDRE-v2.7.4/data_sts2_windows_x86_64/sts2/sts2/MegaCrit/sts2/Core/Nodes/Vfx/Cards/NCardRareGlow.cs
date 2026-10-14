using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x02000233 RID: 563
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardRareGlow.cs")]
	public class NCardRareGlow : GpuParticles2D
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000B9AB2 File Offset: 0x000B7CB2
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardRareGlow._scenePath);
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000B9ABE File Offset: 0x000B7CBE
		[NullableContext(2)]
		public static NCardRareGlow Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NCardRareGlow._scenePath).Instantiate<NCardRareGlow>(0L);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000B9AE0 File Offset: 0x000B7CE0
		public override void _Ready()
		{
			Vector2 vector = base.Scale * 0.92f;
			double num = Rng.Chaotic.NextDouble(0.0, 0.2);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "scale", vector, 1.0).SetEase(0L).SetTrans(7L)
				.SetDelay(num);
			this._tween.TweenProperty(this, "modulate:a", 0.9f, 1.0).SetEase(0L).SetTrans(7L)
				.SetDelay(num);
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000B9BA8 File Offset: 0x000B7DA8
		public void Kill()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0, 0.25);
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000B9BF8 File Offset: 0x000B7DF8
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
		// Token: 0x06001C9C RID: 7324 RVA: 0x000B9C0C File Offset: 0x000B7E0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCardRareGlow.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("GPUParticles2D"), false), 33L, null, null),
				new MethodInfo(NCardRareGlow.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRareGlow.MethodName.Kill, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardRareGlow.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C9D RID: 7325 RVA: 0x000B9CEC File Offset: 0x000B7EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRareGlow.MethodName.Create && args.Count == 0)
			{
				NCardRareGlow ncardRareGlow = NCardRareGlow.Create();
				ret = VariantUtils.CreateFrom<NCardRareGlow>(ref ncardRareGlow);
				return true;
			}
			if ((ref method) == NCardRareGlow.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRareGlow.MethodName.Kill && args.Count == 0)
			{
				this.Kill();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardRareGlow.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000B9D9C File Offset: 0x000B7F9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardRareGlow.MethodName.Create && args.Count == 0)
			{
				NCardRareGlow ncardRareGlow = NCardRareGlow.Create();
				ret = VariantUtils.CreateFrom<NCardRareGlow>(ref ncardRareGlow);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C9F RID: 7327 RVA: 0x000B9DDC File Offset: 0x000B7FDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardRareGlow.MethodName.Create || (ref method) == NCardRareGlow.MethodName._Ready || (ref method) == NCardRareGlow.MethodName.Kill || (ref method) == NCardRareGlow.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001CA0 RID: 7328 RVA: 0x000B9E2C File Offset: 0x000B802C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardRareGlow.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001CA1 RID: 7329 RVA: 0x000B9E51 File Offset: 0x000B8051
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardRareGlow.PropertyName._tween)
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
		// Token: 0x06001CA2 RID: 7330 RVA: 0x000B9E7C File Offset: 0x000B807C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardRareGlow.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001CA3 RID: 7331 RVA: 0x000B9EB1 File Offset: 0x000B80B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardRareGlow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001CA4 RID: 7332 RVA: 0x000B9ED0 File Offset: 0x000B80D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardRareGlow.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04000AF1 RID: 2801
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/rare_glow_vfx");

		// Token: 0x04000AF2 RID: 2802
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011F9 RID: 4601
		public class MethodName : GpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040045B5 RID: 17845
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040045B6 RID: 17846
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Kill' method.
			/// </summary>
			// Token: 0x040045B7 RID: 17847
			public static readonly StringName Kill = "Kill";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040045B8 RID: 17848
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011FA RID: 4602
		public class PropertyName : GpuParticles2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040045B9 RID: 17849
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011FB RID: 4603
		public class SignalName : GpuParticles2D.SignalName
		{
		}
	}
}
