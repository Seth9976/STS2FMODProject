using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MegaCrit.Sts2.Core.Modding
{
	/// <summary>
	/// Represents a mod in the settings save. Used for:
	///  - Representing manual load order
	///  - Disabling/enabling mods
	/// </summary>
	// Token: 0x02000B9D RID: 2973
	[NullableContext(1)]
	[Nullable(0)]
	public class SettingsSaveMod
	{
		// Token: 0x1700225F RID: 8799
		// (get) Token: 0x06007C09 RID: 31753 RVA: 0x002A482F File Offset: 0x002A2A2F
		// (set) Token: 0x06007C0A RID: 31754 RVA: 0x002A4837 File Offset: 0x002A2A37
		[JsonPropertyName("id")]
		public string Id { get; set; } = "";

		// Token: 0x17002260 RID: 8800
		// (get) Token: 0x06007C0B RID: 31755 RVA: 0x002A4840 File Offset: 0x002A2A40
		// (set) Token: 0x06007C0C RID: 31756 RVA: 0x002A4848 File Offset: 0x002A2A48
		[JsonPropertyName("source")]
		public ModSource Source { get; set; }

		// Token: 0x17002261 RID: 8801
		// (get) Token: 0x06007C0D RID: 31757 RVA: 0x002A4851 File Offset: 0x002A2A51
		// (set) Token: 0x06007C0E RID: 31758 RVA: 0x002A4859 File Offset: 0x002A2A59
		[JsonPropertyName("is_enabled")]
		public bool IsEnabled { get; set; } = true;

		// Token: 0x06007C0F RID: 31759 RVA: 0x002A4862 File Offset: 0x002A2A62
		public SettingsSaveMod()
		{
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x002A487C File Offset: 0x002A2A7C
		public SettingsSaveMod(Mod mod)
		{
			ModManifest manifest = mod.manifest;
			string text = ((manifest != null) ? manifest.id : null);
			if (text == null)
			{
				throw new InvalidOperationException();
			}
			this.Id = text;
			this.Source = mod.modSource;
		}
	}
}
