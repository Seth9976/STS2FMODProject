using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Runs.History
{
	// Token: 0x02000121 RID: 289
	[NullableContext(1)]
	[Nullable(0)]
	public struct EventOptionHistoryEntry : IPacketSerializable
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0005AF95 File Offset: 0x00059195
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x0005AF9D File Offset: 0x0005919D
		[JsonPropertyName("title")]
		public LocString Title { readonly get; set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0005AFA6 File Offset: 0x000591A6
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x0005AFAE File Offset: 0x000591AE
		[Nullable(new byte[] { 2, 1, 1 })]
		[JsonInclude]
		[JsonPropertyName("variables")]
		[JsonConverter(typeof(LocStringVariablesJsonConverter))]
		[JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
		public Dictionary<string, object> Variables
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			readonly get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0005AFB8 File Offset: 0x000591B8
		public void Serialize(PacketWriter writer)
		{
			writer.WriteString(this.Title.LocTable);
			writer.WriteString(this.Title.LocEntryKey);
			writer.WriteBool(this.Variables != null);
			if (this.Variables != null)
			{
				List<ValueTuple<string, SerializableDynamicVar>> list = new List<ValueTuple<string, SerializableDynamicVar>>();
				foreach (KeyValuePair<string, object> keyValuePair in this.Variables)
				{
					SerializableDynamicVar? serializableDynamicVar = SerializableDynamicVar.FromDynamicVar(keyValuePair.Value);
					if (serializableDynamicVar != null)
					{
						list.Add(new ValueTuple<string, SerializableDynamicVar>(keyValuePair.Key, serializableDynamicVar.Value));
					}
				}
				writer.WriteInt(list.Count, 32);
				foreach (ValueTuple<string, SerializableDynamicVar> valueTuple in list)
				{
					string item = valueTuple.Item1;
					SerializableDynamicVar item2 = valueTuple.Item2;
					writer.WriteString(item);
					writer.Write<SerializableDynamicVar>(item2);
				}
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0005B0D8 File Offset: 0x000592D8
		public void Deserialize(PacketReader reader)
		{
			string text = reader.ReadString();
			string text2 = reader.ReadString();
			this.Title = new LocString(text, text2);
			if (reader.ReadBool())
			{
				this.Variables = new Dictionary<string, object>();
				int num = reader.ReadInt(32);
				for (int i = 0; i < num; i++)
				{
					string text3 = reader.ReadString();
					this.Variables[text3] = reader.Read<SerializableDynamicVar>().ToDynamicVar(text3);
				}
			}
		}
	}
}
