using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.SourceGeneration;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer
{
	// Token: 0x02000C3B RID: 3131
	[GenerateSubtypes]
	public interface INetAction : IPacketSerializable
	{
		// Token: 0x0600813F RID: 33087
		[NullableContext(1)]
		GameAction ToGameAction(Player player);
	}
}
