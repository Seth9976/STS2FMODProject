using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Runs
{
	/// <summary>
	/// A scope in which cards exist.
	/// </summary>
	// Token: 0x02000105 RID: 261
	[NullableContext(1)]
	public interface ICardScope
	{
		/// <summary>
		/// Create a card within this scope.
		/// </summary>
		/// <param name="owner">Owner of the new card.</param>
		/// <typeparam name="T">Type of the new card.</typeparam>
		/// <returns>Newly-created card.</returns>
		// Token: 0x06000B3D RID: 2877
		T CreateCard<[Nullable(0)] T>(Player owner) where T : CardModel;

		/// <summary>
		/// Create a card within this scope, based on a canonical card.
		/// </summary>
		/// <param name="canonicalCard">Canonical version of the new card.</param>
		/// <param name="owner">Owner of the new card.</param>
		/// <returns>Newly-created card.</returns>
		// Token: 0x06000B3E RID: 2878
		CardModel CreateCard(CardModel canonicalCard, Player owner);

		/// <summary>
		/// WARNING: If you're specifically intending to create a clone in combat for an effect like <see cref="T:MegaCrit.Sts2.Core.Models.Cards.DualWield" />,
		/// you should use <see cref="M:MegaCrit.Sts2.Core.Models.CardModel.CreateClone" /> instead.
		/// Create a clone of a mutable card within this scope.
		/// The card must already have an owner.
		/// </summary>
		// Token: 0x06000B3F RID: 2879
		CardModel CloneCard(CardModel mutableCard);

		/// <summary>
		/// Add a mutable card to this scope.
		/// The card must not have an owner yet.
		/// </summary>
		/// <param name="mutableCard">Mutable card to add.</param>
		/// <param name="owner">The player who should own this card.</param>
		// Token: 0x06000B40 RID: 2880
		void AddCard(CardModel mutableCard, Player owner);

		/// <summary>
		/// Remove a card from this scope.
		/// Be careful with this! Outside of tests, cards should really not be moving around between scopes.
		/// </summary>
		// Token: 0x06000B41 RID: 2881
		void RemoveCard(CardModel card);

		/// <summary>
		/// THIS IS TEMPORARY AND SHOULD ONLY BE USED IN TESTS AND DEV CONSOLE COMMANDS.
		/// </summary>
		// Token: 0x06000B42 RID: 2882 RVA: 0x0004C520 File Offset: 0x0004A720
		public static ICardScope DebugOnlyGet(CardScope scope)
		{
			ICardScope cardScope;
			if (scope != CardScope.Run)
			{
				if (scope != CardScope.Combat)
				{
					throw new ArgumentOutOfRangeException("scope", scope, null);
				}
				cardScope = CombatManager.Instance.DebugOnlyGetState();
			}
			else
			{
				cardScope = RunManager.Instance.DebugOnlyGetState();
			}
			return cardScope;
		}
	}
}
