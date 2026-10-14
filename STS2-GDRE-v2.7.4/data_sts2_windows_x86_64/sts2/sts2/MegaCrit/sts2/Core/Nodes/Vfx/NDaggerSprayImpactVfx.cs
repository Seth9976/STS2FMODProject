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
	// Token: 0x02000198 RID: 408
	[ScriptPath("res://src/Core/Nodes/Vfx/NDaggerSprayImpactVfx.cs")]
	public class NDaggerSprayImpactVfx : Node2D
	{
		// Token: 0x06001405 RID: 5125 RVA: 0x00078F60 File Offset: 0x00077160
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NDaggerSprayImpactVfx Create(Creature creature, Color tint, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NDaggerSprayImpactVfx.Create(ncreature.VfxSpawnPosition, tint, goingRight);
			}
			return null;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00078F9C File Offset: 0x0007719C
		[NullableContext(2)]
		public static NDaggerSprayImpactVfx Create(Vector2 targetCenter, Color tint, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDaggerSprayImpactVfx ndaggerSprayImpactVfx = PreloadManager.Cache.GetScene(NDaggerSprayImpactVfx.scenePath).Instantiate<NDaggerSprayImpactVfx>(0L);
			ndaggerSprayImpactVfx.GlobalPosition = targetCenter;
			ndaggerSprayImpactVfx.ApplyTint(tint);
			ndaggerSprayImpactVfx.Scale = new Vector2((float)(goingRight ? 1 : (-1)), 1f);
			return ndaggerSprayImpactVfx;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x00078FF0 File Offset: 0x000771F0
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00078FFE File Offset: 0x000771FE
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00079010 File Offset: 0x00077210
		public void ApplyTint(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].SelfModulate = tint;
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00079048 File Offset: 0x00077248
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			await Cmd.Wait(this._impactDelay, this._cts.Token, false);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
			}
			for (int i = 0; i < this._particles.Count; i++)
			{
				this._particles[i].Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600140B RID: 5131 RVA: 0x0007908C File Offset: 0x0007728C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDaggerSprayImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenter", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDaggerSprayImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDaggerSprayImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDaggerSprayImpactVfx.MethodName.ApplyTint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600140C RID: 5132 RVA: 0x000791F8 File Offset: 0x000773F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDaggerSprayImpactVfx.MethodName.Create && args.Count == 3)
			{
				NDaggerSprayImpactVfx ndaggerSprayImpactVfx = NDaggerSprayImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NDaggerSprayImpactVfx>(ref ndaggerSprayImpactVfx);
				return true;
			}
			if ((ref method) == NDaggerSprayImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDaggerSprayImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDaggerSprayImpactVfx.MethodName.ApplyTint && args.Count == 1)
			{
				this.ApplyTint(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x000792E0 File Offset: 0x000774E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDaggerSprayImpactVfx.MethodName.Create && args.Count == 3)
			{
				NDaggerSprayImpactVfx ndaggerSprayImpactVfx = NDaggerSprayImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NDaggerSprayImpactVfx>(ref ndaggerSprayImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600140E RID: 5134 RVA: 0x00079348 File Offset: 0x00077548
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDaggerSprayImpactVfx.MethodName.Create || (ref method) == NDaggerSprayImpactVfx.MethodName._Ready || (ref method) == NDaggerSprayImpactVfx.MethodName._ExitTree || (ref method) == NDaggerSprayImpactVfx.MethodName.ApplyTint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600140F RID: 5135 RVA: 0x00079398 File Offset: 0x00077598
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._impactDelay)
			{
				this._impactDelay = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001410 RID: 5136 RVA: 0x00079400 File Offset: 0x00077600
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._modulateParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._modulateParticles);
				return true;
			}
			if ((ref name) == NDaggerSprayImpactVfx.PropertyName._impactDelay)
			{
				value = VariantUtils.CreateFrom<float>(ref this._impactDelay);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001411 RID: 5137 RVA: 0x00079478 File Offset: 0x00077678
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NDaggerSprayImpactVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NDaggerSprayImpactVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(3L, NDaggerSprayImpactVfx.PropertyName._impactDelay, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001412 RID: 5138 RVA: 0x000794F0 File Offset: 0x000776F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDaggerSprayImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NDaggerSprayImpactVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
			info.AddProperty(NDaggerSprayImpactVfx.PropertyName._impactDelay, Variant.From<float>(ref this._impactDelay));
		}

		/// <inheritdoc />
		// Token: 0x06001413 RID: 5139 RVA: 0x00079548 File Offset: 0x00077748
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._modulateParticles, ref variant2))
			{
				this._modulateParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDaggerSprayImpactVfx.PropertyName._impactDelay, ref variant3))
			{
				this._impactDelay = variant3.As<float>();
			}
		}

		// Token: 0x04000728 RID: 1832
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_dagger_spray_impact");

		// Token: 0x04000729 RID: 1833
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x0400072A RID: 1834
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x0400072B RID: 1835
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _impactDelay = 0.1f;

		// Token: 0x0400072C RID: 1836
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FEA RID: 4074
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003F46 RID: 16198
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F47 RID: 16199
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F48 RID: 16200
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ApplyTint' method.
			/// </summary>
			// Token: 0x04003F49 RID: 16201
			public static readonly StringName ApplyTint = "ApplyTint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FEB RID: 4075
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04003F4A RID: 16202
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04003F4B RID: 16203
			public static readonly StringName _modulateParticles = "_modulateParticles";

			/// <summary>
			/// Cached name for the '_impactDelay' field.
			/// </summary>
			// Token: 0x04003F4C RID: 16204
			public static readonly StringName _impactDelay = "_impactDelay";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FEC RID: 4076
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
