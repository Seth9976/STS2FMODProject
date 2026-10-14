using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns
{
	/// <summary>
	/// Remaps a pre-finished Battleworn Dummy combat room from the old single
	/// BattlewornDummyEventEncounter (now abstract, split into V1/V2/V3) to the concrete variant that
	/// matches the difficulty the player picked. Without this, loading such a run save resolves the
	/// now-missing encounter id to DeprecatedEncounter, and BattlewornDummy.Resume hard-casts it to
	/// BattlewornDummyEventEncounter, throwing InvalidCastException and soft-locking the run.
	///
	/// The old encounter recorded the chosen difficulty (and therefore the reward: V1 potion,
	/// V2 card upgrades, V3 relic) in its custom state under "Setting". We map that to the matching
	/// concrete encounter so the resumed event grants the correct reward, then drop the stale key that
	/// the new encounters no longer read.
	/// </summary>
	// Token: 0x020000EC RID: 236
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1 })]
	[Migration(typeof(SerializableRun), 17, 18)]
	public class SerializableRunV17ToV18 : MigrationBase<SerializableRun>
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x00048EF4 File Offset: 0x000470F4
		protected override void ApplyMigration(MigratingData saveData)
		{
			JsonObject jsonObject = saveData.GetRawNode("pre_finished_room") as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			JsonValue jsonValue = jsonObject["encounter_id"] as JsonValue;
			string text;
			if (jsonValue == null || !jsonValue.TryGetValue<string>(out text) || !string.Equals(text, "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			JsonObject jsonObject2 = jsonObject["encounter_state"] as JsonObject;
			JsonValue jsonValue2 = ((jsonObject2 != null) ? jsonObject2["Setting"] : null) as JsonValue;
			string text3;
			string text2 = ((jsonValue2 != null && jsonValue2.TryGetValue<string>(out text3)) ? text3 : null);
			string text4;
			if (!(text2 == "Setting2"))
			{
				if (!(text2 == "Setting3"))
				{
					text4 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V1_ENCOUNTER";
				}
				else
				{
					text4 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V3_ENCOUNTER";
				}
			}
			else
			{
				text4 = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V2_ENCOUNTER";
			}
			string text5 = text4;
			jsonObject["encounter_id"] = text5;
			if (jsonObject2 != null)
			{
				jsonObject2.Remove("Setting");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(89, 2);
			defaultInterpolatedStringHandler.AppendLiteral("SerializableRun migration v17 -> v18: remapped Battleworn Dummy encounter to ");
			defaultInterpolatedStringHandler.AppendFormatted(text5);
			defaultInterpolatedStringHandler.AppendLiteral(" (setting: ");
			defaultInterpolatedStringHandler.AppendFormatted(text2 ?? "none");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
		}

		// Token: 0x040003A9 RID: 937
		private const string _oldEncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_ENCOUNTER";

		// Token: 0x040003AA RID: 938
		private const string _v1EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V1_ENCOUNTER";

		// Token: 0x040003AB RID: 939
		private const string _v2EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V2_ENCOUNTER";

		// Token: 0x040003AC RID: 940
		private const string _v3EncounterId = "ENCOUNTER.BATTLEWORN_DUMMY_EVENT_V3_ENCOUNTER";

		// Token: 0x040003AD RID: 941
		private const string _settingKey = "Setting";
	}
}
