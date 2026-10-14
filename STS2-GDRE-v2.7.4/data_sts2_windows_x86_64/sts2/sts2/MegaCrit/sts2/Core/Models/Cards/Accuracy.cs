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
	// Token: 0x020008F3 RID: 2291
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Accuracy : CardModel
	{
		// Token: 0x06006D72 RID: 28018 RVA: 0x002835E3 File Offset: 0x002817E3
		public Accuracy()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x06006D73 RID: 28019 RVA: 0x002835F0 File Offset: 0x002817F0
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<AccuracyPower>(4m));
			}
		}

		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x06006D74 RID: 28020 RVA: 0x00283602 File Offset: 0x00281802
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(false));
			}
		}

		// Token: 0x06006D75 RID: 28021 RVA: 0x00283610 File Offset: 0x00281810
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<AccuracyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["AccuracyPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006D76 RID: 28022 RVA: 0x0028365B File Offset: 0x0028185B
		protected override void OnUpgrade()
		{
			base.DynamicVars["AccuracyPower"].UpgradeValueBy(2m);
		}
	}
}
