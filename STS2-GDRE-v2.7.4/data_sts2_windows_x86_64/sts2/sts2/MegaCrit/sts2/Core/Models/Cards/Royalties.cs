using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AA8 RID: 2728
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Royalties : CardModel
	{
		// Token: 0x0600768A RID: 30346 RVA: 0x00295EB4 File Offset: 0x002940B4
		public Royalties()
			: base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002005 RID: 8197
		// (get) Token: 0x0600768B RID: 30347 RVA: 0x00295EC1 File Offset: 0x002940C1
		public override bool CanBeGeneratedInCombat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002006 RID: 8198
		// (get) Token: 0x0600768C RID: 30348 RVA: 0x00295EC4 File Offset: 0x002940C4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new GoldVar(30));
			}
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x00295ED4 File Offset: 0x002940D4
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<RoyaltiesPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Gold.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x0600768E RID: 30350 RVA: 0x00295F1F File Offset: 0x0029411F
		protected override void OnUpgrade()
		{
			base.DynamicVars.Gold.UpgradeValueBy(10m);
		}
	}
}
