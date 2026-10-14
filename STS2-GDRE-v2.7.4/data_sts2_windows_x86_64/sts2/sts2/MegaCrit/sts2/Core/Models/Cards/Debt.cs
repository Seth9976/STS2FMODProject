using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x0200097E RID: 2430
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Debt : CardModel
	{
		// Token: 0x06007033 RID: 28723 RVA: 0x00288FFC File Offset: 0x002871FC
		public Debt()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001D4F RID: 7503
		// (get) Token: 0x06007034 RID: 28724 RVA: 0x0028900A File Offset: 0x0028720A
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D50 RID: 7504
		// (get) Token: 0x06007035 RID: 28725 RVA: 0x0028900D File Offset: 0x0028720D
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001D51 RID: 7505
		// (get) Token: 0x06007036 RID: 28726 RVA: 0x00289015 File Offset: 0x00287215
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(10));
			}
		}

		// Token: 0x17001D52 RID: 7506
		// (get) Token: 0x06007037 RID: 28727 RVA: 0x00289023 File Offset: 0x00287223
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007038 RID: 28728 RVA: 0x00289028 File Offset: 0x00287228
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			int num = Mathf.Min(base.DynamicVars.Gold.IntValue, base.Owner.Gold);
			await PlayerCmd.LoseGold(num, base.Owner, GoldLossType.Lost);
		}
	}
}
