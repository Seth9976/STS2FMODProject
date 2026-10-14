using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from the host to the client as the first message after the client connects.
	/// </summary>
	// Token: 0x02000464 RID: 1124
	public struct InitialGameInfoMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060045C2 RID: 17858 RVA: 0x00217A6C File Offset: 0x00215C6C
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060045C3 RID: 17859 RVA: 0x00217A6F File Offset: 0x00215C6F
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x00217A72 File Offset: 0x00215C72
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x00217A75 File Offset: 0x00215C75
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Returns an InitialGameInfoMessage with the version and idDatabaseHash filled in.
		/// </summary>
		// Token: 0x060045C6 RID: 17862 RVA: 0x00217A78 File Offset: 0x00215C78
		public static InitialGameInfoMessage Basic()
		{
			return new InitialGameInfoMessage
			{
				version = NGame.GetGameVersion(),
				branch = PlatformUtil.GetPlatformBranch(),
				idDatabaseHash = ModelIdSerializationCache.Hash,
				gameplayAffectingMods = ModManager.GetGameplayRelevantModNameList(),
				otherMods = ModManager.GetNonGameplayRelevantModNameList()
			};
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x00217ACC File Offset: 0x00215CCC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.version);
			writer.WriteEnum<PlatformBranch>(this.branch);
			writer.WriteUInt(this.idDatabaseHash, 32);
			writer.WriteEnum<GameMode>(this.gameMode);
			writer.WriteEnum<RunSessionState>(this.sessionState);
			writer.WriteBool(this.connectionFailureReason != null);
			if (this.connectionFailureReason != null)
			{
				writer.WriteEnum<ConnectionFailureReason>(this.connectionFailureReason.Value);
			}
			writer.WriteBool(this.gameplayAffectingMods != null);
			if (this.gameplayAffectingMods != null)
			{
				writer.WriteInt(this.gameplayAffectingMods.Count, 32);
				foreach (string text in this.gameplayAffectingMods)
				{
					writer.WriteString(text);
				}
			}
			writer.WriteBool(this.otherMods != null);
			if (this.otherMods != null)
			{
				writer.WriteInt(this.otherMods.Count, 32);
				foreach (string text2 in this.otherMods)
				{
					writer.WriteString(text2);
				}
			}
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x00217C24 File Offset: 0x00215E24
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.version = reader.ReadString();
			this.branch = reader.ReadEnum<PlatformBranch>();
			this.idDatabaseHash = reader.ReadUInt(32);
			this.gameMode = reader.ReadEnum<GameMode>();
			this.sessionState = reader.ReadEnum<RunSessionState>();
			if (reader.ReadBool())
			{
				this.connectionFailureReason = new ConnectionFailureReason?(reader.ReadEnum<ConnectionFailureReason>());
			}
			if (reader.ReadBool())
			{
				int num = reader.ReadInt(32);
				this.gameplayAffectingMods = new List<string>();
				for (int i = 0; i < num; i++)
				{
					this.gameplayAffectingMods.Add(reader.ReadString());
				}
			}
			if (reader.ReadBool())
			{
				int num2 = reader.ReadInt(32);
				this.otherMods = new List<string>();
				for (int j = 0; j < num2; j++)
				{
					this.otherMods.Add(reader.ReadString());
				}
			}
		}

		/// <summary>
		/// The version of the game the host is running.
		/// </summary>
		// Token: 0x04001B5F RID: 7007
		[Nullable(1)]
		public string version;

		/// <summary>
		/// The steam branch that the host is playing on.
		/// DO NOT compare this to the client's when joining to check versions. The same game version might be on different
		/// steam branches. Only compare <see cref="F:MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.InitialGameInfoMessage.version" /> and only use this for diagnostic purposes.
		/// </summary>
		// Token: 0x04001B60 RID: 7008
		public PlatformBranch branch;

		/// <summary>
		/// A hash of all the IDs in the model database.
		/// </summary>
		// Token: 0x04001B61 RID: 7009
		public uint idDatabaseHash;

		/// <summary>
		/// A list of all the gameplay-affecting mods that the host has installed, if any.
		/// </summary>
		// Token: 0x04001B62 RID: 7010
		[Nullable(new byte[] { 2, 1 })]
		public List<string> gameplayAffectingMods;

		/// <summary>
		/// A list of all the non-gameplay-affecting mods that the host has installed, if any.
		/// </summary>
		// Token: 0x04001B63 RID: 7011
		[Nullable(new byte[] { 2, 1 })]
		public List<string> otherMods;

		/// <summary>
		/// What kind of run this is (standard, daily, custom).
		/// </summary>
		// Token: 0x04001B64 RID: 7012
		public GameMode gameMode;

		/// <summary>
		/// What state the run is currently in.
		/// </summary>
		// Token: 0x04001B65 RID: 7013
		public RunSessionState sessionState;

		/// <summary>
		/// If the host is about to disconnect the client, why.
		/// </summary>
		// Token: 0x04001B66 RID: 7014
		public ConnectionFailureReason? connectionFailureReason;
	}
}
