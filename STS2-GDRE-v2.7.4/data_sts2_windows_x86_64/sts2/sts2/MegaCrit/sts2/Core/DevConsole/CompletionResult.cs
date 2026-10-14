using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CDA RID: 3290
	[NullableContext(1)]
	[Nullable(0)]
	public class CompletionResult
	{
		// Token: 0x1700246F RID: 9327
		// (get) Token: 0x06008544 RID: 34116 RVA: 0x002CC230 File Offset: 0x002CA430
		// (set) Token: 0x06008545 RID: 34117 RVA: 0x002CC238 File Offset: 0x002CA438
		public List<string> Candidates { get; set; } = new List<string>();

		// Token: 0x17002470 RID: 9328
		// (get) Token: 0x06008546 RID: 34118 RVA: 0x002CC241 File Offset: 0x002CA441
		// (set) Token: 0x06008547 RID: 34119 RVA: 0x002CC249 File Offset: 0x002CA449
		public string CommonPrefix { get; set; } = string.Empty;

		// Token: 0x17002471 RID: 9329
		// (get) Token: 0x06008548 RID: 34120 RVA: 0x002CC252 File Offset: 0x002CA452
		public bool HasMultipleMatches
		{
			get
			{
				return this.Candidates.Count > 1;
			}
		}

		// Token: 0x17002472 RID: 9330
		// (get) Token: 0x06008549 RID: 34121 RVA: 0x002CC262 File Offset: 0x002CA462
		// (set) Token: 0x0600854A RID: 34122 RVA: 0x002CC26A File Offset: 0x002CA46A
		public CompletionType Type { get; set; }

		// Token: 0x17002473 RID: 9331
		// (get) Token: 0x0600854B RID: 34123 RVA: 0x002CC273 File Offset: 0x002CA473
		// (set) Token: 0x0600854C RID: 34124 RVA: 0x002CC27B File Offset: 0x002CA47B
		public string CommandPrefix { get; set; } = "";

		// Token: 0x17002474 RID: 9332
		// (get) Token: 0x0600854D RID: 34125 RVA: 0x002CC284 File Offset: 0x002CA484
		// (set) Token: 0x0600854E RID: 34126 RVA: 0x002CC28C File Offset: 0x002CA48C
		public int ArgumentIndex { get; set; } = -1;

		// Token: 0x17002475 RID: 9333
		// (get) Token: 0x0600854F RID: 34127 RVA: 0x002CC295 File Offset: 0x002CA495
		// (set) Token: 0x06008550 RID: 34128 RVA: 0x002CC29D File Offset: 0x002CA49D
		public string ArgumentContext { get; set; } = "";
	}
}
