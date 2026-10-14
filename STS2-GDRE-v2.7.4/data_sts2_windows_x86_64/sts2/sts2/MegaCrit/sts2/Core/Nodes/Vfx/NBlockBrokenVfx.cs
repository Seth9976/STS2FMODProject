using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000189 RID: 393
	[ScriptPath("res://src/Core/Nodes/Vfx/NBlockBrokenVfx.cs")]
	public class NBlockBrokenVfx : Sprite2D
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00072643 File Offset: 0x00070843
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_block_broken.tscn");
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00072650 File Offset: 0x00070850
		public override void _Ready()
		{
			AnimationPlayer node = base.GetNode<AnimationPlayer>("AnimationPlayer");
			node.Connect(AnimationMixer.SignalName.AnimationFinished, Callable.From<StringName>(new Action<StringName>(this.OnAnimationFinished)), 0U);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0007268C File Offset: 0x0007088C
		[NullableContext(1)]
		private void OnAnimationFinished(StringName _)
		{
			this.QueueFreeSafely();
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00072694 File Offset: 0x00070894
		[NullableContext(2)]
		public static NBlockBrokenVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_block_broken.tscn").Instantiate<NBlockBrokenVfx>(0L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001310 RID: 4880 RVA: 0x000726B8 File Offset: 0x000708B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBlockBrokenVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBlockBrokenVfx.MethodName.OnAnimationFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(21L, "_", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBlockBrokenVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Sprite2D"), false), 33L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001311 RID: 4881 RVA: 0x0007278C File Offset: 0x0007098C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBlockBrokenVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBlockBrokenVfx.MethodName.OnAnimationFinished && args.Count == 1)
			{
				this.OnAnimationFinished(VariantUtils.ConvertTo<StringName>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBlockBrokenVfx.MethodName.Create && args.Count == 0)
			{
				NBlockBrokenVfx nblockBrokenVfx = NBlockBrokenVfx.Create();
				ret = VariantUtils.CreateFrom<NBlockBrokenVfx>(ref nblockBrokenVfx);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00072828 File Offset: 0x00070A28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBlockBrokenVfx.MethodName.Create && args.Count == 0)
			{
				NBlockBrokenVfx nblockBrokenVfx = NBlockBrokenVfx.Create();
				ret = VariantUtils.CreateFrom<NBlockBrokenVfx>(ref nblockBrokenVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001313 RID: 4883 RVA: 0x00072868 File Offset: 0x00070A68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBlockBrokenVfx.MethodName._Ready || (ref method) == NBlockBrokenVfx.MethodName.OnAnimationFinished || (ref method) == NBlockBrokenVfx.MethodName.Create || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001314 RID: 4884 RVA: 0x0007289E File Offset: 0x00070A9E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001315 RID: 4885 RVA: 0x000728A7 File Offset: 0x00070AA7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040006BF RID: 1727
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/vfx_block_broken.tscn";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FAE RID: 4014
		public class MethodName : Sprite2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E7E RID: 15998
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationFinished' method.
			/// </summary>
			// Token: 0x04003E7F RID: 15999
			public static readonly StringName OnAnimationFinished = "OnAnimationFinished";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003E80 RID: 16000
			public static readonly StringName Create = "Create";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FAF RID: 4015
		public class PropertyName : Sprite2D.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB0 RID: 4016
		public class SignalName : Sprite2D.SignalName
		{
		}
	}
}
