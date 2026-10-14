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
	// Token: 0x0200098B RID: 2443
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Demesne : CardModel
	{
		// Token: 0x0600707F RID: 28799 RVA: 0x002897D0 File Offset: 0x002879D0
		public Demesne()
			: base(3, CardType.Power, CardRarity.Rare, TargetType.Self, true)
		{
		}

		// Token: 0x17001D76 RID: 7542
		// (get) Token: 0x06007080 RID: 28800 RVA: 0x002897DD File Offset: 0x002879DD
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new EnergyVar(1),
					new CardsVar(1)
				});
			}
		}

		// Token: 0x17001D77 RID: 7543
		// (get) Token: 0x06007081 RID: 28801 RVA: 0x002897FC File Offset: 0x002879FC
		public override IEnumerable<CardKeyword> CanonicalKeywords
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
			}
		}

		// Token: 0x17001D78 RID: 7544
		// (get) Token: 0x06007082 RID: 28802 RVA: 0x00289804 File Offset: 0x00287A04
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);
			}
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x00289814 File Offset: 0x00287A14
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<DemesnePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007084 RID: 28804 RVA: 0x0028985F File Offset: 0x00287A5F
		protected override void OnUpgrade()
		{
			base.EnergyCost.UpgradeBy(-1);
		}
	}
}
