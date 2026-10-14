using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// The language dropdown in the OptionsScreen.
	/// </summary>
	// Token: 0x0200029D RID: 669
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageDropdown.cs")]
	public class NLanguageDropdown : NSettingsDropdown
	{
		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x000F21E0 File Offset: 0x000F03E0
		[Nullable(1)]
		private string CurrentLanguage
		{
			[NullableContext(1)]
			get
			{
				return LocManager.Instance.Language;
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000F21EC File Offset: 0x000F03EC
		public override void _Ready()
		{
			this.ConnectSignals();
			this.PopulateOptions();
			this._currentOptionLabel.SetTextAutoSize(NLanguageDropdown.GetLanguageNameForCode(this.CurrentLanguage));
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000F2210 File Offset: 0x000F0410
		private void PopulateOptions()
		{
			base.ClearDropdownItems();
			foreach (string text in LocManager.Languages)
			{
				NLanguageDropdownItem nlanguageDropdownItem = this._dropdownItemScene.Instantiate<NLanguageDropdownItem>(0L);
				this._dropdownItems.AddChildSafely(nlanguageDropdownItem);
				nlanguageDropdownItem.Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
				nlanguageDropdownItem.Init(text);
			}
			this._dropdownItems.GetParent<NDropdownContainer>().RefreshLayout();
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000F22B0 File Offset: 0x000F04B0
		[NullableContext(1)]
		private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
		{
			NLanguageDropdownItem nlanguageDropdownItem = (NLanguageDropdownItem)nDropdownItem;
			if (nlanguageDropdownItem.LanguageCode == this.CurrentLanguage)
			{
				return;
			}
			base.CloseDropdown();
			this._currentOptionLabel.SetTextAutoSize(NLanguageDropdown.GetLanguageNameForCode(nlanguageDropdownItem.LanguageCode));
			SaveManager.Instance.SettingsSave.Language = nlanguageDropdownItem.LanguageCode;
			LocManager.Instance.SetLanguage(nlanguageDropdownItem.LanguageCode);
			NGame.Instance.Relocalize();
			NGame.Instance.MainMenu.OpenSettingsMenu();
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000F2334 File Offset: 0x000F0534
		[NullableContext(1)]
		public static string GetLanguageNameForCode(string languageCode)
		{
			string text;
			if (!NLanguageDropdown._languageCodeToName.TryGetValue(languageCode.ToUpperInvariant(), out text))
			{
				throw new InvalidOperationException("Tried to get language name for code " + languageCode + " but it doesn't exist!");
			}
			return text;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600238C RID: 9100 RVA: 0x000F236C File Offset: 0x000F056C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NLanguageDropdown.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageDropdown.MethodName.PopulateOptions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLanguageDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "nDropdownItem", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NLanguageDropdown.MethodName.GetLanguageNameForCode, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "languageCode", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600238D RID: 9101 RVA: 0x000F2494 File Offset: 0x000F0694
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLanguageDropdown.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageDropdown.MethodName.PopulateOptions && args.Count == 0)
			{
				this.PopulateOptions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageDropdown.MethodName.OnDropdownItemSelected && args.Count == 1)
			{
				this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLanguageDropdown.MethodName.GetLanguageNameForCode && args.Count == 1)
			{
				string languageNameForCode = NLanguageDropdown.GetLanguageNameForCode(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref languageNameForCode);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000F2560 File Offset: 0x000F0760
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLanguageDropdown.MethodName.GetLanguageNameForCode && args.Count == 1)
			{
				string languageNameForCode = NLanguageDropdown.GetLanguageNameForCode(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref languageNameForCode);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600238F RID: 9103 RVA: 0x000F25B0 File Offset: 0x000F07B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLanguageDropdown.MethodName._Ready || (ref method) == NLanguageDropdown.MethodName.PopulateOptions || (ref method) == NLanguageDropdown.MethodName.OnDropdownItemSelected || (ref method) == NLanguageDropdown.MethodName.GetLanguageNameForCode || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002390 RID: 9104 RVA: 0x000F2600 File Offset: 0x000F0800
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLanguageDropdown.PropertyName._dropdownItemScene)
			{
				this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002391 RID: 9105 RVA: 0x000F2628 File Offset: 0x000F0828
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLanguageDropdown.PropertyName.CurrentLanguage)
			{
				string currentLanguage = this.CurrentLanguage;
				value = VariantUtils.CreateFrom<string>(ref currentLanguage);
				return true;
			}
			if ((ref name) == NLanguageDropdown.PropertyName._dropdownItemScene)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002392 RID: 9106 RVA: 0x000F2680 File Offset: 0x000F0880
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLanguageDropdown.PropertyName._dropdownItemScene, 17L, "PackedScene", 4102L, true),
				new PropertyInfo(4L, NLanguageDropdown.PropertyName.CurrentLanguage, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002393 RID: 9107 RVA: 0x000F26D6 File Offset: 0x000F08D6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLanguageDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
		}

		/// <inheritdoc />
		// Token: 0x06002394 RID: 9108 RVA: 0x000F26F8 File Offset: 0x000F08F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLanguageDropdown.PropertyName._dropdownItemScene, ref variant))
			{
				this._dropdownItemScene = variant.As<PackedScene>();
			}
		}

		// Token: 0x04000D90 RID: 3472
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private PackedScene _dropdownItemScene;

		// Token: 0x04000D91 RID: 3473
		[Nullable(1)]
		private static readonly Dictionary<string, string> _languageCodeToName = new Dictionary<string, string>
		{
			{ "ARA", "العربية" },
			{ "BEN", "ব\u09be\u0982ল\u09be" },
			{ "CZE", "Čeština" },
			{ "DEU", "Deutsch" },
			{ "DUT", "Nederlands" },
			{ "ENG", "English" },
			{ "ESP", "Español (Latinoamérica)" },
			{ "FIL", "Filipino" },
			{ "FIN", "Suomi" },
			{ "FRA", "Français" },
			{ "GRE", "Ελληνικά" },
			{ "HIN", "ह\u093fन\u094dद\u0940" },
			{ "IND", "Bahasa Indonesia" },
			{ "ITA", "Italiano" },
			{ "JPN", "日本語" },
			{ "KOR", "한국어" },
			{ "MAL", "Bahasa Melayu" },
			{ "NOR", "Norsk" },
			{ "POL", "Polski" },
			{ "POR", "Português" },
			{ "PTB", "Português Brasileiro" },
			{ "RUS", "Русский" },
			{ "SPA", "Español (Castellano)" },
			{ "SWE", "Svenska" },
			{ "THA", "ไทย" },
			{ "TUR", "Türkçe" },
			{ "UKR", "Українська" },
			{ "VIE", "Tiếng Việt" },
			{ "ZHS", "中文" },
			{ "ZHT", "繁體中文" }
		};

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138D RID: 5005
		public new class MethodName : NSettingsDropdown.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B5B RID: 19291
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PopulateOptions' method.
			/// </summary>
			// Token: 0x04004B5C RID: 19292
			public static readonly StringName PopulateOptions = "PopulateOptions";

			/// <summary>
			/// Cached name for the 'OnDropdownItemSelected' method.
			/// </summary>
			// Token: 0x04004B5D RID: 19293
			public static readonly StringName OnDropdownItemSelected = "OnDropdownItemSelected";

			/// <summary>
			/// Cached name for the 'GetLanguageNameForCode' method.
			/// </summary>
			// Token: 0x04004B5E RID: 19294
			public static readonly StringName GetLanguageNameForCode = "GetLanguageNameForCode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138E RID: 5006
		public new class PropertyName : NSettingsDropdown.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CurrentLanguage' property.
			/// </summary>
			// Token: 0x04004B5F RID: 19295
			public static readonly StringName CurrentLanguage = "CurrentLanguage";

			/// <summary>
			/// Cached name for the '_dropdownItemScene' field.
			/// </summary>
			// Token: 0x04004B60 RID: 19296
			public static readonly StringName _dropdownItemScene = "_dropdownItemScene";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200138F RID: 5007
		public new class SignalName : NSettingsDropdown.SignalName
		{
		}
	}
}
