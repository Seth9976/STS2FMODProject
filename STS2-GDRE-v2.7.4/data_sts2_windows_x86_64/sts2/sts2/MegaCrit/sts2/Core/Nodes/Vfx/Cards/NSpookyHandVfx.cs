using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards
{
	// Token: 0x0200023D RID: 573
	[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NSpookyHandVfx.cs")]
	public class NSpookyHandVfx : Control
	{
		// Token: 0x06001D29 RID: 7465 RVA: 0x000BDB14 File Offset: 0x000BBD14
		public override void _Ready()
		{
			this._totalPauses = Rng.Chaotic.NextInt(2, 7);
			this._canPauseTimer = Rng.Chaotic.NextFloat(0.5f, 1.2f);
			this._speed = Rng.Chaotic.NextFloat(3f, 5f);
			this._intensity = Rng.Chaotic.NextFloat(0.1f, 0.3f);
			this._originalRotation = base.Rotation;
			this._targetScale = base.Scale;
			this.AnimateIn();
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000BDBA0 File Offset: 0x000BBDA0
		private void AnimateIn()
		{
			base.Scale = Vector2.Zero;
			Tween tween = base.CreateTween().SetParallel(true);
			base.Modulate = new Color(base.Modulate.R + Rng.Chaotic.NextFloat(-0.2f, 0.2f), base.Modulate.G, base.Modulate.B, 1f);
			tween.TweenInterval(Rng.Chaotic.NextDouble(0.0, 0.4));
			tween.Chain();
			tween.TweenProperty(this, "scale", this._targetScale, Rng.Chaotic.NextDouble(0.4, 0.5)).SetEase(1L).SetTrans(11L);
			tween.Chain();
			tween.TweenInterval(Rng.Chaotic.NextDouble(0.3, 0.6));
			tween.Chain();
			double num = Rng.Chaotic.NextDouble(0.4, 0.6);
			tween.TweenProperty(this, "scale", this._targetScale * 0.5f, num).SetEase(0L).SetTrans(10L);
			tween.TweenProperty(this, "modulate:a", 0f, num).SetEase(1L).SetTrans(4L);
		}

		/// <summary>
		/// Processes the jittering and rotation logic for this vfx
		/// </summary>
		/// <param name="delta"></param>
		// Token: 0x06001D2B RID: 7467 RVA: 0x000BDD30 File Offset: 0x000BBF30
		public override void _Process(double delta)
		{
			float num = (float)delta;
			this._duration += num * this._speed;
			this._canPauseTimer -= num;
			if (this._isPaused)
			{
				this._elapsedPauseTime += num;
				if (this._elapsedPauseTime >= 0.05f)
				{
					this._isPaused = false;
					this._elapsedPauseTime = 0f;
					this._pauseCounter++;
				}
				base.Rotation = this._originalRotation + Mathf.Sin(this._duration) * this._intensity * 0.5f;
			}
			else
			{
				base.Rotation = this._originalRotation + Mathf.Sin(this._duration) * this._intensity;
			}
			this._timer += num;
			if (this._canPauseTimer < 0f && this._pauseCounter < this._totalPauses)
			{
				this._isPaused = true;
				this._timer = 0f;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D2C RID: 7468 RVA: 0x000BDE28 File Offset: 0x000BC028
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSpookyHandVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpookyHandVfx.MethodName.AnimateIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpookyHandVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D2D RID: 7469 RVA: 0x000BDEF0 File Offset: 0x000BC0F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpookyHandVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpookyHandVfx.MethodName.AnimateIn && args.Count == 0)
			{
				this.AnimateIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpookyHandVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D2E RID: 7470 RVA: 0x000BDF83 File Offset: 0x000BC183
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpookyHandVfx.MethodName._Ready || (ref method) == NSpookyHandVfx.MethodName.AnimateIn || (ref method) == NSpookyHandVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D2F RID: 7471 RVA: 0x000BDFBC File Offset: 0x000BC1BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpookyHandVfx.PropertyName._elapsedPauseTime)
			{
				this._elapsedPauseTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._pauseCounter)
			{
				this._pauseCounter = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._isPaused)
			{
				this._isPaused = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._timer)
			{
				this._timer = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._totalPauses)
			{
				this._totalPauses = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._canPauseTimer)
			{
				this._canPauseTimer = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._intensity)
			{
				this._intensity = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._speed)
			{
				this._speed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._duration)
			{
				this._duration = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._originalRotation)
			{
				this._originalRotation = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._targetScale)
			{
				this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001D30 RID: 7472 RVA: 0x000BE0FC File Offset: 0x000BC2FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpookyHandVfx.PropertyName._elapsedPauseTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._elapsedPauseTime);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._pauseCounter)
			{
				value = VariantUtils.CreateFrom<int>(ref this._pauseCounter);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._isPaused)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPaused);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._timer)
			{
				value = VariantUtils.CreateFrom<float>(ref this._timer);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._totalPauses)
			{
				value = VariantUtils.CreateFrom<int>(ref this._totalPauses);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._canPauseTimer)
			{
				value = VariantUtils.CreateFrom<float>(ref this._canPauseTimer);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._intensity)
			{
				value = VariantUtils.CreateFrom<float>(ref this._intensity);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._speed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._speed);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._duration)
			{
				value = VariantUtils.CreateFrom<float>(ref this._duration);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._originalRotation)
			{
				value = VariantUtils.CreateFrom<float>(ref this._originalRotation);
				return true;
			}
			if ((ref name) == NSpookyHandVfx.PropertyName._targetScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D31 RID: 7473 RVA: 0x000BE274 File Offset: 0x000BC474
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._elapsedPauseTime, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpookyHandVfx.PropertyName._pauseCounter, 0L, "", 4096L, false),
				new PropertyInfo(1L, NSpookyHandVfx.PropertyName._isPaused, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._timer, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpookyHandVfx.PropertyName._totalPauses, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._canPauseTimer, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._intensity, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._speed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._duration, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpookyHandVfx.PropertyName._originalRotation, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSpookyHandVfx.PropertyName._targetScale, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D32 RID: 7474 RVA: 0x000BE3E8 File Offset: 0x000BC5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSpookyHandVfx.PropertyName._elapsedPauseTime, Variant.From<float>(ref this._elapsedPauseTime));
			info.AddProperty(NSpookyHandVfx.PropertyName._pauseCounter, Variant.From<int>(ref this._pauseCounter));
			info.AddProperty(NSpookyHandVfx.PropertyName._isPaused, Variant.From<bool>(ref this._isPaused));
			info.AddProperty(NSpookyHandVfx.PropertyName._timer, Variant.From<float>(ref this._timer));
			info.AddProperty(NSpookyHandVfx.PropertyName._totalPauses, Variant.From<int>(ref this._totalPauses));
			info.AddProperty(NSpookyHandVfx.PropertyName._canPauseTimer, Variant.From<float>(ref this._canPauseTimer));
			info.AddProperty(NSpookyHandVfx.PropertyName._intensity, Variant.From<float>(ref this._intensity));
			info.AddProperty(NSpookyHandVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
			info.AddProperty(NSpookyHandVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
			info.AddProperty(NSpookyHandVfx.PropertyName._originalRotation, Variant.From<float>(ref this._originalRotation));
			info.AddProperty(NSpookyHandVfx.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
		}

		/// <inheritdoc />
		// Token: 0x06001D33 RID: 7475 RVA: 0x000BE4F0 File Offset: 0x000BC6F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._elapsedPauseTime, ref variant))
			{
				this._elapsedPauseTime = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._pauseCounter, ref variant2))
			{
				this._pauseCounter = variant2.As<int>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._isPaused, ref variant3))
			{
				this._isPaused = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._timer, ref variant4))
			{
				this._timer = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._totalPauses, ref variant5))
			{
				this._totalPauses = variant5.As<int>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._canPauseTimer, ref variant6))
			{
				this._canPauseTimer = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._intensity, ref variant7))
			{
				this._intensity = variant7.As<float>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._speed, ref variant8))
			{
				this._speed = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._duration, ref variant9))
			{
				this._duration = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._originalRotation, ref variant10))
			{
				this._originalRotation = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NSpookyHandVfx.PropertyName._targetScale, ref variant11))
			{
				this._targetScale = variant11.As<Vector2>();
			}
		}

		// Token: 0x04000B3A RID: 2874
		private const float _pauseDuration = 0.05f;

		// Token: 0x04000B3B RID: 2875
		private float _elapsedPauseTime;

		// Token: 0x04000B3C RID: 2876
		private int _pauseCounter;

		// Token: 0x04000B3D RID: 2877
		private bool _isPaused;

		// Token: 0x04000B3E RID: 2878
		private float _timer;

		// Token: 0x04000B3F RID: 2879
		private int _totalPauses;

		// Token: 0x04000B40 RID: 2880
		private float _canPauseTimer;

		// Token: 0x04000B41 RID: 2881
		private float _intensity;

		// Token: 0x04000B42 RID: 2882
		private float _speed;

		// Token: 0x04000B43 RID: 2883
		private float _duration;

		// Token: 0x04000B44 RID: 2884
		private float _originalRotation;

		// Token: 0x04000B45 RID: 2885
		private Vector2 _targetScale;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001224 RID: 4644
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400463E RID: 17982
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimateIn' method.
			/// </summary>
			// Token: 0x0400463F RID: 17983
			public static readonly StringName AnimateIn = "AnimateIn";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04004640 RID: 17984
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001225 RID: 4645
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_elapsedPauseTime' field.
			/// </summary>
			// Token: 0x04004641 RID: 17985
			public static readonly StringName _elapsedPauseTime = "_elapsedPauseTime";

			/// <summary>
			/// Cached name for the '_pauseCounter' field.
			/// </summary>
			// Token: 0x04004642 RID: 17986
			public static readonly StringName _pauseCounter = "_pauseCounter";

			/// <summary>
			/// Cached name for the '_isPaused' field.
			/// </summary>
			// Token: 0x04004643 RID: 17987
			public static readonly StringName _isPaused = "_isPaused";

			/// <summary>
			/// Cached name for the '_timer' field.
			/// </summary>
			// Token: 0x04004644 RID: 17988
			public static readonly StringName _timer = "_timer";

			/// <summary>
			/// Cached name for the '_totalPauses' field.
			/// </summary>
			// Token: 0x04004645 RID: 17989
			public static readonly StringName _totalPauses = "_totalPauses";

			/// <summary>
			/// Cached name for the '_canPauseTimer' field.
			/// </summary>
			// Token: 0x04004646 RID: 17990
			public static readonly StringName _canPauseTimer = "_canPauseTimer";

			/// <summary>
			/// Cached name for the '_intensity' field.
			/// </summary>
			// Token: 0x04004647 RID: 17991
			public static readonly StringName _intensity = "_intensity";

			/// <summary>
			/// Cached name for the '_speed' field.
			/// </summary>
			// Token: 0x04004648 RID: 17992
			public static readonly StringName _speed = "_speed";

			/// <summary>
			/// Cached name for the '_duration' field.
			/// </summary>
			// Token: 0x04004649 RID: 17993
			public static readonly StringName _duration = "_duration";

			/// <summary>
			/// Cached name for the '_originalRotation' field.
			/// </summary>
			// Token: 0x0400464A RID: 17994
			public static readonly StringName _originalRotation = "_originalRotation";

			/// <summary>
			/// Cached name for the '_targetScale' field.
			/// </summary>
			// Token: 0x0400464B RID: 17995
			public static readonly StringName _targetScale = "_targetScale";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001226 RID: 4646
		public class SignalName : Control.SignalName
		{
		}
	}
}
