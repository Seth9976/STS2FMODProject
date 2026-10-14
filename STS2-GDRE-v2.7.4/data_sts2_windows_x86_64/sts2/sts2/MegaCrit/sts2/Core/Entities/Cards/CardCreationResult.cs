using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Entities.Cards
{
	// Token: 0x02000CA9 RID: 3241
	[NullableContext(1)]
	[Nullable(0)]
	public class CardCreationResult
	{
		// Token: 0x17002430 RID: 9264
		// (get) Token: 0x06008473 RID: 33907 RVA: 0x002C9DD4 File Offset: 0x002C7FD4
		public CardModel Card
		{
			get
			{
				return this._modifiedCard ?? this.originalCard;
			}
		}

		// Token: 0x17002431 RID: 9265
		// (get) Token: 0x06008474 RID: 33908 RVA: 0x002C9DE6 File Offset: 0x002C7FE6
		public IEnumerable<RelicModel> ModifyingRelics
		{
			get
			{
				return this._modifyingRelics;
			}
		}

		// Token: 0x17002432 RID: 9266
		// (get) Token: 0x06008475 RID: 33909 RVA: 0x002C9DEE File Offset: 0x002C7FEE
		public bool HasBeenModified
		{
			get
			{
				return this._modifiedCard != null;
			}
		}

		// Token: 0x06008476 RID: 33910 RVA: 0x002C9DF9 File Offset: 0x002C7FF9
		public CardCreationResult(CardModel originalCard)
		{
			this.originalCard = originalCard;
		}

		// Token: 0x06008477 RID: 33911 RVA: 0x002C9E13 File Offset: 0x002C8013
		public void ModifyCard(CardModel card, RelicModel modifyingRelic)
		{
			this._modifiedCard = card;
			this._modifyingRelics.Add(modifyingRelic);
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x002C9E28 File Offset: 0x002C8028
		public void ModifyCard(CardModel card)
		{
			this._modifiedCard = card;
		}

		// Token: 0x04002D75 RID: 11637
		public readonly CardModel originalCard;

		// Token: 0x04002D76 RID: 11638
		[Nullable(2)]
		private CardModel _modifiedCard;

		// Token: 0x04002D77 RID: 11639
		private readonly List<RelicModel> _modifyingRelics = new List<RelicModel>();
	}
}
