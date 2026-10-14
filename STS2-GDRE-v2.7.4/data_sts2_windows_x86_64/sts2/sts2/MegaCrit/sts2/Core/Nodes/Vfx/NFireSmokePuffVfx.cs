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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Fire Explosion used by cards and potions
	/// </summary>
	// Token: 0x020001AA RID: 426
	[ScriptPath("res://src/Core/Nodes/Vfx/NFireSmokePuffVfx.cs")]
	public class NFireSmokePuffVfx : Node2D
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x00080F53 File Offset: 0x0007F153
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NFireSmokePuffVfx.ScenePath);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00080F5F File Offset: 0x0007F15F
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x00080F71 File Offset: 0x0007F171
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_fire_smoke_puff");
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00080F80 File Offset: 0x0007F180
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NFireSmokePuffVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFireSmokePuffVfx nfireSmokePuffVfx = PreloadManager.Cache.GetScene(NFireSmokePuffVfx.ScenePath).Instantiate<NFireSmokePuffVfx>(0L);
			nfireSmokePuffVfx.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
			return nfireSmokePuffVfx;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00080FC4 File Offset: 0x0007F1C4
		public override void _Ready()
		{
			this._ember = base.GetNode<GpuParticles2D>("Ember");
			this._clouds = base.GetNode<GpuParticles2D>("Clouds");
			this._ember.Emitting = true;
			this._clouds.Emitting = true;
			TaskHelper.RunSafely(this.DeleteAfterComplete());
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00081024 File Offset: 0x0007F224
		[NullableContext(1)]
		private async Task DeleteAfterComplete()
		{
			this._cts = new CancellationTokenSource();
			await Task.Delay(2500, this._cts.Token);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001522 RID: 5410 RVA: 0x00081068 File Offset: 0x0007F268
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NFireSmokePuffVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFireSmokePuffVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001523 RID: 5411 RVA: 0x000810DC File Offset: 0x0007F2DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFireSmokePuffVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFireSmokePuffVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001524 RID: 5412 RVA: 0x0008113C File Offset: 0x0007F33C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFireSmokePuffVfx.MethodName._ExitTree || (ref method) == NFireSmokePuffVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001525 RID: 5413 RVA: 0x00081163 File Offset: 0x0007F363
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFireSmokePuffVfx.PropertyName._clouds)
			{
				this._clouds = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFireSmokePuffVfx.PropertyName._ember)
			{
				this._ember = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001526 RID: 5414 RVA: 0x000811A4 File Offset: 0x0007F3A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFireSmokePuffVfx.PropertyName._clouds)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._clouds);
				return true;
			}
			if ((ref name) == NFireSmokePuffVfx.PropertyName._ember)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ember);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001527 RID: 5415 RVA: 0x000811FC File Offset: 0x0007F3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFireSmokePuffVfx.PropertyName._clouds, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFireSmokePuffVfx.PropertyName._ember, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001528 RID: 5416 RVA: 0x00081252 File Offset: 0x0007F452
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFireSmokePuffVfx.PropertyName._clouds, Variant.From<GpuParticles2D>(ref this._clouds));
			info.AddProperty(NFireSmokePuffVfx.PropertyName._ember, Variant.From<GpuParticles2D>(ref this._ember));
		}

		/// <inheritdoc />
		// Token: 0x06001529 RID: 5417 RVA: 0x00081288 File Offset: 0x0007F488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFireSmokePuffVfx.PropertyName._clouds, ref variant))
			{
				this._clouds = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFireSmokePuffVfx.PropertyName._ember, ref variant2))
			{
				this._ember = variant2.As<GpuParticles2D>();
			}
		}

		// Token: 0x0400078B RID: 1931
		[Nullable(1)]
		private GpuParticles2D _clouds;

		// Token: 0x0400078C RID: 1932
		[Nullable(1)]
		private GpuParticles2D _ember;

		// Token: 0x0400078D RID: 1933
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200102C RID: 4140
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400400A RID: 16394
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400400B RID: 16395
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200102D RID: 4141
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_clouds' field.
			/// </summary>
			// Token: 0x0400400C RID: 16396
			public static readonly StringName _clouds = "_clouds";

			/// <summary>
			/// Cached name for the '_ember' field.
			/// </summary>
			// Token: 0x0400400D RID: 16397
			public static readonly StringName _ember = "_ember";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200102E RID: 4142
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
