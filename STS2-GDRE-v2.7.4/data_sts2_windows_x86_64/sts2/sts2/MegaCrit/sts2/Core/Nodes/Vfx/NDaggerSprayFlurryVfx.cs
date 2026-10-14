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
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000197 RID: 407
	[ScriptPath("res://src/Core/Nodes/Vfx/NDaggerSprayFlurryVfx.cs")]
	public class NDaggerSprayFlurryVfx : Node2D
	{
		// Token: 0x060013F4 RID: 5108 RVA: 0x00078938 File Offset: 0x00076B38
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NDaggerSprayFlurryVfx Create(Creature creature, Color tint, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NDaggerSprayFlurryVfx.Create(ncreature.VfxSpawnPosition, tint, goingRight);
			}
			return null;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00078974 File Offset: 0x00076B74
		[NullableContext(2)]
		public static NDaggerSprayFlurryVfx Create(Vector2 targetCenter, Color tint, bool goingRight)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = PreloadManager.Cache.GetScene(NDaggerSprayFlurryVfx.scenePath).Instantiate<NDaggerSprayFlurryVfx>(0L);
			ndaggerSprayFlurryVfx.GlobalPosition = targetCenter;
			ndaggerSprayFlurryVfx.ApplyTint(tint);
			ndaggerSprayFlurryVfx.Scale = new Vector2((float)(goingRight ? 1 : (-1)), 1f);
			return ndaggerSprayFlurryVfx;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x000789C8 File Offset: 0x00076BC8
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x000789D6 File Offset: 0x00076BD6
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x000789E8 File Offset: 0x00076BE8
		public void ApplyTint(Color tint)
		{
			for (int i = 0; i < this._modulateParticles.Count; i++)
			{
				this._modulateParticles[i].ProcessMaterial = (ParticleProcessMaterial)this._modulateParticles[i].ProcessMaterial.Duplicate(false);
				this._modulateParticles[i].ProcessMaterial.Set(NDaggerSprayFlurryVfx._color, tint);
			}
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00078A5C File Offset: 0x00076C5C
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
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
		// Token: 0x060013FA RID: 5114 RVA: 0x00078AA0 File Offset: 0x00076CA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDaggerSprayFlurryVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenter", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDaggerSprayFlurryVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDaggerSprayFlurryVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDaggerSprayFlurryVfx.MethodName.ApplyTint, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060013FB RID: 5115 RVA: 0x00078C0C File Offset: 0x00076E0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDaggerSprayFlurryVfx.MethodName.Create && args.Count == 3)
			{
				NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = NDaggerSprayFlurryVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NDaggerSprayFlurryVfx>(ref ndaggerSprayFlurryVfx);
				return true;
			}
			if ((ref method) == NDaggerSprayFlurryVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDaggerSprayFlurryVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDaggerSprayFlurryVfx.MethodName.ApplyTint && args.Count == 1)
			{
				this.ApplyTint(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00078CF4 File Offset: 0x00076EF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDaggerSprayFlurryVfx.MethodName.Create && args.Count == 3)
			{
				NDaggerSprayFlurryVfx ndaggerSprayFlurryVfx = NDaggerSprayFlurryVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NDaggerSprayFlurryVfx>(ref ndaggerSprayFlurryVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060013FD RID: 5117 RVA: 0x00078D5C File Offset: 0x00076F5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDaggerSprayFlurryVfx.MethodName.Create || (ref method) == NDaggerSprayFlurryVfx.MethodName._Ready || (ref method) == NDaggerSprayFlurryVfx.MethodName._ExitTree || (ref method) == NDaggerSprayFlurryVfx.MethodName.ApplyTint || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060013FE RID: 5118 RVA: 0x00078DAC File Offset: 0x00076FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDaggerSprayFlurryVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDaggerSprayFlurryVfx.PropertyName._modulateParticles)
			{
				this._modulateParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060013FF RID: 5119 RVA: 0x00078DEC File Offset: 0x00076FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDaggerSprayFlurryVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NDaggerSprayFlurryVfx.PropertyName._modulateParticles)
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
		// Token: 0x06001400 RID: 5120 RVA: 0x00078E44 File Offset: 0x00077044
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NDaggerSprayFlurryVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NDaggerSprayFlurryVfx.PropertyName._modulateParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001401 RID: 5121 RVA: 0x00078E9C File Offset: 0x0007709C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDaggerSprayFlurryVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NDaggerSprayFlurryVfx.PropertyName._modulateParticles, Variant.CreateFrom<GpuParticles2D>(this._modulateParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001402 RID: 5122 RVA: 0x00078ED4 File Offset: 0x000770D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDaggerSprayFlurryVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDaggerSprayFlurryVfx.PropertyName._modulateParticles, ref variant2))
			{
				this._modulateParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000723 RID: 1827
		[Nullable(1)]
		private static readonly StringName _color = new StringName("color");

		// Token: 0x04000724 RID: 1828
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_dagger_spray_flurry");

		// Token: 0x04000725 RID: 1829
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x04000726 RID: 1830
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _modulateParticles = new Array<GpuParticles2D>();

		// Token: 0x04000727 RID: 1831
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE6 RID: 4070
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003F3C RID: 16188
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F3D RID: 16189
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F3E RID: 16190
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'ApplyTint' method.
			/// </summary>
			// Token: 0x04003F3F RID: 16191
			public static readonly StringName ApplyTint = "ApplyTint";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE7 RID: 4071
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04003F40 RID: 16192
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_modulateParticles' field.
			/// </summary>
			// Token: 0x04003F41 RID: 16193
			public static readonly StringName _modulateParticles = "_modulateParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FE8 RID: 4072
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
