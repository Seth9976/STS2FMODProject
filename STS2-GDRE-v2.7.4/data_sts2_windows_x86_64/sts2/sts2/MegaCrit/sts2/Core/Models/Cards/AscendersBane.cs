using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000904 RID: 2308
	[NullableContext(1)]
	[Nullable(0)]
	public class AscendersBane : CardModel
	{
		// Token: 0x06006DC4 RID: 28100 RVA: 0x00283F70 File Offset: 0x00282170
		public AscendersBane()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001C4A RID: 7242
		// (get) Token: 0x06006DC5 RID: 28101 RVA: 0x00283F7E File Offset: 0x0028217E
		public override bool CanBeGeneratedByModifiers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C4B RID: 7243
		// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x00283F81 File Offset: 0x00282181
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x06006DC7 RID: 28103 RVA: 0x00283F84 File Offset: 0x00282184
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x06006DC8 RID: 28104 RVA: 0x00283F87 File Offset: 0x00282187
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Eternal,
					CardKeyword.Unplayable,
					CardKeyword.Ethereal
				});
			}
		}
	}
}
