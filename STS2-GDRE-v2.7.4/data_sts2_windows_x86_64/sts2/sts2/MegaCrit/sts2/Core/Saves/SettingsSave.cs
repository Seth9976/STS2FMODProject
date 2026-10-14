using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Godot;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Saves
{
	/// <summary>
	/// This class serves as a schema for the Settings save file.
	/// It's used for serializing and deserializing data in the options screen.
	/// This file is stored at the account level, not the profile level, and is NOT to be synced across devices/platforms.
	/// See <see cref="T:MegaCrit.Sts2.Core.Saves.PrefsSave" /> for the profile-level options displayed in the options screen.
	/// </summary>
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	[Nullable(0)]
	public class SettingsSave : ISaveSchema
	{
		/// <summary>
		/// The schema version of this save.
		/// </summary>
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00041578 File Offset: 0x0003F778
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00041580 File Offset: 0x0003F780
		[JsonPropertyName("schema_version")]
		public int SchemaVersion { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00041589 File Offset: 0x0003F789
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00041591 File Offset: 0x0003F791
		[JsonPropertyName("fps_limit")]
		public int FpsLimit { get; set; } = 60;

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0004159A File Offset: 0x0003F79A
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x000415A2 File Offset: 0x0003F7A2
		[Nullable(2)]
		[JsonPropertyName("language")]
		public string Language
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x000415AB File Offset: 0x0003F7AB
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x000415B3 File Offset: 0x0003F7B3
		[JsonPropertyName("window_position")]
		public Vector2I WindowPosition { get; set; } = new Vector2I(-1, -1);

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000415BC File Offset: 0x0003F7BC
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x000415C4 File Offset: 0x0003F7C4
		[JsonPropertyName("window_size")]
		public Vector2I WindowSize { get; set; } = new Vector2I(1920, 1080);

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x000415CD File Offset: 0x0003F7CD
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x000415D5 File Offset: 0x0003F7D5
		[JsonPropertyName("fullscreen")]
		public bool Fullscreen { get; set; } = true;

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x000415DE File Offset: 0x0003F7DE
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x000415E6 File Offset: 0x0003F7E6
		[JsonPropertyName("aspect_ratio")]
		public AspectRatioSetting AspectRatioSetting { get; set; } = AspectRatioSetting.SixteenByNine;

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x000415EF File Offset: 0x0003F7EF
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x000415F7 File Offset: 0x0003F7F7
		[JsonPropertyName("target_display")]
		public int TargetDisplay { get; set; } = -1;

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00041600 File Offset: 0x0003F800
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00041608 File Offset: 0x0003F808
		[JsonPropertyName("resize_windows")]
		public bool ResizeWindows { get; set; } = true;

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00041611 File Offset: 0x0003F811
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00041619 File Offset: 0x0003F819
		[JsonPropertyName("vsync")]
		public VSyncType VSync { get; set; } = VSyncType.Adaptive;

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00041622 File Offset: 0x0003F822
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0004162A File Offset: 0x0003F82A
		[JsonPropertyName("msaa")]
		public int Msaa { get; set; } = 2;

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00041633 File Offset: 0x0003F833
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0004163B File Offset: 0x0003F83B
		[JsonPropertyName("volume_bgm")]
		public float VolumeBgm { get; set; } = 0.5f;

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00041644 File Offset: 0x0003F844
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x0004164C File Offset: 0x0003F84C
		[JsonPropertyName("volume_master")]
		public float VolumeMaster { get; set; } = 0.5f;

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00041655 File Offset: 0x0003F855
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x0004165D File Offset: 0x0003F85D
		[JsonPropertyName("volume_sfx")]
		public float VolumeSfx { get; set; } = 0.5f;

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00041666 File Offset: 0x0003F866
		// (set) Token: 0x060008C9 RID: 2249 RVA: 0x0004166E File Offset: 0x0003F86E
		[JsonPropertyName("volume_ambience")]
		public float VolumeAmbience { get; set; } = 0.5f;

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00041677 File Offset: 0x0003F877
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x0004167F File Offset: 0x0003F87F
		[Nullable(2)]
		[JsonPropertyName("mod_settings")]
		public ModSettings ModSettings
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00041688 File Offset: 0x0003F888
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00041690 File Offset: 0x0003F890
		[JsonPropertyName("skip_intro_logo")]
		public bool SkipIntroLogo { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00041699 File Offset: 0x0003F899
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x000416A1 File Offset: 0x0003F8A1
		[JsonPropertyName("keyboard_mapping")]
		public Dictionary<string, string> KeyboardMapping { get; set; } = new Dictionary<string, string>();

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x000416AA File Offset: 0x0003F8AA
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x000416B2 File Offset: 0x0003F8B2
		[JsonPropertyName("controller_mapping_type")]
		public ControllerMappingType ControllerMappingType { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000416BB File Offset: 0x0003F8BB
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x000416C3 File Offset: 0x0003F8C3
		[JsonPropertyName("controller_mapping")]
		public Dictionary<string, string> ControllerMapping { get; set; } = new Dictionary<string, string>();

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x000416CC File Offset: 0x0003F8CC
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x000416D4 File Offset: 0x0003F8D4
		[JsonPropertyName("limit_fps_in_background")]
		public bool LimitFpsInBackground { get; set; } = true;

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000416DD File Offset: 0x0003F8DD
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x000416E5 File Offset: 0x0003F8E5
		[JsonPropertyName("full_console")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool FullConsole { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000416EE File Offset: 0x0003F8EE
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x000416F6 File Offset: 0x0003F8F6
		[JsonPropertyName("seen_ea_disclaimer")]
		public bool SeenEaDisclaimer { get; set; }
	}
}
