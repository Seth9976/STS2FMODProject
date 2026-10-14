using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Modding
{
	// Token: 0x02000B9B RID: 2971
	[NullableContext(1)]
	[Nullable(0)]
	public class ModSettings
	{
		// Token: 0x1700225D RID: 8797
		// (get) Token: 0x06007C02 RID: 31746 RVA: 0x002A47A2 File Offset: 0x002A29A2
		// (set) Token: 0x06007C03 RID: 31747 RVA: 0x002A47AA File Offset: 0x002A29AA
		[JsonPropertyName("mods_enabled")]
		public bool PlayerAgreedToModLoading { get; set; }

		// Token: 0x1700225E RID: 8798
		// (get) Token: 0x06007C04 RID: 31748 RVA: 0x002A47B3 File Offset: 0x002A29B3
		// (set) Token: 0x06007C05 RID: 31749 RVA: 0x002A47BB File Offset: 0x002A29BB
		[JsonPropertyName("mod_list")]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public List<SettingsSaveMod> ModList { get; set; } = new List<SettingsSaveMod>();

		// Token: 0x06007C06 RID: 31750 RVA: 0x002A47C4 File Offset: 0x002A29C4
		public bool IsModDisabled(Mod mod)
		{
			ModManifest manifest = mod.manifest;
			return this.IsModDisabled((manifest != null) ? manifest.id : null, mod.modSource);
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x002A47E4 File Offset: 0x002A29E4
		[NullableContext(2)]
		public bool IsModDisabled(string id, ModSource source)
		{
			return this.ModList.Any<SettingsSaveMod>((SettingsSaveMod m) => m.Id == id && m.Source == source && !m.IsEnabled);
		}
	}
}
