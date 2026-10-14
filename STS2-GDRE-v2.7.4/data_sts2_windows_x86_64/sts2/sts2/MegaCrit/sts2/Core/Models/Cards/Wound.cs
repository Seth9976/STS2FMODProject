using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B3F RID: 2879
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Wound : CardModel
	{
		// Token: 0x060079C5 RID: 31173 RVA: 0x0029C6B3 File Offset: 0x0029A8B3
		public Wound()
			: base(-1, CardType.Status, CardRarity.Status, TargetType.None, true)
		{
		}

		// Token: 0x17002157 RID: 8535
		// (get) Token: 0x060079C6 RID: 31174 RVA: 0x0029C6C0 File Offset: 0x0029A8C0
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002158 RID: 8536
		// (get) Token: 0x060079C7 RID: 31175 RVA: 0x0029C6C3 File Offset: 0x0029A8C3
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}
	}
}
