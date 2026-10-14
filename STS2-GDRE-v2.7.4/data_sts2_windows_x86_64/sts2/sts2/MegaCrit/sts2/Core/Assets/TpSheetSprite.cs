using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// Individual sprite within a texture page.
	/// </summary>
	// Token: 0x02000D97 RID: 3479
	[NullableContext(1)]
	[Nullable(0)]
	public class TpSheetSprite
	{
		// Token: 0x17002668 RID: 9832
		// (get) Token: 0x06008BF0 RID: 35824 RVA: 0x002EBFDA File Offset: 0x002EA1DA
		// (set) Token: 0x06008BF1 RID: 35825 RVA: 0x002EBFE2 File Offset: 0x002EA1E2
		public string Filename { get; set; } = "";

		// Token: 0x17002669 RID: 9833
		// (get) Token: 0x06008BF2 RID: 35826 RVA: 0x002EBFEB File Offset: 0x002EA1EB
		// (set) Token: 0x06008BF3 RID: 35827 RVA: 0x002EBFF3 File Offset: 0x002EA1F3
		public TpSheetRect Region { get; set; } = new TpSheetRect();

		// Token: 0x1700266A RID: 9834
		// (get) Token: 0x06008BF4 RID: 35828 RVA: 0x002EBFFC File Offset: 0x002EA1FC
		// (set) Token: 0x06008BF5 RID: 35829 RVA: 0x002EC004 File Offset: 0x002EA204
		public TpSheetRect Margin { get; set; } = new TpSheetRect();
	}
}
