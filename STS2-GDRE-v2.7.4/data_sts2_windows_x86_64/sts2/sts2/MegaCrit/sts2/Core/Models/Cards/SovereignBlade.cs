using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AD9 RID: 2777
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SovereignBlade : CardModel
	{
		// Token: 0x06007792 RID: 30610 RVA: 0x00297E8B File Offset: 0x0029608B
		public SovereignBlade()
			: base(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy, true)
		{
		}

		// Token: 0x17002078 RID: 8312
		// (get) Token: 0x06007793 RID: 30611 RVA: 0x00297EB0 File Offset: 0x002960B0
		protected override IEnumerable<string> ExtraRunAssetPaths
		{
			get
			{
				return NSovereignBladeVfx.AssetPaths;
			}
		}

		/// <remarks>
		/// Dynamic TargetType based on whether the player has <see cref="T:MegaCrit.Sts2.Core.Models.Powers.SeekingEdgePower" />.
		/// </remarks>
		// Token: 0x17002079 RID: 8313
		// (get) Token: 0x06007794 RID: 30612 RVA: 0x00297EB7 File Offset: 0x002960B7
		public override TargetType TargetType
		{
			get
			{
				if (!this.HasSeekingEdge)
				{
					return TargetType.AnyEnemy;
				}
				return TargetType.AllEnemies;
			}
		}

		/// <remarks>
		/// Dynamic GainsBlock based on whether the player has <see cref="T:MegaCrit.Sts2.Core.Models.Powers.ParryPower" />.
		/// </remarks>
		// Token: 0x1700207A RID: 8314
		// (get) Token: 0x06007795 RID: 30613 RVA: 0x00297EC4 File Offset: 0x002960C4
		public override bool GainsBlock
		{
			get
			{
				return SovereignBlade.GetOwnerParryAmount(this) > 0m;
			}
		}

		// Token: 0x1700207B RID: 8315
		// (get) Token: 0x06007796 RID: 30614 RVA: 0x00297ED6 File Offset: 0x002960D6
		// (set) Token: 0x06007797 RID: 30615 RVA: 0x00297EDE File Offset: 0x002960DE
		private decimal CurrentDamage
		{
			get
			{
				return this._currentDamage;
			}
			set
			{
				base.AssertMutable();
				this._currentDamage = value;
			}
		}

		// Token: 0x1700207C RID: 8316
		// (get) Token: 0x06007798 RID: 30616 RVA: 0x00297EED File Offset: 0x002960ED
		// (set) Token: 0x06007799 RID: 30617 RVA: 0x00297EF5 File Offset: 0x002960F5
		private decimal CurrentRepeats
		{
			get
			{
				return this._currentRepeats;
			}
			set
			{
				base.AssertMutable();
				this._currentRepeats = value;
			}
		}

		// Token: 0x1700207D RID: 8317
		// (get) Token: 0x0600779A RID: 30618 RVA: 0x00297F04 File Offset: 0x00296104
		// (set) Token: 0x0600779B RID: 30619 RVA: 0x00297F0C File Offset: 0x0029610C
		public bool CreatedThroughForge
		{
			get
			{
				return this._createdThroughForge;
			}
			set
			{
				base.AssertMutable();
				this._createdThroughForge = value;
			}
		}

		// Token: 0x1700207E RID: 8318
		// (get) Token: 0x0600779C RID: 30620 RVA: 0x00297F1B File Offset: 0x0029611B
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
			}
		}

		// Token: 0x1700207F RID: 8319
		// (get) Token: 0x0600779D RID: 30621 RVA: 0x00297F24 File Offset: 0x00296124
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				DynamicVar[] array = new DynamicVar[5];
				array[0] = new DamageVar(10m, ValueProp.Move);
				array[1] = new CalculationBaseVar(0m);
				array[2] = new CalculationExtraVar(1m);
				array[3] = new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, [Nullable(2)] Creature _) => SovereignBlade.GetOwnerParryAmount(card));
				array[4] = new RepeatVar(1);
				return new <>z__ReadOnlyArray<DynamicVar>(array);
			}
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x00297F9C File Offset: 0x0029619C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			string text = ((base.Owner.Character is Regent) ? "sovereignBladeTrigger" : "Cast");
			float num = ((base.Owner.Character is Regent) ? 0.25f : base.Owner.Character.CastAnimDelay);
			AttackCommand attackCommand = DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).WithHitCount(base.DynamicVars.Repeat.IntValue)
				.WithAttackerAnim(text, num, null)
				.WithAttackerFx(null, "event:/sfx/characters/regent/regent_sovereign_blade", null);
			if (this.HasSeekingEdge)
			{
				attackCommand = attackCommand.TargetingAllOpponents(base.CombatState).BeforeDamage(delegate
				{
					IReadOnlyList<Creature> hittableEnemies = this.CombatState.HittableEnemies;
					if (hittableEnemies.Count <= 0)
					{
						return Task.CompletedTask;
					}
					NSovereignBladeVfx vfxNode = SovereignBlade.GetVfxNode(this.Owner, this);
					NCombatRoom instance = NCombatRoom.Instance;
					NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(hittableEnemies[0]) : null);
					if (vfxNode != null && ncreature != null)
					{
						vfxNode.Attack(ncreature.VfxSpawnPosition);
					}
					return Task.CompletedTask;
				}).WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3");
			}
			else
			{
				ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
				AttackCommand attackCommand2 = attackCommand.Targeting(cardPlay.Target).BeforeDamage(delegate
				{
					NSovereignBladeVfx vfxNode2 = SovereignBlade.GetVfxNode(this.Owner, this);
					NCombatRoom instance2 = NCombatRoom.Instance;
					NCreature ncreature2 = ((instance2 != null) ? instance2.GetCreatureNode(cardPlay.Target) : null);
					if (vfxNode2 != null && ncreature2 != null)
					{
						vfxNode2.Attack(ncreature2.VfxSpawnPosition);
					}
					return Task.CompletedTask;
				});
				Func<Creature, Node2D> func;
				if ((func = SovereignBlade.<>O.<0>__Create) == null)
				{
					func = (SovereignBlade.<>O.<0>__Create = new Func<Creature, Node2D>(NBigSlashVfx.Create));
				}
				AttackCommand attackCommand3 = attackCommand2.WithHitVfxNode(func);
				Func<Creature, Node2D> func2;
				if ((func2 = SovereignBlade.<>O.<1>__Create) == null)
				{
					func2 = (SovereignBlade.<>O.<1>__Create = new Func<Creature, Node2D>(NBigSlashImpactVfx.Create));
				}
				attackCommand = attackCommand3.WithHitVfxNode(func2);
			}
			await attackCommand.Execute(choiceContext);
			if (SovereignBlade.GetOwnerParryAmount(this) > 0m)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), base.DynamicVars.CalculatedBlock.Props, cardPlay, false);
			}
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x00297FEF File Offset: 0x002961EF
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x00297FFD File Offset: 0x002961FD
		protected override void AfterCloned()
		{
			base.AfterCloned();
			this.CreatedThroughForge = false;
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x0029800C File Offset: 0x0029620C
		protected override void AfterDowngraded()
		{
			base.AfterDowngraded();
			base.DynamicVars.Damage.BaseValue = this.CurrentDamage;
			base.DynamicVars.Repeat.BaseValue = this.CurrentRepeats;
		}

		// Token: 0x060077A2 RID: 30626 RVA: 0x00298040 File Offset: 0x00296240
		public override void AfterTransformedFrom()
		{
			this.RemoveSovereignBladeNode();
		}

		// Token: 0x060077A3 RID: 30627 RVA: 0x00298048 File Offset: 0x00296248
		public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, [Nullable(2)] AbstractModel clonedBy)
		{
			if (card != this)
			{
				return Task.CompletedTask;
			}
			if ((!this.CreatedThroughForge && oldPileType == PileType.None) || oldPileType == PileType.Exhaust)
			{
				ForgeCmd.PlayCombatRoomForgeVfx(base.Owner, this);
			}
			if (card.Pile.Type == PileType.Exhaust)
			{
				this.RemoveSovereignBladeNode();
			}
			return Task.CompletedTask;
		}

		// Token: 0x060077A4 RID: 30628 RVA: 0x00298088 File Offset: 0x00296288
		public void AddDamage(decimal amount)
		{
			base.DynamicVars.Damage.BaseValue += amount;
			this.CurrentDamage = base.DynamicVars.Damage.BaseValue;
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x002980BC File Offset: 0x002962BC
		public void SetRepeats(decimal amount)
		{
			base.DynamicVars.Repeat.BaseValue = amount;
			this.CurrentRepeats = base.DynamicVars.Repeat.BaseValue;
		}

		// Token: 0x060077A6 RID: 30630 RVA: 0x002980E8 File Offset: 0x002962E8
		[return: Nullable(2)]
		public static NSovereignBladeVfx GetVfxNode(Player player, CardModel card)
		{
			CardModel originalCard = card.DupeOf ?? card;
			NCombatRoom instance = NCombatRoom.Instance;
			NCreature ncreature = ((instance != null) ? instance.GetCreatureNode(player.Creature) : null);
			if (ncreature == null)
			{
				return null;
			}
			return ncreature.GetChildren(false).OfType<NSovereignBladeVfx>().FirstOrDefault<NSovereignBladeVfx>((NSovereignBladeVfx b) => b.Card == originalCard);
		}

		// Token: 0x060077A7 RID: 30631 RVA: 0x00298146 File Offset: 0x00296346
		private void RemoveSovereignBladeNode()
		{
			NSovereignBladeVfx vfxNode = SovereignBlade.GetVfxNode(base.Owner, this);
			if (vfxNode == null)
			{
				return;
			}
			vfxNode.RemoveSovereignBlade();
		}

		// Token: 0x17002080 RID: 8320
		// (get) Token: 0x060077A8 RID: 30632 RVA: 0x0029815E File Offset: 0x0029635E
		private bool HasSeekingEdge
		{
			get
			{
				return base.IsMutable && base.Owner != null && base.Owner.Creature.HasPower<SeekingEdgePower>();
			}
		}

		// Token: 0x060077A9 RID: 30633 RVA: 0x00298184 File Offset: 0x00296384
		private static decimal GetOwnerParryAmount(CardModel card)
		{
			if (!card.IsMutable)
			{
				return 0m;
			}
			if (card.Owner == null)
			{
				return 0m;
			}
			if (card.Pile == null)
			{
				return 0m;
			}
			if (!card.Pile.IsCombatPile)
			{
				return 0m;
			}
			return card.Owner.Creature.GetPowerAmount<ParryPower>();
		}

		// Token: 0x040028BF RID: 10431
		private const int _baseDamage = 10;

		// Token: 0x040028C0 RID: 10432
		private const string _sovereignBladeSfx = "event:/sfx/characters/regent/regent_sovereign_blade";

		// Token: 0x040028C1 RID: 10433
		private decimal _currentDamage = 10m;

		// Token: 0x040028C2 RID: 10434
		private decimal _currentRepeats = 1m;

		// Token: 0x040028C3 RID: 10435
		private bool _createdThroughForge;

		// Token: 0x02002219 RID: 8729
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04008F0F RID: 36623
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <0>__Create;

			// Token: 0x04008F10 RID: 36624
			[Nullable(new byte[] { 0, 1, 2 })]
			public static Func<Creature, Node2D> <1>__Create;
		}
	}
}
