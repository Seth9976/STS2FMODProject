using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// A single texture page in the atlas (multi-page atlases have multiple entries).
	/// </summary>
	// Token: 0x02000D95 RID: 3477
	[NullableContext(1)]
	[Nullable(0)]
	public class TpSheetTexture
	{
		// Token: 0x17002663 RID: 9827
		// (get) Token: 0x06008BE4 RID: 35812 RVA: 0x002EBF54 File Offset: 0x002EA154
		// (set) Token: 0x06008BE5 RID: 35813 RVA: 0x002EBF5C File Offset: 0x002EA15C
		public string Image { get; set; } = "";

		// Token: 0x17002664 RID: 9828
		// (get) Token: 0x06008BE6 RID: 35814 RVA: 0x002EBF65 File Offset: 0x002EA165
		// (set) Token: 0x06008BE7 RID: 35815 RVA: 0x002EBF6D File Offset: 0x002EA16D
		public TpSheetSize Size { get; set; } = new TpSheetSize();

		// Token: 0x17002665 RID: 9829
		// (get) Token: 0x06008BE8 RID: 35816 RVA: 0x002EBF76 File Offset: 0x002EA176
		// (set) Token: 0x06008BE9 RID: 35817 RVA: 0x002EBF7E File Offset: 0x002EA17E
		public List<TpSheetSprite> Sprites { get; set; } = new List<TpSheetSprite>();
	}
}
