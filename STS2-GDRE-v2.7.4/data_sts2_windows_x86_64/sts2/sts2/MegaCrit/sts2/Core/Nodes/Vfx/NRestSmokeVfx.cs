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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E8 RID: 488
	[ScriptPath("res://src/Core/Nodes/Vfx/NRestSmokeVfx.cs")]
	public class NRestSmokeVfx : Node2D
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x0009B95B File Offset: 0x00099B5B
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NRestSmokeVfx.ScenePath);
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0009B967 File Offset: 0x00099B67
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0009B979 File Offset: 0x00099B79
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/rest_smoke_vfx");
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0009B988 File Offset: 0x00099B88
		[NullableContext(2)]
		public static NRestSmokeVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NRestSmokeVfx.ScenePath).Instantiate<NRestSmokeVfx>(0L);
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0009B9B8 File Offset: 0x00099BB8
		public override void _Ready()
		{
			this._clouds = base.GetNode<GpuParticles2D>("Clouds");
			this._clouds.Emitting = true;
			base.GlobalPosition = base.GetViewport().GetVisibleRect().GetCenter();
			TaskHelper.RunSafely(this.DeleteWhenFinished());
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0009BA0C File Offset: 0x00099C0C
		[NullableContext(1)]
		private async Task DeleteWhenFinished()
		{
			this._cts = new CancellationTokenSource();
			await Task.Delay(4000, this._cts.Token);
			if (GodotObject.IsInstanceValid(this))
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018C9 RID: 6345 RVA: 0x0009BA50 File Offset: 0x00099C50
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRestSmokeVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRestSmokeVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, null, null),
				new MethodInfo(NRestSmokeVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018CA RID: 6346 RVA: 0x0009BB00 File Offset: 0x00099D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSmokeVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRestSmokeVfx.MethodName.Create && args.Count == 0)
			{
				NRestSmokeVfx nrestSmokeVfx = NRestSmokeVfx.Create();
				ret = VariantUtils.CreateFrom<NRestSmokeVfx>(ref nrestSmokeVfx);
				return true;
			}
			if ((ref method) == NRestSmokeVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0009BB8C File Offset: 0x00099D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRestSmokeVfx.MethodName.Create && args.Count == 0)
			{
				NRestSmokeVfx nrestSmokeVfx = NRestSmokeVfx.Create();
				ret = VariantUtils.CreateFrom<NRestSmokeVfx>(ref nrestSmokeVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060018CC RID: 6348 RVA: 0x0009BBCC File Offset: 0x00099DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRestSmokeVfx.MethodName._ExitTree || (ref method) == NRestSmokeVfx.MethodName.Create || (ref method) == NRestSmokeVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060018CD RID: 6349 RVA: 0x0009BC02 File Offset: 0x00099E02
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRestSmokeVfx.PropertyName._clouds)
			{
				this._clouds = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060018CE RID: 6350 RVA: 0x0009BC27 File Offset: 0x00099E27
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRestSmokeVfx.PropertyName._clouds)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._clouds);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060018CF RID: 6351 RVA: 0x0009BC54 File Offset: 0x00099E54
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRestSmokeVfx.PropertyName._clouds, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060018D0 RID: 6352 RVA: 0x0009BC89 File Offset: 0x00099E89
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRestSmokeVfx.PropertyName._clouds, Variant.From<GpuParticles2D>(ref this._clouds));
		}

		/// <inheritdoc />
		// Token: 0x060018D1 RID: 6353 RVA: 0x0009BCA8 File Offset: 0x00099EA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRestSmokeVfx.PropertyName._clouds, ref variant))
			{
				this._clouds = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040008FF RID: 2303
		[Nullable(1)]
		private GpuParticles2D _clouds;

		// Token: 0x04000900 RID: 2304
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200110E RID: 4366
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040042A6 RID: 17062
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040042A7 RID: 17063
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040042A8 RID: 17064
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200110F RID: 4367
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_clouds' field.
			/// </summary>
			// Token: 0x040042A9 RID: 17065
			public static readonly StringName _clouds = "_clouds";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001110 RID: 4368
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
