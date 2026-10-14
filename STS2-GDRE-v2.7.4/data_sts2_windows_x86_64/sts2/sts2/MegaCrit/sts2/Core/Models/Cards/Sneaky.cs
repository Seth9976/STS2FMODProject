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
	// Token: 0x02000AD3 RID: 2771
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Sneaky : CardModel
	{
		// Token: 0x06007771 RID: 30577 RVA: 0x00297A93 File Offset: 0x00295C93
		public Sneaky()
			: base(2, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17002069 RID: 8297
		// (get) Token: 0x06007772 RID: 30578 RVA: 0x00297AA0 File Offset: 0x00295CA0
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Sly);
			}
		}

		// Token: 0x1700206A RID: 8298
		// (get) Token: 0x06007773 RID: 30579 RVA: 0x00297AA8 File Offset: 0x00295CA8
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block, Array.Empty<DynamicVar>()));
			}
		}

		// Token: 0x1700206B RID: 8299
		// (get) Token: 0x06007774 RID: 30580 RVA: 0x00297ABA File Offset: 0x00295CBA
		public override CardMultiplayerConstraint MultiplayerConstraint
		{
			get
			{
				return CardMultiplayerConstraint.MultiplayerOnly;
			}
		}

		// Token: 0x1700206C RID: 8300
		// (get) Token: 0x06007775 RID: 30581 RVA: 0x00297ABD File Offset: 0x00295CBD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SneakyPower>(1m));
			}
		}

		// Token: 0x06007776 RID: 30582 RVA: 0x00297AD0 File Offset: 0x00295CD0
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<SneakyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SneakyPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007777 RID: 30583 RVA: 0x00297B1B File Offset: 0x00295D1B
		protected override void OnUpgrade()
		{
			base.DynamicVars["SneakyPower"].UpgradeValueBy(1m);
		}
	}
}
