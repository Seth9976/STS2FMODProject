using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.DevConsole
{
	// Token: 0x02000CD8 RID: 3288
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetConsoleCmdGameAction : INetAction, IPacketSerializable
	{
		// Token: 0x06008540 RID: 34112 RVA: 0x002CC18A File Offset: 0x002CA38A
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.cmd);
			writer.WriteBool(this.inCombat);
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x002CC1A4 File Offset: 0x002CA3A4
		public void Deserialize(PacketReader reader)
		{
			this.cmd = reader.ReadString();
			this.inCombat = reader.ReadBool();
		}

		// Token: 0x06008542 RID: 34114 RVA: 0x002CC1BE File Offset: 0x002CA3BE
		public GameAction ToGameAction(Player player)
		{
			return new ConsoleCmdGameAction(player, this.cmd, this.inCombat);
		}

		// Token: 0x06008543 RID: 34115 RVA: 0x002CC1D4 File Offset: 0x002CA3D4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 3);
			defaultInterpolatedStringHandler.AppendFormatted("NetConsoleCmdGameAction");
			defaultInterpolatedStringHandler.AppendLiteral(" cmd ");
			defaultInterpolatedStringHandler.AppendFormatted(this.cmd);
			defaultInterpolatedStringHandler.AppendLiteral(" inCombat ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.inCombat);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002E4C RID: 11852
		public string cmd;

		// Token: 0x04002E4D RID: 11853
		public bool inCombat;
	}
}
