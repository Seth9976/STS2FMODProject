using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// Migrating LastingCandy.CombatsSeen to LastingCandy.CombatRewardsSeen
	/// </summary>
	// Token: 0x020000EB RID: 235
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 16, 17)]
	public class SerializableRunV16ToV17 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A9B RID: 2715 RVA: 0x00048C40 File Offset: 0x00046E40
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("SerializableRun migration v16 -> v17: Migrating LastingCandy.CombatsSeen to LastingCandy.CombatRewardsSeen", 2);
			JsonObject rawNode = saveData.GetRawNode();
			if (rawNode == null)
			{
				return;
			}
			bool flag = false;
			JsonObject jsonObject = saveData.GetRawNode("pre_finished_room") as JsonObject;
			if (jsonObject != null)
			{
				JsonValue jsonValue = jsonObject["room_type"] as JsonValue;
				string text;
				if (jsonValue != null && jsonValue.TryGetValue<string>(out text) && (text.Equals("monster", StringComparison.OrdinalIgnoreCase) || text.Equals("elite", StringComparison.OrdinalIgnoreCase) || text.Equals("boss", StringComparison.OrdinalIgnoreCase)))
				{
					JsonValue jsonValue2 = jsonObject["encounter_id"] as JsonValue;
					string text3;
					string text2 = ((jsonValue2 != null && jsonValue2.TryGetValue<string>(out text3)) ? text3 : null);
					flag = !string.Equals(text2, "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER", StringComparison.OrdinalIgnoreCase);
				}
			}
			JsonArray jsonArray = rawNode["players"] as JsonArray;
			if (jsonArray == null)
			{
				return;
			}
			foreach (JsonNode jsonNode in jsonArray)
			{
				if (jsonNode != null)
				{
					JsonArray jsonArray2 = jsonNode["relics"] as JsonArray;
					if (jsonArray2 != null)
					{
						foreach (JsonNode jsonNode2 in jsonArray2)
						{
							if (jsonNode2 != null && jsonNode2["id"] != null && !(jsonNode2["id"].GetValue<string>() != "RELIC.LASTING_CANDY"))
							{
								JsonObject jsonObject2 = jsonNode2["props"] as JsonObject;
								if (jsonObject2 != null)
								{
									JsonArray jsonArray3 = jsonObject2["ints"] as JsonArray;
									if (jsonArray3 != null)
									{
										foreach (JsonNode jsonNode3 in jsonArray3)
										{
											if (jsonNode3 != null && jsonNode3["name"] != null && !(jsonNode3["name"].GetValue<string>() != "CombatsSeen"))
											{
												jsonNode3["name"] = "CombatRewardsSeen";
												if (flag)
												{
													jsonNode3["value"] = jsonNode3["value"].GetValue<int>() - 1;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
