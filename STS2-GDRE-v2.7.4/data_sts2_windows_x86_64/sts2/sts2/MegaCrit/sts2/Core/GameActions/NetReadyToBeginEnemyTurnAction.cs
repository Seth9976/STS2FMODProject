using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.GameActions
{
	// Token: 0x02000C2B RID: 3115
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetReadyToBeginEnemyTurnAction : INetAction, IPacketSerializable
	{
		// Token: 0x060080A7 RID: 32935 RVA: 0x002B9CD7 File Offset: 0x002B7ED7
		public GameAction ToGameAction(Player player)
		{
			return new ReadyToBeginEnemyTurnAction(player, null);
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x002B9CE0 File Offset: 0x002B7EE0
		public void Serialize(PacketWriter serializer)
		{
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x002B9CE2 File Offset: 0x002B7EE2
		public void Deserialize(PacketReader deserializer)
		{
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x002B9CE4 File Offset: 0x002B7EE4
		public override string ToString()
		{
			return "NetReadyToBeginEnemyTurnAction";
		}
	}
}
