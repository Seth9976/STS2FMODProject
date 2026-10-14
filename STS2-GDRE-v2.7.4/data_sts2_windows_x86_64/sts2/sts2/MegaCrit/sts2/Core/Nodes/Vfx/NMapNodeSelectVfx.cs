using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
	// Token: 0x020001D0 RID: 464
	[ScriptPath("res://src/Core/Nodes/Vfx/NMapNodeSelectVfx.cs")]
	public class NMapNodeSelectVfx : Control
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00090F79 File Offset: 0x0008F179
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return NMapNodeSelectVfx._textures.Append("res://scenes/vfx/map_node_select_vfx.tscn");
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00090F8A File Offset: 0x0008F18A
		public override void _Ready()
		{
			this._particles = base.GetNode<GpuParticles2D>("Particles");
			this._particles.Emitting = true;
			TaskHelper.RunSafely(this.Play());
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00090FBA File Offset: 0x0008F1BA
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cancelToken.Cancel();
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00090FD0 File Offset: 0x0008F1D0
		[NullableContext(2)]
		public static NMapNodeSelectVfx Create(float scaleMultiplier)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NMapNodeSelectVfx nmapNodeSelectVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/map_node_select_vfx.tscn").Instantiate<NMapNodeSelectVfx>(0L);
			nmapNodeSelectVfx.Scale = Vector2.One * scaleMultiplier;
			return nmapNodeSelectVfx;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00091010 File Offset: 0x0008F210
		[NullableContext(1)]
		private async Task Play()
		{
			await Task.Delay(1000, this._cancelToken.Token);
			if (!this._cancelToken.IsCancellationRequested)
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001763 RID: 5987 RVA: 0x00091054 File Offset: 0x0008F254
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapNodeSelectVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapNodeSelectVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapNodeSelectVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "scaleMultiplier", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001764 RID: 5988 RVA: 0x00091128 File Offset: 0x0008F328
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapNodeSelectVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapNodeSelectVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapNodeSelectVfx.MethodName.Create && args.Count == 1)
			{
				NMapNodeSelectVfx nmapNodeSelectVfx = NMapNodeSelectVfx.Create(VariantUtils.ConvertTo<float>(args[0]));
				ret = VariantUtils.CreateFrom<NMapNodeSelectVfx>(ref nmapNodeSelectVfx);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000911C4 File Offset: 0x0008F3C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapNodeSelectVfx.MethodName.Create && args.Count == 1)
			{
				NMapNodeSelectVfx nmapNodeSelectVfx = NMapNodeSelectVfx.Create(VariantUtils.ConvertTo<float>(args[0]));
				ret = VariantUtils.CreateFrom<NMapNodeSelectVfx>(ref nmapNodeSelectVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001766 RID: 5990 RVA: 0x00091212 File Offset: 0x0008F412
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapNodeSelectVfx.MethodName._Ready || (ref method) == NMapNodeSelectVfx.MethodName._ExitTree || (ref method) == NMapNodeSelectVfx.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001767 RID: 5991 RVA: 0x00091248 File Offset: 0x0008F448
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapNodeSelectVfx.PropertyName._lifeTimer)
			{
				this._lifeTimer = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NMapNodeSelectVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001768 RID: 5992 RVA: 0x00091288 File Offset: 0x0008F488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapNodeSelectVfx.PropertyName._lifeTimer)
			{
				value = VariantUtils.CreateFrom<double>(ref this._lifeTimer);
				return true;
			}
			if ((ref name) == NMapNodeSelectVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001769 RID: 5993 RVA: 0x000912E0 File Offset: 0x0008F4E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NMapNodeSelectVfx.PropertyName._lifeTimer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapNodeSelectVfx.PropertyName._particles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600176A RID: 5994 RVA: 0x00091335 File Offset: 0x0008F535
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapNodeSelectVfx.PropertyName._lifeTimer, Variant.From<double>(ref this._lifeTimer));
			info.AddProperty(NMapNodeSelectVfx.PropertyName._particles, Variant.From<GpuParticles2D>(ref this._particles));
		}

		/// <inheritdoc />
		// Token: 0x0600176B RID: 5995 RVA: 0x0009136C File Offset: 0x0008F56C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapNodeSelectVfx.PropertyName._lifeTimer, ref variant))
			{
				this._lifeTimer = variant.As<double>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapNodeSelectVfx.PropertyName._particles, ref variant2))
			{
				this._particles = variant2.As<GpuParticles2D>();
			}
		}

		// Token: 0x0400085D RID: 2141
		[Nullable(1)]
		private const string _path = "res://scenes/vfx/map_node_select_vfx.tscn";

		// Token: 0x0400085E RID: 2142
		[Nullable(1)]
		private static readonly string[] _textures = new string[] { "res://images/vfx/brush_particle_2.png" };

		// Token: 0x0400085F RID: 2143
		private double _lifeTimer;

		// Token: 0x04000860 RID: 2144
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		// Token: 0x04000861 RID: 2145
		[Nullable(1)]
		private GpuParticles2D _particles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B7 RID: 4279
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400419C RID: 16796
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400419D RID: 16797
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400419E RID: 16798
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B8 RID: 4280
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_lifeTimer' field.
			/// </summary>
			// Token: 0x0400419F RID: 16799
			public static readonly StringName _lifeTimer = "_lifeTimer";

			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040041A0 RID: 16800
			public static readonly StringName _particles = "_particles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B9 RID: 4281
		public class SignalName : Control.SignalName
		{
		}
	}
}
