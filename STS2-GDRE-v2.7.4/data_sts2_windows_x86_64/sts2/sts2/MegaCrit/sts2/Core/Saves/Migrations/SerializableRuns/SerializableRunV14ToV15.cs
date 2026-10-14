using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// Added GameMode. It defaults to None for error-checking purposes, but for existing real runs, it should default to
	/// something reasonable.
	/// </summary>
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 14, 15)]
	public class SerializableRunV14ToV15 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x00048BA8 File Offset: 0x00046DA8
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("SerializableRun migration v14 -> v15: Adding GameMode to save files, initialized as Standard", 2);
			DateTimeOffset? asOrNull = saveData.GetAsOrNull<DateTimeOffset>("dailyTime");
			List<SerializableModifier> @as = saveData.GetAs<List<SerializableModifier>>("modifiers");
			if (asOrNull != null)
			{
				saveData.Set<GameMode>("game_mode", GameMode.Daily);
				return;
			}
			if (@as != null && @as.Count > 0)
			{
				saveData.Set<GameMode>("game_mode", GameMode.Custom);
				return;
			}
			saveData.Set<GameMode>("game_mode", GameMode.Standard);
		}
	}
}
