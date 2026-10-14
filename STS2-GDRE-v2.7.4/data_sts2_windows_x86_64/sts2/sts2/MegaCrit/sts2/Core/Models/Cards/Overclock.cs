using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A64 RID: 2660
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Overclock : CardModel
	{
		// Token: 0x06007525 RID: 29989 RVA: 0x00292F97 File Offset: 0x00291197
		public Overclock()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F74 RID: 8052
		// (get) Token: 0x06007526 RID: 29990 RVA: 0x00292FA4 File Offset: 0x002911A4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		// Token: 0x17001F75 RID: 8053
		// (get) Token: 0x06007527 RID: 29991 RVA: 0x00292FB1 File Offset: 0x002911B1
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Burn>(false));
			}
		}

		// Token: 0x06007528 RID: 29992 RVA: 0x00292FC0 File Offset: 0x002911C0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(base.Owner.Creature, 1f, false);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(nfireBurningVfx);
			}
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
			CardModel cardModel = base.CombatState.CreateCard<Burn>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06007529 RID: 29993 RVA: 0x0029300B File Offset: 0x0029120B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
