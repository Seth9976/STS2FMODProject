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
	// Token: 0x0200019D RID: 413
	[ScriptPath("res://src/Core/Nodes/Vfx/NDesaturateTransitionVfx.cs")]
	public class NDesaturateTransitionVfx : Node
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0007AE8F File Offset: 0x0007908F
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NDesaturateTransitionVfx.ScenePath);
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0007AE9B File Offset: 0x0007909B
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/desaturate_transition_vfx");
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0007AEA8 File Offset: 0x000790A8
		[NullableContext(2)]
		public static NDesaturateTransitionVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NDesaturateTransitionVfx.ScenePath).Instantiate<NDesaturateTransitionVfx>(0L);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0007AED6 File Offset: 0x000790D6
		public override void _Ready()
		{
			this.Animate();
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0007AEDE File Offset: 0x000790DE
		public override void _ExitTree()
		{
			if (this._tween != null && this._tween.IsRunning())
			{
				this._tween.Kill();
				NGame.Instance.DeactivateWorldEnvironment();
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0007AF0C File Offset: 0x0007910C
		private void Animate()
		{
			WorldEnvironment worldEnvironment = NGame.Instance.ActivateWorldEnvironment();
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(worldEnvironment, "environment:tonemap_exposure", 1f, 1.0);
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_brightness", 0.1f, 1.0);
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_contrast", 0.7f, 1.0);
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_saturation", 0.25f, 1.0);
			this._tween.Chain();
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_contrast", 0.8f, 1.0);
			this._tween.Chain();
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_brightness", 1f, 1.0);
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_contrast", 1f, 1.0);
			this._tween.TweenProperty(worldEnvironment, "environment:adjustment_saturation", 1f, 1.0);
			this._tween.Chain().TweenCallback(Callable.From(delegate
			{
				NGame.Instance.DeactivateWorldEnvironment();
				this.QueueFreeSafely();
			}));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600145A RID: 5210 RVA: 0x0007B0C0 File Offset: 0x000792C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDesaturateTransitionVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node"), false), 33L, null, null),
				new MethodInfo(NDesaturateTransitionVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDesaturateTransitionVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDesaturateTransitionVfx.MethodName.Animate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600145B RID: 5211 RVA: 0x0007B1A0 File Offset: 0x000793A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDesaturateTransitionVfx.MethodName.Create && args.Count == 0)
			{
				NDesaturateTransitionVfx ndesaturateTransitionVfx = NDesaturateTransitionVfx.Create();
				ret = VariantUtils.CreateFrom<NDesaturateTransitionVfx>(ref ndesaturateTransitionVfx);
				return true;
			}
			if ((ref method) == NDesaturateTransitionVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDesaturateTransitionVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDesaturateTransitionVfx.MethodName.Animate && args.Count == 0)
			{
				this.Animate();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0007B250 File Offset: 0x00079450
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDesaturateTransitionVfx.MethodName.Create && args.Count == 0)
			{
				NDesaturateTransitionVfx ndesaturateTransitionVfx = NDesaturateTransitionVfx.Create();
				ret = VariantUtils.CreateFrom<NDesaturateTransitionVfx>(ref ndesaturateTransitionVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600145D RID: 5213 RVA: 0x0007B290 File Offset: 0x00079490
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDesaturateTransitionVfx.MethodName.Create || (ref method) == NDesaturateTransitionVfx.MethodName._Ready || (ref method) == NDesaturateTransitionVfx.MethodName._ExitTree || (ref method) == NDesaturateTransitionVfx.MethodName.Animate || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600145E RID: 5214 RVA: 0x0007B2E0 File Offset: 0x000794E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDesaturateTransitionVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600145F RID: 5215 RVA: 0x0007B305 File Offset: 0x00079505
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDesaturateTransitionVfx.PropertyName._tween)
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
		// Token: 0x06001460 RID: 5216 RVA: 0x0007B330 File Offset: 0x00079530
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDesaturateTransitionVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001461 RID: 5217 RVA: 0x0007B365 File Offset: 0x00079565
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDesaturateTransitionVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001462 RID: 5218 RVA: 0x0007B384 File Offset: 0x00079584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDesaturateTransitionVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
		}

		// Token: 0x04000743 RID: 1859
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFD RID: 4093
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003F7B RID: 16251
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F7C RID: 16252
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F7D RID: 16253
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Animate' method.
			/// </summary>
			// Token: 0x04003F7E RID: 16254
			public static readonly StringName Animate = "Animate";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFE RID: 4094
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003F7F RID: 16255
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FFF RID: 4095
		public class SignalName : Node.SignalName
		{
		}
	}
}
