using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace MegaCrit.Sts2.Core.Saves.Migrations.ProgressSaves
{
	/// <summary>
	/// Removes Osty, Byrdpip and Pael's Legion from the enemy stats
	/// because they aren't actually enemies.
	/// </summary>
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableProgress), 21, 22)]
	public class ProgressSaveV21ToV22 : MigrationBase<SerializableProgress>
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0004942C File Offset: 0x0004762C
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("Progress save migration v21 -> v22: Removing Osty, Byrdpip and Pael's Legion from enemy stats", 2);
			string[] array = new string[]
			{
				ModelDb.Monster<Osty>().Id.ToString(),
				ModelDb.Monster<PaelsLegion>().Id.ToString(),
				ModelDb.Monster<Byrdpip>().Id.ToString()
			};
			JsonArray jsonArray = saveData.GetRawNode("enemy_stats") as JsonArray;
			if (jsonArray == null)
			{
				return;
			}
			List<JsonNode> list = new List<JsonNode>();
			foreach (JsonNode jsonNode in jsonArray)
			{
				if (jsonNode != null)
				{
					JsonNode jsonNode2 = jsonNode["enemy_id"];
					if (jsonNode2 != null && array.Contains(jsonNode2.GetValue<string>()))
					{
						list.Add(jsonNode);
					}
				}
			}
			foreach (JsonNode jsonNode3 in list)
			{
				jsonArray.Remove(jsonNode3);
			}
		}
	}
}
