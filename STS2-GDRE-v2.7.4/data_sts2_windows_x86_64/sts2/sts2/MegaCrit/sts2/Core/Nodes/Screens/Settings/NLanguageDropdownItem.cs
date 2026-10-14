using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200029E RID: 670
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageDropdownItem.cs")]
	public class NLanguageDropdownItem : NDropdownItem
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002397 RID: 9111 RVA: 0x000F2927 File Offset: 0x000F0B27
		// (set) Token: 0x06002398 RID: 9112 RVA: 0x000F292F File Offset: 0x000F0B2F
		[Nullable(1)]
		public string LanguageCode
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000F2938 File Offset: 0x000F0B38
		[NullableContext(1)]
		public void Init(string languageCode)
		{
			this.LanguageCode = languageCode;
			string text = NLanguageDropdown.GetLanguageNameForCode(languageCode);
			float languageCompletion = LocManager.Instance.GetLanguageCompletion(languageCode);
			if (languageCompletion < 0.9f)
			{
				text = "[img]res://images/ui/language_warning.png[/img]" + text;
			}
			this._richLabel.SetTextAutoSize(text);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600239A RID: 9114 RVA: 0x000F2980 File Offset: 0x000F0B80
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NLanguageDropdownItem.MethodName.Init, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "languageCode", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600239B RID: 9115 RVA: 0x000F29E9 File Offset: 0x000F0BE9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLanguageDropdownItem.MethodName.Init && args.Count == 1)
			{
				this.Init(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600239C RID: 9116 RVA: 0x000F2A27 File Offset: 0x000F0C27
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLanguageDropdownItem.MethodName.Init || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600239D RID: 9117 RVA: 0x000F2A3F File Offset: 0x000F0C3F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLanguageDropdownItem.PropertyName.LanguageCode)
			{
				this.LanguageCode = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600239E RID: 9118 RVA: 0x000F2A64 File Offset: 0x000F0C64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLanguageDropdownItem.PropertyName.LanguageCode)
			{
				string languageCode = this.LanguageCode;
				value = VariantUtils.CreateFrom<string>(ref languageCode);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600239F RID: 9119 RVA: 0x000F2A9C File Offset: 0x000F0C9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NLanguageDropdownItem.PropertyName.LanguageCode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023A0 RID: 9120 RVA: 0x000F2AD0 File Offset: 0x000F0CD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName languageCode = NLanguageDropdownItem.PropertyName.LanguageCode;
			string languageCode2 = this.LanguageCode;
			info.AddProperty(languageCode, Variant.From<string>(ref languageCode2));
		}

		/// <inheritdoc />
		// Token: 0x060023A1 RID: 9121 RVA: 0x000F2B00 File Offset: 0x000F0D00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLanguageDropdownItem.PropertyName.LanguageCode, ref variant))
			{
				this.LanguageCode = variant.As<string>();
			}
		}

		// Token: 0x04000D92 RID: 3474
		[Nullable(1)]
		public const string languageWarningIconPath = "res://images/ui/language_warning.png";

		// Token: 0x04000D93 RID: 3475
		[Nullable(1)]
		private const string _warnImageTag = "[img]res://images/ui/language_warning.png[/img]";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001390 RID: 5008
		public new class MethodName : NDropdownItem.MethodName
		{
			/// <summary>
			/// Cached name for the 'Init' method.
			/// </summary>
			// Token: 0x04004B61 RID: 19297
			public static readonly StringName Init = "Init";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001391 RID: 5009
		public new class PropertyName : NDropdownItem.PropertyName
		{
			/// <summary>
			/// Cached name for the 'LanguageCode' property.
			/// </summary>
			// Token: 0x04004B62 RID: 19298
			public static readonly StringName LanguageCode = "LanguageCode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001392 RID: 5010
		public new class SignalName : NDropdownItem.SignalName
		{
		}
	}
}
