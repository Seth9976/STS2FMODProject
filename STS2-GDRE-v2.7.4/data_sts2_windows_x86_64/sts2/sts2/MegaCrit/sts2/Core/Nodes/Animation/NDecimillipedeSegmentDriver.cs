using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Animation
{
	// Token: 0x02000424 RID: 1060
	[ScriptPath("res://src/Core/Nodes/Animation/NDecimillipedeSegmentDriver.cs")]
	public class NDecimillipedeSegmentDriver : Node2D
	{
		// Token: 0x060043C0 RID: 17344 RVA: 0x0020E980 File Offset: 0x0020CB80
		public override void _Ready()
		{
			this._originPos = base.Position;
			this._speed = (this._leftSegment ? 0.1f : 0.05f);
			this._magnitude = (this._leftSegment ? 250f : 300f);
			this._noise.NoiseType = 3L;
			this._noise.Frequency = 1f;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0020E9EC File Offset: 0x0020CBEC
		public override void _Process(double delta)
		{
			this._time += (float)delta;
			float num = this._time * this._speed + (this._leftSegment ? 0.25f : 0f);
			Vector2 vector = new Vector2(this._noise.GetNoise1D(num), this._noise.GetNoise1D(num + 0.25f)) * this._magnitude;
			base.Position = this._originPos + vector + this._decimillipedeStrikeOffset;
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0020EA78 File Offset: 0x0020CC78
		public void AttackShake()
		{
			Tween attackTween = this._attackTween;
			if (attackTween != null)
			{
				attackTween.Kill();
			}
			this._attackTween = base.CreateTween();
			this._attackTween.TweenProperty(this, "_decimillipedeStrikeOffset", Vector2.Left * 100f, 0.4).SetEase(2L).SetTrans(1L);
			this._attackTween.TweenProperty(this, "_decimillipedeStrikeOffset", Vector2.Right * 100f, 0.10000000149011612).SetEase(2L).SetTrans(1L);
			this._attackTween.TweenProperty(this, "_decimillipedeStrikeOffset", Vector2.Zero, 0.75).SetEase(2L).SetTrans(1L);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043C3 RID: 17347 RVA: 0x0020EB60 File Offset: 0x0020CD60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NDecimillipedeSegmentDriver.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeSegmentDriver.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDecimillipedeSegmentDriver.MethodName.AttackShake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043C4 RID: 17348 RVA: 0x0020EC28 File Offset: 0x0020CE28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDecimillipedeSegmentDriver.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentDriver.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeSegmentDriver.MethodName.AttackShake && args.Count == 0)
			{
				this.AttackShake();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060043C5 RID: 17349 RVA: 0x0020ECBB File Offset: 0x0020CEBB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDecimillipedeSegmentDriver.MethodName._Ready || (ref method) == NDecimillipedeSegmentDriver.MethodName._Process || (ref method) == NDecimillipedeSegmentDriver.MethodName.AttackShake || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060043C6 RID: 17350 RVA: 0x0020ECF4 File Offset: 0x0020CEF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._leftSegment)
			{
				this._leftSegment = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._speed)
			{
				this._speed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._magnitude)
			{
				this._magnitude = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._originPos)
			{
				this._originPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._noise)
			{
				this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset)
			{
				this._decimillipedeStrikeOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._attackTween)
			{
				this._attackTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060043C7 RID: 17351 RVA: 0x0020EDE4 File Offset: 0x0020CFE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._leftSegment)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._leftSegment);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._speed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._speed);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._magnitude)
			{
				value = VariantUtils.CreateFrom<float>(ref this._magnitude);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._originPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originPos);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._noise)
			{
				value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<float>(ref this._time);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._decimillipedeStrikeOffset);
				return true;
			}
			if ((ref name) == NDecimillipedeSegmentDriver.PropertyName._attackTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._attackTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060043C8 RID: 17352 RVA: 0x0020EEFC File Offset: 0x0020D0FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NDecimillipedeSegmentDriver.PropertyName._leftSegment, 0L, "", 4102L, true),
				new PropertyInfo(3L, NDecimillipedeSegmentDriver.PropertyName._speed, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDecimillipedeSegmentDriver.PropertyName._magnitude, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDecimillipedeSegmentDriver.PropertyName._originPos, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDecimillipedeSegmentDriver.PropertyName._noise, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDecimillipedeSegmentDriver.PropertyName._time, 0L, "", 4096L, false),
				new PropertyInfo(5L, NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDecimillipedeSegmentDriver.PropertyName._attackTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060043C9 RID: 17353 RVA: 0x0020F014 File Offset: 0x0020D214
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._leftSegment, Variant.From<bool>(ref this._leftSegment));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._speed, Variant.From<float>(ref this._speed));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._magnitude, Variant.From<float>(ref this._magnitude));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._originPos, Variant.From<Vector2>(ref this._originPos));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._time, Variant.From<float>(ref this._time));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, Variant.From<Vector2>(ref this._decimillipedeStrikeOffset));
			info.AddProperty(NDecimillipedeSegmentDriver.PropertyName._attackTween, Variant.From<Tween>(ref this._attackTween));
		}

		/// <inheritdoc />
		// Token: 0x060043CA RID: 17354 RVA: 0x0020F0D8 File Offset: 0x0020D2D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._leftSegment, ref variant))
			{
				this._leftSegment = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._speed, ref variant2))
			{
				this._speed = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._magnitude, ref variant3))
			{
				this._magnitude = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._originPos, ref variant4))
			{
				this._originPos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._noise, ref variant5))
			{
				this._noise = variant5.As<FastNoiseLite>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._time, ref variant6))
			{
				this._time = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._decimillipedeStrikeOffset, ref variant7))
			{
				this._decimillipedeStrikeOffset = variant7.As<Vector2>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDecimillipedeSegmentDriver.PropertyName._attackTween, ref variant8))
			{
				this._attackTween = variant8.As<Tween>();
			}
		}

		// Token: 0x04001A54 RID: 6740
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private bool _leftSegment;

		// Token: 0x04001A55 RID: 6741
		private float _speed;

		// Token: 0x04001A56 RID: 6742
		private float _magnitude;

		// Token: 0x04001A57 RID: 6743
		private Vector2 _originPos;

		// Token: 0x04001A58 RID: 6744
		[Nullable(1)]
		private FastNoiseLite _noise = new FastNoiseLite();

		// Token: 0x04001A59 RID: 6745
		private float _time;

		// Token: 0x04001A5A RID: 6746
		private Vector2 _decimillipedeStrikeOffset = Vector2.Zero;

		// Token: 0x04001A5B RID: 6747
		[Nullable(2)]
		private Tween _attackTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001982 RID: 6530
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040064D7 RID: 25815
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040064D8 RID: 25816
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'AttackShake' method.
			/// </summary>
			// Token: 0x040064D9 RID: 25817
			public static readonly StringName AttackShake = "AttackShake";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001983 RID: 6531
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_leftSegment' field.
			/// </summary>
			// Token: 0x040064DA RID: 25818
			public static readonly StringName _leftSegment = "_leftSegment";

			/// <summary>
			/// Cached name for the '_speed' field.
			/// </summary>
			// Token: 0x040064DB RID: 25819
			public static readonly StringName _speed = "_speed";

			/// <summary>
			/// Cached name for the '_magnitude' field.
			/// </summary>
			// Token: 0x040064DC RID: 25820
			public static readonly StringName _magnitude = "_magnitude";

			/// <summary>
			/// Cached name for the '_originPos' field.
			/// </summary>
			// Token: 0x040064DD RID: 25821
			public static readonly StringName _originPos = "_originPos";

			/// <summary>
			/// Cached name for the '_noise' field.
			/// </summary>
			// Token: 0x040064DE RID: 25822
			public static readonly StringName _noise = "_noise";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x040064DF RID: 25823
			public static readonly StringName _time = "_time";

			/// <summary>
			/// Cached name for the '_decimillipedeStrikeOffset' field.
			/// </summary>
			// Token: 0x040064E0 RID: 25824
			public static readonly StringName _decimillipedeStrikeOffset = "_decimillipedeStrikeOffset";

			/// <summary>
			/// Cached name for the '_attackTween' field.
			/// </summary>
			// Token: 0x040064E1 RID: 25825
			public static readonly StringName _attackTween = "_attackTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001984 RID: 6532
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
