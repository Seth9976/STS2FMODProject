using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SettingsSaves
{
	// Token: 0x020000E5 RID: 229
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SettingsSave), 4, 5)]
	public class SettingsSaveV4ToV5 : MigrationBase<SettingsSave>
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x000486DE File Offset: 0x000468DE
		protected override void ApplyMigration(MigratingData saveData)
		{
			saveData.Remove("disabled_mods");
		}
	}
}
