using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// A base TextEdit class that is used to access the virtual keyboard
	/// for various devices/apis (ie Steam Deck)
	/// </summary>
	// Token: 0x020003A0 RID: 928
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NMegaTextEdit.cs")]
	public class NMegaTextEdit : TextEdit
	{
		/// <summary>
		/// This mirrors LineEdit.IsEditing()
		/// </summary>
		// Token: 0x060037D7 RID: 14295 RVA: 0x001A5C4F File Offset: 0x001A3E4F
		public bool IsEditing()
		{
			return this._isEditing;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x001A5C58 File Offset: 0x001A3E58
		public override void _Ready()
		{
			this.RefreshFont();
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.StopEditing)), 0U);
			if (base.HasNode("SelectionReticle"))
			{
				this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
			}
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
			base.Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
			base.Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001A5D29 File Offset: 0x001A3F29
		public void RefreshFont()
		{
			this.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.TextEdit.Font);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x001A5D37 File Offset: 0x001A3F37
		private void OnFocus()
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				NSelectionReticle selectionReticle = this._selectionReticle;
				if (selectionReticle == null)
				{
					return;
				}
				selectionReticle.OnSelect();
			}
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x001A5D55 File Offset: 0x001A3F55
		private void OnUnfocus()
		{
			NSelectionReticle selectionReticle = this._selectionReticle;
			if (selectionReticle == null)
			{
				return;
			}
			selectionReticle.OnDeselect();
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x001A5D68 File Offset: 0x001A3F68
		[NullableContext(1)]
		public override void _GuiInput(InputEvent inputEvent)
		{
			base._GuiInput(inputEvent);
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && inputEventMouseButton.ButtonIndex == 1L && inputEventMouseButton.IsPressed())
			{
				this.OpenKeyboard();
			}
			if (inputEvent.IsActionPressed(MegaInput.select, false, false))
			{
				this.OpenKeyboard();
			}
			if (inputEvent.IsActionPressed(MegaInput.cancel, false, false) && this.IsEditing())
			{
				this.StopEditing();
				Viewport viewport = base.GetViewport();
				if (viewport == null)
				{
					return;
				}
				viewport.SetInputAsHandled();
			}
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x001A5DE0 File Offset: 0x001A3FE0
		private void OpenKeyboard()
		{
			this.TryGrabFocus();
			this._isEditing = true;
			PlatformUtil.OpenVirtualKeyboard();
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x001A5DF4 File Offset: 0x001A3FF4
		private void StopEditing()
		{
			this._isEditing = false;
			PlatformUtil.CloseVirtualKeyboard();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037DF RID: 14303 RVA: 0x001A5E04 File Offset: 0x001A4004
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NMegaTextEdit.MethodName.IsEditing, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName.RefreshFont, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMegaTextEdit.MethodName.OpenKeyboard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaTextEdit.MethodName.StopEditing, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037E0 RID: 14304 RVA: 0x001A5FC4 File Offset: 0x001A41C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMegaTextEdit.MethodName.IsEditing && args.Count == 0)
			{
				bool flag = this.IsEditing();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName.RefreshFont && args.Count == 0)
			{
				this.RefreshFont();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName.OpenKeyboard && args.Count == 0)
			{
				this.OpenKeyboard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaTextEdit.MethodName.StopEditing && args.Count == 0)
			{
				this.StopEditing();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060037E1 RID: 14305 RVA: 0x001A6118 File Offset: 0x001A4318
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMegaTextEdit.MethodName.IsEditing || (ref method) == NMegaTextEdit.MethodName._Ready || (ref method) == NMegaTextEdit.MethodName.RefreshFont || (ref method) == NMegaTextEdit.MethodName.OnFocus || (ref method) == NMegaTextEdit.MethodName.OnUnfocus || (ref method) == NMegaTextEdit.MethodName._GuiInput || (ref method) == NMegaTextEdit.MethodName.OpenKeyboard || (ref method) == NMegaTextEdit.MethodName.StopEditing || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060037E2 RID: 14306 RVA: 0x001A61A4 File Offset: 0x001A43A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMegaTextEdit.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NMegaTextEdit.PropertyName._isEditing)
			{
				this._isEditing = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060037E3 RID: 14307 RVA: 0x001A61E4 File Offset: 0x001A43E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMegaTextEdit.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NMegaTextEdit.PropertyName._isEditing)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isEditing);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037E4 RID: 14308 RVA: 0x001A623C File Offset: 0x001A443C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMegaTextEdit.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMegaTextEdit.PropertyName._isEditing, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037E5 RID: 14309 RVA: 0x001A6291 File Offset: 0x001A4491
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMegaTextEdit.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NMegaTextEdit.PropertyName._isEditing, Variant.From<bool>(ref this._isEditing));
		}

		/// <inheritdoc />
		// Token: 0x060037E6 RID: 14310 RVA: 0x001A62C8 File Offset: 0x001A44C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMegaTextEdit.PropertyName._selectionReticle, ref variant))
			{
				this._selectionReticle = variant.As<NSelectionReticle>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMegaTextEdit.PropertyName._isEditing, ref variant2))
			{
				this._isEditing = variant2.As<bool>();
			}
		}

		// Token: 0x040015B6 RID: 5558
		[Nullable(2)]
		private NSelectionReticle _selectionReticle;

		// Token: 0x040015B7 RID: 5559
		private bool _isEditing;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001773 RID: 6003
		public class MethodName : TextEdit.MethodName
		{
			/// <summary>
			/// Cached name for the 'IsEditing' method.
			/// </summary>
			// Token: 0x04005B7E RID: 23422
			public static readonly StringName IsEditing = "IsEditing";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B7F RID: 23423
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'RefreshFont' method.
			/// </summary>
			// Token: 0x04005B80 RID: 23424
			public static readonly StringName RefreshFont = "RefreshFont";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005B81 RID: 23425
			public static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005B82 RID: 23426
			public static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005B83 RID: 23427
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OpenKeyboard' method.
			/// </summary>
			// Token: 0x04005B84 RID: 23428
			public static readonly StringName OpenKeyboard = "OpenKeyboard";

			/// <summary>
			/// Cached name for the 'StopEditing' method.
			/// </summary>
			// Token: 0x04005B85 RID: 23429
			public static readonly StringName StopEditing = "StopEditing";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001774 RID: 6004
		public class PropertyName : TextEdit.PropertyName
		{
			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005B86 RID: 23430
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_isEditing' field.
			/// </summary>
			// Token: 0x04005B87 RID: 23431
			public static readonly StringName _isEditing = "_isEditing";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001775 RID: 6005
		public class SignalName : TextEdit.SignalName
		{
		}
	}
}
