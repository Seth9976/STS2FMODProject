using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby
{
	/// <summary>
	/// Sent from the host to a newly joined client in response to their ClientLobbyJoinRequestMessage.
	/// On other clients, PlayerJoinedMessage is sent.
	/// </summary>
	// Token: 0x02000461 RID: 1121
	[NullableContext(1)]
	[Nullable(0)]
	public struct ClientLobbyJoinResponseMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x0021787B File Offset: 0x00215A7B
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x0021787E File Offset: 0x00215A7E
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060045B1 RID: 17841 RVA: 0x00217881 File Offset: 0x00215A81
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.Info;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060045B2 RID: 17842 RVA: 0x00217884 File Offset: 0x00215A84
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x00217888 File Offset: 0x00215A88
		public void Serialize(PacketWriter writer)
		{
			if (this.playersInLobby == null)
			{
				throw new InvalidOperationException("Tried to serialize ClientSlotGrantedMessage with null list!");
			}
			writer.WriteList<LobbyPlayer>(this.playersInLobby, 3);
			writer.WriteBool(this.dailyTime != null);
			if (this.dailyTime != null)
			{
				writer.Write<TimeServerResult>(this.dailyTime.Value);
			}
			writer.WriteBool(this.seed != null);
			if (this.seed != null)
			{
				writer.WriteString(this.seed);
			}
			writer.WriteInt(this.ascension, 5);
			writer.WriteList<SerializableModifier>(this.modifiers, 32);
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00217924 File Offset: 0x00215B24
		public void Deserialize(PacketReader reader)
		{
			this.playersInLobby = reader.ReadList<LobbyPlayer>(3);
			if (reader.ReadBool())
			{
				this.dailyTime = new TimeServerResult?(reader.Read<TimeServerResult>());
			}
			if (reader.ReadBool())
			{
				this.seed = reader.ReadString();
			}
			this.ascension = reader.ReadInt(5);
			this.modifiers = reader.ReadList<SerializableModifier>(32);
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00217988 File Offset: 0x00215B88
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 3);
			defaultInterpolatedStringHandler.AppendFormatted("ClientLobbyJoinResponseMessage");
			defaultInterpolatedStringHandler.AppendLiteral(" Players: ");
			List<LobbyPlayer> list = this.playersInLobby;
			defaultInterpolatedStringHandler.AppendFormatted<int?>((list != null) ? new int?(list.Count) : null);
			defaultInterpolatedStringHandler.AppendLiteral(" Ascension: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.ascension);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// List of players already in the lobby.
		/// </summary>
		// Token: 0x04001B58 RID: 7000
		[Nullable(2)]
		public List<LobbyPlayer> playersInLobby;

		/// <summary>
		/// If the run is a daily, this contains information about the time that the host requested from the time server.
		/// </summary>
		// Token: 0x04001B59 RID: 7001
		public TimeServerResult? dailyTime;

		/// <summary>
		/// Current ascension that the lobby is set to.
		/// </summary>
		// Token: 0x04001B5A RID: 7002
		public int ascension;

		/// <summary>
		/// Seed set at time of join. Only non-null if in a custom game. The true seed to use is sent when the run begins.
		/// </summary>
		// Token: 0x04001B5B RID: 7003
		[Nullable(2)]
		public string seed;

		/// <summary>
		/// Modifiers present in the lobby at the time of join.
		/// </summary>
		// Token: 0x04001B5C RID: 7004
		public List<SerializableModifier> modifiers;
	}
}
