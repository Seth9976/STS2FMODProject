using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	/// <summary>
	/// ScreenShake VFX script. Lives as a Node which shakes a specified Control node (_shakeTarget).
	/// Multiple effects can be stacked
	/// Can be called from anywhere using NGame.Instance.ScreenShake(), Screenpunch(), Rumble(), etc.
	/// See CombatRoom for example to attach this effect to other screens.
	/// </summary>
	// Token: 0x0200021A RID: 538
	[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NScreenShake.cs")]
	public class NScreenShake : Node
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x000B2D40 File Offset: 0x000B0F40
		// (set) Token: 0x06001B86 RID: 7046 RVA: 0x000B2D48 File Offset: 0x000B0F48
		[Nullable(2)]
		public Control ShakeTarget
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000B2D54 File Offset: 0x000B0F54
		public override void _Ready()
		{
			this._traumaRumble = new ScreenTraumaRumble();
			this._strength.Add(ShakeStrength.VeryWeak, 2f);
			this._strength.Add(ShakeStrength.Weak, 5f);
			this._strength.Add(ShakeStrength.Medium, 20f);
			this._strength.Add(ShakeStrength.Strong, 40f);
			this._strength.Add(ShakeStrength.TooMuch, 80f);
			this._duration.Add(ShakeDuration.Short, 0.3);
			this._duration.Add(ShakeDuration.Normal, 0.8);
			this._duration.Add(ShakeDuration.Long, 1.2);
			this._duration.Add(ShakeDuration.Forever, 999999999.0);
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000B2E15 File Offset: 0x000B1015
		[NullableContext(1)]
		public void SetTarget(Control targetScreen)
		{
			this.ShakeTarget = targetScreen;
			this._originalTargetPosition = targetScreen.Position;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000B2E2C File Offset: 0x000B102C
		public override void _Process(double delta)
		{
			Vector2 vector = Vector2.Zero;
			if (this._rumbleInstance != null)
			{
				vector = this._rumbleInstance.Update(delta);
				if (this._rumbleInstance.IsDone)
				{
					this._rumbleInstance = null;
				}
			}
			if (this._shakeInstance != null)
			{
				vector = this._shakeInstance.Update(delta);
				if (this._shakeInstance.IsDone)
				{
					this._shakeInstance = null;
				}
			}
			vector += this._traumaRumble.Update(delta);
			if (this.ShakeTarget != null && this.ShakeTarget.IsValid())
			{
				this.ShakeTarget.Position = this._originalTargetPosition + vector;
			}
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000B2ED0 File Offset: 0x000B10D0
		public void Shake(ShakeStrength strength, ShakeDuration duration, float degAngle)
		{
			if (this.ShakeTarget == null)
			{
				Log.Error("Missing screenShake target!", 2);
				return;
			}
			this._shakeInstance = new ScreenPunchInstance(this._strength[strength] * this._multiplier, this._duration[duration], degAngle);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000B2F1C File Offset: 0x000B111C
		public void Rumble(ShakeStrength strength, ShakeDuration duration, RumbleStyle style)
		{
			if (this.ShakeTarget == null)
			{
				Log.Error("Missing screenShake target!", 2);
				return;
			}
			this._rumbleInstance = new ScreenRumbleInstance(this._strength[strength] * this._multiplier, this._duration[duration], 1f, style);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000B2F6D File Offset: 0x000B116D
		public void AddTrauma(ShakeStrength strength)
		{
			this._traumaRumble.AddTrauma(strength);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000B2F7B File Offset: 0x000B117B
		public void ClearTarget()
		{
			this.ShakeTarget = null;
			this._shakeInstance = null;
			this._rumbleInstance = null;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000B2F92 File Offset: 0x000B1192
		private void StopRumble()
		{
			this._rumbleInstance = null;
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000B2F9B File Offset: 0x000B119B
		public void SetMultiplier(float multiplier)
		{
			this._multiplier = multiplier;
			this._traumaRumble.SetMultiplier(multiplier);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B90 RID: 7056 RVA: 0x000B2FB0 File Offset: 0x000B11B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NScreenShake.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenShake.MethodName.SetTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "targetScreen", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NScreenShake.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreenShake.MethodName.Shake, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false),
					new PropertyInfo(3L, "degAngle", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreenShake.MethodName.Rumble, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false),
					new PropertyInfo(2L, "duration", 0L, "", 6L, false),
					new PropertyInfo(2L, "style", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreenShake.MethodName.AddTrauma, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "strength", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreenShake.MethodName.ClearTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenShake.MethodName.StopRumble, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenShake.MethodName.SetMultiplier, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "multiplier", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B91 RID: 7057 RVA: 0x000B32DC File Offset: 0x000B14DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreenShake.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.SetTarget && args.Count == 1)
			{
				this.SetTarget(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.Shake && args.Count == 3)
			{
				this.Shake(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]), VariantUtils.ConvertTo<float>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.Rumble && args.Count == 3)
			{
				this.Rumble(VariantUtils.ConvertTo<ShakeStrength>(args[0]), VariantUtils.ConvertTo<ShakeDuration>(args[1]), VariantUtils.ConvertTo<RumbleStyle>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.AddTrauma && args.Count == 1)
			{
				this.AddTrauma(VariantUtils.ConvertTo<ShakeStrength>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.ClearTarget && args.Count == 0)
			{
				this.ClearTarget();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.StopRumble && args.Count == 0)
			{
				this.StopRumble();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenShake.MethodName.SetMultiplier && args.Count == 1)
			{
				this.SetMultiplier(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001B92 RID: 7058 RVA: 0x000B34C8 File Offset: 0x000B16C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScreenShake.MethodName._Ready || (ref method) == NScreenShake.MethodName.SetTarget || (ref method) == NScreenShake.MethodName._Process || (ref method) == NScreenShake.MethodName.Shake || (ref method) == NScreenShake.MethodName.Rumble || (ref method) == NScreenShake.MethodName.AddTrauma || (ref method) == NScreenShake.MethodName.ClearTarget || (ref method) == NScreenShake.MethodName.StopRumble || (ref method) == NScreenShake.MethodName.SetMultiplier || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001B93 RID: 7059 RVA: 0x000B3564 File Offset: 0x000B1764
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NScreenShake.PropertyName.ShakeTarget)
			{
				this.ShakeTarget = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NScreenShake.PropertyName._originalTargetPosition)
			{
				this._originalTargetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NScreenShake.PropertyName._multiplier)
			{
				this._multiplier = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001B94 RID: 7060 RVA: 0x000B35CC File Offset: 0x000B17CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NScreenShake.PropertyName.ShakeTarget)
			{
				Control shakeTarget = this.ShakeTarget;
				value = VariantUtils.CreateFrom<Control>(ref shakeTarget);
				return true;
			}
			if ((ref name) == NScreenShake.PropertyName._originalTargetPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalTargetPosition);
				return true;
			}
			if ((ref name) == NScreenShake.PropertyName._multiplier)
			{
				value = VariantUtils.CreateFrom<float>(ref this._multiplier);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001B95 RID: 7061 RVA: 0x000B3644 File Offset: 0x000B1844
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NScreenShake.PropertyName._originalTargetPosition, 0L, "", 4096L, false),
				new PropertyInfo(3L, NScreenShake.PropertyName._multiplier, 0L, "", 4096L, false),
				new PropertyInfo(24L, NScreenShake.PropertyName.ShakeTarget, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001B96 RID: 7062 RVA: 0x000B36BC File Offset: 0x000B18BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName shakeTarget = NScreenShake.PropertyName.ShakeTarget;
			Control shakeTarget2 = this.ShakeTarget;
			info.AddProperty(shakeTarget, Variant.From<Control>(ref shakeTarget2));
			info.AddProperty(NScreenShake.PropertyName._originalTargetPosition, Variant.From<Vector2>(ref this._originalTargetPosition));
			info.AddProperty(NScreenShake.PropertyName._multiplier, Variant.From<float>(ref this._multiplier));
		}

		/// <inheritdoc />
		// Token: 0x06001B97 RID: 7063 RVA: 0x000B3718 File Offset: 0x000B1918
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NScreenShake.PropertyName.ShakeTarget, ref variant))
			{
				this.ShakeTarget = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NScreenShake.PropertyName._originalTargetPosition, ref variant2))
			{
				this._originalTargetPosition = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NScreenShake.PropertyName._multiplier, ref variant3))
			{
				this._multiplier = variant3.As<float>();
			}
		}

		// Token: 0x04000A65 RID: 2661
		private Vector2 _originalTargetPosition;

		// Token: 0x04000A66 RID: 2662
		[Nullable(2)]
		private ScreenPunchInstance _shakeInstance;

		// Token: 0x04000A67 RID: 2663
		[Nullable(2)]
		private ScreenRumbleInstance _rumbleInstance;

		// Token: 0x04000A68 RID: 2664
		[Nullable(1)]
		private ScreenTraumaRumble _traumaRumble;

		// Token: 0x04000A69 RID: 2665
		private float _multiplier;

		// Token: 0x04000A6A RID: 2666
		[Nullable(1)]
		private readonly Dictionary<ShakeStrength, float> _strength = new Dictionary<ShakeStrength, float>();

		// Token: 0x04000A6B RID: 2667
		[Nullable(1)]
		private readonly Dictionary<ShakeDuration, double> _duration = new Dictionary<ShakeDuration, double>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B6 RID: 4534
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040044FB RID: 17659
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetTarget' method.
			/// </summary>
			// Token: 0x040044FC RID: 17660
			public static readonly StringName SetTarget = "SetTarget";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040044FD RID: 17661
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'Shake' method.
			/// </summary>
			// Token: 0x040044FE RID: 17662
			public static readonly StringName Shake = "Shake";

			/// <summary>
			/// Cached name for the 'Rumble' method.
			/// </summary>
			// Token: 0x040044FF RID: 17663
			public static readonly StringName Rumble = "Rumble";

			/// <summary>
			/// Cached name for the 'AddTrauma' method.
			/// </summary>
			// Token: 0x04004500 RID: 17664
			public static readonly StringName AddTrauma = "AddTrauma";

			/// <summary>
			/// Cached name for the 'ClearTarget' method.
			/// </summary>
			// Token: 0x04004501 RID: 17665
			public static readonly StringName ClearTarget = "ClearTarget";

			/// <summary>
			/// Cached name for the 'StopRumble' method.
			/// </summary>
			// Token: 0x04004502 RID: 17666
			public static readonly StringName StopRumble = "StopRumble";

			/// <summary>
			/// Cached name for the 'SetMultiplier' method.
			/// </summary>
			// Token: 0x04004503 RID: 17667
			public static readonly StringName SetMultiplier = "SetMultiplier";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B7 RID: 4535
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ShakeTarget' property.
			/// </summary>
			// Token: 0x04004504 RID: 17668
			public static readonly StringName ShakeTarget = "ShakeTarget";

			/// <summary>
			/// Cached name for the '_originalTargetPosition' field.
			/// </summary>
			// Token: 0x04004505 RID: 17669
			public static readonly StringName _originalTargetPosition = "_originalTargetPosition";

			/// <summary>
			/// Cached name for the '_multiplier' field.
			/// </summary>
			// Token: 0x04004506 RID: 17670
			public static readonly StringName _multiplier = "_multiplier";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011B8 RID: 4536
		public class SignalName : Node.SignalName
		{
		}
	}
}
