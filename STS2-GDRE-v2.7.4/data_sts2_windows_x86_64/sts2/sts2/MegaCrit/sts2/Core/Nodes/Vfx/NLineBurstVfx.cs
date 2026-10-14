using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001CB RID: 459
	[ScriptPath("res://src/Core/Nodes/Vfx/NLineBurstVfx.cs")]
	public class NLineBurstVfx : GpuParticles2D
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x0008F1CD File Offset: 0x0008D3CD
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NLineBurstVfx.ScenePath);
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x0008F1D9 File Offset: 0x0008D3D9
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_line_burst");
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0008F1E8 File Offset: 0x0008D3E8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NLineBurstVfx Create(Creature target)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NLineBurstVfx nlineBurstVfx = PreloadManager.Cache.GetScene(NLineBurstVfx.ScenePath).Instantiate<NLineBurstVfx>(0L);
			nlineBurstVfx.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
			return nlineBurstVfx;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0008F22C File Offset: 0x0008D42C
		[NullableContext(2)]
		public static NLineBurstVfx Create(Vector2 position)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NLineBurstVfx nlineBurstVfx = PreloadManager.Cache.GetScene(NLineBurstVfx.ScenePath).Instantiate<NLineBurstVfx>(0L);
			nlineBurstVfx.GlobalPosition = position;
			return nlineBurstVfx;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0008F261 File Offset: 0x0008D461
		public override void _Ready()
		{
			base.Emitting = true;
			TaskHelper.RunSafely(this.DeleteAfterComplete());
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0008F278 File Offset: 0x0008D478
		[NullableContext(1)]
		private async Task DeleteAfterComplete()
		{
			await this.AwaitSignal(GpuParticles2D.SignalName.Finished, this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001717 RID: 5911 RVA: 0x0008F2BC File Offset: 0x0008D4BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NLineBurstVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("GPUParticles2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NLineBurstVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001718 RID: 5912 RVA: 0x0008F360 File Offset: 0x0008D560
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLineBurstVfx.MethodName.Create && args.Count == 1)
			{
				NLineBurstVfx nlineBurstVfx = NLineBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NLineBurstVfx>(ref nlineBurstVfx);
				return true;
			}
			if ((ref method) == NLineBurstVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0008F3D4 File Offset: 0x0008D5D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLineBurstVfx.MethodName.Create && args.Count == 1)
			{
				NLineBurstVfx nlineBurstVfx = NLineBurstVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NLineBurstVfx>(ref nlineBurstVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600171A RID: 5914 RVA: 0x0008F422 File Offset: 0x0008D622
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLineBurstVfx.MethodName.Create || (ref method) == NLineBurstVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600171B RID: 5915 RVA: 0x0008F449 File Offset: 0x0008D649
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600171C RID: 5916 RVA: 0x0008F452 File Offset: 0x0008D652
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A4 RID: 4260
		public class MethodName : GpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x0400416C RID: 16748
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400416D RID: 16749
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A5 RID: 4261
		public class PropertyName : GpuParticles2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010A6 RID: 4262
		public class SignalName : GpuParticles2D.SignalName
		{
		}
	}
}
