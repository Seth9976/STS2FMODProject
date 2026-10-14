using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B21 RID: 2849
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Turbo : CardModel
	{
		// Token: 0x06007923 RID: 31011 RVA: 0x0029B3BB File Offset: 0x002995BB
		public Turbo()
			: base(0, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17002112 RID: 8466
		// (get) Token: 0x06007924 RID: 31012 RVA: 0x0029B3C8 File Offset: 0x002995C8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));
			}
		}

		// Token: 0x17002113 RID: 8467
		// (get) Token: 0x06007925 RID: 31013 RVA: 0x0029B3D5 File Offset: 0x002995D5
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					base.EnergyHoverTip,
					HoverTipFactory.FromCard<Void>(false)
				});
			}
		}

		// Token: 0x06007926 RID: 31014 RVA: 0x0029B3F4 File Offset: 0x002995F4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
			CardModel cardModel = base.CombatState.CreateCard<Void>(base.Owner);
			CardPileAddResult cardPileAddResult = await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Discard, base.Owner, CardPilePosition.Bottom);
			CardCmd.PreviewCardPileAdd(cardPileAddResult, 1.2f, CardPreviewStyle.HorizontalLayout);
			await Cmd.Wait(0.5f, false);
		}

		// Token: 0x06007927 RID: 31015 RVA: 0x0029B437 File Offset: 0x00299637
		protected override void OnUpgrade()
		{
			base.DynamicVars.Energy.UpgradeValueBy(1m);
		}
	}
}
