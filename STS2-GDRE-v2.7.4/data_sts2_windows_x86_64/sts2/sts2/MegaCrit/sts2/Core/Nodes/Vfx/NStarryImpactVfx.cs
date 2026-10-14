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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001FF RID: 511
	[ScriptPath("res://src/Core/Nodes/Vfx/NStarryImpactVfx.cs")]
	public class NStarryImpactVfx : Node2D
	{
		// Token: 0x06001A50 RID: 6736 RVA: 0x000A8E20 File Offset: 0x000A7020
		[NullableContext(2)]
		public static NStarryImpactVfx Create(Vector2 position)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NStarryImpactVfx nstarryImpactVfx = PreloadManager.Cache.GetScene(NStarryImpactVfx.scenePath).Instantiate<NStarryImpactVfx>(0L);
			nstarryImpactVfx.GlobalPosition = position;
			return nstarryImpactVfx;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000A8E55 File Offset: 0x000A7055
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000A8E63 File Offset: 0x000A7063
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000A8E78 File Offset: 0x000A7078
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, -1f);
			}
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
		// Token: 0x06001A54 RID: 6740 RVA: 0x000A8EBC File Offset: 0x000A70BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NStarryImpactVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NStarryImpactVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NStarryImpactVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A55 RID: 6741 RVA: 0x000A8F90 File Offset: 0x000A7190
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStarryImpactVfx.MethodName.Create && args.Count == 1)
			{
				NStarryImpactVfx nstarryImpactVfx = NStarryImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NStarryImpactVfx>(ref nstarryImpactVfx);
				return true;
			}
			if ((ref method) == NStarryImpactVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NStarryImpactVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000A902C File Offset: 0x000A722C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NStarryImpactVfx.MethodName.Create && args.Count == 1)
			{
				NStarryImpactVfx nstarryImpactVfx = NStarryImpactVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NStarryImpactVfx>(ref nstarryImpactVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A57 RID: 6743 RVA: 0x000A907A File Offset: 0x000A727A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NStarryImpactVfx.MethodName.Create || (ref method) == NStarryImpactVfx.MethodName._Ready || (ref method) == NStarryImpactVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A58 RID: 6744 RVA: 0x000A90B0 File Offset: 0x000A72B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NStarryImpactVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A59 RID: 6745 RVA: 0x000A90D5 File Offset: 0x000A72D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NStarryImpactVfx.PropertyName._particles)
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
		// Token: 0x06001A5A RID: 6746 RVA: 0x000A9100 File Offset: 0x000A7300
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NStarryImpactVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A5B RID: 6747 RVA: 0x000A9136 File Offset: 0x000A7336
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NStarryImpactVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
		}

		/// <inheritdoc />
		// Token: 0x06001A5C RID: 6748 RVA: 0x000A9158 File Offset: 0x000A7358
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NStarryImpactVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x040009C4 RID: 2500
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_starry_impact");

		// Token: 0x040009C5 RID: 2501
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x040009C6 RID: 2502
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200116A RID: 4458
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040043F7 RID: 17399
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043F8 RID: 17400
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043F9 RID: 17401
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200116B RID: 4459
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040043FA RID: 17402
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200116C RID: 4460
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
