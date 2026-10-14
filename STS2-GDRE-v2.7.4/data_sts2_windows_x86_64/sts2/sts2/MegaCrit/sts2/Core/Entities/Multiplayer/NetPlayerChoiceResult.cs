using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Models;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	/// <summary>
	/// Serializable version of PlayerChoiceResult, representing the result of a choice the player made mid-combat.
	/// </summary>
	// Token: 0x02000C8C RID: 3212
	[NullableContext(1)]
	[Nullable(0)]
	public struct NetPlayerChoiceResult : IPacketSerializable
	{
		// Token: 0x0600838C RID: 33676 RVA: 0x002C6C18 File Offset: 0x002C4E18
		public void Serialize(PacketWriter writer)
		{
			writer.WriteEnum<PlayerChoiceType>(this.type);
			switch (this.type)
			{
			case PlayerChoiceType.CanonicalCard:
			{
				writer.WriteInt(this.canonicalCards.Count, 32);
				using (List<CardModel>.Enumerator enumerator = this.canonicalCards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CardModel cardModel = enumerator.Current;
						writer.WriteModel(cardModel);
					}
					return;
				}
				break;
			}
			case PlayerChoiceType.CombatCard:
				break;
			case PlayerChoiceType.DeckCard:
				writer.WriteList<NetDeckCard>(this.deckCards, 32);
				return;
			case PlayerChoiceType.MutableCard:
				writer.WriteList<SerializableCard>(this.mutableCards, 32);
				writer.WriteBool(this.mutableCardOwner != null);
				if (this.mutableCardOwner != null)
				{
					writer.WriteULong(this.mutableCardOwner.Value, 64);
					return;
				}
				return;
			case PlayerChoiceType.Player:
				writer.WriteBool(this.playerId != null);
				if (this.playerId != null)
				{
					writer.WriteULong(this.playerId.Value, 64);
					return;
				}
				return;
			case PlayerChoiceType.Index:
			{
				writer.WriteInt(this.indexes.Count, 32);
				using (List<int>.Enumerator enumerator2 = this.indexes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int num = enumerator2.Current;
						writer.WriteInt(num, 32);
					}
					return;
				}
				goto IL_0164;
			}
			default:
				goto IL_0164;
			}
			writer.WriteList<NetCombatCard>(this.combatCards, 32);
			return;
			IL_0164:
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x002C6DAC File Offset: 0x002C4FAC
		public void Deserialize(PacketReader reader)
		{
			this.type = reader.ReadEnum<PlayerChoiceType>();
			switch (this.type)
			{
			case PlayerChoiceType.CanonicalCard:
			{
				this.canonicalCards = new List<CardModel>();
				int num = reader.ReadInt(32);
				for (int i = 0; i < num; i++)
				{
					this.canonicalCards.Add(reader.ReadModel<CardModel>());
				}
				return;
			}
			case PlayerChoiceType.CombatCard:
				this.combatCards = reader.ReadList<NetCombatCard>(32);
				return;
			case PlayerChoiceType.DeckCard:
				this.deckCards = reader.ReadList<NetDeckCard>(32);
				return;
			case PlayerChoiceType.MutableCard:
				this.mutableCards = reader.ReadList<SerializableCard>(32);
				if (reader.ReadBool())
				{
					this.mutableCardOwner = new ulong?(reader.ReadULong(64));
					return;
				}
				break;
			case PlayerChoiceType.Player:
				if (reader.ReadBool())
				{
					this.playerId = new ulong?(reader.ReadULong(64));
					return;
				}
				break;
			case PlayerChoiceType.Index:
			{
				this.indexes = new List<int>();
				int num2 = reader.ReadInt(32);
				for (int j = 0; j < num2; j++)
				{
					this.indexes.Add(reader.ReadInt(32));
				}
				return;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x002C6EC8 File Offset: 0x002C50C8
		public override string ToString()
		{
			switch (this.type)
			{
			case PlayerChoiceType.CanonicalCard:
				return "NetPlayerChoiceResult canonical " + string.Join<CardModel>(",", this.canonicalCards ?? new List<CardModel>());
			case PlayerChoiceType.CombatCard:
				return "NetPlayerChoiceResult combat " + string.Join<NetCombatCard>(",", this.combatCards ?? new List<NetCombatCard>());
			case PlayerChoiceType.DeckCard:
				return "NetPlayerChoiceResult deck " + string.Join<NetDeckCard>(",", this.deckCards ?? new List<NetDeckCard>());
			case PlayerChoiceType.MutableCard:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 3);
				defaultInterpolatedStringHandler.AppendFormatted("NetPlayerChoiceResult");
				defaultInterpolatedStringHandler.AppendLiteral(" mutable cards ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join<SerializableCard>(",", this.mutableCards ?? new List<SerializableCard>()));
				defaultInterpolatedStringHandler.AppendLiteral(", owner: ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this.mutableCardOwner);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			case PlayerChoiceType.Player:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
				defaultInterpolatedStringHandler.AppendFormatted("NetPlayerChoiceResult");
				defaultInterpolatedStringHandler.AppendLiteral(" player ID ");
				defaultInterpolatedStringHandler.AppendFormatted<ulong?>(this.playerId);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			case PlayerChoiceType.Index:
				return "NetPlayerChoiceResult indexes " + string.Join<int>(",", this.indexes ?? new List<int>());
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04002CBE RID: 11454
		public PlayerChoiceType type;

		// Token: 0x04002CBF RID: 11455
		[Nullable(new byte[] { 2, 1 })]
		public List<CardModel> canonicalCards;

		// Token: 0x04002CC0 RID: 11456
		[Nullable(2)]
		public List<NetCombatCard> combatCards;

		// Token: 0x04002CC1 RID: 11457
		[Nullable(2)]
		public List<NetDeckCard> deckCards;

		// Token: 0x04002CC2 RID: 11458
		[Nullable(new byte[] { 2, 1 })]
		public List<SerializableCard> mutableCards;

		// Token: 0x04002CC3 RID: 11459
		public ulong? mutableCardOwner;

		// Token: 0x04002CC4 RID: 11460
		[Nullable(2)]
		public List<int> indexes;

		// Token: 0x04002CC5 RID: 11461
		public ulong? playerId;
	}
}
