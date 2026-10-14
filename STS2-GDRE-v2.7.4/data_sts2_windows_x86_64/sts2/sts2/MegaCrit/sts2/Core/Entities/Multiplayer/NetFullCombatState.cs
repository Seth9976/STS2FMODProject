using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Entities.Multiplayer
{
	// Token: 0x02000C8B RID: 3211
	[NullableContext(1)]
	[Nullable(0)]
	public class NetFullCombatState : IPacketSerializable
	{
		// Token: 0x170023E9 RID: 9193
		// (get) Token: 0x06008380 RID: 33664 RVA: 0x002C591B File Offset: 0x002C3B1B
		// (set) Token: 0x06008381 RID: 33665 RVA: 0x002C5923 File Offset: 0x002C3B23
		public List<NetFullCombatState.CreatureState> Creatures { get; private set; } = new List<NetFullCombatState.CreatureState>();

		// Token: 0x170023EA RID: 9194
		// (get) Token: 0x06008382 RID: 33666 RVA: 0x002C592C File Offset: 0x002C3B2C
		// (set) Token: 0x06008383 RID: 33667 RVA: 0x002C5934 File Offset: 0x002C3B34
		public List<NetFullCombatState.PlayerState> Players { get; private set; } = new List<NetFullCombatState.PlayerState>();

		// Token: 0x170023EB RID: 9195
		// (get) Token: 0x06008384 RID: 33668 RVA: 0x002C593D File Offset: 0x002C3B3D
		// (set) Token: 0x06008385 RID: 33669 RVA: 0x002C5945 File Offset: 0x002C3B45
		public SerializableRunRngSet Rng { get; private set; }

		// Token: 0x06008386 RID: 33670 RVA: 0x002C5950 File Offset: 0x002C3B50
		public static NetFullCombatState FromRun(IRunState runState, [Nullable(2)] GameAction justFinishedAction)
		{
			NetFullCombatState netFullCombatState = new NetFullCombatState
			{
				nextChoiceIds = new List<uint>(),
				nextRewardIds = new List<int>(),
				Creatures = new List<NetFullCombatState.CreatureState>(),
				Players = new List<NetFullCombatState.PlayerState>(),
				Rng = runState.Rng.ToSerializable()
			};
			netFullCombatState.nextChoiceIds.AddRange(RunManager.Instance.PlayerChoiceSynchronizer.ChoiceIds);
			netFullCombatState.nextRewardIds.AddRange(RunManager.Instance.RewardsSetSynchronizer.GetNextRewardIds());
			NetFullCombatState netFullCombatState2 = netFullCombatState;
			GenericHookGameAction genericHookGameAction = justFinishedAction as GenericHookGameAction;
			netFullCombatState2.lastExecutedHookId = ((genericHookGameAction != null) ? new uint?(genericHookGameAction.HookId) : null);
			netFullCombatState.lastExecutedActionId = ((justFinishedAction != null) ? justFinishedAction.Id : null);
			ICombatState combatState = runState.Players[0].Creature.CombatState;
			IReadOnlyList<Creature> readOnlyList = ((combatState != null) ? combatState.Creatures : null) ?? Array.Empty<Creature>();
			foreach (Creature creature in readOnlyList)
			{
				NetFullCombatState.CreatureState creatureState = default(NetFullCombatState.CreatureState);
				MonsterModel monster = creature.Monster;
				creatureState.monsterId = ((monster != null) ? monster.Id : null);
				Player player = creature.Player;
				creatureState.playerId = ((player != null) ? new ulong?(player.NetId) : null);
				creatureState.currentHp = creature.CurrentHp;
				creatureState.maxHp = creature.MaxHp;
				creatureState.block = creature.Block;
				creatureState.powers = new List<NetFullCombatState.PowerState>();
				NetFullCombatState.CreatureState creatureState2 = creatureState;
				foreach (PowerModel powerModel in creature.Powers)
				{
					creatureState2.powers.Add(new NetFullCombatState.PowerState
					{
						id = powerModel.Id,
						amount = powerModel.Amount
					});
				}
				netFullCombatState.Creatures.Add(creatureState2);
			}
			foreach (Player player2 in runState.Players)
			{
				PlayerCombatState playerCombatState = player2.PlayerCombatState;
				NetFullCombatState.PlayerState playerState = new NetFullCombatState.PlayerState
				{
					playerId = player2.NetId,
					characterId = player2.Character.Id,
					turnNumber = ((playerCombatState != null) ? playerCombatState.TurnNumber : 0),
					phase = ((playerCombatState != null) ? playerCombatState.Phase : PlayerTurnPhase.None),
					energy = ((playerCombatState != null) ? playerCombatState.Energy : 0),
					stars = ((playerCombatState != null) ? playerCombatState.Stars : 0),
					maxPotionCount = player2.MaxPotionCount,
					gold = player2.Gold,
					piles = new List<NetFullCombatState.CombatPileState>(),
					potions = new List<NetFullCombatState.PotionState>(),
					relics = new List<NetFullCombatState.RelicState>(),
					orbs = new List<NetFullCombatState.OrbState>(),
					rngSet = player2.PlayerRng.ToSerializable(),
					relicGrabBag = player2.RelicGrabBag.ToSerializable()
				};
				playerState.rngSet.Rngs.Remove(PlayerRngType.Rewards);
				playerState.rngSet.Rngs.Remove(PlayerRngType.Shops);
				if (playerCombatState != null && CombatManager.Instance.IsInProgress)
				{
					playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.Hand));
					playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.DrawPile));
					playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.DiscardPile));
					playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.ExhaustPile));
					playerState.piles.Add(NetFullCombatState.CombatPileState.From(playerCombatState.PlayPile));
					List<NetFullCombatState.OrbState> orbs = playerState.orbs;
					IEnumerable<OrbModel> orbs2 = playerCombatState.OrbQueue.Orbs;
					Func<OrbModel, NetFullCombatState.OrbState> func;
					if ((func = NetFullCombatState.<>O.<0>__From) == null)
					{
						func = (NetFullCombatState.<>O.<0>__From = new Func<OrbModel, NetFullCombatState.OrbState>(NetFullCombatState.OrbState.From));
					}
					orbs.AddRange(orbs2.Select<OrbModel, NetFullCombatState.OrbState>(func));
				}
				foreach (PotionModel potionModel in player2.Potions)
				{
					playerState.potions.Add(new NetFullCombatState.PotionState
					{
						id = potionModel.Id
					});
				}
				foreach (RelicModel relicModel in player2.Relics)
				{
					playerState.relics.Add(new NetFullCombatState.RelicState
					{
						relic = relicModel.ToSerializable()
					});
				}
				netFullCombatState.Players.Add(playerState);
			}
			return netFullCombatState;
		}

		// Token: 0x06008387 RID: 33671 RVA: 0x002C5EB8 File Offset: 0x002C40B8
		public void Serialize(PacketWriter writer)
		{
			writer.WriteList<NetFullCombatState.CreatureState>(this.Creatures, 32);
			writer.WriteList<NetFullCombatState.PlayerState>(this.Players, 32);
			writer.Write<SerializableRunRngSet>(this.Rng);
			writer.WriteInt(this.nextChoiceIds.Count, 32);
			foreach (uint num in this.nextChoiceIds)
			{
				writer.WriteUInt(num, 32);
			}
			writer.WriteInt(this.nextRewardIds.Count, 32);
			foreach (int num2 in this.nextRewardIds)
			{
				writer.WriteInt(num2, 32);
			}
			writer.WriteBool(this.lastExecutedActionId != null);
			if (this.lastExecutedActionId != null)
			{
				writer.WriteUInt(this.lastExecutedActionId.Value, 32);
			}
			writer.WriteBool(this.lastExecutedHookId != null);
			if (this.lastExecutedHookId != null)
			{
				writer.WriteUInt(this.lastExecutedHookId.Value, 32);
			}
		}

		// Token: 0x06008388 RID: 33672 RVA: 0x002C6004 File Offset: 0x002C4204
		public void Deserialize(PacketReader reader)
		{
			this.Creatures = reader.ReadList<NetFullCombatState.CreatureState>(32);
			this.Players = reader.ReadList<NetFullCombatState.PlayerState>(32);
			this.Rng = reader.Read<SerializableRunRngSet>();
			int num = reader.ReadInt(32);
			this.nextChoiceIds = new List<uint>();
			for (int i = 0; i < num; i++)
			{
				this.nextChoiceIds.Add(reader.ReadUInt(32));
			}
			int num2 = reader.ReadInt(32);
			this.nextRewardIds = new List<int>();
			for (int j = 0; j < num2; j++)
			{
				this.nextRewardIds.Add(reader.ReadInt(32));
			}
			if (reader.ReadBool())
			{
				this.lastExecutedActionId = new uint?(reader.ReadUInt(32));
			}
			if (reader.ReadBool())
			{
				this.lastExecutedHookId = new uint?(reader.ReadUInt(32));
			}
		}

		// Token: 0x06008389 RID: 33673 RVA: 0x002C60D8 File Offset: 0x002C42D8
		public NetFullCombatState Anonymized()
		{
			NetFullCombatState netFullCombatState = new NetFullCombatState();
			netFullCombatState.Creatures = this.Creatures.Select<NetFullCombatState.CreatureState, NetFullCombatState.CreatureState>((NetFullCombatState.CreatureState c) => c.Anonymized()).ToList<NetFullCombatState.CreatureState>();
			netFullCombatState.Players = this.Players.Select<NetFullCombatState.PlayerState, NetFullCombatState.PlayerState>((NetFullCombatState.PlayerState p) => p.Anonymized()).ToList<NetFullCombatState.PlayerState>();
			netFullCombatState.Rng = this.Rng;
			netFullCombatState.nextChoiceIds = this.nextChoiceIds;
			netFullCombatState.nextRewardIds = this.nextRewardIds;
			netFullCombatState.lastExecutedHookId = this.lastExecutedHookId;
			netFullCombatState.lastExecutedActionId = this.lastExecutedActionId;
			return netFullCombatState;
		}

		// Token: 0x0600838A RID: 33674 RVA: 0x002C6190 File Offset: 0x002C4390
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Choice IDs: ");
			appendInterpolatedStringHandler.AppendFormatted(string.Join<uint>(",", this.nextChoiceIds));
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Reward IDs: ");
			appendInterpolatedStringHandler.AppendFormatted(string.Join<int>(",", this.nextRewardIds));
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(25, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Last executed action ID: ");
			appendInterpolatedStringHandler.AppendFormatted<uint?>(this.lastExecutedActionId);
			stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder6 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Last executed hook ID: ");
			appendInterpolatedStringHandler.AppendFormatted<uint?>(this.lastExecutedHookId);
			stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
			foreach (NetFullCombatState.CreatureState creatureState in this.Creatures)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder7 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(38, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Creature with monster ID: ");
				appendInterpolatedStringHandler.AppendFormatted<ModelId>(creatureState.monsterId);
				appendInterpolatedStringHandler.AppendLiteral(" player ID: ");
				appendInterpolatedStringHandler.AppendFormatted<ulong?>(creatureState.playerId);
				stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder8 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(44, 4, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\tCurrent HP: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(creatureState.currentHp);
				appendInterpolatedStringHandler.AppendLiteral(" Max HP: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(creatureState.maxHp);
				appendInterpolatedStringHandler.AppendLiteral(" Block: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(creatureState.block);
				appendInterpolatedStringHandler.AppendLiteral(" Power count: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(creatureState.powers.Count);
				stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
				foreach (NetFullCombatState.PowerState powerState in creatureState.powers)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder9 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tPower ");
					appendInterpolatedStringHandler.AppendFormatted<ModelId>(powerState.id);
					appendInterpolatedStringHandler.AppendLiteral(" Amount: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(powerState.amount);
					stringBuilder9.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			foreach (NetFullCombatState.PlayerState playerState in this.Players)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder10 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(28, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Player with ID: ");
				appendInterpolatedStringHandler.AppendFormatted<ulong>(playerState.playerId);
				appendInterpolatedStringHandler.AppendLiteral(" Character: ");
				appendInterpolatedStringHandler.AppendFormatted<ModelId>(playerState.characterId);
				stringBuilder10.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder11 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 2, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\tTurn: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.turnNumber);
				appendInterpolatedStringHandler.AppendLiteral(" Phase: ");
				appendInterpolatedStringHandler.AppendFormatted<PlayerTurnPhase>(playerState.phase);
				stringBuilder11.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder12 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(38, 4, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\tEnergy: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.energy);
				appendInterpolatedStringHandler.AppendLiteral(" Stars: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.stars);
				appendInterpolatedStringHandler.AppendLiteral(" Max Potions: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.maxPotionCount);
				appendInterpolatedStringHandler.AppendLiteral(" Gold: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.gold);
				stringBuilder12.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder13 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(54, 4, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("\tPile Count: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.piles.Count);
				appendInterpolatedStringHandler.AppendLiteral(" Potion Count: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.potions.Count);
				appendInterpolatedStringHandler.AppendLiteral(" Relic Count: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.relics.Count);
				appendInterpolatedStringHandler.AppendLiteral(" Orb Count: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(playerState.orbs.Count);
				stringBuilder13.AppendLine(ref appendInterpolatedStringHandler);
				foreach (NetFullCombatState.CombatPileState combatPileState in playerState.piles)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder14 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(6, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tPile ");
					appendInterpolatedStringHandler.AppendFormatted<PileType>(combatPileState.pileType);
					stringBuilder14.AppendLine(ref appendInterpolatedStringHandler);
					foreach (NetFullCombatState.CardState cardState in combatPileState.cards)
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder15 = stringBuilder2;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(19, 1, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("\t\tSerialized card: ");
						appendInterpolatedStringHandler.AppendFormatted<SerializableCard>(cardState.card);
						stringBuilder15.AppendLine(ref appendInterpolatedStringHandler);
						if (cardState.affliction != null)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder16 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(34, 2, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("\t\t\tAffliction: ");
							appendInterpolatedStringHandler.AppendFormatted<ModelId>(cardState.affliction);
							appendInterpolatedStringHandler.AppendLiteral(" Affliction Count: ");
							appendInterpolatedStringHandler.AppendFormatted<int>(cardState.afflictionCount);
							stringBuilder16.AppendLine(ref appendInterpolatedStringHandler);
						}
						if (cardState.keywords != null)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder17 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("\t\t\tKeywords: ");
							appendInterpolatedStringHandler.AppendFormatted(string.Join<CardKeyword>(",", cardState.keywords));
							stringBuilder17.AppendLine(ref appendInterpolatedStringHandler);
						}
						if (cardState.energyCost != null)
						{
							stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder18 = stringBuilder2;
							appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("\t\t\tEnergy Cost: ");
							string text = ",";
							object obj = cardState.energyCost;
							appendInterpolatedStringHandler.AppendFormatted(string.Join(text, new ReadOnlySpan<object>(ref obj)));
							stringBuilder18.AppendLine(ref appendInterpolatedStringHandler);
						}
					}
				}
				foreach (NetFullCombatState.PotionState potionState in playerState.potions)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder19 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tPotion ");
					appendInterpolatedStringHandler.AppendFormatted<ModelId>(potionState.id);
					stringBuilder19.AppendLine(ref appendInterpolatedStringHandler);
				}
				foreach (NetFullCombatState.RelicState relicState in playerState.relics)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder20 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(29, 3, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tRelic ");
					appendInterpolatedStringHandler.AppendFormatted<ModelId>(relicState.relic.Id);
					appendInterpolatedStringHandler.AppendLiteral(" Props: ");
					appendInterpolatedStringHandler.AppendFormatted<SavedProperties>(relicState.relic.Props);
					appendInterpolatedStringHandler.AppendLiteral(" Floor added: ");
					appendInterpolatedStringHandler.AppendFormatted<int?>(relicState.relic.FloorAddedToDeck);
					stringBuilder20.AppendLine(ref appendInterpolatedStringHandler);
				}
				foreach (NetFullCombatState.OrbState orbState in playerState.orbs)
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder21 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(24, 3, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tOrb ");
					appendInterpolatedStringHandler.AppendFormatted<ModelId>(orbState.id);
					appendInterpolatedStringHandler.AppendLiteral(", passive: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(orbState.passive);
					appendInterpolatedStringHandler.AppendLiteral(" evoke: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(orbState.evoke);
					stringBuilder21.AppendLine(ref appendInterpolatedStringHandler);
				}
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder22 = stringBuilder2;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Player RNG seed: ");
				appendInterpolatedStringHandler.AppendFormatted<ulong>(playerState.rngSet.Seed);
				stringBuilder22.AppendLine(ref appendInterpolatedStringHandler);
				foreach (PlayerRngType playerRngType in Enum.GetValues<PlayerRngType>())
				{
					SerializableRng serializableRng;
					if (playerState.rngSet.Rngs.TryGetValue(playerRngType, out serializableRng))
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder23 = stringBuilder2;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 2, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("\tRNG counter ");
						appendInterpolatedStringHandler.AppendFormatted<PlayerRngType>(playerRngType);
						appendInterpolatedStringHandler.AppendLiteral(": ");
						appendInterpolatedStringHandler.AppendFormatted<SerializableRng>(serializableRng);
						stringBuilder23.AppendLine(ref appendInterpolatedStringHandler);
					}
				}
				stringBuilder.AppendLine("Player relic grab bag:");
				foreach (RelicRarity relicRarity in Enum.GetValues<RelicRarity>())
				{
					List<ModelId> list;
					if (playerState.relicGrabBag.RelicIdLists.TryGetValue(relicRarity, out list))
					{
						stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder24 = stringBuilder2;
						appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(10, 2, stringBuilder2);
						appendInterpolatedStringHandler.AppendLiteral("\tRarity ");
						appendInterpolatedStringHandler.AppendFormatted<RelicRarity>(relicRarity);
						appendInterpolatedStringHandler.AppendLiteral(": ");
						appendInterpolatedStringHandler.AppendFormatted(string.Join(",", list.Select<ModelId, string>((ModelId m) => m.Entry)));
						stringBuilder24.AppendLine(ref appendInterpolatedStringHandler);
					}
				}
			}
			stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder25 = stringBuilder2;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("RNG global seed: ");
			appendInterpolatedStringHandler.AppendFormatted(this.Rng.Seed);
			stringBuilder25.AppendLine(ref appendInterpolatedStringHandler);
			foreach (RunRngType runRngType in Enum.GetValues<RunRngType>())
			{
				SerializableRng serializableRng2;
				if (this.Rng.Rngs.TryGetValue(runRngType, out serializableRng2))
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder26 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(15, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral("\tRNG counter ");
					appendInterpolatedStringHandler.AppendFormatted<RunRngType>(runRngType);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<SerializableRng>(serializableRng2);
					stringBuilder26.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002CBA RID: 11450
		public List<uint> nextChoiceIds;

		// Token: 0x04002CBB RID: 11451
		public List<int> nextRewardIds;

		// Token: 0x04002CBC RID: 11452
		public uint? lastExecutedHookId;

		// Token: 0x04002CBD RID: 11453
		public uint? lastExecutedActionId;

		// Token: 0x020023A8 RID: 9128
		[Nullable(0)]
		public struct CreatureState : IPacketSerializable
		{
			// Token: 0x0600C17E RID: 49534 RVA: 0x0042294C File Offset: 0x00420B4C
			public void Serialize(PacketWriter writer)
			{
				writer.WriteBool(this.monsterId != null);
				if (this.monsterId != null)
				{
					writer.WriteModelEntry(this.monsterId);
				}
				writer.WriteBool(this.playerId != null);
				if (this.playerId != null)
				{
					writer.WriteULong(this.playerId.Value, 64);
				}
				writer.WriteInt(this.currentHp, 32);
				writer.WriteInt(this.maxHp, 32);
				writer.WriteInt(this.block, 32);
				writer.WriteList<NetFullCombatState.PowerState>(this.powers, 32);
			}

			// Token: 0x0600C17F RID: 49535 RVA: 0x004229F0 File Offset: 0x00420BF0
			public void Deserialize(PacketReader reader)
			{
				bool flag = reader.ReadBool();
				if (flag)
				{
					this.monsterId = reader.ReadModelIdAssumingType<MonsterModel>();
				}
				bool flag2 = reader.ReadBool();
				if (flag2)
				{
					this.playerId = new ulong?(reader.ReadULong(64));
				}
				this.currentHp = reader.ReadInt(32);
				this.maxHp = reader.ReadInt(32);
				this.block = reader.ReadInt(32);
				this.powers = reader.ReadList<NetFullCombatState.PowerState>(32);
			}

			// Token: 0x0600C180 RID: 49536 RVA: 0x00422A68 File Offset: 0x00420C68
			public NetFullCombatState.CreatureState Anonymized()
			{
				NetFullCombatState.CreatureState creatureState = this;
				creatureState.playerId = ((this.playerId != null) ? new ulong?(IdAnonymizer.Anonymize(this.playerId.Value)) : null);
				return creatureState;
			}

			// Token: 0x0400970C RID: 38668
			[Nullable(2)]
			public ModelId monsterId;

			// Token: 0x0400970D RID: 38669
			public ulong? playerId;

			// Token: 0x0400970E RID: 38670
			public int currentHp;

			// Token: 0x0400970F RID: 38671
			public int maxHp;

			// Token: 0x04009710 RID: 38672
			public int block;

			// Token: 0x04009711 RID: 38673
			public List<NetFullCombatState.PowerState> powers;
		}

		// Token: 0x020023A9 RID: 9129
		[Nullable(0)]
		public struct PowerState : IPacketSerializable
		{
			// Token: 0x0600C181 RID: 49537 RVA: 0x00422AB1 File Offset: 0x00420CB1
			public void Serialize(PacketWriter writer)
			{
				writer.WriteModelEntry(this.id);
				writer.WriteInt(this.amount, 32);
			}

			// Token: 0x0600C182 RID: 49538 RVA: 0x00422ACD File Offset: 0x00420CCD
			public void Deserialize(PacketReader reader)
			{
				this.id = reader.ReadModelIdAssumingType<PowerModel>();
				this.amount = reader.ReadInt(32);
			}

			// Token: 0x04009712 RID: 38674
			public ModelId id;

			// Token: 0x04009713 RID: 38675
			public int amount;
		}

		// Token: 0x020023AA RID: 9130
		[Nullable(0)]
		public struct OrbState : IPacketSerializable
		{
			// Token: 0x0600C183 RID: 49539 RVA: 0x00422AE9 File Offset: 0x00420CE9
			public void Serialize(PacketWriter writer)
			{
				writer.WriteModelEntry(this.id);
				writer.WriteInt(this.passive, 16);
				writer.WriteInt(this.evoke, 16);
			}

			// Token: 0x0600C184 RID: 49540 RVA: 0x00422B13 File Offset: 0x00420D13
			public void Deserialize(PacketReader reader)
			{
				this.id = reader.ReadModelIdAssumingType<OrbModel>();
				this.passive = reader.ReadInt(16);
				this.evoke = reader.ReadInt(16);
			}

			// Token: 0x0600C185 RID: 49541 RVA: 0x00422B40 File Offset: 0x00420D40
			public static NetFullCombatState.OrbState From(OrbModel orb)
			{
				return new NetFullCombatState.OrbState
				{
					id = orb.Id,
					passive = (int)orb.PassiveVal,
					evoke = (int)orb.EvokeVal
				};
			}

			// Token: 0x04009714 RID: 38676
			public ModelId id;

			// Token: 0x04009715 RID: 38677
			public int passive;

			// Token: 0x04009716 RID: 38678
			public int evoke;
		}

		/// <summary>
		/// All the combat-relevant state from the Player class.
		/// Includes most state except:
		///  - Rewards and Shop RNG counters (stripped out in FromRun)
		///  - Rarity odds
		/// </summary>
		// Token: 0x020023AB RID: 9131
		[Nullable(0)]
		public struct PlayerState : IPacketSerializable
		{
			// Token: 0x0600C186 RID: 49542 RVA: 0x00422B88 File Offset: 0x00420D88
			public void Serialize(PacketWriter writer)
			{
				writer.WriteULong(this.playerId, 64);
				writer.WriteModelEntry(this.characterId);
				writer.WriteInt(this.turnNumber, 32);
				writer.WriteEnum<PlayerTurnPhase>(this.phase);
				writer.WriteInt(this.energy, 32);
				writer.WriteInt(this.stars, 32);
				writer.WriteInt(this.maxPotionCount, 32);
				writer.WriteInt(this.gold, 32);
				writer.WriteList<NetFullCombatState.CombatPileState>(this.piles, 32);
				writer.WriteList<NetFullCombatState.PotionState>(this.potions, 32);
				writer.WriteList<NetFullCombatState.RelicState>(this.relics, 32);
				writer.WriteList<NetFullCombatState.OrbState>(this.orbs, 32);
				writer.Write<SerializablePlayerRngSet>(this.rngSet);
				writer.Write<SerializableRelicGrabBag>(this.relicGrabBag);
			}

			// Token: 0x0600C187 RID: 49543 RVA: 0x00422C54 File Offset: 0x00420E54
			public void Deserialize(PacketReader reader)
			{
				this.playerId = reader.ReadULong(64);
				this.characterId = reader.ReadModelIdAssumingType<CharacterModel>();
				this.turnNumber = reader.ReadInt(32);
				this.phase = reader.ReadEnum<PlayerTurnPhase>();
				this.energy = reader.ReadInt(32);
				this.stars = reader.ReadInt(32);
				this.maxPotionCount = reader.ReadInt(32);
				this.gold = reader.ReadInt(32);
				this.piles = reader.ReadList<NetFullCombatState.CombatPileState>(32);
				this.potions = reader.ReadList<NetFullCombatState.PotionState>(32);
				this.relics = reader.ReadList<NetFullCombatState.RelicState>(32);
				this.orbs = reader.ReadList<NetFullCombatState.OrbState>(32);
				this.rngSet = reader.Read<SerializablePlayerRngSet>();
				this.relicGrabBag = reader.Read<SerializableRelicGrabBag>();
			}

			// Token: 0x0600C188 RID: 49544 RVA: 0x00422D20 File Offset: 0x00420F20
			public NetFullCombatState.PlayerState Anonymized()
			{
				NetFullCombatState.PlayerState playerState = this;
				playerState.playerId = IdAnonymizer.Anonymize(this.playerId);
				return playerState;
			}

			// Token: 0x04009717 RID: 38679
			public ulong playerId;

			// Token: 0x04009718 RID: 38680
			public ModelId characterId;

			// Token: 0x04009719 RID: 38681
			public int turnNumber;

			// Token: 0x0400971A RID: 38682
			public PlayerTurnPhase phase;

			// Token: 0x0400971B RID: 38683
			public int energy;

			// Token: 0x0400971C RID: 38684
			public int stars;

			// Token: 0x0400971D RID: 38685
			public int maxPotionCount;

			// Token: 0x0400971E RID: 38686
			public int gold;

			// Token: 0x0400971F RID: 38687
			public List<NetFullCombatState.CombatPileState> piles;

			// Token: 0x04009720 RID: 38688
			public List<NetFullCombatState.PotionState> potions;

			// Token: 0x04009721 RID: 38689
			public List<NetFullCombatState.RelicState> relics;

			// Token: 0x04009722 RID: 38690
			public List<NetFullCombatState.OrbState> orbs;

			// Token: 0x04009723 RID: 38691
			public SerializablePlayerRngSet rngSet;

			// Token: 0x04009724 RID: 38692
			public SerializableRelicGrabBag relicGrabBag;
		}

		// Token: 0x020023AC RID: 9132
		[Nullable(0)]
		public struct CombatPileState : IPacketSerializable
		{
			// Token: 0x0600C189 RID: 49545 RVA: 0x00422D47 File Offset: 0x00420F47
			public void Serialize(PacketWriter writer)
			{
				writer.WriteInt((int)this.pileType, 32);
				writer.WriteList<NetFullCombatState.CardState>(this.cards, 32);
			}

			// Token: 0x0600C18A RID: 49546 RVA: 0x00422D65 File Offset: 0x00420F65
			public void Deserialize(PacketReader reader)
			{
				this.pileType = (PileType)reader.ReadInt(32);
				this.cards = reader.ReadList<NetFullCombatState.CardState>(32);
			}

			// Token: 0x0600C18B RID: 49547 RVA: 0x00422D84 File Offset: 0x00420F84
			public static NetFullCombatState.CombatPileState From(CardPile pile)
			{
				NetFullCombatState.CombatPileState combatPileState = default(NetFullCombatState.CombatPileState);
				combatPileState.pileType = pile.Type;
				combatPileState.cards = new List<NetFullCombatState.CardState>();
				foreach (CardModel cardModel in pile.Cards)
				{
					combatPileState.cards.Add(NetFullCombatState.CardState.From(cardModel));
				}
				return combatPileState;
			}

			// Token: 0x04009725 RID: 38693
			public PileType pileType;

			// Token: 0x04009726 RID: 38694
			public List<NetFullCombatState.CardState> cards;
		}

		// Token: 0x020023AD RID: 9133
		[Nullable(0)]
		public struct CardState : IPacketSerializable
		{
			// Token: 0x0600C18C RID: 49548 RVA: 0x00422E00 File Offset: 0x00421000
			public void Serialize(PacketWriter writer)
			{
				writer.Write<SerializableCard>(this.card);
				writer.WriteBool(this.affliction != null);
				if (this.affliction != null)
				{
					writer.WriteModelEntry(this.affliction);
					writer.WriteInt(this.afflictionCount, 32);
				}
				writer.WriteBool(this.energyCost != null);
				if (this.energyCost != null)
				{
					writer.WriteInt(this.energyCost.Value, 32);
				}
				writer.WriteBool(this.keywords != null);
				if (this.keywords != null)
				{
					writer.WriteInt(this.keywords.Count, 3);
					foreach (CardKeyword cardKeyword in this.keywords)
					{
						writer.WriteEnum<CardKeyword>(cardKeyword);
					}
				}
			}

			// Token: 0x0600C18D RID: 49549 RVA: 0x00422EF4 File Offset: 0x004210F4
			public void Deserialize(PacketReader reader)
			{
				this.card = reader.Read<SerializableCard>();
				bool flag = reader.ReadBool();
				if (flag)
				{
					this.affliction = reader.ReadModelIdAssumingType<AfflictionModel>();
					this.afflictionCount = reader.ReadInt(32);
				}
				if (reader.ReadBool())
				{
					this.energyCost = new int?(reader.ReadInt(32));
				}
				if (reader.ReadBool())
				{
					this.keywords = new List<CardKeyword>();
					int num = reader.ReadInt(3);
					for (int i = 0; i < num; i++)
					{
						this.keywords.Add(reader.ReadEnum<CardKeyword>());
					}
				}
			}

			// Token: 0x0600C18E RID: 49550 RVA: 0x00422F84 File Offset: 0x00421184
			public static NetFullCombatState.CardState From(CardModel card)
			{
				NetFullCombatState.CardState cardState = default(NetFullCombatState.CardState);
				cardState.card = card.ToSerializable();
				AfflictionModel afflictionModel = card.Affliction;
				cardState.affliction = ((afflictionModel != null) ? afflictionModel.Id : null);
				AfflictionModel afflictionModel2 = card.Affliction;
				cardState.afflictionCount = ((afflictionModel2 != null) ? afflictionModel2.Amount : 0);
				IReadOnlySet<CardKeyword> readOnlySet = card.Keywords;
				if (readOnlySet.Count > 0)
				{
					cardState.keywords = new List<CardKeyword>();
					foreach (CardKeyword cardKeyword in Enum.GetValues<CardKeyword>())
					{
						if (readOnlySet.Contains(cardKeyword))
						{
							cardState.keywords.Add(cardKeyword);
						}
					}
				}
				else
				{
					cardState.keywords = null;
				}
				int withModifiers = card.EnergyCost.GetWithModifiers(CostModifiers.All);
				if (withModifiers != card.EnergyCost.Canonical)
				{
					cardState.energyCost = new int?(withModifiers);
				}
				return cardState;
			}

			// Token: 0x04009727 RID: 38695
			public SerializableCard card;

			// Token: 0x04009728 RID: 38696
			[Nullable(2)]
			public ModelId affliction;

			// Token: 0x04009729 RID: 38697
			public int afflictionCount;

			// Token: 0x0400972A RID: 38698
			public int? energyCost;

			// Token: 0x0400972B RID: 38699
			[Nullable(2)]
			public List<CardKeyword> keywords;
		}

		// Token: 0x020023AE RID: 9134
		[Nullable(0)]
		public struct PotionState : IPacketSerializable
		{
			// Token: 0x0600C18F RID: 49551 RVA: 0x0042305C File Offset: 0x0042125C
			public void Serialize(PacketWriter writer)
			{
				writer.WriteModelEntry(this.id);
			}

			// Token: 0x0600C190 RID: 49552 RVA: 0x0042306A File Offset: 0x0042126A
			public void Deserialize(PacketReader reader)
			{
				this.id = reader.ReadModelIdAssumingType<PotionModel>();
			}

			// Token: 0x0400972C RID: 38700
			public ModelId id;
		}

		// Token: 0x020023AF RID: 9135
		[Nullable(0)]
		public struct RelicState : IPacketSerializable
		{
			// Token: 0x0600C191 RID: 49553 RVA: 0x00423078 File Offset: 0x00421278
			public void Serialize(PacketWriter writer)
			{
				writer.Write<SerializableRelic>(this.relic);
			}

			// Token: 0x0600C192 RID: 49554 RVA: 0x00423086 File Offset: 0x00421286
			public void Deserialize(PacketReader reader)
			{
				this.relic = reader.Read<SerializableRelic>();
			}

			// Token: 0x0400972D RID: 38701
			public SerializableRelic relic;
		}

		// Token: 0x020023B0 RID: 9136
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400972E RID: 38702
			[Nullable(0)]
			public static Func<OrbModel, NetFullCombatState.OrbState> <0>__From;
		}
	}
}
