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
	// Token: 0x020001DC RID: 476
	[ScriptPath("res://src/Core/Nodes/Vfx/NPoisonImpactVfx.cs")]
	public class NPoisonImpactVfx : Node2D
	{
		// Token: 0x06001812 RID: 6162 RVA: 0x00096C10 File Offset: 0x00094E10
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NPoisonImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NPoisonImpactVfx.Create(ncreature.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00096C4C File Offset: 0x00094E4C
		[NullableContext(2)]
		public static NPoisonImpactVfx Create(Vector2 targetCenter)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPoisonImpactVfx npoisonImpactVfx = PreloadManager.Cache.GetScene(NPoisonImpactVfx.scenePath).Instantiate<NPoisonImpactVfx>(0L);
			npoisonImpactVfx.GlobalPosition = targetCenter;
			npoisonImpactVfx._horizontalSmokeContainer.Scale = new Vector2(((double)GD.Randf() > 0.5) ? 1f : (-1f), 1f);
			return npoisonImpactVfx;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00096CB3 File Offset: 0x00094EB3
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00096CC1 File Offset: 0x00094EC1
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00096CD4 File Offset: 0x00094ED4
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
		// Token: 0x06001817 RID: 6167 RVA: 0x00096D18 File Offset: 0x00094F18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NPoisonImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenter", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPoisonImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPoisonImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001818 RID: 6168 RVA: 0x00096DEC File Offset: 0x00094FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPoisonImpactVfx.MethodName.Create && args.Count == 1)
			{
				NPoisonImpactVfx npoisonImpactVfx = NPoisonImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPoisonImpactVfx>(ref npoisonImpactVfx);
				return true;
			}
			if ((ref method) == NPoisonImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPoisonImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00096E88 File Offset: 0x00095088
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPoisonImpactVfx.MethodName.Create && args.Count == 1)
			{
				NPoisonImpactVfx npoisonImpactVfx = NPoisonImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPoisonImpactVfx>(ref npoisonImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600181A RID: 6170 RVA: 0x00096ED6 File Offset: 0x000950D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPoisonImpactVfx.MethodName.Create || (ref method) == NPoisonImpactVfx.MethodName._Ready || (ref method) == NPoisonImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600181B RID: 6171 RVA: 0x00096F0C File Offset: 0x0009510C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPoisonImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NPoisonImpactVfx.PropertyName._horizontalSmokeContainer)
			{
				this._horizontalSmokeContainer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600181C RID: 6172 RVA: 0x00096F4C File Offset: 0x0009514C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPoisonImpactVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NPoisonImpactVfx.PropertyName._horizontalSmokeContainer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._horizontalSmokeContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600181D RID: 6173 RVA: 0x00096FA4 File Offset: 0x000951A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NPoisonImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NPoisonImpactVfx.PropertyName._horizontalSmokeContainer, 34L, "Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600181E RID: 6174 RVA: 0x00096FFC File Offset: 0x000951FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPoisonImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NPoisonImpactVfx.PropertyName._horizontalSmokeContainer, Variant.From<Node2D>(ref this._horizontalSmokeContainer));
		}

		/// <inheritdoc />
		// Token: 0x0600181F RID: 6175 RVA: 0x00097034 File Offset: 0x00095234
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPoisonImpactVfx.PropertyName._impactParticles, ref variant))
			{
				this._impactParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPoisonImpactVfx.PropertyName._horizontalSmokeContainer, ref variant2))
			{
				this._horizontalSmokeContainer = variant2.As<Node2D>();
			}
		}

		// Token: 0x040008B1 RID: 2225
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_poison_impact");

		// Token: 0x040008B2 RID: 2226
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040008B3 RID: 2227
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _horizontalSmokeContainer;

		// Token: 0x040008B4 RID: 2228
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E3 RID: 4323
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400422C RID: 16940
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400422D RID: 16941
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400422E RID: 16942
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E4 RID: 4324
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x0400422F RID: 16943
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_horizontalSmokeContainer' field.
			/// </summary>
			// Token: 0x04004230 RID: 16944
			public static readonly StringName _horizontalSmokeContainer = "_horizontalSmokeContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010E5 RID: 4325
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
