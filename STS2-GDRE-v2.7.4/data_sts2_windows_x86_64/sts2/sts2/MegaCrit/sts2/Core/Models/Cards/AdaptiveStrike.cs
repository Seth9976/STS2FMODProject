using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020008F5 RID: 2293
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AdaptiveStrike : CardModel
	{
		// Token: 0x06006D7B RID: 28027 RVA: 0x002836F6 File Offset: 0x002818F6
		public AdaptiveStrike()
			: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x06006D7C RID: 28028 RVA: 0x00283703 File Offset: 0x00281903
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag> { CardTag.Strike };
			}
		}

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x06006D7D RID: 28029 RVA: 0x00283712 File Offset: 0x00281912
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DamageVar(18m, ValueProp.Move));
			}
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x00283728 File Offset: 0x00281928
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
				.WithHitFx("vfx/vfx_attack_slash", null, null)
				.Execute(choiceContext);
			CardModel cardModel = base.CreateClone();
			cardModel.EnergyCost.SetThisCombat(0, false);
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.5f, CardPreviewStyle.HorizontalLayout);
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x0028377B File Offset: 0x0028197B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Damage.UpgradeValueBy(5m);
		}
	}
}
