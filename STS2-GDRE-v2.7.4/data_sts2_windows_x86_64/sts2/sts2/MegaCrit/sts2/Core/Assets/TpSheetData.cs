using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// Root structure for TexturePacker .tpsheet files.
	/// </summary>
	// Token: 0x02000D94 RID: 3476
	[NullableContext(1)]
	[Nullable(0)]
	public class TpSheetData
	{
		// Token: 0x17002662 RID: 9826
		// (get) Token: 0x06008BE1 RID: 35809 RVA: 0x002EBF30 File Offset: 0x002EA130
		// (set) Token: 0x06008BE2 RID: 35810 RVA: 0x002EBF38 File Offset: 0x002EA138
		public List<TpSheetTexture> Textures { get; set; } = new List<TpSheetTexture>();
	}
}
