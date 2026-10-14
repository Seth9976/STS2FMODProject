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
	// Token: 0x0200020E RID: 526
	[ScriptPath("res://src/Core/Nodes/Vfx/NWormyImpactVfx.cs")]
	public class NWormyImpactVfx : Node2D
	{
		// Token: 0x06001B43 RID: 6979 RVA: 0x000B1268 File Offset: 0x000AF468
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000B127C File Offset: 0x000AF47C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NWormyImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NWormyImpactVfx.Create(ncreature.GetBottomOfHitbox(), ncreature.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000B12BC File Offset: 0x000AF4BC
		[NullableContext(2)]
		public static NWormyImpactVfx Create(Vector2 targetGroundPosition, Vector2 targetCenterPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NWormyImpactVfx nwormyImpactVfx = PreloadManager.Cache.GetScene(NWormyImpactVfx.scenePath).Instantiate<NWormyImpactVfx>(0L);
			nwormyImpactVfx.GlobalPosition = targetGroundPosition;
			nwormyImpactVfx.Initialize(targetGroundPosition, targetCenterPosition);
			return nwormyImpactVfx;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000B12F9 File Offset: 0x000AF4F9
		private void Initialize(Vector2 targetGroundPosition, Vector2 targetCenterPosition)
		{
			this._groundPivot.GlobalPosition = targetGroundPosition;
			this._centerPivot.GlobalPosition = targetCenterPosition;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000B1313 File Offset: 0x000AF513
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000B1324 File Offset: 0x000AF524
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
		// Token: 0x06001B49 RID: 6985 RVA: 0x000B1368 File Offset: 0x000AF568
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NWormyImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NWormyImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetGroundPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NWormyImpactVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetGroundPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NWormyImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B4A RID: 6986 RVA: 0x000B14D4 File Offset: 0x000AF6D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NWormyImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWormyImpactVfx.MethodName.Create && args.Count == 2)
			{
				NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NWormyImpactVfx>(ref nwormyImpactVfx);
				return true;
			}
			if ((ref method) == NWormyImpactVfx.MethodName.Initialize && args.Count == 2)
			{
				this.Initialize(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NWormyImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000B15BC File Offset: 0x000AF7BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NWormyImpactVfx.MethodName.Create && args.Count == 2)
			{
				NWormyImpactVfx nwormyImpactVfx = NWormyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NWormyImpactVfx>(ref nwormyImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001B4C RID: 6988 RVA: 0x000B1618 File Offset: 0x000AF818
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NWormyImpactVfx.MethodName._ExitTree || (ref method) == NWormyImpactVfx.MethodName.Create || (ref method) == NWormyImpactVfx.MethodName.Initialize || (ref method) == NWormyImpactVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B4D RID: 6989 RVA: 0x000B1668 File Offset: 0x000AF868
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NWormyImpactVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NWormyImpactVfx.PropertyName._centerPivot)
			{
				this._centerPivot = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NWormyImpactVfx.PropertyName._groundPivot)
			{
				this._groundPivot = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B4E RID: 6990 RVA: 0x000B16D0 File Offset: 0x000AF8D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NWormyImpactVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NWormyImpactVfx.PropertyName._centerPivot)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._centerPivot);
				return true;
			}
			if ((ref name) == NWormyImpactVfx.PropertyName._groundPivot)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._groundPivot);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B4F RID: 6991 RVA: 0x000B1748 File Offset: 0x000AF948
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NWormyImpactVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NWormyImpactVfx.PropertyName._centerPivot, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NWormyImpactVfx.PropertyName._groundPivot, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B50 RID: 6992 RVA: 0x000B17C4 File Offset: 0x000AF9C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NWormyImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NWormyImpactVfx.PropertyName._centerPivot, Variant.From<Node2D>(ref this._centerPivot));
			info.AddProperty(NWormyImpactVfx.PropertyName._groundPivot, Variant.From<Node2D>(ref this._groundPivot));
		}

		/// <inheritdoc />
		// Token: 0x06001B51 RID: 6993 RVA: 0x000B181C File Offset: 0x000AFA1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NWormyImpactVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NWormyImpactVfx.PropertyName._centerPivot, ref variant2))
			{
				this._centerPivot = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NWormyImpactVfx.PropertyName._groundPivot, ref variant3))
			{
				this._groundPivot = variant3.As<Node2D>();
			}
		}

		// Token: 0x04000A2E RID: 2606
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_wormy_impact");

		// Token: 0x04000A2F RID: 2607
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x04000A30 RID: 2608
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _centerPivot;

		// Token: 0x04000A31 RID: 2609
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _groundPivot;

		// Token: 0x04000A32 RID: 2610
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A3 RID: 4515
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040044C4 RID: 17604
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040044C5 RID: 17605
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x040044C6 RID: 17606
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040044C7 RID: 17607
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A4 RID: 4516
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040044C8 RID: 17608
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_centerPivot' field.
			/// </summary>
			// Token: 0x040044C9 RID: 17609
			public static readonly StringName _centerPivot = "_centerPivot";

			/// <summary>
			/// Cached name for the '_groundPivot' field.
			/// </summary>
			// Token: 0x040044CA RID: 17610
			public static readonly StringName _groundPivot = "_groundPivot";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011A5 RID: 4517
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
