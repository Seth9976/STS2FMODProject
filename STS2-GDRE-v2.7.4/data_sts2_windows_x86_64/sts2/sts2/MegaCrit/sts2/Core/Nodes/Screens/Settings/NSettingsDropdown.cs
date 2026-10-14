using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// The display mode dropdown in the SettingsScreen.
	/// </summary>
	// Token: 0x020002B6 RID: 694
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsDropdown.cs")]
	public abstract class NSettingsDropdown : NDropdown
	{
		// Token: 0x060024B2 RID: 9394 RVA: 0x000F8A28 File Offset: 0x000F6C28
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000F8A46 File Offset: 0x000F6C46
		protected override void OnFocus()
		{
			base.OnFocus();
			this._currentOptionHighlight.Modulate = new Color("3C5B6B");
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000F8A7A File Offset: 0x000F6C7A
		protected override void OnUnfocus()
		{
			this._selectionReticle.OnDeselect();
			this._currentOptionHighlight.Modulate = new Color("2C434F");
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024B5 RID: 9397 RVA: 0x000F8A9C File Offset: 0x000F6C9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSettingsDropdown.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsDropdown.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsDropdown.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024B6 RID: 9398 RVA: 0x000F8B40 File Offset: 0x000F6D40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsDropdown.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsDropdown.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsDropdown.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060024B7 RID: 9399 RVA: 0x000F8BC5 File Offset: 0x000F6DC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsDropdown.MethodName.ConnectSignals || (ref method) == NSettingsDropdown.MethodName.OnFocus || (ref method) == NSettingsDropdown.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060024B8 RID: 9400 RVA: 0x000F8BFB File Offset: 0x000F6DFB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsDropdown.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060024B9 RID: 9401 RVA: 0x000F8C20 File Offset: 0x000F6E20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsDropdown.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060024BA RID: 9402 RVA: 0x000F8C4C File Offset: 0x000F6E4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsDropdown.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060024BB RID: 9403 RVA: 0x000F8C81 File Offset: 0x000F6E81
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsDropdown.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x060024BC RID: 9404 RVA: 0x000F8CA0 File Offset: 0x000F6EA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsDropdown.PropertyName._selectionReticle, ref variant))
			{
				this._selectionReticle = variant.As<NSelectionReticle>();
			}
		}

		// Token: 0x04000DC0 RID: 3520
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DA RID: 5082
		public new class MethodName : NDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004BDE RID: 19422
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004BDF RID: 19423
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004BE0 RID: 19424
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DB RID: 5083
		public new class PropertyName : NDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004BE1 RID: 19425
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013DC RID: 5084
		public new class SignalName : NDropdown.SignalName
		{
		}
	}
}
