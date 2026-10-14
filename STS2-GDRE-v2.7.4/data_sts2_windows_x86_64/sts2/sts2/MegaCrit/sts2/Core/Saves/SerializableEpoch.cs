using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// Holds progress data on a per Epoch basis.
	/// If a SerializableEpoch is NOT in the list of epochs in SerializableProgress,
	/// it means the player has not revealed that Epoch's Slot via Timeline Expansion.
	/// </summary>
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	public class SerializableEpoch
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x00040262 File Offset: 0x0003E462
		[JsonPropertyName("id")]
		public string Id { get; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0004026A File Offset: 0x0003E46A
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x00040272 File Offset: 0x0003E472
		[JsonPropertyName("state")]
		public EpochState State { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0004027B File Offset: 0x0003E47B
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00040283 File Offset: 0x0003E483
		[JsonPropertyName("obtain_date")]
		public long ObtainDate { get; set; }

		// Token: 0x0600080A RID: 2058 RVA: 0x0004028C File Offset: 0x0003E48C
		public SerializableEpoch(string id, EpochState state)
		{
			this.Id = id;
			this.State = EpochState.NotObtained;
			bool flag = state - EpochState.ObtainedNoSlot <= 2;
			if (flag)
			{
				this.SetObtained(state);
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000402C4 File Offset: 0x0003E4C4
		public void SetObtained(EpochState state)
		{
			if (this.State != EpochState.ObtainedNoSlot && this.State != EpochState.Obtained && this.State != EpochState.Revealed)
			{
				this.ObtainDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				this.State = state;
			}
		}
	}
}
