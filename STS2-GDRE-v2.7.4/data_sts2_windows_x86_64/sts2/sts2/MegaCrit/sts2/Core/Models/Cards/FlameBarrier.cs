using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x020009C7 RID: 2503
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FlameBarrier : CardModel
	{
		// Token: 0x060071CB RID: 29131 RVA: 0x0028C0A8 File Offset: 0x0028A2A8
		public FlameBarrier()
			: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001E0D RID: 7693
		// (get) Token: 0x060071CC RID: 29132 RVA: 0x0028C0B5 File Offset: 0x0028A2B5
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E0E RID: 7694
		// (get) Token: 0x060071CD RID: 29133 RVA: 0x0028C0B8 File Offset: 0x0028A2B8
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(12m, ValueProp.Move),
					new DynamicVar("DamageBack", 4m)
				});
			}
		}

		// Token: 0x060071CE RID: 29134 RVA: 0x0028C0E8 File Offset: 0x0028A2E8
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			NFireBurningVfx nfireBurningVfx = NFireBurningVfx.Create(base.Owner.Creature, 0.75f, false);
			NCombatRoom instance = NCombatRoom.Instance;
			if (instance != null)
			{
				instance.CombatVfxContainer.AddChildSafely(nfireBurningVfx);
			}
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<FlameBarrierPower>(choiceContext, base.Owner.Creature, base.DynamicVars["DamageBack"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060071CF RID: 29135 RVA: 0x0028C13B File Offset: 0x0028A33B
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(4m);
			base.DynamicVars["DamageBack"].UpgradeValueBy(2m);
		}

		// Token: 0x04002870 RID: 10352
		private const string _damageBackKey = "DamageBack";
	}
}
