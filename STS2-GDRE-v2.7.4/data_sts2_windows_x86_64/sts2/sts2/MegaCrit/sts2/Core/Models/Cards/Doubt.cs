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
	// Token: 0x02000999 RID: 2457
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Doubt : CardModel
	{
		// Token: 0x060070CB RID: 28875 RVA: 0x0028A085 File Offset: 0x00288285
		public Doubt()
			: base(-1, CardType.Curse, CardRarity.Curse, TargetType.None, true)
		{
		}

		// Token: 0x17001D99 RID: 7577
		// (get) Token: 0x060070CC RID: 28876 RVA: 0x0028A093 File Offset: 0x00288293
		public override int MaxUpgradeLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001D9A RID: 7578
		// (get) Token: 0x060070CD RID: 28877 RVA: 0x0028A096 File Offset: 0x00288296
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
			}
		}

		// Token: 0x17001D9B RID: 7579
		// (get) Token: 0x060070CE RID: 28878 RVA: 0x0028A0A0 File Offset: 0x002882A0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<WeakPower>(null));
			}
		}

		// Token: 0x17001D9C RID: 7580
		// (get) Token: 0x060070CF RID: 28879 RVA: 0x0028A0C0 File Offset: 0x002882C0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<WeakPower>(1m));
			}
		}

		// Token: 0x17001D9D RID: 7581
		// (get) Token: 0x060070D0 RID: 28880 RVA: 0x0028A0D1 File Offset: 0x002882D1
		public override bool HasTurnEndInHandEffect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060070D1 RID: 28881 RVA: 0x0028A0D4 File Offset: 0x002882D4
		protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
		{
			bool alreadyHasWeak = base.Owner.Creature.HasPower<WeakPower>();
			WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Weak.BaseValue, null, this, false);
			PowerModel powerModel = weakPower;
			if (powerModel != null && !alreadyHasWeak)
			{
				powerModel.SkipNextDurationTick = true;
			}
		}
	}
}
