using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace MegaCrit.Sts2.Core.Models.Cards
{
	// Token: 0x02000AF8 RID: 2808
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SummonForth : CardModel
	{
		// Token: 0x06007840 RID: 30784 RVA: 0x002996D4 File Offset: 0x002978D4
		public SummonForth()
			: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
		{
		}

		// Token: 0x170020B6 RID: 8374
		// (get) Token: 0x06007841 RID: 30785 RVA: 0x002996E1 File Offset: 0x002978E1
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(8));
			}
		}

		// Token: 0x170020B7 RID: 8375
		// (get) Token: 0x06007842 RID: 30786 RVA: 0x002996EE File Offset: 0x002978EE
		protected override IEnumerable<IHoverTip> ExtraHoverTips
		{
			get
			{
				List<IHoverTip> list = new List<IHoverTip>();
				list.AddRange(HoverTipFactory.FromForge());
				list.Add(HoverTipFactory.FromKeyword(CardKeyword.Retain));
				return new <>z__ReadOnlyList<IHoverTip>(list);
			}
		}

		// Token: 0x06007843 RID: 30787 RVA: 0x00299714 File Offset: 0x00297914
		protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
		{
			await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			IEnumerable<SovereignBlade> enumerable = base.Owner.PlayerCombatState.AllCards.OfType<SovereignBlade>().Where<SovereignBlade>(delegate(SovereignBlade c)
			{
				CardPile pile = c.Pile;
				return pile == null || pile.Type != PileType.Hand;
			});
			await CardPileCmd.Add(enumerable, PileType.Hand, CardPilePosition.Bottom, null, false);
			await ForgeCmd.Forge(base.DynamicVars.Forge.IntValue, base.Owner, this);
		}

		// Token: 0x06007844 RID: 30788 RVA: 0x00299757 File Offset: 0x00297957
		protected override void OnUpgrade()
		{
			base.DynamicVars.Forge.UpgradeValueBy(3m);
		}
	}
}
