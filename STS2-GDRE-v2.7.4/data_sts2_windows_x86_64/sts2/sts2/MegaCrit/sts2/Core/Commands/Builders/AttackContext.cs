using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Commands.Builders
{
	/// <summary>
	/// A context for grouping multiple damage calls as a single attack for hook purposes.
	/// Use with C#'s `await using` statement to automatically call BeforeAttack and AfterAttack hooks.
	/// </summary>
	// Token: 0x02000D3E RID: 3390
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AttackContext : IAsyncDisposable
	{
		/// <summary>
		/// Private constructor. Use <see cref="M:MegaCrit.Sts2.Core.Commands.Builders.AttackContext.CreateAsync(MegaCrit.Sts2.Core.Combat.ICombatState,MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext,MegaCrit.Sts2.Core.Entities.Cards.CardPlay)" /> to create instances.
		/// </summary>
		// Token: 0x060088BF RID: 35007 RVA: 0x002E0531 File Offset: 0x002DE731
		private AttackContext(ICombatState combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			this._combatState = combatState;
			this._choiceContext = choiceContext;
			this._attackCommand = new AttackCommand(0m).FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(combatState);
		}

		/// <summary>
		/// Creates a new AttackContext and calls the BeforeAttack hook.
		/// </summary>
		/// <param name="combatState">The current combat state</param>
		/// <param name="choiceContext">The context that is signalled in the event of a player choice.</param>
		/// <param name="cardPlay">The card play that is the source of this attack</param>
		/// <returns>An initialized AttackContext ready for use with await using</returns>
		// Token: 0x060088C0 RID: 35008 RVA: 0x002E056C File Offset: 0x002DE76C
		public static async Task<AttackContext> CreateAsync(ICombatState combatState, PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			AttackContext context = new AttackContext(combatState, choiceContext, cardPlay);
			await Hook.BeforeAttack(combatState, context._attackCommand);
			return context;
		}

		/// <summary>
		/// Add to the list of hits that have been done within the context of this attack.
		/// </summary>
		/// <param name="results">DamageResults from the hit.</param>
		// Token: 0x060088C1 RID: 35009 RVA: 0x002E05BF File Offset: 0x002DE7BF
		public void AddHit(IEnumerable<DamageResult> results)
		{
			this._attackCommand.IncrementHitsInternal();
			this._attackCommand.AddResultsInternal(results);
		}

		/// <summary>
		/// Disposes the context and calls the AfterAttack hook.
		/// This is automatically called when exiting an await using block.
		/// </summary>
		// Token: 0x060088C2 RID: 35010 RVA: 0x002E05D8 File Offset: 0x002DE7D8
		public async ValueTask DisposeAsync()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				try
				{
					await Hook.AfterAttack(this._combatState, this._choiceContext, this._attackCommand);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), 2);
				}
			}
		}

		// Token: 0x04002F9E RID: 12190
		private readonly ICombatState _combatState;

		// Token: 0x04002F9F RID: 12191
		private readonly PlayerChoiceContext _choiceContext;

		// Token: 0x04002FA0 RID: 12192
		private readonly AttackCommand _attackCommand;

		// Token: 0x04002FA1 RID: 12193
		private bool _disposed;
	}
}
