using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms
{
	/// <summary>
	/// Handles combat rooms (Monster, Elite, Boss).
	/// Applies massive defensive buffs and plays all cards each turn.
	/// </summary>
	// Token: 0x02000D82 RID: 3458
	[NullableContext(1)]
	[Nullable(0)]
	public class CombatRoomHandler : IRoomHandler, IHandler
	{
		// Token: 0x17002648 RID: 9800
		// (get) Token: 0x06008B44 RID: 35652 RVA: 0x002E7BF3 File Offset: 0x002E5DF3
		public RoomType[] HandledTypes
		{
			get
			{
				return new RoomType[]
				{
					RoomType.Monster,
					RoomType.Elite,
					RoomType.Boss
				};
			}
		}

		// Token: 0x17002649 RID: 9801
		// (get) Token: 0x06008B45 RID: 35653 RVA: 0x002E7C06 File Offset: 0x002E5E06
		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromMinutes(5L);
			}
		}

		// Token: 0x06008B46 RID: 35654 RVA: 0x002E7C10 File Offset: 0x002E5E10
		public async Task HandleAsync(Rng random, CancellationToken ct)
		{
			CombatRoomHandler.<>c__DisplayClass4_0 CS$<>8__locals1 = new CombatRoomHandler.<>c__DisplayClass4_0();
			AutoSlayLog.Action("Waiting for combat to start");
			await WaitHelper.Until(() => CombatManager.Instance.IsInProgress, ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Combat not started");
			AutoSlayLog.Action("Combat started, applying defensive buffs");
			CS$<>8__locals1.player = LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState());
			Creature playerCreature = CS$<>8__locals1.player.Creature;
			await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), playerCreature, 999m, playerCreature, null, false);
			await PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), playerCreature, 999m, playerCreature, null, false);
			int turnCount = 0;
			while (CombatManager.Instance.IsInProgress && turnCount < 100)
			{
				CombatRoomHandler.<>c__DisplayClass4_1 CS$<>8__locals2 = new CombatRoomHandler.<>c__DisplayClass4_1();
				ct.ThrowIfCancellationRequested();
				turnCount++;
				Func<bool> func;
				if ((func = CS$<>8__locals1.<>9__2) == null)
				{
					CombatRoomHandler.<>c__DisplayClass4_0 CS$<>8__locals3 = CS$<>8__locals1;
					Func<bool> func2 = delegate
					{
						PlayerCombatState playerCombatState3 = CS$<>8__locals1.player.PlayerCombatState;
						return (playerCombatState3 != null && playerCombatState3.Phase == PlayerTurnPhase.Play) || !CombatManager.Instance.IsInProgress;
					};
					CS$<>8__locals3.<>9__2 = func2;
					func = func2;
				}
				await WaitHelper.Until(func, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Play phase not started");
				if (!CombatManager.Instance.IsInProgress)
				{
					break;
				}
				Watchdog currentWatchdog = AutoSlayer.CurrentWatchdog;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (currentWatchdog != null)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Combat turn ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(turnCount);
					currentWatchdog.Reset(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (turnCount >= 3)
				{
					await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), playerCreature, 200m, playerCreature, null, false);
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Turn ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(turnCount);
				defaultInterpolatedStringHandler.AppendLiteral(": playing cards");
				AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
				await CombatRoomHandler.UseAllPotions(CS$<>8__locals1.player, random, ct);
				int cardsPlayed = 0;
				CS$<>8__locals2.attemptedCards = new HashSet<CardModel>();
				while (cardsPlayed < 50)
				{
					PlayerCombatState playerCombatState = CS$<>8__locals1.player.PlayerCombatState;
					if (playerCombatState == null || playerCombatState.Phase != PlayerTurnPhase.Play)
					{
						break;
					}
					ct.ThrowIfCancellationRequested();
					if (cardsPlayed > 0 && cardsPlayed % 10 == 0)
					{
						Watchdog currentWatchdog2 = AutoSlayer.CurrentWatchdog;
						if (currentWatchdog2 != null)
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Combat turn ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(turnCount);
							defaultInterpolatedStringHandler.AppendLiteral(", played ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(cardsPlayed);
							defaultInterpolatedStringHandler.AppendLiteral(" cards");
							currentWatchdog2.Reset(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					IEnumerable<CardModel> cards = PileType.Hand.GetPile(CS$<>8__locals1.player).Cards;
					Func<CardModel, bool> func3;
					if ((func3 = CS$<>8__locals2.<>9__3) == null)
					{
						CombatRoomHandler.<>c__DisplayClass4_1 CS$<>8__locals4 = CS$<>8__locals2;
						Func<CardModel, bool> func4 = delegate(CardModel c)
						{
							UnplayableReason unplayableReason;
							AbstractModel abstractModel;
							return c.CanPlay(out unplayableReason, out abstractModel) && !CS$<>8__locals2.attemptedCards.Contains(c);
						};
						CS$<>8__locals4.<>9__3 = func4;
						func3 = func4;
					}
					List<CardModel> list = cards.Where<CardModel>(func3).ToList<CardModel>();
					if (list.Count == 0)
					{
						AutoSlayLog.Action("No more playable cards, ending turn");
						break;
					}
					CardModel cardModel = random.NextItem<CardModel>(list);
					Creature randomTarget = CombatRoomHandler.GetRandomTarget(cardModel, random);
					CS$<>8__locals2.attemptedCards.Add(cardModel);
					AutoSlayLog.Info("Playing " + cardModel.Id.Entry);
					await CardCmd.AutoPlay(new BlockingPlayerChoiceContext(), cardModel, randomTarget, AutoPlayType.Default, false, false);
					cardsPlayed++;
					await Task.Delay(100, ct);
				}
				PlayerCombatState playerCombatState2 = CS$<>8__locals1.player.PlayerCombatState;
				if (playerCombatState2 != null && playerCombatState2.Phase == PlayerTurnPhase.Play && CombatManager.Instance.IsInProgress)
				{
					PlayerCmd.EndTurn(CS$<>8__locals1.player, false, null);
				}
				CS$<>8__locals2 = null;
			}
			await WaitHelper.Until(() => !CombatManager.Instance.IsInProgress, ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Combat did not end");
			AutoSlayLog.Action("Combat finished");
		}

		// Token: 0x06008B47 RID: 35655 RVA: 0x002E7C5C File Offset: 0x002E5E5C
		[return: Nullable(2)]
		private static Creature GetRandomTarget(CardModel card, Rng random)
		{
			if (card.TargetType != TargetType.AnyEnemy)
			{
				return null;
			}
			ICombatState combatState = card.CombatState;
			if (combatState == null)
			{
				return null;
			}
			List<Creature> list = combatState.HittableEnemies.ToList<Creature>();
			if (list.Count == 0)
			{
				return null;
			}
			return random.NextItem<Creature>(list);
		}

		// Token: 0x06008B48 RID: 35656 RVA: 0x002E7CA0 File Offset: 0x002E5EA0
		private static async Task UseAllPotions(Player player, Rng random, CancellationToken ct)
		{
			List<PotionModel> list = player.Potions.ToList<PotionModel>();
			if (list.Count != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Using ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(list.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" potion(s)");
				AutoSlayLog.Action(defaultInterpolatedStringHandler.ToStringAndClear());
				ICombatState combatState = player.Creature.CombatState;
				foreach (PotionModel potionModel in list)
				{
					ct.ThrowIfCancellationRequested();
					PlayerCombatState playerCombatState = player.PlayerCombatState;
					if (playerCombatState == null || playerCombatState.Phase != PlayerTurnPhase.Play || !CombatManager.Instance.IsInProgress)
					{
						break;
					}
					Creature potionTarget = CombatRoomHandler.GetPotionTarget(potionModel, combatState, random);
					if (potionTarget == null && potionModel.TargetType.IsSingleTarget())
					{
						AutoSlayLog.Warn("Skipping potion " + potionModel.Id.Entry + ": no valid target");
					}
					else
					{
						AutoSlayLog.Info("Using potion: " + potionModel.Id.Entry);
						potionModel.EnqueueManualUse(potionTarget);
						await Task.Delay(300, ct);
					}
				}
				List<PotionModel>.Enumerator enumerator = default(List<PotionModel>.Enumerator);
			}
		}

		// Token: 0x06008B49 RID: 35657 RVA: 0x002E7CF4 File Offset: 0x002E5EF4
		[return: Nullable(2)]
		private static Creature GetPotionTarget(PotionModel potion, [Nullable(2)] ICombatState combatState, Rng random)
		{
			if (combatState == null)
			{
				return null;
			}
			switch (potion.TargetType)
			{
			case TargetType.Self:
				return combatState.PlayerCreatures.FirstOrDefault<Creature>((Creature c) => c.IsAlive);
			case TargetType.AnyEnemy:
				return random.NextItem<Creature>(combatState.HittableEnemies.ToList<Creature>());
			case TargetType.AnyPlayer:
				return combatState.PlayerCreatures.FirstOrDefault<Creature>((Creature c) => c.IsAlive);
			case TargetType.AnyAlly:
				return combatState.PlayerCreatures.FirstOrDefault<Creature>((Creature c) => c.IsAlive);
			}
			return null;
		}
	}
}
