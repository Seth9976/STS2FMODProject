using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000857 RID: 2135
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TabletOfTruth : EventModel
	{
		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x060068FA RID: 26874 RVA: 0x00279F14 File Offset: 0x00278114
		// (set) Token: 0x060068FB RID: 26875 RVA: 0x00279F1C File Offset: 0x0027811C
		private int DecipherCount
		{
			get
			{
				return this._decipherCount;
			}
			set
			{
				base.AssertMutable();
				this._decipherCount = value;
			}
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x00279F2C File Offset: 0x0027812C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Decipher), "TABLET_OF_TRUTH.pages.INITIAL.options.DECIPHER_1", Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Player p) => p.Creature.MaxHp <= base.DynamicVars["DecipherMaxHpLoss"].BaseValue),
				new EventOption(this, new Func<Task>(this.Smash), "TABLET_OF_TRUTH.pages.INITIAL.options.SMASH", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x060068FD RID: 26877 RVA: 0x00279F93 File Offset: 0x00278193
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DynamicVar("SmashHPGain", 20m),
					new DynamicVar("DecipherMaxHpLoss", 3m)
				});
			}
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x00279FC8 File Offset: 0x002781C8
		private async Task Smash()
		{
			Creature creature = base.Owner.Creature;
			await CreatureCmd.Heal(creature, base.DynamicVars["SmashHPGain"].BaseValue, true);
			base.SetEventFinished(base.L10NLookup("TABLET_OF_TRUTH.pages.SMASH.description"));
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x0027A00C File Offset: 0x0027820C
		private async Task Decipher()
		{
			await this.LoseMaxHpAndUpgrade(base.DynamicVars["DecipherMaxHpLoss"].BaseValue);
			this.DecipherCount++;
			if (this.DecipherCount == 5)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("TABLET_OF_TRUTH.pages.DECIPHER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DecipherCount);
				defaultInterpolatedStringHandler.AppendLiteral(".description");
				base.SetEventFinished(base.L10NLookup(defaultInterpolatedStringHandler.ToStringAndClear()));
			}
			else
			{
				base.DynamicVars["DecipherMaxHpLoss"].BaseValue = this.GetDecipherCost();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("TABLET_OF_TRUTH.pages.DECIPHER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DecipherCount);
				defaultInterpolatedStringHandler.AppendLiteral(".description");
				LocString locString = base.L10NLookup(defaultInterpolatedStringHandler.ToStringAndClear());
				EventOption[] array = new EventOption[2];
				int num = 0;
				Func<Task> func = new Func<Task>(this.Decipher);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("TABLET_OF_TRUTH.pages.DECIPHER_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DecipherCount);
				defaultInterpolatedStringHandler.AppendLiteral(".options.DECIPHER");
				array[num] = new EventOption(this, func, defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<IHoverTip>()).ThatWillKillPlayerIf((Player p) => p.Creature.MaxHp <= base.DynamicVars["DecipherMaxHpLoss"].BaseValue);
				array[1] = new EventOption(this, new Func<Task>(this.GiveUp), "TABLET_OF_TRUTH.pages.DECIPHER.options.GIVE_UP", Array.Empty<IHoverTip>());
				this.SetEventState(locString, new <>z__ReadOnlyArray<EventOption>(array));
			}
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x0027A050 File Offset: 0x00278250
		public int GetDecipherCost()
		{
			Player owner = base.Owner;
			switch (this.DecipherCount)
			{
			case 1:
				return 6;
			case 2:
				return 12;
			case 3:
				return 24;
			case 4:
				return owner.Creature.MaxHp - 1;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("DecipherCount: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DecipherCount);
				defaultInterpolatedStringHandler.AppendLiteral(" should not be called.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return 999999999;
			}
			}
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x0027A0DC File Offset: 0x002782DC
		private Task GiveUp()
		{
			base.SetEventFinished(base.L10NLookup("TABLET_OF_TRUTH.pages.GIVE_UP.description"));
			return Task.CompletedTask;
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x0027A0F4 File Offset: 0x002782F4
		private async Task LoseMaxHpAndUpgrade(decimal hp)
		{
			if (hp < base.Owner.Creature.MaxHp)
			{
				await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, hp, false);
				List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where<CardModel>((CardModel c) => c.IsUpgradable).ToList<CardModel>();
				if (this._decipherCount == 4)
				{
					foreach (CardModel cardModel in list)
					{
						CardCmd.Upgrade(cardModel, CardPreviewStyle.MessyLayout);
						await Cmd.CustomScaledWait(0.1f, 0.2f, false, default(CancellationToken));
					}
					List<CardModel>.Enumerator enumerator = default(List<CardModel>.Enumerator);
					await Cmd.CustomScaledWait(0.6f, 1.2f, false, default(CancellationToken));
				}
				else if (list.Count != 0)
				{
					CardCmd.Upgrade(base.Rng.NextItem<CardModel>(list), CardPreviewStyle.EventLayout);
				}
			}
			else
			{
				await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.Owner.Creature.MaxHp - 1, false);
				await CreatureCmd.Kill(base.Owner.Creature, false);
			}
		}

		// Token: 0x040027C4 RID: 10180
		private const string _smashHpGainKey = "SmashHPGain";

		// Token: 0x040027C5 RID: 10181
		private const string _decipherHpLossKey = "DecipherMaxHpLoss";

		// Token: 0x040027C6 RID: 10182
		private int _decipherCount;
	}
}
