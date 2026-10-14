using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Runs.History
{
	/// <summary>
	/// Ancient choice logs are used exclusively to to make Ancient Metrics to send to our metrics site. This makes them a
	/// bit more flexible to use than the MapPointHistoryEntry.EventChoices which are required to be LocStrings.
	/// </summary>
	// Token: 0x0200011D RID: 285
	[NullableContext(1)]
	[Nullable(0)]
	[Serializable]
	public class AncientChoiceHistoryEntry : IPacketSerializable
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0005AD89 File Offset: 0x00058F89
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0005AD91 File Offset: 0x00058F91
		[JsonPropertyName("title")]
		public LocString Title { get; set; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0005AD9A File Offset: 0x00058F9A
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0005ADA2 File Offset: 0x00058FA2
		[JsonPropertyName("was_chosen")]
		public bool WasChosen { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0005ADAB File Offset: 0x00058FAB
		[JsonPropertyName("TextKey")]
		public string TextKey
		{
			get
			{
				string[] array = this.Title.LocEntryKey.Split(".", StringSplitOptions.None);
				return array[array.Length - 2];
			}
		}

		/// <summary>
		/// For serialization only
		/// </summary>
		// Token: 0x06000E13 RID: 3603 RVA: 0x0005ADC9 File Offset: 0x00058FC9
		public AncientChoiceHistoryEntry()
		{
			this.Title = new LocString(string.Empty, string.Empty);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0005ADE6 File Offset: 0x00058FE6
		public AncientChoiceHistoryEntry(LocString title, bool wasChosen)
		{
			this.Title = title;
			this.WasChosen = wasChosen;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0005ADFC File Offset: 0x00058FFC
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.Title.LocTable);
			writer.WriteString(this.Title.LocEntryKey);
			writer.WriteBool(this.WasChosen);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0005AE2C File Offset: 0x0005902C
		public void Deserialize(PacketReader reader)
		{
			string text = reader.ReadString();
			string text2 = reader.ReadString();
			this.Title = new LocString(text, text2);
			this.WasChosen = reader.ReadBool();
		}
	}
}
