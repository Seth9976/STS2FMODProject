using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B44 RID: 2884
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockAttackCard : MockCardModel
	{
		// Token: 0x17002160 RID: 8544
		// (get) Token: 0x060079D9 RID: 31193 RVA: 0x0029C911 File Offset: 0x0029AB11
		public override CardType Type
		{
			get
			{
				return CardType.Attack;
			}
		}

		// Token: 0x17002161 RID: 8545
		// (get) Token: 0x060079DA RID: 31194 RVA: 0x0029C914 File Offset: 0x0029AB14
		public override TargetType TargetType
		{
			get
			{
				return this._targetingType;
			}
		}

		// Token: 0x17002162 RID: 8546
		// (get) Token: 0x060079DB RID: 31195 RVA: 0x0029C91C File Offset: 0x0029AB1C
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new DamageVar(6m, ValueProp.Move),
					new OstyDamageVar(6m, ValueProp.Move),
					new BlockVar(0m, ValueProp.Move)
				});
			}
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x0029C955 File Offset: 0x0029AB55
		[PreserveBaseOverrides]
		public new MockAttackCard MockBlock(int block)
		{
			base.AssertMutable();
			base.DynamicVars.Block.BaseValue = block;
			return this;
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x0029C974 File Offset: 0x0029AB74
		public MockAttackCard MockDamage(decimal damage)
		{
			base.AssertMutable();
			base.DynamicVars.Damage.BaseValue = damage;
			return this;
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x0029C98E File Offset: 0x0029AB8E
		public MockAttackCard MockOstyDamage(decimal damage)
		{
			base.AssertMutable();
			base.DynamicVars.OstyDamage.BaseValue = damage;
			return this;
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x0029C9A8 File Offset: 0x0029ABA8
		public MockAttackCard MockHitCount(int hitCount)
		{
			base.AssertMutable();
			this._hitCount = hitCount;
			return this;
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x0029C9B8 File Offset: 0x0029ABB8
		public MockAttackCard MockFromOsty()
		{
			base.AssertMutable();
			this._fromOsty = true;
			base.MockTag(CardTag.OstyAttack);
			return this;
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x0029C9D0 File Offset: 0x0029ABD0
		public MockAttackCard MockTargetingType(TargetType targetingType)
		{
			base.AssertMutable();
			this._targetingType = targetingType;
			return this;
		}

		// Token: 0x060079E2 RID: 31202 RVA: 0x0029C9E0 File Offset: 0x0029ABE0
		public MockAttackCard MockUnpoweredDamage()
		{
			base.AssertMutable();
			base.DynamicVars.Damage.Props = ValueProp.Unpowered;
			base.DynamicVars.OstyDamage.Props = ValueProp.Unpowered;
			return this;
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x0029CA0B File Offset: 0x0029AC0B
		protected override int GetBaseBlock()
		{
			return base.DynamicVars.Block.IntValue;
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x0029CA20 File Offset: 0x0029AC20
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			if (this._mockSelfHpLoss > 0)
			{
				await CreatureCmd.Damage(choiceContext, base.Owner.Creature, this._mockSelfHpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
			}
			if (base.DynamicVars.Block.BaseValue > 0m)
			{
				await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			}
			int num;
			if (base.EnergyCost.CostsX)
			{
				num = base.ResolveEnergyXValue();
			}
			else if (this.HasStarCostX)
			{
				num = base.ResolveStarXValue();
			}
			else
			{
				num = this._hitCount;
			}
			AttackCommand attackCommand = DamageCmd.Attack(this._fromOsty ? base.DynamicVars.OstyDamage.BaseValue : base.DynamicVars.Damage.BaseValue).WithHitCount(num);
			if (this._fromOsty)
			{
				if (base.Owner.Osty == null)
				{
					throw new InvalidOperationException("Must summon Osty before using osty attack!");
				}
				attackCommand = attackCommand.FromOsty(base.Owner.Osty, this, cardPlay);
			}
			else
			{
				attackCommand = attackCommand.FromCard(this, cardPlay);
			}
			switch (this._targetingType)
			{
			case TargetType.AnyEnemy:
				ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
				attackCommand = attackCommand.Targeting(cardPlay.Target);
				break;
			case TargetType.AllEnemies:
				attackCommand = attackCommand.TargetingAllOpponents(base.CombatState);
				break;
			case TargetType.RandomEnemy:
				attackCommand = attackCommand.TargetingRandomOpponents(base.CombatState, true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (base.DynamicVars.Damage.Props.HasFlag(ValueProp.Unpowered))
			{
				attackCommand = attackCommand.Unpowered();
			}
			await attackCommand.Execute(choiceContext);
			if (this._mockExtraLogic != null)
			{
				await this._mockExtraLogic(this);
			}
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x0029CA73 File Offset: 0x0029AC73
		protected override void OnUpgrade()
		{
			if (this._mockUpgradeLogic != null)
			{
				this._mockUpgradeLogic(this);
				return;
			}
			base.DynamicVars.Damage.UpgradeValueBy(3m);
		}

		// Token: 0x040028DC RID: 10460
		private int _hitCount = 1;

		// Token: 0x040028DD RID: 10461
		private bool _fromOsty;

		// Token: 0x040028DE RID: 10462
		private TargetType _targetingType = TargetType.AnyEnemy;
	}
}
