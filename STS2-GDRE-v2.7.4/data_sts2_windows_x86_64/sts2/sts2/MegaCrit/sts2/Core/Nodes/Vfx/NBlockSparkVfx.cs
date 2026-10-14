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
	// Token: 0x0200018A RID: 394
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NBlockSparkVfx.cs")]
	public class NBlockSparkVfx : Node2D
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x000728B8 File Offset: 0x00070AB8
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/block_spark_vfx");
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x000728C4 File Offset: 0x00070AC4
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NBlockSparkVfx.ScenePath);
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x000728D0 File Offset: 0x00070AD0
		[return: Nullable(2)]
		public static NBlockSparkVfx Create(Creature target)
		{
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(target) : null);
			if (ncreature == null || !ncreature.IsInteractable)
			{
				return null;
			}
			NBlockSparkVfx nblockSparkVfx = PreloadManager.Cache.GetScene(NBlockSparkVfx.ScenePath).Instantiate<NBlockSparkVfx>(0L);
			nblockSparkVfx._creatureNode = ncreature;
			return nblockSparkVfx;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0007291C File Offset: 0x00070B1C
		public override void _Ready()
		{
			base.GlobalPosition = this._creatureNode.VfxSpawnPosition;
			for (int i = 0; i < this._particles.Count; i++)
			{
				this._particles[i].Restart();
			}
			TaskHelper.RunSafely(this.FlashAndFree());
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00072970 File Offset: 0x00070B70
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
		// Token: 0x0600131C RID: 4892 RVA: 0x000729B4 File Offset: 0x00070BB4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NBlockSparkVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600131D RID: 4893 RVA: 0x000729F8 File Offset: 0x00070BF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBlockSparkVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600131E RID: 4894 RVA: 0x00072A28 File Offset: 0x00070C28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBlockSparkVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600131F RID: 4895 RVA: 0x00072A40 File Offset: 0x00070C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBlockSparkVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBlockSparkVfx.PropertyName._specks)
			{
				this._specks = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NBlockSparkVfx.PropertyName._creatureNode)
			{
				this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001320 RID: 4896 RVA: 0x00072AA8 File Offset: 0x00070CA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBlockSparkVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NBlockSparkVfx.PropertyName._specks)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._specks);
				return true;
			}
			if ((ref name) == NBlockSparkVfx.PropertyName._creatureNode)
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
		// Token: 0x06001321 RID: 4897 RVA: 0x00072B20 File Offset: 0x00070D20
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NBlockSparkVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NBlockSparkVfx.PropertyName._specks, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NBlockSparkVfx.PropertyName._creatureNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001322 RID: 4898 RVA: 0x00072B9C File Offset: 0x00070D9C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBlockSparkVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NBlockSparkVfx.PropertyName._specks, Variant.From<GpuParticles2D>(ref this._specks));
			info.AddProperty(NBlockSparkVfx.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
		}

		/// <inheritdoc />
		// Token: 0x06001323 RID: 4899 RVA: 0x00072BF4 File Offset: 0x00070DF4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBlockSparkVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBlockSparkVfx.PropertyName._specks, ref variant2))
			{
				this._specks = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBlockSparkVfx.PropertyName._creatureNode, ref variant3))
			{
				this._creatureNode = variant3.As<NCreature>();
			}
		}

		// Token: 0x040006C0 RID: 1728
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x040006C1 RID: 1729
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _specks;

		// Token: 0x040006C2 RID: 1730
		private NCreature _creatureNode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB1 RID: 4017
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E81 RID: 16001
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB2 RID: 4018
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04003E82 RID: 16002
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_specks' field.
			/// </summary>
			// Token: 0x04003E83 RID: 16003
			public static readonly StringName _specks = "_specks";

			/// <summary>
			/// Cached name for the '_creatureNode' field.
			/// </summary>
			// Token: 0x04003E84 RID: 16004
			public static readonly StringName _creatureNode = "_creatureNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB3 RID: 4019
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
