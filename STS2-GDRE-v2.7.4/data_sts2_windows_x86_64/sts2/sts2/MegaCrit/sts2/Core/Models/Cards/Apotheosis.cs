using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000900 RID: 2304
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Apotheosis : CardModel
	{
		// Token: 0x06006DB1 RID: 28081 RVA: 0x00283D1F File Offset: 0x00281F1F
		public Apotheosis()
			: base(2, CardType.Skill, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x06006DB2 RID: 28082 RVA: 0x00283D2C File Offset: 0x00281F2C
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlyArray<CardKeyword>(new CardKeyword[]
				{
					CardKeyword.Exhaust,
					CardKeyword.Innate
				});
			}
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x00283D44 File Offset: 0x00281F44
		protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			foreach (CardModel cardModel in base.Owner.PlayerCombatState.AllCards)
			{
				if (cardModel != this && cardModel.IsUpgradable)
				{
					CardCmd.Upgrade(cardModel, CardPreviewStyle.HorizontalLayout);
				}
			}
			return Task.CompletedTask;
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x00283DAC File Offset: 0x00281FAC
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
