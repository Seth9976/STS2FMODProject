using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Godot;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;
using SmartFormat;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Formatting;
using SmartFormat.Core.Parsing;
using SmartFormat.Extensions;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BBB RID: 3003
	[NullableContext(1)]
	[Nullable(0)]
	public class LocManager
	{
		// Token: 0x17002292 RID: 8850
		// (get) Token: 0x06007D0D RID: 32013 RVA: 0x002A9605 File Offset: 0x002A7805
		// (set) Token: 0x06007D0E RID: 32014 RVA: 0x002A960C File Offset: 0x002A780C
		public static LocManager Instance { get; private set; } = null;

		// Token: 0x17002293 RID: 8851
		// (get) Token: 0x06007D0F RID: 32015 RVA: 0x002A9614 File Offset: 0x002A7814
		private static string LocalizationAssetDir
		{
			get
			{
				return "res://localization";
			}
		}

		/// <summary>
		/// Indicates whether any localization override files were loaded from the user override directory.
		/// Useful for debugging and UI indicators.
		/// </summary>
		// Token: 0x17002294 RID: 8852
		// (get) Token: 0x06007D10 RID: 32016 RVA: 0x002A961B File Offset: 0x002A781B
		// (set) Token: 0x06007D11 RID: 32017 RVA: 0x002A9623 File Offset: 0x002A7823
		public bool OverridesActive { get; private set; }

		/// <summary>
		/// List of validation errors found in localization override files.
		/// Populated during LoadTablesFromPath() when override files contain JSON parsing errors or invalid SmartFormat syntax.
		/// </summary>
		// Token: 0x17002295 RID: 8853
		// (get) Token: 0x06007D12 RID: 32018 RVA: 0x002A962C File Offset: 0x002A782C
		// (set) Token: 0x06007D13 RID: 32019 RVA: 0x002A9634 File Offset: 0x002A7834
		public IReadOnlyList<LocValidationError> ValidationErrors { get; private set; } = Array.Empty<LocValidationError>();

		// Token: 0x17002296 RID: 8854
		// (get) Token: 0x06007D14 RID: 32020 RVA: 0x002A963D File Offset: 0x002A783D
		// (set) Token: 0x06007D15 RID: 32021 RVA: 0x002A9645 File Offset: 0x002A7845
		public string Language { get; private set; }

		// Token: 0x17002297 RID: 8855
		// (get) Token: 0x06007D16 RID: 32022 RVA: 0x002A964E File Offset: 0x002A784E
		public static List<string> Languages { get; }

		// Token: 0x17002298 RID: 8856
		// (get) Token: 0x06007D17 RID: 32023 RVA: 0x002A9655 File Offset: 0x002A7855
		// (set) Token: 0x06007D18 RID: 32024 RVA: 0x002A965D File Offset: 0x002A785D
		public CultureInfo CultureInfo { get; private set; }

		// Token: 0x17002299 RID: 8857
		// (get) Token: 0x06007D19 RID: 32025 RVA: 0x002A9666 File Offset: 0x002A7866
		// (set) Token: 0x06007D1A RID: 32026 RVA: 0x002A966E File Offset: 0x002A786E
		public StringComparer StringComparer { get; private set; }

		// Token: 0x06007D1B RID: 32027 RVA: 0x002A9678 File Offset: 0x002A7878
		private static CultureInfo GetCultureInfoSafe(string name)
		{
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = CultureInfo.GetCultureInfo(name);
			}
			catch (CultureNotFoundException)
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			return cultureInfo;
		}

		/// <summary>
		/// Initialize the singleton LocManager.
		/// This is favored over a static constructor so that we can precisely control the initialization time relative to
		/// other steps (mods must be initialized before this).
		/// </summary>
		// Token: 0x06007D1C RID: 32028 RVA: 0x002A96A8 File Offset: 0x002A78A8
		public static void Initialize()
		{
			LocManager.Instance = new LocManager();
			string text = ProjectSettings.GlobalizePath("user://localization_override");
			if (!DirAccess.DirExistsAbsolute(text))
			{
				DirAccess.MakeDirAbsolute(text);
			}
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x002A96DC File Offset: 0x002A78DC
		public LocManager()
		{
			if (LocManager._englishCultureInfo.Equals(CultureInfo.InvariantCulture))
			{
				Log.Warn("Running in .NET globalization-invariant mode. Locale-specific formatting will be disabled.", 2);
			}
			string text = SaveManager.Instance.SettingsSave.Language;
			if (string.IsNullOrEmpty(text))
			{
				string rawLanguage = PlatformUtil.GetRawLanguage();
				text = PlatformUtil.GetThreeLetterLanguageCode();
				if (text == null || !LocManager.Languages.Contains(text))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(90, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Could not initialize language from platform language: ");
					defaultInterpolatedStringHandler.AppendFormatted(rawLanguage);
					defaultInterpolatedStringHandler.AppendLiteral(" (resolved: ");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral("). Defaulting to english");
					Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					text = "eng";
				}
				else
				{
					Log.Info("Initializing language for the first time from platform locale: " + rawLanguage + " -> " + text, 2);
				}
				SaveManager.Instance.SettingsSave.Language = text;
			}
			else if (!LocManager.Languages.Contains(text))
			{
				Log.Warn("Saved language '" + text + "' is not supported. Defaulting to english", 2);
				text = "eng";
				SaveManager.Instance.SettingsSave.Language = text;
			}
			this.SetLanguage(text);
			this.LoadLocFormatters();
			this.LoadLocCompletionFile();
		}

		/// <summary>
		/// Converts our three-letter code to a CultureInfo.
		/// Most of our language codes are ISO 639-2 language codes, but not all of them.
		/// </summary>
		// Token: 0x06007D1E RID: 32030 RVA: 0x002A9838 File Offset: 0x002A7A38
		private CultureInfo CultureInfoFromThreeLetterCode(string language)
		{
			string text;
			if (language != null)
			{
				int length = language.Length;
				if (length == 3)
				{
					char c = language[0];
					switch (c)
					{
					case 'd':
						if (language == "deu")
						{
							text = "de";
							goto IL_020B;
						}
						break;
					case 'e':
						if (language == "eng")
						{
							text = "en";
							goto IL_020B;
						}
						if (language == "esp")
						{
							text = "es-419";
							goto IL_020B;
						}
						break;
					case 'f':
						if (language == "fra")
						{
							text = "fr";
							goto IL_020B;
						}
						break;
					case 'g':
					case 'h':
					case 'l':
					case 'm':
					case 'n':
					case 'o':
					case 'q':
						break;
					case 'i':
						if (language == "ita")
						{
							text = "it";
							goto IL_020B;
						}
						break;
					case 'j':
						if (language == "jpn")
						{
							text = "ja";
							goto IL_020B;
						}
						break;
					case 'k':
						if (language == "kor")
						{
							text = "ko";
							goto IL_020B;
						}
						break;
					case 'p':
						if (language == "pol")
						{
							text = "pl";
							goto IL_020B;
						}
						if (language == "ptb")
						{
							text = "pt-br";
							goto IL_020B;
						}
						break;
					case 'r':
						if (language == "rus")
						{
							text = "ru";
							goto IL_020B;
						}
						break;
					case 's':
						if (language == "spa")
						{
							text = "es-ES";
							goto IL_020B;
						}
						break;
					case 't':
						if (language == "tha")
						{
							text = "th";
							goto IL_020B;
						}
						if (language == "tur")
						{
							text = "tr";
							goto IL_020B;
						}
						break;
					default:
						if (c == 'z')
						{
							if (language == "zhs")
							{
								text = "zh-hans";
								goto IL_020B;
							}
							if (language == "zht")
							{
								text = "zh-hant";
								goto IL_020B;
							}
						}
						break;
					}
				}
			}
			text = null;
			IL_020B:
			string text2 = text;
			if (text2 == null)
			{
				string text3 = "Language code " + language + " could not be mapped to CultureInfo! Add a new manual mapping";
				Log.Error(text3, 2);
				SentryService.CaptureMessage(text3, 1, null);
				return LocManager.GetCultureInfoSafe("en");
			}
			return LocManager.GetCultureInfoSafe(text2);
		}

		/// <summary>
		/// This is where we explicitly load the localization formatters we need and only the ones we need.
		/// </summary>
		// Token: 0x06007D1F RID: 32031 RVA: 0x002A9A8C File Offset: 0x002A7C8C
		private void LoadLocFormatters()
		{
			LocManager._smartFormatter = new SmartFormatter(null);
			ListFormatter listFormatter = new ListFormatter();
			LocManager._smartFormatter.AddExtensions(new ISource[]
			{
				listFormatter,
				new DictionarySource(),
				new ValueTupleSource(),
				new ReflectionSource(),
				new DefaultSource()
			});
			LocManager._smartFormatter.AddExtensions(new IFormatter[]
			{
				listFormatter,
				new PluralLocalizationFormatter(),
				new ConditionalFormatter(),
				new ChooseFormatter(),
				new SubStringFormatter(),
				new IsMatchFormatter(),
				new LocaleNumberFormatter(),
				new DefaultFormatter(),
				new AbsoluteValueFormatter(),
				new EnergyIconsFormatter(),
				new StarIconsFormatter(),
				new HighlightDifferencesFormatter(),
				new HighlightDifferencesInverseFormatter(),
				new PercentMoreFormatter(),
				new PercentLessFormatter(),
				new ShowIfUpgradedFormatter()
			});
			Smart.Default = LocManager._smartFormatter;
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x002A9B80 File Offset: 0x002A7D80
		private void LoadLocCompletionFile()
		{
			using (FileAccess fileAccess = FileAccess.Open("res://localization/completion.json", 1L))
			{
				if (fileAccess == null)
				{
					throw new LocException("Cannot find language completion file: res://localization/completion.json");
				}
				string asText = fileAccess.GetAsText(false);
				this._languageKeyCount = JsonSerializer.Deserialize<Dictionary<string, int>>(asText, LocManagerSerializerContext.Default.DictionaryStringInt32);
			}
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x002A9BE4 File Offset: 0x002A7DE4
		public float GetLanguageCompletion(string language)
		{
			int num;
			if (!this._languageKeyCount.TryGetValue(language, out num))
			{
				num = 0;
			}
			return (float)num / (float)this._languageKeyCount["eng"];
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x002A9C18 File Offset: 0x002A7E18
		public string SmartFormat(LocString locString, Dictionary<string, object> variables)
		{
			string rawText = locString.GetRawText();
			LocTable table = this.GetTable(locString.LocTable);
			CultureInfo cultureInfo = (table.IsLocalKey(locString.LocEntryKey) ? this.CultureInfo : LocManager._englishCultureInfo);
			string text;
			try
			{
				text = LocManager._smartFormatter.Format(cultureInfo, rawText, new object[] { variables });
			}
			catch (Exception ex) when (ex is FormattingException || ex is ParsingErrors)
			{
				if (TestMode.IsOn)
				{
					throw;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 4);
				defaultInterpolatedStringHandler.AppendLiteral("message=");
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				defaultInterpolatedStringHandler.AppendLiteral("\ntable=");
				defaultInterpolatedStringHandler.AppendFormatted(locString.LocTable);
				defaultInterpolatedStringHandler.AppendLiteral(" key=");
				defaultInterpolatedStringHandler.AppendFormatted(locString.LocEntryKey);
				defaultInterpolatedStringHandler.AppendLiteral(" variables=");
				defaultInterpolatedStringHandler.AppendFormatted(LocManager.ToString(variables));
				string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
				Log.Error("Localization formatting error! " + text2, 2);
				string errorPattern = Regex.Replace(ex.Message.Split('\n', StringSplitOptions.None)[0], " at \\d+$", "");
				SentryService.CaptureException(new LocException(text2), delegate(Scope scope)
				{
					EventLikeExtensions.SetFingerprint(scope, new string[] { "LocException", errorPattern });
				});
				text = rawText;
			}
			return text;
		}

		/// <summary>
		/// Converts text to W/w characters for localization debugging while preserving
		/// template expressions {like:this} and BBCode [tags].
		/// </summary>
		// Token: 0x06007D23 RID: 32035 RVA: 0x002A9D90 File Offset: 0x002A7F90
		private static string ConvertToW(string input)
		{
			int num = 0;
			int num2 = 0;
			char[] array = new char[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				if (c == '{')
				{
					num++;
				}
				else if (c == '[')
				{
					num2++;
				}
				else if (c == '}')
				{
					num--;
				}
				else if (c == ']')
				{
					num2--;
				}
				if (char.IsLetter(c) && num == 0 && num2 == 0)
				{
					array[i] = (char.IsUpper(c) ? 'W' : 'w');
				}
				else
				{
					array[i] = c;
				}
			}
			return new string(array);
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x002A9E23 File Offset: 0x002A8023
		private static string ToString(Dictionary<string, object> variables)
		{
			return "{" + string.Join(",", variables.Select<KeyValuePair<string, object>, string>(delegate(KeyValuePair<string, object> kp)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(kp.Key);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted<object>(kp.Value);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			})) + "}";
		}

		/// <summary>
		/// Updates what language is being used for translation.
		/// Note that, since this is called temporarily sometimes, that it does not update the settings save file.
		/// </summary>
		/// <param name="language">The three-letter code of the language to load. These are based on ISO 639-2, but are not
		/// all actual 639-2 codes because they don't map exactly to what we need.</param>
		// Token: 0x06007D25 RID: 32037 RVA: 0x002A9E64 File Offset: 0x002A8064
		[MemberNotNull(new string[] { "CultureInfo", "StringComparer", "Language" })]
		public void SetLanguage(string language)
		{
			ValueTuple<Dictionary<string, LocTable>, bool, List<LocValidationError>> valueTuple = LocManager.LoadTablesFromPath(language, true);
			Dictionary<string, LocTable> item = valueTuple.Item1;
			bool item2 = valueTuple.Item2;
			List<LocValidationError> item3 = valueTuple.Item3;
			this.SetLanguageInternal(language, item, item2, item3);
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x002A9E98 File Offset: 0x002A8098
		[MemberNotNull(new string[] { "CultureInfo", "StringComparer", "Language" })]
		private void SetLanguageInternal(string language, Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors)
		{
			this._tables = tables;
			this.OverridesActive = overridesActive;
			this.ValidationErrors = validationErrors.AsReadOnly();
			this.Language = language;
			if (this.OverridesActive)
			{
				Log.Info("Localization overrides are active for language '" + language + "'", 2);
			}
			this.CultureInfo = this.CultureInfoFromThreeLetterCode(this.Language);
			this.StringComparer = StringComparer.Create(this.CultureInfo, CompareOptions.None);
			if (TestMode.IsOn)
			{
				Callable.From(new Action(this.TriggerLocaleChange)).CallDeferred(Array.Empty<Variant>());
				return;
			}
			this.TriggerLocaleChange();
		}

		/// <summary>
		/// Overrides the current language with english.
		/// When we upload metrics, we always want to use the english locale. Importantly, we also don't allow overriding of
		/// any sort in this mode (no localization override).
		/// This method also caches the english tables so that we don't have to re-load them the next time we do this.
		/// </summary>
		// Token: 0x06007D27 RID: 32039 RVA: 0x002A9F38 File Offset: 0x002A8138
		public void StartOverridingLanguageAsEnglish()
		{
			if (this._engTables == null)
			{
				if (!this.OverridesActive && this.Language == "eng")
				{
					this._engTables = this._tables;
				}
				else
				{
					ValueTuple<Dictionary<string, LocTable>, bool, List<LocValidationError>> valueTuple = LocManager.LoadTablesFromPath("eng", false);
					this._engTables = valueTuple.Item1;
					bool item = valueTuple.Item2;
					if (item)
					{
						throw new InvalidOperationException("Overrides should never be active when overriding as english!");
					}
				}
			}
			this._stateBeforeOverridingWithEnglish = new LocManager.PreOverrideState
			{
				language = this.Language,
				overridesActive = this.OverridesActive,
				validationErrors = this.ValidationErrors,
				tables = this._tables
			};
			this.SetLanguageInternal("eng", this._engTables, false, new List<LocValidationError>());
		}

		// Token: 0x06007D28 RID: 32040 RVA: 0x002A9FF4 File Offset: 0x002A81F4
		public void StopOverridingLanguageAsEnglish()
		{
			if (this._stateBeforeOverridingWithEnglish == null)
			{
				Log.Error("StopOverridingLanguageAsEnglish called, but we aren't overriding with english!", 2);
				return;
			}
			this.SetLanguageInternal(this._stateBeforeOverridingWithEnglish.language, this._stateBeforeOverridingWithEnglish.tables, this._stateBeforeOverridingWithEnglish.overridesActive, this._stateBeforeOverridingWithEnglish.validationErrors.ToList<LocValidationError>());
			this._stateBeforeOverridingWithEnglish = null;
		}

		// Token: 0x06007D29 RID: 32041 RVA: 0x002AA054 File Offset: 0x002A8254
		[return: TupleElementNames(new string[] { "tables", "overridesActive", "validationErrors" })]
		[return: Nullable(new byte[] { 0, 1, 1, 1, 1, 1 })]
		private static ValueTuple<Dictionary<string, LocTable>, bool, List<LocValidationError>> LoadTablesFromPath(string language, bool allowOverride = true)
		{
			Dictionary<string, LocTable> dictionary = null;
			if (language != "eng")
			{
				dictionary = LocManager.LoadTablesFromPath("eng", true).Item1;
			}
			Dictionary<string, LocTable> dictionary2 = new Dictionary<string, LocTable>();
			List<LocValidationError> list = new List<LocValidationError>();
			string text = LocManager.LocalizationAssetDir + "/" + language;
			bool flag = false;
			bool flag2 = false;
			if (!DirAccess.DirExistsAbsolute(text))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Dir path ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" for language ");
				defaultInterpolatedStringHandler.AppendFormatted(language);
				defaultInterpolatedStringHandler.AppendLiteral(" does not exist, falling back to eng");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				text = LocManager.LocalizationAssetDir + "/eng";
				flag = OS.IsDebugBuild();
			}
			string text2 = ProjectSettings.GlobalizePath("user://localization_override");
			string text3 = Path.Combine(text2, language);
			bool flag3 = DirAccess.DirExistsAbsolute(text3);
			string text4 = Path.Combine(text2, "slaythespire2");
			bool flag4 = DirAccess.DirExistsAbsolute(text4);
			if (flag3)
			{
				Log.Info("Found flat localization override directory: " + text3, 2);
			}
			if (flag4)
			{
				Log.Info("Found Weblate nested override directory: " + text4, 2);
			}
			Log.Info("Loading locale path=" + text, 2);
			IEnumerable<string> enumerable = LocManager.ListLocalizationFiles(text);
			foreach (string text5 in enumerable)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text5);
				string text6 = text + "/" + text5;
				Dictionary<string, string> dictionary3 = LocManager.LoadTable(text6);
				if (flag)
				{
					dictionary3 = dictionary3.ToDictionary<KeyValuePair<string, string>, string, string>((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => LocManager.ConvertToW(kvp.Value));
				}
				LocTable locTable = ((dictionary != null) ? dictionary.GetValueOrDefault(fileNameWithoutExtension) : null);
				LocTable locTable2 = new LocTable(fileNameWithoutExtension, dictionary3, locTable);
				if (!flag && allowOverride)
				{
					if (flag4 && LocManager.TryLoadWeblateNestedOverrides(text2, language, text5, locTable2, list))
					{
						flag2 = true;
					}
					if (flag3)
					{
						string text7 = Path.Combine(text3, text5);
						if (LocManager.TryLoadOverrideFile(text7, locTable2, list))
						{
							flag2 = true;
						}
					}
				}
				foreach (string text8 in ModManager.GetModdedLocTables(language, text5))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Found loc table from mod: ");
					defaultInterpolatedStringHandler.AppendFormatted(language);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(text5);
					defaultInterpolatedStringHandler.AppendLiteral(". Merging with base loc table");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
					Dictionary<string, string> dictionary4 = LocManager.LoadTable(text8);
					if (flag)
					{
						dictionary4 = dictionary4.ToDictionary<KeyValuePair<string, string>, string, string>((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => LocManager.ConvertToW(kvp.Value));
					}
					locTable2.MergeWith(dictionary4);
				}
				dictionary2[fileNameWithoutExtension] = locTable2;
			}
			return new ValueTuple<Dictionary<string, LocTable>, bool, List<LocValidationError>>(dictionary2, flag2, list);
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x002AA3A8 File Offset: 0x002A85A8
		public LocTable GetTable(string name)
		{
			LocTable locTable;
			if (this._tables.TryGetValue(name, out locTable))
			{
				return locTable;
			}
			throw new LocException("The loc table='" + name + "' does not exist!");
		}

		// Token: 0x06007D2B RID: 32043 RVA: 0x002AA3DC File Offset: 0x002A85DC
		private static Dictionary<string, string> LoadTable(string path)
		{
			Dictionary<string, string> dictionary2;
			using (FileAccess fileAccess = FileAccess.Open(path, 1L))
			{
				if (fileAccess == null)
				{
					throw new LocException("Cannot find language file: " + path);
				}
				string asText = fileAccess.GetAsText(false);
				try
				{
					Dictionary<string, string> dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(asText, LocManagerSerializerContext.Default.DictionaryStringString);
					dictionary2 = dictionary;
				}
				catch (Exception ex)
				{
					throw new LocException("Failed to parse language file: " + path, ex);
				}
			}
			return dictionary2;
		}

		// Token: 0x06007D2C RID: 32044 RVA: 0x002AA460 File Offset: 0x002A8660
		private static IEnumerable<string> ListLocalizationFiles(string path)
		{
			IEnumerable<string> enumerable;
			using (DirAccess dirAccess = DirAccess.Open(path))
			{
				if (dirAccess == null)
				{
					throw new LocException("Path does not exist: " + path);
				}
				enumerable = (from s in dirAccess.GetFiles()
					where s.EndsWith(".json")
					select s).ToArray<string>();
			}
			return enumerable;
		}

		/// <summary>
		/// Tries to load and validate an override file, merging valid entries into the LocTable.
		/// </summary>
		/// <param name="overrideFilePath">Absolute path to the override JSON file.</param>
		/// <param name="locTable">The LocTable to merge validated entries into.</param>
		/// <param name="validationErrors">List to append any validation errors to.</param>
		/// <returns>True if the file was found and at least partially loaded, false if not found.</returns>
		// Token: 0x06007D2D RID: 32045 RVA: 0x002AA4D8 File Offset: 0x002A86D8
		private static bool TryLoadOverrideFile(string overrideFilePath, LocTable locTable, List<LocValidationError> validationErrors)
		{
			if (!FileAccess.FileExists(overrideFilePath))
			{
				return false;
			}
			Log.Info("Loading localization override: " + overrideFilePath, 2);
			bool flag;
			try
			{
				Dictionary<string, string> dictionary = LocManager.LoadTable(overrideFilePath);
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					string text;
					if (keyValuePair.Key.StartsWith("EXTENSION.") || LocValidator.ValidateFormatString(keyValuePair.Value, out text))
					{
						dictionary2[keyValuePair.Key] = keyValuePair.Value;
					}
					else
					{
						validationErrors.Add(new LocValidationError(overrideFilePath, keyValuePair.Key, text));
						Log.Warn("[LocValidation] Invalid format in override file: " + overrideFilePath, 2);
						Log.Warn("  Key: " + keyValuePair.Key, 2);
						Log.Warn("  Error: " + text, 2);
					}
				}
				locTable.MergeWith(dictionary2);
				flag = true;
			}
			catch (LocException ex)
			{
				string text2 = "(JSON parsing error)";
				Exception innerException = ex.InnerException;
				validationErrors.Add(new LocValidationError(overrideFilePath, text2, ((innerException != null) ? innerException.Message : null) ?? ex.Message));
				Log.Warn("[LocValidation] Failed to parse override file: " + overrideFilePath, 2);
				string text3 = "  Error: ";
				Exception innerException2 = ex.InnerException;
				Log.Warn(text3 + (((innerException2 != null) ? innerException2.Message : null) ?? ex.Message), 2);
				flag = false;
			}
			return flag;
		}

		/// <summary>
		/// Tries to load overrides from Weblate's nested export structure.
		/// Weblate exports as: {overrideDir}/{project}/{component}/{weblateCode}/{filename}
		/// Example: localization_override/slaythespire2/cards/de/cards.json
		/// </summary>
		/// <param name="globalizedOverrideDir">The globalized path to the override directory.</param>
		/// <param name="language">The game's 3-letter language code (e.g., "deu").</param>
		/// <param name="filename">The localization filename (e.g., "cards.json").</param>
		/// <param name="locTable">The LocTable to merge validated entries into.</param>
		/// <param name="validationErrors">List to append any validation errors to.</param>
		/// <returns>True if a nested override was found and loaded, false otherwise.</returns>
		// Token: 0x06007D2E RID: 32046 RVA: 0x002AA664 File Offset: 0x002A8864
		private unsafe static bool TryLoadWeblateNestedOverrides(string globalizedOverrideDir, string language, string filename, LocTable locTable, List<LocValidationError> validationErrors)
		{
			string text;
			if (!LocManager._gameToWeblateLanguage.TryGetValue(language, out text))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
			<>y__InlineArray5<string> <>y__InlineArray = default(<>y__InlineArray5<string>);
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray5<string>, string>(ref <>y__InlineArray, 0) = globalizedOverrideDir;
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray5<string>, string>(ref <>y__InlineArray, 1) = "slaythespire2";
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray5<string>, string>(ref <>y__InlineArray, 2) = fileNameWithoutExtension;
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray5<string>, string>(ref <>y__InlineArray, 3) = text;
			*<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray5<string>, string>(ref <>y__InlineArray, 4) = filename;
			string text2 = Path.Combine(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray5<string>, string>(in <>y__InlineArray, 5));
			if (LocManager.TryLoadOverrideFile(text2, locTable, validationErrors))
			{
				Log.Info("Found Weblate nested override structure: " + text2, 2);
				return true;
			}
			return false;
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x002AA6F4 File Offset: 0x002A88F4
		public void SubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
		{
			this._localeChangeCallbacks.Add(callback);
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x002AA702 File Offset: 0x002A8902
		public void UnsubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
		{
			this._localeChangeCallbacks.Remove(callback);
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x002AA714 File Offset: 0x002A8914
		private void TriggerLocaleChange()
		{
			TranslationServer.SetLocale(this.CultureInfo.Name);
			foreach (LocManager.LocaleChangeCallback localeChangeCallback in this._localeChangeCallbacks)
			{
				localeChangeCallback();
			}
			GC.Collect();
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x002AA77C File Offset: 0x002A897C
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static LocManager()
		{
			int num = 15;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			int num2 = 0;
			*span[num2] = "eng";
			num2++;
			*span[num2] = "zhs";
			num2++;
			*span[num2] = "zht";
			num2++;
			*span[num2] = "deu";
			num2++;
			*span[num2] = "esp";
			num2++;
			*span[num2] = "fra";
			num2++;
			*span[num2] = "ita";
			num2++;
			*span[num2] = "jpn";
			num2++;
			*span[num2] = "kor";
			num2++;
			*span[num2] = "pol";
			num2++;
			*span[num2] = "ptb";
			num2++;
			*span[num2] = "rus";
			num2++;
			*span[num2] = "spa";
			num2++;
			*span[num2] = "tha";
			num2++;
			*span[num2] = "tur";
			LocManager.Languages = list;
			LocManager._englishCultureInfo = LocManager.GetCultureInfoSafe("en");
		}

		// Token: 0x040029CA RID: 10698
		private Dictionary<string, LocTable> _tables = new Dictionary<string, LocTable>();

		// Token: 0x040029CB RID: 10699
		[Nullable(new byte[] { 2, 1, 1 })]
		private Dictionary<string, LocTable> _engTables;

		// Token: 0x040029CC RID: 10700
		[Nullable(2)]
		private LocManager.PreOverrideState _stateBeforeOverridingWithEnglish;

		// Token: 0x040029CD RID: 10701
		private static SmartFormatter _smartFormatter = null;

		/// <summary>
		/// Weblate project slug used in nested export structure.
		/// </summary>
		// Token: 0x040029CE RID: 10702
		private const string _weblateProjectSlug = "slaythespire2";

		/// <summary>
		/// Maps Weblate language codes to game's 3-letter language codes.
		/// Keep in sync with ci/scripts/weblate.py LANGUAGES dictionary.
		/// </summary>
		// Token: 0x040029CF RID: 10703
		private static readonly Dictionary<string, string> _weblateToGameLanguage = new Dictionary<string, string>
		{
			{ "de", "deu" },
			{ "es", "spa" },
			{ "es_LATAM", "esp" },
			{ "fr", "fra" },
			{ "it", "ita" },
			{ "ja", "jpn" },
			{ "ko", "kor" },
			{ "pl", "pol" },
			{ "pt_BR", "ptb" },
			{ "ru", "rus" },
			{ "th", "tha" },
			{ "tr", "tur" },
			{ "zh_Hans", "zhs" },
			{ "zh_Hant", "zht" }
		};

		/// <summary>
		/// Maps game's 3-letter language codes to Weblate language codes.
		/// </summary>
		// Token: 0x040029D0 RID: 10704
		private static readonly Dictionary<string, string> _gameToWeblateLanguage = LocManager._weblateToGameLanguage.ToDictionary<KeyValuePair<string, string>, string, string>((KeyValuePair<string, string> kvp) => kvp.Value, (KeyValuePair<string, string> kvp) => kvp.Key);

		/// <summary>
		/// Dictionary counting the number of keys in each language set.
		/// We don't want to calculate this dynamically, as it would involve loading a large number of JSON tables, so this
		/// is loaded from a pre-computed JSON file. Re-generate it with the script 'localization/gen_completion.py'.
		/// </summary>
		// Token: 0x040029D1 RID: 10705
		private Dictionary<string, int> _languageKeyCount = new Dictionary<string, int>();

		/// <summary>
		/// User-accessible directory for localization overrides.
		/// Translators can place modified JSON files here to test their translations without rebuilding the game.
		/// Path resolves to: %AppData%/SlayTheSpire2/localization_override/ on Windows.
		///
		/// Override files are validated for SmartFormat syntax errors during load.
		/// Invalid entries are logged to console and skipped (game uses base localization as fallback).
		/// Check console output or game logs for validation errors.
		/// </summary>
		// Token: 0x040029D2 RID: 10706
		public const string locOverrideDir = "user://localization_override";

		// Token: 0x040029D5 RID: 10709
		private readonly List<LocManager.LocaleChangeCallback> _localeChangeCallbacks = new List<LocManager.LocaleChangeCallback>();

		// Token: 0x040029DA RID: 10714
		private static readonly CultureInfo _englishCultureInfo;

		// Token: 0x020022EE RID: 8942
		[Nullable(0)]
		[RequiredMember]
		private class PreOverrideState
		{
			// Token: 0x0600BFCF RID: 49103 RVA: 0x004143CB File Offset: 0x004125CB
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public PreOverrideState()
			{
			}

			// Token: 0x040092BD RID: 37565
			[RequiredMember]
			public string language;

			// Token: 0x040092BE RID: 37566
			public bool overridesActive;

			// Token: 0x040092BF RID: 37567
			[RequiredMember]
			public IReadOnlyList<LocValidationError> validationErrors;

			// Token: 0x040092C0 RID: 37568
			[RequiredMember]
			public Dictionary<string, LocTable> tables;
		}

		// Token: 0x020022EF RID: 8943
		// (Invoke) Token: 0x0600BFD1 RID: 49105
		[NullableContext(0)]
		public delegate void LocaleChangeCallback();
	}
}
