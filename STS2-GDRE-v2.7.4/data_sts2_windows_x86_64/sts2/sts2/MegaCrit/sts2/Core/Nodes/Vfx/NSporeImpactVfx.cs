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
	// Token: 0x020001FD RID: 509
	[ScriptPath("res://src/Core/Nodes/Vfx/NSporeImpactVfx.cs")]
	public class NSporeImpactVfx : Node2D
	{
		// Token: 0x06001A2F RID: 6703 RVA: 0x000A7E8C File Offset: 0x000A608C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NSporeImpactVfx Create(Creature creature, Color color)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NSporeImpactVfx.Create(ncreature.GetBottomOfHitbox(), color);
			}
			return null;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000A7EC8 File Offset: 0x000A60C8
		[NullableContext(2)]
		public static NSporeImpactVfx Create(Vector2 targetGroundPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSporeImpactVfx nsporeImpactVfx = PreloadManager.Cache.GetScene(NSporeImpactVfx.scenePath).Instantiate<NSporeImpactVfx>(0L);
			nsporeImpactVfx.GlobalPosition = targetGroundPosition;
			nsporeImpactVfx.Initialize();
			nsporeImpactVfx.ModulateParticles(tint);
			return nsporeImpactVfx;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000A7F0A File Offset: 0x000A610A
		private void Initialize()
		{
			this._poofPivot.Scale = Vector2.One * (float)GD.RandRange((double)this._scaleRange.X, (double)this._scaleRange.Y);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000A7F3F File Offset: 0x000A613F
		private void ModulateParticles(Color tint)
		{
			base.Modulate = tint;
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x000A7F48 File Offset: 0x000A6148
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x000A7F56 File Offset: 0x000A6156
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x000A7F68 File Offset: 0x000A6168
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._impactParticles.Count; i++)
			{
				this._impactParticles[i].Restart();
			}
			await Cmd.Wait(3.5f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A36 RID: 6710 RVA: 0x000A7FAC File Offset: 0x000A61AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSporeImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetGroundPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSporeImpactVfx.MethodName.Initialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSporeImpactVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSporeImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSporeImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A37 RID: 6711 RVA: 0x000A8128 File Offset: 0x000A6328
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSporeImpactVfx.MethodName.Create && args.Count == 2)
			{
				NSporeImpactVfx nsporeImpactVfx = NSporeImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSporeImpactVfx>(ref nsporeImpactVfx);
				return true;
			}
			if ((ref method) == NSporeImpactVfx.MethodName.Initialize && args.Count == 0)
			{
				this.Initialize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSporeImpactVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSporeImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSporeImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x000A8228 File Offset: 0x000A6428
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSporeImpactVfx.MethodName.Create && args.Count == 2)
			{
				NSporeImpactVfx nsporeImpactVfx = NSporeImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSporeImpactVfx>(ref nsporeImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A39 RID: 6713 RVA: 0x000A8284 File Offset: 0x000A6484
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSporeImpactVfx.MethodName.Create || (ref method) == NSporeImpactVfx.MethodName.Initialize || (ref method) == NSporeImpactVfx.MethodName.ModulateParticles || (ref method) == NSporeImpactVfx.MethodName._Ready || (ref method) == NSporeImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A3A RID: 6714 RVA: 0x000A82E4 File Offset: 0x000A64E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSporeImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSporeImpactVfx.PropertyName._poofPivot)
			{
				this._poofPivot = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSporeImpactVfx.PropertyName._scaleRange)
			{
				this._scaleRange = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A3B RID: 6715 RVA: 0x000A834C File Offset: 0x000A654C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSporeImpactVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NSporeImpactVfx.PropertyName._poofPivot)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._poofPivot);
				return true;
			}
			if ((ref name) == NSporeImpactVfx.PropertyName._scaleRange)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._scaleRange);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A3C RID: 6716 RVA: 0x000A83C4 File Offset: 0x000A65C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSporeImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NSporeImpactVfx.PropertyName._poofPivot, 34L, "Node2D", 4102L, true),
				new PropertyInfo(5L, NSporeImpactVfx.PropertyName._scaleRange, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A3D RID: 6717 RVA: 0x000A843C File Offset: 0x000A663C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSporeImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NSporeImpactVfx.PropertyName._poofPivot, Variant.From<Node2D>(ref this._poofPivot));
			info.AddProperty(NSporeImpactVfx.PropertyName._scaleRange, Variant.From<Vector2>(ref this._scaleRange));
		}

		/// <inheritdoc />
		// Token: 0x06001A3E RID: 6718 RVA: 0x000A8494 File Offset: 0x000A6694
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSporeImpactVfx.PropertyName._impactParticles, ref variant))
			{
				this._impactParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSporeImpactVfx.PropertyName._poofPivot, ref variant2))
			{
				this._poofPivot = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSporeImpactVfx.PropertyName._scaleRange, ref variant3))
			{
				this._scaleRange = variant3.As<Vector2>();
			}
		}

		// Token: 0x040009B8 RID: 2488
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_spore_impact");

		// Token: 0x040009B9 RID: 2489
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040009BA RID: 2490
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _poofPivot;

		// Token: 0x040009BB RID: 2491
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Vector2 _scaleRange;

		// Token: 0x040009BC RID: 2492
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001162 RID: 4450
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040043DD RID: 17373
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'Initialize' method.
			/// </summary>
			// Token: 0x040043DE RID: 17374
			public static readonly StringName Initialize = "Initialize";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x040043DF RID: 17375
			public static readonly StringName ModulateParticles = "ModulateParticles";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043E0 RID: 17376
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043E1 RID: 17377
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001163 RID: 4451
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x040043E2 RID: 17378
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_poofPivot' field.
			/// </summary>
			// Token: 0x040043E3 RID: 17379
			public static readonly StringName _poofPivot = "_poofPivot";

			/// <summary>
			/// Cached name for the '_scaleRange' field.
			/// </summary>
			// Token: 0x040043E4 RID: 17380
			public static readonly StringName _scaleRange = "_scaleRange";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001164 RID: 4452
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
