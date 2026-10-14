using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001E3 RID: 483
	[ScriptPath("res://src/Core/Nodes/Vfx/NRadialBlurVfx.cs")]
	public class NRadialBlurVfx : BackBufferCopy
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x00098F44 File Offset: 0x00097144
		public override void _Ready()
		{
			this._rect = base.GetNode<Control>("Rect");
			this._blurShader = (ShaderMaterial)this._rect.GetMaterial();
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00098F74 File Offset: 0x00097174
		public void Activate(VfxPosition vfxPosition = VfxPosition.Center)
		{
			if (TestMode.IsOn || base.Visible)
			{
				return;
			}
			base.Visible = true;
			this._rect.SetDeferred(Control.PropertyName.Size, base.GetViewportRect().Size);
			if (vfxPosition != VfxPosition.Left)
			{
				if (vfxPosition != VfxPosition.Right)
				{
					this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, 0.45f);
				}
				else
				{
					this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, 0.6f);
				}
			}
			else
			{
				this._blurShader.SetShaderParameter(NRadialBlurVfx._blurCenterx, 0.3f);
			}
			TaskHelper.RunSafely(this.Animate());
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00099023 File Offset: 0x00097223
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00099038 File Offset: 0x00097238
		[NullableContext(1)]
		private async Task Animate()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._blurShader, "shader_parameter/blur_power", 0.005f, 0.10000000149011612);
			this._tween.Chain();
			this._tween.TweenProperty(this._blurShader, "shader_parameter/blur_power", 0f, 0.8999999761581421).SetEase(0L).SetTrans(7L);
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
				base.Visible = false;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600187E RID: 6270 RVA: 0x0009907C File Offset: 0x0009727C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NRadialBlurVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRadialBlurVfx.MethodName.Activate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "vfxPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRadialBlurVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600187F RID: 6271 RVA: 0x00099144 File Offset: 0x00097344
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRadialBlurVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRadialBlurVfx.MethodName.Activate && args.Count == 1)
			{
				this.Activate(VariantUtils.ConvertTo<VfxPosition>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRadialBlurVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001880 RID: 6272 RVA: 0x000991D7 File Offset: 0x000973D7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRadialBlurVfx.MethodName._Ready || (ref method) == NRadialBlurVfx.MethodName.Activate || (ref method) == NRadialBlurVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001881 RID: 6273 RVA: 0x00099210 File Offset: 0x00097410
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRadialBlurVfx.PropertyName._blurShader)
			{
				this._blurShader = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._vfxPosition)
			{
				this._vfxPosition = VariantUtils.ConvertTo<VfxPosition>(ref value);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._rect)
			{
				this._rect = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001882 RID: 6274 RVA: 0x00099294 File Offset: 0x00097494
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRadialBlurVfx.PropertyName._blurShader)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blurShader);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._vfxPosition)
			{
				value = VariantUtils.CreateFrom<VfxPosition>(ref this._vfxPosition);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._rect)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._rect);
				return true;
			}
			if ((ref name) == NRadialBlurVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001883 RID: 6275 RVA: 0x0009932C File Offset: 0x0009752C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRadialBlurVfx.PropertyName._blurShader, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRadialBlurVfx.PropertyName._vfxPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRadialBlurVfx.PropertyName._rect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRadialBlurVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001884 RID: 6276 RVA: 0x000993C4 File Offset: 0x000975C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRadialBlurVfx.PropertyName._blurShader, Variant.From<ShaderMaterial>(ref this._blurShader));
			info.AddProperty(NRadialBlurVfx.PropertyName._vfxPosition, Variant.From<VfxPosition>(ref this._vfxPosition));
			info.AddProperty(NRadialBlurVfx.PropertyName._rect, Variant.From<Control>(ref this._rect));
			info.AddProperty(NRadialBlurVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001885 RID: 6277 RVA: 0x00099430 File Offset: 0x00097630
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRadialBlurVfx.PropertyName._blurShader, ref variant))
			{
				this._blurShader = variant.As<ShaderMaterial>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRadialBlurVfx.PropertyName._vfxPosition, ref variant2))
			{
				this._vfxPosition = variant2.As<VfxPosition>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRadialBlurVfx.PropertyName._rect, ref variant3))
			{
				this._rect = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRadialBlurVfx.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x040008D2 RID: 2258
		[Nullable(1)]
		private static readonly StringName _blurCenterx = new StringName("blur_center:x");

		// Token: 0x040008D3 RID: 2259
		[Nullable(1)]
		private ShaderMaterial _blurShader;

		// Token: 0x040008D4 RID: 2260
		private VfxPosition _vfxPosition;

		// Token: 0x040008D5 RID: 2261
		[Nullable(1)]
		private Control _rect;

		// Token: 0x040008D6 RID: 2262
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FD RID: 4349
		public class MethodName : BackBufferCopy.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004268 RID: 17000
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Activate' method.
			/// </summary>
			// Token: 0x04004269 RID: 17001
			public static readonly StringName Activate = "Activate";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400426A RID: 17002
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FE RID: 4350
		public class PropertyName : BackBufferCopy.PropertyName
		{
			/// <summary>
			/// Cached name for the '_blurShader' field.
			/// </summary>
			// Token: 0x0400426B RID: 17003
			public static readonly StringName _blurShader = "_blurShader";

			/// <summary>
			/// Cached name for the '_vfxPosition' field.
			/// </summary>
			// Token: 0x0400426C RID: 17004
			public static readonly StringName _vfxPosition = "_vfxPosition";

			/// <summary>
			/// Cached name for the '_rect' field.
			/// </summary>
			// Token: 0x0400426D RID: 17005
			public static readonly StringName _rect = "_rect";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400426E RID: 17006
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FF RID: 4351
		public class SignalName : BackBufferCopy.SignalName
		{
		}
	}
}
