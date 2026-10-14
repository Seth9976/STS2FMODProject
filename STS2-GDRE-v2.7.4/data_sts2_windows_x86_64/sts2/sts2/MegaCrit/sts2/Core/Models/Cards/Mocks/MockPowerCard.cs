using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards.Mocks
{
	// Token: 0x02000B47 RID: 2887
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MockPowerCard : MockCardModel
	{
		// Token: 0x17002173 RID: 8563
		// (get) Token: 0x06007A0C RID: 31244 RVA: 0x0029CCD6 File Offset: 0x0029AED6
		protected override int CanonicalEnergyCost
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17002174 RID: 8564
		// (get) Token: 0x06007A0D RID: 31245 RVA: 0x0029CCD9 File Offset: 0x0029AED9
		public override CardType Type
		{
			get
			{
				return CardType.Power;
			}
		}

		// Token: 0x17002175 RID: 8565
		// (get) Token: 0x06007A0E RID: 31246 RVA: 0x0029CCDC File Offset: 0x0029AEDC
		public override TargetType TargetType
		{
			get
			{
				return TargetType.Self;
			}
		}

		// Token: 0x17002176 RID: 8566
		// (get) Token: 0x06007A0F RID: 31247 RVA: 0x0029CCDF File Offset: 0x0029AEDF
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<StrengthPower>(2m),
					new BlockVar(0m, ValueProp.Move)
				});
			}
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x0029CD08 File Offset: 0x0029AF08
		public override MockCardModel MockBlock(int block)
		{
			base.AssertMutable();
			base.DynamicVars.Block.BaseValue = block;
			return this;
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x0029CD27 File Offset: 0x0029AF27
		protected override int GetBaseBlock()
		{
			return base.DynamicVars.Block.IntValue;
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x0029CD3C File Offset: 0x0029AF3C
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
			decimal baseValue = base.DynamicVars.Strength.BaseValue;
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, baseValue, base.Owner.Creature, this, false);
			if (this._mockExtraLogic != null)
			{
				await this._mockExtraLogic(this);
			}
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x0029CD8F File Offset: 0x0029AF8F
		protected override void OnUpgrade()
		{
			if (this._mockUpgradeLogic != null)
			{
				this._mockUpgradeLogic(this);
				return;
			}
			base.DynamicVars.Strength.UpgradeValueBy(1m);
		}
	}
}
