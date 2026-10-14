using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Sub Vfx for doom that manage the front and back particle effects
	/// (the glow, the light spears, the ground)
	/// </summary>
	// Token: 0x020001A0 RID: 416
	[ScriptPath("res://src/Core/Nodes/Vfx/NDoomSubEmitterVfx.cs")]
	public class NDoomSubEmitterVfx : Node2D
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x0007BF05 File Offset: 0x0007A105
		// (set) Token: 0x06001483 RID: 5251 RVA: 0x0007BF0D File Offset: 0x0007A10D
		public float CurScaleX
		{
			get
			{
				return this._curScaleX;
			}
			set
			{
				this._curScaleX = value;
				this.UpdateWidth(this._curScaleX);
			}
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0007BF24 File Offset: 0x0007A124
		public override void _Ready()
		{
			this._baseScales = new Array<Vector2>();
			this._indeces = new Array<int>();
			foreach (Node2D node2D in this._scalableLayers)
			{
				this._baseScales.Add(node2D.Scale);
				this._indeces.Add(node2D.GetIndex(false));
			}
			this._isOn = false;
			this.SetVisibility(false);
			this.UpdateWidth(0f);
			this.ShowOrHide(0f, 0f);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0007BFCC File Offset: 0x0007A1CC
		[NullableContext(1)]
		private void FireSpear(TextureRect textureRect = null)
		{
			Vector2 position = textureRect.Position;
			position.X = Rng.Chaotic.NextFloat(this._baseSpearRegionWidth * -0.5f, this._baseSpearRegionWidth * 0.5f) + this._dumbHackBecauseOfHowTexturerectsWork;
			textureRect.Position = position;
			textureRect.RotationDegrees = textureRect.Position.X * this._spearAngleIntensity + this._rotationHackForSameDumbReason;
			textureRect.Modulate = Colors.Transparent;
			float num = this._spearFixedHScale / this._curScaleX;
			textureRect.Scale = new Vector2(num, Rng.Chaotic.NextFloat(this._minSpearSize, this._maxSpearSize));
			Vector2 vector;
			vector..ctor(num, 0f);
			float num2 = Rng.Chaotic.NextFloat(this._minSpearTime, this._maxSpearTime);
			Tween tween = textureRect.CreateTween();
			tween.TweenProperty(textureRect, "scale", vector, (double)num2).From(new Vector2(num, Rng.Chaotic.NextFloat(this._minSpearSize, this._maxSpearSize)));
			if (this._isOn)
			{
				tween.TweenCallback(Callable.From(delegate
				{
					this.FireSpear(textureRect);
				}));
			}
			Tween tween2 = textureRect.CreateTween();
			float num3 = Rng.Chaotic.NextFloat(0.4f, 0.7f);
			tween2.TweenProperty(textureRect, "modulate", new Color(1f, 1f, 1f, num3), 0.20000000298023224).SetEase(0L).SetTrans(4L);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0007C1AC File Offset: 0x0007A3AC
		private void FireAllSpears()
		{
			foreach (TextureRect textureRect in this._spears)
			{
				this.FireSpear(textureRect);
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0007C1FC File Offset: 0x0007A3FC
		public void ShowOrHide(float widthScale, float tweenTime)
		{
			this._isOn = widthScale > 0.1f;
			float num = (this._isOn ? 0f : 0.3f);
			float num2 = widthScale;
			Tween.EaseType easeType;
			if (this._isOn)
			{
				this.SetVisibility(true);
				easeType = 1L;
				int num3 = (((int)widthScale * this._baseParticleDensity > 1) ? ((int)widthScale * this._baseParticleDensity) : 1);
				this._particlesToKeepDense.Amount = num3;
			}
			else
			{
				easeType = 0L;
				num2 = 0f;
				tweenTime = this._curScaleX * 0.15f;
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "CurScaleX", num2, (double)tweenTime).SetDelay((double)num).SetEase(easeType)
				.SetTrans(7L);
			if (!this._isOn)
			{
				this._tween.TweenCallback(Callable.From(delegate
				{
					this.SetVisibility(false);
				}));
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0007C2F4 File Offset: 0x0007A4F4
		private void UpdateWidth(float width)
		{
			this._curScaleX = width;
			int num = 0;
			foreach (Node2D node2D in this._scalableLayers)
			{
				Vector2 vector = this._baseScales[num];
				Vector2 vector2;
				if (node2D == this._verticalShrinkingLayer && width < 1f)
				{
					vector2..ctor(vector.X * this._curScaleX, vector.Y * width / 1f);
				}
				else
				{
					vector2..ctor(vector.X * this._curScaleX, vector.Y);
				}
				node2D.Scale = vector2;
				num++;
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0007C3AC File Offset: 0x0007A5AC
		private void SetVisibility(bool isOn)
		{
			int num = 0;
			foreach (Node2D node2D in this._scalableLayers)
			{
				GpuParticles2D gpuParticles2D = node2D as GpuParticles2D;
				if (gpuParticles2D != null)
				{
					if (isOn)
					{
						gpuParticles2D.Restart();
					}
					else
					{
						gpuParticles2D.Emitting = false;
					}
				}
				else if (isOn)
				{
					this.MoveChildSafely(node2D, this._indeces[num]);
				}
				num++;
			}
			if (isOn)
			{
				this.FireAllSpears();
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0007C438 File Offset: 0x0007A638
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
		// Token: 0x0600148B RID: 5259 RVA: 0x0007C44C File Offset: 0x0007A64C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NDoomSubEmitterVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName.FireSpear, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "textureRect", 0L, "", 6L, new StringName("TextureRect"), false)
				}, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName.FireAllSpears, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName.ShowOrHide, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "widthScale", 0L, "", 6L, false),
					new PropertyInfo(3L, "tweenTime", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName.UpdateWidth, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "width", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName.SetVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isOn", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDoomSubEmitterVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600148C RID: 5260 RVA: 0x0007C66C File Offset: 0x0007A86C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDoomSubEmitterVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName.FireSpear && args.Count == 1)
			{
				this.FireSpear(VariantUtils.ConvertTo<TextureRect>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName.FireAllSpears && args.Count == 0)
			{
				this.FireAllSpears();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName.ShowOrHide && args.Count == 2)
			{
				this.ShowOrHide(VariantUtils.ConvertTo<float>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName.UpdateWidth && args.Count == 1)
			{
				this.UpdateWidth(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName.SetVisibility && args.Count == 1)
			{
				this.SetVisibility(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDoomSubEmitterVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600148D RID: 5261 RVA: 0x0007C7CC File Offset: 0x0007A9CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDoomSubEmitterVfx.MethodName._Ready || (ref method) == NDoomSubEmitterVfx.MethodName.FireSpear || (ref method) == NDoomSubEmitterVfx.MethodName.FireAllSpears || (ref method) == NDoomSubEmitterVfx.MethodName.ShowOrHide || (ref method) == NDoomSubEmitterVfx.MethodName.UpdateWidth || (ref method) == NDoomSubEmitterVfx.MethodName.SetVisibility || (ref method) == NDoomSubEmitterVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600148E RID: 5262 RVA: 0x0007C84C File Offset: 0x0007AA4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDoomSubEmitterVfx.PropertyName.CurScaleX)
			{
				this.CurScaleX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._scalableLayers)
			{
				this._scalableLayers = VariantUtils.ConvertToArray<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spears)
			{
				this._spears = VariantUtils.ConvertToArray<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer)
			{
				this._verticalShrinkingLayer = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._particlesToKeepDense)
			{
				this._particlesToKeepDense = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseScales)
			{
				this._baseScales = VariantUtils.ConvertToArray<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._indeces)
			{
				this._indeces = VariantUtils.ConvertToArray<int>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth)
			{
				this._baseSpearRegionWidth = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork)
			{
				this._dumbHackBecauseOfHowTexturerectsWork = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason)
			{
				this._rotationHackForSameDumbReason = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseParticleDensity)
			{
				this._baseParticleDensity = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spearFixedHScale)
			{
				this._spearFixedHScale = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spearAngleIntensity)
			{
				this._spearAngleIntensity = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._minSpearSize)
			{
				this._minSpearSize = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._maxSpearSize)
			{
				this._maxSpearSize = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._minSpearTime)
			{
				this._minSpearTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._maxSpearTime)
			{
				this._maxSpearTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._outerMargin)
			{
				this._outerMargin = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._innerMargin)
			{
				this._innerMargin = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._time)
			{
				this._time = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._isOn)
			{
				this._isOn = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._curScaleX)
			{
				this._curScaleX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600148F RID: 5263 RVA: 0x0007CAD0 File Offset: 0x0007ACD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDoomSubEmitterVfx.PropertyName.CurScaleX)
			{
				float curScaleX = this.CurScaleX;
				value = VariantUtils.CreateFrom<float>(ref curScaleX);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._scalableLayers)
			{
				value = VariantUtils.CreateFromArray<Node2D>(this._scalableLayers);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spears)
			{
				value = VariantUtils.CreateFromArray<TextureRect>(this._spears);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._verticalShrinkingLayer);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._particlesToKeepDense)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particlesToKeepDense);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseScales)
			{
				value = VariantUtils.CreateFromArray<Vector2>(this._baseScales);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._indeces)
			{
				value = VariantUtils.CreateFromArray<int>(this._indeces);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseSpearRegionWidth);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork)
			{
				value = VariantUtils.CreateFrom<float>(ref this._dumbHackBecauseOfHowTexturerectsWork);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rotationHackForSameDumbReason);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._baseParticleDensity)
			{
				value = VariantUtils.CreateFrom<int>(ref this._baseParticleDensity);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spearFixedHScale)
			{
				value = VariantUtils.CreateFrom<float>(ref this._spearFixedHScale);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._spearAngleIntensity)
			{
				value = VariantUtils.CreateFrom<float>(ref this._spearAngleIntensity);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._minSpearSize)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minSpearSize);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._maxSpearSize)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxSpearSize);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._minSpearTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minSpearTime);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._maxSpearTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._maxSpearTime);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._outerMargin)
			{
				value = VariantUtils.CreateFrom<float>(ref this._outerMargin);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._innerMargin)
			{
				value = VariantUtils.CreateFrom<float>(ref this._innerMargin);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._time)
			{
				value = VariantUtils.CreateFrom<double>(ref this._time);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._isOn)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isOn);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._curScaleX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._curScaleX);
				return true;
			}
			if ((ref name) == NDoomSubEmitterVfx.PropertyName._tween)
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
		// Token: 0x06001490 RID: 5264 RVA: 0x0007CDC8 File Offset: 0x0007AFC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NDoomSubEmitterVfx.PropertyName._scalableLayers, 23L, "24/34:Node2D", 4102L, true),
				new PropertyInfo(28L, NDoomSubEmitterVfx.PropertyName._spears, 23L, "24/34:TextureRect", 4102L, true),
				new PropertyInfo(24L, NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NDoomSubEmitterVfx.PropertyName._baseScales, 0L, "", 4096L, false),
				new PropertyInfo(28L, NDoomSubEmitterVfx.PropertyName._indeces, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDoomSubEmitterVfx.PropertyName._baseParticleDensity, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._spearFixedHScale, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._minSpearSize, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._maxSpearSize, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._minSpearTime, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._maxSpearTime, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._outerMargin, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._innerMargin, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._time, 0L, "", 4096L, false),
				new PropertyInfo(1L, NDoomSubEmitterVfx.PropertyName._isOn, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName._curScaleX, 0L, "", 4096L, false),
				new PropertyInfo(3L, NDoomSubEmitterVfx.PropertyName.CurScaleX, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDoomSubEmitterVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001491 RID: 5265 RVA: 0x0007D0C8 File Offset: 0x0007B2C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName curScaleX = NDoomSubEmitterVfx.PropertyName.CurScaleX;
			float curScaleX2 = this.CurScaleX;
			info.AddProperty(curScaleX, Variant.From<float>(ref curScaleX2));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._scalableLayers, Variant.CreateFrom<Node2D>(this._scalableLayers));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._spears, Variant.CreateFrom<TextureRect>(this._spears));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, Variant.From<Node2D>(ref this._verticalShrinkingLayer));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, Variant.From<GpuParticles2D>(ref this._particlesToKeepDense));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseScales, Variant.CreateFrom<Vector2>(this._baseScales));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._indeces, Variant.CreateFrom<int>(this._indeces));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, Variant.From<float>(ref this._baseSpearRegionWidth));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, Variant.From<float>(ref this._dumbHackBecauseOfHowTexturerectsWork));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, Variant.From<float>(ref this._rotationHackForSameDumbReason));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseParticleDensity, Variant.From<int>(ref this._baseParticleDensity));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._spearFixedHScale, Variant.From<float>(ref this._spearFixedHScale));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, Variant.From<float>(ref this._spearAngleIntensity));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._minSpearSize, Variant.From<float>(ref this._minSpearSize));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._maxSpearSize, Variant.From<float>(ref this._maxSpearSize));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._minSpearTime, Variant.From<float>(ref this._minSpearTime));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._maxSpearTime, Variant.From<float>(ref this._maxSpearTime));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._outerMargin, Variant.From<float>(ref this._outerMargin));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._innerMargin, Variant.From<float>(ref this._innerMargin));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._time, Variant.From<double>(ref this._time));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._isOn, Variant.From<bool>(ref this._isOn));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._curScaleX, Variant.From<float>(ref this._curScaleX));
			info.AddProperty(NDoomSubEmitterVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001492 RID: 5266 RVA: 0x0007D2DC File Offset: 0x0007B4DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName.CurScaleX, ref variant))
			{
				this.CurScaleX = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._scalableLayers, ref variant2))
			{
				this._scalableLayers = variant2.AsGodotArray<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spears, ref variant3))
			{
				this._spears = variant3.AsGodotArray<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, ref variant4))
			{
				this._verticalShrinkingLayer = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, ref variant5))
			{
				this._particlesToKeepDense = variant5.As<GpuParticles2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseScales, ref variant6))
			{
				this._baseScales = variant6.AsGodotArray<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._indeces, ref variant7))
			{
				this._indeces = variant7.AsGodotArray<int>();
			}
			Variant variant8;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, ref variant8))
			{
				this._baseSpearRegionWidth = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, ref variant9))
			{
				this._dumbHackBecauseOfHowTexturerectsWork = variant9.As<float>();
			}
			Variant variant10;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, ref variant10))
			{
				this._rotationHackForSameDumbReason = variant10.As<float>();
			}
			Variant variant11;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseParticleDensity, ref variant11))
			{
				this._baseParticleDensity = variant11.As<int>();
			}
			Variant variant12;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spearFixedHScale, ref variant12))
			{
				this._spearFixedHScale = variant12.As<float>();
			}
			Variant variant13;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, ref variant13))
			{
				this._spearAngleIntensity = variant13.As<float>();
			}
			Variant variant14;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._minSpearSize, ref variant14))
			{
				this._minSpearSize = variant14.As<float>();
			}
			Variant variant15;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._maxSpearSize, ref variant15))
			{
				this._maxSpearSize = variant15.As<float>();
			}
			Variant variant16;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._minSpearTime, ref variant16))
			{
				this._minSpearTime = variant16.As<float>();
			}
			Variant variant17;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._maxSpearTime, ref variant17))
			{
				this._maxSpearTime = variant17.As<float>();
			}
			Variant variant18;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._outerMargin, ref variant18))
			{
				this._outerMargin = variant18.As<float>();
			}
			Variant variant19;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._innerMargin, ref variant19))
			{
				this._innerMargin = variant19.As<float>();
			}
			Variant variant20;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._time, ref variant20))
			{
				this._time = variant20.As<double>();
			}
			Variant variant21;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._isOn, ref variant21))
			{
				this._isOn = variant21.As<bool>();
			}
			Variant variant22;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._curScaleX, ref variant22))
			{
				this._curScaleX = variant22.As<float>();
			}
			Variant variant23;
			if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._tween, ref variant23))
			{
				this._tween = variant23.As<Tween>();
			}
		}

		// Token: 0x0400074B RID: 1867
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<Node2D> _scalableLayers;

		// Token: 0x0400074C RID: 1868
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<TextureRect> _spears;

		// Token: 0x0400074D RID: 1869
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _verticalShrinkingLayer;

		// Token: 0x0400074E RID: 1870
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _particlesToKeepDense;

		// Token: 0x0400074F RID: 1871
		[Nullable(1)]
		private Array<Vector2> _baseScales;

		// Token: 0x04000750 RID: 1872
		[Nullable(1)]
		private Array<int> _indeces;

		// Token: 0x04000751 RID: 1873
		private float _baseSpearRegionWidth = 140f;

		// Token: 0x04000752 RID: 1874
		private float _dumbHackBecauseOfHowTexturerectsWork = -20f;

		// Token: 0x04000753 RID: 1875
		private float _rotationHackForSameDumbReason = 4f;

		// Token: 0x04000754 RID: 1876
		private int _baseParticleDensity = 300;

		// Token: 0x04000755 RID: 1877
		private float _spearFixedHScale = 0.4f;

		// Token: 0x04000756 RID: 1878
		private float _spearAngleIntensity = 0.15f;

		// Token: 0x04000757 RID: 1879
		private float _minSpearSize = 1f;

		// Token: 0x04000758 RID: 1880
		private float _maxSpearSize = 1.5f;

		// Token: 0x04000759 RID: 1881
		private float _minSpearTime = 0.3f;

		// Token: 0x0400075A RID: 1882
		private float _maxSpearTime = 0.6f;

		// Token: 0x0400075B RID: 1883
		private float _outerMargin = 0.2f;

		// Token: 0x0400075C RID: 1884
		private float _innerMargin = -0.2f;

		// Token: 0x0400075D RID: 1885
		private double _time = 2.0;

		// Token: 0x0400075E RID: 1886
		private bool _isOn;

		// Token: 0x0400075F RID: 1887
		private float _curScaleX = 1f;

		// Token: 0x04000760 RID: 1888
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001007 RID: 4103
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F8F RID: 16271
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'FireSpear' method.
			/// </summary>
			// Token: 0x04003F90 RID: 16272
			public static readonly StringName FireSpear = "FireSpear";

			/// <summary>
			/// Cached name for the 'FireAllSpears' method.
			/// </summary>
			// Token: 0x04003F91 RID: 16273
			public static readonly StringName FireAllSpears = "FireAllSpears";

			/// <summary>
			/// Cached name for the 'ShowOrHide' method.
			/// </summary>
			// Token: 0x04003F92 RID: 16274
			public static readonly StringName ShowOrHide = "ShowOrHide";

			/// <summary>
			/// Cached name for the 'UpdateWidth' method.
			/// </summary>
			// Token: 0x04003F93 RID: 16275
			public static readonly StringName UpdateWidth = "UpdateWidth";

			/// <summary>
			/// Cached name for the 'SetVisibility' method.
			/// </summary>
			// Token: 0x04003F94 RID: 16276
			public static readonly StringName SetVisibility = "SetVisibility";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F95 RID: 16277
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001008 RID: 4104
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurScaleX' property.
			/// </summary>
			// Token: 0x04003F96 RID: 16278
			public static readonly StringName CurScaleX = "CurScaleX";

			/// <summary>
			/// Cached name for the '_scalableLayers' field.
			/// </summary>
			// Token: 0x04003F97 RID: 16279
			public static readonly StringName _scalableLayers = "_scalableLayers";

			/// <summary>
			/// Cached name for the '_spears' field.
			/// </summary>
			// Token: 0x04003F98 RID: 16280
			public static readonly StringName _spears = "_spears";

			/// <summary>
			/// Cached name for the '_verticalShrinkingLayer' field.
			/// </summary>
			// Token: 0x04003F99 RID: 16281
			public static readonly StringName _verticalShrinkingLayer = "_verticalShrinkingLayer";

			/// <summary>
			/// Cached name for the '_particlesToKeepDense' field.
			/// </summary>
			// Token: 0x04003F9A RID: 16282
			public static readonly StringName _particlesToKeepDense = "_particlesToKeepDense";

			/// <summary>
			/// Cached name for the '_baseScales' field.
			/// </summary>
			// Token: 0x04003F9B RID: 16283
			public static readonly StringName _baseScales = "_baseScales";

			/// <summary>
			/// Cached name for the '_indeces' field.
			/// </summary>
			// Token: 0x04003F9C RID: 16284
			public static readonly StringName _indeces = "_indeces";

			/// <summary>
			/// Cached name for the '_baseSpearRegionWidth' field.
			/// </summary>
			// Token: 0x04003F9D RID: 16285
			public static readonly StringName _baseSpearRegionWidth = "_baseSpearRegionWidth";

			/// <summary>
			/// Cached name for the '_dumbHackBecauseOfHowTexturerectsWork' field.
			/// </summary>
			// Token: 0x04003F9E RID: 16286
			public static readonly StringName _dumbHackBecauseOfHowTexturerectsWork = "_dumbHackBecauseOfHowTexturerectsWork";

			/// <summary>
			/// Cached name for the '_rotationHackForSameDumbReason' field.
			/// </summary>
			// Token: 0x04003F9F RID: 16287
			public static readonly StringName _rotationHackForSameDumbReason = "_rotationHackForSameDumbReason";

			/// <summary>
			/// Cached name for the '_baseParticleDensity' field.
			/// </summary>
			// Token: 0x04003FA0 RID: 16288
			public static readonly StringName _baseParticleDensity = "_baseParticleDensity";

			/// <summary>
			/// Cached name for the '_spearFixedHScale' field.
			/// </summary>
			// Token: 0x04003FA1 RID: 16289
			public static readonly StringName _spearFixedHScale = "_spearFixedHScale";

			/// <summary>
			/// Cached name for the '_spearAngleIntensity' field.
			/// </summary>
			// Token: 0x04003FA2 RID: 16290
			public static readonly StringName _spearAngleIntensity = "_spearAngleIntensity";

			/// <summary>
			/// Cached name for the '_minSpearSize' field.
			/// </summary>
			// Token: 0x04003FA3 RID: 16291
			public static readonly StringName _minSpearSize = "_minSpearSize";

			/// <summary>
			/// Cached name for the '_maxSpearSize' field.
			/// </summary>
			// Token: 0x04003FA4 RID: 16292
			public static readonly StringName _maxSpearSize = "_maxSpearSize";

			/// <summary>
			/// Cached name for the '_minSpearTime' field.
			/// </summary>
			// Token: 0x04003FA5 RID: 16293
			public static readonly StringName _minSpearTime = "_minSpearTime";

			/// <summary>
			/// Cached name for the '_maxSpearTime' field.
			/// </summary>
			// Token: 0x04003FA6 RID: 16294
			public static readonly StringName _maxSpearTime = "_maxSpearTime";

			/// <summary>
			/// Cached name for the '_outerMargin' field.
			/// </summary>
			// Token: 0x04003FA7 RID: 16295
			public static readonly StringName _outerMargin = "_outerMargin";

			/// <summary>
			/// Cached name for the '_innerMargin' field.
			/// </summary>
			// Token: 0x04003FA8 RID: 16296
			public static readonly StringName _innerMargin = "_innerMargin";

			/// <summary>
			/// Cached name for the '_time' field.
			/// </summary>
			// Token: 0x04003FA9 RID: 16297
			public static readonly StringName _time = "_time";

			/// <summary>
			/// Cached name for the '_isOn' field.
			/// </summary>
			// Token: 0x04003FAA RID: 16298
			public static readonly StringName _isOn = "_isOn";

			/// <summary>
			/// Cached name for the '_curScaleX' field.
			/// </summary>
			// Token: 0x04003FAB RID: 16299
			public static readonly StringName _curScaleX = "_curScaleX";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003FAC RID: 16300
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001009 RID: 4105
		public class SignalName : Node2D.SignalName
		{
		}
	}
}
