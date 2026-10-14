using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Saves.Migrations.ProfileSaves
{
	/// <summary>
	/// No-op migration for release save wipe. All pre-release saves are invalidated
	/// via minimum supported version bump.
	/// </summary>
	// Token: 0x020000F2 RID: 242
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(ProfileSave), 1, 2)]
	public class ProfileSaveV1ToV2 : MigrationBase<ProfileSave>
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x00049548 File Offset: 0x00047748
		protected override void ApplyMigration(MigratingData saveData)
		{
		}
	}
}
