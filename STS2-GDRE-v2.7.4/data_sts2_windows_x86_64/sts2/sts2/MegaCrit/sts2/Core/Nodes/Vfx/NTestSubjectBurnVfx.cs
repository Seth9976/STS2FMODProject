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
	// Token: 0x02000203 RID: 515
	[ScriptPath("res://src/Core/Nodes/Vfx/NTestSubjectBurnVfx.cs")]
	public class NTestSubjectBurnVfx : ColorRect
	{
		// Token: 0x06001A8E RID: 6798 RVA: 0x000AA24A File Offset: 0x000A844A
		[NullableContext(2)]
		public static NTestSubjectBurnVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NTestSubjectBurnVfx._scenePath).Instantiate<NTestSubjectBurnVfx>(0L);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000AA26C File Offset: 0x000A846C
		public override void _Ready()
		{
			base._Ready();
			base.Modulate = Colors.Transparent;
			Tween tween = base.CreateTween();
			tween.Chain().TweenInterval(0.25);
			tween.Chain().TweenProperty(this, "modulate", Colors.White, 0.30000001192092896);
			tween.Chain().TweenInterval(0.10000000149011612);
			tween.Chain().TweenProperty(this, "modulate", Colors.White * 0.5f, 0.15000000596046448);
			tween.Chain().TweenProperty(this, "modulate", Colors.White, 0.25);
			tween.Chain().TweenInterval(0.3499999940395355);
			tween.Chain().TweenProperty(this, "modulate", Colors.Transparent, 0.30000001192092896);
			tween.Chain().TweenCallback(Callable.From(new Action(base.QueueFreeSafely)));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A90 RID: 6800 RVA: 0x000AA3A0 File Offset: 0x000A85A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NTestSubjectBurnVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("ColorRect"), false), 33L, null, null),
				new MethodInfo(NTestSubjectBurnVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A91 RID: 6801 RVA: 0x000AA420 File Offset: 0x000A8620
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTestSubjectBurnVfx.MethodName.Create && args.Count == 0)
			{
				NTestSubjectBurnVfx ntestSubjectBurnVfx = NTestSubjectBurnVfx.Create();
				ret = VariantUtils.CreateFrom<NTestSubjectBurnVfx>(ref ntestSubjectBurnVfx);
				return true;
			}
			if ((ref method) == NTestSubjectBurnVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000AA488 File Offset: 0x000A8688
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTestSubjectBurnVfx.MethodName.Create && args.Count == 0)
			{
				NTestSubjectBurnVfx ntestSubjectBurnVfx = NTestSubjectBurnVfx.Create();
				ret = VariantUtils.CreateFrom<NTestSubjectBurnVfx>(ref ntestSubjectBurnVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A93 RID: 6803 RVA: 0x000AA4C8 File Offset: 0x000A86C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTestSubjectBurnVfx.MethodName.Create || (ref method) == NTestSubjectBurnVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A94 RID: 6804 RVA: 0x000AA4EF File Offset: 0x000A86EF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001A95 RID: 6805 RVA: 0x000AA4F8 File Offset: 0x000A86F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040009D9 RID: 2521
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/monsters/test_subject_burn_vfx");

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117A RID: 4474
		public class MethodName : ColorRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004422 RID: 17442
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004423 RID: 17443
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117B RID: 4475
		public class PropertyName : ColorRect.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200117C RID: 4476
		public class SignalName : ColorRect.SignalName
		{
		}
	}
}
