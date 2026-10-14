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
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001C6 RID: 454
	[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestGrenadeVfx.cs")]
	public class NKinPriestGrenadeVfx : Node2D
	{
		// Token: 0x060016BE RID: 5822 RVA: 0x0008C41C File Offset: 0x0008A61C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NKinPriestGrenadeVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
			if (creatureNode == null)
			{
				return null;
			}
			NKinPriestGrenadeVfx nkinPriestGrenadeVfx = PreloadManager.Cache.GetScene(NKinPriestGrenadeVfx._scenePath).Instantiate<NKinPriestGrenadeVfx>(0L);
			nkinPriestGrenadeVfx.GlobalPosition = creatureNode.GetBottomOfHitbox();
			return nkinPriestGrenadeVfx;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x0008C468 File Offset: 0x0008A668
		public override void _Ready()
		{
			this._cryptoParticles = base.GetNode<GpuParticles2D>("CryptoParticles");
			this._noiseParticles = base.GetNode<GpuParticles2D>("NoiseParticles");
			this._explosionBase = base.GetNode<GpuParticles2D>("ExplosionBaseParticle");
			this._cryptoParticles.Emitting = false;
			this._cryptoParticles.OneShot = true;
			this._noiseParticles.Emitting = false;
			this._noiseParticles.OneShot = true;
			this._explosionBase.Emitting = false;
			this._explosionBase.OneShot = true;
			TaskHelper.RunSafely(this.Play());
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0008C50C File Offset: 0x0008A70C
		[NullableContext(1)]
		private async Task Play()
		{
			NDebugAudioManager instance = NDebugAudioManager.Instance;
			if (instance != null)
			{
				instance.Play("blunt_attack.mp3", 1f, PitchVariance.None);
			}
			this._noiseParticles.SetEmitting(true);
			this._explosionBase.SetEmitting(true);
			await Task.Delay(100, this._cancelToken.Token);
			this._cryptoParticles.SetEmitting(true);
			await Task.Delay(5000, this._cancelToken.Token);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016C1 RID: 5825 RVA: 0x0008C550 File Offset: 0x0008A750
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NKinPriestGrenadeVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016C2 RID: 5826 RVA: 0x0008C594 File Offset: 0x0008A794
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKinPriestGrenadeVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060016C3 RID: 5827 RVA: 0x0008C5C4 File Offset: 0x0008A7C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKinPriestGrenadeVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016C4 RID: 5828 RVA: 0x0008C5DC File Offset: 0x0008A7DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._cryptoParticles)
			{
				this._cryptoParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._noiseParticles)
			{
				this._noiseParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._explosionBase)
			{
				this._explosionBase = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016C5 RID: 5829 RVA: 0x0008C644 File Offset: 0x0008A844
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._cryptoParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cryptoParticles);
				return true;
			}
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._noiseParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._noiseParticles);
				return true;
			}
			if ((ref name) == NKinPriestGrenadeVfx.PropertyName._explosionBase)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionBase);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016C6 RID: 5830 RVA: 0x0008C6BC File Offset: 0x0008A8BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKinPriestGrenadeVfx.PropertyName._cryptoParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestGrenadeVfx.PropertyName._noiseParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestGrenadeVfx.PropertyName._explosionBase, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016C7 RID: 5831 RVA: 0x0008C734 File Offset: 0x0008A934
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKinPriestGrenadeVfx.PropertyName._cryptoParticles, Variant.From<GpuParticles2D>(ref this._cryptoParticles));
			info.AddProperty(NKinPriestGrenadeVfx.PropertyName._noiseParticles, Variant.From<GpuParticles2D>(ref this._noiseParticles));
			info.AddProperty(NKinPriestGrenadeVfx.PropertyName._explosionBase, Variant.From<GpuParticles2D>(ref this._explosionBase));
		}

		/// <inheritdoc />
		// Token: 0x060016C8 RID: 5832 RVA: 0x0008C78C File Offset: 0x0008A98C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._cryptoParticles, ref variant))
			{
				this._cryptoParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._noiseParticles, ref variant2))
			{
				this._noiseParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKinPriestGrenadeVfx.PropertyName._explosionBase, ref variant3))
			{
				this._explosionBase = variant3.As<GpuParticles2D>();
			}
		}

		// Token: 0x0400081E RID: 2078
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/monsters/kin_priest_grenade_vfx");

		// Token: 0x0400081F RID: 2079
		[Nullable(1)]
		private GpuParticles2D _cryptoParticles;

		// Token: 0x04000820 RID: 2080
		[Nullable(1)]
		private GpuParticles2D _noiseParticles;

		// Token: 0x04000821 RID: 2081
		[Nullable(1)]
		private GpuParticles2D _explosionBase;

		// Token: 0x04000822 RID: 2082
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001090 RID: 4240
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004122 RID: 16674
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001091 RID: 4241
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_cryptoParticles' field.
			/// </summary>
			// Token: 0x04004123 RID: 16675
			public static readonly StringName _cryptoParticles = "_cryptoParticles";

			/// <summary>
			/// Cached name for the '_noiseParticles' field.
			/// </summary>
			// Token: 0x04004124 RID: 16676
			public static readonly StringName _noiseParticles = "_noiseParticles";

			/// <summary>
			/// Cached name for the '_explosionBase' field.
			/// </summary>
			// Token: 0x04004125 RID: 16677
			public static readonly StringName _explosionBase = "_explosionBase";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001092 RID: 4242
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
