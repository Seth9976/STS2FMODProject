using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// SerializableReward.CardPoolId (singular) changed to CardPoolIds (plural).
	/// </summary>
	// Token: 0x020000E8 RID: 232
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 13, 14)]
	public class SerializableRunV13ToV14 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x00048A34 File Offset: 0x00046C34
		protected override void ApplyMigration(MigratingData saveData)
		{
			Log.Info("SerializableRun migration v13 -> v14: Migrating SerializableRun.CardPoolId to CardPoolIds", 2);
			JsonObject jsonObject = saveData.GetRawNode("pre_finished_room") as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			JsonObject jsonObject2 = jsonObject["extra_rewards"] as JsonObject;
			if (jsonObject2 == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
			{
				string text;
				JsonNode jsonNode;
				keyValuePair.Deconstruct(out text, out jsonNode);
				JsonNode jsonNode2 = jsonNode;
				JsonArray jsonArray = jsonNode2 as JsonArray;
				if (jsonArray != null)
				{
					foreach (JsonNode jsonNode3 in jsonArray)
					{
						JsonObject jsonObject3 = jsonNode3 as JsonObject;
						if (jsonObject3 != null)
						{
							if (!jsonObject3.ContainsKey("card_pools"))
							{
								jsonObject3["card_pools"] = new JsonArray(null);
							}
							JsonNode jsonNode4;
							if (jsonObject3.TryGetPropertyValue("card_pool", out jsonNode4))
							{
								string text2 = ((jsonNode4 != null) ? jsonNode4.GetValue<string>() : null);
								jsonObject3.Remove("card_pool");
								if (text2 != null && !(text2 == ModelId.none.ToString()))
								{
									((JsonArray)jsonObject3["card_pools"]).Add<string>(text2);
								}
							}
						}
					}
				}
			}
		}
	}
}
