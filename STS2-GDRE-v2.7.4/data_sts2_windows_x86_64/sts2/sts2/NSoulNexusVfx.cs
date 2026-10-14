using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Vfx;

// Token: 0x0200000A RID: 10
[ScriptPath("res://src/Core/Nodes/Vfx/NSoulNexusVfx.cs")]
public class NSoulNexusVfx : Node
{
	// Token: 0x06000067 RID: 103 RVA: 0x00004B18 File Offset: 0x00002D18
	public override void _Ready()
	{
		this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
		this._fireTexture = base.GetNode<TextureRect>("../HeadFireSlot/FireTexture");
		this._trail1 = base.GetNode<NBasicTrail>("../PathSlot1/Trail");
		this._trail2 = base.GetNode<NBasicTrail>("../PathSlot2/Trail");
		this._trail3 = base.GetNode<NBasicTrail>("../PathSlot3/Trail");
		this._trail1.Visible = false;
		this._trail2.Visible = false;
		this._trail3.Visible = false;
		this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004BD4 File Offset: 0x00002DD4
	[NullableContext(1)]
	private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
	{
		string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
		if (eventName != null)
		{
			switch (eventName.Length)
			{
			case 9:
			{
				char c = eventName[0];
				if (c != 'h')
				{
					if (c != 's')
					{
						return;
					}
					if (!(eventName == "show_fire"))
					{
						return;
					}
					this.ShowFire(true);
					return;
				}
				else
				{
					if (!(eventName == "hide_fire"))
					{
						return;
					}
					this.ShowFire(false);
					return;
				}
				break;
			}
			case 10:
				break;
			case 11:
				switch (eventName[5])
				{
				case '1':
					if (!(eventName == "path_1_stop"))
					{
						return;
					}
					this.EndPath1();
					return;
				case '2':
					if (!(eventName == "path_2_stop"))
					{
						return;
					}
					this.EndPath2();
					return;
				case '3':
					if (!(eventName == "path_3_stop"))
					{
						return;
					}
					this.EndPath3();
					break;
				default:
					return;
				}
				break;
			case 12:
				switch (eventName[5])
				{
				case '1':
					if (!(eventName == "path_1_start"))
					{
						return;
					}
					this.StartPath1();
					return;
				case '2':
					if (!(eventName == "path_2_start"))
					{
						return;
					}
					this.StartPath2();
					return;
				case '3':
					if (!(eventName == "path_3_start"))
					{
						return;
					}
					this.StartPath3();
					return;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004D17 File Offset: 0x00002F17
	private void ShowFire(bool show)
	{
		this._fireTexture.Visible = show;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00004D25 File Offset: 0x00002F25
	private void StartPath1()
	{
		this._trail1.Visible = true;
		this._trail1.ClearPoints();
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00004D3E File Offset: 0x00002F3E
	private void EndPath1()
	{
		this._trail1.Visible = false;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00004D4C File Offset: 0x00002F4C
	private void StartPath2()
	{
		this._trail2.Visible = true;
		this._trail2.ClearPoints();
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004D65 File Offset: 0x00002F65
	private void EndPath2()
	{
		this._trail2.Visible = false;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004D73 File Offset: 0x00002F73
	private void StartPath3()
	{
		this._trail3.Visible = true;
		this._trail3.ClearPoints();
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004D8C File Offset: 0x00002F8C
	private void EndPath3()
	{
		this._trail3.Visible = false;
	}

	/// <summary>
	/// Get the method information for all the methods declared in this class.
	/// This method is used by Godot to register the available methods in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000070 RID: 112 RVA: 0x00004D9C File Offset: 0x00002F9C
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(NSoulNexusVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
				new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
				new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
				new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
			}, null),
			new MethodInfo(NSoulNexusVfx.MethodName.ShowFire, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
			{
				new PropertyInfo(1L, "show", 0L, "", 6L, false)
			}, null),
			new MethodInfo(NSoulNexusVfx.MethodName.StartPath1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.EndPath1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.StartPath2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.EndPath2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.StartPath3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
			new MethodInfo(NSoulNexusVfx.MethodName.EndPath3, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000071 RID: 113 RVA: 0x00005034 File Offset: 0x00003234
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if ((ref method) == NSoulNexusVfx.MethodName._Ready && args.Count == 0)
		{
			this._Ready();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.OnAnimationEvent && args.Count == 4)
		{
			this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.ShowFire && args.Count == 1)
		{
			this.ShowFire(VariantUtils.ConvertTo<bool>(args[0]));
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.StartPath1 && args.Count == 0)
		{
			this.StartPath1();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.EndPath1 && args.Count == 0)
		{
			this.EndPath1();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.StartPath2 && args.Count == 0)
		{
			this.StartPath2();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.EndPath2 && args.Count == 0)
		{
			this.EndPath2();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.StartPath3 && args.Count == 0)
		{
			this.StartPath3();
			ret = default(godot_variant);
			return true;
		}
		if ((ref method) == NSoulNexusVfx.MethodName.EndPath3 && args.Count == 0)
		{
			this.EndPath3();
			ret = default(godot_variant);
			return true;
		}
		return base.InvokeGodotClassMethod(ref method, args, ref ret);
	}

	/// <inheritdoc />
	// Token: 0x06000072 RID: 114 RVA: 0x000051DC File Offset: 0x000033DC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		return (ref method) == NSoulNexusVfx.MethodName._Ready || (ref method) == NSoulNexusVfx.MethodName.OnAnimationEvent || (ref method) == NSoulNexusVfx.MethodName.ShowFire || (ref method) == NSoulNexusVfx.MethodName.StartPath1 || (ref method) == NSoulNexusVfx.MethodName.EndPath1 || (ref method) == NSoulNexusVfx.MethodName.StartPath2 || (ref method) == NSoulNexusVfx.MethodName.EndPath2 || (ref method) == NSoulNexusVfx.MethodName.StartPath3 || (ref method) == NSoulNexusVfx.MethodName.EndPath3 || base.HasGodotClassMethod(ref method);
	}

	/// <inheritdoc />
	// Token: 0x06000073 RID: 115 RVA: 0x00005278 File Offset: 0x00003478
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if ((ref name) == NSoulNexusVfx.PropertyName._trail1)
		{
			this._trail1 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._trail2)
		{
			this._trail2 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._trail3)
		{
			this._trail3 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._fireTexture)
		{
			this._fireTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
			return true;
		}
		return base.SetGodotClassPropertyValue(ref name, ref value);
	}

	/// <inheritdoc />
	// Token: 0x06000074 RID: 116 RVA: 0x000052FC File Offset: 0x000034FC
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if ((ref name) == NSoulNexusVfx.PropertyName._trail1)
		{
			value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail1);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._trail2)
		{
			value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail2);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._trail3)
		{
			value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail3);
			return true;
		}
		if ((ref name) == NSoulNexusVfx.PropertyName._fireTexture)
		{
			value = VariantUtils.CreateFrom<TextureRect>(ref this._fireTexture);
			return true;
		}
		return base.GetGodotClassPropertyValue(ref name, ref value);
	}

	/// <summary>
	/// Get the property information for all the properties declared in this class.
	/// This method is used by Godot to register the available properties in the editor.
	/// Do not call this method.
	/// </summary>
	// Token: 0x06000075 RID: 117 RVA: 0x00005394 File Offset: 0x00003594
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(24L, NSoulNexusVfx.PropertyName._trail1, 0L, "", 4096L, false),
			new PropertyInfo(24L, NSoulNexusVfx.PropertyName._trail2, 0L, "", 4096L, false),
			new PropertyInfo(24L, NSoulNexusVfx.PropertyName._trail3, 0L, "", 4096L, false),
			new PropertyInfo(24L, NSoulNexusVfx.PropertyName._fireTexture, 0L, "", 4096L, false)
		};
	}

	/// <inheritdoc />
	// Token: 0x06000076 RID: 118 RVA: 0x0000542C File Offset: 0x0000362C
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(NSoulNexusVfx.PropertyName._trail1, Variant.From<NBasicTrail>(ref this._trail1));
		info.AddProperty(NSoulNexusVfx.PropertyName._trail2, Variant.From<NBasicTrail>(ref this._trail2));
		info.AddProperty(NSoulNexusVfx.PropertyName._trail3, Variant.From<NBasicTrail>(ref this._trail3));
		info.AddProperty(NSoulNexusVfx.PropertyName._fireTexture, Variant.From<TextureRect>(ref this._fireTexture));
	}

	/// <inheritdoc />
	// Token: 0x06000077 RID: 119 RVA: 0x00005498 File Offset: 0x00003698
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		Variant variant;
		if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail1, ref variant))
		{
			this._trail1 = variant.As<NBasicTrail>();
		}
		Variant variant2;
		if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail2, ref variant2))
		{
			this._trail2 = variant2.As<NBasicTrail>();
		}
		Variant variant3;
		if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail3, ref variant3))
		{
			this._trail3 = variant3.As<NBasicTrail>();
		}
		Variant variant4;
		if (info.TryGetProperty(NSoulNexusVfx.PropertyName._fireTexture, ref variant4))
		{
			this._fireTexture = variant4.As<TextureRect>();
		}
	}

	// Token: 0x04000025 RID: 37
	[Nullable(1)]
	private MegaSprite _megaSprite;

	// Token: 0x04000026 RID: 38
	[Nullable(1)]
	private NBasicTrail _trail1;

	// Token: 0x04000027 RID: 39
	[Nullable(1)]
	private NBasicTrail _trail2;

	// Token: 0x04000028 RID: 40
	[Nullable(1)]
	private NBasicTrail _trail3;

	// Token: 0x04000029 RID: 41
	[Nullable(1)]
	private TextureRect _fireTexture;

	/// <summary>
	/// Cached StringNames for the methods contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC2 RID: 3522
	public class MethodName : Node.MethodName
	{
		/// <summary>
		/// Cached name for the '_Ready' method.
		/// </summary>
		// Token: 0x04003184 RID: 12676
		public static readonly StringName _Ready = "_Ready";

		/// <summary>
		/// Cached name for the 'OnAnimationEvent' method.
		/// </summary>
		// Token: 0x04003185 RID: 12677
		public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

		/// <summary>
		/// Cached name for the 'ShowFire' method.
		/// </summary>
		// Token: 0x04003186 RID: 12678
		public static readonly StringName ShowFire = "ShowFire";

		/// <summary>
		/// Cached name for the 'StartPath1' method.
		/// </summary>
		// Token: 0x04003187 RID: 12679
		public static readonly StringName StartPath1 = "StartPath1";

		/// <summary>
		/// Cached name for the 'EndPath1' method.
		/// </summary>
		// Token: 0x04003188 RID: 12680
		public static readonly StringName EndPath1 = "EndPath1";

		/// <summary>
		/// Cached name for the 'StartPath2' method.
		/// </summary>
		// Token: 0x04003189 RID: 12681
		public static readonly StringName StartPath2 = "StartPath2";

		/// <summary>
		/// Cached name for the 'EndPath2' method.
		/// </summary>
		// Token: 0x0400318A RID: 12682
		public static readonly StringName EndPath2 = "EndPath2";

		/// <summary>
		/// Cached name for the 'StartPath3' method.
		/// </summary>
		// Token: 0x0400318B RID: 12683
		public static readonly StringName StartPath3 = "StartPath3";

		/// <summary>
		/// Cached name for the 'EndPath3' method.
		/// </summary>
		// Token: 0x0400318C RID: 12684
		public static readonly StringName EndPath3 = "EndPath3";
	}

	/// <summary>
	/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC3 RID: 3523
	public class PropertyName : Node.PropertyName
	{
		/// <summary>
		/// Cached name for the '_trail1' field.
		/// </summary>
		// Token: 0x0400318D RID: 12685
		public static readonly StringName _trail1 = "_trail1";

		/// <summary>
		/// Cached name for the '_trail2' field.
		/// </summary>
		// Token: 0x0400318E RID: 12686
		public static readonly StringName _trail2 = "_trail2";

		/// <summary>
		/// Cached name for the '_trail3' field.
		/// </summary>
		// Token: 0x0400318F RID: 12687
		public static readonly StringName _trail3 = "_trail3";

		/// <summary>
		/// Cached name for the '_fireTexture' field.
		/// </summary>
		// Token: 0x04003190 RID: 12688
		public static readonly StringName _fireTexture = "_fireTexture";
	}

	/// <summary>
	/// Cached StringNames for the signals contained in this class, for fast lookup.
	/// </summary>
	// Token: 0x02000DC4 RID: 3524
	public class SignalName : Node.SignalName
	{
	}
}
