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
	// Token: 0x02000A70 RID: 2672
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PhantomBlades : CardModel
	{
		// Token: 0x06007563 RID: 30051 RVA: 0x002937A6 File Offset: 0x002919A6
		public PhantomBlades()
			: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x17001F8D RID: 8077
		// (get) Token: 0x06007564 RID: 30052 RVA: 0x002937B3 File Offset: 0x002919B3
		protected override HashSet<CardTag> CanonicalTags
		{
			get
			{
				return new HashSet<CardTag>();
			}
		}

		// Token: 0x17001F8E RID: 8078
		// (get) Token: 0x06007565 RID: 30053 RVA: 0x002937BA File Offset: 0x002919BA
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				return new <>z__ReadOnlyArray<IHoverTip>(new IHoverTip[]
				{
					HoverTipFactory.FromCard<Shiv>(false),
					HoverTipFactory.FromKeyword(CardKeyword.Retain)
				});
			}
		}

		// Token: 0x17001F8F RID: 8079
		// (get) Token: 0x06007566 RID: 30054 RVA: 0x002937D9 File Offset: 0x002919D9
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PhantomBladesPower>(9m));
			}
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x002937EC File Offset: 0x002919EC
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.PowerUpAnimDelay);
			await PowerCmd.Apply<PhantomBladesPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PhantomBladesPower"].BaseValue, base.Owner.Creature, this, false);
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x00293837 File Offset: 0x00291A37
		protected override void OnUpgrade()
		{
			base.DynamicVars["PhantomBladesPower"].UpgradeValueBy(3m);
		}
	}
}
