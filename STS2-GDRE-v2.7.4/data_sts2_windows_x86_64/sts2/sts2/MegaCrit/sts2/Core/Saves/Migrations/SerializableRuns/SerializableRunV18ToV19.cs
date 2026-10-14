using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// Upgrades counter-based RNGs to the new method of saving RNG state.
	/// </summary>
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 18, 19)]
	public class SerializableRunV18ToV19 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x0004903C File Offset: 0x0004723C
		protected override void ApplyMigration(MigratingData saveData)
		{
			this.MigrateRunRngs(saveData);
			this.MigratePlayerRngs(saveData);
			Log.Info("SerializableRun migration v18 -> v19: Upgraded RNG serialization", 2);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00049058 File Offset: 0x00047258
		private void MigrateRunRngs(MigratingData saveData)
		{
			JsonObject jsonObject = saveData.GetRawNode("rng") as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			JsonValue jsonValue = jsonObject["seed"] as JsonValue;
			string text;
			if (jsonValue == null || !jsonValue.TryGetValue<string>(out text))
			{
				return;
			}
			JsonObject jsonObject2 = jsonObject["counters"] as JsonObject;
			if (jsonObject2 == null)
			{
				return;
			}
			JsonObject jsonObject3 = new JsonObject(null);
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
			{
				JsonValue jsonValue2 = keyValuePair.Value as JsonValue;
				int num;
				RunRngType runRngType;
				if (jsonValue2 != null && jsonValue2.TryGetValue<int>(out num) && Enum.TryParse<RunRngType>(keyValuePair.Key, out runRngType))
				{
					string text2 = StringHelper.SnakeCase(runRngType.ToString());
					MegaRandom megaRandom = SerializableRunV18ToV19.GenerateRng((ulong)((long)StringHelper.GetDeterministicHashCodeOld(text)), text2, num);
					SerializableRng serializableRng = new SerializableRng
					{
						counter = num
					};
					megaRandom.FillSerializableState(serializableRng);
					string text3 = JsonSerializer.Serialize<SerializableRng>(serializableRng, JsonSerializationUtility.GetTypeInfo<SerializableRng>());
					jsonObject3[keyValuePair.Key] = JsonNode.Parse(text3, null, default(JsonDocumentOptions));
				}
			}
			jsonObject.Remove("counters");
			jsonObject["rngs"] = jsonObject3;
			jsonObject["seed"] = "old" + text;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000491DC File Offset: 0x000473DC
		private void MigratePlayerRngs(MigratingData saveData)
		{
			JsonArray jsonArray = saveData.GetRawNode("players") as JsonArray;
			if (jsonArray == null)
			{
				return;
			}
			foreach (JsonNode jsonNode in jsonArray)
			{
				JsonObject jsonObject = ((jsonNode != null) ? jsonNode["rng"] : null) as JsonObject;
				if (jsonObject != null)
				{
					JsonValue jsonValue = jsonObject["seed"] as JsonValue;
					ulong num;
					if (jsonValue != null && jsonValue.TryGetValue<ulong>(out num))
					{
						JsonObject jsonObject2 = jsonObject["counters"] as JsonObject;
						if (jsonObject2 != null)
						{
							JsonObject jsonObject3 = new JsonObject(null);
							foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
							{
								JsonValue jsonValue2 = keyValuePair.Value as JsonValue;
								int num2;
								PlayerRngType playerRngType;
								if (jsonValue2 != null && jsonValue2.TryGetValue<int>(out num2) && Enum.TryParse<PlayerRngType>(keyValuePair.Key, out playerRngType))
								{
									string text = StringHelper.SnakeCase(playerRngType.ToString());
									MegaRandom megaRandom = SerializableRunV18ToV19.GenerateRng(num, text, num2);
									SerializableRng serializableRng = new SerializableRng
									{
										counter = num2
									};
									megaRandom.FillSerializableState(serializableRng);
									string text2 = JsonSerializer.Serialize<SerializableRng>(serializableRng, JsonSerializationUtility.GetTypeInfo<SerializableRng>());
									jsonObject3[keyValuePair.Key] = JsonNode.Parse(text2, null, default(JsonDocumentOptions));
								}
							}
							jsonObject.Remove("counters");
							jsonObject["rngs"] = jsonObject3;
						}
					}
				}
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000493B8 File Offset: 0x000475B8
		public static MegaRandom GenerateRng(ulong seed, string type, int counter)
		{
			uint num = (uint)seed + (uint)StringHelper.GetDeterministicHashCodeOld(type);
			MegaRandom megaRandom = new MegaRandom((ulong)num);
			for (int i = 0; i < counter; i++)
			{
				megaRandom.NextULong();
			}
			return megaRandom;
		}

		// Token: 0x040003AE RID: 942
		public const string seedPrefix = "old";
	}
}
