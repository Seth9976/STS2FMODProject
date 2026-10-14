using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableModifier : IPacketSerializable
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x000446DF File Offset: 0x000428DF
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x000446E7 File Offset: 0x000428E7
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x000446F0 File Offset: 0x000428F0
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x000446F8 File Offset: 0x000428F8
		[JsonPropertyName("props")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SavedProperties Props { get; set; }

		// Token: 0x06000994 RID: 2452 RVA: 0x00044701 File Offset: 0x00042901
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.WriteBool(this.Props != null);
			if (this.Props != null)
			{
				writer.Write<SavedProperties>(this.Props);
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00044732 File Offset: 0x00042932
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<ModifierModel>();
			if (reader.ReadBool())
			{
				this.Props = reader.Read<SavedProperties>();
			}
		}
	}
}
