using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200093D RID: 2365
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ByrdonisEgg : CardModel
	{
		// Token: 0x06006EEE RID: 28398 RVA: 0x002866B0 File Offset: 0x002848B0
		public ByrdonisEgg()
			: base(-1, CardType.Quest, CardRarity.Quest, TargetType.None, true)
		{
		}

		// Token: 0x17001CC9 RID: 7369
		// (get) Token: 0x06006EEF RID: 28399 RVA: 0x002866BE File Offset: 0x002848BE
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001CCA RID: 7370
		// (get) Token: 0x06006EF0 RID: 28400 RVA: 0x002866C1 File Offset: 0x002848C1
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x002866C9 File Offset: 0x002848C9
		public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
		{
			if (player != base.Owner)
			{
				return false;
			}
			options.Add(new HatchRestSiteOption(player));
			return true;
		}
	}
}
