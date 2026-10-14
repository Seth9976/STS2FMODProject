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
	// Token: 0x02000201 RID: 513
	[ScriptPath("res://src/Core/Nodes/Vfx/NSweepingBeamImpactVfx.cs")]
	public class NSweepingBeamImpactVfx : Node2D
	{
		// Token: 0x06001A6E RID: 6766 RVA: 0x000A95EE File Offset: 0x000A77EE
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x000A9600 File Offset: 0x000A7800
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NSweepingBeamImpactVfx Create(Creature creature)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(creature) : null);
			if (ncreature != null)
			{
				return NSweepingBeamImpactVfx.Create(ncreature.VfxSpawnPosition);
			}
			return null;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x000A963C File Offset: 0x000A783C
		[NullableContext(2)]
		public static NSweepingBeamImpactVfx Create(Vector2 targetCenter)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSweepingBeamImpactVfx nsweepingBeamImpactVfx = PreloadManager.Cache.GetScene(NSweepingBeamImpactVfx.scenePath).Instantiate<NSweepingBeamImpactVfx>(0L);
			nsweepingBeamImpactVfx.GlobalPosition = targetCenter;
			return nsweepingBeamImpactVfx;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000A9671 File Offset: 0x000A7871
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000A9680 File Offset: 0x000A7880
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
		// Token: 0x06001A73 RID: 6771 RVA: 0x000A96C4 File Offset: 0x000A78C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSweepingBeamImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSweepingBeamImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetCenter", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSweepingBeamImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A74 RID: 6772 RVA: 0x000A9798 File Offset: 0x000A7998
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSweepingBeamImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSweepingBeamImpactVfx.MethodName.Create && args.Count == 1)
			{
				NSweepingBeamImpactVfx nsweepingBeamImpactVfx = NSweepingBeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NSweepingBeamImpactVfx>(ref nsweepingBeamImpactVfx);
				return true;
			}
			if ((ref method) == NSweepingBeamImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000A9834 File Offset: 0x000A7A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSweepingBeamImpactVfx.MethodName.Create && args.Count == 1)
			{
				NSweepingBeamImpactVfx nsweepingBeamImpactVfx = NSweepingBeamImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NSweepingBeamImpactVfx>(ref nsweepingBeamImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A76 RID: 6774 RVA: 0x000A9882 File Offset: 0x000A7A82
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSweepingBeamImpactVfx.MethodName._ExitTree || (ref method) == NSweepingBeamImpactVfx.MethodName.Create || (ref method) == NSweepingBeamImpactVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A77 RID: 6775 RVA: 0x000A98B8 File Offset: 0x000A7AB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSweepingBeamImpactVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A78 RID: 6776 RVA: 0x000A98DD File Offset: 0x000A7ADD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSweepingBeamImpactVfx.PropertyName._impactParticles)
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
		// Token: 0x06001A79 RID: 6777 RVA: 0x000A9908 File Offset: 0x000A7B08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSweepingBeamImpactVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A7A RID: 6778 RVA: 0x000A993E File Offset: 0x000A7B3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSweepingBeamImpactVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001A7B RID: 6779 RVA: 0x000A9960 File Offset: 0x000A7B60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSweepingBeamImpactVfx.PropertyName._impactParticles, ref variant))
			{
				this._impactParticles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040009CD RID: 2509
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sweeping_beam_impact");

		// Token: 0x040009CE RID: 2510
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040009CF RID: 2511
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001172 RID: 4466
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004408 RID: 17416
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004409 RID: 17417
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400440A RID: 17418
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001173 RID: 4467
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x0400440B RID: 17419
			public static readonly StringName _impactParticles = "_impactParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001174 RID: 4468
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
