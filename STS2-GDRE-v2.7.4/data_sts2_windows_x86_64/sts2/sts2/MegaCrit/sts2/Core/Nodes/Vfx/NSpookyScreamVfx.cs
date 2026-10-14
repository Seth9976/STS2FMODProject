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

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001FC RID: 508
	[ScriptPath("res://src/Core/Nodes/Vfx/NSpookyScreamVfx.cs")]
	public class NSpookyScreamVfx : Node2D
	{
		// Token: 0x06001A20 RID: 6688 RVA: 0x000A79A8 File Offset: 0x000A5BA8
		[NullableContext(2)]
		public static NSpookyScreamVfx Create(Vector2 position)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSpookyScreamVfx nspookyScreamVfx = PreloadManager.Cache.GetScene(NSpookyScreamVfx.scenePath).Instantiate<NSpookyScreamVfx>(0L);
			nspookyScreamVfx.GlobalPosition = position;
			return nspookyScreamVfx;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000A79DD File Offset: 0x000A5BDD
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000A79EB File Offset: 0x000A5BEB
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000A7A00 File Offset: 0x000A5C00
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			for (int i = 0; i < this._oneShotParticles.Count; i++)
			{
				this._oneShotParticles[i].Lifetime = (double)this._duration;
				this._oneShotParticles[i].Restart();
			}
			for (int j = 0; j < this._continuousParticles.Count; j++)
			{
				this._continuousParticles[j].Restart();
				this._continuousParticles[j].Emitting = true;
			}
			await Cmd.Wait(this._duration, this._cts.Token, false);
			for (int k = 0; k < this._continuousParticles.Count; k++)
			{
				this._continuousParticles[k].Emitting = false;
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A24 RID: 6692 RVA: 0x000A7A44 File Offset: 0x000A5C44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSpookyScreamVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSpookyScreamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpookyScreamVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A25 RID: 6693 RVA: 0x000A7B18 File Offset: 0x000A5D18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpookyScreamVfx.MethodName.Create && args.Count == 1)
			{
				NSpookyScreamVfx nspookyScreamVfx = NSpookyScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NSpookyScreamVfx>(ref nspookyScreamVfx);
				return true;
			}
			if ((ref method) == NSpookyScreamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpookyScreamVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000A7BB4 File Offset: 0x000A5DB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpookyScreamVfx.MethodName.Create && args.Count == 1)
			{
				NSpookyScreamVfx nspookyScreamVfx = NSpookyScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NSpookyScreamVfx>(ref nspookyScreamVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A27 RID: 6695 RVA: 0x000A7C02 File Offset: 0x000A5E02
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpookyScreamVfx.MethodName.Create || (ref method) == NSpookyScreamVfx.MethodName._Ready || (ref method) == NSpookyScreamVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A28 RID: 6696 RVA: 0x000A7C38 File Offset: 0x000A5E38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpookyScreamVfx.PropertyName._continuousParticles)
			{
				this._continuousParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpookyScreamVfx.PropertyName._oneShotParticles)
			{
				this._oneShotParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSpookyScreamVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A29 RID: 6697 RVA: 0x000A7CA0 File Offset: 0x000A5EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpookyScreamVfx.PropertyName._continuousParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._continuousParticles);
				return true;
			}
			if ((ref name) == NSpookyScreamVfx.PropertyName._oneShotParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._oneShotParticles);
				return true;
			}
			if ((ref name) == NSpookyScreamVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A2A RID: 6698 RVA: 0x000A7D18 File Offset: 0x000A5F18
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSpookyScreamVfx.PropertyName._continuousParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSpookyScreamVfx.PropertyName._oneShotParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(3L, NSpookyScreamVfx.PropertyName._duration, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A2B RID: 6699 RVA: 0x000A7D90 File Offset: 0x000A5F90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSpookyScreamVfx.PropertyName._continuousParticles, Variant.CreateFrom<GpuParticles2D>(this._continuousParticles));
			info.AddProperty(NSpookyScreamVfx.PropertyName._oneShotParticles, Variant.CreateFrom<GpuParticles2D>(this._oneShotParticles));
			info.AddProperty(NSpookyScreamVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
		}

		/// <inheritdoc />
		// Token: 0x06001A2C RID: 6700 RVA: 0x000A7DE8 File Offset: 0x000A5FE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpookyScreamVfx.PropertyName._continuousParticles, ref variant))
			{
				this._continuousParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpookyScreamVfx.PropertyName._oneShotParticles, ref variant2))
			{
				this._oneShotParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpookyScreamVfx.PropertyName._duration, ref variant3))
			{
				this._duration = variant3.As<float>();
			}
		}

		// Token: 0x040009B3 RID: 2483
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_spooky_scream");

		// Token: 0x040009B4 RID: 2484
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _continuousParticles = new Array<GpuParticles2D>();

		// Token: 0x040009B5 RID: 2485
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _oneShotParticles = new Array<GpuParticles2D>();

		// Token: 0x040009B6 RID: 2486
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _duration = 1f;

		// Token: 0x040009B7 RID: 2487
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200115E RID: 4446
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040043D3 RID: 17363
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043D4 RID: 17364
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043D5 RID: 17365
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200115F RID: 4447
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_continuousParticles' field.
			/// </summary>
			// Token: 0x040043D6 RID: 17366
			public static readonly StringName _continuousParticles = "_continuousParticles";

			/// <summary>
			/// Cached name for the '_oneShotParticles' field.
			/// </summary>
			// Token: 0x040043D7 RID: 17367
			public static readonly StringName _oneShotParticles = "_oneShotParticles";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x040043D8 RID: 17368
			public static readonly StringName _duration = "_duration";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001160 RID: 4448
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
