using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands
{
	// Token: 0x02000D03 RID: 3331
	[NullableContext(1)]
	[Nullable(0)]
	public class UnlockConsoleCmd : AbstractConsoleCmd
	{
		// Token: 0x17002517 RID: 9495
		// (get) Token: 0x06008688 RID: 34440 RVA: 0x002D20AA File Offset: 0x002D02AA
		public override string CmdName
		{
			get
			{
				return "unlock";
			}
		}

		// Token: 0x17002518 RID: 9496
		// (get) Token: 0x06008689 RID: 34441 RVA: 0x002D20B1 File Offset: 0x002D02B1
		public override string Args
		{
			get
			{
				return "<type:string>";
			}
		}

		// Token: 0x17002519 RID: 9497
		// (get) Token: 0x0600868A RID: 34442 RVA: 0x002D20B8 File Offset: 0x002D02B8
		public override string Description
		{
			get
			{
				return "Marks all cards/potions/relics/monsters/events/acts/epochs/ascensions as discovered, or 'all' to unlock everything.";
			}
		}

		// Token: 0x1700251A RID: 9498
		// (get) Token: 0x0600868B RID: 34443 RVA: 0x002D20BF File Offset: 0x002D02BF
		public override bool IsNetworked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600868C RID: 34444 RVA: 0x002D20C4 File Offset: 0x002D02C4
		public override CmdResult Process([Nullable(2)] Player issuingPlayer, string[] args)
		{
			if (args.Length < 1)
			{
				return new CmdResult(false, "No argument specified.\n" + this.Args);
			}
			List<string> list = ((args.Length > 1) ? RuntimeHelpers.GetSubArray<string>(args, Range.StartAt(1)).ToList<string>() : null);
			return this.ProcessUnlock(args[0], list);
		}

		// Token: 0x0600868D RID: 34445 RVA: 0x002D2118 File Offset: 0x002D0318
		private CmdResult ProcessUnlock(string discoveryType, [Nullable(new byte[] { 2, 1 })] List<string> args)
		{
			Action<UnlockConsoleCmd, List<string>> action;
			if (!UnlockConsoleCmd._unlockActions.TryGetValue(discoveryType, out action))
			{
				return new CmdResult(false, "Argument " + discoveryType + " not recognized as a discovery type.\n" + this.Args);
			}
			if (args != null)
			{
				for (int i = 0; i < args.Count; i++)
				{
					args[i] = args[i].ToUpperInvariant();
				}
				Func<IEnumerable<string>> func;
				if (UnlockConsoleCmd._validIdSources.TryGetValue(discoveryType, out func))
				{
					HashSet<string> validIds = func().ToHashSet<string>();
					List<string> list = args.Where<string>((string id) => !validIds.Contains(id)).ToList<string>();
					if (list.Count > 0)
					{
						return new CmdResult(false, "Unknown " + discoveryType + ": " + string.Join(", ", list));
					}
				}
			}
			action(this, args);
			SaveManager.Instance.SaveProgressFile();
			return new CmdResult(true, "Unlocked " + discoveryType + " " + ((args != null) ? string.Join(", ", args) : ""));
		}

		// Token: 0x0600868E RID: 34446 RVA: 0x002D2224 File Offset: 0x002D0424
		private void UnlockCards([Nullable(new byte[] { 2, 1 })] List<string> cards)
		{
			IEnumerable<ModelId> enumerable;
			if (cards == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = cards.Select<string, ModelId>((string c) => new ModelId("CARD", c));
			}
			IEnumerable<ModelId> enumerable2;
			if ((enumerable2 = enumerable) == null)
			{
				enumerable2 = ModelDb.AllCards.Select<CardModel, ModelId>((CardModel c) => c.Id);
			}
			IEnumerable<ModelId> enumerable3 = enumerable2;
			foreach (ModelId modelId in enumerable3)
			{
				SaveManager.Instance.Progress.MarkCardAsSeen(modelId);
			}
		}

		// Token: 0x0600868F RID: 34447 RVA: 0x002D22D0 File Offset: 0x002D04D0
		private void UnlockRelics([Nullable(new byte[] { 2, 1 })] List<string> relics)
		{
			IEnumerable<ModelId> enumerable;
			if (relics == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = relics.Select<string, ModelId>((string c) => new ModelId("RELIC", c));
			}
			IEnumerable<ModelId> enumerable2;
			if ((enumerable2 = enumerable) == null)
			{
				enumerable2 = ModelDb.AllRelics.Select<RelicModel, ModelId>((RelicModel c) => c.Id);
			}
			IEnumerable<ModelId> enumerable3 = enumerable2;
			foreach (ModelId modelId in enumerable3)
			{
				SaveManager.Instance.Progress.MarkRelicAsSeen(modelId);
			}
		}

		// Token: 0x06008690 RID: 34448 RVA: 0x002D237C File Offset: 0x002D057C
		private void UnlockPotions([Nullable(new byte[] { 2, 1 })] List<string> potions)
		{
			IEnumerable<ModelId> enumerable;
			if (potions == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = potions.Select<string, ModelId>((string c) => new ModelId("POTION", c));
			}
			IEnumerable<ModelId> enumerable2;
			if ((enumerable2 = enumerable) == null)
			{
				enumerable2 = ModelDb.AllPotions.Select<PotionModel, ModelId>((PotionModel c) => c.Id);
			}
			IEnumerable<ModelId> enumerable3 = enumerable2;
			foreach (ModelId modelId in enumerable3)
			{
				SaveManager.Instance.Progress.MarkPotionAsSeen(modelId);
			}
		}

		// Token: 0x06008691 RID: 34449 RVA: 0x002D2428 File Offset: 0x002D0628
		private void UnlockMonsters([Nullable(new byte[] { 2, 1 })] List<string> monsters)
		{
			IEnumerable<ModelId> enumerable;
			if (monsters == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = monsters.Select<string, ModelId>((string c) => new ModelId("MONSTER", c));
			}
			IEnumerable<ModelId> enumerable2;
			if ((enumerable2 = enumerable) == null)
			{
				enumerable2 = ModelDb.Monsters.Select<MonsterModel, ModelId>((MonsterModel c) => c.Id);
			}
			IEnumerable<ModelId> enumerable3 = enumerable2;
			foreach (ModelId modelId in enumerable3)
			{
				EnemyStats orCreateEnemyStats = SaveManager.Instance.Progress.GetOrCreateEnemyStats(modelId);
				if (orCreateEnemyStats.FightStats.Count == 0)
				{
					orCreateEnemyStats.FightStats.Add(new FightStats
					{
						Character = ModelDb.GetId<Ironclad>(),
						Wins = 1
					});
				}
			}
		}

		// Token: 0x06008692 RID: 34450 RVA: 0x002D2504 File Offset: 0x002D0704
		private void UnlockActs()
		{
			foreach (ActModel actModel in ModelDb.Acts)
			{
				SaveManager.Instance.Progress.MarkActAsSeen(actModel.Id);
			}
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x002D2560 File Offset: 0x002D0760
		private void UnlockEvents([Nullable(new byte[] { 2, 1 })] List<string> events)
		{
			IEnumerable<ModelId> enumerable;
			if (events == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = events.Select<string, ModelId>((string c) => new ModelId("EVENT", c));
			}
			IEnumerable<ModelId> enumerable2;
			if ((enumerable2 = enumerable) == null)
			{
				enumerable2 = ModelDb.AllEvents.Select<EventModel, ModelId>((EventModel c) => c.Id);
			}
			IEnumerable<ModelId> enumerable3 = enumerable2;
			foreach (ModelId modelId in enumerable3)
			{
				SaveManager.Instance.Progress.MarkEventAsSeen(modelId);
			}
		}

		// Token: 0x06008694 RID: 34452 RVA: 0x002D260C File Offset: 0x002D080C
		private void UnlockEpochs([Nullable(new byte[] { 2, 1 })] List<string> epochs)
		{
			IEnumerable<string> enumerable = (epochs ?? EpochModel.AllEpochIds).Except<string>(from e in SaveManager.Instance.Progress.Epochs
				where e.State == EpochState.Revealed
				select e.Id);
			foreach (string text in enumerable)
			{
				SaveManager.Instance.ObtainEpochOverride(text, EpochState.Revealed);
			}
		}

		// Token: 0x06008695 RID: 34453 RVA: 0x002D26C4 File Offset: 0x002D08C4
		private void UnlockAscensions([Nullable(new byte[] { 2, 1 })] List<string> ascensions)
		{
			if (ascensions != null)
			{
				throw new NotImplementedException();
			}
			SaveManager.Instance.Progress.MaxMultiplayerAscension = 10;
			foreach (CharacterModel characterModel in ModelDb.AllCharacters)
			{
				CharacterStats orCreateCharacterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(characterModel.Id);
				orCreateCharacterStats.MaxAscension = 10;
			}
		}

		// Token: 0x06008696 RID: 34454 RVA: 0x002D2744 File Offset: 0x002D0944
		public override CompletionResult GetArgumentCompletions([Nullable(2)] Player player, string[] args)
		{
			if (args.Length <= 1)
			{
				string partial = ((args.Length != 0) ? args[0] : "");
				List<string> list;
				if (string.IsNullOrWhiteSpace(partial))
				{
					list = UnlockConsoleCmd._validDiscoveryTypes;
				}
				else
				{
					list = UnlockConsoleCmd._validDiscoveryTypes.Where<string>((string type) => type.Contains(partial, StringComparison.OrdinalIgnoreCase)).ToList<string>();
				}
				return new CompletionResult
				{
					Candidates = list,
					Type = CompletionType.Argument,
					ArgumentContext = this.CmdName
				};
			}
			return new CompletionResult
			{
				Type = CompletionType.Argument,
				ArgumentContext = this.CmdName
			};
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x002D27E4 File Offset: 0x002D09E4
		// Note: this type is marked as 'beforefieldinit'.
		static UnlockConsoleCmd()
		{
			Dictionary<string, Action<UnlockConsoleCmd, List<string>>> dictionary = new Dictionary<string, Action<UnlockConsoleCmd, List<string>>>();
			dictionary["cards"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockCards(s);
			};
			dictionary["potions"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockPotions(s);
			};
			dictionary["relics"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockRelics(s);
			};
			dictionary["monsters"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockMonsters(s);
			};
			dictionary["events"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockEvents(s);
			};
			dictionary["acts"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockActs();
			};
			dictionary["epochs"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockEpochs(s);
			};
			dictionary["ascensions"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockAscensions(s);
			};
			dictionary["all"] = delegate(UnlockConsoleCmd c, [Nullable(new byte[] { 2, 1 })] List<string> s)
			{
				c.UnlockCards(null);
				c.UnlockPotions(null);
				c.UnlockRelics(null);
				c.UnlockMonsters(null);
				c.UnlockEvents(null);
				c.UnlockActs();
				c.UnlockEpochs(null);
				c.UnlockAscensions(null);
			};
			UnlockConsoleCmd._unlockActions = dictionary;
			UnlockConsoleCmd._validDiscoveryTypes = UnlockConsoleCmd._unlockActions.Keys.ToList<string>();
			Dictionary<string, Func<IEnumerable<string>>> dictionary2 = new Dictionary<string, Func<IEnumerable<string>>>();
			dictionary2["cards"] = () => ModelDb.AllCards.Select<CardModel, string>((CardModel c) => c.Id.Entry);
			dictionary2["potions"] = () => ModelDb.AllPotions.Select<PotionModel, string>((PotionModel c) => c.Id.Entry);
			dictionary2["relics"] = () => ModelDb.AllRelics.Select<RelicModel, string>((RelicModel c) => c.Id.Entry);
			dictionary2["monsters"] = () => ModelDb.Monsters.Select<MonsterModel, string>((MonsterModel c) => c.Id.Entry);
			dictionary2["events"] = () => ModelDb.AllEvents.Select<EventModel, string>((EventModel c) => c.Id.Entry);
			dictionary2["epochs"] = () => EpochModel.AllEpochIds;
			UnlockConsoleCmd._validIdSources = dictionary2;
		}

		// Token: 0x04002E6D RID: 11885
		[Nullable(new byte[] { 1, 1, 1, 1, 2, 1 })]
		private static readonly Dictionary<string, Action<UnlockConsoleCmd, List<string>>> _unlockActions;

		// Token: 0x04002E6E RID: 11886
		private static readonly List<string> _validDiscoveryTypes;

		// Token: 0x04002E6F RID: 11887
		private static readonly Dictionary<string, Func<IEnumerable<string>>> _validIdSources;
	}
}
