using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	/// <summary>
	/// For use when we are quite certain that no path will ever result in a player choice call deeper in the callstack.
	/// This is currently used in a few scenarios:
	///  - In events, where we're out-of-combat, and so player choice cannot occur.
	///  - In tests where we are certain that calling a cmd will not trigger a player choice.
	///  - In Sleight of Flesh Power's trigger, to prevent having to pass PlayerChoiceContext to PowerCmd.Apply. (This could
	///    change if we find out that player choice can be triggered in that scenario.)
	/// </summary>
	// Token: 0x02000C41 RID: 3137
	[NullableContext(1)]
	[Nullable(0)]
	public class ThrowingPlayerChoiceContext : PlayerChoiceContext
	{
		// Token: 0x17002358 RID: 9048
		// (get) Token: 0x0600816B RID: 33131 RVA: 0x002BE318 File Offset: 0x002BC518
		public override ulong? OwnerId
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x002BE32E File Offset: 0x002BC52E
		public override Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x002BE335 File Offset: 0x002BC535
		public override Task SignalPlayerChoiceEnded()
		{
			throw new NotImplementedException();
		}
	}
}
