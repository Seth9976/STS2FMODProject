using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001A5 RID: 421
	[ScriptPath("res://src/Core/Nodes/Vfx/NEpochSlotParticle.cs")]
	public class NEpochSlotParticle : Sprite2D
	{
		// Token: 0x060014D3 RID: 5331 RVA: 0x0007F380 File Offset: 0x0007D580
		[NullableContext(1)]
		public static NEpochSlotParticle Create(Control target)
		{
			NEpochSlotParticle nepochSlotParticle = PreloadManager.Cache.GetScene(NEpochSlotParticle.scenePath).Instantiate<NEpochSlotParticle>(0L);
			nepochSlotParticle._target = target;
			return nepochSlotParticle;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0007F3AC File Offset: 0x0007D5AC
		public override void _Ready()
		{
			base.GlobalPosition = this._target.GlobalPosition + new Vector2(Rng.Chaotic.NextFloat(40f, 350f) * (Rng.Chaotic.NextBool() ? (-1f) : 1f), Rng.Chaotic.NextFloat(40f, 300f) * (Rng.Chaotic.NextBool() ? (-1f) : 1f));
			float num = Rng.Chaotic.NextFloat(0.5f, 1.5f);
			base.Scale = new Vector2(1f, 1f) * num;
			base.Modulate = new Color(Rng.Chaotic.NextFloat(0.75f, 0.9f), Rng.Chaotic.NextFloat(0.7f, 0.8f), Rng.Chaotic.NextFloat(0f, 0.4f), 0f);
			this._speed = Rng.Chaotic.NextFloat(3f, 4f) / num;
			Vector2 vector = this._target.GlobalPosition - base.GlobalPosition;
			base.Rotation = Mathf.Atan2(vector.Y, vector.X) + 1.5707964f;
			Tween tween = base.CreateTween();
			tween.TweenProperty(this, "modulate:a", Rng.Chaotic.NextFloat(0.75f, 1f), 0.25);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0007F538 File Offset: 0x0007D738
		public override void _Process(double delta)
		{
			this._timer -= delta;
			if (this._timer < 0.0)
			{
				this._timer += 0.25;
				if (base.GlobalPosition.DistanceSquaredTo(this._target.GlobalPosition) < 1500f)
				{
					this.QueueFreeSafely();
				}
			}
			base.GlobalPosition = base.GlobalPosition.Lerp(this._target.GlobalPosition, (float)((double)this._speed * delta));
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014D6 RID: 5334 RVA: 0x0007F5CC File Offset: 0x0007D7CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NEpochSlotParticle.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Sprite2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "target", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NEpochSlotParticle.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochSlotParticle.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014D7 RID: 5335 RVA: 0x0007F6D0 File Offset: 0x0007D8D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochSlotParticle.MethodName.Create && args.Count == 1)
			{
				NEpochSlotParticle nepochSlotParticle = NEpochSlotParticle.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NEpochSlotParticle>(ref nepochSlotParticle);
				return true;
			}
			if ((ref method) == NEpochSlotParticle.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochSlotParticle.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0007F778 File Offset: 0x0007D978
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochSlotParticle.MethodName.Create && args.Count == 1)
			{
				NEpochSlotParticle nepochSlotParticle = NEpochSlotParticle.Create(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<NEpochSlotParticle>(ref nepochSlotParticle);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060014D9 RID: 5337 RVA: 0x0007F7C6 File Offset: 0x0007D9C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochSlotParticle.MethodName.Create || (ref method) == NEpochSlotParticle.MethodName._Ready || (ref method) == NEpochSlotParticle.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014DA RID: 5338 RVA: 0x0007F7FC File Offset: 0x0007D9FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochSlotParticle.PropertyName._timer)
			{
				this._timer = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlotParticle.PropertyName._target)
			{
				this._target = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NEpochSlotParticle.PropertyName._speed)
			{
				this._speed = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060014DB RID: 5339 RVA: 0x0007F864 File Offset: 0x0007DA64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochSlotParticle.PropertyName._timer)
			{
				value = VariantUtils.CreateFrom<double>(ref this._timer);
				return true;
			}
			if ((ref name) == NEpochSlotParticle.PropertyName._target)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._target);
				return true;
			}
			if ((ref name) == NEpochSlotParticle.PropertyName._speed)
			{
				value = VariantUtils.CreateFrom<float>(ref this._speed);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014DC RID: 5340 RVA: 0x0007F8DC File Offset: 0x0007DADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NEpochSlotParticle.PropertyName._timer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochSlotParticle.PropertyName._target, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochSlotParticle.PropertyName._speed, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014DD RID: 5341 RVA: 0x0007F954 File Offset: 0x0007DB54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochSlotParticle.PropertyName._timer, Variant.From<double>(ref this._timer));
			info.AddProperty(NEpochSlotParticle.PropertyName._target, Variant.From<Control>(ref this._target));
			info.AddProperty(NEpochSlotParticle.PropertyName._speed, Variant.From<float>(ref this._speed));
		}

		/// <inheritdoc />
		// Token: 0x060014DE RID: 5342 RVA: 0x0007F9AC File Offset: 0x0007DBAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochSlotParticle.PropertyName._timer, ref variant))
			{
				this._timer = variant.As<double>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochSlotParticle.PropertyName._target, ref variant2))
			{
				this._target = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochSlotParticle.PropertyName._speed, ref variant3))
			{
				this._speed = variant3.As<float>();
			}
		}

		// Token: 0x0400077A RID: 1914
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/epoch_slot_particle_vfx");

		// Token: 0x0400077B RID: 1915
		private const double _checkRate = 0.25;

		// Token: 0x0400077C RID: 1916
		private double _timer;

		// Token: 0x0400077D RID: 1917
		[Nullable(1)]
		private Control _target;

		// Token: 0x0400077E RID: 1918
		private float _speed;

		// Token: 0x0400077F RID: 1919
		private const float _deleteDistance = 1500f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101B RID: 4123
		public class MethodName : Sprite2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FEA RID: 16362
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FEB RID: 16363
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003FEC RID: 16364
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101C RID: 4124
		public class PropertyName : Sprite2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_timer' field.
			/// </summary>
			// Token: 0x04003FED RID: 16365
			public static readonly StringName _timer = "_timer";

			/// <summary>
			/// Cached name for the '_target' field.
			/// </summary>
			// Token: 0x04003FEE RID: 16366
			public static readonly StringName _target = "_target";

			/// <summary>
			/// Cached name for the '_speed' field.
			/// </summary>
			// Token: 0x04003FEF RID: 16367
			public static readonly StringName _speed = "_speed";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101D RID: 4125
		public class SignalName : Sprite2D.SignalName
		{
		}
	}
}
