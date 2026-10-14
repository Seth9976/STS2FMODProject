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
	// Token: 0x020009AB RID: 2475
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Entropy : CardModel
	{
		// Token: 0x0600712D RID: 28973 RVA: 0x0028AB69 File Offset: 0x00288D69
		public Entropy()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001DC7 RID: 7623
		// (get) Token: 0x0600712E RID: 28974 RVA: 0x0028AB76 File Offset: 0x00288D76
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17001DC8 RID: 7624
		// (get) Token: 0x0600712F RID: 28975 RVA: 0x0028AB88 File Offset: 0x00288D88
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
			}
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x0028AB98 File Offset: 0x00288D98
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PowerCmd.Apply<EntropyPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.IntValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x0028ABE3 File Offset: 0x00288DE3
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
