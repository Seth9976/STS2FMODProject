using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Localization
{
	// Token: 0x02000BB8 RID: 3000
	[NullableContext(1)]
	[Nullable(0)]
	public class LanguageCode
	{
		// Token: 0x06007CF3 RID: 31987 RVA: 0x002A9092 File Offset: 0x002A7292
		public LanguageCode(string code)
		{
			if (string.IsNullOrEmpty(code) || code.Length < 2 || code.Length > 3)
			{
				throw new ArgumentException("Language code must be 2 to 3 characters long.");
			}
			this.Code = code.ToLowerInvariant();
		}

		// Token: 0x1700228B RID: 8843
		// (get) Token: 0x06007CF4 RID: 31988 RVA: 0x002A90CB File Offset: 0x002A72CB
		public string Code { get; }

		// Token: 0x06007CF5 RID: 31989 RVA: 0x002A90D4 File Offset: 0x002A72D4
		public bool IsValid()
		{
			int length = this.Code.Length;
			return length >= 2 && length <= 3;
		}

		// Token: 0x06007CF6 RID: 31990 RVA: 0x002A90FA File Offset: 0x002A72FA
		public override string ToString()
		{
			return this.Code;
		}
	}
}
