using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

// Token: 0x02000003 RID: 3
[ScriptPath("res://src/Core/Nodes/Debug/NOrbVfxTester.cs")]
public class NOrbVfxTester : Control
{
	// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00000364
	public override void _Ready()
	{
		this._passiveVal = (decimal)this._basePassiveValue;
		this._evokeVal = (decimal)this._baseEvokeValue;
		this._orbVfx.SetOverrideCombatVfxContainer(this._combatVfxContainer);
		this._orbVfx.SetOverridePlayerNode(this._playerCenter);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000021B8 File Offset: 0x000003B8
	[NullableContext(1)]
	public override void _Input(InputEvent inputEvent)
	{
		if (!base.Visible)
		{
			return;
		}
		base._Input(inputEvent);
		InputEventKey inputEventKey = inputEvent as InputEventKey;
		if (inputEventKey != null && inputEventKey.Keycode == 65L && inputEventKey.Pressed)
		{
			if (this._testModelType == NOrbVfxTester.OrbVfxTestModelType.Dark)
			{
				this._evokeVal += this._passiveVal;
			}
			this._orbVfx.OnPassiveActivated(this._passiveVal, this._evokeVal);
			if (this._orbVfx is NGlassOrbVfx && this._passiveVal > 0m)
			{
				(this._orbVfx as NGlassOrbVfx).ShowPassiveImpact(new Vector2[] { this._target.GlobalPosition });
			}
			if (this._testModelType == NOrbVfxTester.OrbVfxTestModelType.Glass)
			{
				this._passiveVal = Math.Clamp(this._passiveVal - 1m, 0m, (decimal)this._basePassiveValue);
			}
			this._orbVfx.AfterPassiveActivated(this._passiveVal, this._evokeVal);
		}
		inputEventKey = inputEvent as InputEventKey;
		if (inputEventKey != null && inputEventKey.Keycode == 90L && inputEventKey.Pressed)
		{
			this._passiveVal = (decimal)this._basePassiveValue;
			this._evokeVal = (decimal)this._baseEvokeValue;
			this._orbVfx.OnPassiveActivated(0m, this._evokeVal);
			this._orbVfx.Modulate = new Color(1f, 1f, 1f, 1f);
		}
		inputEventKey = inputEvent as InputEventKey;
		if (inputEventKey != null && inputEventKey.Keycode == 83L && inputEventKey.Pressed)
		{
			this._isFocused = !this._isFocused;
			this._orbVfx.SetForcedFocusPower(this._isFocused);
		}
		inputEventKey = inputEvent as InputEventKey;
		if (inputEventKey != null && inputEventKey.Keycode == 68L && inputEventKey.Pressed)
		{
			this._orbVfx.OnEvoke(new Vector2[] { (this._testModelType == NOrbVfxTester.OrbVfxTestModelType.Frost || this._testModelType == NOrbVfxTester.OrbVfxTestModelType.Plasma) ? this._playerCenter.Position : this._target.Position });
			this._orbVfx.Modulate = new Color(1f, 1f, 1f, 0f);
		}
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000009 RID: 9 RVA: 0x00002400 File Offset: 0x00000600
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(NOrbVfxTester.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NOrbVfxTester.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
			}, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600000A RID: 10 RVA: 0x000024A4 File Offset: 0x000006A4
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NOrbVfxTester.MethodName._Ready && args.Count == 0)
		{
			this._Ready();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NOrbVfxTester.MethodName._Input && args.Count == 1)
		{
			this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(ref method, args, ref ret);
	}

	/// <inheritdoc />
	// Token: 0x0600000B RID: 11 RVA: 0x00002512 File Offset: 0x00000712
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NOrbVfxTester.MethodName._Ready || (ref method) == NOrbVfxTester.MethodName._Input || base.HasGodotClassMethod(ref method);
	}

	/// <inheritdoc />
	// Token: 0x0600000C RID: 12 RVA: 0x0000253C File Offset: 0x0000073C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NOrbVfxTester.PropertyName._testModelType)
		{
			this._testModelType = VariantUtils.ConvertTo<NOrbVfxTester.OrbVfxTestModelType>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._orbVfx)
		{
			this._orbVfx = VariantUtils.ConvertTo<NOrbVfx>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._basePassiveValue)
		{
			this._basePassiveValue = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._baseEvokeValue)
		{
			this._baseEvokeValue = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._passiveIncrements)
		{
			this._passiveIncrements = VariantUtils.ConvertTo<float>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._playerCenter)
		{
			this._playerCenter = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._target)
		{
			this._target = VariantUtils.ConvertTo<Node2D>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._combatVfxContainer)
		{
			this._combatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._isFocused)
		{
			this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(ref name, ref value);
	}

