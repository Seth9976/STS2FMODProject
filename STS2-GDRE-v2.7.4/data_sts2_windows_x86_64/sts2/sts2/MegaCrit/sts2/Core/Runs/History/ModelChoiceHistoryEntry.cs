using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public struct ModelChoiceHistoryEntry
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x0005B54E File Offset: 0x0005974E
		public ModelChoiceHistoryEntry(ModelId choice, bool wasPicked)
		{
			this.choice = choice;
			this.wasPicked = wasPicked;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0005B55E File Offset: 0x0005975E
		public void Serialize<[Nullable(0)] T>(PacketWriter writer) where T : AbstractModel
		{
			writer.WriteModelEntry(this.choice);
			writer.WriteBool(this.wasPicked);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0005B578 File Offset: 0x00059778
		public void Deserialize<[Nullable(0)] T>(PacketReader reader) where T : AbstractModel
		{
			this.choice = reader.ReadModelIdAssumingType<T>();
			this.wasPicked = reader.ReadBool();
		}

		// Token: 0x04000531 RID: 1329
		[JsonPropertyName("choice")]
		public ModelId choice;

		// Token: 0x04000532 RID: 1330
		[JsonPropertyName("was_picked")]
		public bool wasPicked;
	}
}
