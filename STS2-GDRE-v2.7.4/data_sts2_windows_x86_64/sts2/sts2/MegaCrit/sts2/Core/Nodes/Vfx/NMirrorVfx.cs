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
	// Token: 0x020001D4 RID: 468
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NMirrorVfx.cs")]
	public class NMirrorVfx : Control
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x000934FC File Offset: 0x000916FC
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/whole_screen/mirror_vfx");
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00093508 File Offset: 0x00091708
		[NullableContext(2)]
		public static NMirrorVfx Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NMirrorVfx.ScenePath).Instantiate<NMirrorVfx>(0L);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0009352C File Offset: 0x0009172C
		public override void _Ready()
		{
			this._mask1 = base.GetNode<Sprite2D>("Mask1");
			this._reflection1 = base.GetNode<Control>("Mask1/Reflection");
			this._mask2 = base.GetNode<Sprite2D>("Mask2");
			this._reflection2 = base.GetNode<Control>("Mask2/Reflection");
			this._mask3 = base.GetNode<Sprite2D>("Mask3");
			this._reflection3 = base.GetNode<Control>("Mask3/Reflection");
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000935C0 File Offset: 0x000917C0
		public override void _Process(double delta)
		{
			this._totalTime += (float)delta * 2f;
			this._noise.Seed = 0;
			float num = 1.05f + this._noise.GetNoise1D(this._totalTime);
			this._mask1.Scale = new Vector2(num, num);
			this._reflection1.Scale = this._mask1.Scale;
			this._noise.Seed = 1;
			num = this._noise.GetNoise1D(this._totalTime);
			this._mask1.RotationDegrees = Mathf.Abs(num) * 10f;
			this._noise.Seed = 2;
			num = 1.05f + this._noise.GetNoise1D(this._totalTime);
			this._mask2.Scale = new Vector2(num, num);
			this._reflection2.Scale = new Vector2(num, num);
			this._noise.Seed = 3;
			num = this._noise.GetNoise1D(this._totalTime);
			this._mask2.RotationDegrees = Mathf.Abs(num) * 20f;
			this._noise.Seed = 4;
			num = 1.05f + this._noise.GetNoise1D(this._totalTime);
			this._mask3.Scale = new Vector2(num, num);
			this._reflection3.Scale = new Vector2(num, num);
			this._noise.Seed = 5;
			num = this._noise.GetNoise1D(this._totalTime);
			this._mask3.RotationDegrees = Mathf.Abs(num) * 30f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017A7 RID: 6055 RVA: 0x00093760 File Offset: 0x00091960
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMirrorVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NMirrorVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMirrorVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017A8 RID: 6056 RVA: 0x00093834 File Offset: 0x00091A34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMirrorVfx.MethodName.Create && args.Count == 0)
			{
				NMirrorVfx nmirrorVfx = NMirrorVfx.Create();
				ret = VariantUtils.CreateFrom<NMirrorVfx>(ref nmirrorVfx);
				return true;
			}
			if ((ref method) == NMirrorVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMirrorVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000938D0 File Offset: 0x00091AD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMirrorVfx.MethodName.Create && args.Count == 0)
			{
				NMirrorVfx nmirrorVfx = NMirrorVfx.Create();
				ret = VariantUtils.CreateFrom<NMirrorVfx>(ref nmirrorVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060017AA RID: 6058 RVA: 0x00093910 File Offset: 0x00091B10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMirrorVfx.MethodName.Create || (ref method) == NMirrorVfx.MethodName._Ready || (ref method) == NMirrorVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017AB RID: 6059 RVA: 0x00093948 File Offset: 0x00091B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMirrorVfx.PropertyName._mask1)
			{
				this._mask1 = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection1)
			{
				this._reflection1 = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._mask2)
			{
				this._mask2 = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection2)
			{
				this._reflection2 = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._mask3)
			{
				this._mask3 = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection3)
			{
				this._reflection3 = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._noise)
			{
				this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._totalTime)
			{
				this._totalTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017AC RID: 6060 RVA: 0x00093A38 File Offset: 0x00091C38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMirrorVfx.PropertyName._mask1)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask1);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection1)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._reflection1);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._mask2)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask2);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection2)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._reflection2);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._mask3)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._mask3);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._reflection3)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._reflection3);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._noise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
				return true;
			}
			if ((ref name) == NMirrorVfx.PropertyName._totalTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._totalTime);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017AD RID: 6061 RVA: 0x00093B50 File Offset: 0x00091D50
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMirrorVfx.PropertyName._mask1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._reflection1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._mask2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._reflection2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._mask3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._reflection3, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMirrorVfx.PropertyName._noise, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMirrorVfx.PropertyName._totalTime, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017AE RID: 6062 RVA: 0x00093C6C File Offset: 0x00091E6C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMirrorVfx.PropertyName._mask1, Variant.From<Sprite2D>(ref this._mask1));
			info.AddProperty(NMirrorVfx.PropertyName._reflection1, Variant.From<Control>(ref this._reflection1));
			info.AddProperty(NMirrorVfx.PropertyName._mask2, Variant.From<Sprite2D>(ref this._mask2));
			info.AddProperty(NMirrorVfx.PropertyName._reflection2, Variant.From<Control>(ref this._reflection2));
			info.AddProperty(NMirrorVfx.PropertyName._mask3, Variant.From<Sprite2D>(ref this._mask3));
			info.AddProperty(NMirrorVfx.PropertyName._reflection3, Variant.From<Control>(ref this._reflection3));
			info.AddProperty(NMirrorVfx.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
			info.AddProperty(NMirrorVfx.PropertyName._totalTime, Variant.From<float>(ref this._totalTime));
		}

		/// <inheritdoc />
		// Token: 0x060017AF RID: 6063 RVA: 0x00093D30 File Offset: 0x00091F30
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._mask1, ref variant))
			{
				this._mask1 = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection1, ref variant2))
			{
				this._reflection1 = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._mask2, ref variant3))
			{
				this._mask2 = variant3.As<Sprite2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection2, ref variant4))
			{
				this._reflection2 = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._mask3, ref variant5))
			{
				this._mask3 = variant5.As<Sprite2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._reflection3, ref variant6))
			{
				this._reflection3 = variant6.As<Control>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._noise, ref variant7))
			{
				this._noise = variant7.As<FastNoiseLite>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMirrorVfx.PropertyName._totalTime, ref variant8))
			{
				this._totalTime = variant8.As<float>();
			}
		}

		// Token: 0x04000881 RID: 2177
		private Sprite2D _mask1;

		// Token: 0x04000882 RID: 2178
		private Control _reflection1;

		// Token: 0x04000883 RID: 2179
		private Sprite2D _mask2;

		// Token: 0x04000884 RID: 2180
		private Control _reflection2;

		// Token: 0x04000885 RID: 2181
		private Sprite2D _mask3;

		// Token: 0x04000886 RID: 2182
		private Control _reflection3;

		// Token: 0x04000887 RID: 2183
		private FastNoiseLite _noise = new FastNoiseLite();

		// Token: 0x04000888 RID: 2184
		private float _totalTime;

		// Token: 0x04000889 RID: 2185
		private const float _noiseSpeed = 2f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C6 RID: 4294
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040041DF RID: 16863
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041E0 RID: 16864
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040041E1 RID: 16865
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C7 RID: 4295
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_mask1' field.
			/// </summary>
			// Token: 0x040041E2 RID: 16866
			public static readonly StringName _mask1 = "_mask1";

			/// <summary>
			/// Cached name for the '_reflection1' field.
			/// </summary>
			// Token: 0x040041E3 RID: 16867
			public static readonly StringName _reflection1 = "_reflection1";

			/// <summary>
			/// Cached name for the '_mask2' field.
			/// </summary>
			// Token: 0x040041E4 RID: 16868
			public static readonly StringName _mask2 = "_mask2";

			/// <summary>
			/// Cached name for the '_reflection2' field.
			/// </summary>
			// Token: 0x040041E5 RID: 16869
			public static readonly StringName _reflection2 = "_reflection2";

			/// <summary>
			/// Cached name for the '_mask3' field.
			/// </summary>
			// Token: 0x040041E6 RID: 16870
			public static readonly StringName _mask3 = "_mask3";

			/// <summary>
			/// Cached name for the '_reflection3' field.
			/// </summary>
			// Token: 0x040041E7 RID: 16871
			public static readonly StringName _reflection3 = "_reflection3";

			/// <summary>
			/// Cached name for the '_noise' field.
			/// </summary>
			// Token: 0x040041E8 RID: 16872
			public static readonly StringName _noise = "_noise";

			/// <summary>
			/// Cached name for the '_totalTime' field.
			/// </summary>
			// Token: 0x040041E9 RID: 16873
			public static readonly StringName _totalTime = "_totalTime";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010C8 RID: 4296
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}
