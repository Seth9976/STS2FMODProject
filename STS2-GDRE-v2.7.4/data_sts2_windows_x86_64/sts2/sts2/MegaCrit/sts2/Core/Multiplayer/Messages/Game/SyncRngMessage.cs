using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game
{
	/// <summary>
	/// Sent to all clients from host at the start of combat to sync RNG state.
	/// TODO: This might not be the right thing to do! If a client has rolled Niche but the host hasn't, and we roll that
	///       back, will we end up generating the same RNG again for the same thing?
	/// </summary>
	// Token: 0x0200047D RID: 1149
	[NullableContext(1)]
	[Nullable(0)]
	public struct SyncRngMessage : INetMessage, IPacketSerializable
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06004668 RID: 18024 RVA: 0x00218598 File Offset: 0x00216798
		public bool ShouldBroadcast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x0021859B File Offset: 0x0021679B
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x0600466A RID: 18026 RVA: 0x0021859E File Offset: 0x0021679E
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x0600466B RID: 18027 RVA: 0x002185A1 File Offset: 0x002167A1
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x002185A4 File Offset: 0x002167A4
		public void Serialize(PacketWriter writer)
		{
			writer.Write<SerializableRunRngSet>(this.rng);
			writer.Write<SerializableRelicGrabBag>(this.sharedRelicGrabBag);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x002185BE File Offset: 0x002167BE
		public void Deserialize(PacketReader reader)
		{
			this.rng = reader.Read<SerializableRunRngSet>();
			this.sharedRelicGrabBag = reader.Read<SerializableRelicGrabBag>();
		}

		// Token: 0x04001B8C RID: 7052
		public SerializableRunRngSet rng;

		// Token: 0x04001B8D RID: 7053
		public SerializableRelicGrabBag sharedRelicGrabBag;
	}
}
