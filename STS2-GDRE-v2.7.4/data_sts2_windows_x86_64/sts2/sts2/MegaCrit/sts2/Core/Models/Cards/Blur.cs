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

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000924 RID: 2340
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Blur : CardModel
	{
		// Token: 0x06006E6B RID: 28267 RVA: 0x002854E4 File Offset: 0x002836E4
		public Blur()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x06006E6C RID: 28268 RVA: 0x002854F1 File Offset: 0x002836F1
		public override bool GainsBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x06006E6D RID: 28269 RVA: 0x002854F4 File Offset: 0x002836F4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new BlockVar(5m, ValueProp.Move),
					new DynamicVar("Blur", 1m)
				});
			}
		}

		// Token: 0x06006E6E RID: 28270 RVA: 0x00285524 File Offset: 0x00283724
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay, false);
			await PowerCmd.Apply<BlurPower>(choiceContext, base.Owner.Creature, base.DynamicVars["Blur"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E6F RID: 28271 RVA: 0x00285577 File Offset: 0x00283777
		protected override void OnUpgrade()
		{
			base.DynamicVars.Block.UpgradeValueBy(3m);
		}

		// Token: 0x0400284B RID: 10315
		private const string _blurKey = "Blur";
	}
}
