using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000A59 RID: 2649
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NoxiousFumes : CardModel
	{
		// Token: 0x060074EE RID: 29934 RVA: 0x00292897 File Offset: 0x00290A97
		public NoxiousFumes()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F5E RID: 8030
		// (get) Token: 0x060074EF RID: 29935 RVA: 0x002928A4 File Offset: 0x00290AA4
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("PoisonPerTurn", 2m));
			}
		}

		// Token: 0x17001F5F RID: 8031
		// (get) Token: 0x060074F0 RID: 29936 RVA: 0x002928BC File Offset: 0x00290ABC
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x002928DC File Offset: 0x00290ADC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<NoxiousFumesPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PoisonPerTurn"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x060074F2 RID: 29938 RVA: 0x00292927 File Offset: 0x00290B27
		protected override void OnUpgrade()
		{
			base.DynamicVars["PoisonPerTurn"].UpgradeValueBy(1m);
		}

		// Token: 0x040028AA RID: 10410
		private const string _poisonPerTurnKey = "PoisonPerTurn";
	}
}
