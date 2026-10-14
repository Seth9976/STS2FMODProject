using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// For use when we don't care if player choice blocks the task.
	/// In almost all combat-related scenarios, we use HookPlayerChoiceContext to unblock other player queues when a player
	/// choice is made. However, there are several scenarios in which we can't do so:
	///  - Relic AfterObtained callbacks. The only way these can occur during combat is via console.
	///  - Enemy turn. When it's the enemy turn, we want to block the rest of the enemy turn from executing until after the
	///    player has finished making their choice. This should only happen in very rare circumstances, like when Centennial
	///    Puzzle triggers, drawing a Seeking Strike which is then autoplayed via Hellraiser.
	///
	/// In all other circumstances, you should prefer either HookPlayerChoiceContext or ThrowingPlayerChoiceContext,
	/// depending on the situation.
	/// </summary>
	// Token: 0x02000C37 RID: 3127
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockingPlayerChoiceContext : PlayerChoiceContext
	{
		// Token: 0x17002344 RID: 9028
		// (get) Token: 0x06008118 RID: 33048 RVA: 0x002BD028 File Offset: 0x002BB228
		public override ulong? OwnerId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06008119 RID: 33049 RVA: 0x002BD03E File Offset: 0x002BB23E
		public override Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
		{
			return Task.CompletedTask;
		}

		// Token: 0x0600811A RID: 33050 RVA: 0x002BD045 File Offset: 0x002BB245
		public override Task SignalPlayerChoiceEnded()
		{
			return Task.CompletedTask;
		}
	}
}
