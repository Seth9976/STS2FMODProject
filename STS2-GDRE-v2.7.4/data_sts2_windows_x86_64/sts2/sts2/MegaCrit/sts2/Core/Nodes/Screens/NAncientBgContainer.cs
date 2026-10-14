using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	// Token: 0x02000248 RID: 584
	[ScriptPath("res://src/Core/Nodes/Screens/NAncientBgContainer.cs")]
	public class NAncientBgContainer : Control
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x000C2094 File Offset: 0x000C0294
		public override void _Ready()
		{
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
		}

		/// <summary>
		/// A great way to resize bg assets. Interpolates from 16:9 to other aspect ratios.
		/// TODO: Adopt for normal events??
		/// </summary>
		// Token: 0x06001DD4 RID: 7636 RVA: 0x000C20D0 File Offset: 0x000C02D0
		private void OnWindowChange()
		{
			float num = Mathf.Clamp(base.Size.X / base.Size.Y, 1.3333f, 2.3333f);
			base.PivotOffset = base.Size * 0.5f;
			if (num < 1.7777f)
			{
				float num2 = Mathf.InverseLerp(1.3333f, 1.7777f, num);
				base.Position = this._pos43.Lerp(this._pos169, num2);
				base.Scale = this._scale43.Lerp(this._scale169, num2);
				return;
			}
			float num3 = Mathf.InverseLerp(1.7777f, 2.3333f, num);
			base.Position = this._pos169.Lerp(this._pos219, num3);
			base.Scale = this._scale169.Lerp(this._scale219, num3);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DD5 RID: 7637 RVA: 0x000C21A8 File Offset: 0x000C03A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NAncientBgContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientBgContainer.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DD6 RID: 7638 RVA: 0x000C221C File Offset: 0x000C041C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientBgContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientBgContainer.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001DD7 RID: 7639 RVA: 0x000C227C File Offset: 0x000C047C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientBgContainer.MethodName._Ready || (ref method) == NAncientBgContainer.MethodName.OnWindowChange || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001DD8 RID: 7640 RVA: 0x000C22A4 File Offset: 0x000C04A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientBgContainer.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos43)
			{
				this._pos43 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale43)
			{
				this._scale43 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos169)
			{
				this._pos169 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale169)
			{
				this._scale169 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos219)
			{
				this._pos219 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale219)
			{
				this._scale219 = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001DD9 RID: 7641 RVA: 0x000C2378 File Offset: 0x000C0578
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientBgContainer.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos43)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._pos43);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale43)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._scale43);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos169)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._pos169);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale169)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._scale169);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._pos219)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._pos219);
				return true;
			}
			if ((ref name) == NAncientBgContainer.PropertyName._scale219)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._scale219);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DDA RID: 7642 RVA: 0x000C2470 File Offset: 0x000C0670
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAncientBgContainer.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._pos43, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._scale43, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._pos169, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._scale169, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._pos219, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAncientBgContainer.PropertyName._scale219, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DDB RID: 7643 RVA: 0x000C2568 File Offset: 0x000C0768
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientBgContainer.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NAncientBgContainer.PropertyName._pos43, Variant.From<Vector2>(ref this._pos43));
			info.AddProperty(NAncientBgContainer.PropertyName._scale43, Variant.From<Vector2>(ref this._scale43));
			info.AddProperty(NAncientBgContainer.PropertyName._pos169, Variant.From<Vector2>(ref this._pos169));
			info.AddProperty(NAncientBgContainer.PropertyName._scale169, Variant.From<Vector2>(ref this._scale169));
			info.AddProperty(NAncientBgContainer.PropertyName._pos219, Variant.From<Vector2>(ref this._pos219));
			info.AddProperty(NAncientBgContainer.PropertyName._scale219, Variant.From<Vector2>(ref this._scale219));
		}

		/// <inheritdoc />
		// Token: 0x06001DDC RID: 7644 RVA: 0x000C2618 File Offset: 0x000C0818
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._window, ref variant))
			{
				this._window = variant.As<Window>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos43, ref variant2))
			{
				this._pos43 = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._scale43, ref variant3))
			{
				this._scale43 = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos169, ref variant4))
			{
				this._pos169 = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._scale169, ref variant5))
			{
				this._scale169 = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos219, ref variant6))
			{
				this._pos219 = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NAncientBgContainer.PropertyName._scale219, ref variant7))
			{
				this._scale219 = variant7.As<Vector2>();
			}
		}

		// Token: 0x04000B7E RID: 2942
		[Nullable(1)]
		private Window _window;

		// Token: 0x04000B7F RID: 2943
		private const float _ratioMin = 1.3333f;

		// Token: 0x04000B80 RID: 2944
		private const float _ratioNormal = 1.7777f;

		// Token: 0x04000B81 RID: 2945
		private const float _ratioMax = 2.3333f;

		// Token: 0x04000B82 RID: 2946
		private Vector2 _pos43 = new Vector2(-140f, 110f);

		// Token: 0x04000B83 RID: 2947
		private Vector2 _scale43 = new Vector2(1f, 1f);

		// Token: 0x04000B84 RID: 2948
		private Vector2 _pos169 = new Vector2(0f, 40f);

		// Token: 0x04000B85 RID: 2949
		private Vector2 _scale169 = new Vector2(0.89f, 0.89f);

		// Token: 0x04000B86 RID: 2950
		private Vector2 _pos219 = new Vector2(330f, 40f);

		// Token: 0x04000B87 RID: 2951
		private Vector2 _scale219 = new Vector2(1f, 1f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001250 RID: 4688
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040046E4 RID: 18148
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x040046E5 RID: 18149
			public static readonly StringName OnWindowChange = "OnWindowChange";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001251 RID: 4689
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x040046E6 RID: 18150
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_pos43' field.
			/// </summary>
			// Token: 0x040046E7 RID: 18151
			public static readonly StringName _pos43 = "_pos43";

			/// <summary>
			/// Cached name for the '_scale43' field.
			/// </summary>
			// Token: 0x040046E8 RID: 18152
			public static readonly StringName _scale43 = "_scale43";

			/// <summary>
			/// Cached name for the '_pos169' field.
			/// </summary>
			// Token: 0x040046E9 RID: 18153
			public static readonly StringName _pos169 = "_pos169";

			/// <summary>
			/// Cached name for the '_scale169' field.
			/// </summary>
			// Token: 0x040046EA RID: 18154
			public static readonly StringName _scale169 = "_scale169";

			/// <summary>
			/// Cached name for the '_pos219' field.
			/// </summary>
			// Token: 0x040046EB RID: 18155
			public static readonly StringName _pos219 = "_pos219";

			/// <summary>
			/// Cached name for the '_scale219' field.
			/// </summary>
			// Token: 0x040046EC RID: 18156
			public static readonly StringName _scale219 = "_scale219";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001252 RID: 4690
		public class SignalName : Control.SignalName
		{
		}
	}
}
