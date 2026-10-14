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
	// Token: 0x02000919 RID: 2329
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BiasedCognition : CardModel
	{
		// Token: 0x06006E30 RID: 28208 RVA: 0x00284DE6 File Offset: 0x00282FE6
		public BiasedCognition()
			: base(1, CardType.Power, CardRarity.Ancient, TargetType.Self, true)
		{
		}

		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x06006E31 RID: 28209 RVA: 0x00284DF4 File Offset: 0x00282FF4
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>(null));
			}
		}

		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x06006E32 RID: 28210 RVA: 0x00284E14 File Offset: 0x00283014
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new PowerVar<FocusPower>(4m),
					new PowerVar<BiasedCognitionPower>(1m)
				});
			}
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x00284E3C File Offset: 0x0028303C
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<FocusPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FocusPower"].BaseValue, base.Owner.Creature, this, false);
			await PowerCmd.Apply<BiasedCognitionPower>(choiceContext, base.Owner.Creature, base.DynamicVars["BiasedCognitionPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x00284E87 File Offset: 0x00283087
		protected override void OnUpgrade()
		{
			base.DynamicVars["FocusPower"].UpgradeValueBy(1m);
		}
	}
}
