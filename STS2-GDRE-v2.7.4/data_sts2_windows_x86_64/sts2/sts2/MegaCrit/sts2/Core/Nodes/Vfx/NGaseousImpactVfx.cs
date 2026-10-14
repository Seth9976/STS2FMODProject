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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001B3 RID: 435
	[ScriptPath("res://src/Core/Nodes/Vfx/NGaseousImpactVfx.cs")]
	public class NGaseousImpactVfx : Node2D
	{
		// Token: 0x0600158F RID: 5519 RVA: 0x00083D44 File Offset: 0x00081F44
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGaseousImpactVfx Create(CombatSide side, ICombatState combatState, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return NGaseousImpactVfx.Create(VfxCmd.GetSideCenter(side, combatState).Value, tint);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00083D70 File Offset: 0x00081F70
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGaseousImpactVfx Create(Creature creature, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NGaseousImpactVfx.Create(ncreature.VfxSpawnPosition, tint);
			}
			return null;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x00083DAC File Offset: 0x00081FAC
		[NullableContext(2)]
		public static NGaseousImpactVfx Create(Vector2 targetCenter, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGaseousImpactVfx ngaseousImpactVfx = PreloadManager.Cache.GetScene(NGaseousImpactVfx.scenePath).Instantiate<NGaseousImpactVfx>(0L);
			ngaseousImpactVfx.GlobalPosition = targetCenter;
			ngaseousImpactVfx.Modulate = tint;
			return ngaseousImpactVfx;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00083DE8 File Offset: 0x00081FE8
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00083DF6 File Offset: 0x00081FF6
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00083E08 File Offset: 0x00082008
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._impactParticles.Count; i++)
			{
				this._impactParticles[i].Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001595 RID: 5525 RVA: 0x00083E4C File Offset: 0x0008204C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NGaseousImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenter", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGaseousImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGaseousImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001596 RID: 5526 RVA: 0x00083F44 File Offset: 0x00082144
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGaseousImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NGaseousImpactVfx>(ref ngaseousImpactVfx);
				return true;
			}
			if ((ref method) == NGaseousImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGaseousImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00083FEC File Offset: 0x000821EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGaseousImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGaseousImpactVfx ngaseousImpactVfx = NGaseousImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NGaseousImpactVfx>(ref ngaseousImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001598 RID: 5528 RVA: 0x00084047 File Offset: 0x00082247
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGaseousImpactVfx.MethodName.Create || (ref method) == NGaseousImpactVfx.MethodName._Ready || (ref method) == NGaseousImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001599 RID: 5529 RVA: 0x0008407D File Offset: 0x0008227D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGaseousImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600159A RID: 5530 RVA: 0x000840A2 File Offset: 0x000822A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGaseousImpactVfx.PropertyName._impactParticles)
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
		// Token: 0x0600159B RID: 5531 RVA: 0x000840CC File Offset: 0x000822CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NGaseousImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600159C RID: 5532 RVA: 0x00084102 File Offset: 0x00082302
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGaseousImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
		}

		/// <inheritdoc />
		// Token: 0x0600159D RID: 5533 RVA: 0x00084124 File Offset: 0x00082324
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGaseousImpactVfx.PropertyName._impactParticles, ref variant))
			{
				this._impactParticles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040007AA RID: 1962
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_gaseous_impact");

		// Token: 0x040007AB RID: 1963
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040007AC RID: 1964
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200104A RID: 4170
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400404A RID: 16458
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400404B RID: 16459
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400404C RID: 16460
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200104B RID: 4171
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x0400404D RID: 16461
			public static readonly StringName _impactParticles = "_impactParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200104C RID: 4172
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
