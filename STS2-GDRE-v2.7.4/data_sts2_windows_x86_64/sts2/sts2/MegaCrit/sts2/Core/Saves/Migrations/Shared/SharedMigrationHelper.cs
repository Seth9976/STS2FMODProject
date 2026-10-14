using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace MegaCrit.Sts2.Core.Saves.Migrations.Shared
{
	// Token: 0x020000E3 RID: 227
	[NullableContext(1)]
	[Nullable(0)]
	public static class SharedMigrationHelper
	{
		/// <summary>
		/// Recursively walks a JSON tree and replaces string values found in the renames dictionary.
		/// </summary>
		// Token: 0x06000A88 RID: 2696 RVA: 0x00047CE0 File Offset: 0x00045EE0
		public static void ReplaceModelIds([Nullable(2)] JsonNode node, IReadOnlyDictionary<string, string> renames)
		{
			JsonObject jsonObject = node as JsonObject;
			JsonArray jsonArray;
			if (jsonObject == null)
			{
				jsonArray = node as JsonArray;
				if (jsonArray == null)
				{
					return;
				}
			}
			else
			{
				using (List<string>.Enumerator enumerator = new List<string>(jsonObject.Select<KeyValuePair<string, JsonNode>, string>((KeyValuePair<string, JsonNode> kvp) => kvp.Key)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						JsonNode jsonNode = jsonObject[text];
						JsonValue jsonValue = jsonNode as JsonValue;
						string text2;
						string text3;
						if (jsonValue != null && jsonValue.TryGetValue<string>(out text2) && renames.TryGetValue(text2, out text3))
						{
							jsonObject[text] = text3;
						}
						else
						{
							SharedMigrationHelper.ReplaceModelIds(jsonNode, renames);
						}
					}
					return;
				}
			}
			for (int i = 0; i < jsonArray.Count; i++)
			{
				JsonNode jsonNode2 = jsonArray[i];
				JsonValue jsonValue2 = jsonNode2 as JsonValue;
				string text4;
				string text5;
				if (jsonValue2 != null && jsonValue2.TryGetValue<string>(out text4) && renames.TryGetValue(text4, out text5))
				{
					jsonArray[i] = text5;
				}
				else
				{
					SharedMigrationHelper.ReplaceModelIds(jsonNode2, renames);
				}
			}
		}

		/// <summary>
		/// Used in Run History V2-&gt;V3 and SerializableRun V3-&gt;V4.
		/// migratingData is assumed to be the list of list of MapPointHistoryEntry.
		/// </summary>
		// Token: 0x06000A89 RID: 2697 RVA: 0x00047E0C File Offset: 0x0004600C
		[NullableContext(2)]
		public static void MigrateMapPointHistoryRooms(JsonNode jsonNode)
		{
			JsonArray jsonArray = jsonNode as JsonArray;
			if (jsonArray == null)
			{
				return;
			}
			foreach (JsonNode jsonNode2 in jsonArray)
			{
				JsonArray jsonArray2 = jsonNode2 as JsonArray;
				if (jsonArray2 != null)
				{
					foreach (JsonNode jsonNode3 in jsonArray2)
					{
						JsonObject jsonObject = jsonNode3 as JsonObject;
						JsonNode jsonNode4;
						if (jsonObject != null && jsonObject.TryGetPropertyValue("room_types", out jsonNode4))
						{
							JsonArray jsonArray3 = jsonNode4 as JsonArray;
							if (jsonArray3 != null)
							{
								JsonArray jsonArray4 = new JsonArray(null);
								jsonObject.Add("rooms", jsonArray4);
								foreach (JsonNode jsonNode5 in jsonArray3)
								{
									if (jsonNode5 != null)
									{
										JsonObject jsonObject2 = new JsonObject(null) { 
										{
											"room_type",
											jsonNode5.DeepClone()
										} };
										jsonArray4.Add(jsonObject2);
									}
								}
								jsonObject.Remove("room_types");
								JsonObject jsonObject3 = (JsonObject)jsonArray4[0];
								JsonArray jsonArray5 = jsonArray4;
								JsonObject jsonObject4 = (JsonObject)jsonArray5[jsonArray5.Count - 1];
								JsonNode jsonNode6;
								if (jsonObject.TryGetPropertyValue("model_id", out jsonNode6))
								{
									jsonObject3.Add("model_id", jsonNode6.DeepClone());
									jsonObject.Remove("model_id");
								}
								JsonNode jsonNode7;
								if (jsonObject.TryGetPropertyValue("monster_ids", out jsonNode7))
								{
									jsonObject4.Add("monster_ids", jsonNode7.DeepClone());
									jsonObject.Remove("monster_ids");
								}
								JsonNode jsonNode8;
								if (jsonObject.TryGetPropertyValue("turns_taken", out jsonNode8))
								{
									jsonObject4.Add("turns_taken", jsonNode8.DeepClone());
									jsonObject.Remove("turns_taken");
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Recursively removes schema_version from everything except for the first JSON object.
		/// </summary>
		// Token: 0x06000A8A RID: 2698 RVA: 0x00048048 File Offset: 0x00046248
		public static void RecursiveRemoveSchema(JsonNode node, int depth = 0)
		{
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject != null)
			{
				if (depth > 0)
				{
					jsonObject.Remove("schema_version");
				}
				using (IEnumerator<KeyValuePair<string, JsonNode>> enumerator = jsonObject.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, JsonNode> keyValuePair = enumerator.Current;
						if (keyValuePair.Value != null)
						{
							SharedMigrationHelper.RecursiveRemoveSchema(keyValuePair.Value, depth + 1);
						}
					}
					return;
				}
			}
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray != null)
			{
				foreach (JsonNode jsonNode in jsonArray)
				{
					if (jsonNode != null)
					{
						SharedMigrationHelper.RecursiveRemoveSchema(jsonNode, depth + 1);
					}
				}
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0004810C File Offset: 0x0004630C
		[NullableContext(2)]
		public static void MigrateMapPointHistoryCardChoices(JsonNode jsonNode)
		{
			JsonArray jsonArray = jsonNode as JsonArray;
			if (jsonArray == null)
			{
				return;
			}
			foreach (JsonNode jsonNode2 in jsonArray)
			{
				JsonArray jsonArray2 = jsonNode2 as JsonArray;
				if (jsonArray2 != null)
				{
					foreach (JsonNode jsonNode3 in jsonArray2)
					{
						JsonObject jsonObject = jsonNode3 as JsonObject;
						JsonNode jsonNode4;
						if (jsonObject != null && jsonObject.TryGetPropertyValue("player_stats", out jsonNode4))
						{
							JsonArray jsonArray3 = jsonNode4 as JsonArray;
							if (jsonArray3 != null)
							{
								foreach (JsonNode jsonNode5 in jsonArray3)
								{
									JsonObject jsonObject2 = jsonNode5 as JsonObject;
									if (jsonObject2 != null)
									{
										JsonNode jsonNode6;
										if (jsonObject2.TryGetPropertyValue("cards_gained", out jsonNode6))
										{
											JsonArray jsonArray4 = jsonNode6 as JsonArray;
											if (jsonArray4 != null)
											{
												JsonArray jsonArray5 = new JsonArray(null);
												foreach (JsonNode jsonNode7 in jsonArray4)
												{
													JsonObject jsonObject3 = new JsonObject(null);
													jsonObject3["id"] = jsonNode7.GetValue<string>();
													jsonObject3["current_upgrade_level"] = 0;
													jsonArray5.Add(jsonObject3);
												}
												jsonNode5["cards_gained"] = jsonArray5;
											}
										}
										JsonNode jsonNode8;
										if (jsonObject2.TryGetPropertyValue("cards_removed", out jsonNode8))
										{
											JsonArray jsonArray6 = jsonNode8 as JsonArray;
											if (jsonArray6 != null)
											{
												JsonArray jsonArray7 = new JsonArray(null);
												foreach (JsonNode jsonNode9 in jsonArray6)
												{
													JsonObject jsonObject4 = new JsonObject(null);
													jsonObject4["id"] = jsonNode9.GetValue<string>();
													jsonObject4["current_upgrade_level"] = 0;
													jsonArray7.Add(jsonObject4);
												}
												jsonNode5["cards_removed"] = jsonArray7;
											}
										}
										JsonNode jsonNode10;
										if (jsonObject2.TryGetPropertyValue("card_choices", out jsonNode10))
										{
											JsonArray jsonArray8 = jsonNode10 as JsonArray;
											if (jsonArray8 == null)
											{
												continue;
											}
											foreach (JsonNode jsonNode11 in jsonArray8)
											{
												JsonObject jsonObject5 = jsonNode11 as JsonObject;
												if (jsonObject5 != null)
												{
													JsonObject jsonObject6 = new JsonObject(null);
													jsonObject6["id"] = jsonObject5["choice"].GetValue<string>();
													jsonObject6["current_upgrade_level"] = 0;
													jsonObject5["card"] = jsonObject6;
													jsonObject5.Remove("choice");
												}
											}
										}
										JsonNode jsonNode12;
										if (jsonObject2.TryGetPropertyValue("cards_enchanted", out jsonNode12))
										{
											JsonArray jsonArray9 = jsonNode12 as JsonArray;
											if (jsonArray9 == null)
											{
												continue;
											}
											foreach (JsonNode jsonNode13 in jsonArray9)
											{
												JsonObject jsonObject7 = jsonNode13 as JsonObject;
												if (jsonObject7 != null)
												{
													JsonObject jsonObject8 = new JsonObject(null);
													jsonObject8["id"] = jsonObject7["card"].GetValue<string>();
													jsonObject8["current_upgrade_level"] = 0;
													jsonObject7["card"] = jsonObject8;
												}
											}
										}
										JsonNode jsonNode14;
										if (jsonObject2.TryGetPropertyValue("cards_transformed", out jsonNode14))
										{
											JsonArray jsonArray10 = jsonNode14 as JsonArray;
											if (jsonArray10 != null)
											{
												foreach (JsonNode jsonNode15 in jsonArray10)
												{
													JsonObject jsonObject9 = jsonNode15 as JsonObject;
													if (jsonObject9 != null)
													{
														JsonObject jsonObject10 = new JsonObject(null);
														jsonObject10["id"] = jsonObject9["original_card"].GetValue<string>();
														jsonObject10["current_upgrade_level"] = 0;
														jsonObject9["original_card"] = jsonObject10;
														JsonObject jsonObject11 = new JsonObject(null);
														jsonObject11["id"] = jsonObject9["final_card"].GetValue<string>();
														jsonObject11["current_upgrade_level"] = 0;
														jsonObject9["final_card"] = jsonObject11;
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

		// Token: 0x06000A8C RID: 2700 RVA: 0x00048698 File Offset: 0x00046898
		// Note: this type is marked as 'beforefieldinit'.
		static SharedMigrationHelper()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["CARD.PREPARE"] = "CARD.PREPARED";
			dictionary["ENCOUNTER.TOADPOLES_NORMAL"] = "ENCOUNTER.SEAPUNK_NORMAL";
			dictionary["MONSTER.DOOR"] = "MONSTER.DEPRECATED_MONSTER";
			SharedMigrationHelper.v100Renames = dictionary;
		}

		/// <summary>
		/// Renames used by SerializableRun V15-&gt;V16 and RunHistory V8-&gt;V9.
		/// </summary>
		// Token: 0x040003A7 RID: 935
		public static readonly IReadOnlyDictionary<string, string> v100Renames;
	}
}
