using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001EE RID: 494
	[ScriptPath("res://src/Core/Nodes/Vfx/NShivThrowVfx.cs")]
	public class NShivThrowVfx : Node2D
	{
		// Token: 0x06001929 RID: 6441 RVA: 0x0009E010 File Offset: 0x0009C210
		[NullableContext(2)]
		public static NShivThrowVfx Create([Nullable(1)] Creature owner, Creature target, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(owner) : null);
			if (ncreature == null)
			{
				return null;
			}
			NCombatRoom instance2 = NCombatRoom.Instance;
			NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(target) : null);
			if (ncreature2 == null)
			{
				return null;
			}
			return NShivThrowVfx.Create(ncreature.VfxSpawnPosition, ncreature2.VfxSpawnPosition, tint);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0009E068 File Offset: 0x0009C268
		[NullableContext(2)]
		public static NShivThrowVfx Create(Vector2 throwerCenterPosition, Vector2 targetCenterPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NShivThrowVfx nshivThrowVfx = PreloadManager.Cache.GetScene(NShivThrowVfx.scenePath).Instantiate<NShivThrowVfx>(0L);
			nshivThrowVfx.GlobalPosition = targetCenterPosition;
			nshivThrowVfx.ApplyRotation(throwerCenterPosition, targetCenterPosition);
			nshivThrowVfx.ApplyTint(tint);
			return nshivThrowVfx;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0009E0AC File Offset: 0x0009C2AC
		public void ApplyTint(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].ProcessMaterial = (ParticleProcessMaterial)this._modulateParticles[i].ProcessMaterial.Duplicate(false);
				this._modulateParticles[i].ProcessMaterial.Set(NShivThrowVfx._color, tint);
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0009E120 File Offset: 0x0009C320
		public void ApplyRotation(Vector2 throwerPosition, Vector2 targetPosition)
		{
			Vector2 vector = targetPosition - throwerPosition;
			float num = Mathf.RadToDeg(Mathf.Atan2(vector.Y, vector.X));
			base.RotationDegrees = num;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0009E153 File Offset: 0x0009C353
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0009E161 File Offset: 0x0009C361
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0009E174 File Offset: 0x0009C374
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._throwParticles.Count; i++)
			{
				this._throwParticles[i].Restart();
			}
			await Cmd.Wait(0.15f, this._cts.Token, false);
			for (int j = 0; j < this._impactParticles.Count; j++)
			{
				this._impactParticles[j].Restart();
			}
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001930 RID: 6448 RVA: 0x0009E1B8 File Offset: 0x0009C3B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NShivThrowVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "throwerCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShivThrowVfx.MethodName.ApplyTint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShivThrowVfx.MethodName.ApplyRotation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "throwerPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NShivThrowVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShivThrowVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001931 RID: 6449 RVA: 0x0009E398 File Offset: 0x0009C598
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShivThrowVfx.MethodName.Create && args.Count == 3)
			{
				NShivThrowVfx nshivThrowVfx = NShivThrowVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NShivThrowVfx>(ref nshivThrowVfx);
				return true;
			}
			if ((ref method) == NShivThrowVfx.MethodName.ApplyTint && args.Count == 1)
			{
				this.ApplyTint(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShivThrowVfx.MethodName.ApplyRotation && args.Count == 2)
			{
				this.ApplyRotation(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShivThrowVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShivThrowVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0009E4C0 File Offset: 0x0009C6C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShivThrowVfx.MethodName.Create && args.Count == 3)
			{
				NShivThrowVfx nshivThrowVfx = NShivThrowVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]), VariantUtils.ConvertTo<Color>(args[2]));
				ret = VariantUtils.CreateFrom<NShivThrowVfx>(ref nshivThrowVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001933 RID: 6451 RVA: 0x0009E528 File Offset: 0x0009C728
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NShivThrowVfx.MethodName.Create || (ref method) == NShivThrowVfx.MethodName.ApplyTint || (ref method) == NShivThrowVfx.MethodName.ApplyRotation || (ref method) == NShivThrowVfx.MethodName._Ready || (ref method) == NShivThrowVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001934 RID: 6452 RVA: 0x0009E588 File Offset: 0x0009C788
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NShivThrowVfx.PropertyName._throwParticles)
			{
				this._throwParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NShivThrowVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NShivThrowVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001935 RID: 6453 RVA: 0x0009E5F0 File Offset: 0x0009C7F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NShivThrowVfx.PropertyName._throwParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._throwParticles);
				return true;
			}
			if ((ref name) == NShivThrowVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NShivThrowVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001936 RID: 6454 RVA: 0x0009E668 File Offset: 0x0009C868
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NShivThrowVfx.PropertyName._throwParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NShivThrowVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NShivThrowVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001937 RID: 6455 RVA: 0x0009E6E4 File Offset: 0x0009C8E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NShivThrowVfx.PropertyName._throwParticles, Variant.CreateFrom<GpuParticles2D>(this._throwParticles));
			info.AddProperty(NShivThrowVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NShivThrowVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001938 RID: 6456 RVA: 0x0009E73C File Offset: 0x0009C93C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NShivThrowVfx.PropertyName._throwParticles, ref variant))
			{
				this._throwParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NShivThrowVfx.PropertyName._impactParticles, ref variant2))
			{
				this._impactParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NShivThrowVfx.PropertyName._modulateParticles, ref variant3))
			{
				this._modulateParticles = variant3.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x0400092A RID: 2346
		[Nullable(1)]
		private static readonly StringName _color = new StringName("color");

		// Token: 0x0400092B RID: 2347
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_shiv_throw");

		// Token: 0x0400092C RID: 2348
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _throwParticles = new Array<GpuParticles2D>();

		// Token: 0x0400092D RID: 2349
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x0400092E RID: 2350
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x0400092F RID: 2351
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001127 RID: 4391
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040042F1 RID: 17137
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'ApplyTint' method.
			/// </summary>
			// Token: 0x040042F2 RID: 17138
			public static readonly StringName ApplyTint = "ApplyTint";

			/// <summary>
			/// Cached name for the 'ApplyRotation' method.
			/// </summary>
			// Token: 0x040042F3 RID: 17139
			public static readonly StringName ApplyRotation = "ApplyRotation";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042F4 RID: 17140
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040042F5 RID: 17141
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001128 RID: 4392
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_throwParticles' field.
			/// </summary>
			// Token: 0x040042F6 RID: 17142
			public static readonly StringName _throwParticles = "_throwParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x040042F7 RID: 17143
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x040042F8 RID: 17144
			public static readonly StringName _modulateParticles = "_modulateParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001129 RID: 4393
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
