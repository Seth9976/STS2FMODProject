using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AC0 RID: 2752
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Shame : CardModel
	{
		// Token: 0x06007704 RID: 30468 RVA: 0x00296D71 File Offset: 0x00294F71
		public Shame()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17002036 RID: 8246
		// (get) Token: 0x06007705 RID: 30469 RVA: 0x00296D7F File Offset: 0x00294F7F
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17002037 RID: 8247
		// (get) Token: 0x06007706 RID: 30470 RVA: 0x00296D82 File Offset: 0x00294F82
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17002038 RID: 8248
		// (get) Token: 0x06007707 RID: 30471 RVA: 0x00296D8C File Offset: 0x00294F8C
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FrailPower>(null));
			}
		}

		// Token: 0x17002039 RID: 8249
		// (get) Token: 0x06007708 RID: 30472 RVA: 0x00296DAC File Offset: 0x00294FAC
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Frail", 1m));
			}
		}

		// Token: 0x1700203A RID: 8250
		// (get) Token: 0x06007709 RID: 30473 RVA: 0x00296DC2 File Offset: 0x00294FC2
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x00296DC8 File Offset: 0x00294FC8
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			bool alreadyHasFrail = base.Owner.Creature.HasPower<FrailPower>();
			FrailPower frailPower = await PowerCmd.Apply<FrailPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Frail"].BaseValue, null, this, false);
			PowerModel powerModel = frailPower;
			if (powerModel != null && !alreadyHasFrail)
			{
				powerModel.SkipNextDurationTick = true;
			}
		}

		// Token: 0x040028BB RID: 10427
		private const string _frailKey = "Frail";
	}
}
