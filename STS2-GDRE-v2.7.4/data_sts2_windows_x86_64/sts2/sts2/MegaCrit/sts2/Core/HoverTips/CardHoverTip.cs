using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.HoverTips
{
	// Token: 0x02000BF7 RID: 3063
	[NullableContext(1)]
	[Nullable(0)]
	public class CardHoverTip : IHoverTip
	{
		// Token: 0x170022F0 RID: 8944
		// (get) Token: 0x06007E91 RID: 32401 RVA: 0x002AE439 File Offset: 0x002AC639
		public CardModel Card { get; }

		// Token: 0x170022F1 RID: 8945
		// (get) Token: 0x06007E92 RID: 32402 RVA: 0x002AE441 File Offset: 0x002AC641
		public string Id { get; }

		// Token: 0x170022F2 RID: 8946
		// (get) Token: 0x06007E93 RID: 32403 RVA: 0x002AE449 File Offset: 0x002AC649
		public bool IsSmart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170022F3 RID: 8947
		// (get) Token: 0x06007E94 RID: 32404 RVA: 0x002AE44C File Offset: 0x002AC64C
		public bool IsDebuff
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170022F4 RID: 8948
		// (get) Token: 0x06007E95 RID: 32405 RVA: 0x002AE44F File Offset: 0x002AC64F
		public bool IsInstanced
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170022F5 RID: 8949
		// (get) Token: 0x06007E96 RID: 32406 RVA: 0x002AE452 File Offset: 0x002AC652
		public AbstractModel CanonicalModel
		{
			get
			{
				return this.Card.CanonicalInstance;
			}
		}

		// Token: 0x06007E97 RID: 32407 RVA: 0x002AE460 File Offset: 0x002AC660
		public CardHoverTip(CardModel card)
		{
			if (!card.IsMutable)
			{
				this.Card = card.ToMutable();
			}
			else
			{
				this.Card = card;
			}
			this.Id = this.Card.Id.ToString();
			if (card.IsUpgraded)
			{
				this.Id += "+";
			}
		}
	}
}
