using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x02000118 RID: 280
	[NullableContext(1)]
	[Nullable(0)]
	public struct EventChoiceMetric
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x00052758 File Offset: 0x00050958
		public EventChoiceMetric(MapPointHistoryEntry entry, ulong playerId, SerializableActModel actModel)
		{
			this.id = entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId.Entry;
			this.act = actModel.Id.Entry;
			LocString title = entry.GetEntry(playerId).EventChoices.Last<EventOptionHistoryEntry>().Title;
			string[] array = title.LocEntryKey.Split(".", StringSplitOptions.None);
			this.picked = array[array.Length - 2];
		}

		// Token: 0x04000492 RID: 1170
		public readonly string id;

		// Token: 0x04000493 RID: 1171
		public readonly string act;

		// Token: 0x04000494 RID: 1172
		public readonly string picked;
	}
}
