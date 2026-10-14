using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BBF RID: 3007
	[NullableContext(1)]
	[Nullable(0)]
	public class LocTable
	{
		// Token: 0x06007D6A RID: 32106 RVA: 0x002AB8D0 File Offset: 0x002A9AD0
		public LocTable(string name, Dictionary<string, string> data, [Nullable(2)] LocTable fallback = null)
		{
			this._name = name;
			this._translations = data;
			this._fallback = fallback;
		}

		// Token: 0x170022A7 RID: 8871
		// (get) Token: 0x06007D6B RID: 32107 RVA: 0x002AB8ED File Offset: 0x002A9AED
		public IEnumerable<string> Keys
		{
			get
			{
				return this._translations.Keys;
			}
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x002AB8FC File Offset: 0x002A9AFC
		public void MergeWith(Dictionary<string, string> otherTable)
		{
			foreach (KeyValuePair<string, string> keyValuePair in otherTable)
			{
				this._translations[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x002AB95C File Offset: 0x002A9B5C
		public LocString GetLocString(string key)
		{
			if (!this._translations.ContainsKey(key))
			{
				LocTable fallback = this._fallback;
				if (fallback == null || !fallback.HasEntry(key))
				{
					throw new LocException("Key=" + key + " not found in table=" + this._name);
				}
			}
			return new LocString(this._name, key);
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x002AB9B4 File Offset: 0x002A9BB4
		public string GetRawText(string key)
		{
			string text;
			if (this._translations.TryGetValue(key, out text))
			{
				return text;
			}
			if (this._fallback != null)
			{
				return this._fallback.GetRawText(key);
			}
			throw new LocException("Key=" + key + " not found in table=" + this._name);
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x002ABA04 File Offset: 0x002A9C04
		public IReadOnlyList<LocString> GetLocStringsWithPrefix(string keyPrefix)
		{
			HashSet<string> hashSet = new HashSet<string>(this._translations.Keys.Where<string>((string k) => k.StartsWith(keyPrefix)));
			if (this._fallback != null)
			{
				IEnumerable<string> keys = this._fallback.Keys;
				Func<string, bool> <>9__2;
				Func<string, bool> func;
				if ((func = <>9__2) == null)
				{
					func = (<>9__2 = (string k) => k.StartsWith(keyPrefix));
				}
				foreach (string text in keys.Where<string>(func))
				{
					hashSet.Add(text);
				}
			}
			return hashSet.Select<string, LocString>((string k) => new LocString(this._name, k)).ToList<LocString>();
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x002ABAD4 File Offset: 0x002A9CD4
		public bool IsLocalKey(string key)
		{
			return this._translations.ContainsKey(key);
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x002ABAE2 File Offset: 0x002A9CE2
		public bool HasEntry(string key)
		{
			if (!this._translations.ContainsKey(key))
			{
				LocTable fallback = this._fallback;
				return fallback != null && fallback.HasEntry(key);
			}
			return true;
		}

		// Token: 0x040029EC RID: 10732
		private readonly string _name;

		// Token: 0x040029ED RID: 10733
		private readonly Dictionary<string, string> _translations;

		// Token: 0x040029EE RID: 10734
		[Nullable(2)]
		private readonly LocTable _fallback;
	}
}
