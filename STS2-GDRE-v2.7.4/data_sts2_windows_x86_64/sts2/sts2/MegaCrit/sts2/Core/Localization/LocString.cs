using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Localization
{
	/// <summary>
	/// A Localized String, uses:
	/// https://github.com/axuno/SmartFormat
	/// </summary>
	// Token: 0x02000BBC RID: 3004
	[NullableContext(1)]
	[Nullable(0)]
	public class LocString : IComparable<LocString>
	{
		/// <summary>
		/// A Localized String, uses:
		/// https://github.com/axuno/SmartFormat
		/// </summary>
		// Token: 0x06007D33 RID: 32051 RVA: 0x002AA9E5 File Offset: 0x002A8BE5
		public LocString(string locTable, string locEntryKey)
		{
		}

		// Token: 0x1700229A RID: 8858
		// (get) Token: 0x06007D34 RID: 32052 RVA: 0x002AAA06 File Offset: 0x002A8C06
		[JsonPropertyName("table")]
		public string LocTable
		{
			get
			{
				return this.<locTable>P;
			}
		} = locTable;

		// Token: 0x1700229B RID: 8859
		// (get) Token: 0x06007D35 RID: 32053 RVA: 0x002AAA0E File Offset: 0x002A8C0E
		[JsonPropertyName("key")]
		public string LocEntryKey
		{
			get
			{
				return this.<locEntryKey>P;
			}
		} = locEntryKey;

		// Token: 0x1700229C RID: 8860
		// (get) Token: 0x06007D36 RID: 32054 RVA: 0x002AAA16 File Offset: 0x002A8C16
		[JsonIgnore]
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.LocEntryKey) && string.IsNullOrEmpty(this.LocTable);
			}
		}

		// Token: 0x1700229D RID: 8861
		// (get) Token: 0x06007D37 RID: 32055 RVA: 0x002AAA32 File Offset: 0x002A8C32
		[JsonIgnore]
		public IReadOnlyDictionary<string, object> Variables
		{
			get
			{
				return this._variables;
			}
		}

		// Token: 0x06007D38 RID: 32056 RVA: 0x002AAA3A File Offset: 0x002A8C3A
		public static bool Exists(string table, string key)
		{
			return LocManager.Instance.GetTable(table).HasEntry(key);
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x002AAA4D File Offset: 0x002A8C4D
		[return: Nullable(2)]
		public static LocString GetIfExists(string table, string key)
		{
			if (!LocString.Exists(table, key))
			{
				return null;
			}
			return new LocString(table, key);
		}

		/// <summary>
		/// Returns a formatted string, ie:
		/// "Deal 6 damage."
		/// rather than:
		/// "Deal {Damage:diff()} damage."
		/// </summary>
		// Token: 0x06007D3A RID: 32058 RVA: 0x002AAA61 File Offset: 0x002A8C61
		public string GetFormattedText()
		{
			return LocManager.Instance.SmartFormat(this, this._variables);
		}

		/// <summary>
		/// Returns the raw text in the JSON file, ie:
		/// "Deal {Damage:diff()} damage."
		/// rather than:
		/// "Deal 6 damage."
		/// </summary>
		// Token: 0x06007D3B RID: 32059 RVA: 0x002AAA74 File Offset: 0x002A8C74
		public string GetRawText()
		{
			return LocManager.Instance.GetTable(this.LocTable).GetRawText(this.LocEntryKey);
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x002AAA91 File Offset: 0x002A8C91
		public bool Exists()
		{
			return LocString.Exists(this.LocTable, this.LocEntryKey);
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x002AAAA4 File Offset: 0x002A8CA4
		[NullableContext(2)]
		public static bool IsNullOrWhitespace(LocString locString)
		{
			return locString == null || locString.IsEmpty || string.IsNullOrWhiteSpace(locString.GetRawText());
		}

		// Token: 0x06007D3E RID: 32062 RVA: 0x002AAABE File Offset: 0x002A8CBE
		public static void SubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
		{
			LocManager.Instance.SubscribeToLocaleChange(callback);
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x002AAACB File Offset: 0x002A8CCB
		public static void UnsubscribeToLocaleChange(LocManager.LocaleChangeCallback callback)
		{
			LocManager.Instance.UnsubscribeToLocaleChange(callback);
		}

		// Token: 0x06007D40 RID: 32064 RVA: 0x002AAAD8 File Offset: 0x002A8CD8
		public void Add(DynamicVar dynamicVar)
		{
			this.AddObj(dynamicVar.Name, dynamicVar);
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x002AAAE7 File Offset: 0x002A8CE7
		public void Add(string name, decimal variable)
		{
			this.AddObj(name, variable);
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x002AAAF6 File Offset: 0x002A8CF6
		public void Add(string name, bool variable)
		{
			this.AddObj(name, variable);
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x002AAB05 File Offset: 0x002A8D05
		public void Add(string name, string variable)
		{
			this.AddObj(name, variable);
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x002AAB0F File Offset: 0x002A8D0F
		public void Add(string name, IList<string> variable)
		{
			this.AddObj(name, variable);
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x002AAB19 File Offset: 0x002A8D19
		public void Add(string name, LocString variable)
		{
			this.AddObj(name, variable.GetFormattedText());
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x002AAB28 File Offset: 0x002A8D28
		public void AddObj(string name, object variable)
		{
			ArgumentException.ThrowIfNullOrEmpty(name, "name");
			ArgumentNullException.ThrowIfNull(variable, "variable");
			name = name.Replace(' ', '-');
			if (!this._variables.TryAdd(name, variable))
			{
				this._variables[name] = variable;
			}
		}

		// Token: 0x1700229E RID: 8862
		private object this[string key]
		{
			get
			{
				return this._variables[key];
			}
			set
			{
				this._variables[key] = value;
			}
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x002AAB88 File Offset: 0x002A8D88
		public static LocString KeyPathToLocString(string keyPath)
		{
			string[] array = keyPath.Split('/', StringSplitOptions.None);
			return new LocString(array[0], array[1]);
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x002AABAC File Offset: 0x002A8DAC
		public void AddVariablesFrom(LocString smartDescription)
		{
			foreach (string text in smartDescription._variables.Keys)
			{
				this.AddObj(text, smartDescription[text]);
			}
		}

		/// <summary>
		/// Helper function to grab a random LocString with a given key prefix.
		///
		/// Example:
		/// If there are multiple entries of MAP_POINT_HISTORY.abandon in run_history.json and you want to get a random
		/// LocString:
		///
		/// The JSON would be constructed like:
		/// "MAP_POINT_HISTORY.abandon.0": "{character} had simply given up.",
		/// "MAP_POINT_HISTORY.abandon.1": "{character} was tired...",
		/// "MAP_POINT_HISTORY.abandon.2": "{character} abandoned {pronounPossessive} destiny.",
		///
		/// You can grab a random entry via:
		/// LocString abandonMessage = LocString.GetRandomKey("run_history", "MAP_POINT_HISTORY.abandon");
		/// </summary>
		/// <param name="table">Name of the table to pull the keys from.</param>
		/// <param name="keyPrefix">Prefix for the keys we want.</param>
		/// <param name="rng">Optional rng. Chaotic will be used if not specified.</param>
		/// <returns>Random LocString with the specified key prefix.</returns>
		// Token: 0x06007D4B RID: 32075 RVA: 0x002AAC0C File Offset: 0x002A8E0C
		public static LocString GetRandomWithPrefix(string table, string keyPrefix, [Nullable(2)] Rng rng = null)
		{
			IReadOnlyList<LocString> locStringsWithPrefix = LocManager.Instance.GetTable(table).GetLocStringsWithPrefix(keyPrefix);
			if (rng == null)
			{
				rng = Rng.Chaotic;
			}
			return rng.NextItem<LocString>(locStringsWithPrefix);
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x002AAC3C File Offset: 0x002A8E3C
		[NullableContext(2)]
		public int CompareTo(LocString other)
		{
			if (this.<locTable>P != ((other != null) ? other.LocTable : null))
			{
				return string.Compare(this.LocTable, (other != null) ? other.LocTable : null, StringComparison.Ordinal);
			}
			return string.Compare(this.LocEntryKey, other.LocEntryKey, StringComparison.Ordinal);
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x002AAC8D File Offset: 0x002A8E8D
		public override string ToString()
		{
			return "LocString table " + this.LocTable + " entry " + this.<locEntryKey>P;
		}

		// Token: 0x040029DB RID: 10715
		[CompilerGenerated]
		private string <locTable>P;

		// Token: 0x040029DC RID: 10716
		[CompilerGenerated]
		private string <locEntryKey>P;

		// Token: 0x040029DD RID: 10717
		[JsonIgnore]
		private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
	}
}
