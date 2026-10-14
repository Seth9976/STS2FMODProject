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
	// Token: 0x020001B4 RID: 436
	[ScriptPath("res://src/Core/Nodes/Vfx/NGoopyImpactVfx.cs")]
	public class NGoopyImpactVfx : Node2D
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x00084178 File Offset: 0x00082378
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGoopyImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NGoopyImpactVfx.Create(ncreature.VfxSpawnPosition, Colors.Green);
			}
			return null;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x000841B8 File Offset: 0x000823B8
		[NullableContext(2)]
		public static NGoopyImpactVfx Create(Vector2 targetCenterPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGoopyImpactVfx ngoopyImpactVfx = PreloadManager.Cache.GetScene(NGoopyImpactVfx.scenePath).Instantiate<NGoopyImpactVfx>(0L);
			ngoopyImpactVfx.GlobalPosition = targetCenterPosition;
			ngoopyImpactVfx.ModulateParticles(tint);
			return ngoopyImpactVfx;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x000841F4 File Offset: 0x000823F4
		private void ModulateParticles(Color tint)
		{
			base.Modulate = tint;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x000841FD File Offset: 0x000823FD
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0008420B File Offset: 0x0008240B
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00084220 File Offset: 0x00082420
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
		// Token: 0x060015A6 RID: 5542 RVA: 0x00084264 File Offset: 0x00082464
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGoopyImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGoopyImpactVfx.MethodName.ModulateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGoopyImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGoopyImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015A7 RID: 5543 RVA: 0x000843B0 File Offset: 0x000825B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGoopyImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGoopyImpactVfx ngoopyImpactVfx = NGoopyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NGoopyImpactVfx>(ref ngoopyImpactVfx);
				return true;
			}
			if ((ref method) == NGoopyImpactVfx.MethodName.ModulateParticles && args.Count == 1)
			{
				this.ModulateParticles(VariantUtils.ConvertTo<Color>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoopyImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGoopyImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0008448C File Offset: 0x0008268C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGoopyImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGoopyImpactVfx ngoopyImpactVfx = NGoopyImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NGoopyImpactVfx>(ref ngoopyImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060015A9 RID: 5545 RVA: 0x000844E8 File Offset: 0x000826E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGoopyImpactVfx.MethodName.Create || (ref method) == NGoopyImpactVfx.MethodName.ModulateParticles || (ref method) == NGoopyImpactVfx.MethodName._Ready || (ref method) == NGoopyImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015AA RID: 5546 RVA: 0x00084538 File Offset: 0x00082738
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGoopyImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015AB RID: 5547 RVA: 0x0008455D File Offset: 0x0008275D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGoopyImpactVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015AC RID: 5548 RVA: 0x00084588 File Offset: 0x00082788
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NGoopyImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015AD RID: 5549 RVA: 0x000845BE File Offset: 0x000827BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGoopyImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
		}

		/// <inheritdoc />
		// Token: 0x060015AE RID: 5550 RVA: 0x000845E0 File Offset: 0x000827E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGoopyImpactVfx.PropertyName._impactParticles, ref variant))
			{
				this._impactParticles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040007AD RID: 1965
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_goopy_impact");

		// Token: 0x040007AE RID: 1966
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040007AF RID: 1967
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200104E RID: 4174
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004052 RID: 16466
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'ModulateParticles' method.
			/// </summary>
			// Token: 0x04004053 RID: 16467
			public static readonly StringName ModulateParticles = "ModulateParticles";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004054 RID: 16468
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004055 RID: 16469
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200104F RID: 4175
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x04004056 RID: 16470
			public static readonly StringName _impactParticles = "_impactParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001050 RID: 4176
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
