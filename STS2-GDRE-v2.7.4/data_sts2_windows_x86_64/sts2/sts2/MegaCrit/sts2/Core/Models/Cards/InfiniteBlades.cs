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
	// Token: 0x02000A12 RID: 2578
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class InfiniteBlades : CardModel
	{
		// Token: 0x06007364 RID: 29540 RVA: 0x0028F41C File Offset: 0x0028D61C
		public InfiniteBlades()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001EBB RID: 7867
		// (get) Token: 0x06007365 RID: 29541 RVA: 0x0028F429 File Offset: 0x0028D629
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x0028F438 File Offset: 0x0028D638
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<InfiniteBladesPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this, false);
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x0028F483 File Offset: 0x0028D683
		protected override void OnUpgrade()
		{
			base.AddKeyword(CardKeyword.Innate);
		}
	}
}
