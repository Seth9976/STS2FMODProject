using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// A script specifically used to make the Settings panels (General Settings, Graphics Settings, Audio Settings) fade in nicely
	/// </summary>
	// Token: 0x020002B8 RID: 696
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsPanel.cs")]
	public class NSettingsPanel : Control
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x000F9060 File Offset: 0x000F7260
		// (set) Token: 0x060024CA RID: 9418 RVA: 0x000F9068 File Offset: 0x000F7268
		[Nullable(1)]
		public VBoxContainer Content
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000F9071 File Offset: 0x000F7271
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._firstControl;
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000F907C File Offset: 0x000F727C
		public override void _Ready()
		{
			this.Content = base.GetNode<VBoxContainer>("VBoxContainer");
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
			base.GetViewport().Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.RefreshSize)), 0U);
			this.RefreshSize();
			List<Control> list = new List<Control>();
			this.GetSettingsOptionsRecursive(this.Content, list, true);
			foreach (Control control in list)
			{
				control.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateNavigation)), 0U);
			}
			this.UpdateNavigation();
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000F915C File Offset: 0x000F735C
		private void RefreshSize()
		{
			Vector2 size = base.GetParent<Control>().Size;
			Vector2 minimumSize = this.Content.GetMinimumSize();
			if (minimumSize.Y + this._minPadding >= size.Y)
			{
				base.Size = new Vector2(this.Content.Size.X, minimumSize.Y + size.Y * 0.4f);
				return;
			}
			base.Size = new Vector2(this.Content.Size.X, minimumSize.Y);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000F91E8 File Offset: 0x000F73E8
		protected virtual void OnVisibilityChange()
		{
			if (base.Visible)
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this, "modulate", Colors.White, 0.5).From(StsColors.transparentBlack).SetEase(1L)
					.SetTrans(7L);
			}
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000F9268 File Offset: 0x000F7468
		protected virtual void UpdateNavigation()
		{
			List<Control> list = new List<Control>();
			this.GetSettingsOptionsRecursive(this.Content, list, false);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].FocusNeighborLeft = list[i].GetPath();
				list[i].FocusNeighborRight = list[i].GetPath();
				list[i].FocusNeighborTop = ((i > 0) ? list[i - 1].GetPath() : list[i].GetPath());
				list[i].FocusNeighborBottom = ((i < list.Count - 1) ? list[i + 1].GetPath() : list[i].GetPath());
			}
			this._firstControl = list.FirstOrDefault<Control>();
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000F933C File Offset: 0x000F753C
		[NullableContext(1)]
		private void GetSettingsOptionsRecursive(Control parent, List<Control> ancestors, bool includeInvisibleSettings = false)
		{
			foreach (Control control in parent.GetChildren(false).OfType<Control>())
			{
				if (!this.IsSettingsOption(control))
				{
					this.GetSettingsOptionsRecursive(control, ancestors, includeInvisibleSettings);
				}
				else if ((control.GetParent<Control>().IsVisible() || includeInvisibleSettings) && control.FocusMode == 2L)
				{
					ancestors.Add(control);
				}
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000F93BC File Offset: 0x000F75BC
		[NullableContext(1)]
		private bool IsSettingsOption(Control c)
		{
			NButton nbutton = c as NButton;
			if (nbutton != null)
			{
				return nbutton.IsEnabled;
			}
			return c is NPaginator || c is NTickbox || c is NButton || c is NDropdownPositioner || c is NSettingsSlider;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024D2 RID: 9426 RVA: 0x000F940C File Offset: 0x000F760C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSettingsPanel.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsPanel.MethodName.RefreshSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsPanel.MethodName.OnVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsPanel.MethodName.UpdateNavigation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsPanel.MethodName.IsSettingsOption, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "c", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024D3 RID: 9427 RVA: 0x000F953C File Offset: 0x000F773C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsPanel.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsPanel.MethodName.RefreshSize && args.Count == 0)
			{
				this.RefreshSize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsPanel.MethodName.OnVisibilityChange && args.Count == 0)
			{
				this.OnVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsPanel.MethodName.UpdateNavigation && args.Count == 0)
			{
				this.UpdateNavigation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsPanel.MethodName.IsSettingsOption && args.Count == 1)
			{
				bool flag = this.IsSettingsOption(VariantUtils.ConvertTo<Control>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060024D4 RID: 9428 RVA: 0x000F9620 File Offset: 0x000F7820
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsPanel.MethodName._Ready || (ref method) == NSettingsPanel.MethodName.RefreshSize || (ref method) == NSettingsPanel.MethodName.OnVisibilityChange || (ref method) == NSettingsPanel.MethodName.UpdateNavigation || (ref method) == NSettingsPanel.MethodName.IsSettingsOption || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060024D5 RID: 9429 RVA: 0x000F9680 File Offset: 0x000F7880
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsPanel.PropertyName.Content)
			{
				this.Content = VariantUtils.ConvertTo<VBoxContainer>(ref value);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._minPadding)
			{
				this._minPadding = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._firstControl)
			{
				this._firstControl = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060024D6 RID: 9430 RVA: 0x000F9704 File Offset: 0x000F7904
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsPanel.PropertyName.Content)
			{
				VBoxContainer content = this.Content;
				value = VariantUtils.CreateFrom<VBoxContainer>(ref content);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._minPadding)
			{
				value = VariantUtils.CreateFrom<float>(ref this._minPadding);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._firstControl)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._firstControl);
				return true;
			}
			if ((ref name) == NSettingsPanel.PropertyName._tween)
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
		// Token: 0x060024D7 RID: 9431 RVA: 0x000F97C0 File Offset: 0x000F79C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(3L, NSettingsPanel.PropertyName._minPadding, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsPanel.PropertyName._firstControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsPanel.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsPanel.PropertyName.Content, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsPanel.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024D8 RID: 9432 RVA: 0x000F9878 File Offset: 0x000F7A78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName content = NSettingsPanel.PropertyName.Content;
			VBoxContainer content2 = this.Content;
			info.AddProperty(content, Variant.From<VBoxContainer>(ref content2));
			info.AddProperty(NSettingsPanel.PropertyName._minPadding, Variant.From<float>(ref this._minPadding));
			info.AddProperty(NSettingsPanel.PropertyName._firstControl, Variant.From<Control>(ref this._firstControl));
			info.AddProperty(NSettingsPanel.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x060024D9 RID: 9433 RVA: 0x000F98E8 File Offset: 0x000F7AE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsPanel.PropertyName.Content, ref variant))
			{
				this.Content = variant.As<VBoxContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsPanel.PropertyName._minPadding, ref variant2))
			{
				this._minPadding = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSettingsPanel.PropertyName._firstControl, ref variant3))
			{
				this._firstControl = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSettingsPanel.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		// Token: 0x04000DC5 RID: 3525
		private float _minPadding = 50f;

		// Token: 0x04000DC6 RID: 3526
		[Nullable(2)]
		protected Control _firstControl;

		// Token: 0x04000DC7 RID: 3527
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E0 RID: 5088
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BE6 RID: 19430
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshSize' method.
			/// </summary>
			// Token: 0x04004BE7 RID: 19431
			public static readonly StringName RefreshSize = "RefreshSize";

			/// <summary>
			/// Cached name for the 'OnVisibilityChange' method.
			/// </summary>
			// Token: 0x04004BE8 RID: 19432
			public static readonly StringName OnVisibilityChange = "OnVisibilityChange";

			/// <summary>
			/// Cached name for the 'UpdateNavigation' method.
			/// </summary>
			// Token: 0x04004BE9 RID: 19433
			public static readonly StringName UpdateNavigation = "UpdateNavigation";

			/// <summary>
			/// Cached name for the 'IsSettingsOption' method.
			/// </summary>
			// Token: 0x04004BEA RID: 19434
			public static readonly StringName IsSettingsOption = "IsSettingsOption";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E1 RID: 5089
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Content' property.
			/// </summary>
			// Token: 0x04004BEB RID: 19435
			public static readonly StringName Content = "Content";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004BEC RID: 19436
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_minPadding' field.
			/// </summary>
			// Token: 0x04004BED RID: 19437
			public static readonly StringName _minPadding = "_minPadding";

			/// <summary>
			/// Cached name for the '_firstControl' field.
			/// </summary>
			// Token: 0x04004BEE RID: 19438
			public static readonly StringName _firstControl = "_firstControl";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004BEF RID: 19439
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013E2 RID: 5090
		public class SignalName : Control.SignalName
		{
		}
	}
}
