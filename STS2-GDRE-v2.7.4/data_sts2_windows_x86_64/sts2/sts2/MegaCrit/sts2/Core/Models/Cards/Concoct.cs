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
	// Token: 0x0200095C RID: 2396
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Concoct : CardModel
	{
		// Token: 0x06006F86 RID: 28550 RVA: 0x0028798B File Offset: 0x00285B8B
		public Concoct()
			: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly, true)
		{
		}

		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x06006F87 RID: 28551 RVA: 0x00287998 File Offset: 0x00285B98
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x06006F88 RID: 28552 RVA: 0x0028799B File Offset: 0x00285B9B
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ConcoctPower>(3m));
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x06006F89 RID: 28553 RVA: 0x002879B0 File Offset: 0x00285BB0
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>(null));
			}
		}

		// Token: 0x06006F8A RID: 28554 RVA: 0x002879D0 File Offset: 0x00285BD0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<ConcoctPower>(choiceContext, cardPlay.Target, base.DynamicVars["ConcoctPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x00287A23 File Offset: 0x00285C23
		protected override void OnUpgrade()
		{
			base.DynamicVars["ConcoctPower"].UpgradeValueBy(1m);
		}
	}
}
