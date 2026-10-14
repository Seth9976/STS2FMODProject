using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui
{
	// Token: 0x02000226 RID: 550
	[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NEpochChains.cs")]
	public class NEpochChains : TextureRect
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x000B4C2C File Offset: 0x000B2E2C
		private void UpdateParticles(int index)
		{
			if (this._previousParticleIndex == index)
			{
				return;
			}
			this._previousParticleIndex = index;
			for (int i = 0; i < this._particles.Count; i++)
			{
				if (i == index)
				{
					this._particles[i].Restart();
				}
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000B4C78 File Offset: 0x000B2E78
		private void SetProperties(float interpolation)
		{
			if (this._asShaderMaterial == null)
			{
				return;
			}
			float num = this._brightEnabledCurve.Sample(interpolation);
			float num2 = this._erosionEnabledCurve.Sample(interpolation);
			float num3 = this._erosionBaseCurve.Sample(interpolation);
			this._asShaderMaterial.SetShaderParameter(NEpochChains._brightEnabledString, num);
			this._asShaderMaterial.SetShaderParameter(NEpochChains._erosionEnabledString, num2);
			this._asShaderMaterial.SetShaderParameter(NEpochChains._erosionBaseString, num3);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000B4CF7 File Offset: 0x000B2EF7
		public void Unlock()
		{
			TaskHelper.RunSafely(this.Unlocking());
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000B4D08 File Offset: 0x000B2F08
		[NullableContext(1)]
		public async Task Unlocking()
		{
			this._previousParticleIndex = -1;
			base.SelfModulate = Colors.White;
			double timer = 0.0;
			Material originalMaterial = base.Material;
			this._asShaderMaterial = (ShaderMaterial)originalMaterial.Duplicate(true);
			base.Material = this._asShaderMaterial;
			this.SetProperties(0f);
			while (timer < (double)this._duration)
			{
				float num = (float)timer / this._duration;
				float num2 = this._particlesCurve.Sample(num);
				this.SetProperties(num);
				this.UpdateParticles(Mathf.FloorToInt(num2));
				timer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			this.SetProperties(1f);
			base.Material = originalMaterial;
			this._asShaderMaterial.Dispose();
			base.SelfModulate = new Color(1f, 1f, 1f, 0f);
			this._endParticles.Restart();
			base.EmitSignal(NEpochChains.SignalName.OnAnimationFinished, Array.Empty<Variant>());
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BDC RID: 7132 RVA: 0x000B4D4C File Offset: 0x000B2F4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NEpochChains.MethodName.UpdateParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochChains.MethodName.SetProperties, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "interpolation", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochChains.MethodName.Unlock, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BDD RID: 7133 RVA: 0x000B4E38 File Offset: 0x000B3038
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochChains.MethodName.UpdateParticles && args.Count == 1)
			{
				this.UpdateParticles(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochChains.MethodName.SetProperties && args.Count == 1)
			{
				this.SetProperties(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochChains.MethodName.Unlock && args.Count == 0)
			{
				this.Unlock();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001BDE RID: 7134 RVA: 0x000B4ED9 File Offset: 0x000B30D9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochChains.MethodName.UpdateParticles || (ref method) == NEpochChains.MethodName.SetProperties || (ref method) == NEpochChains.MethodName.Unlock || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001BDF RID: 7135 RVA: 0x000B4F10 File Offset: 0x000B3110
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochChains.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._endParticles)
			{
				this._endParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._particlesCurve)
			{
				this._particlesCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._brightEnabledCurve)
			{
				this._brightEnabledCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._erosionEnabledCurve)
			{
				this._erosionEnabledCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._erosionBaseCurve)
			{
				this._erosionBaseCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._previousParticleIndex)
			{
				this._previousParticleIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._asShaderMaterial)
			{
				this._asShaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001BE0 RID: 7136 RVA: 0x000B5018 File Offset: 0x000B3218
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochChains.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<NParticlesContainer>(this._particles);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._endParticles)
			{
				value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._endParticles);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._particlesCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._particlesCurve);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._brightEnabledCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._brightEnabledCurve);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._erosionEnabledCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._erosionEnabledCurve);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._erosionBaseCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._erosionBaseCurve);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._previousParticleIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._previousParticleIndex);
				return true;
			}
			if ((ref name) == NEpochChains.PropertyName._asShaderMaterial)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._asShaderMaterial);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BE1 RID: 7137 RVA: 0x000B5150 File Offset: 0x000B3350
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NEpochChains.PropertyName._duration, 0L, "", 4102L, true),
				new PropertyInfo(28L, NEpochChains.PropertyName._particles, 23L, "24/34:Node2D", 4102L, true),
				new PropertyInfo(24L, NEpochChains.PropertyName._endParticles, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NEpochChains.PropertyName._particlesCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NEpochChains.PropertyName._brightEnabledCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NEpochChains.PropertyName._erosionEnabledCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NEpochChains.PropertyName._erosionBaseCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(2L, NEpochChains.PropertyName._previousParticleIndex, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochChains.PropertyName._asShaderMaterial, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001BE2 RID: 7138 RVA: 0x000B5294 File Offset: 0x000B3494
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochChains.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NEpochChains.PropertyName._particles, Variant.CreateFrom<NParticlesContainer>(this._particles));
			info.AddProperty(NEpochChains.PropertyName._endParticles, Variant.From<NParticlesContainer>(ref this._endParticles));
			info.AddProperty(NEpochChains.PropertyName._particlesCurve, Variant.From<Curve>(ref this._particlesCurve));
			info.AddProperty(NEpochChains.PropertyName._brightEnabledCurve, Variant.From<Curve>(ref this._brightEnabledCurve));
			info.AddProperty(NEpochChains.PropertyName._erosionEnabledCurve, Variant.From<Curve>(ref this._erosionEnabledCurve));
			info.AddProperty(NEpochChains.PropertyName._erosionBaseCurve, Variant.From<Curve>(ref this._erosionBaseCurve));
			info.AddProperty(NEpochChains.PropertyName._previousParticleIndex, Variant.From<int>(ref this._previousParticleIndex));
			info.AddProperty(NEpochChains.PropertyName._asShaderMaterial, Variant.From<ShaderMaterial>(ref this._asShaderMaterial));
			info.AddSignalEventDelegate(NEpochChains.SignalName.OnAnimationFinished, this.backing_OnAnimationFinished);
		}

		/// <inheritdoc />
		// Token: 0x06001BE3 RID: 7139 RVA: 0x000B5380 File Offset: 0x000B3580
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochChains.PropertyName._duration, ref variant))
			{
				this._duration = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochChains.PropertyName._particles, ref variant2))
			{
				this._particles = variant2.AsGodotArray<NParticlesContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochChains.PropertyName._endParticles, ref variant3))
			{
				this._endParticles = variant3.As<NParticlesContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEpochChains.PropertyName._particlesCurve, ref variant4))
			{
				this._particlesCurve = variant4.As<Curve>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEpochChains.PropertyName._brightEnabledCurve, ref variant5))
			{
				this._brightEnabledCurve = variant5.As<Curve>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEpochChains.PropertyName._erosionEnabledCurve, ref variant6))
			{
				this._erosionEnabledCurve = variant6.As<Curve>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEpochChains.PropertyName._erosionBaseCurve, ref variant7))
			{
				this._erosionBaseCurve = variant7.As<Curve>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEpochChains.PropertyName._previousParticleIndex, ref variant8))
			{
				this._previousParticleIndex = variant8.As<int>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEpochChains.PropertyName._asShaderMaterial, ref variant9))
			{
				this._asShaderMaterial = variant9.As<ShaderMaterial>();
			}
			NEpochChains.OnAnimationFinishedEventHandler onAnimationFinishedEventHandler;
			if (info.TryGetSignalEventDelegate<NEpochChains.OnAnimationFinishedEventHandler>(NEpochChains.SignalName.OnAnimationFinished, ref onAnimationFinishedEventHandler))
			{
				this.backing_OnAnimationFinished = onAnimationFinishedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001BE4 RID: 7140 RVA: 0x000B54A8 File Offset: 0x000B36A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NEpochChains.SignalName.OnAnimationFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Vfx.Ui.NEpochChains.OnAnimationFinishedEventHandler" />
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001BE5 RID: 7141 RVA: 0x000B54EC File Offset: 0x000B36EC
		// (remove) Token: 0x06001BE6 RID: 7142 RVA: 0x000B5505 File Offset: 0x000B3705
		public event NEpochChains.OnAnimationFinishedEventHandler OnAnimationFinished
		{
			add
			{
				this.backing_OnAnimationFinished = (NEpochChains.OnAnimationFinishedEventHandler)Delegate.Combine(this.backing_OnAnimationFinished, value);
			}
			remove
			{
				this.backing_OnAnimationFinished = (NEpochChains.OnAnimationFinishedEventHandler)Delegate.Remove(this.backing_OnAnimationFinished, value);
			}
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000B551E File Offset: 0x000B371E
		protected void EmitSignalOnAnimationFinished()
		{
			base.EmitSignal(NEpochChains.SignalName.OnAnimationFinished, Array.Empty<Variant>());
		}

		/// <inheritdoc />
		// Token: 0x06001BE8 RID: 7144 RVA: 0x000B5531 File Offset: 0x000B3731
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NEpochChains.SignalName.OnAnimationFinished) || args.Count != 0)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NEpochChains.OnAnimationFinishedEventHandler onAnimationFinishedEventHandler = this.backing_OnAnimationFinished;
			if (onAnimationFinishedEventHandler == null)
			{
				return;
			}
			onAnimationFinishedEventHandler();
		}

		/// <inheritdoc />
		// Token: 0x06001BE9 RID: 7145 RVA: 0x000B5562 File Offset: 0x000B3762
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NEpochChains.SignalName.OnAnimationFinished || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x04000A9D RID: 2717
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _duration = 0.5f;

		// Token: 0x04000A9E RID: 2718
		[Nullable(new byte[] { 2, 1 })]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<NParticlesContainer> _particles;

		// Token: 0x04000A9F RID: 2719
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private NParticlesContainer _endParticles;

		// Token: 0x04000AA0 RID: 2720
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _particlesCurve;

		// Token: 0x04000AA1 RID: 2721
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _brightEnabledCurve;

		// Token: 0x04000AA2 RID: 2722
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _erosionEnabledCurve;

		// Token: 0x04000AA3 RID: 2723
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _erosionBaseCurve;

		// Token: 0x04000AA4 RID: 2724
		[Nullable(1)]
		private static readonly StringName _brightEnabledString = new StringName("bright_enabled");

		// Token: 0x04000AA5 RID: 2725
		[Nullable(1)]
		private static readonly StringName _erosionEnabledString = new StringName("erosion_enabled");

		// Token: 0x04000AA6 RID: 2726
		[Nullable(1)]
		private static readonly StringName _erosionBaseString = new StringName("erosion_base");

		// Token: 0x04000AA7 RID: 2727
		private int _previousParticleIndex = -1;

		// Token: 0x04000AA8 RID: 2728
		[Nullable(2)]
		private ShaderMaterial _asShaderMaterial;

		// Token: 0x04000AA9 RID: 2729
		private NEpochChains.OnAnimationFinishedEventHandler backing_OnAnimationFinished;

		// Token: 0x020011C6 RID: 4550
		// (Invoke) Token: 0x06009BFF RID: 39935
		[Signal]
		public delegate void OnAnimationFinishedEventHandler();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C7 RID: 4551
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'UpdateParticles' method.
			/// </summary>
			// Token: 0x04004521 RID: 17697
			public static readonly StringName UpdateParticles = "UpdateParticles";

			/// <summary>
			/// Cached name for the 'SetProperties' method.
			/// </summary>
			// Token: 0x04004522 RID: 17698
			public static readonly StringName SetProperties = "SetProperties";

			/// <summary>
			/// Cached name for the 'Unlock' method.
			/// </summary>
			// Token: 0x04004523 RID: 17699
			public static readonly StringName Unlock = "Unlock";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C8 RID: 4552
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004524 RID: 17700
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x04004525 RID: 17701
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_endParticles' field.
			/// </summary>
			// Token: 0x04004526 RID: 17702
			public static readonly StringName _endParticles = "_endParticles";

			/// <summary>
			/// Cached name for the '_particlesCurve' field.
			/// </summary>
			// Token: 0x04004527 RID: 17703
			public static readonly StringName _particlesCurve = "_particlesCurve";

			/// <summary>
			/// Cached name for the '_brightEnabledCurve' field.
			/// </summary>
			// Token: 0x04004528 RID: 17704
			public static readonly StringName _brightEnabledCurve = "_brightEnabledCurve";

			/// <summary>
			/// Cached name for the '_erosionEnabledCurve' field.
			/// </summary>
			// Token: 0x04004529 RID: 17705
			public static readonly StringName _erosionEnabledCurve = "_erosionEnabledCurve";

			/// <summary>
			/// Cached name for the '_erosionBaseCurve' field.
			/// </summary>
			// Token: 0x0400452A RID: 17706
			public static readonly StringName _erosionBaseCurve = "_erosionBaseCurve";

			/// <summary>
			/// Cached name for the '_previousParticleIndex' field.
			/// </summary>
			// Token: 0x0400452B RID: 17707
			public static readonly StringName _previousParticleIndex = "_previousParticleIndex";

			/// <summary>
			/// Cached name for the '_asShaderMaterial' field.
			/// </summary>
			// Token: 0x0400452C RID: 17708
			public static readonly StringName _asShaderMaterial = "_asShaderMaterial";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011C9 RID: 4553
		public class SignalName : TextureRect.SignalName
		{
			/// <summary>
			/// Cached name for the 'OnAnimationFinished' signal.
			/// </summary>
			// Token: 0x0400452D RID: 17709
			public static readonly StringName OnAnimationFinished = "OnAnimationFinished";
		}
	}
}
