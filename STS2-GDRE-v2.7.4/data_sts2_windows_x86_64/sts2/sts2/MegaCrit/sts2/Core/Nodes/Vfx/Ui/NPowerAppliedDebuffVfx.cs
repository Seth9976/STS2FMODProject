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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui
{
	// Token: 0x0200022B RID: 555
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NPowerAppliedDebuffVfx.cs")]
	public class NPowerAppliedDebuffVfx : Node2D
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000B71BC File Offset: 0x000B53BC
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPowerAppliedDebuffVfx.scenePath);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000B71C8 File Offset: 0x000B53C8
		[NullableContext(2)]
		public static NPowerAppliedDebuffVfx Create(Vector2 globalPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPowerAppliedDebuffVfx npowerAppliedDebuffVfx = PreloadManager.Cache.GetScene(NPowerAppliedDebuffVfx.scenePath).Instantiate<NPowerAppliedDebuffVfx>(0L);
			npowerAppliedDebuffVfx.GlobalPosition = globalPosition;
			return npowerAppliedDebuffVfx;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000B71FD File Offset: 0x000B53FD
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000B720C File Offset: 0x000B540C
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			foreach (GpuParticles2D gpuParticles2D in this._particles)
			{
				gpuParticles2D.Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000B724F File Offset: 0x000B544F
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C31 RID: 7217 RVA: 0x000B7264 File Offset: 0x000B5464
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NPowerAppliedDebuffVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "globalPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPowerAppliedDebuffVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerAppliedDebuffVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C32 RID: 7218 RVA: 0x000B7338 File Offset: 0x000B5538
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerAppliedDebuffVfx.MethodName.Create && args.Count == 1)
			{
				NPowerAppliedDebuffVfx npowerAppliedDebuffVfx = NPowerAppliedDebuffVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerAppliedDebuffVfx>(ref npowerAppliedDebuffVfx);
				return true;
			}
			if ((ref method) == NPowerAppliedDebuffVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerAppliedDebuffVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000B73D4 File Offset: 0x000B55D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerAppliedDebuffVfx.MethodName.Create && args.Count == 1)
			{
				NPowerAppliedDebuffVfx npowerAppliedDebuffVfx = NPowerAppliedDebuffVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerAppliedDebuffVfx>(ref npowerAppliedDebuffVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C34 RID: 7220 RVA: 0x000B7422 File Offset: 0x000B5622
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerAppliedDebuffVfx.MethodName.Create || (ref method) == NPowerAppliedDebuffVfx.MethodName._Ready || (ref method) == NPowerAppliedDebuffVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C35 RID: 7221 RVA: 0x000B7458 File Offset: 0x000B5658
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerAppliedDebuffVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C36 RID: 7222 RVA: 0x000B747D File Offset: 0x000B567D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerAppliedDebuffVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C37 RID: 7223 RVA: 0x000B74A8 File Offset: 0x000B56A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NPowerAppliedDebuffVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C38 RID: 7224 RVA: 0x000B74DE File Offset: 0x000B56DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerAppliedDebuffVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
		}

		/// <inheritdoc />
		// Token: 0x06001C39 RID: 7225 RVA: 0x000B7500 File Offset: 0x000B5700
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerAppliedDebuffVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000ACD RID: 2765
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_debuff_applied");

		// Token: 0x04000ACE RID: 2766
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x04000ACF RID: 2767
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011DB RID: 4571
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400456A RID: 17770
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400456B RID: 17771
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400456C RID: 17772
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011DC RID: 4572
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x0400456D RID: 17773
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011DD RID: 4573
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
