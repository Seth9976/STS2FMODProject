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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001B5 RID: 437
	[ScriptPath("res://src/Core/Nodes/Vfx/NGrandFinaleImpactVfx.cs")]
	public class NGrandFinaleImpactVfx : Node2D
	{
		// Token: 0x060015B1 RID: 5553 RVA: 0x00084634 File Offset: 0x00082834
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGrandFinaleImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NGrandFinaleImpactVfx.Create(ncreature.VfxSpawnPosition, ncreature.GetBottomOfHitbox());
			}
			return null;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00084674 File Offset: 0x00082874
		[NullableContext(2)]
		public static NGrandFinaleImpactVfx Create(Vector2 targetCenterPosition, Vector2 targetGroundPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGrandFinaleImpactVfx ngrandFinaleImpactVfx = PreloadManager.Cache.GetScene(NGrandFinaleImpactVfx.scenePath).Instantiate<NGrandFinaleImpactVfx>(0L);
			ngrandFinaleImpactVfx.InitializePositions(targetCenterPosition, targetGroundPosition);
			return ngrandFinaleImpactVfx;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000846AA File Offset: 0x000828AA
		private void InitializePositions(Vector2 targetCenterPosition, Vector2 targetGroundPosition)
		{
			this._centerParticles.GlobalPosition = targetCenterPosition;
			this._groundParticles.GlobalPosition = targetGroundPosition;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000846C4 File Offset: 0x000828C4
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000846D2 File Offset: 0x000828D2
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000846E4 File Offset: 0x000828E4
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			this._centerParticles.Restart();
			this._groundParticles.Restart();
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015B7 RID: 5559 RVA: 0x00084728 File Offset: 0x00082928
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NGrandFinaleImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetGroundPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGrandFinaleImpactVfx.MethodName.InitializePositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenterPosition", 0L, "", 6L, false),
					new PropertyInfo(5L, "targetGroundPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGrandFinaleImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGrandFinaleImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015B8 RID: 5560 RVA: 0x00084894 File Offset: 0x00082A94
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGrandFinaleImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGrandFinaleImpactVfx ngrandFinaleImpactVfx = NGrandFinaleImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NGrandFinaleImpactVfx>(ref ngrandFinaleImpactVfx);
				return true;
			}
			if ((ref method) == NGrandFinaleImpactVfx.MethodName.InitializePositions && args.Count == 2)
			{
				this.InitializePositions(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGrandFinaleImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGrandFinaleImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0008497C File Offset: 0x00082B7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGrandFinaleImpactVfx.MethodName.Create && args.Count == 2)
			{
				NGrandFinaleImpactVfx ngrandFinaleImpactVfx = NGrandFinaleImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Vector2>(args[1]));
				ret = VariantUtils.CreateFrom<NGrandFinaleImpactVfx>(ref ngrandFinaleImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060015BA RID: 5562 RVA: 0x000849D8 File Offset: 0x00082BD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGrandFinaleImpactVfx.MethodName.Create || (ref method) == NGrandFinaleImpactVfx.MethodName.InitializePositions || (ref method) == NGrandFinaleImpactVfx.MethodName._Ready || (ref method) == NGrandFinaleImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060015BB RID: 5563 RVA: 0x00084A28 File Offset: 0x00082C28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGrandFinaleImpactVfx.PropertyName._centerParticles)
			{
				this._centerParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NGrandFinaleImpactVfx.PropertyName._groundParticles)
			{
				this._groundParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060015BC RID: 5564 RVA: 0x00084A68 File Offset: 0x00082C68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGrandFinaleImpactVfx.PropertyName._centerParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._centerParticles);
				return true;
			}
			if ((ref name) == NGrandFinaleImpactVfx.PropertyName._groundParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._groundParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060015BD RID: 5565 RVA: 0x00084AC0 File Offset: 0x00082CC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGrandFinaleImpactVfx.PropertyName._centerParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NGrandFinaleImpactVfx.PropertyName._groundParticles, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060015BE RID: 5566 RVA: 0x00084B18 File Offset: 0x00082D18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGrandFinaleImpactVfx.PropertyName._centerParticles, Variant.From<NParticlesContainer>(ref this._centerParticles));
			info.AddProperty(NGrandFinaleImpactVfx.PropertyName._groundParticles, Variant.From<NParticlesContainer>(ref this._groundParticles));
		}

		/// <inheritdoc />
		// Token: 0x060015BF RID: 5567 RVA: 0x00084B50 File Offset: 0x00082D50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGrandFinaleImpactVfx.PropertyName._centerParticles, ref variant))
			{
				this._centerParticles = variant.As<NParticlesContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGrandFinaleImpactVfx.PropertyName._groundParticles, ref variant2))
			{
				this._groundParticles = variant2.As<NParticlesContainer>();
			}
		}

		// Token: 0x040007B0 RID: 1968
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_grand_finale_impact");

		// Token: 0x040007B1 RID: 1969
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _centerParticles;

		// Token: 0x040007B2 RID: 1970
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _groundParticles;

		// Token: 0x040007B3 RID: 1971
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001052 RID: 4178
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400405B RID: 16475
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'InitializePositions' method.
			/// </summary>
			// Token: 0x0400405C RID: 16476
			public static readonly StringName InitializePositions = "InitializePositions";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400405D RID: 16477
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400405E RID: 16478
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001053 RID: 4179
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_centerParticles' field.
			/// </summary>
			// Token: 0x0400405F RID: 16479
			public static readonly StringName _centerParticles = "_centerParticles";

			/// <summary>
			/// Cached name for the '_groundParticles' field.
			/// </summary>
			// Token: 0x04004060 RID: 16480
			public static readonly StringName _groundParticles = "_groundParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001054 RID: 4180
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
