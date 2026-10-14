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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000186 RID: 390
	[ScriptPath("res://src/Core/Nodes/Vfx/NBgGroundSpikeVfx.cs")]
	public class NBgGroundSpikeVfx : Sprite2D
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x00070BB8 File Offset: 0x0006EDB8
		[NullableContext(2)]
		public static NBgGroundSpikeVfx Create(Vector2 position, bool movingRight = true, VfxColor vfxColor = VfxColor.Red)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBgGroundSpikeVfx nbgGroundSpikeVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/bg_ground_spike_vfx.tscn").Instantiate<NBgGroundSpikeVfx>(0L);
			nbgGroundSpikeVfx._startPosition = position;
			nbgGroundSpikeVfx._movingRight = movingRight;
			nbgGroundSpikeVfx._vfxColor = vfxColor;
			return nbgGroundSpikeVfx;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00070BFC File Offset: 0x0006EDFC
		public override void _Ready()
		{
			base.Skew = (this._movingRight ? (Rng.Chaotic.NextFloat(15f, 30f) * 0.0174533f) : (Rng.Chaotic.NextFloat(-30f, -15f) * 0.0174533f));
			float num = Rng.Chaotic.NextFloat(0.5f, 1.5f);
			base.Scale = new Vector2(Rng.Chaotic.NextFloat(0.8f, 1.2f), Rng.Chaotic.NextFloat(0.8f, 2f)) * num;
			this.AdjustStartPosition();
			base.GlobalPosition = this._startPosition;
			this._velocity = new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(50f, 250f) : Rng.Chaotic.NextFloat(-250f, -50f), Rng.Chaotic.NextFloat(-5f, 5f)) / num;
			this.SetColor();
			TaskHelper.RunSafely(this.Animate());
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00070D18 File Offset: 0x0006EF18
		private void SetColor()
		{
			switch (this._vfxColor)
			{
			case VfxColor.Red:
				base.Modulate = new Color(1f, Rng.Chaotic.NextFloat(0.2f, 0.8f), Rng.Chaotic.NextFloat(0f, 0.2f), 0.5f);
				return;
			case VfxColor.Purple:
				base.Modulate = new Color(Rng.Chaotic.NextFloat(0f, 0.2f), Rng.Chaotic.NextFloat(0.2f, 0.8f), 1f, 0.5f);
				return;
			case VfxColor.White:
			{
				float num = Rng.Chaotic.NextFloat(0.2f, 0.8f);
				base.Modulate = new Color(num, num, num, 0.5f);
				return;
			}
			case VfxColor.Cyan:
			{
				float num2 = Rng.Chaotic.NextFloat(0.6f, 1f);
				base.Modulate = new Color(0.2f, num2, num2, 0.5f);
				return;
			}
			case VfxColor.Gold:
			{
				float num3 = Rng.Chaotic.NextFloat(0.6f, 1f);
				base.Modulate = new Color(num3, num3, 0.2f, 1f);
				return;
			}
			}
			Log.Error("Color: " + this._vfxColor.ToString() + " not implemented", 2);
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00070E84 File Offset: 0x0006F084
		protected virtual void AdjustStartPosition()
		{
			this._startPosition += new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(40f, 160f) : Rng.Chaotic.NextFloat(-160f, -40f), Rng.Chaotic.NextFloat(-96f, -10f));
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x00070EED File Offset: 0x0006F0ED
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00070F00 File Offset: 0x0006F100
		[NullableContext(1)]
		private async Task Animate()
		{
			float num = Rng.Chaotic.NextFloat(0.25f, 1f);
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenInterval((double)Rng.Chaotic.NextFloat(0.01f, 0.2f));
			this._tween.Chain();
			this._tween.TweenProperty(this, "skew", this._movingRight ? (Rng.Chaotic.NextFloat(30f, 60f) * 0.0174533f) : (Rng.Chaotic.NextFloat(-60f, -30f) * 0.0174533f), (double)num).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this, "scale", base.Scale * Rng.Chaotic.NextFloat(0.1f, 0.5f), (double)num).SetEase(1L).SetTrans(5L);
			this._tween.TweenProperty(this, "modulate:a", 0f, (double)num).SetEase(1L).SetTrans(5L);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00070F43 File Offset: 0x0006F143
		public override void _Process(double delta)
		{
			base.Position += this._velocity * (float)delta;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012DD RID: 4829 RVA: 0x00070F64 File Offset: 0x0006F164
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NBgGroundSpikeVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Sprite2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false),
					new PropertyInfo(1L, "movingRight", 0L, "", 6L, false),
					new PropertyInfo(2L, "vfxColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBgGroundSpikeVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgGroundSpikeVfx.MethodName.SetColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgGroundSpikeVfx.MethodName.AdjustStartPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgGroundSpikeVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgGroundSpikeVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012DE RID: 4830 RVA: 0x0007112C File Offset: 0x0006F32C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBgGroundSpikeVfx.MethodName.Create && args.Count == 3)
			{
				NBgGroundSpikeVfx nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<VfxColor>(args[2]));
				ret = VariantUtils.CreateFrom<NBgGroundSpikeVfx>(ref nbgGroundSpikeVfx);
				return true;
			}
			if ((ref method) == NBgGroundSpikeVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgGroundSpikeVfx.MethodName.SetColor && args.Count == 0)
			{
				this.SetColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgGroundSpikeVfx.MethodName.AdjustStartPosition && args.Count == 0)
			{
				this.AdjustStartPosition();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgGroundSpikeVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgGroundSpikeVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0007125C File Offset: 0x0006F45C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBgGroundSpikeVfx.MethodName.Create && args.Count == 3)
			{
				NBgGroundSpikeVfx nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]), VariantUtils.ConvertTo<VfxColor>(args[2]));
				ret = VariantUtils.CreateFrom<NBgGroundSpikeVfx>(ref nbgGroundSpikeVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060012E0 RID: 4832 RVA: 0x000712C4 File Offset: 0x0006F4C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBgGroundSpikeVfx.MethodName.Create || (ref method) == NBgGroundSpikeVfx.MethodName._Ready || (ref method) == NBgGroundSpikeVfx.MethodName.SetColor || (ref method) == NBgGroundSpikeVfx.MethodName.AdjustStartPosition || (ref method) == NBgGroundSpikeVfx.MethodName._ExitTree || (ref method) == NBgGroundSpikeVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012E1 RID: 4833 RVA: 0x00071334 File Offset: 0x0006F534
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._startPosition)
			{
				this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._movingRight)
			{
				this._movingRight = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._velocity)
			{
				this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012E2 RID: 4834 RVA: 0x000713D0 File Offset: 0x0006F5D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._startPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._movingRight)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._movingRight);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._velocity)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
				return true;
			}
			if ((ref name) == NBgGroundSpikeVfx.PropertyName._tween)
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
		// Token: 0x060012E3 RID: 4835 RVA: 0x00071488 File Offset: 0x0006F688
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NBgGroundSpikeVfx.PropertyName._startPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBgGroundSpikeVfx.PropertyName._movingRight, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBgGroundSpikeVfx.PropertyName._vfxColor, 0L, "", 4096L, false),
				new PropertyInfo(5L, NBgGroundSpikeVfx.PropertyName._velocity, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBgGroundSpikeVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012E4 RID: 4836 RVA: 0x00071540 File Offset: 0x0006F740
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBgGroundSpikeVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
			info.AddProperty(NBgGroundSpikeVfx.PropertyName._movingRight, Variant.From<bool>(ref this._movingRight));
			info.AddProperty(NBgGroundSpikeVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
			info.AddProperty(NBgGroundSpikeVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
			info.AddProperty(NBgGroundSpikeVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060012E5 RID: 4837 RVA: 0x000715C4 File Offset: 0x0006F7C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._startPosition, ref variant))
			{
				this._startPosition = variant.As<Vector2>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._movingRight, ref variant2))
			{
				this._movingRight = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._vfxColor, ref variant3))
			{
				this._vfxColor = variant3.As<VfxColor>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._velocity, ref variant4))
			{
				this._velocity = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._tween, ref variant5))
			{
				this._tween = variant5.As<Tween>();
			}
		}

		// Token: 0x040006AF RID: 1711
		[Nullable(1)]
		private const string _scenePath = "res://scenes/vfx/bg_ground_spike_vfx.tscn";

		// Token: 0x040006B0 RID: 1712
		protected Vector2 _startPosition;

		// Token: 0x040006B1 RID: 1713
		protected bool _movingRight = true;

		// Token: 0x040006B2 RID: 1714
		protected VfxColor _vfxColor;

		// Token: 0x040006B3 RID: 1715
		private Vector2 _velocity;

		// Token: 0x040006B4 RID: 1716
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA2 RID: 4002
		public class MethodName : Sprite2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003E58 RID: 15960
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E59 RID: 15961
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetColor' method.
			/// </summary>
			// Token: 0x04003E5A RID: 15962
			public static readonly StringName SetColor = "SetColor";

			/// <summary>
			/// Cached name for the 'AdjustStartPosition' method.
			/// </summary>
			// Token: 0x04003E5B RID: 15963
			public static readonly StringName AdjustStartPosition = "AdjustStartPosition";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003E5C RID: 15964
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003E5D RID: 15965
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA3 RID: 4003
		public class PropertyName : Sprite2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_startPosition' field.
			/// </summary>
			// Token: 0x04003E5E RID: 15966
			public static readonly StringName _startPosition = "_startPosition";

			/// <summary>
			/// Cached name for the '_movingRight' field.
			/// </summary>
			// Token: 0x04003E5F RID: 15967
			public static readonly StringName _movingRight = "_movingRight";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x04003E60 RID: 15968
			public static readonly StringName _vfxColor = "_vfxColor";

			/// <summary>
			/// Cached name for the '_velocity' field.
			/// </summary>
			// Token: 0x04003E61 RID: 15969
			public static readonly StringName _velocity = "_velocity";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003E62 RID: 15970
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FA4 RID: 4004
		public class SignalName : Sprite2D.SignalName
		{
		}
	}
}
