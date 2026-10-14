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
	// Token: 0x020001EB RID: 491
	[ScriptPath("res://src/Core/Nodes/Vfx/NScreamVfx.cs")]
	public class NScreamVfx : Node2D
	{
		// Token: 0x060018FB RID: 6395 RVA: 0x0009C9DC File Offset: 0x0009ABDC
		[NullableContext(2)]
		public static NScreamVfx Create(Vector2 position)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NScreamVfx nscreamVfx = PreloadManager.Cache.GetScene(NScreamVfx.scenePath).Instantiate<NScreamVfx>(0L);
			nscreamVfx.GlobalPosition = position;
			return nscreamVfx;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0009CA11 File Offset: 0x0009AC11
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0009CA1F File Offset: 0x0009AC1F
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0009CA34 File Offset: 0x0009AC34
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			this._cts = new CancellationTokenSource();
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Normal, -1f);
			}
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
		// Token: 0x060018FF RID: 6399 RVA: 0x0009CA78 File Offset: 0x0009AC78
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NScreamVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreamVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreamVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001900 RID: 6400 RVA: 0x0009CB4C File Offset: 0x0009AD4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreamVfx.MethodName.Create && args.Count == 1)
			{
				NScreamVfx nscreamVfx = NScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NScreamVfx>(ref nscreamVfx);
				return true;
			}
			if ((ref method) == NScreamVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreamVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0009CBE8 File Offset: 0x0009ADE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreamVfx.MethodName.Create && args.Count == 1)
			{
				NScreamVfx nscreamVfx = NScreamVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NScreamVfx>(ref nscreamVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001902 RID: 6402 RVA: 0x0009CC36 File Offset: 0x0009AE36
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScreamVfx.MethodName.Create || (ref method) == NScreamVfx.MethodName._Ready || (ref method) == NScreamVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001903 RID: 6403 RVA: 0x0009CC6C File Offset: 0x0009AE6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScreamVfx.PropertyName._continuousParticles)
			{
				this._continuousParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NScreamVfx.PropertyName._oneShotParticles)
			{
				this._oneShotParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NScreamVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001904 RID: 6404 RVA: 0x0009CCD4 File Offset: 0x0009AED4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScreamVfx.PropertyName._continuousParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._continuousParticles);
				return true;
			}
			if ((ref name) == NScreamVfx.PropertyName._oneShotParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._oneShotParticles);
				return true;
			}
			if ((ref name) == NScreamVfx.PropertyName._duration)
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
		// Token: 0x06001905 RID: 6405 RVA: 0x0009CD4C File Offset: 0x0009AF4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NScreamVfx.PropertyName._continuousParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NScreamVfx.PropertyName._oneShotParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(3L, NScreamVfx.PropertyName._duration, 0L, "", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001906 RID: 6406 RVA: 0x0009CDC4 File Offset: 0x0009AFC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NScreamVfx.PropertyName._continuousParticles, Variant.CreateFrom<GpuParticles2D>(this._continuousParticles));
			info.AddProperty(NScreamVfx.PropertyName._oneShotParticles, Variant.CreateFrom<GpuParticles2D>(this._oneShotParticles));
			info.AddProperty(NScreamVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
		}

		/// <inheritdoc />
		// Token: 0x06001907 RID: 6407 RVA: 0x0009CE1C File Offset: 0x0009B01C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScreamVfx.PropertyName._continuousParticles, ref variant))
			{
				this._continuousParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NScreamVfx.PropertyName._oneShotParticles, ref variant2))
			{
				this._oneShotParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NScreamVfx.PropertyName._duration, ref variant3))
			{
				this._duration = variant3.As<float>();
			}
		}

		// Token: 0x04000917 RID: 2327
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_scream");

		// Token: 0x04000918 RID: 2328
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _continuousParticles = new Array<GpuParticles2D>();

		// Token: 0x04000919 RID: 2329
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _oneShotParticles = new Array<GpuParticles2D>();

		// Token: 0x0400091A RID: 2330
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _duration = 1f;

		// Token: 0x0400091B RID: 2331
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200111C RID: 4380
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040042CF RID: 17103
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042D0 RID: 17104
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040042D1 RID: 17105
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200111D RID: 4381
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_continuousParticles' field.
			/// </summary>
			// Token: 0x040042D2 RID: 17106
			public static readonly StringName _continuousParticles = "_continuousParticles";

			/// <summary>
			/// Cached name for the '_oneShotParticles' field.
			/// </summary>
			// Token: 0x040042D3 RID: 17107
			public static readonly StringName _oneShotParticles = "_oneShotParticles";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x040042D4 RID: 17108
			public static readonly StringName _duration = "_duration";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200111E RID: 4382
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