	/// <inheritdoc />
	// Token: 0x0600000D RID: 13 RVA: 0x00002644 File Offset: 0x00000844
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NOrbVfxTester.PropertyName._testModelType)
		{
			value = VariantUtils.CreateFrom<NOrbVfxTester.OrbVfxTestModelType>(ref this._testModelType);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._orbVfx)
		{
			value = VariantUtils.CreateFrom<NOrbVfx>(ref this._orbVfx);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._basePassiveValue)
		{
			value = VariantUtils.CreateFrom<float>(ref this._basePassiveValue);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._baseEvokeValue)
		{
			value = VariantUtils.CreateFrom<float>(ref this._baseEvokeValue);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._passiveIncrements)
		{
			value = VariantUtils.CreateFrom<float>(ref this._passiveIncrements);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._playerCenter)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._playerCenter);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._target)
		{
			value = VariantUtils.CreateFrom<Node2D>(ref this._target);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._combatVfxContainer)
		{
			value = VariantUtils.CreateFrom<Control>(ref this._combatVfxContainer);
			return true;
		}
		if ((ref name) == NOrbVfxTester.PropertyName._isFocused)
		{
			value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
			return true;
		}
		return base.GetGodotClassPropertyValue(ref name, ref value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x0600000E RID: 14 RVA: 0x0000277C File Offset: 0x0000097C
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(2L, NOrbVfxTester.PropertyName._testModelType, 2L, "Lightning,Dark,Frost,Glass,Plasma", 4102L, true),
			new PropertyInfo(24L, NOrbVfxTester.PropertyName._orbVfx, 34L, "Node2D", 4102L, true),
			new PropertyInfo(3L, NOrbVfxTester.PropertyName._basePassiveValue, 0L, "", 4102L, true),
			new PropertyInfo(3L, NOrbVfxTester.PropertyName._baseEvokeValue, 0L, "", 4102L, true),
			new PropertyInfo(3L, NOrbVfxTester.PropertyName._passiveIncrements, 0L, "", 4102L, true),
			new PropertyInfo(24L, NOrbVfxTester.PropertyName._playerCenter, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NOrbVfxTester.PropertyName._target, 34L, "Node2D", 4102L, true),
			new PropertyInfo(24L, NOrbVfxTester.PropertyName._combatVfxContainer, 34L, "Control", 4102L, true),
			new PropertyInfo(1L, NOrbVfxTester.PropertyName._isFocused, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x0600000F RID: 15 RVA: 0x000028B8 File Offset: 0x00000AB8
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NOrbVfxTester.PropertyName._testModelType, Variant.From<NOrbVfxTester.OrbVfxTestModelType>(ref this._testModelType));
		info.AddProperty(NOrbVfxTester.PropertyName._orbVfx, Variant.From<NOrbVfx>(ref this._orbVfx));
		info.AddProperty(NOrbVfxTester.PropertyName._basePassiveValue, Variant.From<float>(ref this._basePassiveValue));
		info.AddProperty(NOrbVfxTester.PropertyName._baseEvokeValue, Variant.From<float>(ref this._baseEvokeValue));
		info.AddProperty(NOrbVfxTester.PropertyName._passiveIncrements, Variant.From<float>(ref this._passiveIncrements));
		info.AddProperty(NOrbVfxTester.PropertyName._playerCenter, Variant.From<Node2D>(ref this._playerCenter));
		info.AddProperty(NOrbVfxTester.PropertyName._target, Variant.From<Node2D>(ref this._target));
		info.AddProperty(NOrbVfxTester.PropertyName._combatVfxContainer, Variant.From<Control>(ref this._combatVfxContainer));
		info.AddProperty(NOrbVfxTester.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
	}

	/// <inheritdoc />
	// Token: 0x06000010 RID: 16 RVA: 0x00002994 File Offset: 0x00000B94
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._testModelType, ref variant))
		{
			this._testModelType = variant.As<NOrbVfxTester.OrbVfxTestModelType>();
		}
		Variant variant2;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._orbVfx, ref variant2))
		{
			this._orbVfx = variant2.As<NOrbVfx>();
		}
		Variant variant3;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._basePassiveValue, ref variant3))
		{
			this._basePassiveValue = variant3.As<float>();
		}
		Variant variant4;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._baseEvokeValue, ref variant4))
		{
			this._baseEvokeValue = variant4.As<float>();
		}
		Variant variant5;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._passiveIncrements, ref variant5))
		{
			this._passiveIncrements = variant5.As<float>();
		}
		Variant variant6;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._playerCenter, ref variant6))
		{
			this._playerCenter = variant6.As<Node2D>();
		}
		Variant variant7;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._target, ref variant7))
		{
			this._target = variant7.As<Node2D>();
		}
		Variant variant8;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._combatVfxContainer, ref variant8))
		{
			this._combatVfxContainer = variant8.As<Control>();
		}
		Variant variant9;
		if (info.TryGetProperty(NOrbVfxTester.PropertyName._isFocused, ref variant9))
		{
			this._isFocused = variant9.As<bool>();
		}
	}

	// Token: 0x04000003 RID: 3
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NOrbVfxTester.OrbVfxTestModelType _testModelType;

	// Token: 0x04000004 RID: 4
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private NOrbVfx _orbVfx;

	// Token: 0x04000005 RID: 5
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _basePassiveValue = 4f;

	// Token: 0x04000006 RID: 6
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _baseEvokeValue = 4f;

	// Token: 0x04000007 RID: 7
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private float _passiveIncrements;

	// Token: 0x04000008 RID: 8
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Node2D _playerCenter;

	// Token: 0x04000009 RID: 9
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Node2D _target;

	// Token: 0x0400000A RID: 10
	[Nullable(1)]
	[Export(/*Failed to decode CustomAttribute blob!*/)]
	private Control _combatVfxContainer;

	// Token: 0x0400000B RID: 11
	private bool _isFocused;

	// Token: 0x0400000C RID: 12
	private decimal _passiveVal;

	// Token: 0x0400000D RID: 13
	private decimal _evokeVal;

	// Token: 0x02000DAA RID: 3498
	private enum OrbVfxTestModelType
	{
		// Token: 0x04003143 RID: 12611
		Lightning,
		// Token: 0x04003144 RID: 12612
		Dark,
		// Token: 0x04003145 RID: 12613
		Frost,
		// Token: 0x04003146 RID: 12614
		Glass,
		// Token: 0x04003147 RID: 12615
		Plasma
	}

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DAB RID: 3499
	public class MethodName : Control.MethodName
	{
		/// <summary>
		/// Cached name for the '_Ready' method.
		/// </summary>
		// Token: 0x04003148 RID: 12616
		public static readonly StringName _Ready = "_Ready";

		/// <summary>
		/// Cached name for the '_Input' method.
		/// </summary>
		// Token: 0x04003149 RID: 12617
		public static readonly StringName _Input = "_Input";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DAC RID: 3500
	public class PropertyName : Control.PropertyName
	{
		/// <summary>
		/// Cached name for the '_testModelType' field.
		/// </summary>
		// Token: 0x0400314A RID: 12618
		public static readonly StringName _testModelType = "_testModelType";

		/// <summary>
		/// Cached name for the '_orbVfx' field.
		/// </summary>
		// Token: 0x0400314B RID: 12619
		public static readonly StringName _orbVfx = "_orbVfx";

		/// <summary>
		/// Cached name for the '_basePassiveValue' field.
		/// </summary>
		// Token: 0x0400314C RID: 12620
		public static readonly StringName _basePassiveValue = "_basePassiveValue";

		/// <summary>
		/// Cached name for the '_baseEvokeValue' field.
		/// </summary>
		// Token: 0x0400314D RID: 12621
		public static readonly StringName _baseEvokeValue = "_baseEvokeValue";

		/// <summary>
		/// Cached name for the '_passiveIncrements' field.
		/// </summary>
		// Token: 0x0400314E RID: 12622
		public static readonly StringName _passiveIncrements = "_passiveIncrements";

		/// <summary>
		/// Cached name for the '_playerCenter' field.
		/// </summary>
		// Token: 0x0400314F RID: 12623
		public static readonly StringName _playerCenter = "_playerCenter";

		/// <summary>
		/// Cached name for the '_target' field.
		/// </summary>
		// Token: 0x04003150 RID: 12624
		public static readonly StringName _target = "_target";

		/// <summary>
		/// Cached name for the '_combatVfxContainer' field.
		/// </summary>
		// Token: 0x04003151 RID: 12625
		public static readonly StringName _combatVfxContainer = "_combatVfxContainer";

		/// <summary>
		/// Cached name for the '_isFocused' field.
		/// </summary>
		// Token: 0x04003152 RID: 12626
		public static readonly StringName _isFocused = "_isFocused";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DAD RID: 3501
	public class SignalName : Control.SignalName
	{
	}
}
