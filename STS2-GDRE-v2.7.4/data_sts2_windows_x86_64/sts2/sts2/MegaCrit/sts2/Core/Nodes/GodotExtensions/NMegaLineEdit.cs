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
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions
{
	/// <summary>
	/// A base LineEdit class that is used to access the virtual keyboard
	/// for various devices/apis (ie Steam Deck)
	/// </summary>
	// Token: 0x0200039F RID: 927
	[ScriptPath("res://src/Core/Nodes/GodotExtensions/NMegaLineEdit.cs")]
	public class NMegaLineEdit : LineEdit
	{
		// Token: 0x060037CE RID: 14286 RVA: 0x001A59FC File Offset: 0x001A3BFC
		public override void _Ready()
		{
			this.ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.LineEdit.Font);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x001A5A0C File Offset: 0x001A3C0C
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
			if (inputEvent.IsActionPressed(MegaInput.cancel, false, false) && base.IsEditing())
			{
				base.Unedit();
				Viewport viewport = base.GetViewport();
				if (viewport != null)
				{
					viewport.SetInputAsHandled();
				}
				PlatformUtil.CloseVirtualKeyboard();
			}
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x001A5A8A File Offset: 0x001A3C8A
		private void OpenKeyboard()
		{
			base.Edit();
			PlatformUtil.OpenVirtualKeyboard();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060037D1 RID: 14289 RVA: 0x001A5A98 File Offset: 0x001A3C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMegaLineEdit.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMegaLineEdit.MethodName._GuiInput, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NMegaLineEdit.MethodName.OpenKeyboard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060037D2 RID: 14290 RVA: 0x001A5B6C File Offset: 0x001A3D6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMegaLineEdit.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaLineEdit.MethodName._GuiInput && args.Count == 1)
			{
				this._GuiInput(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMegaLineEdit.MethodName.OpenKeyboard && args.Count == 0)
			{
				this.OpenKeyboard();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060037D3 RID: 14291 RVA: 0x001A5BFF File Offset: 0x001A3DFF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMegaLineEdit.MethodName._Ready || (ref method) == NMegaLineEdit.MethodName._GuiInput || (ref method) == NMegaLineEdit.MethodName.OpenKeyboard || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060037D4 RID: 14292 RVA: 0x001A5C35 File Offset: 0x001A3E35
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060037D5 RID: 14293 RVA: 0x001A5C3E File Offset: 0x001A3E3E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001770 RID: 6000
		public class MethodName : LineEdit.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B7B RID: 23419
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_GuiInput' method.
			/// </summary>
			// Token: 0x04005B7C RID: 23420
			public static readonly StringName _GuiInput = "_GuiInput";

			/// <summary>
			/// Cached name for the 'OpenKeyboard' method.
			/// </summary>
			// Token: 0x04005B7D RID: 23421
			public static readonly StringName OpenKeyboard = "OpenKeyboard";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001771 RID: 6001
		public class PropertyName : LineEdit.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001772 RID: 6002
		public class SignalName : LineEdit.SignalName
		{
		}
	}
}
