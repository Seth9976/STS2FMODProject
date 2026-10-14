using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001BB RID: 443
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NHitSparkVfx.cs")]
	public class NHitSparkVfx : Node2D
	{
		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x00086E1B File Offset: 0x0008501B
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/hit_spark_vfx");
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x00086E27 File Offset: 0x00085027
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NHitSparkVfx.ScenePath);
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00086E34 File Offset: 0x00085034
		[return: Nullable(2)]
		public static NHitSparkVfx Create(Creature target, bool requireInteractable = true)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature == null)
			{
				return null;
			}
			if (requireInteractable && !ncreature.IsInteractable)
			{
				return null;
			}
			NHitSparkVfx nhitSparkVfx = PreloadManager.Cache.GetScene(NHitSparkVfx.ScenePath).Instantiate<NHitSparkVfx>(0L);
			nhitSparkVfx._creatureNode = ncreature;
			return nhitSparkVfx;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00086E88 File Offset: 0x00085088
		public override void _Ready()
		{
			base.GlobalPosition = this._creatureNode.VfxSpawnPosition;
			foreach (GpuParticles2D gpuParticles2D in this._particles)
			{
				gpuParticles2D.Restart();
			}
			TaskHelper.RunSafely(this.FlashAndFree());
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00086EF4 File Offset: 0x000850F4
		private async Task FlashAndFree()
		{
			await this._specks.AwaitSignal(GpuParticles2D.SignalName.Finished, this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001616 RID: 5654 RVA: 0x00086F38 File Offset: 0x00085138
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NHitSparkVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001617 RID: 5655 RVA: 0x00086F7C File Offset: 0x0008517C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHitSparkVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001618 RID: 5656 RVA: 0x00086FAC File Offset: 0x000851AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHitSparkVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001619 RID: 5657 RVA: 0x00086FC4 File Offset: 0x000851C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHitSparkVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHitSparkVfx.PropertyName._specks)
			{
				this._specks = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHitSparkVfx.PropertyName._creatureNode)
			{
				this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600161A RID: 5658 RVA: 0x0008702C File Offset: 0x0008522C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHitSparkVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NHitSparkVfx.PropertyName._specks)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._specks);
				return true;
			}
			if ((ref name) == NHitSparkVfx.PropertyName._creatureNode)
			{
				value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600161B RID: 5659 RVA: 0x000870A4 File Offset: 0x000852A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NHitSparkVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NHitSparkVfx.PropertyName._specks, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NHitSparkVfx.PropertyName._creatureNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600161C RID: 5660 RVA: 0x00087120 File Offset: 0x00085320
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHitSparkVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NHitSparkVfx.PropertyName._specks, Variant.From<GpuParticles2D>(ref this._specks));
			info.AddProperty(NHitSparkVfx.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
		}

		/// <inheritdoc />
		// Token: 0x0600161D RID: 5661 RVA: 0x00087178 File Offset: 0x00085378
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHitSparkVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHitSparkVfx.PropertyName._specks, ref variant2))
			{
				this._specks = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHitSparkVfx.PropertyName._creatureNode, ref variant3))
			{
				this._creatureNode = variant3.As<NCreature>();
			}
		}

		// Token: 0x040007D9 RID: 2009
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x040007DA RID: 2010
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _specks;

		// Token: 0x040007DB RID: 2011
		private NCreature _creatureNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200106A RID: 4202
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040A5 RID: 16549
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200106B RID: 4203
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040040A6 RID: 16550
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_specks' field.
			/// </summary>
			// Token: 0x040040A7 RID: 16551
			public static readonly StringName _specks = "_specks";

			/// <summary>
			/// Cached name for the '_creatureNode' field.
			/// </summary>
			// Token: 0x040040A8 RID: 16552
			public static readonly StringName _creatureNode = "_creatureNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200106C RID: 4204
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
