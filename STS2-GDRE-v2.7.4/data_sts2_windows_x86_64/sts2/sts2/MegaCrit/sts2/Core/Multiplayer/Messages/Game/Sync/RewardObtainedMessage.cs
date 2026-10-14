using System;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync
{
	// Token: 0x02000484 RID: 1156
	[RequiredMember]
	public struct RewardObtainedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
	{
		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06004697 RID: 18071 RVA: 0x0021895C File Offset: 0x00216B5C
		public bool ShouldBroadcast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06004698 RID: 18072 RVA: 0x0021895F File Offset: 0x00216B5F
		public NetTransferMode Mode
		{
			get
			{
				return NetTransferMode.Reliable;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x00218962 File Offset: 0x00216B62
		public LogLevel LogLevel
		{
			get
			{
				return LogLevel.VeryDebug;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x0600469A RID: 18074 RVA: 0x00218965 File Offset: 0x00216B65
		public bool ShouldBuffer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x0600469B RID: 18075 RVA: 0x00218968 File Offset: 0x00216B68
		public RunLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x00218970 File Offset: 0x00216B70
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<RewardType>(this.rewardType);
			writer.Write<RunLocation>(this.location);
			switch (this.rewardType)
			{
			case RewardType.Card:
				writer.Write<SerializableCard>(this.cardModel.ToSerializable());
				break;
			case RewardType.Gold:
				writer.WriteInt(this.goldAmount.Value, 32);
				break;
			case RewardType.Potion:
				writer.WriteModelEntry(this.potionModel.Id);
				break;
			case RewardType.Relic:
				writer.Write<SerializableRelic>(this.relicModel.ToSerializable());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			writer.WriteBool(this.wasSkipped);
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x00218A18 File Offset: 0x00216C18
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.rewardType = reader.ReadEnum<RewardType>();
			this.location = reader.Read<RunLocation>();
			switch (this.rewardType)
			{
			case RewardType.Card:
				this.cardModel = CardModel.FromSerializable(reader.Read<SerializableCard>());
				break;
			case RewardType.Gold:
				this.goldAmount = new int?(reader.ReadInt(32));
				break;
			case RewardType.Potion:
				this.potionModel = ModelDb.GetById<PotionModel>(reader.ReadModelIdAssumingType<PotionModel>());
				break;
			case RewardType.Relic:
				this.relicModel = RelicModel.FromSerializable(reader.Read<SerializableRelic>());
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.wasSkipped = reader.ReadBool();
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00218AC0 File Offset: 0x00216CC0
		[NullableContext(1)]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(29, 4, stringBuilder2);
			appendInterpolatedStringHandler.AppendFormatted("RewardObtainedMessage");
			appendInterpolatedStringHandler.AppendLiteral(" type: ");
			appendInterpolatedStringHandler.AppendFormatted<RewardType>(this.rewardType);
			appendInterpolatedStringHandler.AppendLiteral(" location: ");
			appendInterpolatedStringHandler.AppendFormatted<RunLocation>(this.location);
			appendInterpolatedStringHandler.AppendLiteral(" skipped: ");
			appendInterpolatedStringHandler.AppendFormatted<bool>(this.wasSkipped);
			appendInterpolatedStringHandler.AppendLiteral(" ");
			stringBuilder3.Append(ref appendInterpolatedStringHandler);
			switch (this.rewardType)
			{
			case RewardType.Card:
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Card: ");
				appendInterpolatedStringHandler.AppendFormatted<CardModel>(this.cardModel);
				stringBuilder4.Append(ref appendInterpolatedStringHandler);
				break;
			}
			case RewardType.Gold:
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Gold: ");
				appendInterpolatedStringHandler.AppendFormatted<int?>(this.goldAmount);
				stringBuilder5.Append(ref appendInterpolatedStringHandler);
				break;
			}
			case RewardType.Potion:
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder6 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Potion: ");
				appendInterpolatedStringHandler.AppendFormatted<PotionModel>(this.potionModel);
				stringBuilder6.Append(ref appendInterpolatedStringHandler);
				break;
			}
			case RewardType.Relic:
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder7 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Relic: ");
				appendInterpolatedStringHandler.AppendFormatted<RelicModel>(this.relicModel);
				stringBuilder7.Append(ref appendInterpolatedStringHandler);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001BA0 RID: 7072
		[RequiredMember]
		public RewardType rewardType;

		// Token: 0x04001BA1 RID: 7073
		[RequiredMember]
		public RunLocation location;

		// Token: 0x04001BA2 RID: 7074
		[Nullable(2)]
		public CardModel cardModel;

		// Token: 0x04001BA3 RID: 7075
		[Nullable(2)]
		public PotionModel potionModel;

		// Token: 0x04001BA4 RID: 7076
		[Nullable(2)]
		public RelicModel relicModel;

		// Token: 0x04001BA5 RID: 7077
		public int? goldAmount;

		// Token: 0x04001BA6 RID: 7078
		[RequiredMember]
		public bool wasSkipped;
	}
}
