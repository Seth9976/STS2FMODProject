using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002BD RID: 701
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTickbox.cs")]
	public class NSettingsTickbox : NTickbox
	{
		// Token: 0x0600253A RID: 9530 RVA: 0x000FCC77 File Offset: 0x000FAE77
		protected override void ConnectSignals()
		{
			base.ConnectSignals();
			this._selectionReticle = base.GetNode<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000FCC95 File Offset: 0x000FAE95
		protected override void OnFocus()
		{
			base.OnFocus();
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000FCCB4 File Offset: 0x000FAEB4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._selectionReticle.OnDeselect();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600253D RID: 9533 RVA: 0x000FCCC8 File Offset: 0x000FAEC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSettingsTickbox.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTickbox.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSettingsTickbox.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600253E RID: 9534 RVA: 0x000FCD6C File Offset: 0x000FAF6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSettingsTickbox.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTickbox.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSettingsTickbox.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600253F RID: 9535 RVA: 0x000FCDF1 File Offset: 0x000FAFF1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSettingsTickbox.MethodName.ConnectSignals || (ref method) == NSettingsTickbox.MethodName.OnFocus || (ref method) == NSettingsTickbox.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002540 RID: 9536 RVA: 0x000FCE27 File Offset: 0x000FB027
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSettingsTickbox.PropertyName._settingsScreen)
			{
				this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
				return true;
			}
			if ((ref name) == NSettingsTickbox.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002541 RID: 9537 RVA: 0x000FCE68 File Offset: 0x000FB068
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSettingsTickbox.PropertyName._settingsScreen)
			{
				value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
				return true;
			}
			if ((ref name) == NSettingsTickbox.PropertyName._selectionReticle)
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
		// Token: 0x06002542 RID: 9538 RVA: 0x000FCEC0 File Offset: 0x000FB0C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSettingsTickbox.PropertyName._settingsScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSettingsTickbox.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002543 RID: 9539 RVA: 0x000FCF16 File Offset: 0x000FB116
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSettingsTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
			info.AddProperty(NSettingsTickbox.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x06002544 RID: 9540 RVA: 0x000FCF4C File Offset: 0x000FB14C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSettingsTickbox.PropertyName._settingsScreen, ref variant))
			{
				this._settingsScreen = variant.As<NSettingsScreen>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSettingsTickbox.PropertyName._selectionReticle, ref variant2))
			{
				this._selectionReticle = variant2.As<NSelectionReticle>();
			}
		}

		// Token: 0x04000DEA RID: 3562
		[Nullable(1)]
		private NSettingsScreen _settingsScreen;

		// Token: 0x04000DEB RID: 3563
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F4 RID: 5108
		public new class MethodName : NTickbox.MethodName
		{
			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x04004C31 RID: 19505
			public new static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004C32 RID: 19506
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004C33 RID: 19507
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F5 RID: 5109
		public new class PropertyName : NTickbox.PropertyName
		{
			/// <summary>
			/// Cached name for the '_settingsScreen' field.
			/// </summary>
			// Token: 0x04004C34 RID: 19508
			public static readonly StringName _settingsScreen = "_settingsScreen";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04004C35 RID: 19509
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013F6 RID: 5110
		public new class SignalName : NTickbox.SignalName
		{
		}
	}
}
