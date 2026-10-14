using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000B22 RID: 2850
	public class Tutor : CardModel
	{
		// Token: 0x06007928 RID: 31016 RVA: 0x0029B44E File Offset: 0x0029964E
		public Tutor()
			: base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17002114 RID: 8468
		// (get) Token: 0x06007929 RID: 31017 RVA: 0x0029B45B File Offset: 0x0029965B
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x0600792A RID: 31018 RVA: 0x0029B460 File Offset: 0x00299660
		[NullableContext(1)]
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			Player player = cardPlay.Target.Player;
			IEnumerable<CardModel> enumerable = await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(player), player, new CardSelectorPrefs(base.SelectionScreenPrompt, 1), null);
			IEnumerable<CardModel> enumerable2 = enumerable;
			CardModel cardModel = enumerable2.FirstOrDefault<CardModel>();
			if (cardModel != null)
			{
				await CardPileCmd.Add(cardModel, PileType.Hand, CardPilePosition.Bottom, null, false);
			}
		}

		// Token: 0x0600792B RID: 31019 RVA: 0x0029B4B3 File Offset: 0x002996B3
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
