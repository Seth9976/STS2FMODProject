using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C84 RID: 3204
	[NullableContext(1)]
	[Nullable(0)]
	public struct LobbyPlayer : IPacketSerializable
	{
		// Token: 0x0600835B RID: 33627 RVA: 0x002C4A04 File Offset: 0x002C2C04
		public void Serialize(PacketWriter writer)
		{
			writer.WriteULong(this.id, 64);
			writer.WriteInt(this.slotId, 2);
			writer.WriteModel(this.character);
			writer.Write<SerializableUnlockState>(this.unlockState);
			writer.WriteInt(this.maxMultiplayerAscensionUnlocked, 32);
			writer.WriteBool(this.isReady);
		}

		// Token: 0x0600835C RID: 33628 RVA: 0x002C4A60 File Offset: 0x002C2C60
		public void Deserialize(PacketReader reader)
		{
			this.id = reader.ReadULong(64);
			this.slotId = reader.ReadInt(2);
			this.character = reader.ReadModel<CharacterModel>();
			this.unlockState = reader.Read<SerializableUnlockState>();
			this.maxMultiplayerAscensionUnlocked = reader.ReadInt(32);
			this.isReady = reader.ReadBool();
		}

		// Token: 0x0600835D RID: 33629 RVA: 0x002C4ABC File Offset: 0x002C2CBC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Player ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.id);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(this.character.Id.Entry);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002C8A RID: 11402
		public ulong id;

		// Token: 0x04002C8B RID: 11403
		public int slotId;

		// Token: 0x04002C8C RID: 11404
		public CharacterModel character;

		// Token: 0x04002C8D RID: 11405
		public SerializableUnlockState unlockState;

		// Token: 0x04002C8E RID: 11406
		public int maxMultiplayerAscensionUnlocked;

		// Token: 0x04002C8F RID: 11407
		public bool isReady;
	}
}
