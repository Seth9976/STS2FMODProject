using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves.Migrations.Shared;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// Migrates renamed/deleted ModelIds:
	/// - CARD.PREPARE -&gt; CARD.PREPARED (class Prepare renamed back to Prepared)
	/// - ENCOUNTER.TOADPOLES_NORMAL -&gt; ENCOUNTER.SEAPUNK_NORMAL (class ToadpolesNormal renamed to SeapunkNormal)
	/// - MONSTER.DOOR -&gt; MONSTER.DEPRECATED_MONSTER (Door monster deleted in Doormaker rework)
	/// </summary>
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 15, 16)]
	public class SerializableRunV15ToV16 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x00048C1B File Offset: 0x00046E1B
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("SerializableRun migration v15 -> v16: Migrating renamed/deleted ModelIds", 2);
			SharedMigrationHelper.ReplaceModelIds(saveData.GetRawNode(), SharedMigrationHelper.v100Renames);
		}
	}
}
