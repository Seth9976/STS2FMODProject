using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009E3 RID: 2531
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Glitterstream : CardModel
	{
		// Token: 0x0600726D RID: 29293 RVA: 0x0028D4AA File Offset: 0x0028B6AA
		public Glitterstream()
			: base(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
		{
		}

		// Token: 0x17001E51 RID: 7761
		// (get) Token: 0x0600726E RID: 29294 RVA: 0x0028D4B7 File Offset: 0x0028B6B7
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E52 RID: 7762
		// (get) Token: 0x0600726F RID: 29295 RVA: 0x0028D4BA File Offset: 0x0028B6BA
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(11m, ValueProp.Move),
					new BlockVar("BlockNextTurn", 5m, ValueProp.Move)
				});
			}
		}

		// Token: 0x06007270 RID: 29296 RVA: 0x0028D4EC File Offset: 0x0028B6EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			BlockVar blockVar = (BlockVar)base.DynamicVars["BlockNextTurn"];
			IEnumerable<AbstractModel> enumerable;
			decimal blockNextTurnAmount = Hook.ModifyBlock(base.CombatState, base.Owner.Creature, blockVar.BaseValue, blockVar.Props, this, cardPlay, out enumerable);
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, base.Owner.Creature, blockNextTurnAmount, base.Owner.Creature, this, false);
		}

		// Token: 0x06007271 RID: 29297 RVA: 0x0028D53F File Offset: 0x0028B73F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(2m);
			base.DynamicVars["BlockNextTurn"].UpgradeValueBy(2m);
		}

		// Token: 0x04002878 RID: 10360
		private const string _blockNextTurnKey = "BlockNextTurn";
	}
}
