using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	/// <summary>
	/// Overridden so the <see cref="T:MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NContinueRunInfo" /> node appear when this button is hovered. That's it.
	/// Same exact behavior for the AbandonRun button btw. They should be identical.
	/// </summary>
	// Token: 0x02000309 RID: 777
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuContinueButton.cs")]
	public class NMainMenuContinueButton : NMainMenuTextButton
	{
		// Token: 0x06002ACF RID: 10959 RVA: 0x0012CC5A File Offset: 0x0012AE5A
		public override void _Ready()
		{
			this.ConnectSignals();
			this._mainMenu = base.GetParent().GetParent<NMainMenu>();
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x0012CC73 File Offset: 0x0012AE73
		protected override void OnFocus()
		{
			base.OnFocus();
			if (this._mainMenu.ContinueRunInfo.HasResult)
			{
				this._mainMenu.ContinueRunInfo.AnimShow();
			}
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x0012CC9D File Offset: 0x0012AE9D
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._mainMenu.ContinueRunInfo.AnimHide();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AD2 RID: 10962 RVA: 0x0012CCB8 File Offset: 0x0012AEB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMainMenuContinueButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuContinueButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMainMenuContinueButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AD3 RID: 10963 RVA: 0x0012CD5C File Offset: 0x0012AF5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMainMenuContinueButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuContinueButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMainMenuContinueButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002AD4 RID: 10964 RVA: 0x0012CDE1 File Offset: 0x0012AFE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMainMenuContinueButton.MethodName._Ready || (ref method) == NMainMenuContinueButton.MethodName.OnFocus || (ref method) == NMainMenuContinueButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002AD5 RID: 10965 RVA: 0x0012CE17 File Offset: 0x0012B017
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMainMenuContinueButton.PropertyName._mainMenu)
			{
				this._mainMenu = VariantUtils.ConvertTo<NMainMenu>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002AD6 RID: 10966 RVA: 0x0012CE3C File Offset: 0x0012B03C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMainMenuContinueButton.PropertyName._mainMenu)
			{
				value = VariantUtils.CreateFrom<NMainMenu>(ref this._mainMenu);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002AD7 RID: 10967 RVA: 0x0012CE68 File Offset: 0x0012B068
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMainMenuContinueButton.PropertyName._mainMenu, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002AD8 RID: 10968 RVA: 0x0012CE9D File Offset: 0x0012B09D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMainMenuContinueButton.PropertyName._mainMenu, Variant.From<NMainMenu>(ref this._mainMenu));
		}

		/// <inheritdoc />
		// Token: 0x06002AD9 RID: 10969 RVA: 0x0012CEBC File Offset: 0x0012B0BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMainMenuContinueButton.PropertyName._mainMenu, ref variant))
			{
				this._mainMenu = variant.As<NMainMenu>();
			}
		}

		// Token: 0x04001007 RID: 4103
		[Nullable(1)]
		private NMainMenu _mainMenu;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001510 RID: 5392
		public new class MethodName : NMainMenuTextButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400505A RID: 20570
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400505B RID: 20571
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400505C RID: 20572
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001511 RID: 5393
		public new class PropertyName : NMainMenuTextButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_mainMenu' field.
			/// </summary>
			// Token: 0x0400505D RID: 20573
			public static readonly StringName _mainMenu = "_mainMenu";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001512 RID: 5394
		public new class SignalName : NMainMenuTextButton.SignalName
		{
		}
	}
}
