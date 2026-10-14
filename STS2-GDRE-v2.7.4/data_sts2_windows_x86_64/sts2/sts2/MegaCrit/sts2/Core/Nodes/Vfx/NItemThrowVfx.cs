using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001C1 RID: 449
	[ScriptPath("res://src/Core/Nodes/Vfx/NItemThrowVfx.cs")]
	public class NItemThrowVfx : Node2D
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x00089538 File Offset: 0x00087738
		[NullableContext(2)]
		public static NItemThrowVfx Create(Vector2 sourcePosition, Vector2 targetPosition, Texture2D itemTexture, Vector2? scale = null)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NItemThrowVfx nitemThrowVfx = PreloadManager.Cache.GetScene(NItemThrowVfx.scenePath).Instantiate<NItemThrowVfx>(0L);
			nitemThrowVfx._sourcePosition = sourcePosition;
			nitemThrowVfx._targetPosition = targetPosition;
			if (nitemThrowVfx._itemSprite != null)
			{
				nitemThrowVfx._itemSprite.Visible = false;
				nitemThrowVfx._itemSprite.Scale = scale ?? Vector2.One;
				if (itemTexture != null)
				{
					nitemThrowVfx._itemSprite.Texture = itemTexture;
					nitemThrowVfx._itemSprite.Scale *= 80f / (float)itemTexture.GetWidth();
				}
			}
			return nitemThrowVfx;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x000895DE File Offset: 0x000877DE
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.ThrowItem());
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000895EC File Offset: 0x000877EC
		[NullableContext(1)]
		private async Task ThrowItem()
		{
			this._itemSprite.Visible = true;
			this._itemSprite.GlobalPosition = this._sourcePosition;
			this._itemSprite.RotationDegrees = Rng.Chaotic.NextFloat(360f);
			double timer = 0.0;
			while (timer < (double)this._flightTime)
			{
				double processDeltaTime = base.GetProcessDeltaTime();
				float num = (float)(timer / (double)this._flightTime);
				float num2 = this._horizontalCurve.Sample(num);
				float num3 = this._verticalCurve.Sample(num);
				float num4 = this._rotationInfluenceCurve.Sample(num);
				this._itemSprite.Rotate((float)Mathf.DegToRad((double)(num4 * this._rotationMultiplier) * processDeltaTime));
				Vector2 vector = this._sourcePosition.Lerp(this._targetPosition, num2) + Vector2.Up * num3 * this._heightMultiplier;
				this._itemSprite.GlobalPosition = vector;
				timer += processDeltaTime;
				await this.AwaitProcessFrame(default(CancellationToken));
			}
			this._itemSprite.Visible = false;
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001672 RID: 5746 RVA: 0x00089630 File Offset: 0x00087830
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NItemThrowVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001673 RID: 5747 RVA: 0x00089674 File Offset: 0x00087874
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NItemThrowVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001674 RID: 5748 RVA: 0x000896A4 File Offset: 0x000878A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NItemThrowVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001675 RID: 5749 RVA: 0x000896BC File Offset: 0x000878BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NItemThrowVfx.PropertyName._itemSprite)
			{
				this._itemSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._flightTime)
			{
				this._flightTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._heightMultiplier)
			{
				this._heightMultiplier = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._horizontalCurve)
			{
				this._horizontalCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._verticalCurve)
			{
				this._verticalCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._rotationMultiplier)
			{
				this._rotationMultiplier = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._rotationInfluenceCurve)
			{
				this._rotationInfluenceCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._sourcePosition)
			{
				this._sourcePosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._targetPosition)
			{
				this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001676 RID: 5750 RVA: 0x000897C4 File Offset: 0x000879C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NItemThrowVfx.PropertyName._itemSprite)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._itemSprite);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._flightTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._flightTime);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._heightMultiplier)
			{
				value = VariantUtils.CreateFrom<float>(ref this._heightMultiplier);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._horizontalCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._horizontalCurve);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._verticalCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._verticalCurve);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._rotationMultiplier)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rotationMultiplier);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._rotationInfluenceCurve)
			{
				value = VariantUtils.CreateFrom<Curve>(ref this._rotationInfluenceCurve);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._sourcePosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._sourcePosition);
				return true;
			}
			if ((ref name) == NItemThrowVfx.PropertyName._targetPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001677 RID: 5751 RVA: 0x000898FC File Offset: 0x00087AFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NItemThrowVfx.PropertyName._itemSprite, 34L, "Sprite2D", 4102L, true),
				new PropertyInfo(3L, NItemThrowVfx.PropertyName._flightTime, 0L, "", 4102L, true),
				new PropertyInfo(3L, NItemThrowVfx.PropertyName._heightMultiplier, 0L, "", 4102L, true),
				new PropertyInfo(24L, NItemThrowVfx.PropertyName._horizontalCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NItemThrowVfx.PropertyName._verticalCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(3L, NItemThrowVfx.PropertyName._rotationMultiplier, 0L, "", 4102L, true),
				new PropertyInfo(24L, NItemThrowVfx.PropertyName._rotationInfluenceCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(5L, NItemThrowVfx.PropertyName._sourcePosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NItemThrowVfx.PropertyName._targetPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001678 RID: 5752 RVA: 0x00089A38 File Offset: 0x00087C38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NItemThrowVfx.PropertyName._itemSprite, Variant.From<Sprite2D>(ref this._itemSprite));
			info.AddProperty(NItemThrowVfx.PropertyName._flightTime, Variant.From<float>(ref this._flightTime));
			info.AddProperty(NItemThrowVfx.PropertyName._heightMultiplier, Variant.From<float>(ref this._heightMultiplier));
			info.AddProperty(NItemThrowVfx.PropertyName._horizontalCurve, Variant.From<Curve>(ref this._horizontalCurve));
			info.AddProperty(NItemThrowVfx.PropertyName._verticalCurve, Variant.From<Curve>(ref this._verticalCurve));
			info.AddProperty(NItemThrowVfx.PropertyName._rotationMultiplier, Variant.From<float>(ref this._rotationMultiplier));
			info.AddProperty(NItemThrowVfx.PropertyName._rotationInfluenceCurve, Variant.From<Curve>(ref this._rotationInfluenceCurve));
			info.AddProperty(NItemThrowVfx.PropertyName._sourcePosition, Variant.From<Vector2>(ref this._sourcePosition));
			info.AddProperty(NItemThrowVfx.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
		}

		/// <inheritdoc />
		// Token: 0x06001679 RID: 5753 RVA: 0x00089B14 File Offset: 0x00087D14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._itemSprite, ref variant))
			{
				this._itemSprite = variant.As<Sprite2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._flightTime, ref variant2))
			{
				this._flightTime = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._heightMultiplier, ref variant3))
			{
				this._heightMultiplier = variant3.As<float>();
			}
			Variant variant4;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._horizontalCurve, ref variant4))
			{
				this._horizontalCurve = variant4.As<Curve>();
			}
			Variant variant5;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._verticalCurve, ref variant5))
			{
				this._verticalCurve = variant5.As<Curve>();
			}
			Variant variant6;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._rotationMultiplier, ref variant6))
			{
				this._rotationMultiplier = variant6.As<float>();
			}
			Variant variant7;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._rotationInfluenceCurve, ref variant7))
			{
				this._rotationInfluenceCurve = variant7.As<Curve>();
			}
			Variant variant8;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._sourcePosition, ref variant8))
			{
				this._sourcePosition = variant8.As<Vector2>();
			}
			Variant variant9;
			if (info.TryGetProperty(NItemThrowVfx.PropertyName._targetPosition, ref variant9))
			{
				this._targetPosition = variant9.As<Vector2>();
			}
		}

		// Token: 0x040007F7 RID: 2039
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_item_throw");

		/// <summary>
		/// The base size in pixels that we want our item icon to be at.
		/// </summary>
		// Token: 0x040007F8 RID: 2040
		private const float _baseItemSize = 80f;

		// Token: 0x040007F9 RID: 2041
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Sprite2D _itemSprite;

		// Token: 0x040007FA RID: 2042
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _flightTime;

		// Token: 0x040007FB RID: 2043
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _heightMultiplier;

		// Token: 0x040007FC RID: 2044
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _horizontalCurve;

		// Token: 0x040007FD RID: 2045
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _verticalCurve;

		// Token: 0x040007FE RID: 2046
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private float _rotationMultiplier;

		// Token: 0x040007FF RID: 2047
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Curve _rotationInfluenceCurve;

		// Token: 0x04000800 RID: 2048
		private Vector2 _sourcePosition;

		// Token: 0x04000801 RID: 2049
		private Vector2 _targetPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001080 RID: 4224
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040DF RID: 16607
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001081 RID: 4225
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_itemSprite' field.
			/// </summary>
			// Token: 0x040040E0 RID: 16608
			public static readonly StringName _itemSprite = "_itemSprite";

			/// <summary>
			/// Cached name for the '_flightTime' field.
			/// </summary>
			// Token: 0x040040E1 RID: 16609
			public static readonly StringName _flightTime = "_flightTime";

			/// <summary>
			/// Cached name for the '_heightMultiplier' field.
			/// </summary>
			// Token: 0x040040E2 RID: 16610
			public static readonly StringName _heightMultiplier = "_heightMultiplier";

			/// <summary>
			/// Cached name for the '_horizontalCurve' field.
			/// </summary>
			// Token: 0x040040E3 RID: 16611
			public static readonly StringName _horizontalCurve = "_horizontalCurve";

			/// <summary>
			/// Cached name for the '_verticalCurve' field.
			/// </summary>
			// Token: 0x040040E4 RID: 16612
			public static readonly StringName _verticalCurve = "_verticalCurve";

			/// <summary>
			/// Cached name for the '_rotationMultiplier' field.
			/// </summary>
			// Token: 0x040040E5 RID: 16613
			public static readonly StringName _rotationMultiplier = "_rotationMultiplier";

			/// <summary>
			/// Cached name for the '_rotationInfluenceCurve' field.
			/// </summary>
			// Token: 0x040040E6 RID: 16614
			public static readonly StringName _rotationInfluenceCurve = "_rotationInfluenceCurve";

			/// <summary>
			/// Cached name for the '_sourcePosition' field.
			/// </summary>
			// Token: 0x040040E7 RID: 16615
			public static readonly StringName _sourcePosition = "_sourcePosition";

			/// <summary>
			/// Cached name for the '_targetPosition' field.
			/// </summary>
			// Token: 0x040040E8 RID: 16616
			public static readonly StringName _targetPosition = "_targetPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001082 RID: 4226
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
