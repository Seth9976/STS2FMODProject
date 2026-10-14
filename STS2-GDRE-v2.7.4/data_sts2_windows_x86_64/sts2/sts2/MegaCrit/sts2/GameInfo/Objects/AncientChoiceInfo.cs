using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.GameInfo.Objects
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	[Serializable]
	public class AncientChoiceInfo : IGameInfo
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00010EA1 File Offset: 0x0000F0A1
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00010EA9 File Offset: 0x0000F0A9
		[RequiredMember]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00010EB2 File Offset: 0x0000F0B2
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00010EBA File Offset: 0x0000F0BA
		[RequiredMember]
		[JsonPropertyName("bot_keyword")]
		public string BotKeyword { get; set; }

		/// This is for feedback bot (the discord bot), the text here is what it will display when called.
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00010EC3 File Offset: 0x0000F0C3
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00010ECB File Offset: 0x0000F0CB
		[RequiredMember]
		[JsonPropertyName("bot_text")]
		public string BotText { get; set; }

		/// The id for the ancient choice
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00010EDC File Offset: 0x0000F0DC
		[RequiredMember]
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// This is for the metrics system, the text here is what will be displayed in the metrics website screen.
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00010EE5 File Offset: 0x0000F0E5
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00010EED File Offset: 0x0000F0ED
		[RequiredMember]
		[JsonPropertyName("text")]
		public string Text { get; set; }

		/// Represents the ancient's id
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00010EF6 File Offset: 0x0000F0F6
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00010EFE File Offset: 0x0000F0FE
		[RequiredMember]
		[JsonPropertyName("ancient")]
		public string Ancient { get; set; }

		// Token: 0x06000194 RID: 404 RVA: 0x00010F07 File Offset: 0x0000F107
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public AncientChoiceInfo()
		{
		}
	}
}
