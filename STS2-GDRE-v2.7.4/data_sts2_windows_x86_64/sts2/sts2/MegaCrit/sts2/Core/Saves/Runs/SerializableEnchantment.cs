using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Saves.Runs
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(2)]
	[Nullable(0)]
	public class SerializableEnchantment : IPacketSerializable
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x000442C7 File Offset: 0x000424C7
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x000442CF File Offset: 0x000424CF
		[JsonPropertyName("id")]
		public ModelId Id { get; set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x000442D8 File Offset: 0x000424D8
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x000442E0 File Offset: 0x000424E0
		[JsonPropertyName("amount")]
		public int Amount { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x000442E9 File Offset: 0x000424E9
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x000442F1 File Offset: 0x000424F1
		[JsonPropertyName("props")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public SavedProperties Props { get; set; }

		// Token: 0x06000975 RID: 2421 RVA: 0x000442FA File Offset: 0x000424FA
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteModelEntry(this.Id);
			writer.WriteInt(this.Amount, 8);
			writer.WriteBool(this.Props != null);
			if (this.Props != null)
			{
				writer.Write<SavedProperties>(this.Props);
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00044338 File Offset: 0x00042538
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.Id = reader.ReadModelIdAssumingType<EnchantmentModel>();
			this.Amount = reader.ReadInt(8);
			bool flag = reader.ReadBool();
			if (flag)
			{
				this.Props = reader.Read<SavedProperties>();
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00044374 File Offset: 0x00042574
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			SerializableEnchantment serializableEnchantment = (SerializableEnchantment)obj;
			return this.Id.Equals(serializableEnchantment.Id) && this.Amount.Equals(serializableEnchantment.Amount);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000443CB File Offset: 0x000425CB
		public override int GetHashCode()
		{
			return HashCode.Combine<ModelId, int>(this.Id, this.Amount);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000443E0 File Offset: 0x000425E0
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
			defaultInterpolatedStringHandler.AppendLiteral("SerializableEnchantment ");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral(" with amount ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Amount);
			defaultInterpolatedStringHandler.AppendLiteral(" Props: ");
			defaultInterpolatedStringHandler.AppendFormatted<SavedProperties>(this.Props);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
