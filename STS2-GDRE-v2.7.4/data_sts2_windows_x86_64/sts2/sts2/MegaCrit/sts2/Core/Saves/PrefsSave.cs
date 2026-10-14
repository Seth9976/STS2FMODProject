using System;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// This class serves as a schema for the Preferences save file.
	/// It's used for serializing and deserializing data in the options screen.
	/// This file is synced across devices/platforms.
	/// It is also scoped per-profile.
	/// </summary>
	// Token: 0x020000A3 RID: 163
	public class PrefsSave : ISaveSchema
	{
		/// <summary>
		/// The schema version of this save.
		/// </summary>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0003C01C File Offset: 0x0003A21C
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0003C024 File Offset: 0x0003A224
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0003C02D File Offset: 0x0003A22D
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0003C035 File Offset: 0x0003A235
		[JsonPropertyName("fast_mode")]
		public FastModeType FastMode { get; set; } = FastModeType.Normal;

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0003C03E File Offset: 0x0003A23E
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0003C046 File Offset: 0x0003A246
		[JsonPropertyName("phobia_mode")]
		public bool PhobiaMode { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0003C04F File Offset: 0x0003A24F
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0003C057 File Offset: 0x0003A257
		[JsonPropertyName("screenshake")]
		public int ScreenShakeOptionIndex { get; set; } = 2;

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0003C060 File Offset: 0x0003A260
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0003C068 File Offset: 0x0003A268
		[JsonPropertyName("show_run_timer")]
		public bool ShowRunTimer { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0003C071 File Offset: 0x0003A271
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0003C079 File Offset: 0x0003A279
		[JsonPropertyName("show_card_indices")]
		public bool ShowCardIndices { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0003C082 File Offset: 0x0003A282
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0003C08A File Offset: 0x0003A28A
		[JsonPropertyName("upload_data")]
		public bool UploadData { get; set; } = true;

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0003C093 File Offset: 0x0003A293
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0003C09B File Offset: 0x0003A29B
		[JsonPropertyName("mute_in_background")]
		public bool MuteInBackground { get; set; } = true;

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0003C0A4 File Offset: 0x0003A2A4
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0003C0AC File Offset: 0x0003A2AC
		[JsonPropertyName("long_press")]
		public bool IsLongPressEnabled { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0003C0B5 File Offset: 0x0003A2B5
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0003C0BD File Offset: 0x0003A2BD
		[JsonPropertyName("text_effects_enabled")]
		public bool TextEffectsEnabled { get; set; } = true;

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0003C0C6 File Offset: 0x0003A2C6
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0003C0CE File Offset: 0x0003A2CE
		[JsonPropertyName("show_mp_drawings")]
		public bool ShowMultiplayerDrawings { get; set; } = true;

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0003C0D7 File Offset: 0x0003A2D7
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0003C0DF File Offset: 0x0003A2DF
		[JsonPropertyName("bestiary_actions_preferred")]
		public bool IsBestiaryActionsPreferred { get; set; } = true;
	}
}
