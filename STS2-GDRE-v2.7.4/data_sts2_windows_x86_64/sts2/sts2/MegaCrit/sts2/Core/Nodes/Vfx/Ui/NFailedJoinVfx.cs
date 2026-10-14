using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui
{
	// Token: 0x02000227 RID: 551
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NFailedJoinVfx.cs")]
	public class NFailedJoinVfx : Control
	{
		// Token: 0x06001BEC RID: 7148 RVA: 0x000B55C3 File Offset: 0x000B37C3
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayAndSelfDestruct());
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000B55D4 File Offset: 0x000B37D4
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NFailedJoinVfx Create(string text)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NFailedJoinVfx nfailedJoinVfx = PreloadManager.Cache.GetScene(NFailedJoinVfx._scenePath).Instantiate<NFailedJoinVfx>(0L);
			nfailedJoinVfx.GetNode<MegaRichTextLabel>("%Label").SetTextAutoSize(text);
			return nfailedJoinVfx;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000B5618 File Offset: 0x000B3818
		[NullableContext(1)]
		private async Task PlayAndSelfDestruct()
		{
			base.Modulate = StsColors.transparentWhite;
			Vector2 position = base.Position;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.05);
			for (int i = 0; i < 5; i++)
			{
				float num = 1f - (float)i / 5f;
				float num2 = 24f * num * (float)((i % 2 == 0) ? 1 : (-1));
				this._tween.Chain().TweenProperty(this, "position:x", position.X + num2, 0.05000000074505806).SetTrans(1L)
					.SetEase(2L);
			}
			this._tween.Chain().TweenProperty(this, "position:x", position.X, 0.05000000074505806).SetTrans(1L)
				.SetEase(1L);
			this._tween.TweenInterval(3.0);
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.5);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.QueueFreeSafely();
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000B565B File Offset: 0x000B385B
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BF0 RID: 7152 RVA: 0x000B5670 File Offset: 0x000B3870
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NFailedJoinVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFailedJoinVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFailedJoinVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BF1 RID: 7153 RVA: 0x000B5744 File Offset: 0x000B3944
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFailedJoinVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFailedJoinVfx.MethodName.Create && args.Count == 1)
			{
				NFailedJoinVfx nfailedJoinVfx = NFailedJoinVfx.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NFailedJoinVfx>(ref nfailedJoinVfx);
				return true;
			}
			if ((ref method) == NFailedJoinVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000B57E0 File Offset: 0x000B39E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFailedJoinVfx.MethodName.Create && args.Count == 1)
			{
				NFailedJoinVfx nfailedJoinVfx = NFailedJoinVfx.Create(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<NFailedJoinVfx>(ref nfailedJoinVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001BF3 RID: 7155 RVA: 0x000B582E File Offset: 0x000B3A2E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFailedJoinVfx.MethodName._Ready || (ref method) == NFailedJoinVfx.MethodName.Create || (ref method) == NFailedJoinVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001BF4 RID: 7156 RVA: 0x000B5864 File Offset: 0x000B3A64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFailedJoinVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NFailedJoinVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BF5 RID: 7157 RVA: 0x000B58A4 File Offset: 0x000B3AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFailedJoinVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NFailedJoinVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BF6 RID: 7158 RVA: 0x000B58FC File Offset: 0x000B3AFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFailedJoinVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFailedJoinVfx.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BF7 RID: 7159 RVA: 0x000B5952 File Offset: 0x000B3B52
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFailedJoinVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NFailedJoinVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06001BF8 RID: 7160 RVA: 0x000B5988 File Offset: 0x000B3B88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFailedJoinVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFailedJoinVfx.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
		}

		// Token: 0x04000AAA RID: 2730
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_failed_join");

		// Token: 0x04000AAB RID: 2731
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000AAC RID: 2732
		[Nullable(1)]
		private MegaRichTextLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011CB RID: 4555
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004534 RID: 17716
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004535 RID: 17717
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004536 RID: 17718
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011CC RID: 4556
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004537 RID: 17719
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004538 RID: 17720
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011CD RID: 4557
		public class SignalName : Control.SignalName
		{
		}
	}
}
