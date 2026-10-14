using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001BC RID: 444
	[ScriptPath("res://src/Core/Nodes/Vfx/NHorizontalLinesVfx.cs")]
	public class NHorizontalLinesVfx : GpuParticles2D
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x000871F3 File Offset: 0x000853F3
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NHorizontalLinesVfx._scenePath);
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00087200 File Offset: 0x00085400
		[NullableContext(2)]
		public static NHorizontalLinesVfx Create(Color color, double duration = 2.0, bool movingRightwards = true)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NHorizontalLinesVfx nhorizontalLinesVfx = PreloadManager.Cache.GetScene(NHorizontalLinesVfx._scenePath).Instantiate<NHorizontalLinesVfx>(0L);
			nhorizontalLinesVfx._duration = Mathf.Max(1.0, duration);
			nhorizontalLinesVfx._mat = (ParticleProcessMaterial)nhorizontalLinesVfx.ProcessMaterial;
			nhorizontalLinesVfx._mat.Color = color;
			nhorizontalLinesVfx._isMovingRight = movingRightwards;
			return nhorizontalLinesVfx;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00087268 File Offset: 0x00085468
		public override void _Ready()
		{
			Control parent = base.GetParent<Control>();
			this._mat.EmissionShapeOffset = new Vector3(-500f, parent.Size.Y * 0.5f, 0f);
			this._mat.EmissionShapeScale = new Vector3(200f, parent.Size.Y * 0.5f, 1f);
			if (!this._isMovingRight)
			{
				base.RotationDegrees = 180f;
				base.Position = new Vector2(parent.Size.X, parent.Size.Y);
			}
			TaskHelper.RunSafely(this.PlayAnim());
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00087312 File Offset: 0x00085512
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00087324 File Offset: 0x00085524
		[NullableContext(1)]
		private async Task PlayAnim()
		{
			base.Modulate = StsColors.transparentWhite;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.45);
			this._tween.Chain();
			this._tween.TweenInterval(this._duration - 0.9);
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 0.45);
			bool flag = await this._tween.AwaitFinished(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001624 RID: 5668 RVA: 0x00087368 File Offset: 0x00085568
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NHorizontalLinesVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("GPUParticles2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(20L, "color", 0L, "", 6L, false),
					new PropertyInfo(3L, "duration", 0L, "", 6L, false),
					new PropertyInfo(1L, "movingRightwards", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHorizontalLinesVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHorizontalLinesVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001625 RID: 5669 RVA: 0x00087480 File Offset: 0x00085680
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHorizontalLinesVfx.MethodName.Create && args.Count == 3)
			{
				NHorizontalLinesVfx nhorizontalLinesVfx = NHorizontalLinesVfx.Create(VariantUtils.ConvertTo<Color>(args[0]), VariantUtils.ConvertTo<double>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NHorizontalLinesVfx>(ref nhorizontalLinesVfx);
				return true;
			}
			if ((ref method) == NHorizontalLinesVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHorizontalLinesVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00087534 File Offset: 0x00085734
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHorizontalLinesVfx.MethodName.Create && args.Count == 3)
			{
				NHorizontalLinesVfx nhorizontalLinesVfx = NHorizontalLinesVfx.Create(VariantUtils.ConvertTo<Color>(args[0]), VariantUtils.ConvertTo<double>(args[1]), VariantUtils.ConvertTo<bool>(args[2]));
				ret = VariantUtils.CreateFrom<NHorizontalLinesVfx>(ref nhorizontalLinesVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001627 RID: 5671 RVA: 0x0008759C File Offset: 0x0008579C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHorizontalLinesVfx.MethodName.Create || (ref method) == NHorizontalLinesVfx.MethodName._Ready || (ref method) == NHorizontalLinesVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001628 RID: 5672 RVA: 0x000875D4 File Offset: 0x000857D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHorizontalLinesVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._mat)
			{
				this._mat = VariantUtils.ConvertTo<ParticleProcessMaterial>(ref value);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._isMovingRight)
			{
				this._isMovingRight = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001629 RID: 5673 RVA: 0x00087658 File Offset: 0x00085858
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHorizontalLinesVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<double>(ref this._duration);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._mat)
			{
				value = VariantUtils.CreateFrom<ParticleProcessMaterial>(ref this._mat);
				return true;
			}
			if ((ref name) == NHorizontalLinesVfx.PropertyName._isMovingRight)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isMovingRight);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600162A RID: 5674 RVA: 0x000876F0 File Offset: 0x000858F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHorizontalLinesVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NHorizontalLinesVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHorizontalLinesVfx.PropertyName._mat, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHorizontalLinesVfx.PropertyName._isMovingRight, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600162B RID: 5675 RVA: 0x00087788 File Offset: 0x00085988
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHorizontalLinesVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NHorizontalLinesVfx.PropertyName._duration, Variant.From<double>(ref this._duration));
			info.AddProperty(NHorizontalLinesVfx.PropertyName._mat, Variant.From<ParticleProcessMaterial>(ref this._mat));
			info.AddProperty(NHorizontalLinesVfx.PropertyName._isMovingRight, Variant.From<bool>(ref this._isMovingRight));
		}

		/// <inheritdoc />
		// Token: 0x0600162C RID: 5676 RVA: 0x000877F4 File Offset: 0x000859F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._duration, ref variant2))
			{
				this._duration = variant2.As<double>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._mat, ref variant3))
			{
				this._mat = variant3.As<ParticleProcessMaterial>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHorizontalLinesVfx.PropertyName._isMovingRight, ref variant4))
			{
				this._isMovingRight = variant4.As<bool>();
			}
		}

		// Token: 0x040007DC RID: 2012
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040007DD RID: 2013
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/whole_screen/horizontal_lines_vfx");

		// Token: 0x040007DE RID: 2014
		private double _duration;

		// Token: 0x040007DF RID: 2015
		[Nullable(1)]
		private ParticleProcessMaterial _mat;

		// Token: 0x040007E0 RID: 2016
		private bool _isMovingRight;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200106E RID: 4206
		public class MethodName : GpuParticles2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040040AD RID: 16557
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040AE RID: 16558
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040040AF RID: 16559
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200106F RID: 4207
		public class PropertyName : GpuParticles2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040040B0 RID: 16560
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x040040B1 RID: 16561
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_mat' field.
			/// </summary>
			// Token: 0x040040B2 RID: 16562
			public static readonly StringName _mat = "_mat";

			/// <summary>
			/// Cached name for the '_isMovingRight' field.
			/// </summary>
			// Token: 0x040040B3 RID: 16563
			public static readonly StringName _isMovingRight = "_isMovingRight";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001070 RID: 4208
		public class SignalName : GpuParticles2D.SignalName
		{
		}
	}
}
