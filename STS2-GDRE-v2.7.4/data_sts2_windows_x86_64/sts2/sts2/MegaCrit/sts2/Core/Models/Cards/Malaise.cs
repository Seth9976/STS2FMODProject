using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A32 RID: 2610
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Malaise : CardModel
	{
		// Token: 0x06007418 RID: 29720 RVA: 0x00290D13 File Offset: 0x0028EF13
		public Malaise()
			: base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001F01 RID: 7937
		// (get) Token: 0x06007419 RID: 29721 RVA: 0x00290D20 File Offset: 0x0028EF20
		public override TargetType TargetType
		{
			get
			{
				return TargetType.AnyEnemy;
			}
		}

		// Token: 0x17001F02 RID: 7938
		// (get) Token: 0x0600741A RID: 29722 RVA: 0x00290D23 File Offset: 0x0028EF23
		protected override bool HasEnergyCostX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F03 RID: 7939
		// (get) Token: 0x0600741B RID: 29723 RVA: 0x00290D28 File Offset: 0x0028EF28
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromPower<StrengthPower>(null),
					HoverTipFactory.FromPower<WeakPower>(null)
				});
			}
		}

		// Token: 0x17001F04 RID: 7940
		// (get) Token: 0x0600741C RID: 29724 RVA: 0x00290D62 File Offset: 0x0028EF62
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x0600741D RID: 29725 RVA: 0x00290D6C File Offset: 0x0028EF6C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			int powerAmount = base.ResolveEnergyXValue();
			if (base.IsUpgraded)
			{
				int num = powerAmount;
				powerAmount = num + 1;
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -powerAmount, base.Owner.Creature, this, false);
			await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, powerAmount, base.Owner.Creature, this, false);
		}
	}
}
