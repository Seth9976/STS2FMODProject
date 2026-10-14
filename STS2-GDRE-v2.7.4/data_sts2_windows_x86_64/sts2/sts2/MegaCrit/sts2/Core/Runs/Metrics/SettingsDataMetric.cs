using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Godot;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Runs.Metrics
{
	// Token: 0x0200011C RID: 284
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public struct SettingsDataMetric
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0005AC57 File Offset: 0x00058E57
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0005AC5F File Offset: 0x00058E5F
		[RequiredMember]
		[JsonPropertyName("buildId")]
		public string BuildId { readonly get; set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0005AC68 File Offset: 0x00058E68
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0005AC70 File Offset: 0x00058E70
		[JsonPropertyName("os")]
		public string Os { readonly get; set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0005AC79 File Offset: 0x00058E79
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0005AC81 File Offset: 0x00058E81
		[JsonPropertyName("platform")]
		public string Platform { readonly get; set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0005AC8A File Offset: 0x00058E8A
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x0005AC92 File Offset: 0x00058E92
		[JsonPropertyName("systemRam")]
		public int SystemRam { readonly get; set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0005AC9B File Offset: 0x00058E9B
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x0005ACA3 File Offset: 0x00058EA3
		[JsonPropertyName("language")]
		public string LanguageCode { readonly get; set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0005ACAC File Offset: 0x00058EAC
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x0005ACB4 File Offset: 0x00058EB4
		[RequiredMember]
		[JsonPropertyName("combatSpeed")]
		public FastModeType FastModeType { readonly get; set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0005ACBD File Offset: 0x00058EBD
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0005ACC5 File Offset: 0x00058EC5
		[JsonPropertyName("screenshake")]
		public int Screenshake { readonly get; set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0005ACCE File Offset: 0x00058ECE
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0005ACD6 File Offset: 0x00058ED6
		[JsonPropertyName("runTimer")]
		public bool ShowRunTimer { readonly get; set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0005ACDF File Offset: 0x00058EDF
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0005ACE7 File Offset: 0x00058EE7
		[JsonPropertyName("phobiaMode")]
		public bool PhobiaMode { readonly get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0005ACF0 File Offset: 0x00058EF0
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0005ACF8 File Offset: 0x00058EF8
		[JsonPropertyName("cardIndices")]
		public bool ShowCardIndices { readonly get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0005AD01 File Offset: 0x00058F01
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0005AD09 File Offset: 0x00058F09
		[JsonPropertyName("displayCount")]
		public int DisplayCount { readonly get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0005AD12 File Offset: 0x00058F12
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x0005AD1A File Offset: 0x00058F1A
		[JsonPropertyName("displayResolution")]
		public Vector2I DisplayResolution { readonly get; set; }

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0005AD23 File Offset: 0x00058F23
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0005AD2B File Offset: 0x00058F2B
		[JsonPropertyName("fullscreen")]
		public bool Fullscreen { readonly get; set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0005AD34 File Offset: 0x00058F34
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0005AD3C File Offset: 0x00058F3C
		[JsonPropertyName("aspectRatio")]
		public AspectRatioSetting AspectRatio { readonly get; set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0005AD45 File Offset: 0x00058F45
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x0005AD4D File Offset: 0x00058F4D
		[JsonPropertyName("resizeWindows")]
		public bool ResizeWindows { readonly get; set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0005AD56 File Offset: 0x00058F56
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0005AD5E File Offset: 0x00058F5E
		[JsonPropertyName("vSync")]
		public VSyncType VSync { readonly get; set; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0005AD67 File Offset: 0x00058F67
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0005AD6F File Offset: 0x00058F6F
		[JsonPropertyName("fpsLimit")]
		public int FpsLimit { readonly get; set; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0005AD78 File Offset: 0x00058F78
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0005AD80 File Offset: 0x00058F80
		[JsonPropertyName("msaa")]
		public int Msaa { readonly get; set; }
	}
}
