using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

// Token: 0x0200000B RID: 11
[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NShaker.cs")]
public class NShaker : Node
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00005524 File Offset: 0x00003724
	// (set) Token: 0x0600007A RID: 122 RVA: 0x0000552C File Offset: 0x0000372C
	public float Strength
	{
		get
		{
			return this._strength;
		}
		set
		{
			this._strength = value;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00005535 File Offset: 0x00003735
	public override void _Ready()
	{
		base._Ready();
		this._timer = 0f;
		this._previousShakePos = Vector2.Zero;
		this._previousShakeRot = 0f;
		this._shakePos = Vector2.Zero;
		this._shakeRot = 0f;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00005574 File Offset: 0x00003774
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (this._target == null)
		{
			return;
		}
		if (this._strength == 0f)
		{
			this._target.Position = Vector2.Zero;
			this._target.RotationDegrees = 0f;
			return;
		}
		if (this._timer > 0f)
		{
			this._timer = Mathf.Clamp(this._timer - (float)delta, 0f, this._frequency);
			float num = 1f - this._timer / this._frequency;
			Vector2 vector = this._previousShakePos.Lerp(this._shakePos, num);
			float num2 = Mathf.Lerp(this._previousShakeRot, this._shakeRot, num);
			this.SetTargetTransform(vector, num2);
			return;
		}
		this._timer = this._frequency;
		this._previousShakePos = this._shakePos;
		this._previousShakeRot = this._shakeRot;
		float num3 = Mathf.DegToRad(GD.Randf() * 360f);
		float num4 = GD.Randf() * this._maxPosOffset;
		Vector2 vector2 = new Vector2(Mathf.Cos(num3), Mathf.Sin(num3)).Normalized();
		this._shakePos = vector2 * num4 * this._strength;
		this._shakeRot = (float)GD.RandRange((double)(-(double)this._maxRotOffset), (double)this._maxRotOffset);
		this.SetTargetTransform(this._shakePos, this._shakeRot);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000056D4 File Offset: 0x000038D4
	private void SetTargetTransform(Vector2 position, float rotation)
	{
		if (this._target == null)
		{
			return;
		}
		this._target.Position = position;
		this._target.RotationDegrees = rotation;
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600007E RID: 126 RVA: 0x000056F8 File Offset: 0x000038F8
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(NShaker.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NShaker.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(3L, "delta", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NShaker.MethodName.SetTargetTransform, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(5L, "position", 0L, "", 6L, false),
				new PropertyInfo(3L, "rotation", 0L, "", 6L, false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600007F RID: 127 RVA: 0x00005808 File Offset: 0x00003A08
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NShaker.MethodName._Ready && args.Count == 0)
		{
			this._Ready();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NShaker.MethodName._Process && args.Count == 1)
		{
			this._Process(VariantUtils.ConvertTo<double>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NShaker.MethodName.SetTargetTransform && args.Count == 2)
		{
			this.SetTargetTransform(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<float>(args[1]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(ref method, args, ref ret);
	}

	/// <inheritdoc />
	// Token: 0x06000080 RID: 128 RVA: 0x000058B6 File Offset: 0x00003AB6
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NShaker.MethodName._Ready || (ref method) == NShaker.MethodName._Process || (ref method) == NShaker.MethodName.SetTargetTransform || base.HasGodotClassMethod(ref method);
	}

	/// <inheritdoc />
	// Token: 0x06000081 RID: 129 RVA: 0x000058EC File Offset: 0x00003AEC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NShaker.PropertyName.Strength)
		{
			this.Strength = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._target)
		{
			this._target = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._maxPosOffset)
		{
			this._maxPosOffset = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._maxRotOffset)
		{
			this._maxRotOffset = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._frequency)
		{
			this._frequency = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._strength)
		{
			this._strength = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._timer)
		{
			this._timer = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._previousShakePos)
		{
			this._previousShakePos = VariantUtils.ConvertTo<Vector2>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._previousShakeRot)
		{
			this._previousShakeRot = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._shakePos)
		{
			this._shakePos = VariantUtils.ConvertTo<Vector2>(ref value);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._shakeRot)
		{
			this._shakeRot = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(ref name, ref value);
	}

	/// <inheritdoc />
	// Token: 0x06000082 RID: 130 RVA: 0x00005A2C File Offset: 0x00003C2C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NShaker.PropertyName.Strength)
		{
			float strength = this.Strength;
			value = VariantUtils.CreateFrom<float>(ref strength);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._target)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._target);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._maxPosOffset)
		{
			value = VariantUtils.CreateFrom<float>(ref this._maxPosOffset);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._maxRotOffset)
		{
			value = VariantUtils.CreateFrom<float>(ref this._maxRotOffset);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._frequency)
		{
			value = VariantUtils.CreateFrom<float>(ref this._frequency);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._strength)
		{
			value = VariantUtils.CreateFrom<float>(ref this._strength);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._timer)
		{
			value = VariantUtils.CreateFrom<float>(ref this._timer);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._previousShakePos)
		{
			value = VariantUtils.CreateFrom<Vector2>(ref this._previousShakePos);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._previousShakeRot)
		{
			value = VariantUtils.CreateFrom<float>(ref this._previousShakeRot);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._shakePos)
		{
			value = VariantUtils.CreateFrom<Vector2>(ref this._shakePos);
			return true;
		}
		if ((ref name) == NShaker.PropertyName._shakeRot)
		{
			value = VariantUtils.CreateFrom<float>(ref this._shakeRot);
			return true;
		}
		return base.GetGodotClassPropertyValue(ref name, ref value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000083 RID: 131 RVA: 0x00005BA4 File Offset: 0x00003DA4
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NShaker.PropertyName._target, 34L, "Node2D", 4102L, true),
			new PropertyInfo(3L, NShaker.PropertyName._maxPosOffset, 0L, "", 4102L, true),
			new PropertyInfo(3L, NShaker.PropertyName._maxRotOffset, 0L, "", 4102L, true),
			new PropertyInfo(3L, NShaker.PropertyName._frequency, 0L, "", 4102L, true),
			new PropertyInfo(3L, NShaker.PropertyName._strength, 0L, "", 4102L, true),
			new PropertyInfo(3L, NShaker.PropertyName._timer, 0L, "", 4096L, false),
			new PropertyInfo(5L, NShaker.PropertyName._previousShakePos, 0L, "", 4096L, false),
			new PropertyInfo(3L, NShaker.PropertyName._previousShakeRot, 0L, "", 4096L, false),
			new PropertyInfo(5L, NShaker.PropertyName._shakePos, 0L, "", 4096L, false),
			new PropertyInfo(3L, NShaker.PropertyName._shakeRot, 0L, "", 4096L, false),
			new PropertyInfo(3L, NShaker.PropertyName.Strength, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000084 RID: 132 RVA: 0x00005D1C File Offset: 0x00003F1C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		StringName strength = NShaker.PropertyName.Strength;
		float strength2 = this.Strength;
		info.AddProperty(strength, Variant.From<float>(ref strength2));
		info.AddProperty(NShaker.PropertyName._target, Variant.From<Node2D>(ref this._target));
		info.AddProperty(NShaker.PropertyName._maxPosOffset, Variant.From<float>(ref this._maxPosOffset));
		info.AddProperty(NShaker.PropertyName._maxRotOffset, Variant.From<float>(ref this._maxRotOffset));
		info.AddProperty(NShaker.PropertyName._frequency, Variant.From<float>(ref this._frequency));
		info.AddProperty(NShaker.PropertyName._strength, Variant.From<float>(ref this._strength));
		info.AddProperty(NShaker.PropertyName._timer, Variant.From<float>(ref this._timer));
		info.AddProperty(NShaker.PropertyName._previousShakePos, Variant.From<Vector2>(ref this._previousShakePos));
		info.AddProperty(NShaker.PropertyName._previousShakeRot, Variant.From<float>(ref this._previousShakeRot));
		info.AddProperty(NShaker.PropertyName._shakePos, Variant.From<Vector2>(ref this._shakePos));
		info.AddProperty(NShaker.PropertyName._shakeRot, Variant.From<float>(ref this._shakeRot));
	}

	/// <inheritdoc />
	// Token: 0x06000085 RID: 133 RVA: 0x00005E28 File Offset: 0x00004028
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NShaker.PropertyName.Strength, ref variant))
		{
			this.Strength = variant.As<float>();
		}
		Variant variant2;
		if (info.TryGetProperty(NShaker.PropertyName._target, ref variant2))
		{
			this._target = variant2.As<Node2D>();
		}
		Variant variant3;
		if (info.TryGetProperty(NShaker.PropertyName._maxPosOffset, ref variant3))
		{
			this._maxPosOffset = variant3.As<float>();
		}
		Variant variant4;
		if (info.TryGetProperty(NShaker.PropertyName._maxRotOffset, ref variant4))
		{
			this._maxRotOffset = variant4.As<float>();
		}
		Variant variant5;
		if (info.TryGetProperty(NShaker.PropertyName._frequency, ref variant5))
		{
			this._frequency = variant5.As<float>();
		}
		Variant variant6;
		if (info.TryGetProperty(NShaker.PropertyName._strength, ref variant6))
		{
			this._strength = variant6.As<float>();
		}
		Variant variant7;
		if (info.TryGetProperty(NShaker.PropertyName._timer, ref variant7))
		{
			this._timer = variant7.As<float>();
		}
		Variant variant8;
		if (info.TryGetProperty(NShaker.PropertyName._previousShakePos, ref variant8))
		{
			this._previousShakePos = variant8.As<Vector2>();
		}
		Variant variant9;
		if (info.TryGetProperty(NShaker.PropertyName._previousShakeRot, ref variant9))
		{
			this._previousShakeRot = variant9.As<float>();
		}
		Variant variant10;
		if (info.TryGetProperty(NShaker.PropertyName._shakePos, ref variant10))
		{
			this._shakePos = variant10.As<Vector2>();
		}
		Variant variant11;
		if (info.TryGetProperty(NShaker.PropertyName._shakeRot, ref variant11))
		{
			this._shakeRot = variant11.As<float>();
		}
	}

	// Token: 0x0400002A RID: 42
	[Nullable(2)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Node2D _target;

	// Token: 0x0400002B RID: 43
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _maxPosOffset;

	// Token: 0x0400002C RID: 44
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _maxRotOffset;

	// Token: 0x0400002D RID: 45
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _frequency;

	// Token: 0x0400002E RID: 46
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _strength;

	// Token: 0x0400002F RID: 47
	private float _timer;

	// Token: 0x04000030 RID: 48
	private Vector2 _previousShakePos;

	// Token: 0x04000031 RID: 49
	private float _previousShakeRot;

	// Token: 0x04000032 RID: 50
	private Vector2 _shakePos;

	// Token: 0x04000033 RID: 51
	private float _shakeRot;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC5 RID: 3525
	public class MethodName : Node.MethodName
	{
		/// <summary>
		/// Cached name for the '_Ready' method.
		/// </summary>
		// Token: 0x04003191 RID: 12689
		public static readonly StringName _Ready = "_Ready";

		/// <summary>
		/// Cached name for the '_Process' method.
		/// </summary>
		// Token: 0x04003192 RID: 12690
		public static readonly StringName _Process = "_Process";

		/// <summary>
		/// Cached name for the 'SetTargetTransform' method.
		/// </summary>
		// Token: 0x04003193 RID: 12691
		public static readonly StringName SetTargetTransform = "SetTargetTransform";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC6 RID: 3526
	public class PropertyName : Node.PropertyName
	{
		/// <summary>
		/// Cached name for the 'Strength' property.
		/// </summary>
		// Token: 0x04003194 RID: 12692
		public static readonly StringName Strength = "Strength";

		/// <summary>
		/// Cached name for the '_target' field.
		/// </summary>
		// Token: 0x04003195 RID: 12693
		public static readonly StringName _target = "_target";

		/// <summary>
		/// Cached name for the '_maxPosOffset' field.
		/// </summary>
		// Token: 0x04003196 RID: 12694
		public static readonly StringName _maxPosOffset = "_maxPosOffset";

		/// <summary>
		/// Cached name for the '_maxRotOffset' field.
		/// </summary>
		// Token: 0x04003197 RID: 12695
		public static readonly StringName _maxRotOffset = "_maxRotOffset";

		/// <summary>
		/// Cached name for the '_frequency' field.
		/// </summary>
		// Token: 0x04003198 RID: 12696
		public static readonly StringName _frequency = "_frequency";

		/// <summary>
		/// Cached name for the '_strength' field.
		/// </summary>
		// Token: 0x04003199 RID: 12697
		public static readonly StringName _strength = "_strength";

		/// <summary>
		/// Cached name for the '_timer' field.
		/// </summary>
		// Token: 0x0400319A RID: 12698
		public static readonly StringName _timer = "_timer";

		/// <summary>
		/// Cached name for the '_previousShakePos' field.
		/// </summary>
		// Token: 0x0400319B RID: 12699
		public static readonly StringName _previousShakePos = "_previousShakePos";

		/// <summary>
		/// Cached name for the '_previousShakeRot' field.
		/// </summary>
		// Token: 0x0400319C RID: 12700
		public static readonly StringName _previousShakeRot = "_previousShakeRot";

		/// <summary>
		/// Cached name for the '_shakePos' field.
		/// </summary>
		// Token: 0x0400319D RID: 12701
		public static readonly StringName _shakePos = "_shakePos";

		/// <summary>
		/// Cached name for the '_shakeRot' field.
		/// </summary>
		// Token: 0x0400319E RID: 12702
		public static readonly StringName _shakeRot = "_shakeRot";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC7 RID: 3527
	public class SignalName : Node.SignalName
	{
	}
}
