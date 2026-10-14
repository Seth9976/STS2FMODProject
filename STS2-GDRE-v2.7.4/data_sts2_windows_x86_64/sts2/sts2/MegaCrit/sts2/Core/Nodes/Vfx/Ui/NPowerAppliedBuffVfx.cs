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
	// Token: 0x0200022A RID: 554
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NPowerAppliedBuffVfx.cs")]
	public class NPowerAppliedBuffVfx : Node2D
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x000B6E25 File Offset: 0x000B5025
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NPowerAppliedBuffVfx._scenePath);
			}
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000B6E31 File Offset: 0x000B5031
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000B6E44 File Offset: 0x000B5044
		[NullableContext(2)]
		public static NPowerAppliedBuffVfx Create(Vector2 globalPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NPowerAppliedBuffVfx npowerAppliedBuffVfx = PreloadManager.Cache.GetScene(NPowerAppliedBuffVfx._scenePath).Instantiate<NPowerAppliedBuffVfx>(0L);
			npowerAppliedBuffVfx.GlobalPosition = globalPosition;
			return npowerAppliedBuffVfx;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000B6E79 File Offset: 0x000B5079
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000B6E88 File Offset: 0x000B5088
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C21 RID: 7201 RVA: 0x000B6ECC File Offset: 0x000B50CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NPowerAppliedBuffVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPowerAppliedBuffVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "globalPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NPowerAppliedBuffVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C22 RID: 7202 RVA: 0x000B6FA0 File Offset: 0x000B51A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerAppliedBuffVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPowerAppliedBuffVfx.MethodName.Create && args.Count == 1)
			{
				NPowerAppliedBuffVfx npowerAppliedBuffVfx = NPowerAppliedBuffVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerAppliedBuffVfx>(ref npowerAppliedBuffVfx);
				return true;
			}
			if ((ref method) == NPowerAppliedBuffVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000B703C File Offset: 0x000B523C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPowerAppliedBuffVfx.MethodName.Create && args.Count == 1)
			{
				NPowerAppliedBuffVfx npowerAppliedBuffVfx = NPowerAppliedBuffVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NPowerAppliedBuffVfx>(ref npowerAppliedBuffVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001C24 RID: 7204 RVA: 0x000B708A File Offset: 0x000B528A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPowerAppliedBuffVfx.MethodName._ExitTree || (ref method) == NPowerAppliedBuffVfx.MethodName.Create || (ref method) == NPowerAppliedBuffVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C25 RID: 7205 RVA: 0x000B70C0 File Offset: 0x000B52C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPowerAppliedBuffVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C26 RID: 7206 RVA: 0x000B70E5 File Offset: 0x000B52E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPowerAppliedBuffVfx.PropertyName._particles)
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
		// Token: 0x06001C27 RID: 7207 RVA: 0x000B7110 File Offset: 0x000B5310
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NPowerAppliedBuffVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C28 RID: 7208 RVA: 0x000B7146 File Offset: 0x000B5346
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPowerAppliedBuffVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
		}

		/// <inheritdoc />
		// Token: 0x06001C29 RID: 7209 RVA: 0x000B7168 File Offset: 0x000B5368
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPowerAppliedBuffVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
		}

		// Token: 0x04000ACA RID: 2762
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_buff_applied");

		// Token: 0x04000ACB RID: 2763
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x04000ACC RID: 2764
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D7 RID: 4567
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004562 RID: 17762
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004563 RID: 17763
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004564 RID: 17764
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D8 RID: 4568
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04004565 RID: 17765
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011D9 RID: 4569
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
