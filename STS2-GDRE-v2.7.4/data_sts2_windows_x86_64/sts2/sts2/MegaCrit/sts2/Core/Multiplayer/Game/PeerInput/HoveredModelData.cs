using System;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput
{
	/// <summary>
	/// A bundle of data that resolves to a model that a player is hovering.
	/// This intermediate structure is necessary because references to models may not yet be available at the time that we
	/// receive the hover data. For example:
	/// - Player 1 is in fast mode, plays Alchemize, and hovers the resulting potion
	/// - Player 2 is in slow mode, and is still in the middle of the enemy turn when player 1 hovers the potion
	/// </summary>
	// Token: 0x020004AA RID: 1194
	public struct HoveredModelData : IPacketSerializable, IEquatable<HoveredModelData>
	{
		// Token: 0x060047F8 RID: 18424 RVA: 0x0021FF30 File Offset: 0x0021E130
		public bool Equals(HoveredModelData other)
		{
			if (this.type == other.type && this.hoveredCombatCard == other.hoveredCombatCard)
			{
				int? num = this.hoveredRelicIndex;
				int? num2 = other.hoveredRelicIndex;
				if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
				{
					num2 = this.hoveredPotionIndex;
					num = other.hoveredPotionIndex;
					if ((num2.GetValueOrDefault() == num.GetValueOrDefault()) & (num2 != null == (num != null)))
					{
						return this.hoveredModelId == other.hoveredModelId;
					}
				}
			}
			return false;
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x00220008 File Offset: 0x0021E208
		[NullableContext(2)]
		public static HoveredModelData FromModel(AbstractModel model)
		{
			if (model == null)
			{
				return default(HoveredModelData);
			}
			HoveredModelData hoveredModelData = default(HoveredModelData);
			CardModel cardModel = model as CardModel;
			if (cardModel == null)
			{
				RelicModel relicModel = model as RelicModel;
				if (relicModel == null)
				{
					PotionModel potionModel = model as PotionModel;
					if (potionModel == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Model ");
						defaultInterpolatedStringHandler.AppendFormatted<AbstractModel>(model);
						defaultInterpolatedStringHandler.AppendLiteral(" has unsupported type for hovering");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					hoveredModelData.type = HoveredModelType.Potion;
					hoveredModelData.hoveredPotionIndex = new int?(potionModel.Owner.GetPotionSlotIndex(potionModel));
				}
				else
				{
					hoveredModelData.type = HoveredModelType.Relic;
					hoveredModelData.hoveredRelicIndex = new int?(relicModel.Owner.Relics.IndexOf(relicModel));
				}
			}
			else
			{
				hoveredModelData.type = HoveredModelType.Card;
				hoveredModelData.hoveredCombatCard = new NetCombatCard?(NetCombatCard.FromModel(cardModel));
			}
			hoveredModelData.hoveredModelId = model.Id;
			return hoveredModelData;
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x002200FC File Offset: 0x0021E2FC
		[NullableContext(1)]
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<HoveredModelType>(this.type);
			switch (this.type)
			{
			case HoveredModelType.Card:
				writer.Write<NetCombatCard>(this.hoveredCombatCard.Value);
				break;
			case HoveredModelType.Relic:
				writer.WriteInt(this.hoveredRelicIndex.Value, 8);
				break;
			case HoveredModelType.Potion:
				writer.WriteInt(this.hoveredPotionIndex.Value, 4);
				break;
			}
			if (this.type != HoveredModelType.None)
			{
				writer.WriteModelEntry(this.hoveredModelId);
			}
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x00220180 File Offset: 0x0021E380
		[NullableContext(1)]
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<HoveredModelType>();
			switch (this.type)
			{
			case HoveredModelType.Card:
				this.hoveredCombatCard = new NetCombatCard?(reader.Read<NetCombatCard>());
				this.hoveredModelId = reader.ReadModelIdAssumingType<CardModel>();
				return;
			case HoveredModelType.Relic:
				this.hoveredRelicIndex = new int?(reader.ReadInt(8));
				this.hoveredModelId = reader.ReadModelIdAssumingType<RelicModel>();
				return;
			case HoveredModelType.Potion:
				this.hoveredPotionIndex = new int?(reader.ReadInt(4));
				this.hoveredModelId = reader.ReadModelIdAssumingType<PotionModel>();
				return;
			default:
				return;
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x00220210 File Offset: 0x0021E410
		[NullableContext(1)]
		public override string ToString()
		{
			string text;
			switch (this.type)
			{
			case HoveredModelType.None:
				text = "HoveredModelData none";
				break;
			case HoveredModelType.Card:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
				defaultInterpolatedStringHandler.AppendLiteral("HoveredModelData ");
				defaultInterpolatedStringHandler.AppendFormatted<NetCombatCard?>(this.hoveredCombatCard);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				break;
			}
			case HoveredModelType.Relic:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("HoveredModelData relic index ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(this.hoveredRelicIndex);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				break;
			}
			case HoveredModelType.Potion:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("HoveredModelData potion index ");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(this.hoveredPotionIndex);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return text;
		}

		// Token: 0x04001C3F RID: 7231
		public HoveredModelType type;

		// Token: 0x04001C40 RID: 7232
		public NetCombatCard? hoveredCombatCard;

		// Token: 0x04001C41 RID: 7233
		public int? hoveredRelicIndex;

		// Token: 0x04001C42 RID: 7234
		public int? hoveredPotionIndex;

		// Token: 0x04001C43 RID: 7235
		[Nullable(2)]
		public ModelId hoveredModelId;
	}
}
