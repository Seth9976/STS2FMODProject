using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Relics
{
	// Token: 0x020005A8 RID: 1448
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaelsLegion : RelicModel
	{
		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06005348 RID: 21320 RVA: 0x0024474F File Offset: 0x0024294F
		public override bool AddsPet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06005349 RID: 21321 RVA: 0x00244752 File Offset: 0x00242952
		public override RelicRarity Rarity
		{
			get
			{
				return RelicRarity.Ancient;
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x0600534A RID: 21322 RVA: 0x00244755 File Offset: 0x00242955
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x0600534B RID: 21323 RVA: 0x00244767 File Offset: 0x00242967
		public override bool ShowCounter
		{
			get
			{
				return this.DisplayAmount > 0;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x0600534C RID: 21324 RVA: 0x00244772 File Offset: 0x00242972
		public static string[] SkinOptions
		{
			get
			{
				return new string[] { "eyes", "horns", "spikes", "wings" };
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x0600534D RID: 21325 RVA: 0x0024479A File Offset: 0x0024299A
		// (set) Token: 0x0600534E RID: 21326 RVA: 0x002447A2 File Offset: 0x002429A2
		[SavedProperty]
		public string Skin
		{
			get
			{
				return this._skin;
			}
			set
			{
				base.AssertMutable();
				this._skin = value;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x0600534F RID: 21327 RVA: 0x002447B1 File Offset: 0x002429B1
		public override int DisplayAmount
		{
			get
			{
				if (!CombatManager.Instance.IsInProgress)
				{
					return -1;
				}
				if (base.IsCanonical)
				{
					return -1;
				}
				if (this._cooldown <= 0)
				{
					return -1;
				}
				return this._cooldown;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06005350 RID: 21328 RVA: 0x002447DC File Offset: 0x002429DC
		// (set) Token: 0x06005351 RID: 21329 RVA: 0x002447E4 File Offset: 0x002429E4
		private int Cooldown
		{
			get
			{
				return this._cooldown;
			}
			set
			{
				base.AssertMutable();
				this._cooldown = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x002447F9 File Offset: 0x002429F9
		// (set) Token: 0x06005353 RID: 21331 RVA: 0x00244801 File Offset: 0x00242A01
		private bool TriggeredBlockLastTurn
		{
			get
			{
				return this._triggeredBlockLastTurn;
			}
			set
			{
				base.AssertMutable();
				this._triggeredBlockLastTurn = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06005354 RID: 21332 RVA: 0x00244816 File Offset: 0x00242A16
		// (set) Token: 0x06005355 RID: 21333 RVA: 0x0024481E File Offset: 0x00242A1E
		[Nullable(2)]
		private CardPlay AffectedCardPlay
		{
			[NullableContext(2)]
			get
			{
				return this._affectedCardPlay;
			}
			[NullableContext(2)]
			set
			{
				base.AssertMutable();
				this._affectedCardPlay = value;
				base.InvokeDisplayAmountChanged();
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06005356 RID: 21334 RVA: 0x00244833 File Offset: 0x00242A33
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Turns", 2m));
			}
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0024484C File Offset: 0x00242A4C
		public override async Task AfterObtained()
		{
			this.Skin = new Rng(base.Owner, base.Id, 0UL).NextItem<string>(PaelsLegion.SkinOptions);
			if (CombatManager.Instance.IsInProgress)
			{
				await this.SummonPet();
			}
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x00244890 File Offset: 0x00242A90
		public override async Task BeforeCombatStart()
		{
			await this.SummonPet();
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x002448D4 File Offset: 0x00242AD4
		[NullableContext(2)]
		public override decimal ModifyBlockMultiplicative([Nullable(1)] Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
		{
			if (!props.IsCardOrMonsterMove())
			{
				return 1m;
			}
			if (cardSource == null)
			{
				return 1m;
			}
			if (cardSource.Owner != base.Owner)
			{
				return 1m;
			}
			if (this.Cooldown > 0)
			{
				return 1m;
			}
			return 2m;
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x00244924 File Offset: 0x00242B24
		[NullableContext(2)]
		[return: Nullable(1)]
		public override Task AfterModifyingBlockAmount(decimal modifiedAmount, CardModel cardSource, CardPlay cardPlay)
		{
			if (modifiedAmount <= 0m)
			{
				return Task.CompletedTask;
			}
			if (cardPlay == null)
			{
				return Task.CompletedTask;
			}
			if (this.AffectedCardPlay != null && this.AffectedCardPlay != cardPlay)
			{
				return Task.CompletedTask;
			}
			this.AffectedCardPlay = cardPlay;
			return Task.CompletedTask;
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x00244970 File Offset: 0x00242B70
		public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this.AffectedCardPlay != null)
			{
				if (this.AffectedCardPlay == cardPlay)
				{
					base.Flash();
					this.AffectedCardPlay = null;
					this.Cooldown = base.DynamicVars["Turns"].IntValue;
					base.Status = RelicStatus.Normal;
					PaelsLegion paelsLegion = (PaelsLegion)base.Owner.PlayerCombatState.GetPet<PaelsLegion>().Monster;
					await CreatureCmd.TriggerAnim(paelsLegion.Creature, "BlockTrigger", 0.15f);
					this.TriggeredBlockLastTurn = true;
				}
			}
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x002449BC File Offset: 0x00242BBC
		public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
		{
			if (participants.Contains(base.Owner.Creature))
			{
				bool flag = this.Cooldown > 0;
				int cooldown = this.Cooldown;
				this.Cooldown = cooldown - 1;
				if (this.Cooldown <= 0)
				{
					base.Status = RelicStatus.Active;
					base.InvokeDisplayAmountChanged();
				}
				PaelsLegion paelsLegion = (PaelsLegion)base.Owner.PlayerCombatState.GetPet<PaelsLegion>().Monster;
				if (this.Cooldown > 0 && this.TriggeredBlockLastTurn)
				{
					await CreatureCmd.TriggerAnim(paelsLegion.Creature, "SleepTrigger", 0.15f);
				}
				else if (this.Cooldown <= 0 && flag)
				{
					await CreatureCmd.TriggerAnim(paelsLegion.Creature, "WakeUpTrigger", 0.15f);
				}
				this.TriggeredBlockLastTurn = false;
			}
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x00244A07 File Offset: 0x00242C07
		public override Task AfterCombatEnd(CombatRoom _)
		{
			base.Status = RelicStatus.Normal;
			this.Cooldown = 0;
			this.TriggeredBlockLastTurn = false;
			this.AffectedCardPlay = null;
			base.InvokeDisplayAmountChanged();
			return Task.CompletedTask;
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x00244A30 File Offset: 0x00242C30
		private async Task SummonPet()
		{
			await PlayerCmd.AddPet<PaelsLegion>(base.Owner);
		}

		// Token: 0x04002489 RID: 9353
		private const string _turnsKey = "Turns";

		// Token: 0x0400248A RID: 9354
		private string _skin = PaelsLegion.SkinOptions[0];

		// Token: 0x0400248B RID: 9355
		private int _cooldown;

		// Token: 0x0400248C RID: 9356
		private bool _triggeredBlockLastTurn;

		// Token: 0x0400248D RID: 9357
		[Nullable(2)]
		private CardPlay _affectedCardPlay;
	}
}
