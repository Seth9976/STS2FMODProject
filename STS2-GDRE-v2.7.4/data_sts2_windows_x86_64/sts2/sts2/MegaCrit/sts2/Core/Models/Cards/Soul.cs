using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AD6 RID: 2774
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Soul : CardModel
	{
		// Token: 0x06007781 RID: 30593 RVA: 0x00297C16 File Offset: 0x00295E16
		public Soul()
			: base(0, CardType.Skill, CardRarity.Token, TargetType.Self, true)
		{
		}

		// Token: 0x17002072 RID: 8306
		// (get) Token: 0x06007782 RID: 30594 RVA: 0x00297C23 File Offset: 0x00295E23
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
			}
		}

		// Token: 0x17002073 RID: 8307
		// (get) Token: 0x06007783 RID: 30595 RVA: 0x00297C2B File Offset: 0x00295E2B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));
			}
		}

		/// <summary>
		/// Create new Soul cards, owned by the specified player, and add them to their hand.
		/// </summary>
		/// <param name="owner">Player who will own the Soul cards.</param>
		/// <param name="amount">Number of Soul cards to create.</param>
		/// <param name="combatState">CombatState to create the Soul cards in.</param>
		/// <param name="creator">The player that created the cards. If null, owner is used.</param>
		/// <returns>The newly-created Soul cards.</returns>
		// Token: 0x06007784 RID: 30596 RVA: 0x00297C38 File Offset: 0x00295E38
		public static async Task<IEnumerable<Soul>> CreateInHand(Player owner, int amount, ICombatState combatState, [Nullable(2)] Player creator = null)
		{
			IEnumerable<Soul> souls = Soul.Create(owner, amount, combatState);
			await CardPileCmd.AddGeneratedCardsToCombat(souls, PileType.Hand, creator ?? owner, CardPilePosition.Bottom);
			return souls;
		}

		/// <summary>
		/// Create new Soul cards, owned by the specified player.
		/// </summary>
		/// <param name="owner">Player who will own the Soul cards.</param>
		/// <param name="amount">Number of Soul cards to create.</param>
		/// <param name="combatState">CombatState to create the Soul cards in.</param>
		/// <returns>The newly-created Soul cards.</returns>
		// Token: 0x06007785 RID: 30597 RVA: 0x00297C94 File Offset: 0x00295E94
		public static IEnumerable<Soul> Create(Player owner, int amount, ICombatState combatState)
		{
			List<Soul> list = new List<Soul>();
			for (int i = 0; i < amount; i++)
			{
				list.Add(combatState.CreateCard<Soul>(owner));
			}
			return list;
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x00297CC4 File Offset: 0x00295EC4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner, false);
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x00297D0F File Offset: 0x00295F0F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Cards.UpgradeValueBy(1m);
		}
	}
}
